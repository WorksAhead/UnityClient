using ArkCrossEngine;

public class UIGuideDlg : UnityEngine.MonoBehaviour
{

    public UILabel lblDesc = null;
    public UILabel lblLeftDesc;
    public UILabel lblRightDesc;
    public UnityEngine.GameObject goLeftDialog;
    public UnityEngine.GameObject goRightDialog;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //用于新手关引导对话
    public void SetDescription(int descId)
    {
        string chn_des = StrDictionaryProvider.Instance.GetDictString(descId);
        if (lblDesc != null)
            lblDesc.text = chn_des;
    }
    public void SetDescription(string desc, bool isLeft)
    {
        if (lblDesc != null) lblDesc.text = desc;
        if (isLeft)
        {
            if (lblLeftDesc != null) lblLeftDesc.text = desc;
        }
        else
        {
            if (lblRightDesc != null) lblRightDesc.text = desc;
        }
        if (goLeftDialog != null) NGUITools.SetActive(goLeftDialog, isLeft);
        if (goRightDialog != null) NGUITools.SetActive(goRightDialog, !isLeft);
    }
}
