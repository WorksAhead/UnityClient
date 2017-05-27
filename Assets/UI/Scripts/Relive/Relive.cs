using UnityEngine;
using System.Collections;

public class Relive : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBtnClick()
    {
        //发送复活消息
        ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_player_relive", "player");
        NGUITools.Destroy(this.gameObject);
    }

}
