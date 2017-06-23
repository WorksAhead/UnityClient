// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEngine;

namespace Cow
{
    public class GridGenerator
    {
        Vector2 m_UnclampedGridSize = new Vector2(10.0f, 10.0f);
        Vector2 m_GridSize;
        Vector3 m_GridCoordinateCenter = Vector3.zero;

        float m_NodeSize = 0.5f;

        int m_MaxNodeNumInWidth = 100;
        int m_MaxNodeNumInDepth = 100;
        int m_NodeNumInWidth;
        int m_NodeNumInDepth;

        bool m_NodeSizeSelfAdaption = true;

        Matrix4x4 m_Matrix = Matrix4x4.identity;
        Matrix4x4 m_InverseMatrix = Matrix4x4.identity;

        GridNode[] m_Nodes;

        NavMeshObject m_NavMeshObject;

        GameObject m_DebugGameObject;

        public bool Initial()
        {
            m_NavMeshObject = new NavMeshObject();
            if (!m_NavMeshObject.Initial())
            {
                return false;
            }

            m_UnclampedGridSize = new Vector2(m_NavMeshObject.bounds.size.x, m_NavMeshObject.bounds.size.z);
            m_GridCoordinateCenter = new Vector3(m_NavMeshObject.bounds.min.x, m_NavMeshObject.bounds.center.y, m_NavMeshObject.bounds.min.z);

            return GenerateMatrix();
        }

        public bool Initial(float nodeSize, int maxNodeNumInWidth, int maxNodeNumInDepth, bool nodeSizeSelfAdaption = true)
        {
            m_NavMeshObject = new NavMeshObject();
            if (!m_NavMeshObject.Initial())
            {
                return false;
            }

            m_UnclampedGridSize = new Vector2(m_NavMeshObject.bounds.size.x, m_NavMeshObject.bounds.size.z);
            m_GridCoordinateCenter = new Vector3(m_NavMeshObject.bounds.min.x, m_NavMeshObject.bounds.center.y, m_NavMeshObject.bounds.min.z);

            m_NodeSize = nodeSize;
            m_MaxNodeNumInWidth = maxNodeNumInWidth;
            m_MaxNodeNumInDepth = maxNodeNumInDepth;
            m_NodeSizeSelfAdaption = nodeSizeSelfAdaption;

            return GenerateMatrix();
        }

        public void Scan()
        {
            m_Nodes = new GridNode[m_NodeNumInWidth * m_NodeNumInDepth];
            for (int i = 0; i < m_Nodes.Length; i++)
            {
                m_Nodes[i] = new GridNode();
            }

            for (int z = 0; z < m_NodeNumInDepth; z++)
            {
                for (int x = 0; x < m_NodeNumInWidth; x++)
                {
                    GridNode node = m_Nodes[z * m_NodeNumInWidth + x];

                    node.gridIndex = z * m_NodeNumInWidth + x;

                    node.indexX = x;
                    node.indexZ = z;

                    node.nodeSize = m_NodeSize;

                    node.position = GraphPointToWorld(x, z, 0);
                    if (m_NavMeshObject.Contains((Vector3)node.position))
                    {
                        node.walkable = true;
                    }
                    else
                    {
                        node.walkable = false;
                    }
                }
            }
        }

        public void EnableDebugMesh()
        {
            GridDrawer gridDrawer;

            m_DebugGameObject = GameObject.Find("CowGridDebug");
            if (m_DebugGameObject == null)
            {
                m_DebugGameObject = new GameObject("CowGridDebug");
                gridDrawer = m_DebugGameObject.AddComponent<GridDrawer>();
                gridDrawer.Initial();
            }
            else
            {
                gridDrawer = m_DebugGameObject.GetComponent<GridDrawer>();
            }
            
            gridDrawer.Draw(m_Nodes, m_NodeNumInWidth, m_NodeNumInDepth);
        }

        public void DisableDebugMesh()
        {
            if (m_DebugGameObject != null)
            {
                GameObject.DestroyImmediate(m_DebugGameObject);
            }
        }

        bool GenerateMatrix()
        {
            Vector2 newSize = m_UnclampedGridSize;

            newSize.x *= Mathf.Sign(newSize.x);
            newSize.y *= Mathf.Sign(newSize.y);

            if (m_NodeSize < newSize.x / (float)m_MaxNodeNumInWidth || m_NodeSize < newSize.y / (float)m_MaxNodeNumInDepth)
            {
                if (m_NodeSizeSelfAdaption)
                {
                    Debug.LogWarning("Gird width and depth are not able to over num " + m_MaxNodeNumInWidth + " and " + m_MaxNodeNumInDepth + ".\n" +
                                     "Node size will be scaled automatically to fit this constraint.");
                }
                else
                {
                    Debug.LogWarning("Gird width and depth are not able to over num " + m_MaxNodeNumInWidth + " and " + m_MaxNodeNumInDepth);
                    return false;
                }
            }

            float nodeWidthSize = Mathf.Clamp(m_NodeSize, newSize.x / (float)m_MaxNodeNumInWidth, Mathf.Infinity);
            float nodeDepthSize = Mathf.Clamp(m_NodeSize, newSize.y / (float)m_MaxNodeNumInDepth, Mathf.Infinity);
            m_NodeSize = Mathf.Max(nodeWidthSize, nodeDepthSize);

            newSize.x = newSize.x < m_NodeSize ? m_NodeSize : newSize.x;
            newSize.y = newSize.y < m_NodeSize ? m_NodeSize : newSize.y;

            m_GridSize = newSize;

            m_NodeNumInWidth = Mathf.FloorToInt(m_GridSize.x / m_NodeSize);
            m_NodeNumInDepth = Mathf.FloorToInt(m_GridSize.y / m_NodeSize);

            if (Mathf.Approximately(m_GridSize.x / m_NodeSize, Mathf.CeilToInt(m_GridSize.x / m_NodeSize)))
            {
                m_NodeNumInWidth = Mathf.CeilToInt(m_GridSize.x / m_NodeSize);
            }

            if (Mathf.Approximately(m_GridSize.y / m_NodeSize, Mathf.CeilToInt(m_GridSize.y / m_NodeSize)))
            {
                m_NodeNumInDepth = Mathf.CeilToInt(m_GridSize.y / m_NodeSize);
            }

            float remainderX = m_GridSize.x - ((float)m_NodeNumInWidth * m_NodeSize);
            float remainderY = m_GridSize.y - ((float)m_NodeNumInDepth * m_NodeSize);

            Vector3 newCenter = new Vector3(m_GridCoordinateCenter.x + remainderX * 0.5f, m_GridCoordinateCenter.y + 0.5f, m_GridCoordinateCenter.z + remainderY * 0.5f);

            Matrix4x4 m = Matrix4x4.TRS(newCenter, Quaternion.Euler(Vector3.zero), new Vector3(m_NodeSize, 1, m_NodeSize));

            m_Matrix = m;
            m_InverseMatrix = m.inverse;

            return true;
        }

        Int3 GraphPointToWorld(int x, int z, float height)
        {
            return (Int3)m_Matrix.MultiplyPoint3x4(new Vector3(x + 0.5f, height, z + 0.5f));
        }
    }
}
