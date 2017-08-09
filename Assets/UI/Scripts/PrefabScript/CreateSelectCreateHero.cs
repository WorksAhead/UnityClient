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

            if (MainCamera.CameraOriginalPosition == UnityEngine.Vector3.zero)
            {
                MainCamera.CameraOriginalPosition = UnityEngine.Camera.main.transform.position;
                MainCamera.CameraOriginalRotation = UnityEngine.Camera.main.transform.rotation;
            }
            else
            {
                UnityEngine.Camera.main.transform.position = MainCamera.CameraOriginalPosition;
                UnityEngine.Camera.main.transform.rotation = MainCamera.CameraOriginalRotation;
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
