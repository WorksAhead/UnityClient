using System;
using StoryDlg;
using ArkCrossEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StoryDlgPanel : UnityEngine.MonoBehaviour
{
    public enum StoryDlgType
    {
        Small,
        Big,
    }
    /* ScriptForStory 用于区分该脚本挂在StoryDlgBig上还是StoryDlgSmall，需要编辑器中修改*/
    public StoryDlgType ScriptForStory = StoryDlgType.Big;
    public float ShowWordDuration = 0.1f;
    public UnityEngine.GameObject EffectForSpeaker;
    public UILabel lblName;
    public UILabel lblWords;
    public UILabel lblSpeakerNameLeft01;
    public UILabel lblSpeakerNameLeft02;
    public UILabel lblWordsLeft;
    public UILabel lblSpeakerNameRight01;
    public UILabel lblSpeakerNameRight02;
    public UILabel lblWordsRight;
    public UILabel lblSpeakerMonsterName;
    public UILabel lblSpeakerMonsterWords;
    public UISprite spriteLeft;
    public UISprite spriteRight;
    public UITexture[] texAnimationArr = new UITexture[2];
    private UnityEngine.GameObject m_RunTimeEffect;
    private UnityEngine.Transform m_EfHeadTrans = null;
    private UnityEngine.GameObject m_StoryDlgGO = null;
    private StoryDlgInfo m_StoryInfo = null;
    private int m_Count = 0;
    private int m_StepNumber = 0;
    private bool m_IsStoryDlgActive = false;
    private int m_StoryId = 0;
    private string m_DescriptionWords;
    private StoryDlgType m_StoryDlgType = StoryDlgType.Big;
    private StoryDlgItem m_StoryDlgItem = new StoryDlgItem();
    private UITexture m_TexAnimation;
    private List<object> m_EventList = new List<object>();
    public void UnSubscribe()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                if (m_EventList[i] != null)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
                }
            }
            /*
	      foreach (object eo in m_EventList) {
	        if (eo != null) {
	          ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
	        }
	      }*/
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            if (ScriptForStory == StoryDlgType.Small)
            {
                object obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_actor_id", "ui", this.HandlerPlayParticle);
                if (obj != null) m_EventList.Add(obj);
                obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
                if (obj != null) m_EventList.Add(obj);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnDestroy()
    {
        try
        {
            UnSubscribe();
            if (m_RunTimeEffect != null)
            {
                Destroy(m_RunTimeEffect);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnTriggerStory(StoryDlgInfo storyInfo)
    {
        StoryDlgManager.Instance.FireStoryStartMsg();
        if (m_IsStoryDlgActive)
        {
            //剧情对话中途被打断，中止当前对话
            KillStoryDlg();
        }
        if (m_IsStoryDlgActive == false)
        {
            m_StoryInfo = storyInfo;
            if (m_StoryInfo != null)
            {
                //Debug.LogError("===== Trigger A New Story !!! " + arg.m_IntervalTime);
                m_StoryId = m_StoryInfo.ID;
                m_StoryDlgType = m_StoryInfo.DlgType;
                m_IsStoryDlgActive = true;
                m_StoryDlgGO = this.gameObject;
                m_Count = 0;    //剧情对话计数器，触发一个新的剧情时重置为0         
                m_StepNumber = m_StoryInfo.StoryItems.Count;
                StoryDlgItem item = m_StoryInfo.StoryItems[m_Count];
                UpdateStoryDlg(m_StoryDlgGO.transform, item, m_Count);
                NGUITools.SetActive(m_StoryDlgGO, true);
                ShowWordDuration = item.WordDuration;
                m_Count++;
                float RealIntervalTime = item.IntervalTime;
                if (m_StoryDlgType == StoryDlgType.Big)
                    RealIntervalTime = GetRealIntervalTime(item);
                if (RealIntervalTime > 0.0f)
                {
                    Invoke("NextStoryItem", RealIntervalTime);
                }
            }
        }
    }
    public void OnNextBtnClicked()
    {
        this.NextStoryItem();
    }
    public void OnStopBtnClicked()
    {
        this.StopStoryDlg();
    }

    private void UpdateStoryDlg(UnityEngine.Transform storyTrans, StoryDlgItem item, int count)
    {

        if (m_StoryDlgType == StoryDlgType.Big)
        {
            //大对话
            ExchangeDepth(count);
            if (lblWords != null) lblWords.text = "";
            m_DescriptionWords = item.Words;
            ShowWordDuration = item.WordDuration;
            m_StoryDlgItem = item;
            UITexture texAnimation = texAnimationArr[count % 2];
            m_TexAnimation = texAnimation;
            TweenAlphaAtStart(texAnimation);
            SetTexAnimaitionImage(item.TextureAnimationPath, texAnimation);
            EnlargerTexAnimation(2f, texAnimation);
            UnityEngine.Vector3 fromPos = GetAnimationTexWorldPos(item.FromOffsetLeft, item.FromOffsetBottom, texAnimation);
            UnityEngine.Vector3 toPos = GetAnimationTexWorldPos(item.ToOffsetLeft, item.ToOffsetBottom, texAnimation);
            texAnimation.transform.position = fromPos;
            fromPos = texAnimation.transform.localPosition;
            texAnimation.transform.position = toPos;
            toPos = texAnimation.transform.localPosition;
            TweenTexAnimationPos(fromPos, toPos, item, texAnimation);
            TweenTexAnimationScale(item, texAnimation);
            //TweenTexAnimationAlpha(item, texAnimation);
        }
        else
        {
            //小对话
            PlayParticleByUnitId(item.UnitId);
            if (lblName != null) lblName.text = string.Format("[c9b2ae]{0}:[-]", item.SpeakerName);
            item.Words = item.Words.Replace("[\\n]", "\n");
            //if(lblWords!=null) lblWords.text = item.Words;
            UnityEngine.GameObject goAtlas = ArkCrossEngine.ResourceSystem.GetSharedResource(item.ImageLeftAtlas) as UnityEngine.GameObject;
            bool isMonsterSpeaker = true;//判断是否为小怪
            if (goAtlas != null && spriteLeft != null)
            {
                NGUITools.SetActive(spriteLeft.gameObject, true);
                UIAtlas atlas = goAtlas.GetComponent<UIAtlas>();
                if (atlas != null)
                {
                    spriteLeft.atlas = atlas;
                    spriteLeft.spriteName = item.ImageLeft;
                }
                if (lblSpeakerNameLeft01 != null) lblSpeakerNameLeft01.text = item.SpeakerName;
                if (lblSpeakerNameLeft02 != null) lblSpeakerNameLeft02.text = item.SpeakerName;
                if (lblWordsLeft != null) lblWordsLeft.text = item.Words;
                isMonsterSpeaker = false;
            }
            else
            {
                if (spriteLeft != null)
                    NGUITools.SetActive(spriteLeft.gameObject, false);
#if DEBUG
                Debug.Log("!!!ImageLeftAtlas or spriteLeft is null.");
#endif
            }
            try
            {
                goAtlas = ArkCrossEngine.ResourceSystem.GetSharedResource(item.ImageRightAtlas) as UnityEngine.GameObject;
            }
            catch (System.Exception ex)
            {
                goAtlas = null;
            }
            
            if (goAtlas != null && spriteRight != null)
            {
                NGUITools.SetActive(spriteRight.gameObject, true);
                UIAtlas atlas = goAtlas.GetComponent<UIAtlas>();
                if (atlas != null)
                {
                    spriteRight.atlas = atlas;
                    spriteRight.spriteName = item.ImageRight;
                }
                if (lblSpeakerNameLeft01 != null) lblSpeakerNameRight01.text = item.SpeakerName;
                if (lblSpeakerNameLeft02 != null) lblSpeakerNameRight02.text = item.SpeakerName;
                if (lblWordsRight != null) lblWordsRight.text = item.Words;
                isMonsterSpeaker = false;
            }
            else
            {
                if (spriteRight != null)
                    NGUITools.SetActive(spriteRight.gameObject, false);
#if DEBUG
                Debug.Log("!!!ImageLeftAtlas or spriteRight is null.");
#endif
            }
            if (isMonsterSpeaker)
            {
                if (lblSpeakerMonsterName != null) lblSpeakerMonsterName.text = item.SpeakerName;
                if (lblSpeakerMonsterWords != null) lblSpeakerMonsterWords.text = item.Words;
            }
            else
            {
                if (lblSpeakerMonsterName != null) lblSpeakerMonsterName.text = "";
                if (lblSpeakerMonsterWords != null) lblSpeakerMonsterWords.text = "";
            }
        }
    }
    //下一句
    private void NextStoryItem()
    {
        //剧情对话框处于活跃状态时，处理单击操作    
        if (m_IsStoryDlgActive == true)
        {
            CancelInvoke("NextStoryItem");
            if (null != m_StoryDlgGO)
            {
                bool isActive = NGUITools.GetActive(m_StoryDlgGO);
                if (isActive == true)
                {
                    if (m_Count < m_StepNumber)
                    {
                        StoryDlgItem item = m_StoryInfo.StoryItems[m_Count];
                        UpdateStoryDlg(m_StoryDlgGO.transform, item, m_Count);
                        NGUITools.SetActive(m_StoryDlgGO, true);
                        m_Count++;
                        float RealIntervalTime = item.IntervalTime;
                        if (m_StoryDlgType == StoryDlgType.Big)
                            RealIntervalTime = GetRealIntervalTime(item);
                        if (RealIntervalTime > 0.0f)
                        {
                            Invoke("NextStoryItem", RealIntervalTime);
                        }
                    }
                    else
                    {
                        FinishStoryDlg();
                    }
                }
            }
        }
    }
    //直接结束剧情对话
    private void StopStoryDlg()
    {
        CancelInvoke("NextStoryItem");
        if (m_IsStoryDlgActive == true)
        {
            if (null != m_StoryDlgGO)
            {
                FinishStoryDlg();
            }
        }
    }
    private void FinishStoryDlg()
    {
        CancelInvoke("NextStoryItem");
        m_IsStoryDlgActive = false;
        UIManager.Instance.HideWindowByName("StoryDlgSmall");
        NGUITools.SetActive(m_StoryDlgGO, false);
        m_StoryDlgGO = null;
        m_StoryInfo = null;
        m_EfHeadTrans = null;
        if (m_RunTimeEffect != null)
        {
            Destroy(m_RunTimeEffect);
        }
        RaiseStoryDlgOverEvent();
    }
    //剧情对话结束引发事件
    private void RaiseStoryDlgOverEvent()
    {
        ArkCrossEngine.LogicSystem.SendStoryMessage("dialogover:" + m_StoryId);
        StoryDlgManager.Instance.FireStoryEndMsg(m_StoryId);
    }
    //直接结束剧情对话，且不触发结束事件
    public void KillStoryDlg()
    {
        CancelInvoke("NextStoryItem");
        if (m_IsStoryDlgActive == true)
        {
            if (null != m_StoryDlgGO)
            {
                m_IsStoryDlgActive = false;
                UIManager.Instance.HideWindowByName("StoryDlgSmall");
                NGUITools.SetActive(m_StoryDlgGO, false);
                m_StoryDlgGO = null;
                m_StoryInfo = null;
                m_EfHeadTrans = null;
                if (m_RunTimeEffect != null)
                    Destroy(m_RunTimeEffect);
            }
        }
    }
    //在说话的角色头上播放特效
    private void PlayParticleByUnitId(int unitId)
    {
        //20001代表玩家的UnitId
        if (unitId == 20001)
        {
            //玩家说话
            UnityEngine.GameObject obj = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(LogicSystem.PlayerSelf);
            PlayParticleByGameObject(obj);
            return;
        }
        //NPC说话
        LogicSystem.PublishLogicEvent("ge_switch_actorid", "game", unitId);
    }
    private void PlayParticleByGameObject(UnityEngine.GameObject obj)
    {
        if (obj == null) return;
        UnityEngine.GameObject _gameobject = obj;
        UnityEngine.Transform trans = GfxModule.Skill.Trigers.TriggerUtil.GetChildNodeByName(_gameobject, "ef_head");
        m_EfHeadTrans = CrossObjectHelper.TryCastObject<UnityEngine.Transform>(trans);
        if (trans != null && EffectForSpeaker != null)
        {
            if (m_RunTimeEffect == null)
                m_RunTimeEffect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(EffectForSpeaker));
            if (m_RunTimeEffect != null)
            {
                m_RunTimeEffect.SetActive(true);
                m_RunTimeEffect.transform.position = new UnityEngine.Vector3(trans.position.x, trans.position.y, trans.position.z);
            }
        }
    }
    private void HandlerPlayParticle(int actorId)
    {
        UnityEngine.GameObject obj = LogicSystem.GetGameObject(actorId) as UnityEngine.GameObject;
        if (null != obj)
        {
            UnityEngine.Transform trans = GfxModule.Skill.Trigers.TriggerUtil.GetChildNodeByName(obj, "ef_head");
            m_EfHeadTrans = CrossObjectHelper.TryCastObject<UnityEngine.Transform>(trans);
            if (trans != null && EffectForSpeaker != null)
            {
                if (m_RunTimeEffect == null)
                    m_RunTimeEffect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(EffectForSpeaker));
                if (m_RunTimeEffect != null)
                {
                    m_RunTimeEffect.SetActive(true);
                    m_RunTimeEffect.transform.position = new UnityEngine.Vector3(trans.position.x, trans.position.y, trans.position.z);
                }
            }
        }
    }
    /*设置texAnimation图片*/
    private void SetTexAnimaitionImage(string path, UITexture texAnimation)
    {
        UnityEngine.Texture tex = CrossObjectHelper.TryCastObject<UnityEngine.Texture>(ArkCrossEngine.ResourceSystem.GetSharedResource(path));
        if (tex != null && texAnimation != null)
        {
            texAnimation.mainTexture = tex;
        }
    }
    /*统一按照屏幕大小，将TexAnimation放大Scale倍*/
    private void EnlargerTexAnimation(float scale, UITexture texAnimation)
    {
        if (texAnimation != null)
        {
            int texHeight = texAnimation.height;
            texAnimation.height = (int)(Screen.height * scale);
            //宽度等比例缩放
            texAnimation.width = (int)(texAnimation.width * (texAnimation.height / (float)texHeight));
        }
    }
    /*移动TexAnimation*/
    private void TweenTexAnimationPos(UnityEngine.Vector3 from, UnityEngine.Vector3 to, StoryDlgItem dlg_item, UITexture texAnimation)
    {
        TweenPosition tweenPos = texAnimation.GetComponent<TweenPosition>();
        if (tweenPos != null) Destroy(tweenPos);
        if (dlg_item == null) return;
        TweenPosition tween = texAnimation.gameObject.AddComponent<TweenPosition>();
        if (tween != null)
        {
            tween.from = from;
            tween.to = to;
            tween.delay = dlg_item.TweenPosDelay;
            tween.duration = dlg_item.TweenPosDuration;
            tween.AddOnFinished(OnTweenPositionFinished);
        }
    }
    private void OnTweenPositionFinished()
    {
        StartCoroutine(ShowWordOneByOne(ShowWordDuration));
    }
    /*缩放TexAnimation*/
    private void TweenTexAnimationScale(StoryDlgItem dlg_item, UITexture texAnimation)
    {
        if (dlg_item == null) return;
        if (dlg_item.ToScale < 0 || dlg_item.ToScale > 1) return;
        UnityEngine.Vector3 toScale = new UnityEngine.Vector3(dlg_item.ToScale, dlg_item.ToScale, 0);
        TweenScale tween = TweenScale.Begin(texAnimation.gameObject, dlg_item.TweenScaleDuration, toScale);
        if (tween != null)
        {
            tween.delay = dlg_item.TweenScaleDelay;
            tween.from = new UnityEngine.Vector3(dlg_item.FromScale, dlg_item.FromScale, 0);
        }
    }
    /*TexAnimation开始alpha为0，*/
    private void TweenAlphaAtStart(UITexture texAnimation)
    {
        TweenAlpha tween = TweenAlpha.Begin(texAnimation.gameObject, 0.1f, 1f);
        if (tween != null)
        {
            tween.delay = 0f;
            tween.AddOnFinished(OnTweenAlphaAtStartFinished);
            m_TweenAlphaOnStart = tween;
        }
    }
    private TweenAlpha m_TweenAlphaOnStart;
    private void OnTweenAlphaAtStartFinished()
    {
        if (m_TweenAlphaOnStart != null)
            EventDelegate.Remove(m_TweenAlphaOnStart.onFinished, OnTweenAlphaAtStartFinished);
        if (m_StoryDlgItem != null && m_TexAnimation != null)
            TweenTexAnimationAlpha(m_StoryDlgItem, m_TexAnimation);
    }
    /*TweenAlpha*/
    private void TweenTexAnimationAlpha(StoryDlgItem dlg_item, UITexture texAnimation)
    {
        TweenAlpha tw = texAnimation.GetComponent<TweenAlpha>();
        if (tw != null) Destroy(tw);
        if (dlg_item == null) return;
        TweenAlpha tween = texAnimation.gameObject.AddComponent<TweenAlpha>();
        tween.duration = dlg_item.TweenAlphaDuration;
        tween.delay = dlg_item.TweenAlphaDelay;
        tween.from = dlg_item.FromAlpha;
        tween.to = dlg_item.ToAlpha;
    }
    /*逐字显示*/
    public IEnumerator ShowWordOneByOne(float duration)
    {
        if (string.IsNullOrEmpty(m_DescriptionWords))
            yield break;
        float durationPerWord = duration / m_DescriptionWords.Length;
        m_DescriptionWords = m_DescriptionWords.Replace("[\\n]", "|");
        string[] wordsList = m_DescriptionWords.Split('|');
        for (int row = 0; row < wordsList.Length; ++row)
        {
            string row_words = wordsList[row];
            if (lblWords != null) lblWords.text = "";
            if (string.IsNullOrEmpty(row_words)) continue;
            for (int index = 0; index < row_words.Length; ++index)
            {
                if (lblWords != null)
                {
                    lblWords.text = row_words.Substring(0, index);
                    yield return new WaitForSeconds(durationPerWord);
                }
            }
        }
    }
    //根据图片显示的中心点位置、计算图片UI的世界坐标
    private UnityEngine.Vector3 GetAnimationTexWorldPos(float offsetLeft, float offsetBottom, UITexture texAnimation)
    {
        if (texAnimation == null) return UnityEngine.Vector3.zero;
        int texWidth = texAnimation.width;
        int texHeight = texAnimation.height;
        UnityEngine.Vector3 ScreenCenter = new UnityEngine.Vector3(Screen.width / 2, Screen.height / 2, 0);
        UnityEngine.Vector3 texScreenPos = new UnityEngine.Vector3();
        texScreenPos.z = 0;
        texScreenPos.x = ScreenCenter.x + (texWidth / 2 - texWidth * offsetLeft);
        texScreenPos.y = ScreenCenter.y + (texHeight / 2 - texHeight * offsetBottom);
        UnityEngine.Vector3 world_pos = UICamera.mainCamera.ScreenToWorldPoint(texScreenPos);
        return world_pos;
    }
    /*在 DlgPanelBig下，IntervaTime需要重新计算，即加上动画播放时间*/
    private float GetRealIntervalTime(StoryDlgItem item)
    {
        if (item == null) return 0f;
        float totle = 0f;
        //float alpha = item.TweenAlphaDelay + item.TweenAlphaDuration;
        float scale = item.TweenScaleDelay + item.TweenScaleDuration;
        float pos = item.TweenPosDelay + item.TweenPosDuration;
        float words = item.WordDuration;
        if (scale > totle) totle = scale;
        if (pos + words > totle) totle = pos + words;
        return totle + item.IntervalTime;
    }
    /*交换TexAnimationArr[0]和TexAnimationArr[1]的Depth,即将消失的TexAnimation在上面*/
    private void ExchangeDepth(int current)
    {
        if (current < 0) return;
        if (texAnimationArr[0] == null || texAnimationArr[1] == null) return;
        current = current % 2;
        if (texAnimationArr[current].depth > texAnimationArr[current ^ 1].depth)
        {
            int depth = texAnimationArr[current].depth;
            texAnimationArr[current].depth = texAnimationArr[current ^ 1].depth;
            texAnimationArr[current ^ 1].depth = depth;
        }
    }

    void Update()
    {
        try
        {
            if (m_RunTimeEffect != null && m_EfHeadTrans != null)
                m_RunTimeEffect.transform.position = m_EfHeadTrans.position;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}


