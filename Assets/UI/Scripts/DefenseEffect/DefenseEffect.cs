public class DefenseEffect : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject goTweenContainer = null;// 容器

    //动画的两个字
    public UISprite character1 = null;
    public UISprite character2 = null;
    public UILabel fangyuLabel = null;
    public UILabel tuxiLabel = null;
    public UnityEngine.GameObject tweenContainer = null;
    public UnityEngine.GameObject pos = null;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 往右上移动
    void TweenRightLeft()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        UnityEngine.Transform pos0 = go.transform.Find("UIPanel_3/UIAnchor-TopRight/PveFightInfo/TimeOrSome/Sprite/pos");

        TweenPosition tp = tweenContainer.GetComponent<TweenPosition>();
        if (null != tp)
        {
            pos.transform.position = pos0.position;
            tp.to = pos.transform.localPosition;
        }
    }

    //初始化，章节 type = 0,1,2,3(被击，防御，挑战，突袭) 
    public void InitType(int type)
    {
        NGUITools.SetActive(fangyuLabel.gameObject, false);
        NGUITools.SetActive(tuxiLabel.gameObject, false);
        switch (type)
        {
            case 0:
                character1.spriteName = "zd_fsan1";
                character2.spriteName = "zd_fsan2";
                break;
            case 1:
                character1.spriteName = "zd_fsan1";
                character2.spriteName = "zd_fsan2";
                NGUITools.SetActive(fangyuLabel.gameObject, true);
                break;
            case 2:
                character1.spriteName = "zd_fsan1";
                character2.spriteName = "zd_fsan2";
                break;
            case 3:
                character1.spriteName = "zd_txan1";
                character2.spriteName = "zd_txan2";
                NGUITools.SetActive(tuxiLabel.gameObject, true);
                break;
        }
        TweenRightLeft();
    }
    public void DestroyEffect()
    {
        Destroy(this.gameObject);
    }
}
