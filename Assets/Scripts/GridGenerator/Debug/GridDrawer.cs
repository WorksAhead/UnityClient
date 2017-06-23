// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEngine;
using System.Collections.Generic;

namespace Cow
{
    class DrawableNode
    {
        GridNode m_Node = null;
        int[] m_Indices = null;

        public GridNode node
        {
            get
            {
                return m_Node;
            }

            set
            {
                m_Node = value;
            }
        }

        public int[] indices
        {
            get
            {
                return m_Indices;
            }

            set
            {
                m_Indices = value;
            }
        }

        public Color color
        {
            get
            {
                if (m_Node.walkable)
                {
                    return Color.white;
                }
                else
                {
                    return Color.red;
                }
            }
        }
    }

    public class GridDrawer : MonoBehaviour
    {
        DrawableNode[] m_DrawableNodes;
        MeshFilter meshFilter;

        public void Initial()
        {
            meshFilter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
            }

            if (meshFilter.sharedMesh == null)
            {
                meshFilter.sharedMesh = new Mesh();
                meshFilter.sharedMesh.name = "GridMesh";
                meshFilter.sharedMesh.MarkDynamic();
            }

            MeshRenderer renderer = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
            if (renderer == null)
            {
                renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            }

            if (GridTextureMgr.Instance().Initial() == false)
            {
                return;
            }

            if (renderer.sharedMaterial == null)
            {
                renderer.sharedMaterial = new Material(Shader.Find("Cow/VertexColor"));
                renderer.sharedMaterial.SetTexture("_MainTex", GridTextureMgr.Instance().GetAtlasTexture());
            }
        }

        public void Draw(GridNode[] gridNodes, int gridWidth, int gridDepth)
        {
            m_DrawableNodes = new DrawableNode[gridNodes.Length];
            for (int i = 0; i < gridNodes.Length; i++)
            {
                DrawableNode drawableNode = new DrawableNode();
                drawableNode.node = gridNodes[i];

                m_DrawableNodes[i] = drawableNode;
            }

            int totalQuads = gridWidth * gridDepth;
            int totalTriangles = totalQuads * 2;
            int totalVertices = totalTriangles * 3;

            Debug.Log("Grid vertices num: " + totalVertices);

            List<Vector3> verticesList = new List<Vector3>();
            List<Vector2> uvsList = new List<Vector2>();
            int[] indices = new int[totalVertices];
            Color[] colors = new Color[totalVertices];

            for (int i = 0; i < gridNodes.Length; i++)
            {
                GridNode gridNode = gridNodes[i];
                DrawableNode drawableNode = m_DrawableNodes[i];

                string info = "-.png";
                GridTextureInfo textureInfo = GridTextureMgr.Instance().GetTextureInfo(info);

                verticesList.Add(gridNode.lowerLeftPos);
                verticesList.Add(gridNode.topLeftPos);
                verticesList.Add(gridNode.lowerRightPos);

                Vector2 uvA0 = textureInfo.imgOffset / textureInfo.atlasSize;
                Vector2 uvA1 = new Vector2(textureInfo.imgOffset.x, textureInfo.imgOffset.y + textureInfo.imgSize) / textureInfo.atlasSize;
                Vector2 uvA2 = new Vector2(textureInfo.imgOffset.x + textureInfo.imgSize, textureInfo.imgOffset.y) / textureInfo.atlasSize;

                uvsList.Add(uvA0);
                uvsList.Add(uvA1);
                uvsList.Add(uvA2);

                verticesList.Add(gridNode.topLeftPos);
                verticesList.Add(gridNode.topRightPos);
                verticesList.Add(gridNode.lowerRightPos);

                Vector2 uvB0 = uvA1;
                Vector2 uvB1 = new Vector2(textureInfo.imgOffset.x + textureInfo.imgSize, textureInfo.imgOffset.y + textureInfo.imgSize) / textureInfo.atlasSize;
                Vector2 uvB2 = uvA2;

                uvsList.Add(uvB0);
                uvsList.Add(uvB1);
                uvsList.Add(uvB2);

                int[] quadIndices = new int[6];
                quadIndices[0] = i * 6;
                quadIndices[1] = i * 6 + 1;
                quadIndices[2] = i * 6 + 2;
                quadIndices[3] = i * 6 + 3;
                quadIndices[4] = i * 6 + 4;
                quadIndices[5] = i * 6 + 5;

                indices[i * 6] = quadIndices[0];
                indices[i * 6 + 1] = quadIndices[1];
                indices[i * 6 + 2] = quadIndices[2];
                indices[i * 6 + 3] = quadIndices[3];
                indices[i * 6 + 4] = quadIndices[4];
                indices[i * 6 + 5] = quadIndices[5];

                drawableNode.indices = quadIndices;

                colors[i * 6] = drawableNode.color;
                colors[i * 6 + 1] = drawableNode.color;
                colors[i * 6 + 2] = drawableNode.color;
                colors[i * 6 + 3] = drawableNode.color;
                colors[i * 6 + 4] = drawableNode.color;
                colors[i * 6 + 5] = drawableNode.color;
            }

            meshFilter.sharedMesh.vertices = verticesList.ToArray();
            meshFilter.sharedMesh.uv = uvsList.ToArray();
            meshFilter.sharedMesh.triangles = indices;
            meshFilter.sharedMesh.colors = colors;

            Vector3[] normals = new Vector3[totalVertices];
            for (int i = 0; i < totalVertices; i++)
            {
                normals[i] = Vector3.up;
            }
            meshFilter.sharedMesh.normals = normals;

            meshFilter.sharedMesh.RecalculateBounds();

            MeshCollider collider = gameObject.GetComponent(typeof(MeshCollider)) as MeshCollider;
            if (collider == null)
            {
                collider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
            }

            collider.sharedMesh = meshFilter.sharedMesh;
        }

        void Update()
        {
            //UpdateGrid();
        }

        void UpdateGrid()
        {
            Color[] colors = meshFilter.mesh.colors;

            DrawableNode drawableNode;
            for (int i = 0; i < m_DrawableNodes.Length; i++)
            {
                drawableNode = m_DrawableNodes[i];

                colors[drawableNode.indices[0]] = drawableNode.color;
                colors[drawableNode.indices[1]] = drawableNode.color;
                colors[drawableNode.indices[2]] = drawableNode.color;
                colors[drawableNode.indices[3]] = drawableNode.color;
                colors[drawableNode.indices[4]] = drawableNode.color;
                colors[drawableNode.indices[5]] = drawableNode.color;
            }

            meshFilter.mesh.colors = colors;
        }
    }
}
