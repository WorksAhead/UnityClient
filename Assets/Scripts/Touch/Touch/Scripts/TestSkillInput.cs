using UnityEngine;
using System.Collections;
using GfxModule.Skill;
using ArkCrossEngine;

public class TestSkillInput : UnityEngine.MonoBehaviour
{
    public KeyCode m_AttackKey = KeyCode.J;
    public KeyCode m_SkillAKey = KeyCode.I;
    public KeyCode m_SkillBKey = KeyCode.O;
    public KeyCode m_SkillCKey = KeyCode.K;
    public KeyCode m_SkillDKey = KeyCode.L;
    public KeyCode m_SkillQKey = KeyCode.Q;
    public KeyCode m_SkillEKey = KeyCode.E;
    public KeyCode m_SkillEX = KeyCode.Y;
    public KeyCode m_ChangeInput = KeyCode.F8;

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
            LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.GameObject _gameobject = LogicSystem.PlayerSelf;
        try
        {
            if (gameObject != LogicSystem.PlayerSelf)
            {
                return;
            }
            if (Input.GetKeyDown(m_AttackKey))
            {
                GfxSkillSystem.Instance.StartAttack(_gameobject, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_AttackKey))
            {
                GfxSkillSystem.Instance.StopAttack(_gameobject);
            }
            if (Input.GetKeyDown(m_SkillAKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillA, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_SkillAKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillA);
            }
            if (Input.GetKeyDown(m_SkillBKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillB, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_SkillBKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillB);
            }
            if (Input.GetKeyDown(m_SkillCKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillC, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_SkillCKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillC);
            }
            if (Input.GetKeyDown(m_SkillDKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillD, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_SkillDKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillD);
            }
            if (Input.GetKeyDown(m_SkillQKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillQ, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_SkillQKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillQ);
            }
            if (Input.GetKeyDown(m_SkillEKey))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kSkillE, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_SkillEKey))
            {
                GfxSkillSystem.Instance.BreakSkill(_gameobject, SkillCategory.kSkillE);
            }
            if (Input.GetKeyUp(m_SkillEX))
            {
                GfxSkillSystem.Instance.PushSkill(_gameobject, SkillCategory.kEx, UnityEngine.Vector3.zero);
            }
            if (Input.GetKeyUp(m_ChangeInput))
            {
                SkillControlMode mode;
                if (DFMUiRoot.InputMode == InputType.Joystick)
                {
                    PlayerPrefs.SetString(DFMUiRoot.INPUT_MODE, DFMUiRoot.INPUT_MODE_TOUCH);
                    DFMUiRoot.InputMode = InputType.Touch;
                    mode = SkillControlMode.kTouch;
                }
                else
                {
                    PlayerPrefs.SetString(DFMUiRoot.INPUT_MODE, DFMUiRoot.INPUT_MODE_JOYSTICK);
                    DFMUiRoot.InputMode = InputType.Joystick;
                    mode = SkillControlMode.kJoystick;
                }
                GfxSkillSystem.Instance.ChangeSkillControlMode(_gameobject, mode);
            }
            if (Input.GetKeyUp(KeyCode.F9))
            {
                BuyFightCount();
            }
            if (Input.GetKeyUp(KeyCode.F10))
            {
            }
            if (Input.GetKeyUp(KeyCode.F11))
            {
            }
            if (Input.GetKeyUp(KeyCode.F2))
            {
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
            }
        }
        catch (System.Exception ex)
        {
            LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
