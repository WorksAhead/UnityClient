using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

namespace ArkCrossEngine
{
    public class AssetEx
    {
        private ResVersionData m_ResInfoRef = null;
        private AssetBundle m_AssetbundleRef = null;
        private UnityEngine.Object m_AssetRef = null;

        public int GetId()
        {
            return m_ResInfoRef.GetId();
        }
        public string AssetName()
        {
            return m_ResInfoRef.m_AssetName;
        }
        public string AssetShortName()
        {
            return m_ResInfoRef.m_AssetShortName;
        }
        public string AssetBundleName()
        {
            return m_ResInfoRef.m_AssetBundleName;
        }
        public string Md5()
        {
            return m_ResInfoRef.m_MD5;
        }
        public string RemoteUrl()
        {
            return ResLoadHelper.GetResServerURLAbs() + m_ResInfoRef.m_AssetBundleName;
        }
        public string LocateUrl()
        {
            if (IsCached())
            {
                return ResLoadHelper.GetCachedURLAbs() + m_ResInfoRef.m_AssetBundleName;
            }
            return string.Empty;
        }
        public bool IsCached()
        {
            return ResLoadHelper.IsResABCached(m_ResInfoRef.m_AssetBundleName, m_ResInfoRef.m_MD5);
        }
        public UnityEngine.Object GetAssetRef()
        {
            return m_AssetRef;
        }
        public void SetAssetRef(UnityEngine.Object assetRef)
        {
            m_AssetRef = assetRef; ;
        }
        public AssetBundle GetAssetbundleRef()
        {
            return m_AssetbundleRef;
        }
        public AssetEx(ResVersionData resInfo)
        {
            m_ResInfoRef = resInfo;
            Clear();
        }
        public void Clear()
        {
            ReleaseAssetBundle();
            ReleaseAsset();
        }
        public void CacheAssetBundle()
        {
            try
            {
                if (!IsCached()) return;
                m_AssetbundleRef = AssetBundle.LoadFromFile(LocateUrl());
                if (m_AssetbundleRef == null)
                {
                    ResLoadHelper.Log("CacheAssetBundle failed. url:" + LocateUrl());
                    return;
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log("LoadAssetFromAssetBundle failed url:" + LocateUrl() + " ex:" + ex);
            }
            finally
            {
            }
        }
        public ResAsyncInfo CacheAssetBundleAsync()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(CacheAssetBundleAsyncImpl(info));
            return info;
        }
        public void ReleaseAssetBundle()
        {
            if (m_AssetbundleRef != null)
            {
                m_AssetbundleRef.Unload(false);
                m_AssetbundleRef = null;
            }
        }
        public UnityEngine.Object ExtractAsset(bool isReleaseAsset = false, bool isReleaseAssetbundle = true)
        {
            try
            {
                if (m_AssetRef != null)
                {
                    return m_AssetRef;
                }
                UnityEngine.Object targetAsset = null;
                if (IsCached())
                {
                    CacheAssetBundle();
                    if (m_AssetbundleRef != null)
                    {
                        //targetAsset = m_AssetbundleRef.Load(AssetName());
                        throw new CodeNotImplException();
                    }
                    m_AssetRef = targetAsset;
                    return targetAsset;
                }
            }
            catch (OutOfMemoryException exception)
            {
                ResLoadHelper.ErrLog(exception.ToString());
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
            catch (Exception exception2)
            {
                ResLoadHelper.ErrLog("AssetEx Load Exception: " + exception2.ToString());
            }
            finally
            {
                if (isReleaseAsset)
                {
                    ReleaseAsset();
                }
                if (isReleaseAssetbundle)
                {
                    ReleaseAssetBundle();
                }
            }
            return null;
        }
        public void ReleaseAsset()
        {
            if (m_AssetRef != null)
            {
                m_AssetRef = null;
            }
        }
        internal IEnumerator CacheAssetBundleAsyncImpl(ResAsyncInfo info)
        {
            if (m_AssetbundleRef == null && IsCached())
            {
                if (!m_ResInfoRef.m_AssetName.EndsWith(".unity"))
                {
                    CacheAssetBundle();
                }
                else
                {
                    string url = LocateUrl();
                    using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, (int)fs.Length);
                        if (buffer == null || buffer.Length == 0)
                        {
                            ResLoadHelper.Log("CacheAssetBundleAsyncImpl bytes null url:" + url);
                            info.IsError = true;
                            yield break;
                        }
                        AssetBundleCreateRequest abRequest = AssetBundle.LoadFromMemoryAsync(buffer);
                        yield return abRequest;
                        try
                        {
                            m_AssetbundleRef = abRequest.assetBundle;
                        }
                        catch (System.Exception ex)
                        {
                            ResLoadHelper.Log("CacheAssetBundleAsyncImpl failed url:" + url + " ex:" + ex);
                            info.IsError = true;
                        }
                        finally
                        {
                            if (abRequest != null)
                            {
                                abRequest = null;
                            }
                        }
                    }
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
    };
}