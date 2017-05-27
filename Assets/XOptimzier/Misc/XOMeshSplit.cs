#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;

namespace XOpt
{
    [ExecuteInEditMode]
    public class XOMeshSplit
    {
        /// <summary>
        /// Author: Evelyn Liu
        /// Create a new mesh with one of oldMesh's submesh
        /// http://wiki.unity3d.com/index.php?title=MeshCreationHelper
        /// </summary>
        public static Mesh CreateMesh(Mesh oldMesh, int subIndex)
        {
            Mesh newMesh = new Mesh();

            List<int> triangles = new List<int>();
            triangles.AddRange(oldMesh.GetTriangles(subIndex)); // the triangles of the sub mesh

            List<UnityEngine.Vector3> newVertices = new List<UnityEngine.Vector3>();
            List<UnityEngine.Vector2> newUvs = new List<UnityEngine.Vector2>();
            List<UnityEngine.Vector2> newUv2s = new List<UnityEngine.Vector2>();

            // Mark's method. 
            Dictionary<int, int> oldToNewIndices = new Dictionary<int, int>();
            int newIndex = 0;

            // Collect the vertices and uvs
            for (int i = 0; i < oldMesh.vertices.Length; i++)
            {
                if (triangles.Contains(i))
                {
                    newVertices.Add(oldMesh.vertices[i]);
                    newUvs.Add(oldMesh.uv[i]);
                    newUv2s.Add(oldMesh.uv2[i]);
                    oldToNewIndices.Add(i, newIndex);
                    ++newIndex;
                }
            }

            int[] newTriangles = new int[triangles.Count];

            // Collect the new triangles indecies
            for (int i = 0; i < newTriangles.Length; i++)
            {
                newTriangles[i] = oldToNewIndices[triangles[i]];
            }
            // Assemble the new mesh with the new vertices/uv/triangles.
            newMesh.vertices = newVertices.ToArray();
            newMesh.uv = newUvs.ToArray();
            newMesh.uv2 = newUv2s.ToArray();
            newMesh.triangles = newTriangles;

            // Re-calculate bounds and normals for the renderer.
            newMesh.RecalculateBounds();
            newMesh.RecalculateNormals();

            return newMesh;
        }




        public static bool CanSplit(UnityEngine.GameObject rObject)
        {
            MeshFilter lMeshFilter = rObject.GetComponent<MeshFilter>();
            if (lMeshFilter != null)
            {
                if (lMeshFilter.sharedMesh != null)
                {
                    int lSubMeshCount = lMeshFilter.sharedMesh.subMeshCount;
                    if (lSubMeshCount > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public static void ExtractMaterialMeshes(UnityEngine.GameObject rObject)
        {
            if (rObject == null) { return; }
            if (!rObject.activeInHierarchy) { return; }

            // If this object has a mesh, extract it
            MeshFilter lMeshFilter = rObject.GetComponent<MeshFilter>();
            if (lMeshFilter == null) { return; }

            // Ensure we're only extracting rendered meshes
            MeshRenderer lMeshRenderer = rObject.GetComponent<MeshRenderer>();
            if (lMeshRenderer == null) { return; }

            // We need to break the mesh into it's sub mesh parts so we can assign materials
            Mesh lMesh = null;
            Material lMaterial = null;

            int lSubMeshCount = lMeshFilter.sharedMesh.subMeshCount;
            for (int i = 0; i < lSubMeshCount; i++)
            {
                // This is easy, use the shared instance
                if (i == 0)
                {
                    lMesh = lMeshFilter.sharedMesh;
                    lMaterial = lMeshRenderer.sharedMaterial;
                }
                // Process each sub-mesh individually
                else
                {
                    // Create a mesh from the sub-mesh
                    lMesh = CreateMesh(lMeshFilter.sharedMesh, i);

                    // Find the material it's using
                    if (lMeshRenderer.sharedMaterials.Length > i)
                    {
                        lMaterial = lMeshRenderer.sharedMaterials[i];
                    }
                    else
                    {
                        lMaterial = lMeshRenderer.sharedMaterials[lMeshRenderer.sharedMaterials.Length - 1];
                    }
                }

                string newname = "split-" + rObject.name + i;
                UnityEngine.GameObject newobj = new UnityEngine.GameObject(newname);
                newobj.transform.parent = XOGloble.IterM.transform;
                newobj.transform.position = lMeshFilter.transform.position;
                newobj.transform.rotation = lMeshFilter.transform.rotation;
                newobj.transform.localScale = lMeshFilter.transform.lossyScale;


                newobj.isStatic = true;


                MeshFilter lMergedMeshFilter = newobj.AddComponent(typeof(MeshFilter)) as MeshFilter;
                lMergedMeshFilter.mesh = lMesh;


                // CombineInstance[] lMeshes = new CombineInstance[1];
                // lMeshes[0].transform = lMeshFilter.transform.localToWorldMatrix;
                // lMeshes[0].mesh = lMesh;

                lMergedMeshFilter.sharedMesh.name = "Merged Mesh of " + lSubMeshCount + " items";
                // lMergedMeshFilter.sharedMesh.CombineMeshes(lMeshes, true);
                // Unwrapping.GenerateSecondaryUVSet(lMergedMeshFilter.sharedMesh);



                MeshRenderer lMergedMeshRenderer = newobj.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                lMergedMeshRenderer.sharedMaterial = lMaterial;

                XOCp_OptimizerHandler xoh = XOGloble.Root.GetComponent<XOCp_OptimizerHandler>();
                if (xoh != null)
                {
                    xoh.nlst.Add(newobj);
                    xoh.AddObjLstByShaderMaterial(newobj);
                }
            }
        }
    }
}
#endif