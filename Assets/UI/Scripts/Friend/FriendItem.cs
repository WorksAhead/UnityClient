using UnityEngine;

public class FriendItem : UnityEngine.MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        try
        {
            UIEventListener.Get(gameObject).onDrag = onDragItem;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*点击聊天*/
    public void OnClickChat()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_chat_friend", "friend", this.gameObject);
        Debug.Log("chat");
    }
    /*点击组队*/
    public void OnClickTeam()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_Team_friend", "friend", this.gameObject);
        Debug.Log("team");
    }
    /*点击删除好友*/
    public void OnClickDelete()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_Delete_friend", "friend", this.gameObject);
        Debug.Log("team");
    }
    /*drag好友item*/
    public void onDragItem(UnityEngine.GameObject go, UnityEngine.Vector2 vec)
    {
        if (vec.x < -15 && UnityEngine.Mathf.Abs(vec.y) < 6)
        {
            UnityEngine.Transform tf = transform.Find("zudui");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_drag_Item_friend", "friend", this.gameObject);
        }
    }
    /*点击好友item*/
    public void onClickItem()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_click_Item_friend", "friend", this.gameObject);
    }
}
