using UnityEngine;

public class UIChargeMoney : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject item = null;
    public UnityEngine.GameObject gridGo = null;
    // Use this for initialization
    void Start()
    {
        try
        {
            InitChargeMoneyScrollBar();
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
    //读取数据库中的数据初始化充值选项
    public void InitChargeMoneyScrollBar()
    {
        if (item == null || gridGo == null)
        {
            Debug.LogError("!!Did not initialize Item or fatherGo.");
            return;
        }
        for (int index = 0; index < 5; index++)
        {
            NGUITools.AddChild(gridGo, item);
        }
        UIGrid grid = gridGo.GetComponent<UIGrid>();
        if (null != grid)
            grid.Reposition();
    }


}
