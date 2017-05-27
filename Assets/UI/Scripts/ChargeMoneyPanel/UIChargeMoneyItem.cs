using UnityEngine;

public class UIChargeMoneyItem : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    //商品Id
    private int m_GoodsId = -1;
    //商品图标、信息由商品Id获得
    public UISprite goodsIcon = null;
    public UILabel chargeInfo = null;
    public UILabel goodsName = null;
    public UILabel goodsPrice = null;
    public UILabel diamondAmount = null;

    //购买按钮
    public UnityEngine.GameObject purchaseBtnGo = null;
    void Start()
    {
        try
        {
            if (purchaseBtnGo != null)
            {
                UIEventListener.Get(purchaseBtnGo).onClick = OnPurchaseClick;
            }
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

    public void OnPurchaseClick(UnityEngine.GameObject go)
    {
        Debug.Log("点击购买物品！！");
    }

    public int GoodsId
    {
        get { return m_GoodsId; }
        set { m_GoodsId = value; }
    }
    public void InitItem()
    {
        //设置商品信息
        //     goodsIcon.spriteName = "";
        //     chargeInfo.text = "";
        //     goodsName.text = "";
        //     goodsPrice.text = "";
        //     diamondAmount.text = "";
    }


}
