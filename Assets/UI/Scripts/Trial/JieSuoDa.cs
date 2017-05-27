public class JieSuoDa : UnityEngine.MonoBehaviour
{
    public float finishiTime = 0;

    public UILabel lblLv = null;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetLblLv(string txt)
    {
        if (lblLv != null)
        {
            lblLv.text = txt;
        }
    }
}
