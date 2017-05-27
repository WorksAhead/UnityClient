using ArkCrossEngine;

public class GlobalStoryObject : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerselfScale(object[] args)
    {
        if (null != args && 3 == args.Length)
        {
            float x = (float)args[0];
            float y = (float)args[1];
            float z = (float)args[2];
            UnityEngine.GameObject playerself = CrossObjectHelper.TryCastObject < UnityEngine.GameObject > (LogicSystem.PlayerSelf);
            if (null != playerself)
            {
                playerself.transform.localScale = new UnityEngine.Vector3(x, y, z);
            }
        }
    }

    public void SetPlayerselfPosition(object[] args)
    {
        if (null != args && 3 == args.Length)
        {
            float x = (float)args[0];
            float y = (float)args[1];
            float z = (float)args[2];
            UnityEngine.GameObject playerself = LogicSystem.PlayerSelf._GetImpl() as UnityEngine.GameObject;
            if (null != playerself)
            {
                LogicSystem.NotifyGfxMoveControlStart(LogicSystem.PlayerSelf, 0, false);
                LogicSystem.NotifyGfxUpdatePosition(LogicSystem.PlayerSelf, x, y, z);
                playerself.transform.position = new UnityEngine.Vector3(x, y, z);
                LogicSystem.NotifyGfxMoveControlFinish(LogicSystem.PlayerSelf, 0, false);
            }
        }
    }
}
