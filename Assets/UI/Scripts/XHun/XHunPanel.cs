using UnityEngine;
using System.Collections.Generic;
using ArkCrossEngine;
using System;

public class XHunPanel : UnityEngine.MonoBehaviour
{

    private List<object> m_EventList = new List<object>();
    private List<UnityEngine.GameObject> m_ViewList = new List<UnityEngine.GameObject>();

    public UnityEngine.GameObject effectUpgrade = null;
    public UnityEngine.GameObject posUpgrade = null;
    public XHunTitle title = null;
    public UnityEngine.GameObject viewItem = null;
    public BottomInfoContainer bottomInfoContainer = null;
    public RightInjectContainer rightInjectContainer = null;
    public UILabel nameLabel = null;
    public UILabel zhanliLabel = null;
    public UILabel lvLabel = null;
    public UILabel progressLabel = null;
    public UIProgressBar progress = null;
    public UISprite currentView = null;
    public UnityEngine.GameObject view = null;
    public UIScrollView scrollView = null;
    public UICenterOnChild centerOnChild = null;
    public float injectInterval = 0.3f;

    public XSoulPart currentPart;
    private int currentId;
    private int currentLv;
    private int currentExp;
    private bool isInjectBtnPressed;
    private float pressedTime;
    private string atlasName = "";
    private TweenProgressBar pressTween = null;
    private bool lvup = false;
    private bool isStart = true;
    private int showLv = -1;
    private int realLv = -1;

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
    void Start()
    {
        try
        {
            Init();
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, int>("ge_addsoul_experience_result", "XSoul", OnAddXSoulExperienceResult);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<XSoulPart, int, int>("ge_xsoul_changemodel_result", "XSoul", OnChangeShowResult);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_item_change", "item", CheckUpgradeTip);
            if (obj != null)
                m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void Init()
    {
        isStart = true;
        UIManager.Instance.HideWindowByName("XHun");
        CheckUpgradeTip();
    }

    void Update()
    {
        try
        {
            if (isInjectBtnPressed)
            {
                if (UnityEngine.Time.time - pressedTime > injectInterval)
                {
                    pressedTime = UnityEngine.Time.time;
                    OnInjectButtonClick();
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnEnable()
    {
        try
        {
            isInjectBtnPressed = false;
            currentPart = XSoulPart.kWeapon;//todo test(需要在点击开面板时赋值)

            if (progress != null)
            {
                pressTween = progress.gameObject.GetComponent<TweenProgressBar>();
                pressTween.onProgressFun = OnExpProgress;
                pressTween.onProgressFinish = OnExpProgressFinish;
                pressTween.onProgressMax = OnExpProgressMax;
            }
            //更新数据
            UpdateAll();

            InitViews();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnDisable()
    {
        try
        {
            for (int i = 0; i < m_ViewList.Count; i++)
            {
                Destroy(m_ViewList[i]);
            }
            m_ViewList.Clear();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void UpdateShowSelect()
    {
        realLv = realLv < currentLv ? currentLv : realLv;
        for (int i = 0; i < m_ViewList.Count; i++)
        {
            UnityEngine.Transform tf = m_ViewList[i].transform.Find("CheckBox");
            if (tf != null)
            {
                if (i + 1 > realLv)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
                else
                {
                    NGUITools.SetActive(tf.gameObject, true);
                }
                UIToggle toggle = tf.GetComponent<UIToggle>();
                if (toggle != null)
                {
                    if (i + 1 == showLv)
                    {
                        toggle.value = true;
                    }
                    else
                    {
                        toggle.value = false;
                    }
                }
            }
        }
    }

    private void OnChangeShowResult(XSoulPart part, int model_level, int result)
    {
        if (result == (int)ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
        {
            showLv = model_level;
        }
        else
        {
            UpdateShowSelect();
            Debug.Log("---show model change failed to level " + model_level);
        }
    }

    //生成预览，并布局
    private void InitViews()
    {
        if (centerOnChild != null && viewItem != null)
        {
            XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(currentId);
            if (config == null)
                return;
            int maxlv = config.m_MaxLevel;
            for (int i = 0; i < maxlv; i++)
            {
                UnityEngine.GameObject go = NGUITools.AddChild(centerOnChild.gameObject, viewItem);
                m_ViewList.Add(go);
                UnityEngine.Transform tf = go.transform.Find("Label");
                if (tf != null)
                {
                    UILabel lblLv = tf.gameObject.GetComponent<UILabel>();
                    if (lblLv != null)
                    {
                        lblLv.text = "Lv." + (i + 1);
                    }
                }
                tf = go.transform.Find("Sprite");
                if (tf != null)
                {
                    UISprite sprite = tf.gameObject.GetComponent<UISprite>();
                    if (sprite != null)
                    {
                        sprite.atlas.name = atlasName;
                        ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(currentId);
                        string[] icons = itemConfig.m_UseIconOnLevel;
                        if (i < icons.Length)
                        {
                            sprite.spriteName = icons[i];
                        }
                    }
                }
                XHunViewItem item = go.GetComponent<XHunViewItem>();
                if (item != null)
                {
                    item.lv = i + 1;
                    item.onToggleChange = SendChangeShowLv;
                }
            }
            UIGrid grid = centerOnChild.gameObject.GetComponent<UIGrid>();
            if (grid != null)
            {
                NGUITools.SetActive(view, false);
                grid.Reposition();//目的使grid执行init();
                NGUITools.SetActive(view, true);//显示，目的是布局
                grid.Reposition();//init后的grid排序才有效
                NGUITools.SetActive(view, false);//重置为隐藏
            }

            UpdateShowSelect();
        }
    }

    //part 类型武器、翅膀等
    //item_id 魂丹id
    //item_num 魂丹数量
    //exp 当前经验
    private void OnAddXSoulExperienceResult(int part, int item_id, int item_num, int exp, int result)
    {
        if (result == (int)ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
        {
            UpdateHunNum();//魂丹数量
                           //更新经验条
            RoleInfo role = LobbyClient.Instance.CurrentRole;
            XSoulInfo<XSoulPartInfo> xsoul_info = role.GetXSoulInfo();
            ItemDataInfo itemInfo = xsoul_info.GetXSoulPartData(currentPart).XSoulPartItem;
            currentExp = itemInfo.CurLevelExperience;
            realLv = itemInfo.Level;

            if (itemInfo.Level != currentLv)
            {//升级
             //OnLevelUp();
                CheckUpgradeTip();
                showLv = realLv;
                UpdateShowSelect();
                lvup = true;
            }
            else
            {
                lvup = false;
            }
            UpdateExp(currentLv, currentExp, lvup);
        }
    }

    //升级时
    private void OnLevelUp()
    {
        UpdateAll(true);
        if (effectUpgrade != null)
        {
            UnityEngine.GameObject ef = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(effectUpgrade));
            if (ef != null && posUpgrade != null)
            {
                ef.transform.position = new UnityEngine.Vector3(posUpgrade.transform.position.x, posUpgrade.transform.position.y, posUpgrade.transform.position.z);
                Destroy(ef, 2f);
            }
        }
    }

    //初始化时更新数据
    private void UpdateAll(bool isLvUp = false)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        XSoulInfo<XSoulPartInfo> xsoul_info = role.GetXSoulInfo();
        if (xsoul_info == null) return;
        XSoulPartInfo xsoul_part_into = xsoul_info.GetXSoulPartData(currentPart);
        if (xsoul_part_into == null) return;
        ItemDataInfo itemInfo = xsoul_part_into.XSoulPartItem;
        if (itemInfo == null)
            return;
        currentExp = itemInfo.CurLevelExperience;
        currentId = itemInfo.ItemId;
        currentLv = itemInfo.Level;
        showLv = xsoul_part_into.ShowModelLevel;
        showLv = showLv <= 0 ? currentLv : showLv;

        ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(currentId);
        //if (nameLabel != null) {
        //  nameLabel.text = itemConfig.m_ItemName;
        //}
        //底部label内容
        if (bottomInfoContainer != null)
        {
            bottomInfoContainer.InitLabels(itemInfo);
        }
        //初始化展示图集
        atlasName = itemConfig.m_ItemTrueName;//图集
                                              //当前等级展示
        if (currentView != null)
        {
            currentView.atlas.name = atlasName;
            string[] icons = itemConfig.m_UseIconOnLevel;
            currentView.spriteName = icons[currentLv - 1];//"UI_Shenqi_03";
        }

        //更新左侧数据,战力，等级，经验条
        UpdateExp(currentLv, currentExp);
        if (zhanliLabel != null)
        {
            zhanliLabel.text = "[i]+" + GetAddedFightScore();
            if (isLvUp == true)
            {
                TweenScale ts = zhanliLabel.gameObject.GetComponent<TweenScale>();
                UnityEngine.AnimationCurve tempcurve = ts.animationCurve;
                ts = TweenScale.Begin(zhanliLabel.gameObject, ts.duration, ts.to);
                ts.animationCurve = tempcurve;
            }
        }
        if (lvLabel != null)
        {
            lvLabel.text = currentLv.ToString();
            if (isLvUp == true)
            {
                TweenScale ts = lvLabel.gameObject.GetComponent<TweenScale>();
                UnityEngine.AnimationCurve tempcurve = ts.animationCurve;
                ts = TweenScale.Begin(lvLabel.gameObject, ts.duration, ts.to);
                ts.animationCurve = tempcurve;
            }
        }
        //更新底部数据，属性，描述
        if (bottomInfoContainer != null)
        {
            bottomInfoContainer.UpdateView(itemInfo);
        }
        //右侧魂丹数量
        UpdateHunNum();
        if (title != null)
        {
            title.UpdateTitleInfo();
        }
    }

    private int GetAddedFightScore()
    {
        float score = 0;
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        UserInfo userInfo = role_info.GetPlayerSelfInfo();
        ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(currentId);
        if (itemConfig != null)
        {
            float addHp = itemConfig.m_AttrData.GetAddHpMax(0, userInfo.GetLevel(), currentLv - 1);
            float addAd = itemConfig.m_AttrData.GetAddAd(0, userInfo.GetLevel(), currentLv - 1);
            float addAdp = itemConfig.m_AttrData.GetAddADp(0, userInfo.GetLevel(), currentLv - 1);
            float addMdp = itemConfig.m_AttrData.GetAddMDp(0, userInfo.GetLevel(), currentLv - 1);

            score += AttributeScoreConfigProvider.Instance.GetAloneAttributeScore(AttributeScoreName.HP, addHp);
            score += AttributeScoreConfigProvider.Instance.GetAloneAttributeScore(AttributeScoreName.AD, addAd);
            score += AttributeScoreConfigProvider.Instance.GetAloneAttributeScore(AttributeScoreName.ADP, addAdp);
            score += AttributeScoreConfigProvider.Instance.GetAloneAttributeScore(AttributeScoreName.MDP, addMdp);
        }

        return (int)score;
    }

    //更新经验条
    private void UpdateExp(int level, int exp, bool isLvUp = false)
    {
        XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(currentId);
        int maxExp = GetMaxExp();
        if (progress != null)
        {
            float val = 0;
            if (level == config.m_MaxLevel)
            {
                val = 1;
            }
            else
            {
                if (isLvUp == true)
                {//升级的话先读满
                    val = 1;
                }
                else
                {//最终等级的进度
                    val = (float)exp / maxExp;
                }
            }
            if (isStart)
            {//第一次不缓动，直接赋值
                progress.value = val;
                isStart = false;
                if (level == config.m_MaxLevel)
                {
                    progressLabel.text = "";
                }
                else
                {
                    progressLabel.text = exp + "/" + maxExp;
                }
            }
            else
            {
                pressTween.SetValue(val);
            }
        }
    }

    //经验条走动过程中update触发
    private void OnExpProgress(float value)
    {
        //更新进度值label变化
        int maxExp = GetMaxExp();
        int exp = (int)(progress.value * maxExp);
        progressLabel.text = exp + "/" + maxExp;
    }

    //经验条停止
    private void OnExpProgressFinish()
    {
        //矫正进度值label显示，最终值
        progressLabel.text = currentExp + "/" + GetMaxExp();
        XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(currentId);
        //满级隐藏
        if (currentLv == config.m_MaxLevel)
        {
            progressLabel.text = "";
        }
    }

    //经验条走满时执行
    private void OnExpProgressMax()
    {
        XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(currentId);
        //如果还没到最大等级-1，设置进度条为0，从头开始
        if (currentLv + 1 < config.m_MaxLevel)
        {
            progress.value = 0;
        }
        //触发升级更新
        OnLevelUp();
    }

    //获得当前等级最大经验值
    private int GetMaxExp()
    {
        XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(currentId);
        int maxExp = 0;
        int lv = currentLv;
        int nextLv = lv + 1 <= config.m_MaxLevel ? lv + 1 : config.m_MaxLevel;
        config.m_LevelExperience.TryGetValue(nextLv, out maxExp);
        return maxExp;
    }

    //更新魂丹数量
    private void UpdateHunNum()
    {
        if (rightInjectContainer != null)
        {
            XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(currentId);
            int[] hunIds = config.m_ExperienceProvideItems;
            rightInjectContainer.UpdateHunNum(hunIds);
        }
    }

    //点击注入按钮，发送注入消息, 返回是否可发送
    private bool OnInjectButtonClick()
    {
        if (rightInjectContainer != null)
        {
            HunType type = rightInjectContainer.GetSelectHunType();
            XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(currentId);
            if (currentLv == config.m_MaxLevel)
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(800),
                  ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null/*fun*/, false);
                return false;
            }
            int[] hunIds = config.m_ExperienceProvideItems;
            int hunid = 0;
            switch (type)
            {
                case HunType.Small:
                    hunid = hunIds[0];
                    break;
                case HunType.Middle:
                    hunid = hunIds[1];
                    break;
                case HunType.Big:
                    hunid = hunIds[2];
                    break;
            }
            LogicSystem.PublishLogicEvent("ge_addxsoul_experience", "lobby", currentPart, hunid, 1);
            return true;
        }
        return false;
    }

    public void OnInjectButtonPress()
    {
        pressedTime = UnityEngine.Time.time;
        isInjectBtnPressed = OnInjectButtonClick();
    }

    public void OnInjectButtonRelease()
    {
        isInjectBtnPressed = false;
    }

    //点击预览
    public void OnViewButtonClick()
    {
        if (view != null)
        {
            NGUITools.SetActive(view, true);
            if (m_ViewList.Count > 0)
            {
                centerOnChild.CenterOn(m_ViewList[showLv - 1].transform);
            }
        }
    }

    public void OnCloseView()
    {
        if (view != null)
        {
            NGUITools.SetActive(view, false);
        }
    }

    private void SendChangeShowLv(int lv)
    {
        if (lv != showLv)
            LogicSystem.PublishLogicEvent("ge_xsoul_changemodel", "lobby", currentPart, lv);
    }

    private void CheckUpgradeTip()
    {
        bool has = false;
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        XSoulInfo<XSoulPartInfo> xsoul_info = role.GetXSoulInfo();
        if (xsoul_info == null)
            return;
        XSoulPartInfo xsoul_part_into = xsoul_info.GetXSoulPartData(currentPart);
        if (xsoul_part_into == null)
            return;
        ItemDataInfo itemInfo = xsoul_part_into.XSoulPartItem;
        if (itemInfo == null)
            return;
        int curExp = itemInfo.CurLevelExperience;
        int id = itemInfo.ItemId;
        int lv = itemInfo.Level;
        XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(id);
        if (lv < config.m_MaxLevel)
        {
            int maxExp = 0;
            int nextLv = lv + 1 <= config.m_MaxLevel ? lv + 1 : config.m_MaxLevel;
            config.m_LevelExperience.TryGetValue(nextLv, out maxExp);
            int remainExp = maxExp - curExp;//剩余经验
            int[] hunIds = config.m_ExperienceProvideItems;
            int hunExp = 0;//魂丹总经验
            for (int i = 0; i < hunIds.Length; i++)
            {
                ItemDataInfo item = GetItem(hunIds[i]);
                if (item != null)
                {
                    ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(hunIds[i]);
                    if (itemConfig != null)
                    {
                        hunExp += item.ItemNum * itemConfig.m_ExperienceProvide;
                    }
                }
            }
            if (remainExp <= hunExp)
            {
                has = true;
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Xhun, has);
    }

    private ItemDataInfo GetItem(int itemid)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        foreach (ItemDataInfo item in role.Items)
        {
            if (item.ItemId == itemid)
            {
                return item;
            }
        }
        return null;
    }
    //private void OnViewCenterOn ()
    //{
    //  int index = Convert.ToInt32(centerOnChild.centeredObject.name.Split('_')[1]);
    //  int j = 0;
    //  int c = 0;
    //  for (int i = index; i > 0; i--) {
    //    UpdateDepth(i, j, c);
    //    j += 5;
    //    c++;
    //  }
    //  j = 5;
    //  c = 1;
    //  for (int i = index + 1; i < 10; i++) {
    //    UpdateDepth(i, j, c);
    //    j += 5;
    //    c++;
    //  }
    //}

    //private void UpdateDepth(int i, int j, int c)
    //{
    //  UnityEngine.Transform tf = centerOnChild.transform.Find("Sprite_" + i);
    //  if (tf != null) {
    //    UISprite sprite = tf.GetComponent<UISprite>();
    //    sprite.depth = 100 - j;
    //    UnityEngine.Transform tf2 = tf.Find("Label");
    //    if (tf2 != null) {
    //      tf2.GetComponent<UILabel>().depth = 101 - j;
    //    }
    //    tf2 = tf.Find("Sprite");
    //    if (tf2 != null) {
    //      tf2.GetComponent<UISprite>().depth = 102 - j;
    //    }
    //    //color
    //    sprite.color = new UnityEngine.Color(1, 1, 1, (255 - 50 * c) / 255f);

    //    if (j == 0) {//中间的
    //      sprite.spriteName = "lv-kuang-di2";
    //      sprite.SetDimensions(290, 290);
    //    } else {//其他
    //      sprite.spriteName = "hei-di";
    //      sprite.SetDimensions(260, 260);
    //    }
    //  }

    //}

}
