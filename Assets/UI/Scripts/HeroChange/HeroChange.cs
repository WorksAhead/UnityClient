using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class HeroChange : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        //通知更换英雄
        /*/
         *根据返回结果设置英雄头像以及血条以及技能栏 
        /*/
        //ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_change_scene", "game", 6);
        /*LogicSystem.PublishLogicEvent("ge_change_hero", "game");*/
        m_HeroId = (m_HeroId + 1) % 4;
        if (m_HeroId == 0)
        {
            m_HeroId = 1;
        }
        ChangeHeroPanel();
        ChangePortrait();
    }

    private void SetHeroPortrait()
    {

    }

    private UnityEngine.GameObject GetSkillBarById(string id)
    {
        UnityEngine.GameObject go = null;
        UnityEngine.Transform trans = null;
        if (this.transform.parent == null)
            return go;
        for (int index = 0; index < this.transform.parent.childCount; ++index)
        {
            trans = this.transform.parent.GetChild(index);
            if (trans == null)
                continue;
            go = trans.gameObject;
            /*      SkillBar panel = go.GetComponent<SkillBar>();*/
            //       if (panel != null && go.name == "SkillBar(Clone)") {
            //         if (id == panel.GetId()) {
            //           return go;
            //         }
            //       }
        }
        return null;

    }
    private UnityEngine.GameObject GetHeroPanelById(string id)
    {
        UnityEngine.GameObject go = null;
        UnityEngine.Transform trans = null;
        if (this.transform.parent == null)
            return go;
        for (int index = 0; index < this.transform.parent.childCount; ++index)
        {
            trans = this.transform.parent.GetChild(index);
            if (trans == null)
                continue;
            go = trans.gameObject;
            HeroPanel panel = go.GetComponent<HeroPanel>();
            if (panel != null && go.name == "HeroPanel(Clone)")
            {

            }
        }
        return null;

    }

    private void ChangePortrait()
    {
        //在更换期间不允许点击
        UnityEngine.BoxCollider collider = this.GetComponent<UnityEngine.BoxCollider>();
        if (collider != null)
            collider.enabled = false;
        UnityEngine.Transform trans = null;
        UnityEngine.GameObject Qiangshou = null;
        UnityEngine.GameObject SwordMan = null;

        trans = this.transform.Find("Qiangshou");
        if (trans != null)
            Qiangshou = trans.gameObject;

        trans = this.transform.Find("SwordMan");
        if (trans != null)
            SwordMan = trans.gameObject;
        Qiangshou.gameObject.SetActive(true);
        SwordMan.gameObject.SetActive(true);
        if (Qiangshou == null || SwordMan == null)
            return;
        if (m_IsOldPortraitShow)
        {
            SetPortaitTween(SwordMan, Qiangshou);
            UISprite sp = Qiangshou.GetComponent<UISprite>();
            if (sp != null && m_HeroId <= m_HeroPortraitId.Length)
            {
                int index = (m_HeroId == m_HeroPortraitId.Length) ? 0 : m_HeroId;
                sp.spriteName = m_HeroPortraitId[index].ToString();
            }
            m_IsOldPortraitShow = false;
        }
        else
        {
            SetPortaitTween(Qiangshou, SwordMan);
            UISprite sp = SwordMan.GetComponent<UISprite>();
            if (sp != null && m_HeroId <= m_HeroPortraitId.Length)
            {
                int index = (m_HeroId == m_HeroPortraitId.Length) ? 0 : m_HeroId;
                sp.spriteName = m_HeroPortraitId[index].ToString();
            }
            m_IsOldPortraitShow = true;
        }
    }

    private void ChangeHeroPanel()
    {
        UnityEngine.GameObject newHeroPanel = null;
        UnityEngine.GameObject oldHeroPanel = null;

        oldHeroPanel = GetHeroPanelById("first");
        if (oldHeroPanel == null)
        {
            return;
        }
        if (!m_IsHeroPanelInitial)
        {
            m_HeroPanelOriginalPos = oldHeroPanel.transform.localPosition;
            m_IsHeroPanelInitial = true;
        }
        if (this.transform.parent != null)
        {
            newHeroPanel = GetHeroPanelById("second");
            if (newHeroPanel == null)
            {
                newHeroPanel = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/HeroPanel"));
                if (null == newHeroPanel)
                    return;
                if (this.transform.parent != null)
                {
                    newHeroPanel = NGUITools.AddChild(this.transform.parent.gameObject, newHeroPanel);
                    if (null != newHeroPanel)
                    {
                        newHeroPanel.transform.localPosition = oldHeroPanel.transform.localPosition;
                        HeroPanel panel = newHeroPanel.GetComponent<HeroPanel>();
                        if (null != panel)
                        {
                        }
                    }
                }
            }
        }
        if (null == newHeroPanel)
            return;
        SetPortrait(newHeroPanel);
        SetHeroPanelTween(oldHeroPanel, newHeroPanel);
        m_OldPanel = oldHeroPanel;
    }

    private void SetSkillBarTween(UnityEngine.GameObject oldSkillBar, UnityEngine.GameObject newSkillBar)
    {
        AddTweenAlpha(newSkillBar, 0f, 1f, 0.3f);
        AddTweenAlpha(oldSkillBar, 1f, 0f, 0.3f);
        UnityEngine.Vector3 oldPos = m_SkillBarOriginalPos;
        UnityEngine.Vector3 newPos = new UnityEngine.Vector3(oldPos.x - 200, oldPos.y, oldPos.z);
        AddTweenPos(oldSkillBar, oldPos, newPos, 0.3f);
        newPos.x += 300;
        AddTweenPos(newSkillBar, newPos, oldPos, 0.3f);

    }
    private void SetPortaitTween(UnityEngine.GameObject oldPortrait, UnityEngine.GameObject newPortait)
    {
        AddTweenAlpha(newPortait, 0f, 1f, 0.3f);
        AddTweenAlpha(oldPortrait, 1f, 0f, 0.3f);
        UnityEngine.Vector3 oldPos = new UnityEngine.Vector3(0f, 0f, 0f);
        UnityEngine.Vector3 newPos = new UnityEngine.Vector3(oldPos.x, oldPos.y - 80, oldPos.z);
        AddTweenPos(oldPortrait, oldPos, newPos, 0.3f);
        newPos.y += 160;
        AddTweenPos(newPortait, newPos, oldPos, 0.3f);

    }

    private void SetPortrait(UnityEngine.GameObject fatherGo)
    {
        if (null == fatherGo)
            return;
        UnityEngine.Transform ts = fatherGo.transform.Find("HeroPortrait");
        if (null != ts)
        {
            UnityEngine.GameObject portrait = ts.gameObject;
            UISprite sp = portrait.GetComponent<UISprite>();
            if (sp != null && m_HeroId <= m_HeroPortraitId.Length)
            {
                sp.spriteName = m_HeroPortraitId[m_HeroId - 1].ToString();
            }
        }

    }

    private void SetHeroPanelTween(UnityEngine.GameObject oldHeroPanel, UnityEngine.GameObject newHeroPanel)
    {
        AddTweenAlpha(newHeroPanel, 0f, 1f, 0.3f);
        AddTweenAlpha(oldHeroPanel, 1f, 0f, 0.3f);
        UnityEngine.Vector3 oldPos = m_HeroPanelOriginalPos;
        UnityEngine.Vector3 newPos = new UnityEngine.Vector3(oldPos.x, oldPos.y - 200, oldPos.z);
        UnityEngine.Vector3 fromScale = new UnityEngine.Vector3(0.5f, 0.5f, 1f);
        UnityEngine.Vector3 toScale = new UnityEngine.Vector3(1f, 1f, 1f);
        AddTweenPos(newHeroPanel, newPos, oldPos, 0.3f);
        newPos.y += 100;
        AddTweenPos(oldHeroPanel, oldPos, newPos, 0.3f);

        AddTweenScale(newHeroPanel, toScale, toScale, 0.3f);
        fromScale.x = 1f;
        fromScale.y = 1f;
        toScale.x = 0.1f;
        toScale.y = 0.1f;
        AddTweenScale(oldHeroPanel, fromScale, toScale, 0.3f);
    }

    private void AddTweenAlpha(UnityEngine.GameObject father, float from, float to, float duration)
    {
        if (null == father)
            return;
        TweenAlpha alpha = father.GetComponent<TweenAlpha>();
        if (null != alpha) { Destroy(alpha); alpha = null; }
        alpha = father.AddComponent<TweenAlpha>();
        if (null != alpha)
        {
            alpha.duration = duration;
            alpha.from = from;
            alpha.to = to;
        }

    }
    private void AddTweenPos(UnityEngine.GameObject father, UnityEngine.Vector3 from, UnityEngine.Vector3 to, float duration)
    {
        if (null == father)
            return;
        TweenPosition tweenPos = father.GetComponent<TweenPosition>();
        if (null != tweenPos) { Destroy(tweenPos); tweenPos = null; }
        tweenPos = father.AddComponent<TweenPosition>();
        if (null != tweenPos)
        {
            tweenPos.duration = duration;
            tweenPos.from = from;
            tweenPos.to = to;
        }
    }
    private void AddTweenScale(UnityEngine.GameObject father, UnityEngine.Vector3 from, UnityEngine.Vector3 to, float duration)
    {
        if (null == father)
            return;
        TweenScale tweenScale = father.GetComponent<TweenScale>();
        if (null != tweenScale) { Destroy(tweenScale); tweenScale = null; }
        tweenScale = father.AddComponent<TweenScale>();
        if (null != tweenScale)
        {
            tweenScale.duration = duration;
            tweenScale.from = from;
            tweenScale.to = to;
        }
        //假定SkillBar和HeroPanel消失的时间相同
        EventDelegate.Add(tweenScale.onFinished, this.OnOldTweenFinished);
    }

    //结束时同时销毁OldPanel和OldSkillBar
    private void OnOldTweenFinished()
    {
        if (m_OldPanel != null)
            DestroyImmediate(m_OldPanel);
        if (m_OldSkillBar != null)
        {
            SkillBar skillBar = m_OldSkillBar.GetComponent<SkillBar>();
            if (null != skillBar)
                skillBar.UnSubscribe();
            DestroyImmediate(m_OldSkillBar);
        }
        //更换结束时、充值按钮使其可被点击
        UnityEngine.BoxCollider collider = this.GetComponent<UnityEngine.BoxCollider>();
        if (null != collider)
        {
            collider.enabled = true;
        }
    }

    public void OnClickTest()
    {
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/StageClear"));
        if (null == go)
            Debug.LogError("StageClear is Null");
        //go = NGUITools.AddChild(this.transform.parent.gameObject, go);
    }

    private UnityEngine.Vector3 m_SkillBarOriginalPos = new UnityEngine.Vector3();
    private UnityEngine.Vector3 m_HeroPanelOriginalPos = new UnityEngine.Vector3();
    private bool m_IsHeroPanelInitial = false;

    private bool m_IsOldPortraitShow = true;
    //切换英雄时，临时记录当前英雄的Id
    private int[] m_HeroPortraitId = new int[3] { 1, 2, 1 };
    private int m_HeroId = 1;
    private UnityEngine.GameObject m_OldPanel = null;
    private UnityEngine.GameObject m_OldSkillBar = null;

}
