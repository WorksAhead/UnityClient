using ArkCrossEngine;
using System.Collections.Generic;
public class UIArtifactIntroduce : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject lockArea = null;
    public UnityEngine.GameObject unlockArea = null;
    public UILabel lblAddHp = null;
    public UILabel lblAddDamage = null;
    public UILabel lblAddArmor = null;
    public UILabel lblAddMp = null;
    public UISprite icon = null;
    public UILabel lblName = null;
    public UILabel lblDesc = null;
    public UILabel lblUnlockHint = null;
    public UISprite spImage = null;
    public UnityEngine.Color color = new UnityEngine.Color();
    private UnityEngine.GameObject effect = null;
    private float duration = 1.0f;
    // Use this for initialization
    void Start()
    {
        try
        {
            effect = ArkCrossEngine.ResourceSystem.GetSharedResource("UI_Fx/7_FX_UI_ShengJi_01") as UnityEngine.GameObject;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetIntroduce(int itemId, bool isUnLock, bool isUpgrade = false)
    {
        ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(itemId);
        if (itemCfg != null)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                int itemLevel = 0;
                for (int index = 0; index < role_info.Legacys.Length; ++index)
                {
                    if (role_info.Legacys[index] != null && role_info.Legacys[index].ItemId == itemId)
                        itemLevel = role_info.Legacys[index].Level;
                }
                UserInfo userInfo = role_info.GetPlayerSelfInfo();
                if (lblName != null)
                    lblName.text = itemCfg.m_ItemName;
                if (itemCfg != null && spImage != null)
                {
                    spImage.spriteName = itemCfg.m_ItemTrueName;
                }
                if (null != userInfo && isUnLock)
                {
                    //解锁
                    NGUITools.SetActive(unlockArea, true);
                    NGUITools.SetActive(lockArea, false);
                    if (lblAddDamage != null)
                    {
                        if (isUpgrade == true)
                        {
                            PlayParticle(lblAddDamage.transform.position);
                        }
                        lblAddDamage.text = ((int)itemCfg.m_AttrData.GetAddAd(0, userInfo.GetLevel(), itemLevel)).ToString();//伤害
                    }
                    if (lblAddHp != null)
                    {
                        if (isUpgrade == true)
                        {
                            PlayParticle(lblAddHp.transform.position);
                        }
                        lblAddHp.text = ((int)itemCfg.m_AttrData.GetAddHpMax(0, userInfo.GetLevel(), itemLevel)).ToString();//血量
                    }
                    if (lblAddArmor != null)
                    {
                        if (isUpgrade == true)
                        {
                            PlayParticle(lblAddArmor.transform.position);
                        }
                        lblAddArmor.text = ((int)itemCfg.m_AttrData.GetAddADp(0, userInfo.GetLevel(), itemLevel)).ToString();//护甲
                    }
                    if (lblAddMp != null)
                    {
                        if (isUpgrade == true)
                        {
                            PlayParticle(lblAddMp.transform.position);
                        }
                        lblAddMp.text = ((int)itemCfg.m_AttrData.GetAddMDp(0, userInfo.GetLevel(), itemLevel)).ToString();//魔抗
                    }
                }
                else
                {
                    //没有解锁
                    NGUITools.SetActive(unlockArea, false);
                    NGUITools.SetActive(lockArea, true);
                    lblUnlockHint.text = UnlockTip(itemId);
                }
                if (lblName != null) lblName.text = itemCfg.m_ItemName;
                if (lblDesc != null) lblDesc.text = itemCfg.m_Description;
            }
        }
    }
    //解锁提示语
    string UnlockTip(int itemId)
    {
        string tip = "";
        MyDictionary<int, object> missDataDic = new MyDictionary<int, object>();
        missDataDic = MissionConfigProvider.Instance.GetData();
        foreach (MissionConfig cfg in missDataDic.Values)
        {
            if (cfg.UnlockLegacyId == itemId)
            {
                tip = cfg.Description;
            }
        }
        return tip;
    }
    private void PlayParticle(UnityEngine.Vector3 nguiPos)
    {
        if (effect != null)
        {
            UnityEngine.GameObject ef = ResourceSystem.NewObject(effect) as UnityEngine.GameObject;
            if (ef != null)
            {
                nguiPos.Set(0.9f, nguiPos.y, nguiPos.z);
                ef.transform.position = nguiPos;
                Destroy(ef, duration);
            }
        }
    }
}
