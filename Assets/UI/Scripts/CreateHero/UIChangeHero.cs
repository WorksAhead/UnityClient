using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;
using System.Text;

public class UIChangeHero : UnityEngine.MonoBehaviour
{
    /// Publics
    
    // camera follow initial transform
    public UnityEngine.Quaternion CameraQuatenion = new UnityEngine.Quaternion(0, 0, 0, 0);
    public UnityEngine.Vector3 OffsetPlayer = new UnityEngine.Vector3();
    public float m_AngleVelocity = 4.0f;

    // move camera to target delta
    public float MoveCameraDelta = 0.2f;
    // name of hero
    public string Hero_Jianshi = "";
    public string Hero_Cike = "";
    // audio of hero
    public UnityEngine.AudioSource m_AudioSource;
    public UnityEngine.AudioSource m_WeaponAudioSource;
    public List<UnityEngine.AudioClip> m_CikeSelectMusic = new List<UnityEngine.AudioClip>();
    public float m_CikeWeaponMusicDelay = 0.0f;
    public List<UnityEngine.AudioClip> m_CikeWeaponMusic = new List<UnityEngine.AudioClip>();
    public List<UnityEngine.AudioClip> m_JianshiSelectMusic = new List<UnityEngine.AudioClip>();
    public float m_JianshiWeaponMusicDelay = 0.0f;
    public List<UnityEngine.AudioClip> m_JianshiWeaponMusic = new List<UnityEngine.AudioClip>();
    // hero ui collider
    public UnityEngine.GameObject m_HeroCollider;
    // hero object
    public UnityEngine.GameObject m_HeroCike;
    public UnityEngine.GameObject m_WeaponCikeLeft;
    public UnityEngine.GameObject m_WeaponCikeRight;
    public UnityEngine.GameObject m_HeroJianshi;
    public UnityEngine.GameObject m_WeaponJianshi;
    // mount points
    public UnityEngine.Transform m_CikeHandLeft;
    public UnityEngine.Transform m_CikeHandRight;
    public UnityEngine.Transform m_CikeBackLeft;
    public UnityEngine.Transform m_CikeBackRight;
    public UnityEngine.Transform m_JianshiHand;
    public UnityEngine.Transform m_JianshiBack;
    // delay of weapon change
    public float m_CikeChangeWeaponDelay = 2.0f;
    public float m_JianshiChangeWeaponDelay = 2.0f;
    // default animation of hero
    public string m_HeroCikeAnim = "";
    public string m_HeroCikeIdleAnim = "";
    public string m_HeroJianshiAnim = "";
    public string m_HeroJianshiIdleAnim = "";
    // ui root object
    public UIRoot UIRootGO = null;
    

    /// Privates
    
    // initial rotation
    private UnityEngine.Quaternion m_CikeInitRotation = UnityEngine.Quaternion.identity;
    private UnityEngine.Quaternion m_JianshiInitRotation = UnityEngine.Quaternion.identity;
    // current visible hero id
    private int m_CurHeroId = 0;
    // for event system
    private List<object> eventlist = new List<object>();
    // button states
    private int m_characterIndex = 4;
    private int signforbuttonpress = 0;
    private bool signforcreate = false;
    private int lastselectbut = 0;
    // cached guid
    private List<ulong> playguidlist = new List<ulong>();
    private List<int> heroidlist = new List<int>();
    // for finger gusture
    private UnityEngine.Vector2 m_LastFingerPos = UnityEngine.Vector2.zero;
    // max hero number
    private static int HeroCount = 4;
    // for nick name from server
    private int m_NicknameCount = 0;
    private List<string> m_NicknameList = new List<string>();

    private enum HeroIdEnum
    {
        WARRIOR = 1,
        MAGICA = 2,
    }
    internal enum RoleEnterResult
    {
        Success = 0,
        Wait,
        UnknownError,
    }
    public void UnSubscribe()
    {
        try
        {
            if (eventlist != null)
            {
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventlist[i]);
                    }
                }
                eventlist.Clear();
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Start()
    {
        try
        {
            // register events
            if (eventlist != null)
            {
                eventlist.Clear();
            }
            object eo = LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_create_hero_scene", "ui", SetSceneAndLoadHero);
            if (eo != null)
            {
                eventlist.Add(eo);
            }
            eo = LogicSystem.EventChannelForGfx.Subscribe<List<string>>("ge_nickname_result", "lobby", OnReceiveNicknames);
            if (eo != null)
            {
                eventlist.Add(eo);
            }
            eo = LogicSystem.EventChannelForGfx.Subscribe<string, int, string, ulong, int, string>("ge_role_enter_log", "log", OnRoleEnterLog);
            if (eo != null)
            {
                eventlist.Add(eo);
            }
            eo = LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_createhero_result", "lobby", CreateHeroResult);
            if (eo != null)
            {
                eventlist.Add(eo);
            }
            eo = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_enter_result", "lobby", OnRoleEnterResult);
            if (eo != null)
            {
                eventlist.Add(eo);
            }
            
            MoveCharacterFromPrefabToScene();

            // change hero introduce text on selection ui
            ChangeHeroIntroduce(0);

            // hide hero selection ui
            SetSelectionSceneVisible(false);

            // register touch event handler
            TouchManager.OnFingerEvent += OnFingerEvent;

            // backup initial transform
            m_JianshiInitRotation = m_HeroJianshi.transform.rotation;
            m_CikeInitRotation = m_HeroCike.transform.rotation;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void MoveCharacterFromPrefabToScene()
    {
        var holder = GameObject.Find("CharacterHolder");
        if (holder != null)
        {
            if (m_HeroCike != null)
            {
                m_HeroCike.transform.SetParent(null);
                m_HeroCike.transform.transform.position = holder.transform.position;
                m_HeroCike.transform.transform.localPosition = holder.transform.localPosition;
                m_HeroCike.transform.transform.rotation = holder.transform.rotation;
                m_HeroCike.transform.transform.localRotation = holder.transform.localRotation;
                m_HeroCike.transform.transform.localScale = holder.transform.localScale;
                m_HeroCike.transform.SetLayer(0);
            }
            if (m_HeroJianshi != null)
            {
                m_HeroJianshi.transform.SetParent(null);
                m_HeroJianshi.transform.transform.position = holder.transform.position;
                m_HeroJianshi.transform.transform.localPosition = holder.transform.localPosition;
                m_HeroJianshi.transform.transform.rotation = holder.transform.rotation;
                m_HeroJianshi.transform.transform.localRotation = holder.transform.localRotation;
                m_HeroJianshi.transform.transform.localScale = holder.transform.localScale;//new UnityEngine.Vector3(2, 2, 2);
                //m_HeroJianshi.transform.transform.Rotate(new UnityEngine.Vector3(0, 180, 0), Space.Self);
                m_HeroJianshi.transform.SetLayer(0);
            }
        }
    }

    private void OnFingerEvent(FingerEvent args)
    {
        FingerMotionEvent motionEventArgs = args as FingerMotionEvent;
        if (null != motionEventArgs)
        {
            // ui collider clicked
            if (UICamera.hoveredObject == m_HeroCollider)
            {
                // only rotate on finger down
                if (motionEventArgs.Phase == FingerMotionPhase.Updated)
                {
                    // rotate left or right
                    if (motionEventArgs.Position.x - m_LastFingerPos.x > 0.5)
                    {
                        OnHeroRotate(m_CurHeroId, -1);
                    }
                    else if (motionEventArgs.Position.x - m_LastFingerPos.x < -0.5)
                    {
                        OnHeroRotate(m_CurHeroId, 1);
                    }
                    else { }
                }
                m_LastFingerPos = motionEventArgs.Position;
            }
        }
    }

    private void OnHeroRotate(int heroId, int clockwise)
    {
        float angle = (float)(m_AngleVelocity / UnityEngine.Mathf.PI * 180.0f * UnityEngine.Time.deltaTime * clockwise);
        switch (heroId)
        {
            case (int)HeroIdEnum.MAGICA:
                m_HeroCike.transform.Rotate(UnityEngine.Vector3.up, angle, Space.Self);
                break;
            case (int)HeroIdEnum.WARRIOR:
                m_HeroJianshi.transform.Rotate(UnityEngine.Vector3.up, angle, Space.Self);
                break;
        }
    }
    
    private string GetHeroNameByHeroId(int heroId)
    {
        switch (heroId)
        {
            case 1:
                return Hero_Jianshi;
            case 2:
                return Hero_Cike;
            default:
                return Hero_Jianshi;
        }
    }

    // FixMe: another way to get active hero game object
    void CameraLookAtHero(int heroId)
    {
        return;
        string playerName = GetHeroNameByHeroId(heroId);

        // find all player in layers
        UnityEngine.GameObject[] playersArr = UnityEngine.GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playersArr.Length; ++i)
        {
            if (playersArr[i] != null && playersArr[i].name.ToLower() == playerName.ToLower())
            {
                UnityEngine.GameObject go = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
                if (go != null)
                {
                    MainCamera cameraScript = go.GetComponent<MainCamera>();
                    if (cameraScript != null)
                    {
                        // follow target hero
                        UnityEngine.GameObject play = playersArr[i];
                        cameraScript.CameraFollowGameObject(play, OffsetPlayer, CameraQuatenion);

                    }
                }
            }
        }
    }

    private void ShowHeroAndDoAction(int heroId)
    {
        // show only selected hero
        SetOnlyHeroVisible(heroId);
        // cache hero id
        m_CurHeroId = heroId;
        // initailize weapon mount position
        InitWeaponMountPos(heroId);

        // play music
        PlaySelectMusicByHeroId(heroId);
        PlayWeaponMusicByHeroId(heroId);

        // stop camera moving coroutine if not finished
        StopAllCoroutines();
        switch (heroId)
        {
            case (int)HeroIdEnum.MAGICA:
                // play queued animation
                HeroPlayAnimation(m_HeroCike, GetAnimNameByHeroId(heroId));
                HeroPlayAniationQueued(m_HeroCike, GetIdleAnimByHeroId(heroId));
                // change weapon queued
                StartCoroutine(DelayChangeWeaponPos(m_HeroCike, m_WeaponCikeLeft, m_CikeBackLeft, m_CikeChangeWeaponDelay));
                StartCoroutine(DelayChangeWeaponPos(m_HeroCike, m_WeaponCikeRight, m_CikeBackRight, m_CikeChangeWeaponDelay));
                // set initial rotation
                m_HeroCike.transform.rotation = m_CikeInitRotation;
                break;
            case (int)HeroIdEnum.WARRIOR:
                // play queued animation
                HeroPlayAnimation(m_HeroJianshi, GetAnimNameByHeroId(heroId));
                HeroPlayAniationQueued(m_HeroJianshi, GetIdleAnimByHeroId(heroId));
                // change weapon queued
                StartCoroutine(DelayChangeWeaponPos(m_HeroJianshi, m_WeaponJianshi, m_JianshiBack, m_JianshiChangeWeaponDelay));
                // set initial rotation
                m_HeroJianshi.transform.rotation = m_JianshiInitRotation;
                break;
        }
    }
    private void PlaySelectMusicByHeroId(int heroId)
    {
        switch (heroId)
        {
            case (int)HeroIdEnum.MAGICA:
                if (null != m_AudioSource && m_CikeSelectMusic.Count > 0)
                {
                    m_AudioSource.clip = m_CikeSelectMusic[UnityEngine.Random.Range(0, m_CikeSelectMusic.Count)];
                    m_AudioSource.Play();
                }
                break;
            case (int)HeroIdEnum.WARRIOR:
                if (null != m_AudioSource && m_JianshiSelectMusic.Count > 0)
                {
                    m_AudioSource.clip = m_JianshiSelectMusic[UnityEngine.Random.Range(0, m_JianshiSelectMusic.Count)];
                    m_AudioSource.Play();
                }
                break;
        }
    }
    private void PlayWeaponMusicByHeroId(int heroId)
    {
        switch (heroId)
        {
            case (int)HeroIdEnum.MAGICA:
                if (null != m_WeaponAudioSource && m_CikeWeaponMusic.Count > 0)
                {
                    m_WeaponAudioSource.clip = m_CikeWeaponMusic[UnityEngine.Random.Range(0, m_CikeWeaponMusic.Count)];
                    m_WeaponAudioSource.Play();
                }
                break;
            case (int)HeroIdEnum.WARRIOR:
                if (null != m_WeaponAudioSource && m_JianshiWeaponMusic.Count > 0)
                {
                    m_WeaponAudioSource.clip = m_JianshiWeaponMusic[UnityEngine.Random.Range(0, m_JianshiWeaponMusic.Count)];
                    m_WeaponAudioSource.Play();
                }
                break;
        }
    }
    private void HeroPlayAnimation(UnityEngine.GameObject obj, string animName)
    {
        if (null != obj && !String.IsNullOrEmpty(animName) && obj.GetComponent<UnityEngine.Animation>() != null)
        {
            if (obj.GetComponent<UnityEngine.Animation>().IsPlaying(animName))
            {
                obj.GetComponent<UnityEngine.Animation>().Stop(animName);
            }
            obj.GetComponent<UnityEngine.Animation>().Play(animName);
        }
    }
    private void HeroPlayAniationQueued(UnityEngine.GameObject obj, string animName)
    {
        if (null != obj && !String.IsNullOrEmpty(animName) && obj.GetComponent<UnityEngine.Animation>())
        {
            obj.GetComponent<UnityEngine.Animation>().PlayQueued(animName);
        }
    }
    IEnumerator DelayChangeWeaponPos(UnityEngine.GameObject hero, UnityEngine.GameObject weapon, UnityEngine.Transform parent, float delay)
    {
        yield return new WaitForSeconds(delay);
        try
        {
            if (null != hero && null != weapon && null != parent)
            {
                weapon.transform.parent = parent;
                weapon.transform.localPosition = UnityEngine.Vector3.zero;
                weapon.transform.localRotation = UnityEngine.Quaternion.identity;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void SetHeroVisibleById(int heroId, bool visible)
    {
        switch (heroId)
        {
            case (int)HeroIdEnum.WARRIOR:
                m_HeroJianshi.SetActive(visible);
                break;
            case (int)HeroIdEnum.MAGICA:
                m_HeroCike.SetActive(visible);
                break;
        }
    }
    private void SetHeroVisibleFalse()
    {
        m_HeroJianshi.SetActive(false);
        m_HeroCike.SetActive(false);
    }
    private void SetOnlyHeroVisible(int onlyHeroId)
    {
        foreach (int heroId in Enum.GetValues(typeof(HeroIdEnum)))
        {
            if (onlyHeroId != heroId)
            {
                SetHeroVisibleById(heroId, false);
            }
            else
            {
                SetHeroVisibleById(heroId, true);
            }
        }
    }
    private void InitWeaponMountPos(int heroId)
    {
        switch (heroId)
        {
            case (int)HeroIdEnum.MAGICA:
                if (m_CikeHandLeft == null || m_CikeHandRight == null) return;
                m_WeaponCikeLeft.transform.parent = m_CikeHandLeft;
                m_WeaponCikeLeft.transform.localPosition = UnityEngine.Vector3.zero;
                m_WeaponCikeLeft.transform.localRotation = UnityEngine.Quaternion.identity;
                m_WeaponCikeRight.transform.parent = m_CikeHandRight;
                m_WeaponCikeRight.transform.localPosition = UnityEngine.Vector3.zero;
                m_WeaponCikeRight.transform.localRotation = UnityEngine.Quaternion.identity;
                break;
            case (int)HeroIdEnum.WARRIOR:
                m_WeaponJianshi.transform.parent = m_JianshiHand;
                m_WeaponJianshi.transform.localPosition = UnityEngine.Vector3.zero;
                m_WeaponJianshi.transform.localRotation = UnityEngine.Quaternion.identity;
                break;
        }
    }
    private string GetAnimNameByHeroId(int heroId)
    {
        string result = "";
        switch (heroId)
        {
            case (int)HeroIdEnum.MAGICA:
                result = m_HeroCikeAnim;
                break;
            case (int)HeroIdEnum.WARRIOR:
                result = m_HeroJianshiAnim;
                break;
        }
        return result;
    }
    private string GetIdleAnimByHeroId(int heroId)
    {
        string result = "";
        switch (heroId)
        {
            case (int)HeroIdEnum.MAGICA:
                result = m_HeroCikeIdleAnim;
                break;
            case (int)HeroIdEnum.WARRIOR:
                result = m_HeroJianshiIdleAnim;
                break;
        }
        return result;
    }

    // FixMe: another way to get active hero game object
    public void MoveCameraToHero(int heroId)
    {
        return;
        string name = GetHeroNameByHeroId(heroId);
        UnityEngine.GameObject[] playersArr = UnityEngine.GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playersArr.Length; ++i)
        {
            if (playersArr[i] != null && playersArr[i].name.ToLower() == name.ToLower())
            {
                UnityEngine.GameObject go = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
                if (go != null)
                {
                    MainCamera cameraScript = go.GetComponent<MainCamera>();
                    if (cameraScript != null)
                    {
                        // move camera with delta
                        UnityEngine.GameObject play = playersArr[i];
                        StartCoroutine(cameraScript.MoveCamera(play.transform.position, OffsetPlayer, MoveCameraDelta, CameraQuatenion));

                    }
                }
            }
        }
    }
    void SetSelectionSceneVisible(bool vis)
    {
        NGUITools.SetActive(UIRootGO.gameObject, vis);
        NGUITools.SetActive(gameObject, vis);
    }

    // called after login clicked
    void SetSceneAndLoadHero(bool vis)
    {
        try
        {
            // close other window
            UIManager.Instance.HideWindowByName("Dialog");
            UIManager.Instance.HideWindowByName("ServerSelect");

            // show character selection scene
            SetSelectionSceneVisible(vis);

            // 
            OkToLoadHero(LobbyClient.Instance.AccountInfo.Players);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnDestroy()
    {
        try
        {
            UnSubscribe();
            TouchManager.OnFingerEvent -= OnFingerEvent;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OkToLoadHero(List<ArkCrossEngine.RoleInfo> playlist)
    {
        NormLog.Instance.Record(GameEventCode.ShowHeroUI);

        // clear cached guid list
        if (playguidlist != null && heroidlist != null)
        {
            heroidlist.Clear();
            playguidlist.Clear();
        }

        // reset camera to zero position
        //UnityEngine.Camera.main.transform.position = UnityEngine.Vector3.zero;
        if (playlist != null && playlist.Count > 0)
        {
            int i = playlist.Count;
            i = (i <= 3 ? i : 3);
            for (int j = 0; j < i; ++j)
            {
                ArkCrossEngine.RoleInfo pi = playlist[j];
                if (pi != null)
                {
                    // set hero level and nick name
                    SetHeroInfo(j, pi);

                    // cache guid
                    if (playguidlist != null)
                    {
                        playguidlist.Add(pi.Guid);
                    }
                    if (heroidlist != null)
                    {
                        heroidlist.Add(pi.HeroId);
                    }
                }
            }

            // enable hero selection panel
            if (i < 3)
            {
                UnityEngine.Transform tf = UIRootGO.transform.Find("SelectHeroPanel");
                if (tf != null)
                {
                    UnityEngine.Transform tfc = tf.Find("SelectHero" + i);
                    if (tfc != null)
                    {
                        NGUITools.SetActive(tfc.gameObject, true);
                    }
                }
            }

            // show default hero
            RoleInfo playerzero = playlist[0];
            if (playerzero != null)
            {
                // set hero description
                ChangeHeroIntroduce(playerzero.HeroId);
                // default hero id
                m_characterIndex = playerzero.HeroId;
                // camera follow default camera
                SetHeroVisible(m_characterIndex, true);
                // move camera to target with coroutine
                MoveCameraToHero(playerzero.HeroId);
                // show default hero and play initial animation & sounds etc.
                ShowHeroAndDoAction(playerzero.HeroId);
            }

            // default active character button 0
            WitchButtonPress(0);
        }
        // null rolelist, may network exception or first login
        else
        {
            // default description
            ChangeHeroIntroduce(0);
            m_characterIndex = 0;
            // show default character
            SetHeroVisible(m_characterIndex, true);
            // 
            SelectHero0();
            MoveCameraToHero(1);
        }
    }
    private void SetHeroInfo(int num, ArkCrossEngine.RoleInfo pi)
    {
        if (num < 0 || num > 3) return;

        UnityEngine.Transform tf = UIRootGO.transform.Find("SelectHeroPanel/SelectHero" + num);
        if (tf != null)
        {
            UISprite us = tf.gameObject.GetComponent<UISprite>();
            if (us != null)
            {
                us.color = UnityEngine.Color.white;
            }
            NGUITools.SetActive(tf.gameObject, true);
            tf = tf.transform.Find("Back");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
                tf = tf.Find("Label");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = "[i]Lv." + pi.Level + "\n" + pi.Nickname + "[-]";
                    }
                }
            }
        }
        tf = UIRootGO.transform.Find("SelectHeroPanel/SelectHero" + num + "/Back/Head");
        if (tf != null)
        {
            UISprite us = tf.gameObject.GetComponent<UISprite>();
            if (us != null)
            {
                //us.spriteName = pi.HeroId == 1 ? "kuang-zhan-shi-tou-xiang2" : pi.HeroId == 2 ? "ci-ke-tou-xiang2" : "";
                us.spriteName = "jue-se-xuan-zhong-kuang";
            }
        }
        tf = UIRootGO.transform.Find("SelectHeroPanel/SelectHero" + num + "/Sprite");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
    }
    
    public void EnterGame()
    {
        // we want to create hero
        if (signforcreate)
        {
            if (UIRootGO != null)
            {
                UnityEngine.Transform tf = UIRootGO.transform.Find("YesOrNot/Sprite/ChatInput/Back/Label");
                if (tf != null && tf.transform != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = "";
                    }
                }
                tf = UIRootGO.transform.Find("YesOrNot/Sprite/ChatInput/Back");
                if (tf != null)
                {
                    UIInput ui = tf.gameObject.GetComponent<UIInput>();
                    if (ui != null)
                    {
                        ui.value = "";
                    }
                }
            }

            // display hint text
            ChooseYesOrNoVisible(StrDictionaryProvider.Instance.GetDictString(135), true);

            // notify nickname creation
            LogicSystem.PublishLogicEvent("ge_create_nickname", "lobby");
        }
        // enter game
        else
        {
            // hide hero
            SetHeroVisibleById(m_CurHeroId, false);
            // enter game scene
            LogicSystem.PublishLogicEvent("ge_role_enter", "lobby", signforbuttonpress);
            LogicSystem.EventChannelForGfx.Publish("ge_connect_hint", "ui", UIConnectEnumType.RoleEnter, true, 20.0f);
        }
    }
    private void ActionCreateHeroFailure()
    {
        
    }

    public void ReturnLogin()
    {
        if (LobbyClient.Instance.AccountInfo.Players == null || LobbyClient.Instance.AccountInfo.Players.Count == 0)
        {
            SetHeroVisibleFalse();
        }
        else
        {
            SetHeroVisibleFalse();
            switch (lastselectbut)
            {
                case 0:
                    SelectHero0();
                    break;
                case 1:
                    SelectHero1();
                    break;
                case 2:
                    SelectHero2();
                    break;
            }
        }
        if (signforcreate)
        {
            if (heroidlist != null && heroidlist.Count > lastselectbut)
            {
                ButtonCreateHeroColourScale(0);
                SetHeroVisible(m_characterIndex % HeroCount, false);
                m_characterIndex = heroidlist[lastselectbut];
                SetHeroVisible(m_characterIndex % HeroCount, true);
                ChangeHeroIntroduce(m_characterIndex % HeroCount);
            }
            WitchButtonPress(lastselectbut);
            ChangeActionShowAbout(false);
        }
        else
        {
            
        }
    }

    private void ChangeHeroIntroduce(int heroid)
    {
        ArkCrossEngine.Data_PlayerConfig dpc = ArkCrossEngine.PlayerConfigProvider.Instance.GetPlayerConfigById(heroid);
        if (heroid >= 0 && UIRootGO != null && dpc != null)
        {
            UnityEngine.Transform tf = UIRootGO.transform.Find("IntroducePanelCopy/Container");
            if (tf != null)
            {
                tf = tf.Find("Sprite/Name");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = "[i]" + dpc.m_Name + "[-]";
                    }
                }
            }
            tf = UIRootGO.transform.Find("IntroducePanelCopy/Container");
            if (tf != null)
            {
                tf = tf.Find("Bula");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = dpc.m_HeroIntroduce2.Replace("\\n", "\n");
                    }
                }
            }
            tf = UIRootGO.transform.Find("IntroducePanelCopy/Container");
            if (tf != null)
            {
                tf = tf.Find("Introduce");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = dpc.m_HeroIntroduce1.Replace("\\n", "\n");
                    }
                }
            }
            tf = UIRootGO.transform.Find("IntroducePanelCopy/Container");
            if (tf != null)
            {
                tf = tf.Find("Sprite/Back/Head");
                if (tf != null)
                {
                    UISprite us = tf.gameObject.GetComponent<UISprite>();
                    if (us != null)
                    {
                        // us.spriteName = heroid == 1 ? "kuang-zhan-shi-tou-xiang2" : heroid == 2 ? "ci-ke-tou-xiang2" : "";
                        us.spriteName = "jue-se-xuan-zhong-kuang";
                    }
                }
            }
        }
    }

    private void SetHeroVisible(int id, bool vis)
    {
        if (vis == true)
        {
            CameraLookAtHero(id);
        }
    }
    public void CreateHeroButton()
    {
        if (true)
        {

        }
    }

    private void ButtonCreateHeroColourScale(int newnum)
    {
        /*
        if (UIRootGO != null)
        {
            UnityEngine.Transform tf = UIRootGO.transform.Find("ButtonCreateHero/" + 0 + "/Back");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
            tf = UIRootGO.transform.Find("ButtonCreateHero/" + 1 + "/Back");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
            tf = UIRootGO.transform.Find("ButtonCreateHero/" + (newnum - 1) + "/Back");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
        }
        */
        SetHeroVisible(m_characterIndex, false);
        m_characterIndex = newnum;
        ChangeHeroIntroduce(m_characterIndex);
    }
    
    // button events handler
    public void ButtonCreateHero0()
    {
        ShowHeroAndDoAction((int)HeroIdEnum.WARRIOR);
        ButtonCreateHeroColourScale(1);
    }
    public void ButtonCreateHero1()
    {
        ShowHeroAndDoAction((int)HeroIdEnum.MAGICA);
        ButtonCreateHeroColourScale(2);
    }
    public void ButtonCreateHero2()
    {
        //ShowHeroAndDoAction((int)HeroIdEnum.MAGIC);
        //ButtonCreateHeroColourScale(3);
    }
    public void ButtonCreateHero3()
    {
        
    }
    public void ButtonForNickName()
    {
        if (m_NicknameCount < m_NicknameList.Count)
        {
            SetHeroNickName(m_NicknameList[m_NicknameCount]);
            m_NicknameCount++;
        }
        else
        {
            LogicSystem.PublishLogicEvent("ge_create_nickname", "lobby");
        }
    }
    public void ButtonDown()
    {
        if (UIRootGO != null)
        {
            UnityEngine.Transform tf = UIRootGO.transform.Find("YesOrNot/Sprite");
            if (tf != null)
            {
                UIAnchor ua = tf.gameObject.GetComponent<UIAnchor>();
                if (ua != null)
                {
                    ua.relativeOffset = new UnityEngine.Vector2(0f, 0f);
                    ua.enabled = true;
                }
            }
        }
    }
    public void SelectHero0()
    {
        // create new hero if null role list
        if (playguidlist.Count < 1)
        {
            CreateHero();
        }
        // active player
        else
        {
            if (heroidlist != null && heroidlist.Count > 0)
            {
                SetHeroVisible(m_characterIndex % HeroCount, false);
                m_characterIndex = heroidlist[0];
                SetHeroVisible(m_characterIndex % HeroCount, true);
                ShowHeroAndDoAction(m_characterIndex);
                ChangeHeroIntroduce(m_characterIndex % HeroCount);
            }
        }
        WitchButtonPress(0);
    }

    public void SelectHero1()
    {
        if (playguidlist.Count < 2)
        {
            CreateHero();
        }
        else
        {
            if (heroidlist != null && heroidlist.Count > 1)
            {
                SetHeroVisible(m_characterIndex % HeroCount, false);
                m_characterIndex = heroidlist[1];
                SetHeroVisible(m_characterIndex % HeroCount, true);
                ShowHeroAndDoAction(m_characterIndex);
                ChangeHeroIntroduce(m_characterIndex % HeroCount);
            }
        }
        WitchButtonPress(1);
    }

    public void SelectHero2()
    {
        if (playguidlist.Count < 3)
        {
            CreateHero();
        }
        else
        {
            if (heroidlist != null && heroidlist.Count > 2)
            {
                SetHeroVisible(m_characterIndex % HeroCount, false);
                m_characterIndex = heroidlist[2];
                SetHeroVisible(m_characterIndex % HeroCount, true);
                ShowHeroAndDoAction(m_characterIndex);
                ChangeHeroIntroduce(m_characterIndex % HeroCount);
            }
        }
        WitchButtonPress(2);
    }

    public void SelectHero3()
    {
        if (playguidlist.Count < 4)
        {
            CreateHero();
        }
        else
        {
            if (heroidlist != null && heroidlist.Count > 3)
            {
                SetHeroVisible(m_characterIndex % HeroCount, false);
                m_characterIndex = heroidlist[3];
                SetHeroVisible(m_characterIndex % HeroCount, true);
                ShowHeroAndDoAction(m_characterIndex);
                ChangeHeroIntroduce(m_characterIndex % HeroCount);
            }
        }
        WitchButtonPress(3);
    }

    public void SelectHero4()
    {
        if (playguidlist.Count < 5)
        {
            CreateHero();
        }
        else
        {
            if (heroidlist != null && heroidlist.Count > 4)
            {
                SetHeroVisible(m_characterIndex % HeroCount, false);
                m_characterIndex = heroidlist[4];
                SetHeroVisible(m_characterIndex % HeroCount, true);
                CameraLookAtHero(m_characterIndex % HeroCount);
                ChangeHeroIntroduce(m_characterIndex % HeroCount);
            }
        }
        WitchButtonPress(4);
    }

    public void LeftButtonClick()
    {
        SetHeroVisible(m_characterIndex-- % HeroCount, false);
        if (m_characterIndex < 0)
        {
            m_characterIndex = HeroCount - 1;
        }
        SetHeroVisible(m_characterIndex % HeroCount, true);
        ChangeHeroIntroduce(m_characterIndex % HeroCount);
    }

    public void RightButtonClick()
    {
        SetHeroVisible(m_characterIndex++ % HeroCount, false);
        SetHeroVisible(m_characterIndex % HeroCount, true);
        ChangeHeroIntroduce(m_characterIndex % HeroCount);
    }

    private void SetHeroNickName(string nickname)
    {
        try
        {
            if (UIRootGO != null && nickname != null)
            {
                UnityEngine.Transform tf = UIRootGO.transform.Find("YesOrNot/Sprite/ChatInput/Back/Label");
                if (tf != null && tf.transform != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = nickname;
                    }
                }
                tf = UIRootGO.transform.Find("YesOrNot/Sprite/ChatInput/Back");
                if (tf != null)
                {
                    UIInput ui = tf.gameObject.GetComponent<UIInput>();
                    if (ui != null)
                    {
                        ui.value = nickname;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnReceiveNicknames(List<string> nicknameList)
    {
        try
        {
            m_NicknameList.Clear();
            m_NicknameList = nicknameList;
            m_NicknameCount = 0;
            if (m_NicknameList.Count >= 1)
            {
                SetHeroNickName(m_NicknameList[m_NicknameCount]);
                m_NicknameCount++;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void Change()
    {
        string str = GetHeroNickName().Trim();
        ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(134), true);
        if (CalculateStringByte(str) > 14)
        {
            ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(362), true);
        }
        else if (CheckIllegalSymbol(str))
        {
            ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(168), true);
        }
    }
    public void Submit()
    {
        if (UIRootGO != null)
        {
            UnityEngine.Transform tf = UIRootGO.transform.Find("YesOrNot/Sprite");
            if (tf != null)
            {
                UIAnchor ua = tf.gameObject.GetComponent<UIAnchor>();
                if (ua != null)
                {
                    ua.relativeOffset = new UnityEngine.Vector2(0f, -0.25f);
                    ua.enabled = true;
                }
            }
        }
    }
    private string GetHeroNickName()
    {
        if (UIRootGO != null)
        {
            UnityEngine.Transform tf = UIRootGO.transform.Find("YesOrNot/Sprite/ChatInput/Back/Label");
            if (tf != null && tf.transform != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    return ul.text;
                }
            }
        }
        return null;
    }

    private void WitchButtonPress(int sign)
    {
        if (sign < 0) return;

        if (UIRootGO != null)
        {
            // disable last pressed button
            UnityEngine.Transform tf = UIRootGO.transform.Find("SelectHeroPanel/SelectHero" + signforbuttonpress);
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    us.width = 400;
                }
                tf = tf.Find("Back/Frame");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
            }
            // enable active button
            tf = UIRootGO.transform.Find("SelectHeroPanel/SelectHero" + sign);
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    us.width = 512;
                }
                tf = tf.Find("Back/Frame");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, true);
                }
            }
        }
        signforbuttonpress = sign;
        NormLog.Instance.Record(GameEventCode.SelectHero);
    }
    private void CreateHero()
    {
        try
        {
            SetHeroVisible(m_characterIndex % HeroCount, false);
            ButtonCreateHero0();
            lastselectbut = signforbuttonpress;
            ButtonCreateHero0();
            ChangeActionShowAbout(true);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void ChangeActionShowAbout(bool actionsign)
    {
        if (UIRootGO == null && UIRootGO.transform != null) return;

        UnityEngine.Transform tf = UIRootGO.transform.Find("ButtonPanel");
        tf = UIRootGO.transform.Find("SelectHeroPanel");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, !actionsign);
        }
        tf = UIRootGO.transform.Find("ChatInput");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, actionsign);
        }
        tf = UIRootGO.transform.Find("ButtonCreateHero");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, actionsign);
        }
        tf = UIRootGO.transform.Find("Title/Sprite");
        if (tf != null)
        {
            UISprite us = tf.gameObject.GetComponent<UISprite>();
            if (us != null)
            {
                if (actionsign)
                {
                    us.spriteName = "chuang-jian-jue-se";
                }
                else
                {
                    us.spriteName = "xuan-ze-jue-se";
                }
            }
        }
        tf = UIRootGO.transform.Find("IntroducePanelCopy");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, actionsign);
        }
        tf = UIRootGO.transform.Find("ScenePanel/ReturnLogin");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, actionsign);
        }

        tf = UIRootGO.transform.Find("ScenePanel");
        UnityEngine.Transform tfc = tf;
        if (tf != null)
        {
            tf = tf.Find("EnterGame");
            if (tf != null)
            {
                tf = tf.Find("Label");
                if (tf != null && tf.gameObject != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        if (actionsign)
                        {
                            ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(133);
                        }
                        else
                        {
                            ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(132);
                        }
                    }
                }
            }
            tf = tfc.Find("ReturnLogin");
            if (tf != null)
            {
                tf = tf.Find("Label");
                if (tf != null && tf.gameObject != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (actionsign)
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(131);
                    }
                    else
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(130);
                    }
                }
            }
        }

        signforcreate = actionsign;
    }

    // callback of nickname creation
    public void Yes()
    {
        string nickname = GetHeroNickName();
        if (nickname == null) return;

        nickname = nickname.Trim();

        // check legal
        if (nickname.Equals(String.Empty))
        {
            ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(129), true);
        }
        else if (CalculateStringByte(nickname) < 3)
        {
            ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(174), true);
        }
        else if (CalculateStringByte(nickname) > 14)
        {
            ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(362), true);
        }
        else if (CheckIllegalSymbol(nickname))
        {
            ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(168), true);
        }
        else
        {
            bool ret = WordFilter.Instance.Check(nickname);
            if (ret == true)
            {
                ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(128), true);
                return;
            }
            NormLog.Instance.Record(GameEventCode.CreateHero);

            // create role impl
            LogicSystem.PublishLogicEvent("ge_create_role", "lobby", m_characterIndex % HeroCount, nickname);
            // display connect hint if possible
            LogicSystem.EventChannelForGfx.Publish("ge_connect_hint", "ui", UIConnectEnumType.RoleEnter, true, 20.0f);
        }
    }
    public void No()
    {
        Submit();
        ChooseYesOrNoVisible(null, false);
    }
    private void ChooseYesOrNoVisible(string str, bool vis)
    {
        if (UIRootGO != null)
        {
            UnityEngine.Transform tf = UIRootGO.transform.Find("YesOrNot");
            if (vis && tf != null)
            {
                if (tf.gameObject != null)
                {
                    NGUITools.SetActive(tf.gameObject, true);
                }
                tf = tf.Find("Sprite");
                if (tf != null)
                {
                    tf = tf.Find("Label");
                    if (tf != null && tf.gameObject != null)
                    {
                        UILabel ul = tf.gameObject.GetComponent<UILabel>();
                        if (ul != null && str != null)
                        {
                            ul.text = str;
                        }
                    }
                }
            }
            else
            {
                if (tf != null && tf.gameObject != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
            }
        }
    }
    private void OnRoleEnterLog(string account, int logicServerId, string nickname, ulong userGuid, int userLevel, string accountId)
    {
        // call back of role enter success
        try
        {
            NormLog.Instance.Record(GameEventCode.Enter);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void CreateHeroResult(bool result)
    {
        try
        {
            LogicSystem.EventChannelForGfx.Publish("ge_connect_hint", "ui", UIConnectEnumType.RoleEnter, false, 0.0f);    //关闭连接提示框
            if (!result)
            {
                CameraLookAtHero(1);
                ChooseYesOrNoVisible(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(127), true);
            }
            else
            {
                NormLog.Instance.Record(GameEventCode.CreateResult);
                SetHeroVisibleById(m_CurHeroId, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnRoleEnterResult(int ret)
    {
        try
        {
            if (UIManager.Instance.GetWindowGoByName("Connect") != null)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_connect_hint", "ui", UIConnectEnumType.RoleEnter, false, 0.0f);
            }
            
            // destroy self if enter success
            if (ret == (int)RoleEnterResult.Success)
            {
                UnSubscribe();
                DestroyImmediate(gameObject);
                NGUITools.DestroyImmediate(UIRootGO);
            }
            // failed
            else if (ret == (int)RoleEnterResult.UnknownError)
            {
                // show ui hint of error
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(42);
                string chn_confirm = StrDictionaryProvider.Instance.GetDictString(4);
                LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, chn_confirm, null, null, null, false);
            }
            else
            {
                // why?
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private int CalculateStringByte(string str)
    {
        if (str.Equals(string.Empty))
            return 0;
        int strlen = 0;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, bytes);
        int count = bytes.Length;
        for (int i = 0; i < count; ++i)
        {
            if (bytes[i] != 0)
            {
                ++strlen;
            }
        }
        return strlen;
    }
    private bool CheckIllegalSymbol(string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;
        string symbolstr = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(169);
        foreach (char forbidsymbol in symbolstr)
        {
            if (str.Contains(forbidsymbol.ToString()))
            {
                return true;
            }
        }
        return false;
    }
    
    void Update()
    {
        try
        {
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.Escape))
            {
                Submit();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}