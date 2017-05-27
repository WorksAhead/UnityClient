using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class XHunItem : UnityEngine.MonoBehaviour
{
    [HideInInspector]
    public bool isSelect = false;

    public UILabel lblName = null;
    public UILabel lblValue = null;
    public UITexture texture = null;

    private int currentId = 0;

    public void OnHunChanged()
    {
        UIToggle toggle = transform.gameObject.GetComponent<UIToggle>();
        if (toggle != null)
        {
            //修改状态
            isSelect = toggle.value;
            //如果是选中状态，则更新按钮状态
            if (isSelect == true)
            {
                RightInjectContainer right = transform.parent.parent.GetComponent<RightInjectContainer>();
                if (right != null)
                {
                    right.UpdateInjectButtonState();
                }
            }
        }
    }

    public void UpdateView(int id)
    {
        if (currentId == id)
        {
            return;
        }
        currentId = id;
        ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(id);
        if (itemConfig != null)
        {
            if (lblName != null)
            {
                lblName.text = itemConfig.m_ItemName;
            }
            if (lblValue != null)
            {
                lblValue.text = itemConfig.m_ExperienceProvide.ToString();
            }
        }
    }
}
