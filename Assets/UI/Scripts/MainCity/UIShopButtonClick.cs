
using UnityEngine;
using System.Collections;

public class UIShopButtonClick : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("GoldBuy");
        if (go != null)
        {
            if (NGUITools.GetActive(go))
            {
                UIManager.Instance.HideWindowByName("GoldBuy");
            }
            else
            {
                UIManager.Instance.ShowWindowByName("GoldBuy");
            }
        }
    }
}
