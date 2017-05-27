using UnityEngine;
using System.Collections;

public class EvaluateChain : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        try
        {
            time = 0.0f;
            UnityEngine.Transform tf = gameObject.transform.Find("Image0");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    du = us.width;
                }
            }
            //du = /*某种屏幕比例算的结果*/50;
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
            time += RealTime.deltaTime;
            int multiple = (int)System.Math.Round(time / 0.03f);
            switch (multiple)
            {
                case 0:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-5 * du, 0, 0), 0.3f, 2.0f);//
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    break;
                case 1:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-4 * du, 0, 0), 0.7f, 1.5f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-4 * du, 0, 0), 0.3f, 2.0f);//
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    break;
                case 2:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-3 * du, 0, 0), 0.7f, 1.5f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-3 * du, 0, 0), 0.3f, 2.0f);//
                    SetPosAlphaScale(3, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    break;
                case 3:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-2 * du, 0, 0), 0.7f, 1.5f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(-2 * du, 0, 0), 0.3f, 2.0f);//
                    SetPosAlphaScale(4, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    break;
                case 4:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(-du, 0, 0), 0.7f, 1.5f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(-du, 0, 0), 0.3f, 2.0f);//
                    SetPosAlphaScale(5, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    break;
                case 5:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(0, 0, 0), 0.7f, 1.5f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(0, 0, 0), 0.3f, 2.0f);//
                    SetPosAlphaScale(6, new UnityEngine.Vector3(-5 * du, 0, 0), 0.0f, 1.0f);
                    break;
                case 6:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(du, 0, 0), 0.7f, 1.5f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(du, 0, 0), 0.3f, 2.0f); //
                    break;
                case 7:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(2 * du, 0, 0), 0.7f, 1.5f);
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 1.0f);
                    break;
                case 28:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 1.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 1.0f);
                    break;
                case 29:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 1.0f, 2.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 1.0f);
                    break;
                case 30:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 1.0f, 2.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 1.0f);
                    break;
                case 31:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 1.0f, 2.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 1.0f);
                    break;
                case 32:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 1.0f, 2.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 1.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 1.0f);
                    break;
                case 33:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 1.0f, 2.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 1.0f);
                    break;
                case 34:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 1.0f, 2.0f);
                    break;
                case 35:
                    SetPosAlphaScale(0, new UnityEngine.Vector3(-3 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(1, new UnityEngine.Vector3(-2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(2, new UnityEngine.Vector3(-du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(3, new UnityEngine.Vector3(0, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(4, new UnityEngine.Vector3(du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(5, new UnityEngine.Vector3(2 * du, 0, 0), 0.0f, 2.0f);
                    SetPosAlphaScale(6, new UnityEngine.Vector3(3 * du, 0, 0), 0.0f, 2.0f);
                    break;
                default: break;
            }
            if (time > 1.1f)
            {
                NGUITools.Destroy(gameObject);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void SetPosAlphaScale(int imagenum, UnityEngine.Vector3 pos, float alpha, float wantscale)
    {
        string widgetname = "Image" + imagenum;
        UnityEngine.Transform tf = this.gameObject.transform.Find(widgetname);
        if (tf != null)
        {
            tf.localPosition = pos;
            UISprite us = tf.GetComponent<UISprite>();
            float scale = wantscale / nowscale[imagenum];
            nowscale[imagenum] = wantscale;
            if (us != null)
            {
                us.alpha = alpha;
                us.width = (int)System.Math.Round(us.width * scale);
                us.height = (int)System.Math.Round(us.height * scale);
            }
        }
    }
    private int du = 0;//数据单位
    private float time = 0.0f;
    private float[] nowscale = { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
}
