using UnityEngine;
using System.Collections;
using ArkCrossEngine;
public class UIJinBiPlayerItem : UnityEngine.MonoBehaviour
{

    public UISprite spHead = null;
    public UILabel lblName = null;
    public UILabel lblMoney = null;
    public UILabel lblDiamond = null;

    private bool canPlayGoldChange = false;
    private float m_tempValue = 0f;
    private float m_SourceValue = 0f;
    private int m_TargetValue = 0;
    private float goldChangeTime = 0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (canPlayGoldChange)
            {
                if ((int)m_SourceValue > m_TargetValue)
                {
                    UpdateValue((int)m_SourceValue);
                    m_SourceValue -= RealTime.deltaTime / goldChangeTime * m_tempValue;
                }
                else
                {
                    m_SourceValue = m_TargetValue;
                    UpdateValue(m_TargetValue);
                    canPlayGoldChange = false;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void InitData(ArkCrossEngine.Teammate info)
    {
        if (spHead != null)
        {
            Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(info.ResId);
            spHead.spriteName = cg.m_PortraitForCell;
        }
        if (lblName != null)
        {
            lblName.text = info.Nick;
        }

        if (lblMoney != null)
        {
            m_SourceValue = info.Money;
            m_tempValue = info.Money;
            lblMoney.text = info.Money.ToString();
        }
        if (lblDiamond != null)
        {
            lblDiamond.text = "";
        }
    }

    public void PlayGoldChange(float time)
    {
        goldChangeTime = time;
        canPlayGoldChange = true;
    }

    private void UpdateValue(int value)
    {
        if (lblMoney != null)
        {
            lblMoney.text = value.ToString();
        }
    }
}
