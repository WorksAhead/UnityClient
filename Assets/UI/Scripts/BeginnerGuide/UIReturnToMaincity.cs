using UnityEngine;
using ArkCrossEngine;
using System.Collections;

public class UIReturnToMaincity : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnClick()
    {
        LogicSystem.SendStoryMessage("returntomaincity");

    }
    public int offset = 10;
}
