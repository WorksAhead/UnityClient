public class ChainBeat : UnityEngine.MonoBehaviour
{
    private UnityEngine.Vector3 chainBeatViewPos = UnityEngine.Vector3.zero;
    public UnityEngine.Transform chainBeatView = null;

    public UnityEngine.Transform chainBest = null;
    public UnityEngine.Transform chain = null;

    public const int c_EvaluateImages = 3;
    private UnityEngine.Vector3 evaluateImagesPos = UnityEngine.Vector3.zero;
    public UnityEngine.Transform[] evaluateImages = new UnityEngine.Transform[c_EvaluateImages];

    public const int c_Number = 10;

    private UnityEngine.Vector3 oneNumbersPos = UnityEngine.Vector3.zero;
    public UnityEngine.Transform[] oneNumbers = new UnityEngine.Transform[c_Number];

    private UnityEngine.Vector3 twoNumbersPos = UnityEngine.Vector3.zero;
    public UnityEngine.Transform[] twoNumbers = new UnityEngine.Transform[c_Number];

    //public const int c_CloenEffects = 4;
    //private UnityEngine.Transform[] cloenEffects = new UnityEngine.Transform[c_CloenEffects];
    //private UnityEngine.Vector3 cloenEffectsPos = new UnityEngine.Vector3(-175f, -70f, 0f);
    //private UnityEngine.Vector3 cloenEffectsScale = new UnityEngine.Vector3(320f, 320f, 320f);

    //public UnityEngine.GameObject cloenEffect1 = null;
    //public UnityEngine.GameObject cloenEffect2 = null;
    //public UnityEngine.GameObject cloenEffect3 = null;
    //public UnityEngine.GameObject cloenEffect4 = null;

    private UnityEngine.Vector3 outPos = new UnityEngine.Vector3(-1000f, -1000f, -1000f);

    private UnityEngine.Vector3 tempVec = UnityEngine.Vector3.zero;

    // Use this for initialization
    void Start()
    {
        try
        {
            if (null != chainBeatView)
            {
                chainBeatViewPos = chainBeatView.localPosition;
                evaluateImagesPos = evaluateImages[0].localPosition;
                oneNumbersPos = oneNumbers[0].localPosition;
                twoNumbersPos = twoNumbers[0].localPosition;
                time = 0.0f;
                SetActive(false);
                //UnityEngine.Transform tf = transform.Find("UIPanel_3/UIAnchor-Center/ChainBeat");
                //cloenEffect1 = NGUITools.AddChild(chainBeatView.gameObject, cloenEffect1);
                //cloenEffects[0] = cloenEffect1.transform;
                //cloenEffects[0].localPosition = cloenEffectsPos;
                //cloenEffects[0].localScale = cloenEffectsScale;

                //cloenEffect2 = NGUITools.AddChild(chainBeatView.gameObject, cloenEffect2);
                //cloenEffects[1] = cloenEffect2.transform;
                //cloenEffects[1].localPosition = cloenEffectsPos;
                //cloenEffects[1].localScale = cloenEffectsScale;

                //cloenEffect3 = NGUITools.AddChild(chainBeatView.gameObject, cloenEffect3);
                //cloenEffects[2] = cloenEffect3.transform;
                //cloenEffects[2].localPosition = cloenEffectsPos;
                //cloenEffects[2].localScale = cloenEffectsScale;

                //cloenEffect4 = NGUITools.AddChild(chainBeatView.gameObject, cloenEffect4);
                //cloenEffects[3] = cloenEffect4.transform;
                //cloenEffects[3].localPosition = cloenEffectsPos;
                //cloenEffects[3].localScale = cloenEffectsScale;
            }
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
            time += RealTime.deltaTime;
            int multiple = (int)System.Math.Round(time / 0.03f);
            switch (multiple)
            {
                case 0: Zoom(1.4f); break;
                case 1: Zoom(1.8f); break;
                case 2: Zoom(1.4f); break;
                case 3: Zoom(1.2f); break;
                case 4: Zoom(1.0f); break;
                default: break;
            }
            if (time > 0.12f)
            {
                Zoom(1.0f);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Zoom(float wantscale)
    {
        tempVec.x = wantscale;
        tempVec.y = wantscale;
        tempVec.z = wantscale;
        if (chain != null)
        {
            chain.localScale = tempVec;
        }
        ScaleHitCount(tempVec);
    }

    public void SetInitTime()
    {
        time = 0.0f;
    }

    public void SetActive(bool val)
    {
        if (val)
        {
            ActiveView(chainBeatView, chainBeatViewPos);
        }
        else
        {
            ActiveView(chainBeatView, outPos);
        }
    }

    private void ActiveView(UnityEngine.Transform tf, UnityEngine.Vector3 pos)
    {
        if (null != tf)
        {
            tf.localPosition = pos;
        }
    }
    private float time = 0.0f;

    // 已连击次数 显示特效
    //public void ShowEffect(int beatnum)
    //{
    //  if (beatnum <= 0) {
    //    ActiveView(cloenEffects[0], outPos);
    //    ActiveView(cloenEffects[1], outPos);
    //    ActiveView(cloenEffects[2], outPos);
    //    ActiveView(cloenEffects[3], outPos);
    //  } else if (beatnum <= 99) {
    //    ActiveView(cloenEffects[0], cloenEffectsPos);
    //    ActiveView(cloenEffects[1], outPos);
    //    ActiveView(cloenEffects[2], outPos);
    //    ActiveView(cloenEffects[3], outPos);
    //  } else if (beatnum <= 199) {
    //    ActiveView(cloenEffects[0], outPos);
    //    ActiveView(cloenEffects[1], cloenEffectsPos);
    //    ActiveView(cloenEffects[2], outPos);
    //    ActiveView(cloenEffects[3], outPos);
    //  } else if (beatnum <= 299) {
    //    ActiveView(cloenEffects[0], outPos);
    //    ActiveView(cloenEffects[1], outPos);
    //    ActiveView(cloenEffects[2], cloenEffectsPos);
    //    ActiveView(cloenEffects[3], outPos);
    //  } else {
    //    ActiveView(cloenEffects[0], outPos);
    //    ActiveView(cloenEffects[1], outPos);
    //    ActiveView(cloenEffects[2], outPos);
    //    ActiveView(cloenEffects[3], cloenEffectsPos);
    //  }
    //}


    public void ShowEvaluateImage(int beatnum)
    {
        if (beatnum <= 33)
        {
            ActiveView(evaluateImages[0], evaluateImagesPos);
            ActiveView(evaluateImages[1], outPos);
            ActiveView(evaluateImages[2], outPos);
        }
        else if (beatnum <= 66)
        {
            ActiveView(evaluateImages[0], outPos);
            ActiveView(evaluateImages[1], evaluateImagesPos);
            ActiveView(evaluateImages[2], outPos);
        }
        else
        {
            ActiveView(evaluateImages[0], outPos);
            ActiveView(evaluateImages[1], outPos);
            ActiveView(evaluateImages[2], evaluateImagesPos);
        }
    }

    void ShowHitCount(int number)
    {
        int oneVal = number % 10;
        int twoVal = (number - oneVal) / 10;
        for (int i = 0; i < c_Number; i++)
        {
            if (i == oneVal)
            {
                ActiveView(oneNumbers[i], oneNumbersPos);
            }
            else
            {
                ActiveView(oneNumbers[i], outPos);
            }
            if (i == twoVal)
            {
                ActiveView(twoNumbers[i], twoNumbersPos);
            }
            else
            {
                ActiveView(twoNumbers[i], outPos);
            }
        }
    }

    void ScaleHitCount(UnityEngine.Vector3 scale)
    {
        for (int i = 0; i < c_Number; i++)
        {
            if (oneNumbers[i].localPosition == oneNumbersPos)
            {
                oneNumbers[i].localScale = scale;
            }
            if (twoNumbers[i].localPosition == twoNumbersPos)
            {
                twoNumbers[i].localScale = scale;
            }
        }
    }

    public void UpdateHitCount(int number)
    {
        //ShowEffect(number);
        if (number <= 0)
        {
            SetActive(false);
        }
        else
        {
            SetActive(true);
            SetInitTime();
            int num = System.Math.Abs(number) % 100;
            if (num < 0)
            {
                num = 0;
            }
            ShowEvaluateImage(num);
            ShowHitCount(num);
        }
    }
}
