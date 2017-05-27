using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public enum TaskCompleteType : int
{
    T_common = 1, //普通
    T_Trea = 2, // 远征
}
public class TaskAward : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject goEffectTitle = null;
    public UnityEngine.GameObject goTitle = null;
    public UnityEngine.GameObject itemEffect = null;
    public UnityEngine.GameObject effectSprite = null;

    private List<ArkCrossEngine.GameObject> effectList = new List<ArkCrossEngine.GameObject>();
    private ArkCrossEngine.GameObject runtimeEffect;

    // Use this for initialization
    void Start()
    {
    }
    //播放任务奖励标题特效
    void PlayEffectTitle()
    {
        runtimeEffect = ArkCrossEngine.ResourceSystem.NewObject(
            CrossObjectHelper.TryConstructCrossObject(goEffectTitle)) as ArkCrossEngine.GameObject;
        if (runtimeEffect != null && goTitle != null)
        {
            runtimeEffect.transform.position = 
                new ArkCrossEngine.Vector3(goTitle.transform.position.x, goTitle.transform.position.y, goTitle.transform.position.z);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void CloseTaskAward()
    {
        if (sign == TaskAwardOpenForWindow.W_GameTask)
        {
            GameTask.awardtask.Remove(awardid);
            if (GameTask.awardtask.Count == 0)
            {
                UIManager.Instance.HideWindowByName("TaskAward");
                UIManager.Instance.ShowWindowByName("GameTask");
            }
            else
            {
                m_TaskId = GameTask.awardtask[0];
                SetAwardProperty(GameTask.awardtask[0]);
            }
            switch (taskType)
            {
                case TaskCompleteType.T_common:
                    //发送已读消息
                    ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_read_finish", "lobby", m_TaskId);
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_ui_award_finished", "ui");//通关副本按钮
                    ArkCrossEngine.GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, true);
                    break;
                case TaskCompleteType.T_Trea:
                    break;
            }
            ArkCrossEngine.RoleInfo role = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (role.LevelUp)
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_user_levelup", "property", role.Level);
                role.LevelUp = false;
            }
        }
        if (GameTask.awardtask.Count > 0)
        {
            SetAwardProperty(GameTask.awardtask[0]);
        }
        if (sign == TaskAwardOpenForWindow.W_MermanKing)
        {
            UIManager.Instance.HideWindowByName("TaskAward");
        }
        if (sign == TaskAwardOpenForWindow.W_SellGain)
        {
            UIManager.Instance.HideWindowByName("TaskAward");
        }
        if (sign == TaskAwardOpenForWindow.W_Trial)
        {
            UIManager.Instance.HideWindowByName("TaskAward");
        }
        if (sign == TaskAwardOpenForWindow.W_Activity)
        {
            UIManager.Instance.HideWindowByName("TaskAward");
        }
        DestroyEffect();
    }
    public int TaskId
    {
        get { return m_TaskId; }
    }
    //初始化任务id
    private int m_TaskId = 0;
    private TaskCompleteType taskType;
    public void InitTaskId(int id, TaskCompleteType type)
    {
        m_TaskId = id;
        taskType = type;
    }
    //销毁特效
    void DestroyEffect()
    {
        Destroy(runtimeEffect._GetImpl());
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i] != null)
            {
                ArkCrossEngine.GameObject.Destroy(effectList[i]);
            }
        }
        /*
    foreach (UnityEngine.GameObject go in effectList) {
      Destroy(go);
    }*/
    }
    public void SetAwardProperty(int id)
    {
        PlayEffectTitle();
        sign = TaskAwardOpenForWindow.W_GameTask;
        awardid = id;
        ArkCrossEngine.MissionConfig missionconfig = ArkCrossEngine.LogicSystem.GetMissionDataById(id);
        if (missionconfig != null)
        {
            UnityEngine.Transform tf = transform.Find("Back/Type/TypeDescription");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = missionconfig.Name;
                }
            }
            tf = transform.Find("Back/Challenge/ChallengeDescription");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = missionconfig.Description;
                }
            }
        }
        ArkCrossEngine.Data_SceneDropOut dsdo = ArkCrossEngine.SceneConfigProvider.Instance.GetSceneDropOutById(missionconfig.DropId);
        if (dsdo != null)
        {
            ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null && ri.GetMissionStateInfo() != null)
            {
                SetSomething(dsdo.m_GoldSum, dsdo.m_Diamond, ri.GetMissionStateInfo().GetMissionsExpReward(id, ri.Level), dsdo.GetRewardItemByHeroId(ri.HeroId), dsdo.m_ItemCountList);
            }
        }
    }
    public void SetSomething(int money, int diamond, int exp, List<int> itemlist, List<int> itemcount)
    {
        for (int i = 0; i < golist.Count; i++)
        {
            if (golist[i] != null)
            {
                NGUITools.DestroyImmediate(golist[i]);
            }
        }
        /*
    foreach (UnityEngine.GameObject go in golist) {
      if (go != null) {
        NGUITools.DestroyImmediate(go);
      }
    }*/
        golist.Clear();

        UnityEngine.Transform tfb = transform.Find("Back");
        if (tfb != null)
        {
            if (money > 0)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, DFMItemIconUtils.Instance.m_Money, money);
                    }
                }
            }
            if (diamond > 0)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, DFMItemIconUtils.Instance.m_Diamond, diamond);
                    }
                }
            }
            if (exp > 0)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, DFMItemIconUtils.Instance.m_Exp, exp);
                    }
                }
            }
            int count = itemlist.Count;
            for (int i = 0; i < count; ++i)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, itemlist[i], itemcount[i]);
                    }
                }
            }
        }
        int number = golist.Count;
        if (number == 0) return;
        int offset = 0;
        int start = 0;
        if (number % 2 != 0)
        {
            UnityEngine.GameObject go = golist[0];
            if (go != null)
            {
                go.transform.localPosition = new UnityEngine.Vector3(0.0f, 25f, 0.0f);
                start = 1;
                offset = 50;
            }
        }
        else
        {
            offset = -60;
        }
        for (int i = start; i < number; ++i)
        {
            int j = i;
            if (number % 2 == 0)
            {
                j = i + 1;
            }
            UnityEngine.GameObject go = golist[i];
            if (go != null)
            {
                if (j % 2 == 0)
                {
                    go.transform.localPosition = new UnityEngine.Vector3(j / 2 * (-120) - offset, 25, 0);
                }
                else
                {
                    go.transform.localPosition = new UnityEngine.Vector3((j / 2 + 1) * 120 + offset, 25f, 0.0f);
                }
            }
        }
        AddEffect(golist);
    }
    //运营活动：签到、七天、礼包
    public void SetAwardForActivity(List<int> items, List<int> nums)
    {
        sign = TaskAwardOpenForWindow.W_Activity;
        UnityEngine.Transform tf = transform.Find("Back/Type/TypeDescription");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(1157);
            }
        }
        SetSomething(0, 0, 0, items, nums);
    }
    //人鱼王宝藏领奖
    public void SetAwardForMermanKing(int money, int diamond, int exp, List<int> itemid, List<int> itemcount, int index)
    {
        sign = TaskAwardOpenForWindow.W_MermanKing;
        UnityEngine.Transform tf = transform.Find("Back/Type/TypeDescription");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(881) + (index + 1);
            }
        }
        SetSomething(money, diamond, exp, itemid, itemcount);
    }
    //试炼
    public void SetAwardForTrial(int money, int diamond, int exp, List<int> itemid, List<int> itemcount)
    {
        PlayEffectTitle();
        sign = TaskAwardOpenForWindow.W_Trial;
        UnityEngine.Transform tf = transform.Find("Back/Type/TypeDescription");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(874);
            }
        }
        SetSomething(money, diamond, exp, itemid, itemcount);
    }

    public void SetSellGain(string money, string diamond)
    {
        sign = TaskAwardOpenForWindow.W_SellGain;
        for (int i = 0; i < golist.Count; i++)
        {
            if (golist[i] != null)
            {
                NGUITools.DestroyImmediate(golist[i]);
            }
        }
        /*
    foreach (UnityEngine.GameObject go in golist) {
      if (go != null) {
        NGUITools.DestroyImmediate(go);
      }
    }*/
        golist.Clear();

        UnityEngine.Transform tfb = transform.Find("Back");
        if (tfb != null)
        {
            if (money != null)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        UnityEngine.Texture utt = GamePokeyManager.GetTextureByPicName("UI/GoodsPhoto/Money");
                        UITexture ut = go.GetComponent<UITexture>();
                        if (ut != null)
                        {
                            if (utt != null)
                            {
                                ut.mainTexture = utt;
                            }
                        }
                        UnityEngine.Transform tf = go.transform.Find("Label");
                        if (tf != null)
                        {
                            UILabel ul = tf.gameObject.GetComponent<UILabel>();
                            if (ul != null)
                            {
                                ul.text = money;
                            }
                        }
                    }
                }
            }
            if (diamond != null)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        UnityEngine.Texture utt = GamePokeyManager.GetTextureByPicName("UI/GoodsPhoto/Diamond");
                        UITexture ut = go.GetComponent<UITexture>();
                        if (ut != null)
                        {
                            if (utt != null)
                            {
                                ut.mainTexture = utt;
                            }
                        }
                        UnityEngine.Transform tf = go.transform.Find("Label");
                        if (tf != null)
                        {
                            UILabel ul = tf.gameObject.GetComponent<UILabel>();
                            if (ul != null)
                            {
                                ul.text = diamond;
                            }
                        }
                    }
                }
            }
        }
        int number = golist.Count;
        if (number == 0) return;
        int offset = 0;
        int start = 0;
        if (number % 2 != 0)
        {
            UnityEngine.GameObject go = golist[0];
            if (go != null)
            {
                go.transform.localPosition = new UnityEngine.Vector3(0.0f, 25f, 0.0f);
                start = 1;
                offset = 50;
            }
        }
        else
        {
            offset = -60;
        }
        for (int i = start; i < number; ++i)
        {
            int j = i;
            if (number % 2 == 0)
            {
                j = i + 1;
            }
            UnityEngine.GameObject go = golist[i];
            if (go != null)
            {
                if (j % 2 == 0)
                {
                    go.transform.localPosition = new UnityEngine.Vector3(j / 2 * (-120) - offset, 25, 0);
                }
                else
                {
                    go.transform.localPosition = new UnityEngine.Vector3((j / 2 + 1) * 120 + offset, 25, 0.0f);
                }
            }
        }
    }
    /*添加奖励物品特效*/
    void AddEffect(List<UnityEngine.GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                ArkCrossEngine.GameObject newGo = ArkCrossEngine.ResourceSystem.NewObject(CrossObjectHelper.TryConstructCrossObject(itemEffect)) as ArkCrossEngine.GameObject;
                if (newGo != null)
                {
                    newGo.transform.position = new ArkCrossEngine.Vector3(list[i].transform.position.x, list[i].transform.position.y, list[i].transform.position.z);
                }
                effectList.Add(newGo);
            }
        }
        /*
    foreach (UnityEngine.GameObject go in list) {
      UnityEngine.GameObject newGo = ArkCrossEngine.ResourceSystem.NewObject(itemEffect) as UnityEngine.GameObject;
      if (newGo != null && go != null) {
        newGo.transform.position = go.transform.position;
      }
      effectList.Add(newGo);
    }*/
    }
    private int awardid = 0;
    private List<UnityEngine.GameObject> golist = new List<UnityEngine.GameObject>();
    private TaskAwardOpenForWindow sign = TaskAwardOpenForWindow.W_GameTask;
}
public enum TaskAwardOpenForWindow : int
{
    W_GameTask = 0,
    W_MermanKing = 1,
    W_SellGain = 2,
    W_Trial = 3,
    W_Activity = 4
}
