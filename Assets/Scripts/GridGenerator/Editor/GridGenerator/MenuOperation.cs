// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEngine;
using UnityEditor;
using System.IO;

namespace Cow
{
    [InitializeOnLoad]
    public static class MenuOperation
    {
        static bool m_DebugEnabled;
        static GridGenerator m_GridGenerator;

        static MenuOperation()
        {
            m_DebugEnabled = EditorPrefs.GetBool("GridGenerator/Debug", false);

            EditorApplication.delayCall += () => {
                ToggleDebug(m_DebugEnabled);
            };
        }

        [MenuItem("GridGenerator/Debug")]
        static void ToggleDebugAction()
        {
            ToggleDebug(!m_DebugEnabled);
        }

        static void ToggleDebug(bool enabled)
        {
            UnityEngine.Debug.Log("Toggle debug " + enabled);

            Menu.SetChecked("GridGenerator/Debug", enabled);
            EditorPrefs.SetBool("GridGenerator/Debug", enabled);

            m_DebugEnabled = enabled;

            if (m_GridGenerator != null)
            {
                if (m_DebugEnabled)
                {
                    m_GridGenerator.EnableDebugMesh();
                }
                else
                {
                    m_GridGenerator.DisableDebugMesh();
                }
            }
        }

        [MenuItem("GridGenerator/Generate")]
        static void GenerateGrid()
        {
            if (m_GridGenerator != null)
            {
                m_GridGenerator.DisableDebugMesh();
            }

            m_GridGenerator = new GridGenerator();
            if (!m_GridGenerator.Initial())
            {
                return;
            }

            m_GridGenerator.Scan();

            if (m_DebugEnabled)
            {
                m_GridGenerator.EnableDebugMesh();
            }
        }

        [MenuItem("GridGenerator/Export")]
        static void ExportGrid()
        {
            string scenePath = Path.GetDirectoryName(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path) + "/";
            string exportFolder = new DirectoryInfo(scenePath).Name;
            string exportPath = "Assets/StreamingAssets/Public/Scenes/" + exportFolder + "/";
            string sceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
            string fileName = sceneName + "_cow_walkable";
            string fileSubfix = ".map";
            string exportFileName = exportPath + fileName + fileSubfix;

            m_GridGenerator.ExportBin(exportFileName);

            UnityEngine.Debug.Log("Export walkable data to " + exportFileName);
        }

        [MenuItem("GridGenerator/Export", true)]
        static bool CheckExport()
        {
            if (m_GridGenerator != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // -----------------------------------------------
        [MenuItem("GridGenerator/Import")]
        static void ImportGrid()
        {
            string importPath = Path.GetDirectoryName(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path) + "/";
            string sceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
            string fileName = sceneName + "_cow_walkable";
            string fileSubfix = ".map";
            string importFileName = importPath + fileName + fileSubfix;

            using (BinaryReader reader = new BinaryReader(File.Open(importFileName, FileMode.Open)))
            {
                Vector2 unclampedGridSize;
                unclampedGridSize.x = reader.ReadSingle();
                unclampedGridSize.y = reader.ReadSingle();

                Debug.Log("unclampedGridSize : " + unclampedGridSize);
            }
        }

    }
}