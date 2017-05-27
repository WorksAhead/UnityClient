using UnityEngine;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIBeginnerGuide
{
    //初始化新手引导事件
    public delegate void OnGuideGameStart();
    public OnGuideGameStart onGuideGameStart;
    public void InitGuide(OnGuideGameStart guideEvent)
    {
        onGuideGameStart = guideEvent;
        UIManager.Instance.OnAllUiLoadedDelegate += ShowStartUI;
    }
    public void ShowStartUI()
    {
        UIManager.Instance.LoadWindowByName("GuideStartGame", UICamera.mainCamera);
        //UIManager.Instance.HideWindowByName("HeroPanel");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (go != null)
        {
            HeroPanel heroView = go.GetComponent<HeroPanel>();
            if (heroView != null) heroView.SetActive(false);
            SkillBar skillBar = go.GetComponent<SkillBar>();
            if (skillBar != null) skillBar.SetActive(false);
        }
        //UIManager.Instance.HideWindowByName("SkillBar");
        JoyStickInputProvider.JoyStickEnable = false;
    }
    public void ClearHandler()
    {
        UIManager.Instance.OnAllUiLoadedDelegate -= ShowStartUI;
    }

    //移动小手到普通攻击按钮、显示一个普通攻击按钮
    public void TransHandInCommonAttact(int index)
    {
        //UIManager.Instance.ShowWindowByName("SkillBar");
        UnityEngine.GameObject goHand = UIManager.Instance.GetWindowGoByName("GuideHand");
        if (goHand == null)
            goHand = UIManager.Instance.LoadWindowByName("GuideHand", UICamera.mainCamera);
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        //UnityEngine.GameObject goSkillbar = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (goHand != null && go != null)
        {
            SkillBar sb = go.GetComponent<SkillBar>();
            if (sb != null)
            {
                sb.SetActive(true);
                if (sb.spAshEx != null) NGUITools.SetActive(sb.spAshEx.gameObject, false);
                if (index == 1) sb.InitSkillBar(null);//关掉所有技能按钮
                if (sb.CommonSkillGo != null)
                {
                    UnityEngine.Vector3 pos = sb.CommonSkillGo.transform.position;
                    pos = UICamera.mainCamera.WorldToScreenPoint(pos);
                    UISprite sp = sb.CommonSkillGo.GetComponent<UISprite>();
                    if (sp != null)
                    {
                        //640、720为UIRoot设置的俩值
                        float scale = 1f;
                        if (Screen.height < UIManager.UIRootMinimumHeight)
                            scale = Screen.height / (float)UIManager.UIRootMinimumHeight;
                        if (Screen.height > UIManager.UIRootMaximunHeight)
                            scale = Screen.height / (float)UIManager.UIRootMaximunHeight;
                        pos = new UnityEngine.Vector3(pos.x - (sp.width / 2 * scale), pos.y + (sp.height / 2) * scale, 0);
                    }
                    pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
                    goHand.transform.position = pos;
                }
            }
        }
        UIManager.Instance.ShowWindowByName("GuideHand");
    }
    //移动小手到第一个技能按钮
    public void TransHandInFirstSkill()
    {
        UnityEngine.GameObject goHand = UIManager.Instance.GetWindowGoByName("GuideHand");
        //UnityEngine.GameObject goSkillbar = UIManager.Instance.GetWindowGoByName("SkillBar");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (goHand != null && go != null)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            SkillBar sb = go.GetComponent<SkillBar>();
            if (sb != null)
            {
                infos.Clear();
                for (int i = 0; i < role_info.SkillInfos.Count; i++)
                {
                    if (role_info.SkillInfos[i].ConfigData.Category == SkillCategory.kSkillA)
                    {
                        infos.Add(role_info.SkillInfos[i]);
                        sb.InitSkillBar(infos);
                        break;
                    }
                }
                /*
        foreach (SkillInfo info in role_info.SkillInfos) {
          if (info.ConfigData.Category == SkillCategory.kSkillA) {
            infos.Add(info);
            sb.InitSkillBar(infos);
            break;
          }
        }*/
                //sb.InitSkillBar(role_info.SkillInfos);
                //sb.UnlockSkill(SkillCategory.kSkillA, true);
                UnityEngine.Transform tsSkillA = sb.SkillBarView.transform.Find("Skill0/skill0");
                if (tsSkillA != null)
                {
                    UnityEngine.Vector3 pos = tsSkillA.position;
                    goHand.transform.position = pos;
                }
            }
        }
        UIManager.Instance.ShowWindowByName("GuideHand");
    }
    //显示回到主城按钮
    public void ShowReturnButton()
    {
        UIManager.Instance.HideWindowByName("GuideHand");
        UIManager.Instance.LoadWindowByName("ReturnToMaincity", UICamera.mainCamera);
    }
    //设置技能按钮不可用
    public void SetSkillBarActive(bool active)
    {
        //UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("SkillBar");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (null != go)
        {
            SkillBar sBar = go.GetComponent<SkillBar>();
            if (sBar != null)
            {
                UIButton[] skillBtnArr = sBar.SkillBarView.GetComponentsInChildren<UIButton>();
                for (int i = 0; i < skillBtnArr.Length; i++)
                {
                    skillBtnArr[i].isEnabled = active;
                }
                /*
        foreach (UIButton btn in skillBtnArr) {
          btn.isEnabled = active;
        }*/
            }
        }
    }
    //
    public void HideSkillBar()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        SkillBar sBar = null;
        if (go != null)
        {
            sBar = go.GetComponent<SkillBar>();
            if (sBar != null) sBar.SetActive(false);
        }
    }
    private bool m_IsShow = false;
    public void ShowGuideDlgInControl(UnityEngine.Vector2 center, float y)
    {
        if (!m_IsShow)
        {
            m_IsShow = true;
            //
            //UIManager.Instance.HideWindowByName("SkillBar");
            float scale = 1f;
            if (Screen.height > UIManager.UIRootMaximunHeight)
                scale = Screen.height / (float)UIManager.UIRootMaximunHeight;
            if (Screen.height < UIManager.UIRootMinimumHeight)
                scale = Screen.height / (float)UIManager.UIRootMinimumHeight;
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(center.x - c_OffsetX * scale, center.y + y * 2 / 3, 0f);
            pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
            UnityEngine.GameObject goGuideDlg = UIManager.Instance.GetWindowGoByName("GuideDlg");
            if (goGuideDlg == null)
            {
                goGuideDlg = UIManager.Instance.LoadWindowByName("GuideDlg", UICamera.mainCamera);
            }
            if (goGuideDlg != null)
            {
                goGuideDlg.transform.position = pos;
                UIGuideDlg guideDlg = goGuideDlg.GetComponent<UIGuideDlg>();
                if (guideDlg != null) guideDlg.SetDescription(501);
            }
        }
    }
    public void ShowGuideDlgAboveCommon(int index)
    {
        UnityEngine.Vector3 pos = new UnityEngine.Vector3(20, 300, 0);

        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        //UnityEngine.GameObject goSkillBar = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (go != null)
        {
            SkillBar skillBar = go.GetComponent<SkillBar>();
            if (skillBar != null && skillBar.CommonSkillGo != null)
            {
                pos = skillBar.CommonSkillGo.transform.position;
                pos = UICamera.mainCamera.WorldToScreenPoint(pos);
                UISprite sp = skillBar.CommonSkillGo.GetComponent<UISprite>();
                if (sp != null)
                {
                    //640、768为UIRoot设置的俩值
                    float scale = 1f;
                    if (Screen.height < UIManager.UIRootMinimumHeight)
                        scale = Screen.height / (float)UIManager.UIRootMinimumHeight;
                    if (Screen.height > UIManager.UIRootMaximunHeight)
                        scale = Screen.height / (float)UIManager.UIRootMaximunHeight;
                    pos = new UnityEngine.Vector3(pos.x - (sp.width / 2 - c_OffsetX) * scale, pos.y + sp.height * scale + c_OffsetY, 0);
                }
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
            }
        }

        UnityEngine.GameObject goGuideDlg = UIManager.Instance.GetWindowGoByName("GuideDlgRight");
        if (goGuideDlg == null)
        {
            goGuideDlg = UIManager.Instance.LoadWindowByName("GuideDlgRight", UICamera.mainCamera);
        }
        if (goGuideDlg != null)
        {
            goGuideDlg.transform.position = pos;
            UIGuideDlg guideDlg = goGuideDlg.GetComponent<UIGuideDlg>();
            if (guideDlg != null)
            {
                if (index == 1) guideDlg.SetDescription(502);
                if (index == 2) guideDlg.SetDescription(505);
            }
        }
        UIManager.Instance.ShowWindowByName("GuideDlgRight");
    }
    public void ShowGuideDlgAboveSkillA(int index)
    {
        UnityEngine.Vector3 pos = new UnityEngine.Vector3(20, 300, 0);

        UnityEngine.GameObject goSkillBar = null;
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (go != null)
        {
            SkillBar sBar = go.GetComponent<SkillBar>();
            if (sBar != null)
            {
                goSkillBar = sBar.SkillBarView;
            }
        }
        //UnityEngine.GameObject goSkillBar = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (goSkillBar != null)
        {
            UnityEngine.Transform tsSkillA = goSkillBar.transform.Find("Skill0/skill0");
            if (tsSkillA != null)
            {
                pos = tsSkillA.position;
                pos = UICamera.mainCamera.WorldToScreenPoint(pos);
                UISprite sp = tsSkillA.GetComponent<UISprite>();
                if (sp != null)
                {
                    float scale = 1f;
                    if (Screen.height < UIManager.UIRootMinimumHeight)
                        scale = Screen.height / (float)UIManager.UIRootMinimumHeight;
                    if (Screen.height > UIManager.UIRootMaximunHeight)
                        scale = Screen.height / (float)UIManager.UIRootMaximunHeight;
                    pos = new UnityEngine.Vector3(pos.x + c_OffsetX * scale, pos.y + sp.height * scale, 0);
                }
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
            }
        }
        UnityEngine.GameObject goGuideDlg = UIManager.Instance.GetWindowGoByName("GuideDlgRight");
        if (goGuideDlg == null)
        {
            goGuideDlg = UIManager.Instance.LoadWindowByName("GuideDlgRight", UICamera.mainCamera);
        }
        if (goGuideDlg != null)
        {
            goGuideDlg.transform.position = pos;
            UIGuideDlg guideDlg = goGuideDlg.GetComponent<UIGuideDlg>();
            if (guideDlg != null)
            {
                if (index == 1) guideDlg.SetDescription(503);
                if (index == 2) guideDlg.SetDescription(504);
            }
        }
        UIManager.Instance.ShowWindowByName("GuideDlgRight");
    }
    //让普攻图标失效
    public void SetCommonSkillBtnActive(bool active)
    {
        /*UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (go == null) return;
        SkillBar skillBar = go.GetComponent<SkillBar>();
        if (skillBar != null && skillBar.CommonSkillGo != null) {
          UIButton btn = skillBar.CommonSkillGo.GetComponent<UIButton>();
          if (btn != null) btn.isEnabled = active;
        }*/

        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (go != null)
        {
            SkillBar sBar = go.GetComponent<SkillBar>();
            if (sBar != null && sBar.CommonSkillGo != null)
            {
                UIButton btn = sBar.CommonSkillGo.GetComponent<UIButton>();
                if (btn != null) btn.isEnabled = active;
            }
        }
    }
    //这是 c_OffsetX = 45 是根据GuideDlg控件箭头到左边距的大小来定的
    // c_OffsetY 
    private const float c_OffsetX = 45f;
    private const float c_OffsetY = 25f;
    public List<SkillInfo> infos = new List<SkillInfo>();
    public static UIBeginnerGuide Instance
    {
        get
        {
            return m_Instance;
        }
    }
    private static UIBeginnerGuide m_Instance = new UIBeginnerGuide();
}
