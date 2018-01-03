using UnityEngine;
using System;
using System.Collections.Generic;
using ArkCrossEngine;

public struct SkillIconInfo
{
    public UnityEngine.Vector3 targetPos;
    public SkillCategory skillType;
    public bool isVetical;
    public float time;

}

public class ReceiveInput : UnityEngine.MonoBehaviour
{
    protected struct CandidateSkillInfo
    {
        public SkillCategory skillType;
        public UnityEngine.Vector3 targetPos;
    }

    internal void Start()
    {
        try
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("Ai_InputSkillPursuitCmd", "Input", this.SkillPursuit);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, float, float, float>("Ai_InputAttackCmd", "Input", this.AttackHandle);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("Ai_InputSkillCmd", "Input", this.SkillHandle);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<GestureEvent, float, float, float, bool, bool>("Op_InputEffect", "Input", this.EffectHandle);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<bool>("SetTouchEnable", "Input", SetTouchEnable);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<bool>("SetJoystickEnable", "Input", SetJoystickEnable);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("hide_skill_tip", "ui", HideSkillTip);
            ///
            TouchManager.OnGestureHintEvent += OnGestureHintEvent;
            TouchManager.OnGestureEvent += OnGestureEvent;
            TouchManager.OnFingerEvent += OnFingerEvent;
            ///
            EasyJoystick.On_JoystickMoveStart += On_JoystickMoveStart;
            EasyJoystick.On_JoystickMove += On_JoystickMove;
            EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
            m_SkillTipObj = ArkCrossEngine.ResourceSystem.NewObject(SkillTipPrefab) as UnityEngine.GameObject;
            if (m_SkillTipObj != null)
            {
                m_SkillTipObj.SetActive(false);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SetTouchEnable(bool value)
    {
        try
        {
            TouchManager.TouchEnable = value;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SetJoystickEnable(bool value)
    {
        try
        {
            JoyStickInputProvider.JoyStickEnable = value;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SkillPursuit(int id)
    {
        //GfxModule.Skill.GfxSkillSystem.Instance.AddBreakSkillTask();
    }

    internal void Update()
    {
        try
        {
            if (!isRegister)
            {
                if (null != skill_ctrl)
                {
                    skill_ctrl.RegisterSkillQECanInputHandler(SkillQEInputHandler);
                    skill_ctrl.RegisterSkillStartHandler(SkillStartHandler);
                    isRegister = true;
                }
            }
            ///
            skill_active_remaintime -= UnityEngine.Time.deltaTime;
            if (skill_active_remaintime <= 0)
            {
                can_conjure_q_skill = false;
                can_conjure_e_skill = false;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    internal void OnDestroy()
    {
        TouchManager.OnGestureHintEvent -= OnGestureHintEvent;
        TouchManager.OnGestureEvent -= OnGestureEvent;
        TouchManager.OnFingerEvent -= OnFingerEvent;
        ///
        EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        //GfxModule.Skill.Trigers.TriggerUtil.IsWantMoving = false;
    }

    private void OnGestureHintEvent(Gesture gesture)
    {
        if (null == gesture)
        {
            return;
        }
        if (HintType.Hint == gesture.HintFlag)
        {
        }
        else if (HintType.RFailure == gesture.HintFlag)
        {
        }
        else if (HintType.RSucceed == gesture.HintFlag)
        {
            ///
            bool isInSkillCD = false;
            if (0 == gesture.SectionNum && skill_ctrl != null && skill_ctrl.IsCategorySkillInCD(gesture.SkillTags))
            {
                isInSkillCD = true;
            }
            ///
            if (gesture.SkillTags == SkillCategory.kSkillE || gesture.SkillTags == SkillCategory.kSkillQ)
            {
                if ((can_conjure_q_skill && gesture.SkillTags == SkillCategory.kSkillQ)
                  || (can_conjure_e_skill && gesture.SkillTags == SkillCategory.kSkillE))
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_ui_angle", "ui", gesture.Position, gesture.SkillTags, isInSkillCD);
                    can_conjure_q_skill = false;
                    can_conjure_e_skill = false;
                }
            }
            else
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_ui_angle", "ui", gesture.StartPosition, gesture.SkillTags, isInSkillCD);
                if (0 == gesture.SectionNum)
                {
                    ShowSkillTip(gesture.GetStartTouchToWorldPoint());
                }
                ///
                List<SkillNode> nodeInfo = null;
                if (skill_ctrl != null)
                {
                    skill_ctrl.QuerySkillQE(gesture.SkillTags, gesture.SectionNum);
                }
                if (null != nodeInfo && nodeInfo.Count > 0)
                {
                    skill_active_remaintime = 1.0f;
                    for (int i = 0; i < nodeInfo.Count; i++)
                    {
                        if (SkillCategory.kSkillQ == nodeInfo[i].Category)
                        {
                            can_conjure_q_skill = true;
                        }
                        else
                        {
                            can_conjure_e_skill = true;
                        }
                    }
                    /*
          foreach (SkillNode node in nodeInfo) {
            if (SkillCategory.kSkillQ == node.Category) {
              can_conjure_q_skill = true;
            } else {
              can_conjure_e_skill = true;
            }
          }*/
                }
            }
        }
    }

    private void ShowSkillTip(UnityEngine.Vector3 touchpos)
    {
        if (m_SkillTipObj == null)
        {
            return;
        }
        if (LogicSystem.PlayerSelf == null)
        {
            return;
        }
        UnityEngine.Vector3 dir = touchpos - new UnityEngine.Vector3(LogicSystem.PlayerSelf.transform.position.x, LogicSystem.PlayerSelf.transform.position.y, LogicSystem.PlayerSelf.transform.position.z);
        dir.y = 0;
        m_SkillTipObj.SetActive(true);
        m_SkillTipObj.transform.position = new UnityEngine.Vector3(touchpos.x, touchpos.y, touchpos.z);
        m_SkillTipObj.transform.forward = new UnityEngine.Vector3(dir.x, dir.y, dir.z);
    }

    private void HideSkillTip()
    {
        if (m_SkillTipObj != null)
        {
            m_SkillTipObj.SetActive(false);
        }
    }

    private void OnGestureEvent(Gesture gesture)
    {
        if (null == gesture)
        {
            return;
        }
        UnityEngine.Vector3 target_pos = gesture.GetStartTouchToWorldPoint();
        if (null != gesture.Recognizer && UnityEngine.Vector3.zero != target_pos)
        {
            if ("OnDoubleTap" == gesture.Recognizer.EventMessageName
              && gesture.SelectedID < 0)
            {
                GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
                //GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(ArkCrossEngine.LogicSystem.PlayerSelf, SkillCategory.kRoll, target_pos);
                return;
            }
        }
        if (SkillCategory.kNone != gesture.SkillTags)
        {
            switch (gesture.SkillTags)
            {
                case SkillCategory.kSkillA:
                case SkillCategory.kSkillB:
                case SkillCategory.kSkillC:
                case SkillCategory.kSkillD:
                case SkillCategory.kSkillQ:
                case SkillCategory.kSkillE:
                    GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
                    if (gesture.SectionNum > 0)
                    {
                        if (waite_skill_buffer.Count > 0)
                        {
                            CandidateSkillInfo candidate_skill_info = new CandidateSkillInfo();
                            candidate_skill_info.skillType = gesture.SkillTags;
                            candidate_skill_info.targetPos = UnityEngine.Vector3.zero;
                            waite_skill_buffer.Add(candidate_skill_info);
                        }
                        else
                        {
                            GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(ArkCrossEngine.LogicSystem.PlayerSelf, gesture.SkillTags, UnityEngine.Vector3.zero);
                        }
                    }
                    else
                    {
                        waite_skill_buffer.Clear();
                        CandidateSkillInfo candidate_skill_info = new CandidateSkillInfo();
                        candidate_skill_info.skillType = gesture.SkillTags;
                        candidate_skill_info.targetPos = target_pos;
                        waite_skill_buffer.Add(candidate_skill_info);
                        GestureArgs e = TouchManager.ToGestureArgs(gesture);
                        LogicSystem.FireGestureEvent(e);
                    }
                    break;
            }
        }
    }

    private void OnFingerEvent(FingerEvent fevent)
    {
        if (TouchManager.GestureEnable)
        {
            if (fevent.Finger.IsDown && !fevent.Finger.WasDown)
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_finger_event", "ui", fevent.Position, true);
            }
            else if (fevent.Finger.WasDown && !fevent.Finger.IsDown)
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_finger_event", "ui", fevent.Position, false);
            }
        }
    }

    private void AttackHandle(int id, float x, float y, float z)
    {
        try
        {
            UnityEngine.GameObject obj = ArkCrossEngine.LogicSystem.PlayerSelf;
            if (null != obj)
            {
                GfxModule.Skill.GfxSkillSystem.Instance.StartAttack(ArkCrossEngine.LogicSystem.PlayerSelf, new UnityEngine.Vector3(x, y, z));
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SkillHandle(int id)
    {
        try
        {
            for (int i = 0; i < waite_skill_buffer.Count; i++)
            {
                push_skill_buffer.Add(waite_skill_buffer[i]);
            }
            waite_skill_buffer.Clear();
            for (int i = 0; i < push_skill_buffer.Count; i++)
            {
                GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(ArkCrossEngine.LogicSystem.PlayerSelf, push_skill_buffer[i].skillType, new UnityEngine.Vector3(push_skill_buffer[i].targetPos.x, push_skill_buffer[i].targetPos.y, push_skill_buffer[i].targetPos.z));
            }
            push_skill_buffer.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SkillQEInputHandler(float remaintime, List<SkillNode> skills)
    {
        if (null != skills && skills.Count > 0)
        {
            skill_active_remaintime = remaintime;
            for (int i = 0; i < skills.Count; i++)
            {
                if (SkillCategory.kSkillQ == skills[i].Category)
                {
                    can_conjure_q_skill = true;
                }
                else
                {
                    can_conjure_e_skill = true;
                }
            }
        }
    }

    private void SkillStartHandler()
    {
        GestureArgs e = new GestureArgs();
        e.name = GestureEvent.OnSkillStart.ToString();
        LogicSystem.FireGestureEvent(e);
    }

    void On_JoystickMoveStart(MovingJoystick move)
    {
        //TouchManager.GestureEnable = false;
    }
    void On_JoystickMoveEnd(MovingJoystick move)
    {
        TriggerMove(move, true);
        //TouchManager.GestureEnable = true;
    }
    void On_JoystickMove(MovingJoystick move)
    {
        if (TouchManager.Touches.Count > 0 && TouchManager.Instance.joystickEnable)
        {
            TriggerMove(move, false);
        }
    }
    private void TriggerMove(MovingJoystick move, bool isLift)
    {
        if (isLift)
        {
            GestureArgs e = new GestureArgs();
            e.name = "OnSingleTap";
            e.airWelGamePosX = 0f;
            e.airWelGamePosY = 0f;
            e.airWelGamePosZ = 0f;
            e.selectedObjID = -1;
            e.towards = -1f;
            e.inputType = InputType.Joystick;
            //LogicSystem.FireGestureEvent(e);
            LogicSystem.SetJoystickInfo(e);
            return;
        }

        UnityEngine.GameObject playerSelf = LogicSystem.PlayerSelf;
        if (playerSelf != null && move.joystickAxis != UnityEngine.Vector2.zero)
        {
            UnityEngine.Vector2 joyStickDir = move.joystickAxis * 10.0f;
            UnityEngine.Vector3 targetRot = new UnityEngine.Vector3(joyStickDir.x, 0, joyStickDir.y);
            UnityEngine.Vector3 targetPos = playerSelf.transform.position + targetRot;

            GestureArgs e = new GestureArgs();
            e.name = "OnSingleTap";
            e.selectedObjID = -1;
            float towards = UnityEngine.Mathf.Atan2(targetPos.x - playerSelf.transform.position.x, targetPos.z - playerSelf.transform.position.z);
            e.towards = towards;
            e.airWelGamePosX = targetPos.x;
            e.airWelGamePosY = targetPos.y;
            e.airWelGamePosZ = targetPos.z;
            e.inputType = InputType.Joystick;
            //LogicSystem.FireGestureEvent(e);
            LogicSystem.SetJoystickInfo(e);
        }
    }

    /// effect handle
    private void EffectHandle(GestureEvent ge, float posX, float posY, float posZ, bool isSelected, bool isLogicCmd)
    {
        try
        {
            if (DFMUiRoot.InputMode == InputType.Joystick)
            {
                return;
            }
            UnityEngine.Vector3 effect_pos = new UnityEngine.Vector3(posX, posY, posZ);
            if (GestureEvent.OnSingleTap == ge)
            {
                if (isSelected)
                {
                    if (!isLogicCmd)
                    {
                        PlayEffect(Go_Lock, effect_pos, Go_Lock_Time);
                    }
                }
                else
                {
                    PlayEffect(Go_Landmark, effect_pos, Go_Landmark_Time);
                    HideSkillTip();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void PlayEffect(UnityEngine.GameObject effectPrefab, UnityEngine.Vector3 position, float playTime)
    {
        UnityEngine.GameObject obj = ArkCrossEngine.ResourceSystem.NewObject(effectPrefab, playTime) as GameObject;
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = UnityEngine.Quaternion.identity;
        }
    }

    protected float skill_active_remaintime = -1;
    protected bool can_conjure_q_skill = false;
    protected bool can_conjure_e_skill = false;
    protected bool isRegister = false;
    protected List<CandidateSkillInfo> waite_skill_buffer = new List<CandidateSkillInfo>();
    protected List<CandidateSkillInfo> push_skill_buffer = new List<CandidateSkillInfo>();
    protected ArkCrossEngine.SkillController skill_ctrl = null;
    protected UnityEngine.GameObject m_SkillTipObj = null;
    public UnityEngine.GameObject Go_Landmark = null;
    public float Go_Landmark_Time = 0.2f;
    public UnityEngine.GameObject Go_Lock = null;
    public UnityEngine.GameObject SkillTipPrefab = null;
    public float Go_Lock_Time = 0.2f;
    ///
    public delegate void EventHandler(float towards);
    public static EventHandler OnJoystickMove;
}