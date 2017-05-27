using UnityEngine;
using System.Collections;
using GfxModule.Skill;
using ArkCrossEngine;

public class TestSkillInput : UnityEngine.MonoBehaviour
{
    public ArkCrossEngine.KeyCode m_AttackKey = ArkCrossEngine.KeyCode.J;
    public ArkCrossEngine.KeyCode m_SkillAKey = ArkCrossEngine.KeyCode.I;
    public ArkCrossEngine.KeyCode m_SkillBKey = ArkCrossEngine.KeyCode.O;
    public ArkCrossEngine.KeyCode m_SkillCKey = ArkCrossEngine.KeyCode.K;
    public ArkCrossEngine.KeyCode m_SkillDKey = ArkCrossEngine.KeyCode.L;
    public ArkCrossEngine.KeyCode m_SkillQKey = ArkCrossEngine.KeyCode.Q;
    public ArkCrossEngine.KeyCode m_SkillEKey = ArkCrossEngine.KeyCode.E;
    public ArkCrossEngine.KeyCode m_SkillEX = ArkCrossEngine.KeyCode.Y;
    public ArkCrossEngine.KeyCode m_ChangeInput = ArkCrossEngine.KeyCode.F8;

    // Use this for initialization
    void Awake()
    {
        try
        {
            //LogicSystem.EventChannelForGfx.Subscribe("arena_info_result", "arena", OnArenaInfo);
            //LogicSystem.EventChannelForGfx.Subscribe("match_group_result", "arena", OnMatchGroupResult);
            //LogicSystem.EventChannelForGfx.Subscribe<int>("start_challenge_result", "arena", OnStartChallengeResult);
            //LogicSystem.EventChannelForGfx.Subscribe<ulong, int, ulong, int, bool>("challenge_result", "arena", OnChallengeResult);
            //LogicSystem.EventChannelForGfx.Subscribe("query_rank_result", "arena", OnQueryRankResult);
            //LogicSystem.EventChannelForGfx.Subscribe("change_partners_result", "arena", OnChangePartnersResult);
            //LogicSystem.EventChannelForGfx.Subscribe("query_history_result", "arena", OnQueryHistory);
            //LogicSystem.EventChannelForGfx.Subscribe<int, int, int>("buy_fight_count_result", "arena", OnBuyFightCountResult);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ArkCrossEngine.GameObject _gameobject = LogicSystem.PlayerSelf;
        try
        {
            if (gameObject != CrossObjectHelper.TryCastObject < UnityEngine.GameObject > (LogicSystem.PlayerSelf))
            {
                return;
            }
            if (ArkCrossEngine.Input.GetKeyDown(m_AttackKey))
            {
                GfxSkillSystem.Instance.StartAttack(_gameobject, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_AttackKey))
            {
                GfxSkillSystem.Instance.StopAttack(_gameobject);
            }
            if (ArkCrossEngine.Input.GetKeyDown(m_SkillAKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillA, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_SkillAKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillA);
            }
            if (ArkCrossEngine.Input.GetKeyDown(m_SkillBKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillB, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_SkillBKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillB);
            }
            if (ArkCrossEngine.Input.GetKeyDown(m_SkillCKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillC, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_SkillCKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillC);
            }
            if (ArkCrossEngine.Input.GetKeyDown(m_SkillDKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillD, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_SkillDKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillD);
            }
            if (ArkCrossEngine.Input.GetKeyDown(m_SkillQKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillQ, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_SkillQKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillQ);
            }
            if (ArkCrossEngine.Input.GetKeyDown(m_SkillEKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillE, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_SkillEKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillE);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_SkillEX))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kEx, ArkCrossEngine.Vector3.zero);
            }
            if (ArkCrossEngine.Input.GetKeyUp(m_ChangeInput))
            {
                ArkCrossEngine.SkillControlMode mode;
                if (DFMUiRoot.InputMode == InputType.Joystick)
                {
                    PlayerPrefs.SetString(DFMUiRoot.INPUT_MODE, DFMUiRoot.INPUT_MODE_TOUCH);
                    DFMUiRoot.InputMode = InputType.Touch;
                    mode = ArkCrossEngine.SkillControlMode.kTouch;
                }
                else
                {
                    PlayerPrefs.SetString(DFMUiRoot.INPUT_MODE, DFMUiRoot.INPUT_MODE_JOYSTICK);
                    DFMUiRoot.InputMode = InputType.Joystick;
                    mode = ArkCrossEngine.SkillControlMode.kJoystick;
                }
                GfxSkillSystem.Instance.ChangeSkillControlMode(_gameobject, mode);
            }
            if (ArkCrossEngine.Input.GetKeyUp(ArkCrossEngine.KeyCode.F9))
            {
                BuyFightCount();
            }
            if (ArkCrossEngine.Input.GetKeyUp(ArkCrossEngine.KeyCode.F10))
            {
            }
            if (ArkCrossEngine.Input.GetKeyUp(ArkCrossEngine.KeyCode.F11))
            {
            }
            if (ArkCrossEngine.Input.GetKeyUp(ArkCrossEngine.KeyCode.F2))
            {
            }
            if (ArkCrossEngine.Input.GetKeyUp(ArkCrossEngine.KeyCode.R))
            {
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void BuyFightCount()
    {
        Debug.Log("---begin buy fight count!");
        LogicSystem.PublishLogicEvent("buy_fight_count", "arena");
    }

    private void OnBuyFightCountResult(int result, int curBuyTime, int curFightCount)
    {
        if (result == (int)ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
        {
            Debug.Log("---buy fight result success! curBuyTime=" + curBuyTime + "  curFightCount=" + curFightCount);
        }
        else
        {
            Debug.Log("---buy fight result failed!");
        }
    }
}
