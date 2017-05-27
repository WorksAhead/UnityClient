using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class TrialUnit : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject goLock = null;
    public UILabel lblName = null;
    public UILabel lblLock = null;
    public UILabel lblTime = null;

    public UnityEngine.Color colorNameLock;
    public UnityEngine.Color colorNameUnLock;

    public UnityEngine.Color colorTimeLock;
    public UnityEngine.Color colorTimeUnLock;

    private bool hasOpen = false;
    private int openLv;

    private UnityEngine.GameObject unLockEffect = null;

    public bool HasOpen
    {
        get
        {
            return hasOpen;
        }
    }

    public int OpenLv
    {
        get
        {
            return openLv;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void UpdateData(string name, bool open, string openTime, int lv)
    {
        if (lblName != null)
        {
            lblName.text = name;
        }

        UpdateOpen(open);
        SetLblLock(lv);
        SetLblTime(openTime);
    }

    internal void UpdateOpen(bool open)
    {
        hasOpen = open;
        if (lblName != null)
        {
            lblName.color = open ? colorNameUnLock : colorNameLock;
        }
        if (lblTime != null)
        {
            lblTime.color = open ? colorTimeUnLock : colorTimeLock;
        }
        if (goLock != null)
        {
            NGUITools.SetActive(goLock, !open);
        }
    }

    internal void SetLblTime(string openTime)
    {
        if (lblTime != null)
        {
            lblTime.text = openTime;
        }
    }

    internal void SetLblLock(int lv)
    {
        openLv = lv;
        if (lblLock != null)
        {
            lblLock.text = ArkCrossEngine.StrDictionaryProvider.Instance.Format(877, lv);
        }
    }

    internal void PlayUnLock()
    {
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Trial/JieSuoDa"));
        unLockEffect = NGUITools.AddChild(gameObject, go);
        if (unLockEffect != null)
        {
            float time = 0f;
            JieSuoDa script = unLockEffect.GetComponent<JieSuoDa>();
            if (script != null)
            {
                time = script.finishiTime;
                script.SetLblLv(lblLock == null ? "" : lblLock.text);
            }
            UnityEngine.BoxCollider box = gameObject.GetComponent<UnityEngine.BoxCollider>();
            UIButton button = gameObject.GetComponent<UIButton>();
            //按钮失效
            if (box != null && button != null)
            {
                button.enabled = false;
                box.enabled = false;
            }
            Invoke("UnLockFinish", time);
        }
    }
    //播完销毁，可点
    private void UnLockFinish()
    {
        if (unLockEffect != null)
        {
            NGUITools.Destroy(unLockEffect);
            unLockEffect = null;
        }
        UnityEngine.BoxCollider box = gameObject.GetComponent<UnityEngine.BoxCollider>();
        UIButton button = gameObject.GetComponent<UIButton>();
        if (box != null && button != null)
        {
            button.isEnabled = true;
            button.enabled = true;
            box.enabled = true;
        }
    }

    void OnDisable()
    {
        if (IsInvoking("UnLockFinish"))
        {
            CancelInvoke("UnLockFinish");
            UnLockFinish();
        }
    }
}
