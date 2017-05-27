using UnityEngine;
using System.Collections;

public class DamageForAddHero : UnityEngine.MonoBehaviour
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

            if (livetime < 0.1f)
            {
                float scale = livetime * 3;
                this.GetComponent<UILabel>().transform.localScale = new UnityEngine.Vector3(oldfontscale.x + scale, oldfontscale.y + scale, oldfontscale.z);
                this.transform.localPosition = new UnityEngine.Vector3(oldpos.x, oldpos.y + ((livetime) * 200), oldpos.z);
            }
            if (livetime > 1.0f)
            {
                this.GetComponent<UILabel>().alpha = 1.0f - (livetime - 1.0f) / 0.25f;
            }
            if (livetime > 1.25f)
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
    private UnityEngine.Vector3 oldpos;
    private bool signforinitpos = true;
    private UnityEngine.Vector3 oldfontscale;
}
