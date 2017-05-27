public enum ButtonState
{
    Openned = 0,
    Finished = 1,
    UnLock = 2,
    Lock = 3
}
public class UITreasureButton : UnityEngine.MonoBehaviour
{

    public UISprite spNormal = null;
    public UnityEngine.GameObject goLock = null;
    public UnityEngine.GameObject goBaoxiang = null;
    public UnityEngine.GameObject goBaoxiangOpen = null;
    public UnityEngine.GameObject goNumber = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetTreasureButtonState(ButtonState state)
    {
        if (spNormal == null || goLock == null || goBaoxiang == null || goBaoxiangOpen == null || goNumber == null)
            return;
        switch (state)
        {
            case ButtonState.Openned:
                NGUITools.SetActive(goBaoxiangOpen, true);
                NGUITools.SetActive(goBaoxiang, false);
                NGUITools.SetActive(goNumber, false);
                NGUITools.SetActive(goLock, false);
                spNormal.enabled = false;
                SetButtonEnable(false);
                break;
            case ButtonState.Finished:
                NGUITools.SetActive(goBaoxiang, true);
                NGUITools.SetActive(goNumber, false);
                NGUITools.SetActive(goLock, false);
                spNormal.enabled = false;
                break;
            case ButtonState.UnLock:
                NGUITools.SetActive(goLock, false);
                NGUITools.SetActive(goBaoxiang, false);
                NGUITools.SetActive(goBaoxiangOpen, false);
                NGUITools.SetActive(goNumber, true);
                spNormal.enabled = true;
                SetButtonEnable(true);
                break;
            case ButtonState.Lock:
                NGUITools.SetActive(goBaoxiang, false);
                NGUITools.SetActive(goBaoxiangOpen, false);
                NGUITools.SetActive(goLock, true);
                NGUITools.SetActive(goNumber, true);
                spNormal.enabled = true;
                SetButtonEnable(false);
                break;
        }
    }
    public void SetButtonEnable(bool enable)
    {
        UIButton uiBtn = this.GetComponent<UIButton>();
        if (uiBtn != null) uiBtn.enabled = enable;
        UnityEngine.BoxCollider box = this.GetComponent<UnityEngine.BoxCollider>();
        if (box != null) box.enabled = enable;
    }
    public void SetLabelNumber(int index)
    {
        if (goNumber != null)
        {
            UILabel lbl = goNumber.GetComponent<UILabel>();
            if (lbl != null) lbl.text = index.ToString();
        }
    }

}
