using UnityEngine;

public class MasterMonsterHealthBar : UnityEngine.MonoBehaviour
{

    private float countdown = -1;
    public float waitTime = 1f;
    public int testDelta = 20;
    public int testValue = 500;
    private UnityEngine.Vector3 positionVec3 = new UnityEngine.Vector3();
    //记录当前Boss所剩血条数(应该用参数赋值，这里做测试用)
    private int m_Index = 5;
    //记录血条的颜色(Config)
    public UnityEngine.Color[] color = new UnityEngine.Color[] { };
    public UILabel healthVal = null;
    public UILabel nameValue = null;
    public UILabel itemNum = null;
    public UISprite forDel = null;
    public UISprite spFore = null;
    public UISprite spBack1 = null;
    public UISprite spBack2 = null;
    public UIProgressBar progressBar = null;
    public UnityEngine.Transform healthBarView = null;

    private UnityEngine.Vector3 viewStartPos = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 viewCurPos = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 tempVec = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 outPos = new UnityEngine.Vector3(-1000f, -1000f, -1000f);
    // Use this for initialization
    void Start()
    {
        try
        {
            if (healthBarView != null)
            {
                positionVec3 = healthBarView.localPosition;
                viewStartPos = positionVec3;
                viewCurPos = viewStartPos;
                if (null == progressBar)
                {
                    Debug.LogError("Can Not Find HealthBar");
                }
                SetActive(false);
            }
            /*UnityEngine.Transform trans = this.transform.FindChild("HealthBar");
            if(trans != null)
              goHealthBar = trans.gameObject;
              */
            //EndShakeHealthBar();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            CastAnimation();
            if (countdown >= 0)
            {
                countdown -= RealTime.deltaTime;
                if (countdown <= 0)
                    SetActive(false);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetActive(bool isActive)
    {
        /*
		if (healthBarView!=null){
	      NGUITools.SetActive(healthBarView, isActive);
		}*/
        if (isActive)
        {
            ActiveView(healthBarView, viewCurPos);
        }
        else
        {
            ActiveView(healthBarView, outPos);
        }
    }

    private void ActiveView(UnityEngine.Transform tf, UnityEngine.Vector3 pos)
    {
        if (null != tf)
        {
            tf.localPosition = pos;
        }
    }

    void SetHealthValueText(int current, int max)
    {
        /*UnityEngine.Transform trans = healthBarView.transform.Find("HealthBar/healthValue");
        if(null == trans)
          return;
        UnityEngine.GameObject go = trans.gameObject;
        if (null != go) {
          UILabel label = go.GetComponent<UILabel>();
          if (null != label)
            label.text = current.ToString() + "/" + max.ToString();
        }*/
        if (null != healthVal)
        {
            SetActive(true);
            healthVal.text = current.ToString() + "/" + max.ToString();
        }
    }

    private void SetName(string name)
    {
        /*UnityEngine.Transform trans = transform.Find("name");
        if(trans ==null )
          return;
        UnityEngine.GameObject go = trans.gameObject;
        UILabel label = null;
        if (go != null)
          label = go.GetComponent<UILabel>();
        if (null != label) {
          label.text = name.ToString();
        }*/
        if (null != nameValue)
        {
            nameValue.text = name.ToString();
        }
    }
    void ShakeHealthBar()
    {
        if (null != healthBarView)
        {
            return;
        }
        tempVec.x = positionVec3.x + 25f;
        tempVec.y = positionVec3.y + 25f;
        tempVec.z = positionVec3.z;
        //TweenPosition.Begin(healthBarView.gameObject, 0.3f, tempVec, positionVec3);
        viewCurPos = positionVec3;
        /*
    TweenPosition tweenPosition = healthBarView.GetComponent<TweenPosition>();
    if (tweenPosition != null) { 
      //Debug.Log("Destory tweenPos");
      Destroy(tweenPosition);
    }
    TweenPosition tween = healthBarView.AddComponent<TweenPosition>();
    if (null != tween) {
      tween.from = new UnityEngine.Vector3(positionVec3.x + 25, positionVec3.y + 25, positionVec3.z);
      tween.to = positionVec3;
      tween.duration = 0.3f;
    }*/
    }

    void UpdateHealthBar(int curValue, int maxValue)
    {
        SetActive(true);
        ShakeHealthBar();
        SetHealthValueText(curValue, maxValue);
        countdown = waitTime;
        int valueOfLine = maxValue / m_Index;
        if (valueOfLine <= 0)
            return;
        int index = curValue / valueOfLine;
        if (curValue % valueOfLine == 0)
            index--;
        float value = (curValue - index * valueOfLine) / (float)valueOfLine;
        if (curValue <= 0)
            value = 0;
        //UIProgressBar progressBar = null;
        //progressBar = goHealthBar.GetComponent<UIProgressBar>();
        if (null != progressBar)
        {
            progressBar.value = value;
        }
        //UnityEngine.GameObject go = null;
        if (index >= 0)
        {
            /*UnityEngine.Transform trans = goHealthBar.transform.Find("fore");
            UISprite spFore = null;
            if(null != trans)
              spFore = trans.gameObject.GetComponent<UISprite>();
            trans = goHealthBar.transform.Find("back");
            UISprite spBack = null;
            if (null != trans)
              spBack = trans.gameObject.GetComponent<UISprite>();
            */
            index = index >= m_Index ? 0 : index;
            if (null != spFore)
            {
                spFore.spriteName = "blood" + (m_Index - index).ToString();
            }
            if (null != spBack1 && null != spBack2)
            {
                if (index <= 0)
                {
                    spBack1.spriteName = "back";
                    spBack2.spriteName = "back";
                }
                else
                {
                    spBack1.spriteName = "blood" + (m_Index - index + 1).ToString();
                    spBack2.spriteName = "blood" + (m_Index - index + 1).ToString();
                }
            }
        }
        /*UnityEngine.Transform trs =  transform.Find("itemNum");
        if(trs!=null)
          go = trs.gameObject;
        UILabel label = null;
        if (go != null)
          label = go.GetComponent<UILabel>();
        */
        if (null != itemNum)
        {
            if (curValue <= 0)
                itemNum.text = "Dead";
            else
            {
                itemNum.text = " * " + index.ToString();
            }
        }
    }

    void CastAnimation()
    {
        /*UnityEngine.Transform trans = null;
        UnityEngine.GameObject goBack = null;
        UIProgressBar progressBar = null;
        if (null != father) {
          trans = father.transform.Find("forDel");
          if(trans!= null)
            goBack = trans.gameObject;
          progressBar = father.GetComponent<UIProgressBar>();
        }

        UISprite spBack = null;
        if (null != goBack) {
          spBack = goBack.GetComponent<UISprite>();

        }
        */
        if (null != forDel && null != progressBar)
        {
            if (forDel.fillAmount <= progressBar.value)
            {
                forDel.fillAmount = progressBar.value;
            }
            else
            {
                forDel.fillAmount -= RealTime.deltaTime * 0.5f;
            }
        }

    }

    public void TestButton()
    {
        if (progressBar != null)
        {
            //ShakeHealthBar();
            testValue -= testDelta;
            //Debug.Log(testValue);
            UpdateHealthBar(testValue, 500);
        }
    }
}
