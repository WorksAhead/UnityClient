using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.IO;
using System.Security.Cryptography;

namespace ArkCrossEngine
{
    public class ResUpdateCallback
    {
        public static void OnStartDetectVersion()
        {
            NormLog.Instance.Record(GameEventCode.CheckUpdate);
        }
        public static void OnEndDetectVersion(bool isNeedUpdate)
        {
            NormLog.Instance.Record(GameEventCode.CheckResult);
        }
        public static void OnStartUpdate()
        {
            NormLog.Instance.Record(GameEventCode.StartUpdate);
        }
        public static void OnEndUpdate()
        {
            NormLog.Instance.Record(GameEventCode.EndUpdate);
        }
        public static void OnStartUnzip()
        {
            NormLog.Instance.Record(GameEventCode.StartExtract);
        }
        public static void OnEndUnzip()
        {
            NormLog.Instance.Record(GameEventCode.EndExtract);
        }
        public static void OnStartLoad(int sceneId)
        {
            if ((int)ArkCrossEngine.SceneTypeEnum.TYPE_SERVER_SELECT == sceneId)
            {
                NormLog.Instance.Record(GameEventCode.LoadAssets);
            }
        }
        public static void OnEndLoad(int sceneId)
        {
            if ((int)ArkCrossEngine.SceneTypeEnum.TYPE_SERVER_SELECT == sceneId)
            {
                NormLog.Instance.Record(GameEventCode.EndAssets);
            }
        }
        public static void OnStartRequestServerList()
        {

        }
        public static void OnEndRequestServerList()
        {
            NormLog.Instance.Record(GameEventCode.ServerList);
        }
        public static void OnStartRequestNoticeConfig()
        {
        }
        public static void OnEndRequestNoticeConfig()
        {
        }
        public static void OnUpdateVersionNum(string versionNum)
        {

        }
    }
}
