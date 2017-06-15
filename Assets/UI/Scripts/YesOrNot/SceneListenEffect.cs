using ArkCrossEngine;
using System.Collections.Generic;
using System;
public class SceneListenEffect : UnityEngine.MonoBehaviour
{

    private List<object> m_EventList = new List<object>();
    public UnityEngine.GameObject goEffect;// boss出场和关卡开始特效
    public UnityEngine.GameObject defensEffect; // 防御关卡特效
    public UnityEngine.GameObject clearEffect; // 区域清楚特效
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
    // Use this for initialization
    void Start()
    {
        try
        {
            goEffect = ArkCrossEngine.ResourceSystem.GetSharedResource("UI/YesOrNot/Changjing") as UnityEngine.GameObject;
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe<string>("pve_boss_enter", "ui_effect", OnBossEnter);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<string, int, int>("pve_checkpoint_begin", "ui_effect", OnSceneBegin);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("pve_area_clear", "ui_effect", OnAreaClear);

            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("pve_checkpoint_type", "ui_effect", OnCheckpointTypeEffect);
            if (obj != null) m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    //关卡类型 （被击，防御，挑战，突袭 等）管卡特效
    void OnCheckpointTypeEffect(int type)
    {
        PlayDefenseEffect(type);
    }
    //区域清楚特效 0:小clear  1：大
    void OnAreaClear(int type)
    {
        PlayAreaEffect(type);
    }
    //boss 出场特效
    void OnBossEnter(string name)
    {
        PlayEffect(ChangJingEffect.OpenSceneType.T_Boss, name);
    }
    //场景 刚刚开始特效
    void OnSceneBegin(string name, int chapter, int section)
    {
        PlayEffect(ChangJingEffect.OpenSceneType.T_Checkpoint, name, chapter, section);
    }
    // 播放特效
    void PlayEffect(ChangJingEffect.OpenSceneType type, string name, int chapter = 0, int section = 0)
    {
        UnityEngine.Object effect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.NewObject(goEffect));
        UnityEngine.GameObject go = NGUITools.AddChild(this.gameObject, effect);
        ChangJingEffect changjing = go.GetComponent<ChangJingEffect>();
        changjing.OpenEffectType(type, name, chapter, section);

    }
    // 播放区域清除特效0:小clear  1：大
    void PlayAreaEffect(int type)
    {
        UnityEngine.Object effect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(clearEffect));
        UnityEngine.GameObject go = NGUITools.AddChild(this.gameObject, effect);
        StageClearEffect stageClear = go.GetComponent<StageClearEffect>();
        stageClear.OpenEffectType(type);
    }
    // 防御关卡播放特效 type = 0,1,2,3(被击，防御，挑战，突袭)

    void PlayDefenseEffect(int type)
    {
        UnityEngine.Object ob = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(defensEffect));
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        UnityEngine.Transform defensPanel = go.transform.Find("UIPanel_3/UIAnchor-Center");
        UnityEngine.GameObject effect = NGUITools.AddChild(defensPanel.gameObject, ob);
        DefenseEffect defenseEffectScript = effect.GetComponent<DefenseEffect>();
        defenseEffectScript.InitType(type);
    }
}
