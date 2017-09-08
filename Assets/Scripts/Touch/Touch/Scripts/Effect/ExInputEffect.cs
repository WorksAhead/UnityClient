using UnityEngine;
using ArkCrossEngine;

public class ExInputEffect : UnityEngine.MonoBehaviour
{
    internal void Start()
    {
        try
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ex_skill_start", "skill", this.SkillStart);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ex_skill_end", "skill", this.SkillEnd);

            TouchManager.OnFingerEvent += OnFingerEvent;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SkillStart()
    {
        active = true;
        if (InputType.Joystick == DFMUiRoot.InputMode)
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
        else
        {
            TouchManager.GestureEnable = false;
        }
    }

    private void SkillEnd()
    {
        active = false;
        if (InputType.Joystick == DFMUiRoot.InputMode)
        {
            UIManager.Instance.ShowJoystick(true);
        }
        else
        {
            TouchManager.GestureEnable = true;
        }
    }

    internal void OnDestroy()
    {
        try
        {
            TouchManager.OnFingerEvent -= OnFingerEvent;
            if (obj != null)
            {
                UnityEngine.GameObject.Destroy(obj);
                obj = null;
            }
            original = null;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Update()
    {
    }

    private void OnFingerEvent(FingerEvent args)
    {
        if (!active)
        {
            return;
        }
        if (null == args)
        {
            return;
        }
        if (ArkCrossEngine.TouchType.Regognizer != TouchManager.curTouchState)
        {
            return;
        }
        if (GestureEvent.OnFingerMove.ToString() == args.Name)
        {
            if (TouchManager.GestureEnable || JoyStickInputProvider.JoyStickEnable)
            {
                return;
            }
            if (args.Finger.IsDown && args.Finger.IsMoving)
            {
                if (args.Finger.DistanceFromStart > validLength)
                {
                    UnityEngine.Vector3 curPos = UnityEngine.Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(args.Position.x, args.Position.y, depth));
                    if (null == obj)
                    {
                        obj = UnityEngine.GameObject.Instantiate(original, curPos, UnityEngine.Quaternion.identity) as UnityEngine.GameObject;
                    }
                    if (null != obj)
                    {
                        obj.transform.position = curPos;
                    }
                }
            }
        }
        else if (GestureEvent.OnFingerUp.ToString() == args.Name)
        {
            if (args.Finger.WasDown && null != obj)
            {
                UnityEngine.GameObject.Destroy(obj, duration);
                obj = null;
            }
        }
    }

    private bool active = false;
    private UnityEngine.GameObject obj = null;
    public UnityEngine.Object original = null;
    [SerializeField]
    public float duration = 1.0f;
    [SerializeField]
    public float depth = 1.0f;
    [SerializeField]
    public float validLength = 10f;
}