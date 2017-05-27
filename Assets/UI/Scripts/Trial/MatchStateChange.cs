using UnityEngine;
using System.Collections;

public class MatchStateChange : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject matching = null;

    void Start()
    {

    }


    void Update()
    {

    }

    public void SetState(bool isShow)
    {
        if (matching != null)
        {
            NGUITools.SetActive(matching, isShow);
        }
    }
}
