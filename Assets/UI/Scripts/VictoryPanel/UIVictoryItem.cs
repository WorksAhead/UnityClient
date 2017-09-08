using UnityEngine;
using System.Collections;

public class UIVictoryItem : UnityEngine.MonoBehaviour
{

    public UILabel lblValue = null;
    public int RealValue = 0;
    private float m_MinValue = 0f;
    private UIItemType m_ItemType = UIItemType.Common;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frameaw
    void Update()
    {
        try
        {
            if ((int)m_MinValue <= RealValue)
            {
                m_MinValue += RealTime.deltaTime * 50;
                if (m_MinValue > RealValue)
                {
                    m_MinValue = RealValue;
                }
                UpdateValue((int)m_MinValue);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void SetValue(int value, UIItemType itemType)
    {
        RealValue = value;
        m_ItemType = itemType;
    }
    public void UpdateValue(int value)
    {
        if (lblValue != null)
        {
            if (m_ItemType == UIItemType.Common)
            {
                lblValue.text = "[ffee00]" + value.ToString() + "[-]";
            }
            else
            {
                int sec = value % 60;
                int minute = value / 60;
                lblValue.text = "[ffee00]" + minute + "'  " + sec.ToString("D2") + "\"[-]";
            }
        }
    }
    public void SetItemName(string text)
    {
        UILabel lbl = this.GetComponent<UILabel>();
        if (null != lbl) lbl.text = text + "[-]";
    }
}
