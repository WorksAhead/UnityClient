using ArkCrossEngine;
/// <summary>
/// 七天登录
/// </summary>
public class UIActivityLoginAward : UnityEngine.MonoBehaviour
{

    public ActivityTypeEnum activityType = ActivityTypeEnum.WEEKLY_LOGIN_REWARD;
    private const int c_WeeklyNum = 7;
    public UIActivityLoginSlot[] uiLoginSlotArr = new UIActivityLoginSlot[c_WeeklyNum];
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        InitLoginAward();
    }
    public void InitLoginAward()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info == null) return;
        if (WeeklyLoginConfigProvider.Instance.IsUnderProgress())
        {
            WeeklyLoginConfig weeklyCfg = WeeklyLoginConfigProvider.Instance.GetDataByType(activityType);
            if (weeklyCfg != null)
            {
                int todayIndex = WeeklyLoginConfigProvider.Instance.GetTodayIndex();
                for (int index = 0; index < weeklyCfg.RewardItemIdList.Count; ++index)
                {
                    if (index < uiLoginSlotArr.Length && uiLoginSlotArr[index] != null)
                    {
                        int itemId = -1;
                        if (weeklyCfg.RewardItemIdList.Count > index)
                            itemId = weeklyCfg.RewardItemIdList[index];
                        int itemNum = 0;
                        if (weeklyCfg.RewardItemNumList.Count > index)
                            itemNum = weeklyCfg.RewardItemNumList[index];
                        bool isGetReward = false;
                        isGetReward = IsGetReward(index);
                        uiLoginSlotArr[index].Init(isGetReward, todayIndex, index, itemId, itemNum, weeklyCfg.StartTime);
                    }
                }
            }
        }
        else
        {
            if (c_WeeklyNum <= uiLoginSlotArr.Length && uiLoginSlotArr[c_WeeklyNum - 1] != null)
            {
                uiLoginSlotArr[c_WeeklyNum - 1].EnableButton(false);
            }
        }
    }
    private bool IsGetReward(int index)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info.WeeklyLoginRewardRecord.Contains(index))
            return true;
        return false;
    }
    public void HandleGetRewardSuccess()
    {
        int index = WeeklyLoginConfigProvider.Instance.GetTodayIndex();
        if (index < uiLoginSlotArr.Length && uiLoginSlotArr[index] != null)
        {
            uiLoginSlotArr[index].HandleSetGetReward();
        }
    }

}
