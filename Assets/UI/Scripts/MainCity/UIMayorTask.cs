using UnityEngine;
using System.Collections;

public class UIMayorTask : UnityEngine.MonoBehaviour
{

    public UILabel mayorTaskLabel = null;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMayorTask(string task)
    {
        if (mayorTaskLabel != null)
        {
            mayorTaskLabel.text = task;
        }
    }
}
