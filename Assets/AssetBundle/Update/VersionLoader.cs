using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class VersionLoader
    {
        public static ResAsyncInfo RequestClientVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(RequestClientVersionAsync(info));
            return info;
        }
        public static ResAsyncInfo RequestServerVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(RequestServerVersionAsync(info));
            return info;
        }
        private static IEnumerator RequestClientVersionAsync(ResAsyncInfo info)
        {
            string versionFileStreamingPath = Path.Combine(ResLoadHelper.GetStreamingAssetPath(),
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ClientVersionFile);
            string versionFilePersistPath = Path.Combine(UnityEngine.Application.persistentDataPath,
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ClientVersionFile);
            bool isPersistFileAlreadyExit = File.Exists(versionFilePersistPath);

            // Read Streaming version file
            ResLoadHelper.Log("RequestClientVersion URL;" + versionFileStreamingPath);
            using (WWW tWWW = new WWW(versionFileStreamingPath))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestClientVersion error");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestClientVersion_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    byte[] bytes = tWWW.bytes;
                    if (bytes == null || bytes.Length == 0)
                    {
                        ResLoadHelper.Log("RequestClientVersion bytes null or empty data;" + versionFileStreamingPath);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestClientVersion_Byte_Error;
                        tWWW.Dispose();
                        yield break;
                    }

                    VersionInfo clientVersionInfo = new VersionInfo();
                    clientVersionInfo.Load(versionFileStreamingPath, bytes);
                    ResUpdateControler.BuildinClientVersionInfo = clientVersionInfo;

                    ResUpdateControler.IsNeedSyncPackage = false;
                    if (isPersistFileAlreadyExit)
                    {
                        clientVersionInfo = new VersionInfo();
                        clientVersionInfo.Load(versionFilePersistPath);
                        ResUpdateControler.PersistClientVersionInfo = clientVersionInfo;
                        
                        VersionNum buildInVersionNum = ResUpdateControler.BuildinClientVersionInfo.Version;
                        VersionNum persistVersionNum = ResUpdateControler.PersistClientVersionInfo.Version;
                        if (ResUpdateControler.BuildinClientVersionInfo.Channel != ResUpdateControler.PersistClientVersionInfo.Channel)
                        {
                            ResUpdateControler.IsNeedSyncPackage = true;
                        }
                        else if (buildInVersionNum.GetVersionValue() != persistVersionNum.GetVersionValue())
                        {
                            if (buildInVersionNum.GetAppVersionForceValue() == persistVersionNum.GetAppVersionForceValue()
                              && buildInVersionNum.GetResVersionValue() < persistVersionNum.GetResVersionValue())
                            {
                                ResUpdateControler.IsNeedSyncPackage = false;
                            }
                            else
                            {
                                ResUpdateControler.IsNeedSyncPackage = true;
                            }
                        }
                        else
                        {
                            ResUpdateControler.IsNeedSyncPackage = false;
                        }
                    }
                    else
                    {
                        ResUpdateControler.IsNeedSyncPackage = true;
                    }

                    if (ResUpdateControler.IsNeedSyncPackage)
                    {
                        string dir = Path.GetDirectoryName(versionFilePersistPath);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        if (null != bytes)
                        {
                            File.WriteAllBytes(versionFilePersistPath, bytes);
                        }
                        else
                        {
                            ResLoadHelper.Log("RequestClientVersion bytes null or empty data;" + versionFileStreamingPath);
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.RequestClientVersion_Byte_Error;
                            yield break;
                        }
                    }

                    clientVersionInfo = new VersionInfo();
                    clientVersionInfo.Load(versionFilePersistPath);
                    ResUpdateControler.ClientVersionInfo = clientVersionInfo;
                    ResUpdateControler.OnUpdateVersionInfo();
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestClientVersion ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.RequestClientVersion_Load_Error;
                    yield break;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static IEnumerator RequestServerVersionAsync(ResAsyncInfo info)
        {
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                ResLoadHelper.Log("RequestServerVersion network not reachable:");
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.Network_Error;
                yield break;
            }

            string versionFilePersistPath = ResLoadHelper.GetResServerURLAbs() + ResUpdateControler.s_ServerVersionFile;
            string persistVersionFilePath = Path.Combine(UnityEngine.Application.persistentDataPath,
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ServerVersionFile);
            versionFilePersistPath = ResLoadHelper.GetDynamicUrl(versionFilePersistPath);
            ResLoadHelper.Log("RequestServerVersion URL;" + versionFilePersistPath);
            using (WWW tWWW = new WWW(versionFilePersistPath))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestServerVersion error");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestServerVersion_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }

                    byte[] bytes = tWWW.bytes;
                    if (bytes == null || bytes.Length == 0)
                    {
                        ResLoadHelper.Log("RequestServerVersion bytes null or empty data;" + ResUpdateControler.s_ServerVersionFile);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestServerVersion_Byte_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    if (!string.IsNullOrEmpty(persistVersionFilePath))
                    {
                        string dir = Path.GetDirectoryName(persistVersionFilePath);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        File.WriteAllBytes(persistVersionFilePath, bytes);
                    }
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResVersion ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.RequestServerVersion_Save_Error;
                    yield break;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            try
            {
                VersionInfo serverVersionInfo = new VersionInfo();
                serverVersionInfo.Load(persistVersionFilePath);
                ResUpdateControler.ServerVersionInfo = serverVersionInfo;
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("LoadVersionInfo parse ini ex:" + ex);
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.RequestServerVersion_Load_Error;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}
