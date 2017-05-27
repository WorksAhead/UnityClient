using UnityEngine;
using System.Collections;

public class DamageForCutHero : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        try
        {
            livetime = 0.0f;
            oldfontscale = this.GetComponent<UILabel>().transform.localScale;
            if (signforinitpos)
            {
                signforinitpos = false;
                oldpos = this.transform.localPosition;
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
        try
        {
            if (signforinitpos)
            {
                signforinitpos = false;
                oldpos = this.transform.localPosition;
            }
            livetime += RealTime.deltaTime;

            if (livetime <= 0.16f)
            {
                float scale = livetime * 3;
                this.GetComponent<UILabel>().transform.localScale = new UnityEngine.Vector3(oldfontscale.x + scale, oldfontscale.y + scale, oldfontscale.z);
                this.transform.localPosition = new UnityEngine.Vector3(oldpos.x, oldpos.y - ((livetime - 0.14f) * 200), oldpos.z);
            }
            if (livetime > 0.16 && livetime <= 0.66)
            {
                this.transform.localPosition = new UnityEngine.Vector3(oldpos.x, oldpos.y + ((livetime - 0.66f) * 5), oldpos.z);
            }
            if (livetime > 0.66f)
            {
                this.GetComponent<UILabel>().alpha = 1.0f - (livetime - 0.66f) / 0.2f;
                this.transform.localPosition = new UnityEngine.Vector3(oldpos.x, oldpos.y - ((livetime - 0.66f) * 200));
            }
            if (livetime > 0.86f)
            {
                this.GetComponent<UILabel>().transform.localScale = oldfontscale;
                this.GetComponent<UILabel>().alpha = 1.0f;
                ArkCrossEngine.GameObject _gameobject = ArkCrossEngine.ObjectFactory.Create<ArkCrossEngine.GameObject>(gameObject);// new ArkCrossEngine.GameObject(gameObject);
                ArkCrossEngine.ResourceSystem.RecycleObject(_gameobject);
                livetime = 0.0f;
                signforinitpos = true;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private float livetime = 0.0f;
    //private int oldfontsize = 0;
    private UnityEngine.Vector3 oldpos;
    private bool signforinitpos = true;
    private UnityEngine.Vector3 oldfontscale;
}
