using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIMarsLoading : UnityEngine.MonoBehaviour
{
    public float playTimeBody = 0f;
    public float playTimeCount1 = 0f;
    public float playTimeCount2 = 0f;
    public float playTimeCount3 = 0f;
    public float playTimeFight = 0f;
    public float playTimeVS = 0f;

    public float duration = 2f;

    public UnityEngine.GameObject effectBody = null;
    public UnityEngine.GameObject effectCount = null;
    public UnityEngine.GameObject effectFight = null;
    public UnityEngine.GameObject effectVS = null;
    public UnityEngine.GameObject posBodyL = null;
    public UnityEngine.GameObject posBodyR = null;
    public UnityEngine.GameObject posCount = null;
    public UnityEngine.GameObject posFight = null;
    public UnityEngine.GameObject posVS = null;

    public UISprite spPlayerLeft;
    public UISprite spPlayerRight;
    public UILabel lblPlayerLevelLeft;
    public UILabel lblPlayerLevelRight;
    public UILabel lblPlayerNameLeft;
    public UILabel lblPlayerNameRight;
    public UILabel lblFightingLeft;
    public UILabel lblFightingRight;
    public Dictionary<int, string> HeroPortraitDict = new Dictionary<int, string>() {
    {
      1,"kuangzhan_heling"
    },
    {
      2,"cike_heling"
    }
  };
    // Use this for initialization
    void Start()
    {
        try
        {
            SetPVPPlayerInfo();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMarsLoadingFinished()
    {
        if (ArkCrossEngine.WorldSystem.Instance.IsPvapScene())
        {
            LogicSystem.PublishLogicEvent("pvp_begin_fight", "pvp");
        }
        UIManager.Instance.HideWindowByName("Marsloading");
    }
    public void SetPVPPlayerInfo()
    {
        List<GfxUserInfo> users = DFMUiRoot.GfxUserInfoListForUI;
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info == null) return;
        UserInfo user_info = role_info.GetPlayerSelfInfo();
        int campId = user_info.GetCampId();
        //users只应该包含两个玩家：自己和对手
        if (users != null)
        {
            for (int index = 0; index < 2; ++index)
            {
                if (users.Count > index && users[index] != null)
                {
                    int heroId = users[index].m_HeroId;
                    int heroLevel = users[index].m_Level;
                    string nickName = users[index].m_Nick;
                    int actorId = users[index].m_ActorId;
                    SharedGameObjectInfo obj_info = LogicSystem.GetSharedGameObjectInfo(actorId);
                    if (obj_info != null)
                    {
                        if (campId == obj_info.CampId)
                        {
                            //自己阵营（也就是自己）
                            if (lblFightingLeft != null) lblFightingLeft.text = obj_info.FightingScore.ToString();
                            if (lblPlayerLevelLeft != null) lblPlayerLevelLeft.text = heroLevel.ToString();
                            if (lblPlayerNameLeft != null) lblPlayerNameLeft.text = nickName;
                            if (HeroPortraitDict.ContainsKey(heroId) && spPlayerLeft != null)
                                spPlayerLeft.spriteName = HeroPortraitDict[heroId];

                        }
                        else
                        {
                            //对手
                            if (lblFightingRight != null) lblFightingRight.text = obj_info.FightingScore.ToString();
                            if (lblPlayerLevelRight != null) lblPlayerLevelRight.text = heroLevel.ToString();
                            if (lblPlayerNameRight != null) lblPlayerNameRight.text = nickName;
                            if (HeroPortraitDict.ContainsKey(heroId) && spPlayerRight != null)
                                spPlayerRight.spriteName = HeroPortraitDict[heroId];
                        }
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        try
        {
            StartCoroutine(PlayEffectBody());
            StartCoroutine(PlayEffectVS());
            StartCoroutine(PlayEffectFight());
            StartCoroutine(PlayCount1());
            StartCoroutine(PlayCount2());
            StartCoroutine(PlayCount3());
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public IEnumerator PlayEffectBody()
    {
        yield return new WaitForSeconds(playTimeBody);
        try
        {
            if (effectBody != null)
            {
                UnityEngine.GameObject efL = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(effectBody));
                if (efL != null && posBodyL != null)
                {
                    efL.transform.position = new UnityEngine.Vector3(posBodyL.transform.position.x, posBodyL.transform.position.y, posBodyL.transform.position.z);
                    Destroy(efL, duration);
                }
                UnityEngine.GameObject efR = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(effectBody));
                if (efR != null && posBodyR != null)
                {
                    efR.transform.Rotate(0f, 180f, 0f);
                    efR.transform.position = new UnityEngine.Vector3(posBodyR.transform.position.x, posBodyR.transform.position.y, posBodyR.transform.position.z);
                    Destroy(efR, duration);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private IEnumerator PlayCount1()
    {
        yield return new WaitForSeconds(playTimeCount1);
        try
        {
            PlayEffectCount();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private IEnumerator PlayCount2()
    {
        yield return new WaitForSeconds(playTimeCount2);
        try
        {
            PlayEffectCount();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private IEnumerator PlayCount3()
    {
        yield return new WaitForSeconds(playTimeCount3);
        try
        {
            PlayEffectCount();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void PlayEffectCount()
    {
        if (effectCount != null)
        {
            UnityEngine.GameObject ef = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(effectCount));
            if (ef != null && posCount != null)
            {
                ef.transform.position = new UnityEngine.Vector3(posCount.transform.position.x, posCount.transform.position.y, posCount.transform.position.z);
                Destroy(ef, duration);
            }
        }
    }

    public IEnumerator PlayEffectFight()
    {
        yield return new WaitForSeconds(playTimeFight);
        try
        {
            if (effectFight != null)
            {
                UnityEngine.GameObject ef = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(effectFight));
                if (ef != null && posFight != null)
                {
                    ef.transform.position = new UnityEngine.Vector3(posFight.transform.position.x, posFight.transform.position.y, posFight.transform.position.z);
                    Destroy(ef, duration);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public IEnumerator PlayEffectVS()
    {
        yield return new WaitForSeconds(playTimeVS);
        try
        {
            if (effectVS != null)
            {
                UnityEngine.GameObject ef = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(effectVS));
                if (ef != null && posVS != null)
                {
                    ef.transform.position = new UnityEngine.Vector3(posVS.transform.position.x, posVS.transform.position.y, posVS.transform.position.z);
                    Destroy(ef, duration);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
