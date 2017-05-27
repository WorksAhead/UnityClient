using UnityEngine;

public enum UIScreenTipPosEnum
{
    AlignNone,
    AlignTop,
    AlignCenter,
    AlignBottom,
}
public class UIScreenTip : UnityEngine.MonoBehaviour
{
    public float AlignTop = 0.67f;
    public float AlignCenter = 0.5f;
    public float AlignBottom = 0.33f;
    public UILabel lblTips = null;

    public void ShowScreenTip(string tips, UIScreenTipPosEnum posType, UnityEngine.Vector3 posVec)
    {
        if (lblTips != null) lblTips.text = tips;
        if (posType == UIScreenTipPosEnum.AlignNone)
        {
            transform.position = posVec;
        }
        else
        {
            transform.position = GetWorldPosition(posType);
        }
        BloodAnimationScript bas = this.GetComponent<BloodAnimationScript>();
        if (bas != null)
        {
            bas.PlayAnimation();
        }
        //NGUITools.SetActive(this.gameObject, true);
    }
    private UnityEngine.Vector3 GetWorldPosition(UIScreenTipPosEnum posType)
    {
        int width = Screen.width;
        int height = Screen.height;
        UnityEngine.Vector3 screen_pos = new UnityEngine.Vector3();
        screen_pos.x = width / 2f;
        screen_pos.z = 0;
        switch (posType)
        {
            case UIScreenTipPosEnum.AlignTop:
                screen_pos.y = height * AlignTop; break;
            case UIScreenTipPosEnum.AlignCenter:
                screen_pos.y = height * AlignCenter; break;
            case UIScreenTipPosEnum.AlignBottom:
                screen_pos.y = height * AlignBottom; break;
            default: screen_pos.y = height / 2f; break;
        }
        UnityEngine.Vector3 world_pos = UICamera.mainCamera.ScreenToWorldPoint(screen_pos);
        return world_pos;
    }
}
