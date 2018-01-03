using System;
using System.Collections.Generic;

namespace ArkCrossEngine
{
    class ClickNpcManager
    {
        public class ClickNpcInfo
        {
            public int LinkId = 0;
            public int StoryId = 0;
            public string UIName = "";
            public int Currency = 0;
            public string CurrencySprite = "";
        }
        public void Init()
        {
            m_NpcInfos.Clear();
            DBC dlgCfg = new DBC();
            if (dlgCfg.Load(HomePath.GetAbsolutePath(FilePathDefine_Client.C_ClickNpc)))
            {
                for (int index = 0; index < dlgCfg.RowNum; index++)
                {
                    DBC_Row node = dlgCfg.GetRowByIndex(index);
                    if (null != node)
                    {
                        ClickNpcInfo info = new ClickNpcInfo();
                        info.LinkId = DBCUtil.ExtractNumeric<int>(node, "LinkId", 0, true);
                        info.StoryId = DBCUtil.ExtractNumeric<int>(node, "StoryId", 0, false);
                        info.UIName = DBCUtil.ExtractString(node, "UIName", "", false);
                        info.Currency = DBCUtil.ExtractNumeric<int>(node, "Currency", 0, true);
                        info.CurrencySprite = DBCUtil.ExtractString(node, "CurrencySprite", "", false);
                        if (!m_NpcInfos.ContainsKey(info.LinkId))
                            m_NpcInfos.Add(info.LinkId, info);
                    }
                }
            }
        }

        private void OnStoryEnd(int storyId)
        {
            if (m_LastTriggerNpcLinkId > 0)
            {
                ClickNpcInfo click_npc = null;
                if (m_NpcInfos.TryGetValue(m_LastTriggerNpcLinkId, out click_npc))
                {
                    UIManager.Instance.SetAllUiVisible(true);
                    string ui_name = click_npc.UIName;
                    if (ui_name.Length > 0)
                        UIManager.Instance.ShowWindowByName(ui_name);
                }
            }
            m_LastTriggerNpcLinkId = 0;
        }

        public void Tick()
        {
            if (m_CurNpcActorId > 0)
            {
                SharedGameObjectInfo share_info = LogicSystem.GetSharedGameObjectInfo(m_CurNpcActorId);
                if (share_info != null)
                {
                    ArkCrossEngine.Vector3 actor_pos = new ArkCrossEngine.Vector3(share_info.X, share_info.Y, share_info.Z);
                    ArkCrossEngine.Vector3 assit_pos = CaclEndPos(actor_pos);
                    UnityEngine.Vector3 end_pos = new UnityEngine.Vector3(assit_pos.X, assit_pos.Y, assit_pos.Z);
                    bool ret = IsFadeIn(end_pos);
                    if (ret)
                    {
                        m_CurNpcActorId = 0;
                        TriggerLogic(share_info.LinkId);
                    }
                }
            }
        }

        private bool IsFadeIn(UnityEngine.Vector3 pos)
        {
            bool result = false;
            UnityEngine.GameObject player = LogicSystem.PlayerSelf;
            if (null != player)
                if (UnityEngine.Vector3.Distance(player.transform.position, new UnityEngine.Vector3(pos.x, pos.y, pos.z)) < 3.0f)
                    result = true;
            return result;
        }

        private Vector3 CaclEndPos(Vector3 tpos)
        {
            SharedGameObjectInfo user = LogicSystem.PlayerSelfInfo;
            if (null != user)
            {
                Vector3 spos = new Vector3(user.X, user.Y, user.Z);
                double dir = Math.Atan2(tpos.X - spos.X, tpos.Z - spos.Z);
                float dis = Vector3.Distance(spos, tpos) - 2.0f;
                Vector3 new_pos = new Vector3((float)Math.Sin(dir) * dis, 0f, (float)Math.Cos(dir) * dis);
                return spos + new_pos;
            }
            return Vector3.Zero;
        }

        public void Execute(int actor_id)
        {
            if (!WorldSystem.Instance.IsPureClientScene())
                return;
            SharedGameObjectInfo share_info = LogicSystem.GetSharedGameObjectInfo(actor_id);
            if (share_info == null)
                return;
            Vector3 actor_pos = new Vector3(share_info.X, share_info.Y, share_info.Z);
            Vector3 assit_pos = CaclEndPos(actor_pos);
            UnityEngine.Vector3 end_pos = new UnityEngine.Vector3(assit_pos.X, assit_pos.Y, assit_pos.Z);
            bool ret = IsFadeIn(end_pos);
            if (ret)
            {
                TriggerLogic(share_info.LinkId);
            }
            else
            {
                m_CurNpcActorId = share_info.m_ActorId;
                LogicSystem.SendStoryMessage("playermovetopos", end_pos.x, end_pos.y, end_pos.z);
            }
        }

        private void TriggerLogic(int link_id)
        {
            ClickNpcInfo click_npc = null;
            if (m_NpcInfos.TryGetValue(link_id, out click_npc))
            {
                int story_id = click_npc.StoryId;
                string ui_name = click_npc.UIName;
                int currency = click_npc.Currency;
                string currencysprite = click_npc.CurrencySprite;
                if (story_id > 0)
                {
                    UnityEngine.GameObject go = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
                    if (go != null)
                    {
                        GameLogic game_logic = go.GetComponent<GameLogic>();
                        if (game_logic != null)
                        {
                            UIManager.Instance.SetAllUiVisible(false);
                            m_LastTriggerNpcLinkId = link_id;
                            game_logic.TriggerStory(story_id);
                        }
                    }
                }
                else if (ui_name.Length > 0)
                {
                    if (currency != 0)
                    {
                        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName(ui_name);
                        if (go != null)
                        {
                            Store store = go.GetComponent<Store>();
                            if (store != null)
                            {
                                store.ChangeStore(currency, currencysprite);
                            }
                        }
                    }
                    UIManager.Instance.ShowWindowByName(ui_name);
                }
            }
        }

        private int m_LastTriggerNpcLinkId = 0;
        private Dictionary<int, ClickNpcInfo> m_NpcInfos = new Dictionary<int, ClickNpcInfo>();
        private int m_CurNpcActorId = 0;
        static private ClickNpcManager m_Instance = new ClickNpcManager();
        static public ClickNpcManager Instance
        {
            get { return m_Instance; }
        }
    }
}
