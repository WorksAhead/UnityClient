using UnityEngine;

public class BossHealthBar : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    private UnityEngine.GameObject goHealthBar = null;
    void Start()
    {
        try
        {
            positionVec3 = this.transform.localPosition;
            goHealthBar = this.transform.FindChild("HealthBar").gameObject;
            if (null == goHealthBar)
            {
                Debug.LogError("Can Not Find HealthBar");
            }
            SetActive(false);
            //EndShakeHealthBar();
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
            CastAnimation(goHealthBar);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetActive(bool isActive)
    {
        NGUITools.SetActive(this.gameObject, isActive);
    }
    void SetHealthValueText(int current, int max)
    {
        UnityEngine.GameObject go = this.transform.Find("HealthBar/healthValue").gameObject;
        if (null != go)
        {
            UILabel label = go.GetComponent<UILabel>();
            if (null != label)
                label.text = current.ToString() + "/" + max.ToString();
        }
    }

    private void SetName(string name)
    {
        UnityEngine.GameObject go = transform.Find("name").gameObject;
        UILabel label = null;
        if (go != null)
            label = go.GetComponent<UILabel>();
        if (null != label)
        {
            label.text = name.ToString();
        }
    }

    void ShakeHealthBar()
    {

        TweenPosition tweenPosition = this.gameObject.GetComponent<TweenPosition>();
        if (tweenPosition != null)
            Destroy(tweenPosition);
        tweenPosition = this.gameObject.AddComponent<TweenPosition>();
        tweenPosition.from = new UnityEngine.Vector3(positionVec3.x + 25, positionVec3.y + 25, positionVec3.z);
        tweenPosition.to = positionVec3;
        tweenPosition.duration = 0.5f;
    }

    void UpdateHealthBar(int curValue, int maxValue)
    {
        int valueOfLine = maxValue / m_Index;
        if (valueOfLine <= 0)
            return;
        int index = curValue / valueOfLine;
        if (curValue % valueOfLine == 0)
            index--;
        float value = (curValue - index * valueOfLine) / (float)valueOfLine;
        if (curValue <= 0)
            value = 0;
        UIProgressBar progressBar = null;
        progressBar = goHealthBar.GetComponent<UIProgressBar>();
        if (null != progressBar)
        {
            progressBar.value = value;
        }
        UnityEngine.GameObject go = null;
        if (null != goHealthBar && index >= 0)
        {
            UISprite sp = goHealthBar.transform.Find("fore").GetComponent<UISprite>();
            index = index >= m_Index ? 0 : index;
            if (null != sp && sp.color != color[m_Index - 1 - index])
                sp.color = color[m_Index - 1 - index];
        }
        go = transform.Find("itemNum").gameObject;
        UILabel label = null;
        if (go != null)
            label = go.GetComponent<UILabel>();
        if (null != label)
        {
            label.text = " * " + index.ToString();
        }

    }

    void CastAnimation(UnityEngine.GameObject father)
    {
        UnityEngine.GameObject goBack = null;
        UIProgressBar progressBar = null;
        if (null != father)
        {
            goBack = father.transform.Find("forDel").gameObject;
            progressBar = father.GetComponent<UIProgressBar>();
        }

        UISprite spBack = null;
        if (null != goBack)
        {
            spBack = goBack.GetComponent<UISprite>();

        }

        if (null != spBack && null != progressBar)
        {
            if (spBack.fillAmount <= progressBar.value)
            {
                spBack.fillAmount = progressBar.value;
            }
            else
            {
                spBack.fillAmount -= RealTime.deltaTime * 0.5f;
            }
        }

    }

    public void TestButton()
    {
        if (goHealthBar != null)
        {
            ShakeHealthBar();
            testValue -= testDelta;
            UpdateHealthBar(testValue, 500);
        }
    }
    public int testDelta = 20;
    public int testValue = 500;
    private UnityEngine.Vector3 positionVec3 = new UnityEngine.Vector3();
    //记录当前Boss所剩血条数(应该用参数赋值，这里做测试用)
    private int m_Index = 5;
    //记录血条的颜色(Config)
    public UnityEngine.Color[] color = new UnityEngine.Color[5] { UnityEngine.Color.red, UnityEngine.Color.blue, UnityEngine.Color.gray, UnityEngine.Color.green, UnityEngine.Color.black };
}

