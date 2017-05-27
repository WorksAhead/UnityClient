using ArkCrossEngine;

public class UIActivityGift : UnityEngine.MonoBehaviour
{

    public UIInput inputField;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //点击兑换
    public void OnExchangeClick()
    {
        if (inputField != null)
        {
            string awards_code = inputField.value;
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
            LogicSystem.PublishLogicEvent("ge_exchange_gift", "lobby", awards_code);
        }
    }
}
