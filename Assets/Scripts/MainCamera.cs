using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using XOpt;
using UnityEngine.SceneManagement;

public class MainCamera : UnityEngine.MonoBehaviour
{
    public void SetFollowEnable(bool value)
    {
        m_IsFollowEnable = value;
    }

    public void SetFollowSpeedByStr(string str_param)
    {
        string[] str_params = str_param.Split('|');
        if (str_params.Length < 5)
        {
            return;
        }
        float maxdistance = float.Parse(str_params[0]);
        float mindistance = float.Parse(str_params[1]);
        float maxspeed = float.Parse(str_params[2]);
        float minspeed = float.Parse(str_params[3]);
        int power = int.Parse(str_params[4]);
        SetFollowSpeed(maxdistance, mindistance, maxspeed, minspeed, power);
    }

    public void SetFollowSpeed(float max_speed_distance, float min_speed_distance,
                               float max_speed, float min_speed, int power)
    {
        m_MaxSpeedDistance = max_speed_distance;
        m_MinSpeedDistance = min_speed_distance;
        m_MaxSpeed = max_speed;
        m_MinSpeed = min_speed;
        m_Power = power;
        ComputeSpeedFactor();
    }

    public void ResetFollowSpeed()
    {
        m_MaxSpeedDistance = m_OrigMaxSpeedDistance;
        m_MinSpeedDistance = m_OrigMinSpeedDistance;
        m_MaxSpeed = m_OrigMaxSpeed;
        m_MinSpeed = m_OrigMinSpeed;
        m_Power = m_OrigPower;
        ComputeSpeedFactor();
    }

    public int GetTargetId()
    {
        return m_CurTargetId;
    }

    public void CameraFollow(int id)
    {
        UnityEngine.GameObject obj = LogicSystem.GetGameObject(id);
        if (null != obj)
        {
            m_CurTargetId = id;
            m_Target = obj.transform;
            m_FixedRoll = 45;
            UnityEngine.Collider collider = m_Target.GetComponent<UnityEngine.Collider>();
            if (null != collider)
            {
                m_CenterOffset = collider.bounds.center - m_Target.position;
                m_HeadOffset = m_CenterOffset;
                m_HeadOffset.y = collider.bounds.max.y - m_Target.position.y;
                m_IsFollow = true;
                Cut();
            }
            else
            {
                m_IsFollow = false;
            }
        }
    }
    //角色创建场景、移动摄像机
    private float factor = 0.0f;
    public IEnumerator MoveCamera(UnityEngine.Vector3 toPos, UnityEngine.Vector3 offsetVec, float delta, UnityEngine.Quaternion quate)
    {
        toPos += offsetVec;
        UnityEngine.Vector3 fromPos = UnityEngine.Camera.main.transform.position;
        float[] posArr = new float[3];
        while (factor <= 1f)
        {
            factor = factor >= 1 ? 1f : factor;
            UnityEngine.Vector3 pos = fromPos + (toPos - fromPos) * factor;
            posArr[0] = pos.x;
            posArr[1] = pos.y;
            posArr[2] = pos.z;
            CameraLookatImmediately(posArr);
            factor += delta;
            yield return new WaitForSeconds(0.00001f);
        }
        m_CameraTransform.localRotation = quate;
        yield break;
    }
    //用于创建角色时移动摄像机
    public void CameraFollowGameObject(UnityEngine.GameObject obj, UnityEngine.Vector3 offsetPlayer, UnityEngine.Quaternion quate)
    {
        if (null != obj)
        {
            UnityEngine.Vector3 fromPos = UnityEngine.Camera.main.transform.position;
            m_Target = obj.transform;
            UnityEngine.Collider collider = m_Target.GetComponent<UnityEngine.Collider>();
            if (null != collider)
            {
                m_CurTargetPos = fromPos + (m_Target.position + offsetPlayer - fromPos) * 0.95f;
                m_CenterOffset = collider.bounds.center - m_Target.position;
                m_HeadOffset = m_CenterOffset;
                m_HeadOffset.y = collider.bounds.max.y - m_Target.position.y;
                m_IsFollow = true;
                Cut();
                m_CameraTransform.localRotation = quate;
            }
            else
            {
                m_IsFollow = false;
            }
        }
    }
    public void EndShake()
    {
        m_IsShaking = false;
    }
    public void BeginShake()
    {
        m_IsShaking = true;
    }
    public void CameraFollowImmediately(int id)
    {
        UnityEngine.GameObject obj = LogicSystem.GetGameObject(id);
        if (null != obj)
        {
            m_CurTargetId = id;
            m_Target = obj.transform;
            UnityEngine.Collider collider = m_Target.GetComponent<UnityEngine.Collider>();
            if (null != collider)
            {
                m_CurTargetPos = m_Target.position;
                m_CenterOffset = collider.bounds.center - m_Target.position;
                m_HeadOffset = m_CenterOffset;
                m_HeadOffset.y = collider.bounds.max.y - m_Target.position.y;
                m_IsFollow = true;
                Cut();
            }
            else
            {
                m_IsFollow = false;
            }
            m_IsFollowEnable = true;
            m_IsShaking = false;
        }
    }
    public void CameraContinueFollowImmediately()
    {
        CameraFollowImmediately(GetTargetId());
    }
    public void CameraLookat(float[] coord)
    {
        m_Target = null;
        m_IsFollow = false;
        m_TargetPos = new UnityEngine.Vector3(coord[0], coord[1], coord[2]);
        //Debug.Log("CameraLookat:" + m_TargetPos.ToString());
        Cut();
    }
    public void CameraLookatImmediately(float[] coord)
    {
        m_Target = null;
        m_IsFollow = false;
        m_TargetPos = new UnityEngine.Vector3(coord[0], coord[1], coord[2]);
        m_CurTargetPos = m_TargetPos;
        //Debug.Log("CameraLookat:" + m_TargetPos.ToString());
        Cut();
    }
    public void LookAtTarget()
    {
        if (m_Target != null)
        {
            m_CameraTransform.LookAt(m_Target.position + m_CenterOffset);
        }
    }
    public void CameraFixedYaw(float dir)
    {
        m_FixedYaw = LogicSystem.RadianToDegree(dir);
        m_AngularSmoothLag = 0.3f;
        m_SnapSmoothLag = 0.2f;
    }
    public void CameraYaw(float[] args)
    {
        if (null == args || args.Length != 2)
            return;
        float dir = args[0];
        float lag = args[1] / 1000.0f;
        m_FixedYaw = LogicSystem.RadianToDegree(dir);
        m_AngularSmoothLag = lag;
        m_SnapSmoothLag = lag;
    }
    public void CameraHeight(float[] args)
    {
        if (null == args || args.Length != 2)
            return;
        float height = args[0];
        float lag = args[1] / 1000.0f;
        if (height >= 0)
            m_Height = height;
        else
            m_Height = m_OrigHeight;
        m_HeightSmoothLag = lag;
        //m_NeedLookat = true;
    }
    public void CameraInit(float[] args)
    {
        if (null == args || args.Length != 2)
            return;
        float dis = args[0];
        float height = args[1];
        m_OrigDistance = dis;
        m_OrigHeight = height;
    }
    public void CameraDistance(float[] args)
    {
        if (null == args || args.Length != 2)
            return;
        float dist = args[0];
        float lag = args[1] / 1000.0f;
        if (dist >= 0)
        {
            m_Distance = dist;
        }
        else
        {
            m_Distance = m_OrigDistance;
        }

        if (m_MaxDistance != m_Distance)
        {
            m_MaxDistance = m_Distance;
            m_CurDistance = m_MaxDistance;
        }
        m_DistanceSmoothLag = lag;
        //m_NeedLookat = true;
    }

    public void CameraEnable(object[] args)
    {
        if (null != args[0] && null != args[1])
        {
            string cameraName = args[0] as string;
            bool isEnable = (bool)args[1];

            if (null != cameraName)
            {
                UnityEngine.GameObject cameraObj = UnityEngine.GameObject.Find(cameraName);
                if (cameraObj != null)
                {
                    UnityEngine.Camera camera = cameraObj.GetComponent<UnityEngine.Camera>();
                    if (null != camera)
                    {
                        camera.enabled = isEnable;
                    }
                }
            }
        }
    }
    public void SetDistanceAndHeight(float[] args)
    {
        if (null == args || args.Length != 2)
        {
            return;
        }
        m_Distance = args[0];
        m_Height = args[1];
    }

    public void ResetDistanceAndHeight()
    {
        m_Distance = m_OrigDistance;
        m_Height = m_OrigHeight;
    }

    internal void Awake()
    {
        try
        {
            m_CameraTransform = UnityEngine.Camera.main.transform;
            m_CameraSpeed = m_CameraFollowSpeed;
            if (!m_CameraTransform)
            {
                Debug.Log("Please assign a camera to the ThirdPersonCamera script.");
                enabled = false;
            }
            ComputeSpeedFactor();
            m_OrigMaxSpeedDistance = m_MaxSpeedDistance;
            m_OrigMinSpeedDistance = m_MinSpeedDistance;
            m_OrigMaxSpeed = m_MaxSpeed;
            m_OrigMinSpeed = m_MinSpeed;
            m_OrigPower = m_Power;

            m_OrigHeight = m_Height;
            m_OrigDistance = m_Distance;
            m_CurDistance = m_Distance;

            EasyJoystick.On_JoystickMoveStart += On_JoystickMoveStart;
            EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }

    internal void LateUpdate()
    {
        try
        {
            if (null == UnityEngine.Camera.main || !UnityEngine.Camera.main.enabled)
            {
                m_CameraTransform = null;
                return;
            }
            if (!m_IsShaking && ArkCrossEngine.LobbyClient.Instance.CurrentRole != null)
            {
                if (!m_CameraSlipping && !UICamera.isOverUI)
                {
                    TraceFingers();
                }
                // Auto rotation

                // In free camera mode, check if camera need to slip in or out.
                if (!m_CameraSlipping)
                {
                    // Begin slip camera in.
                    if (m_CurDistance < m_SlipInDistance && !m_InWatchMode)
                    {
                        // Init slipping in.
                        m_CameraSlipping = true;
                        m_CameraSlippingIn = true;

                        UnityEngine.Vector3 playerBackDir = -m_Target.forward;
                        UnityEngine.Quaternion destRotation = UnityEngine.Quaternion.LookRotation(playerBackDir);

                        m_SlipDestYawAngle = destRotation.eulerAngles.y;
                        float slipDeltaYawAngle = 0.0f;
                        if (m_SlipDestYawAngle > m_CameraTransform.eulerAngles.y)
                        {
                            if (m_SlipDestYawAngle - m_CameraTransform.eulerAngles.y < 180.0f)
                            {
                                slipDeltaYawAngle = m_SlipDestYawAngle - m_CameraTransform.eulerAngles.y;
                            }
                            else
                            {
                                slipDeltaYawAngle = 360.0f - (m_SlipDestYawAngle - m_CameraTransform.eulerAngles.y);
                            }
                        }
                        else
                        {
                            if (m_CameraTransform.eulerAngles.y - m_SlipDestYawAngle < 180.0f)
                            {
                                slipDeltaYawAngle = m_CameraTransform.eulerAngles.y - m_SlipDestYawAngle;
                            }
                            else
                            {
                                slipDeltaYawAngle = 360.0f - (m_CameraTransform.eulerAngles.y - m_SlipDestYawAngle);
                            }
                        }
                        m_SlipYawSpeed = slipDeltaYawAngle / m_SlipTime;
                        if (m_SlipYawSpeed < 1.0f)
                        {
                            m_SlipYawSpeed = 1.0f;
                        }
                        //
                        m_SlipDestRollAngle = destRotation.eulerAngles.x < m_MinCameraAngle ? m_MinCameraAngle : destRotation.eulerAngles.x;
                        float slipDeltaRollAngle = UnityEngine.Mathf.Abs(m_SlipDestRollAngle - m_CameraTransform.eulerAngles.x);
                        m_SlipRollSpeed = slipDeltaRollAngle / m_SlipTime;
                        //
                        m_SlipDestDistance = m_MinDistance;
                        float slipDeltaDistance = m_CurDistance - m_SlipDestDistance;
                        m_SlipZoomSpeed = slipDeltaDistance / m_SlipTime;

                        //
                        m_SlipOriginalYaw = m_CameraTransform.eulerAngles.y;
                        m_SlipOriginalRoll = m_CameraTransform.eulerAngles.x;
                        m_SlipOriginalDistance = m_SlipInDistance + 0.5f;
                    }
                    else if (m_CurDistance >= m_SlipOutDistance && m_InWatchMode)
                    {
                        // Init slipping out.
                        m_CameraSlipping = true;

                        m_SlipDestYawAngle = m_SlipOriginalYaw;
                        float slipDeltaYawAngle = 0.0f;
                        if (m_SlipDestYawAngle > m_CameraTransform.eulerAngles.y)
                        {
                            if (m_SlipDestYawAngle - m_CameraTransform.eulerAngles.y < 180.0f)
                            {
                                slipDeltaYawAngle = m_SlipDestYawAngle - m_CameraTransform.eulerAngles.y;
                            }
                            else
                            {
                                slipDeltaYawAngle = 360.0f - (m_SlipDestYawAngle - m_CameraTransform.eulerAngles.y);
                            }
                        }
                        else
                        {
                            if (m_CameraTransform.eulerAngles.y - m_SlipDestYawAngle < 180.0f)
                            {
                                slipDeltaYawAngle = m_CameraTransform.eulerAngles.y - m_SlipDestYawAngle;
                            }
                            else
                            {
                                slipDeltaYawAngle = 360.0f - (m_CameraTransform.eulerAngles.y - m_SlipDestYawAngle);
                            }
                        }
                        m_SlipYawSpeed = slipDeltaYawAngle / m_SlipTime;
                        if (m_SlipYawSpeed < 1.0f)
                        {
                            m_SlipYawSpeed = 1.0f;
                        }

                        m_SlipDestRollAngle = m_SlipOriginalRoll;
                        float slipDeltaRollAngle = UnityEngine.Mathf.Abs(m_SlipDestRollAngle - m_CameraTransform.eulerAngles.x);
                        m_SlipRollSpeed = slipDeltaRollAngle / m_SlipTime;

                        m_SlipDestDistance = m_SlipOriginalDistance;
                        float slipDeltaDistance = m_SlipDestDistance - m_CurDistance;
                        m_SlipZoomSpeed = slipDeltaDistance / m_SlipTime;
                    }
                }

                // Unlock watch mode in battle scene.
                if (WorldSystem.Instance.IsPvpScene() ||
                    WorldSystem.Instance.IsPveScene() ||
                    WorldSystem.Instance.IsPvapScene() ||
                    WorldSystem.Instance.IsMultiPveScene())
                {
                    m_CameraSlipping = false;
                    m_CameraSlippingIn = false;
                    m_InWatchMode = false;

                    if (!m_IsInBattleScene)
                    {
                        m_IsInBattleScene = true;
                        m_CurDistance = m_MaxDistance;
                    }
                }
                else
                {
                    m_IsInBattleScene = false;
                }

                if (m_CameraSlipping)
                {
                    TouchManager.Instance.joystickEnable = false;

                    bool yawFinished = false;
                    if (m_SlipDestYawAngle > m_CameraTransform.eulerAngles.y)
                    {
                        if (m_SlipDestYawAngle - m_CameraTransform.eulerAngles.y < 180.0f)
                        {
                            m_FixedYaw = m_CameraTransform.eulerAngles.y + m_SlipYawSpeed * Time.deltaTime;
                            if (m_FixedYaw > m_SlipDestYawAngle)
                            {
                                m_FixedYaw = m_SlipDestYawAngle;
                                yawFinished = true;
                            }
                        }
                        else
                        {
                            m_FixedYaw = m_CameraTransform.eulerAngles.y - m_SlipYawSpeed * Time.deltaTime;
                            if (m_FixedYaw + 360.0f < m_SlipDestYawAngle)
                            {
                                m_FixedYaw = m_SlipDestYawAngle - 360.0f;
                                yawFinished = true;
                            }
                        }
                    }
                    else
                    {
                        if (m_CameraTransform.eulerAngles.y - m_SlipDestYawAngle < 180.0f)
                        {
                            m_FixedYaw = m_CameraTransform.eulerAngles.y - m_SlipYawSpeed * Time.deltaTime;
                            if (m_FixedYaw < m_SlipDestYawAngle)
                            {
                                m_FixedYaw = m_SlipDestYawAngle;
                                yawFinished = true;
                            }
                        }
                        else
                        {
                            m_FixedYaw = m_CameraTransform.eulerAngles.y + m_SlipYawSpeed * Time.deltaTime;
                            if (m_FixedYaw > 360.0f + m_SlipDestYawAngle)
                            {
                                m_FixedYaw = 360.0f + m_SlipDestYawAngle;
                                yawFinished = true;
                            }
                        }
                    }


                    bool rollFinished = false;
                    if (UnityEngine.Mathf.Abs(m_SlipDestRollAngle - m_CameraTransform.eulerAngles.x) > 1.0f)
                    {
                        if (m_SlipDestRollAngle > m_CameraTransform.eulerAngles.x)
                        {
                            m_FixedRoll = m_CameraTransform.eulerAngles.x + m_SlipRollSpeed * Time.deltaTime;
                        }
                        else
                        {
                            m_FixedRoll = m_CameraTransform.eulerAngles.x - m_SlipRollSpeed * Time.deltaTime;
                        }
                    }
                    else
                    {
                        rollFinished = true;
                    }

                    bool zoomFinished = false;
                    if (m_CameraSlippingIn)
                    {
                        m_CurDistance -= m_SlipZoomSpeed * Time.deltaTime;
                        if (m_CurDistance <= m_SlipDestDistance)
                        {
                            m_CurDistance = m_SlipDestDistance;
                            zoomFinished = true;
                        }
                    }
                    else
                    {
                        m_CurDistance += m_SlipZoomSpeed * Time.deltaTime;
                        if (m_CurDistance >= m_SlipDestDistance)
                        {
                            m_CurDistance = m_SlipDestDistance;
                            zoomFinished = true;
                        }
                    }

                    if (yawFinished && rollFinished && zoomFinished)
                    {
                        m_CameraSlipping = false;
                        if (m_CameraSlippingIn)
                        {
                            m_CameraSlippingIn = false;
                            m_InWatchMode = true;
                        }
                        else
                        {
                            m_InWatchMode = false;
                            TouchManager.Instance.joystickEnable = true;
                        }
                    }
                }

                // End Auto rotation
                Apply();
            }
        }
        catch (System.Exception ex)
        {
            LogicSystem.LogFromGfx("MainCamera.LateUpdate throw exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelWasLoaded_;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelWasLoaded_;
    }

    private void OnLevelWasLoaded_(Scene scene, LoadSceneMode mode)
    {
        try
        {
            float x, y, z;
            if (WorldSystem.Instance.GetLookAt(out x, out y, out z))
            {
                m_Target = null;
                m_IsFollow = false;
                m_TargetPos = new UnityEngine.Vector3(x, y, z);
                m_FixedYaw = 0;
                m_AngularSmoothLag = 0.001f;
                m_SnapSmoothLag = 0.001f;
                Cut();
            }
            if (null != UnityEngine.Camera.main)
            {
                int layer = LayerMask.NameToLayer("Detail");
                int detailMask = (1 << layer);
                if (GlobalVariables.Instance.IsHD)
                {
                    UnityEngine.Camera.main.cullingMask |= detailMask;
                }
                else
                {
                    UnityEngine.Camera.main.cullingMask &= (~detailMask);

                    UnityEngine.GameObject go = UnityEngine.GameObject.Find("ResourceHolder");
                    if (go != null)
                    {
                        XOCp_ResourceHolder xocp = go.GetComponent<XOCp_ResourceHolder>();
                        if (xocp != null && xocp.m_ParticleArray.Length > 0)
                        {
                            UnityEngine.GameObject temp = null;
                            for (int i = 0; i < xocp.m_ParticleArray.Length; i++)
                            {
                                temp = xocp.m_ParticleArray[i];
                                if (temp != null)
                                {
                                    if (temp.layer == layer)
                                    {
                                        if (temp.GetComponent<UnityEngine.ParticleSystem>() != null)
                                        {
                                            temp.GetComponent<UnityEngine.ParticleSystem>().Stop();
                                        }
                                        temp.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            LogicSystem.LogFromGfx("MainCamera.OnLevelWasLoaded throw exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void DebugDrawStuff()
    {
        Debug.DrawLine(m_Target.position, m_Target.position + m_HeadOffset);
    }

    private float AngleDistance(float a, float b)
    {
        a = UnityEngine.Mathf.Repeat(a, 360);
        b = UnityEngine.Mathf.Repeat(b, 360);

        return UnityEngine.Mathf.Abs(b - a);
    }

    public void Apply()
    {
        if (!m_IsFollowEnable || null == UnityEngine.Camera.main || !UnityEngine.Camera.main.enabled)
        {
            return;
        }
        if (null == m_CameraTransform)
        {
            m_CameraTransform = UnityEngine.Camera.main.transform;
        }
        SetUpPosition();

        UnityEngine.Vector3 targetCenter = m_CurTargetPos + m_CenterOffset;
        UnityEngine.Vector3 targetHead = m_CurTargetPos + m_HeadOffset;

        // Always look at the target	
        SetUpRotation(targetCenter, targetHead);
    }

    public void SetUpPosition()
    {
        if (UnityEngine.Time.deltaTime == 0)
        {
            return;
        }
        AdjustSpeedAndMoveTarget();

        UnityEngine.Vector3 targetCenter = m_CurTargetPos + m_CenterOffset;
        UnityEngine.Vector3 targetHead = m_CurTargetPos + m_HeadOffset;
        //DebugDrawStuff();

        // Calculate the current & target rotation angles
        float originalTargetAngle = m_FixedYaw;//m_Target.eulerAngles.y;
        float currentAngle = m_CameraTransform.eulerAngles.y;

        // Adjust real target angle when camera is locked
        float targetAngle = originalTargetAngle;

        // Setup camera roll.
        if (m_FixedRoll > m_MaxCameraAngle)
        {
            m_FixedRoll = m_MaxCameraAngle;
        }
        else if (m_FixedRoll < m_MinCameraAngle)
        {
            m_FixedRoll = m_MinCameraAngle;
        }
        float originalTargetRollAngle = m_FixedRoll;
        float currentRollAngle = m_CameraTransform.eulerAngles.x;
        float targetRollAngle = originalTargetRollAngle;

        // When pressing Fire2 (alt) the camera will snap to the target direction real quick.
        // It will stop snapping when it reaches the target
        //m_Snap = true;

        if (m_Snap)
        {
            // We are close to the target, so we can stop snapping now!
            if (AngleDistance(currentAngle, originalTargetAngle) < 3.0)
                m_Snap = false;

            currentAngle = targetAngle;// UnityEngine.Mathf.SmoothDampAngle(currentAngle, targetAngle, ref m_AngleVelocity, m_SnapSmoothLag, m_SnapMaxSpeed);
            currentRollAngle = targetRollAngle;// UnityEngine.Mathf.SmoothDampAngle(currentRollAngle, targetRollAngle, ref m_AngleVelocity, m_SnapSmoothLag, m_SnapMaxSpeed);
        }
        // Normal camera motion
        else
        {
            currentAngle = targetAngle;// UnityEngine.Mathf.SmoothDampAngle(currentAngle, targetAngle, ref m_AngleVelocity, m_AngularSmoothLag, m_AngularMaxSpeed);
            currentRollAngle = targetRollAngle;// UnityEngine.Mathf.SmoothDampAngle(currentRollAngle, targetRollAngle, ref m_AngleVelocity, m_AngularSmoothLag, m_AngularMaxSpeed);
        }

        /*
          // When jumping don't move camera upwards but only down!
          if (false)
          {
              // We'd be moving the camera upwards, do that only if it's really high
              float newTargetHeight = targetCenter.y + m_Height;
              if (newTargetHeight < m_TargetHeight || newTargetHeight - m_TargetHeight > 5)
                  m_TargetHeight = targetCenter.y + m_Height;
          }
          // When walking always update the target height
          else*/
        {
            m_TargetHeight = targetCenter.y + m_Height;
        }

        // Damp the height
        //float currentHeight = m_CameraTransform.position.y;
        //currentHeight = UnityEngine.Mathf.SmoothDamp(currentHeight, m_TargetHeight, ref m_HeightVelocity, m_HeightSmoothLag);
        //m_CurDistance = UnityEngine.Mathf.SmoothDamp(m_CurDistance, m_Distance, ref m_DistanceVelocity, m_DistanceSmoothLag);

        // Convert the angle into a rotation, by which we then reposition the camera
        UnityEngine.Quaternion currentRotation = UnityEngine.Quaternion.Euler(currentRollAngle, currentAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        UnityEngine.Vector3 pos = targetCenter;
        pos += currentRotation * UnityEngine.Vector3.back * m_CurDistance;

        // Set the height of the camera
        // pos.y = currentHeight;

        m_CameraTransform.position = pos;
    }

    private void Cut()
    {
        float oldHeightSmooth = m_HeightSmoothLag;
        float oldDistanceSmooth = m_DistanceSmoothLag;
        float oldSnapMaxSpeed = m_SnapMaxSpeed;
        float oldSnapSmooth = m_SnapSmoothLag;

        m_SnapMaxSpeed = 10000;
        m_SnapSmoothLag = 0.001f;
        m_HeightSmoothLag = 0.001f;
        m_DistanceSmoothLag = 0.001f;

        m_Snap = true;
        Apply();

        m_HeightSmoothLag = oldHeightSmooth;
        m_DistanceSmoothLag = oldDistanceSmooth;
        m_SnapMaxSpeed = oldSnapMaxSpeed;
        m_SnapSmoothLag = oldSnapSmooth;
    }

    private void AdjustSpeedAndMoveTarget()
    {
        if (null != m_Target)
        {
            m_TargetPos = m_Target.position;
        }
        float delta = UnityEngine.Time.deltaTime;
        UnityEngine.Vector3 distDir = m_TargetPos - m_CurTargetPos;
        float dist = distDir.magnitude;
        distDir.Normalize();
        m_CameraSpeed = GetCurSpeed(dist);
        UnityEngine.Vector3 motion = distDir * m_CameraSpeed * delta;
        if (motion.magnitude >= dist)
        {
            m_CurTargetPos = m_TargetPos;
            m_CameraSpeed = dist / UnityEngine.Time.deltaTime;
        }
        else
        {
            m_CurTargetPos += motion;
        }
    }

    private void SetUpRotation(UnityEngine.Vector3 centerPos, UnityEngine.Vector3 headPos)
    {
        //height与distance变化时，需要保持lookat目标
        float currentHeight = m_CameraTransform.position.y;
        if (m_NeedLookat)
        {
            if (!Geometry.IsSameFloat(currentHeight, m_TargetHeight) || !Geometry.IsSameFloat(m_CurDistance, m_Distance))
            {
                m_CameraTransform.LookAt(m_CurTargetPos);
            }
            else
            {
                m_NeedLookat = false;
            }
        }
        else
        {
            // Now it's getting hairy. The devil is in the details here, the big issue is jumping of course.
            // * When jumping up and down we don't want to center the guy in screen space.
            //  This is important to give a feel for how high you jump and avoiding large camera movements.
            //   
            // * At the same time we dont want him to ever go out of screen and we want all rotations to be totally smooth.
            //
            // So here is what we will do:
            //
            // 1. We first find the rotation around the y axis. Thus he is always centered on the y-axis
            // 2. When grounded we make him be centered
            // 3. When jumping we keep the camera rotation but rotate the camera to get him back into view if his head is above some threshold
            // 4. When landing we smoothly interpolate towards centering him on screen
            UnityEngine.Vector3 cameraPos = m_CameraTransform.position;
            UnityEngine.Vector3 offsetToCenter = centerPos - cameraPos;

            UnityEngine.Vector3 targetCameraPos = centerPos;
            targetCameraPos.y = cameraPos.y;

            float dist = UnityEngine.Vector3.Distance(cameraPos, targetCameraPos);

            UnityEngine.Vector3 cameraGroundPos = cameraPos;
            cameraGroundPos.y = centerPos.y;

            float height = UnityEngine.Vector3.Distance(cameraPos, cameraGroundPos);


            // Generate base rotation only around y-axis
            UnityEngine.Quaternion yRotation = UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(offsetToCenter.x, 0, offsetToCenter.z));

            UnityEngine.Vector3 relativeOffset = UnityEngine.Vector3.forward * dist + UnityEngine.Vector3.down * height;
            m_CameraTransform.rotation = yRotation * UnityEngine.Quaternion.LookRotation(relativeOffset);

            // Calculate the projected center position and top position in world space
            UnityEngine.Ray centerRay = m_CameraTransform.GetComponent<UnityEngine.Camera>().ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 1));
            UnityEngine.Ray topRay = m_CameraTransform.GetComponent<UnityEngine.Camera>().ViewportPointToRay(new UnityEngine.Vector3(0.5f, m_ClampHeadPositionScreenSpace, 1));

            UnityEngine.Vector3 centerRayPos = centerRay.GetPoint(m_CurDistance);
            UnityEngine.Vector3 topRayPos = topRay.GetPoint(m_CurDistance);

            float centerToTopAngle = UnityEngine.Vector3.Angle(centerRay.direction, topRay.direction);

            float heightToAngle = centerToTopAngle / (centerRayPos.y - topRayPos.y);

            float extraLookAngle = heightToAngle * (centerRayPos.y - centerPos.y);
            if (extraLookAngle < centerToTopAngle)
            {
                extraLookAngle = 0;
            }
            else
            {
                extraLookAngle = extraLookAngle - centerToTopAngle;
                m_CameraTransform.rotation *= UnityEngine.Quaternion.Euler(-extraLookAngle, 0, 0);
            }
        }
    }

    private UnityEngine.Vector3 GetCenterOffset()
    {
        return m_CenterOffset;
    }

    private float GetCurSpeed(float distance)
    {
        float result = m_MinSpeed;
        if (distance > m_MaxSpeedDistance)
        {
            return m_MaxSpeed;
        }
        if (distance < m_MinSpeedDistance)
        {
            return m_MinSpeed;
        }
        result = m_FactorA * UnityEngine.Mathf.Pow(distance, m_Power) + m_FactorB;
        return result;
    }

    private void ComputeSpeedFactor()
    {
        //a*min_distance^n + b = min_speed
        //a*max_distance^n + b = max_speed
        float denominator = UnityEngine.Mathf.Pow(m_MaxSpeedDistance, m_Power) - UnityEngine.Mathf.Pow(m_MinSpeedDistance, m_Power);
        if (denominator != 0)
        {
            m_FactorA = (m_MaxSpeed - m_MinSpeed) / denominator;
        }
        else
        {
            m_FactorA = 0;
        }
        m_FactorB = m_MinSpeed - m_FactorA * UnityEngine.Mathf.Pow(m_MinSpeedDistance, m_Power);
    }

    void On_JoystickMoveStart(MovingJoystick move)
    {
        m_JoystickOperation = true;
    }
    void On_JoystickMoveEnd(MovingJoystick move)
    {
        m_JoystickOperation = false;
    }

    void TraceFingers()
    {
        if (m_JoystickOperation)
        {
            return;
        }

#if UNITY_EDITOR
        // Zoom Camera and keep the distance between [minDistance, maxDistance].
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0)
        {
            m_CurDistance -= Time.deltaTime * m_ZoomSpeed;
            if (m_CurDistance < m_MinDistance)
            {
                m_CurDistance = m_MinDistance;
            }
        }
        else if (mw < 0)
        {
            m_CurDistance += Time.deltaTime * m_ZoomSpeed;
            if (m_CurDistance > m_MaxDistance)
            {
                m_CurDistance = m_MaxDistance;
            }
        }
#endif

        TouchManager.FingerList touches = (TouchManager.FingerList)TouchManager.Touches;

        if (touches.Count == 1 && !m_InWatchMode)
        {
            TouchManager.Finger finger = touches[0];
            if (finger.IsMoving)
            {
                float xScaleFactor = UnityEngine.Mathf.Abs(finger.DeltaPosition.x) / UnityEngine.Mathf.Abs(finger.DeltaPosition.y);
                xScaleFactor = xScaleFactor > 1.0f ? 1.0f : xScaleFactor;
                float yScaleFactor = UnityEngine.Mathf.Abs(finger.DeltaPosition.y) / UnityEngine.Mathf.Abs(finger.DeltaPosition.x);
                yScaleFactor = yScaleFactor > 1.0f ? 1.0f : yScaleFactor;

                if (finger.DeltaPosition.x > 0)
                {
                    // Rotate Left
                    m_FixedYaw = m_CameraTransform.eulerAngles.y + m_AngularMaxSpeed * Time.deltaTime * xScaleFactor;
                }
                else if (finger.DeltaPosition.x < 0)
                {
                    // Rotate Right
                    m_FixedYaw = m_CameraTransform.eulerAngles.y - m_AngularMaxSpeed * Time.deltaTime * xScaleFactor;
                }

                if (finger.DeltaPosition.y > 0)
                {
                    // Rotate Down
                    float camXAngle = m_CameraTransform.eulerAngles.x - m_AngularMaxSpeed * Time.deltaTime * yScaleFactor;
                    if (camXAngle < m_MinCameraAngle)
                    {
                        camXAngle = m_MinCameraAngle;
                    }
                    m_FixedRoll = camXAngle;
                }
                else if (finger.DeltaPosition.y < 0)
                {
                    // Rotate Up
                    float camXAngle = m_CameraTransform.eulerAngles.x + m_AngularMaxSpeed * Time.deltaTime * yScaleFactor;
                    if (camXAngle > m_MaxCameraAngle)
                    {
                        camXAngle = m_MaxCameraAngle;
                    }
                    m_FixedRoll = camXAngle;
                }
            }
        }
        else if (touches.Count == 2)
        {
            TouchManager.Finger finger1 = touches[0];
            TouchManager.Finger finger2 = touches[1];

            if ((finger1.IsDown && !finger1.WasDown) || (finger2.IsDown && !finger2.WasDown))
            {
                m_FingerDistance = UnityEngine.Vector2.Distance(finger1.StartPosition, finger2.StartPosition);
            }

            if (finger1.IsMoving || finger2.IsMoving)
            {
                float currentDistance = UnityEngine.Vector2.Distance(finger1.Position, finger2.Position);
                if (currentDistance > m_FingerDistance)
                {
                    // Zoom In
                    m_CurDistance -= Time.deltaTime * m_ZoomSpeed;
                    if (m_CurDistance < m_MinDistance)
                    {
                        m_CurDistance = m_MinDistance;
                    }
                }
                else if (currentDistance < m_FingerDistance)
                {
                    // Zoom Out
                    m_CurDistance += Time.deltaTime * m_ZoomSpeed;
                    if (m_CurDistance > m_MaxDistance)
                    {
                        m_CurDistance = m_MaxDistance;
                    }
                }
                m_FingerDistance = currentDistance;
            }
        }
    }

    // The distance in the x-z plane to the target
    public float m_Distance = 7.0f;
    // the height we want the camera to be above the target
    public float m_Height = 6.0f;

    public float m_MaxSpeedDistance = 50;
    public float m_MinSpeedDistance = 0.5f;
    public float m_MaxSpeed = 50;
    public float m_MinSpeed = 20;
    public int m_Power = 1;
    public float m_CameraFollowSpeed = 10.0f;
    public float m_CameraSpeed;

    private bool m_IsFollow = false;
    private bool m_IsShaking = false;
    private bool m_NeedLookat = false;

    private float m_FixedYaw_ = 0;
    private float m_FixedYaw
    {
        get { return m_FixedYaw_; }
        set
        {
            m_FixedYaw_ = value; 
            // notify player controller for camera yaw offset
            PlayerControl.Instance.SetCameraYawOffset(m_FixedYaw / 180.0f * UnityEngine.Mathf.PI);
        }
    }

    private float m_FixedRoll_ = 0;
    private float m_FixedRoll
    {
        get { return m_FixedRoll_; }
        set { m_FixedRoll_ = value; }
    }

    private UnityEngine.Transform m_CameraTransform;
    private UnityEngine.Transform m_Target;
    private UnityEngine.Vector3 m_CurTargetPos;
    private UnityEngine.Vector3 m_TargetPos;
    private float m_CurDistance;

    private float m_FactorA;
    private float m_FactorB;
    private bool m_IsFollowEnable = true;
    private float m_OrigMaxSpeedDistance;
    private float m_OrigMinSpeedDistance;
    private float m_OrigMaxSpeed;
    private float m_OrigMinSpeed;
    private int m_OrigPower;
    private float m_OrigHeight;
    private float m_OrigDistance;

    private float m_HeightSmoothLag = 0.3f;
    private float m_DistanceSmoothLag = 3.0f;
    private float m_AngularSmoothLag = 0.3f;
    private float m_AngularMaxSpeed = 100.0f;
    private float m_SnapSmoothLag = 0.2f;
    private float m_SnapMaxSpeed = 720.0f;
    private float m_ClampHeadPositionScreenSpace = 0.75f;
    private UnityEngine.Vector3 m_HeadOffset = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 m_CenterOffset = UnityEngine.Vector3.zero;
    private float m_HeightVelocity = 0.0f;
    private float m_AngleVelocity = 0.0f;
    private float m_DistanceVelocity = 0.0f;
    private bool m_Snap = false;
    private float m_TargetHeight = 100000.0f;
    private int m_CurTargetId;

    private bool m_JoystickOperation = false;
    private float m_FingerDistance = 0.0f;
    private float m_MinCameraAngle = 10.0f;
    private float m_MaxCameraAngle = 80.0f;
    private float m_MaxDistance = 30.0f;
    private float m_MinDistance = 3.0f;
    private float m_ZoomSpeed = 50.0f;

    // Is camera slipping or not.
    private bool m_CameraSlipping = false;
    // Is camera slipping in?
    private bool m_CameraSlippingIn = false;
    // If camera stop after slipping in, the camera is in WATCH MODE.
    private bool m_InWatchMode = false;
    // Once camera distance lower than this value, auto slip the camera close and face to character. 
    private float m_SlipInDistance = 5.0f;
    // Once camera distance great than this value, auto slip the camera far away from character and back to the original angle.
    private float m_SlipOutDistance = 4.0f;
    // Original yaw and roll angle as slip-in occured.
    private float m_SlipOriginalYaw = 0.0f;
    private float m_SlipOriginalRoll = 0.0f;
    private float m_SlipOriginalDistance = 0.0f;
    // Destination angle of slip in / out.
    private float m_SlipDestYawAngle = 0.0f;
    private float m_SlipDestRollAngle = 0.0f;
    private float m_SlipDestDistance = 5.5f;
    //
    private float m_SlipYawSpeed = 0.0f;
    private float m_SlipRollSpeed = 0.0f;
    private float m_SlipZoomSpeed = 0.0f;
    // Camera slipping must be finished in the slip time.
    private float m_SlipTime = 1.0f;

    public static UnityEngine.Vector3 CameraOriginalPosition = UnityEngine.Vector3.zero;
    public static UnityEngine.Quaternion CameraOriginalRotation = UnityEngine.Quaternion.identity;

    private bool m_IsInBattleScene = false;
}
