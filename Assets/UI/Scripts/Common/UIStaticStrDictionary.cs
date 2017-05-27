public class UIStaticStrDictionary : UnityEngine.MonoBehaviour
{

    public int strId = -1;
    // Use this for initialization
    void Start()
    {
        try
        {
            UILabel lbl = GetComponent<UILabel>();
            lbl.text = strId == -1 ? "" : ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(strId);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
