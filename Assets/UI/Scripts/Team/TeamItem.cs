public class TeamItem : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*点击拒绝*/
    public void OnClickReject()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_reject_team", "team", this.gameObject);
    }
    /*点击同意*/
    public void OnClickAgree()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_agree_team", "team", this.gameObject);
    }
    /*点击离开*/
    public void OnClickLeave()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_leave_team", "team", this.gameObject);
    }

    /*点击item*/
    public void onClickItem()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_item_team", "team", this.gameObject);
    }
}
