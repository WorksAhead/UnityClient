public class UIChargePanel : UnityEngine.MonoBehaviour
{

    public UILabel noticeLabel = null;
    public UIChargeMoney chargeMoney = null;
    // Use this for initialization
    void Start()
    {
        try
        {
            SetNotice("1");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //通过读配置表来设置文本信息
    public void SetNotice(string msg)
    {
        msg = "好消息：\n    [ffff00]首次充值将获得2000钻[-]";
        if (noticeLabel == null)
            return;
        noticeLabel.text = msg;
    }

}
