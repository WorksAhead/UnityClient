using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

namespace RapidIteration
{
    [InitializeOnLoad]
    public class RapidIterationManager
    {
        // Hold the current scene name.
        static string sSceneName = "";
        // If the scene was modified.
        static bool sIsSceneDirty = false;

        // Dirty resource list.
        static List<ResourceNode> sPrefabList = new List<ResourceNode>();
        static List<ResourceNode> sMeshList = new List<ResourceNode>();
        static List<ResourceNode> sMaterialList = new List<ResourceNode>();
        static List<ResourceNode> sShaderList = new List<ResourceNode>();
        static List<ResourceNode> sTextureList = new List<ResourceNode>();

        // Hold the scene's initial resources snapshoot.
        static Dictionary<string, ResourceNode> sPrefabMap = new Dictionary<string, ResourceNode>();
        static Dictionary<string, ResourceNode> sMeshMap = new Dictionary<string, ResourceNode>();
        static Dictionary<string, ResourceNode> sMaterialMap = new Dictionary<string, ResourceNode>();
        static Dictionary<string, ResourceNode> sShaderMap = new Dictionary<string, ResourceNode>();
        static Dictionary<string, ResourceNode> sTextureMap = new Dictionary<string, ResourceNode>();

        static RapidIterationManager()
        {
            string openedSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
            if (openedSceneName.Length != 0 && sSceneName.Length == 0)
            {
                UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnSceneOpenedCallback;
                OnSceneOpenedCallback(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene(), UnityEditor.SceneManagement.OpenSceneMode.Single);
            }
        }

        static void OnSceneOpenedCallback(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
        {
            return;
            if (EditorApplication.isPlaying )
            {
                return;
            }

            // Update scene name.
            sSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
            sIsSceneDirty = false;

            sPrefabMap.Clear();
            sMeshMap.Clear();
            sMaterialMap.Clear();
            sShaderMap.Clear();
            sTextureMap.Clear();

            CollectSceneInfo(sPrefabMap, sMeshMap, sMaterialMap, sShaderMap, sTextureMap);
        }

        static void CollectSceneInfo(Dictionary<string, ResourceNode> prefabMap,
                                     Dictionary<string, ResourceNode> meshMap,
                                     Dictionary<string, ResourceNode> materialMap,
                                     Dictionary<string, ResourceNode> shaderMap,
                                     Dictionary<string, ResourceNode> textureMap)
        {
            // Construct the resource tree depend on current scene.
            GameObject[] allSceneObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            Debug.Log("All scene objects num: " + allSceneObjects.Length);
            foreach (GameObject sceneObject in allSceneObjects)
            {
                // Get all game object that created from prefab.
                PrefabType prefabType = PrefabUtility.GetPrefabType(sceneObject);
                if (prefabType == PrefabType.None)
                {
                    continue;
                }

                Object prefabObject = PrefabUtility.GetPrefabParent(sceneObject);
                string resourcePath = AssetDatabase.GetAssetPath(prefabObject);
                Debug.Log(sceneObject.name + " is instantiated from a prefab. Resource path is :" + resourcePath);

                // Get prefab resource last modified utc.
                System.DateTime lastAccessTime = System.IO.File.GetLastWriteTime(resourcePath);
                long lastAccessUtc = lastAccessTime.ToFileTimeUtc();

                ResourceNode resourceNode = new ResourceNode(resourcePath, ResourceNode.NodeType.Prefab);
                resourceNode.lastAccessUtc = lastAccessUtc;

                prefabMap.Add(resourceNode.name, resourceNode);

                // Get all resources that relative to this prefab.
                Object[] allDepObjects = EditorUtility.CollectDependencies(new Object[] { sceneObject });
                foreach (Object depObj in allDepObjects)
                {
                    if (depObj is Shader)
                    {
                        string shaderResPath = AssetDatabase.GetAssetPath(depObj);
                        // Ignore all unity built-in resources.
                        if (System.IO.File.Exists(shaderResPath))
                        {
                            Debug.Log("Shader resource path: " + shaderResPath);

                            ResourceNode shaderResNode;
                            bool found = shaderMap.TryGetValue(shaderResPath, out shaderResNode);
                            if (!found)
                            {
                                System.DateTime accessTime = System.IO.File.GetLastWriteTime(shaderResPath);
                                long accessUtc = accessTime.ToFileTimeUtc();

                                shaderResNode = new ResourceNode(shaderResPath, ResourceNode.NodeType.Shader);
                                shaderResNode.lastAccessUtc = accessUtc;

                                shaderMap.Add(shaderResNode.name, shaderResNode);
                            }

                            shaderResNode.AddParentNode(resourcePath);
                        }
                    }
                    else if (depObj is Material)
                    {
                        string matResPath = AssetDatabase.GetAssetPath(depObj);
                        if (System.IO.File.Exists(matResPath))
                        {
                            Debug.Log("Material resource path: " + matResPath);

                            ResourceNode matResNode;
                            bool found = materialMap.TryGetValue(matResPath, out matResNode);
                            if (!found)
                            {
                                System.DateTime accessTime = System.IO.File.GetLastWriteTime(matResPath);
                                long accessUtc = accessTime.ToFileTimeUtc();

                                matResNode = new ResourceNode(matResPath, ResourceNode.NodeType.Material);
                                matResNode.lastAccessUtc = accessUtc;

                                materialMap.Add(matResNode.name, matResNode);
                            }

                            matResNode.AddParentNode(resourcePath);
                        }
                    }
                    else if (depObj is Texture)
                    {
                        string texResPath = AssetDatabase.GetAssetPath(depObj);
                        if (System.IO.File.Exists(texResPath))
                        {
                            Debug.Log("Texture resource path: " + texResPath);

                            ResourceNode texResNode;
                            bool found = textureMap.TryGetValue(texResPath, out texResNode);
                            if (!found)
                            {
                                System.DateTime accessTime = System.IO.File.GetLastWriteTime(texResPath);
                                long accessUtc = accessTime.ToFileTimeUtc();

                                texResNode = new ResourceNode(texResPath, ResourceNode.NodeType.Texture);
                                texResNode.lastAccessUtc = accessUtc;

                                textureMap.Add(texResNode.name, texResNode);
                            }

                            texResNode.AddParentNode(resourcePath);
                        }
                    }
                    else if (depObj is Mesh)
                    {
                        string meshResPath = AssetDatabase.GetAssetPath(depObj);
                        if (System.IO.File.Exists(meshResPath))
                        {
                            Debug.Log("Mesh resource path: " + meshResPath);

                            ResourceNode meshResNode;
                            bool found = meshMap.TryGetValue(meshResPath, out meshResNode);
                            if (!found)
                            {
                                System.DateTime accessTime = System.IO.File.GetLastWriteTime(meshResPath);
                                long accessUtc = accessTime.ToFileTimeUtc();

                                meshResNode = new ResourceNode(meshResPath, ResourceNode.NodeType.Mesh);
                                meshResNode.lastAccessUtc = accessUtc;

                                meshMap.Add(meshResNode.name, meshResNode);
                            }

                            meshResNode.AddParentNode(resourcePath);
                        }
                    }
                }
            }
        }

        [MenuItem("RapidIteration/Diff Scene")]
        static void DiffScene()
        {
            sIsSceneDirty = UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty;
            Debug.Log("The scene was modified: " + sIsSceneDirty);

            sPrefabList.Clear();
            sMeshList.Clear();
            sMaterialList.Clear();
            sShaderList.Clear();
            sTextureList.Clear();

            // Grab current's resources snapshoot.
            Dictionary<string, ResourceNode> prefabMap = new Dictionary<string, ResourceNode>();
            Dictionary<string, ResourceNode> meshMap = new Dictionary<string, ResourceNode>();
            Dictionary<string, ResourceNode> materialMap = new Dictionary<string, ResourceNode>();
            Dictionary<string, ResourceNode> shaderMap = new Dictionary<string, ResourceNode>();
            Dictionary<string, ResourceNode> textureMap = new Dictionary<string, ResourceNode>();

            CollectSceneInfo(prefabMap, meshMap, materialMap, shaderMap, textureMap);

            // Diff the scene resources.
            foreach(var pair in prefabMap)
            {
                ResourceNode preResNode;
                bool found = sPrefabMap.TryGetValue(pair.Key, out preResNode);

                if (!found)
                {
                    // A new prefab resource.
                    sPrefabList.Add(pair.Value);
                    continue;
                }

                // An old prefab resource.
                if (pair.Value.lastAccessUtc != preResNode.lastAccessUtc)
                {
                    // Prefab was modified.
                    sPrefabList.Add(pair.Value);
                }
            }

            foreach (var pair in meshMap)
            {
                ResourceNode preResNode;
                bool found = sMeshMap.TryGetValue(pair.Key, out preResNode);

                if (!found)
                {
                    // A new mesh resource.
                    sMeshList.Add(pair.Value);
                    continue;
                }

                // An old mesh resource.
                if (pair.Value.lastAccessUtc != preResNode.lastAccessUtc)
                {
                    // mesh was modified.
                    sMeshList.Add(pair.Value);
                }
            }

            foreach (var pair in materialMap)
            {
                ResourceNode preResNode;
                bool found = sMaterialMap.TryGetValue(pair.Key, out preResNode);

                if (!found)
                {
                    // A new material resource.
                    sMaterialList.Add(pair.Value);
                    continue;
                }

                // An old material resource.
                if (pair.Value.lastAccessUtc != preResNode.lastAccessUtc)
                {
                    // Material was modified.
                    sMaterialList.Add(pair.Value);
                }
            }

            foreach (var pair in shaderMap)
            {
                ResourceNode preResNode;
                bool found = sShaderMap.TryGetValue(pair.Key, out preResNode);

                if (!found)
                {
                    // A new shader resource.
                    sShaderList.Add(pair.Value);
                    continue;
                }

                // An old shader resource.
                if (pair.Value.lastAccessUtc != preResNode.lastAccessUtc)
                {
                    // Shader was modified.
                    sShaderList.Add(pair.Value);
                }
            }

            foreach (var pair in textureMap)
            {
                ResourceNode preResNode;
                bool found = sTextureMap.TryGetValue(pair.Key, out preResNode);

                if (!found)
                {
                    // A new texture resource.
                    sTextureList.Add(pair.Value);
                    continue;
                }

                // An old texture resource.
                if (pair.Value.lastAccessUtc != preResNode.lastAccessUtc)
                {
                    // Texture was modified.
                    sTextureList.Add(pair.Value);
                }
            }

            // Debug print.
            Debug.Log("Modifed Prefabs:");
            foreach (var resNode in sPrefabList)
            {
                Debug.Log(resNode.name);
                Debug.Log("And it's parents:");
                foreach (var parentName in resNode.GetParents())
                {
                    Debug.Log(parentName);
                }
            }
            Debug.Log("Modifed Meshes:");
            foreach (var resNode in sMeshList)
            {
                Debug.Log(resNode.name);
                Debug.Log("And it's parents:");
                foreach (var parentName in resNode.GetParents())
                {
                    Debug.Log(parentName);
                }
            }
            Debug.Log("Modifed Materials:");
            foreach (var resNode in sMaterialList)
            {
                Debug.Log(resNode.name);
                Debug.Log("And it's parents:");
                foreach (var parentName in resNode.GetParents())
                {
                    Debug.Log(parentName);
                }
            }
            Debug.Log("Modifed Shaders:");
            foreach (var resNode in sShaderList)
            {
                Debug.Log(resNode.name);
                Debug.Log("And it's parents:");
                foreach (var parentName in resNode.GetParents())
                {
                    Debug.Log(parentName);
                }
            }
            Debug.Log("Modifed Textures:");
            foreach (var resNode in sTextureList)
            {
                Debug.Log(resNode.name);
                Debug.Log("And it's parents:");
                foreach (var parentName in resNode.GetParents())
                {
                    Debug.Log(parentName);
                }
            }
        }
    }
}