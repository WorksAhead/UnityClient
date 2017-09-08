using UnityEngine;
using System.Collections.Generic;
using ArkCrossEngine;

public class Transparent : UnityEngine.MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        try
        {
            m_LayerMask = (1 << LayerMask.NameToLayer("Default"));
            m_TransprentShader = UnityEngine.Shader.Find("Transparent/Diffuse");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            UnityEngine.GameObject target = LogicSystem.PlayerSelf;
            if (null == target || null == m_TransprentShader)
            {
                return;
            }
            UnityEngine.Vector3 targetPos = target.transform.position;
            UnityEngine.Vector3 dir = targetPos - transform.position;
            float distance = dir.magnitude;

            m_CurRenderers.Clear();
            UnityEngine.RaycastHit[] hits;
            hits = UnityEngine.Physics.RaycastAll(transform.position, transform.forward, distance, m_LayerMask);
            int i = 0;
            while (i < hits.Length)
            {
                UnityEngine.RaycastHit hit = hits[i];
                UnityEngine.Renderer renderer = hit.collider.GetComponent<UnityEngine.Renderer>();
                if (renderer)
                {
                    if (!m_OriginalShaders.ContainsKey(renderer))
                    {
                        List<UnityEngine.Shader> shaders = new List<UnityEngine.Shader>();
                        for (int ix = 0; ix < renderer.materials.Length; ++ix)
                        {
                            UnityEngine.Material mat = renderer.materials[ix];
                            shaders.Add(mat.shader);
                        }
                        m_OriginalShaders.Add(renderer, shaders);
                    }
                    for (int ix = 0; ix < renderer.materials.Length; ++ix)
                    {
                        UnityEngine.Material mat = renderer.materials[ix];
                        mat.shader = m_TransprentShader;
                        UnityEngine.Color c = mat.color;
                        c.a = 0.3f;
                        mat.color = c;
                    }

                    m_CurRenderers.Add(renderer);
                    m_LastRenderers.Remove(renderer);
                }
                ++i;
            }
            foreach (UnityEngine.Renderer renderer in m_LastRenderers)
            {
                List<UnityEngine.Shader> shaders = m_OriginalShaders[renderer];
                if (null != shaders && shaders.Count == renderer.materials.Length)
                {
                    for (int ix = 0; ix < renderer.materials.Length; ++ix)
                    {
                        renderer.material.shader = shaders[ix];
                    }
                }
            }
            HashSet<UnityEngine.Renderer> temp = m_LastRenderers;
            m_LastRenderers = m_CurRenderers;
            m_CurRenderers = temp;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private int m_LayerMask = 0;
    private UnityEngine.Shader m_TransprentShader = null;
    private Dictionary<UnityEngine.Renderer, List<UnityEngine.Shader>> m_OriginalShaders = new Dictionary<UnityEngine.Renderer, List<UnityEngine.Shader>>();
    private HashSet<UnityEngine.Renderer> m_LastRenderers = new HashSet<UnityEngine.Renderer>();
    private HashSet<UnityEngine.Renderer> m_CurRenderers = new HashSet<UnityEngine.Renderer>();
}
