using System.Collections.Generic;

namespace ArkCrossEngine
{
    public partial class AssetExManager
    {
        #region Singleton
        private static AssetExManager s_Instance = new AssetExManager();
        public static AssetExManager Instance
        {
            get { return s_Instance; }
        }
        #endregion
        public Dictionary<int, AssetEx> m_AssetExDict { get; private set; }
        public Dictionary<string, AssetEx> m_AssetExShortNameDict { get; private set; }

        private AssetExManager()
        {
            m_AssetExDict = new Dictionary<int, AssetEx>();
            m_AssetExShortNameDict = new Dictionary<string, AssetEx>();
        }
        public bool InitAllAssetEx()
        {
            Cleanup();
            m_AssetExDict.Clear();
            m_AssetExShortNameDict.Clear();
            Dictionary<int, ResVersionData> resInfoDict = ResVersionProvider.Instance.GetData();
            foreach (ResVersionData resInfo in resInfoDict.Values)
            {
                if (!m_AssetExDict.ContainsKey(resInfo.GetId()))
                {
                    AssetEx assetEx = new AssetEx(resInfo);
                    m_AssetExDict.Add(assetEx.GetId(), assetEx);
                    string assetShortName = assetEx.AssetShortName();
                    if (!string.IsNullOrEmpty(assetShortName) && !m_AssetExShortNameDict.ContainsKey(assetShortName))
                    {
                        m_AssetExShortNameDict.Add(assetShortName, assetEx);
                    }
                }
            }
            return true;
        }
        public void Cleanup()
        {
            ClearAllAssetEx();
            ClearAllAssetBundle();
        }
        public bool ClearAllAssetEx()
        {
            foreach (AssetEx assetEx in m_AssetExDict.Values)
            {
                assetEx.Clear();
            }
            return true;
        }
        public void ClearAllAssetBundle()
        {
            foreach (AssetEx assetEx in m_AssetExDict.Values)
            {
                assetEx.ReleaseAssetBundle();
            }
        }
        public AssetEx GetAsset(int id)
        {
            if (m_AssetExDict.ContainsKey(id))
            {
                return m_AssetExDict[id];
            }
            return null;
        }
        public List<AssetEx> GetAsset(IEnumerable<int> idList)
        {
            List<AssetEx> assetList = new List<AssetEx>();
            foreach (int assetId in idList)
            {
                AssetEx asset = GetAsset(assetId);
                if (asset != null)
                {
                    assetList.Add(asset);
                }
            }
            return assetList;
        }
        public List<int> GetAsset(IEnumerable<AssetEx> assetList)
        {
            List<int> assetIdList = new List<int>();
            foreach (AssetEx asset in assetList)
            {
                if (asset != null)
                {
                    assetIdList.Add(asset.GetId());
                }
            }
            return assetIdList;
        }
        public ArkCrossEngine.Object GetAssetByNameWithoutExtention(string assetShortName)
        {
            if (string.IsNullOrEmpty(assetShortName))
            {
                return null;
            }
            if (assetShortName.Contains("."))
            {
                int extIndex = assetShortName.IndexOf(".");
                assetShortName = assetShortName.Substring(0, extIndex - 1);
            }
            if (m_AssetExShortNameDict.ContainsKey(assetShortName))
            {
                AssetEx asset = m_AssetExShortNameDict[assetShortName];
                if (asset != null)
                {
                    ArkCrossEngine.Object assetRef = ArkCrossEngine.ObjectFactory.Create(asset.ExtractAsset(false, true));
                    //ArkCrossEngine.Object assetRef = new ArkCrossEngine.Object(asset.ExtractAsset(false, true));
                    //if (assetRef == null) {
                    //  ResLoadHelper.Log("GetAssetByNameWithoutExtention failed:" + assetNameWithoutExtention);
                    //}
                    return assetRef;
                }
            }

            return null;
        }
        public bool SearchByCacheDataList(Dictionary<int, ResCacheData> cacheDataList, ref List<AssetEx> assetExtractList)
        {
            HashSet<int> assetExtractSet = new HashSet<int>();
            foreach (ResCacheData cacheData in cacheDataList.Values)
            {
                assetExtractSet.UnionWith(cacheData.m_Assets);
            }
            assetExtractList.AddRange(AssetExManager.Instance.GetAsset(assetExtractSet));
            return true;
        }
    }
}