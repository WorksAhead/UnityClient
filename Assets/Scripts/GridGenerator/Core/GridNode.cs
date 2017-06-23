// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEngine;

namespace Cow
{
    public class GridNode
    {
        public bool walkable { get; set; }
        public float nodeSize { get; set; }
        public int gridIndex { get; set; }

        public int indexX { get; set; }
        public int indexZ { get; set; }

        public Int3 position { get; set; }

        public Vector3 lowerLeftPos
        {
            get
            {
                Vector3 pos = (Vector3)position;
                pos.x -= nodeSize / 2.0f;
                pos.z -= nodeSize / 2.0f;
                return pos;
            }
        }

        public Vector3 lowerRightPos
        {
            get
            {
                Vector3 pos = (Vector3)position;
                pos.x += nodeSize / 2.0f;
                pos.z -= nodeSize / 2.0f;
                return pos;
            }
        }

        public Vector3 topLeftPos
        {
            get
            {
                Vector3 pos = (Vector3)position;
                pos.x -= nodeSize / 2.0f;
                pos.z += nodeSize / 2.0f;
                return pos;
            }
        }

        public Vector3 topRightPos
        {
            get
            {
                Vector3 pos = (Vector3)position;
                pos.x += nodeSize / 2.0f;
                pos.z += nodeSize / 2.0f;
                return pos;
            }
        }
    }
}
