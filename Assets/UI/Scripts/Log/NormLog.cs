using System;

namespace ArkCrossEngine
{
    public enum GameEventCode : int
    {
        Begin = 101,
        GameStart = 0,
        CheckUpdate = 1,
        CheckResult = 2,
        StartUpdate = 3,
        EndUpdate = 4,
        StartExtract = 5,
        EndExtract = 6,
        LoadAssets = 7,
        EndAssets = 8,
        ServerList = 9,
        StartLogin = 10,
        EndLogin = 11,
        VerifyToken = 12,
        VerifyResult = 15,
        ShowHeroUI = 16,
        SelectHero = 17,
        CreateHero = 18,
        CreateResult = 20,
        Enter = 21,
    }
    public class NormLog
    {
        public GameInfo Info
        {
            get { return info; }
        }
        public class GameInfo
        {
            public string appkey = "null";
            public string skdversion = "null";
            public string system = "null";
            public string gamechannel = "null";
            public string deviceid = "null";
            public string userid = "null";
            public string gameversion = "null";
            public string time = "null";
            public string code = "null";
            public string hardwarename = "null";
            public string cpuusage = "null";
            public string cpubusfrequency = "null";
            public string memorytotal = "null";
            public string memoryusage = "null";
        }
        public void Record(GameEventCode code)
        {
            string action_sign = ((int)code).ToString();
        }
        public void Print(int code)
        {
            string str = Build(code);
            UnityEngine.Debug.Log("NormLog Print : " + str);
            LogSystem.Debug("NormLog Print : {0}", str);
        }
        public void UpdateIpAndChannelAndDeviceid(string ip, string channel, string deviceid)
        {
            LogicSystem.PublishLogicEvent("ge_set_ip_and_channel", "client", ip, channel);
            if (channel.Length > 0)
            {
                info.gamechannel = channel;
            }
            if (deviceid.Length > 0)
            {
                info.deviceid = deviceid;
            }
        }
        public void UpdateDeviceInfo(string name, string cpuusage, string cpubusfrequency, string memorytotal, string memoryusage)
        {
            info.hardwarename = name;
            info.cpuusage = cpuusage.Length > 0 ? cpuusage : "null";
            info.cpubusfrequency = cpubusfrequency.Length > 0 ? cpubusfrequency : "null";
            info.memorytotal = memorytotal.Length > 0 ? memorytotal : "null";
            info.memoryusage = memoryusage.Length > 0 ? memoryusage : "null";
        }
        public void UpdateDeviceidAndVersion(string deviceid, string version)
        {
            info.deviceid = deviceid;
            info.gameversion = version;
        }
        public void UpdateUserid(string userid)
        {
            info.userid = userid.ToString();
        }
        public void Init()
        {
            info.appkey = "1407921103977";
            info.skdversion = "1.4.20";
            info.system = "all";
            info.gamechannel = "2010071003";
            info.userid = "null";
            info.hardwarename = "null";
            info.cpuusage = "null";
            info.cpubusfrequency = "null";
            info.memorytotal = "null";
            info.memoryusage = "null";
#if UNITY_IPHONE
            info.system = "IOS";
            info.gamechannel = "1010802002";
#elif UNITY_ANDROID
            info.system = "android";
            info.gamechannel = "2010071003";
#endif
            string version = "";
            if (version.Length > 0)
            {
                info.gameversion = version;
            }
        }
        private string Build(int code)
        {
            info.code = code.ToString();
            info.time = DateTime.Now.ToString("yyyyMMddHHmmss");
            char sp = Convert.ToChar(0x01);
            string result = "";
            if (code > 100)
            {
                result = info.appkey + sp + info.skdversion + sp
                + info.system + sp + info.gamechannel + sp + info.deviceid + sp
                + info.userid + sp + info.gameversion;
            }
            else
            {
                result = info.time + sp + info.code + sp + info.hardwarename + sp + info.cpuusage + sp
                + info.cpubusfrequency + sp + info.memorytotal + sp + info.memoryusage;
            }
            return result;
        }
        public string ServerName { get; set; }
        private GameInfo info = new GameInfo();
        static private NormLog m_Instance = new NormLog();
        static public NormLog Instance
        {
            get { return m_Instance; }
        }
    }
}
