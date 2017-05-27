namespace GfxModule.Skill.Script
{
    public class ColliderTriger : UnityEngine.MonoBehaviour
    {
        public void SetOnTriggerEnter(ArkCrossEngine.MyAction<UnityEngine.Collider> onEnter)
        {
            m_OnTrigerEnter += onEnter;
        }
        public void SetOnTriggerExit(ArkCrossEngine.MyAction<UnityEngine.Collider> onExit)
        {
            m_OnTrigerExit += onExit;
        }

        internal void OnTriggerEnter(UnityEngine.Collider collider)
        {
            try
            {
                if (null != m_OnTrigerEnter)
                    m_OnTrigerEnter(collider);
            }
            catch (System.Exception ex)
            {
                ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        internal void OnTriggerExit(UnityEngine.Collider collider)
        {
            try
            {
                if (null != m_OnTrigerExit)
                    m_OnTrigerExit(collider);
            }
            catch (System.Exception ex)
            {
                ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private ArkCrossEngine.MyAction<UnityEngine.Collider> m_OnTrigerEnter;
        private ArkCrossEngine.MyAction<UnityEngine.Collider> m_OnTrigerExit;
    }
}
