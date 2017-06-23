// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEditor;

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
    }
}