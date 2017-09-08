public class YesOrNot : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Yes()
    {
        try
        {
            if (doSomething != null)
            {
                doSomething(true);
            }
            doSomething = null;
            NGUITools.DestroyImmediate(gameObject);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void Not()
    {
        try
        {
            if (doSomething != null)
            {
                doSomething(false);
            }
            doSomething = null;
            NGUITools.DestroyImmediate(gameObject);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void SetMessageAndDO(string message, string button, System.Action<bool> dofunction)
    {
        try
        {
            UnityEngine.Transform tf = gameObject.transform.Find("Sprite/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null && message != null)
                {
                    ul.text = message;
                }
            }
            tf = gameObject.transform.Find("Sprite/YES/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null && button != null)
                {
                    ul.text = button;
                }
            }
            doSomething = dofunction;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private System.Action<bool> doSomething = null;
}
