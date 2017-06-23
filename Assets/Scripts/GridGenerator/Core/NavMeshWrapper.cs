// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace Cow
{
    public class NavMeshTriangle
    {
        private Vector3[] m_Vertices;

        public NavMeshTriangle(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            m_Vertices = new Vector3[3];
            m_Vertices[0] = p0;
            m_Vertices[1] = p1;
            m_Vertices[2] = p2;
        }

        public bool Contains(Vector3 point)
        {
            bool isIn = false;
            int numberOfPoints = 3;

            int i, j = 0;
            for (i = 0, j = numberOfPoints - 1; i < numberOfPoints; j = i++)
            {
                if (
                     (
                       ((m_Vertices[i].z <= point.z) && (point.z < m_Vertices[j].z)) || ((m_Vertices[j].z <= point.z) && (point.z < m_Vertices[i].z))
                     ) &&
                     (point.x < (m_Vertices[j].x - m_Vertices[i].x) * (point.z - m_Vertices[i].z) / (m_Vertices[j].z - m_Vertices[i].z) + m_Vertices[i].x)
                   )
                {
                    isIn = !isIn;
                }
            }

            return isIn;
        }

        public bool GetPoint(int index, out Vector3 point)
        {
            point = Vector3.zero;

            if (index < 0 || index > 2)
            {
                return false;
            }

            point = m_Vertices[index];

            return true;
        }
    }

    public class NavMeshObject
    {
        Bounds m_Bounds;
        List<NavMeshTriangle> m_TriangleList;

        public bool Initial()
        {
            NavMeshTriangulation navMeshTrian = NavMesh.CalculateTriangulation();
            if (navMeshTrian.vertices.Length == 0)
            {
                Debug.LogError("Navigation mesh does not exist.");
                return false;
            }

            m_TriangleList = new List<NavMeshTriangle>();

            int indicesNum = navMeshTrian.indices.Length;
            for (int i = 0; i < indicesNum; i += 3)
            {
                NavMeshTriangle triangle = new NavMeshTriangle(navMeshTrian.vertices[navMeshTrian.indices[i]], 
                                                               navMeshTrian.vertices[navMeshTrian.indices[i + 1]],
                                                               navMeshTrian.vertices[navMeshTrian.indices[i + 2]]);

                if (i == 0)
                {
                    Vector3 point;
                    triangle.GetPoint(0, out point);
                    m_Bounds.min = new Vector3(point.x, point.y - 1.0f, point.z);
                    m_Bounds.max = new Vector3(point.x, point.y + 1.0f, point.z);
                }

                m_TriangleList.Add(triangle);
                UpdateBounds(triangle);
            }

            return true;
        }

        public bool Contains(Vector3 point)
        {
            if (m_Bounds.Contains(point))
            {
                for (int i = 0; i < m_TriangleList.Count; i++)
                {
                    if (m_TriangleList[i].Contains(point))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Bounds bounds { get { return m_Bounds; } }

        private void UpdateBounds(NavMeshTriangle triangle)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 point;
                if (triangle.GetPoint(i, out point))
                {
                    if (point.x < m_Bounds.min.x)
                    {
                        m_Bounds.min = new Vector3(point.x, m_Bounds.min.y, m_Bounds.min.z);
                    }

                    if (point.x > m_Bounds.max.x)
                    {
                        m_Bounds.max = new Vector3(point.x, m_Bounds.max.y, m_Bounds.max.z);
                    }

                    if (point.z < m_Bounds.min.z)
                    {
                        m_Bounds.min = new Vector3(m_Bounds.min.x, m_Bounds.min.y, point.z);
                    }

                    if (point.z > m_Bounds.max.z)
                    {
                        m_Bounds.max = new Vector3(m_Bounds.max.x, m_Bounds.max.y, point.z);
                    }
                }
            }
        }
    }
}
