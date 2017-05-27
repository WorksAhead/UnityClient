using ArkCrossEngine;
public class LoadingNotice : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 更换提示语
    void NoticeChange()
    {
        int num = UnityEngine.Random.Range(650, 660);
        noticeTf.text = StrDictionaryProvider.Instance.GetDictString(num);
    }
    private float noticeTime = 0f;
    public UILabel noticeTf;
    public int maxNotice = 10;
    public float noticeChange = 0.3f;
}
