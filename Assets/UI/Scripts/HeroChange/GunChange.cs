public class GunChange : UnityEngine.MonoBehaviour
{
    void Start()
    {
        try
        {
            NGUITools.SetActive(this.gameObject, false);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Update()
    {
    }
    void OnClick()
    {
    }
    public void SetActive(bool active)
    {
        NGUITools.SetActive(this.gameObject, active);
    }
}
