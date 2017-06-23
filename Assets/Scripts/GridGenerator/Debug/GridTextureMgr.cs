// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEngine;
using System.Collections.Generic;

namespace Cow
{
    public class GridTextureInfo
    {
        public string imgName;
        public Vector2 imgOffset;
        public float imgSize;
        public float atlasSize;
    }

    public class GridTextureMgr
    {
        static GridTextureMgr m_Instance = null;
        public static GridTextureMgr Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new GridTextureMgr();
            }

            return m_Instance;
        }

        Texture2D m_AtlasTexture = null;
        GridTextureInfo m_DefaultTextureInfo = null;
        List<GridTextureInfo> m_AtlasInfoList = null;

        public bool Initial()
        {
            TextAsset atlasInfo = (TextAsset)Resources.Load("Num/GridNumAtlas", typeof(TextAsset));
            if (atlasInfo == null)
            {
                return false;
            }

            m_AtlasTexture = (Texture2D)Resources.Load("Num/GridNumAtlas", typeof(Texture2D));
            if (m_AtlasTexture == null)
            {
                return false;
            }

            if (m_AtlasTexture.width != m_AtlasTexture.height)
            {
                return false;
            }

            ParseAtlasInfo(atlasInfo);

            m_DefaultTextureInfo = GetTextureInfo("-.png");

            return true;
        }

        public Texture2D GetAtlasTexture()
        {
            return m_AtlasTexture;
        }

        public GridTextureInfo GetTextureInfo(string imgName)
        {
            for (int i = 0; i < m_AtlasInfoList.Count; i++)
            {
                if (m_AtlasInfoList[i].imgName == imgName)
                {
                    return m_AtlasInfoList[i];
                }
            }

            return m_DefaultTextureInfo;
        }

        void ParseAtlasInfo(TextAsset atlasInfo)
        {
            m_AtlasInfoList = new List<GridTextureInfo>();

            string infoText = atlasInfo.text;
            string[] infoSplitArray = infoText.Split(new char[]{'\r', '\n'});

            for (int i = 0; i < infoSplitArray.Length; i++)
            {
                string[] textureSplitArray = infoSplitArray[i].Split(new char[] { ',' });

                if (textureSplitArray.Length != 5)
                {
                    continue;
                }

                GridTextureInfo textureInfo = new GridTextureInfo();
                textureInfo.imgName = textureSplitArray[0];
                textureInfo.imgOffset = new Vector2(System.Convert.ToSingle(textureSplitArray[1]), System.Convert.ToSingle(textureSplitArray[2]));
                textureInfo.imgSize = System.Convert.ToSingle(textureSplitArray[3]);
                textureInfo.atlasSize = m_AtlasTexture.width;

                m_AtlasInfoList.Add(textureInfo);
            }
        }
    }
}
