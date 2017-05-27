public class UICloseButton : UnityEngine.MonoBehaviour
{

    public string HideWindowName;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnClick()
    {
        //UIManager.Instance.HideWindowByName(HideWindowName);
        UnityEngine.GameObject window = UIManager.Instance.GetWindowGoByName(HideWindowName);
        if (window != null)
        {
            OpenAndCloseUi oac = window.GetComponentInChildren<OpenAndCloseUi>();
            if (oac != null)
            {
                oac.OnCloseUI(HideWindowName);
            }
            else
            {
                UIManager.Instance.HideWindowByName(HideWindowName);
            }
        }
    }
}
