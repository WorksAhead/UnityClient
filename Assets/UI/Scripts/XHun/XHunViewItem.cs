using UnityEngine;
using System.Collections;

public class XHunViewItem : UnityEngine.MonoBehaviour
{
    [HideInInspector]
    public int lv = -1;

    public UIToggle toggle = null;

    public OnToggleChange onToggleChange;
    public delegate void OnToggleChange(int level);
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnChange()
    {
        if (toggle.value == true)
        {
            if (onToggleChange != null)
            {
                onToggleChange(lv);
            }
        }
    }
}
