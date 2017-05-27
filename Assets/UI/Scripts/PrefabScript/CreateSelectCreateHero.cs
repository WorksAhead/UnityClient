using UnityEngine;
using System.Collections;
using ArkCrossEngine;
public class CreateSelectCreateHero : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        try
        {
            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource("UI/SelectCreateHero"));
            if (go != null)
            {
                Instantiate(go);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
