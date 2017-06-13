using System.Collections.Generic;

namespace RapidIteration
{
    public class ResourceNode
    {
        public enum NodeType
        {
            Unknown  = 1 << 0,
            Shader   = 1 << 1,
            Texture  = 1 << 2,
            Material = 1 << 3,
            Mesh     = 1 << 4,
            Prefab   = 1 << 5,
        }

        public ResourceNode(string name, NodeType type)
        {
            this.name = name;
            this.type = type;
        }

        // Some resource node may have multiple parent(e.g, different material may use the same texture and shader)
        // but parents name MUST be different(e.g, .mtl file can't be the same).
        // So we only allow multiple parents with different name.
        public void AddParentNode(string name)
        {
            foreach (string nodeName in mParentNodes)
            {
                if (nodeName == name)
                {
                    return;
                }
            }

            mParentNodes.Add(name);
        }

        public List<string> GetParents()
        {
            return mParentNodes;
        }

        // Full path with file suffix that relative to Assets folder.
        public string name { get; set; }
        public NodeType type { get; set; }
        public long lastAccessUtc { get; set; }

        List<string> mParentNodes = new List<string>();
    }
}