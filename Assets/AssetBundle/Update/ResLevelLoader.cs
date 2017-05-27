using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class ResLevelLoader
    {
        public static ResAsyncInfo LoadLevelAsync(int levelId)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(LoadLevelImplAsync(info, levelId));
            return info;
        }
        internal static IEnumerator LoadLevelImplAsync(ResAsyncInfo info, int levelId)
        {
            ArkProfiler.Start("LoadLevelImplAsync");
            ResUpdateCallback.OnStartLoad(levelId);
            AssetExManager.Instance.Cleanup();
            ResUpdateControler.OnUpdateProgress(0);

            ArkProfiler.Start("SearchCacheData");
            Dictionary<int, ResCacheData> cacheDataDict = new Dictionary<int, ResCacheData>();
            ResCacheData levelCacheData = null;
            ResCacheProvider.Instance.SearchCacheDataByLevelId(levelId, ref cacheDataDict, out levelCacheData);
            if (levelCacheData == null)
            {
                info.IsDone = true;
                info.Progress = 1.0f;
                yield break;
            }

            List<AssetEx> assetExtractList = new List<AssetEx>();
            AssetExManager.Instance.SearchByCacheDataList(cacheDataDict, ref assetExtractList);
            ArkProfiler.Stop("SearchCacheData");

            ArkProfiler.Start("ExtractAsset");
            int assetexCount = 0;
            int assetexCountMax = assetExtractList.Count;
            foreach (AssetEx assetEx in assetExtractList)
            {
                if (assetEx != null)
                {
                    assetexCount++;
                    if (assetEx.GetAssetRef() != null)
                    {
                        if (assetEx.IsCached())
                        {
                            assetEx.ExtractAsset(false, true);
                        }
                        else
                        {
                            UnityEngine.Object tmpAsset = CrossObjectHelper.TryCastObject<UnityEngine.Object>(ResourceSystem.GetSharedResource(assetEx.AssetShortName(), false));
                            assetEx.SetAssetRef(tmpAsset);
                        }
                    }
                    if (assetEx.GetAssetRef() == null)
                    {
                        ResLoadHelper.Log("Extract Asset failed ab:" + assetEx.ToString());
                        if (ResUpdateControler.s_EnableWeakCheck)
                        {
                            continue;
                        }
                        else
                        {
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.ResLevelLoader_Extract_Error;
                            yield break;
                        }
                    }
                    if ((assetexCount % 10) == 0)
                    {
                        info.Progress = 0.5f * assetexCount / assetexCountMax;
                        ResUpdateControler.OnUpdateProgress(info.Progress);
                        yield return 1;
                    }
                }
            }
            info.Progress = 0.5f;
            ResUpdateControler.OnUpdateProgress(info.Progress);
            ArkProfiler.Stop("ExtractAsset");

            ArkProfiler.Start("LoadLevelAsync");
            AssetEx assetLevel = null;
            if (levelCacheData.m_Assets != null && levelCacheData.m_Assets.Count > 0)
            {
                assetLevel = AssetExManager.Instance.GetAsset(levelCacheData.m_Assets[0]);
            }
            if (assetLevel != null)
            {
                ResAsyncInfo levelCacheInfo = assetLevel.CacheAssetBundleAsync();
                yield return levelCacheInfo.CurCoroutine;
                info.Progress = 0.6f;
                ResUpdateControler.OnUpdateProgress(info.Progress);
                yield return 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(assetLevel.AssetShortName());
                info.Progress = 0.8f;
                ResUpdateControler.OnUpdateProgress(info.Progress);
                yield return 1;
                //AsyncOperation loadLevelAsync = UnityEngine.Application.LoadLevelAsync(assetLevel.AssetShortName());
                //while (loadLevelAsync != null && !loadLevelAsync.isDone) {
                //  info.Progress = 0.6f + 0.4f * loadLevelAsync.progress;
                //  ResUpdateControler.OnUpdateProgress(info.Progress);
                //  yield return loadLevelAsync;
                //}
                assetLevel.ReleaseAssetBundle();
            }
            ArkProfiler.Stop("LoadLevelAsync");

            AssetExManager.Instance.ClearAllAssetBundle();

            ResUpdateControler.OnUpdateProgress(1.0f);
            Resources.UnloadUnusedAssets();
            ResUpdateCallback.OnEndLoad(levelId);

            ArkProfiler.Stop("LoadLevelImplAsync");

            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}
