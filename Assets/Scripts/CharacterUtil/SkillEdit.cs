using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SkillEdit : MonoBehaviour
{
    [System.Serializable]
    public class PlayerActionCfg
    {
        public string idleAnim;
        public string walkAnim;
        public string runAnim;
        public string hurtAnim;
        public string deadAnim;
        public string getupAnim;
        public string airupAnim;
        public string airdownAnim;
        public string airhurtAnim;
        public string airgroundAnim;
    }
    public PlayerActionCfg 角色行动动画配置;

    public enum eHitType
    {
        普通受击,
        击飞
    }

    [System.Serializable]
    public class SkillCfg
    {
        public string 技能动画;
        public eHitType 攻击类型;
    }
    public SkillCfg 普通攻击第一段;
    public SkillCfg 普通攻击第二段;
    public SkillCfg 普通攻击第三段;
    public SkillCfg[] 技能配置 = new SkillCfg[6];

        // Use this for initialization
        void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private bool _SetActionAnim(string s, string anim, int id, int[] positions, out string newLine)
    {
        string[] actions = s.Split(new string[] { "\t" }, System.StringSplitOptions.None);
        int idInLine = -1;

        if (!int.TryParse(actions[0], out idInLine) || idInLine != id)
        {
            newLine = "";
            return false;
        }

        foreach (int pos in positions)
        {
            actions[pos] = anim;
        }

        newLine = actions[0];
        for (int i=1; i<actions.Length; ++i)
        {
            newLine += '\t';
            newLine += actions[i];
        }

        return true;
    }

    public void SaveSelected()
    {
#if UNITY_EDITOR
        // Get player id
        int id = -1;
        int normalAttackSkillId = -1;
        List<int> skillList = new List<int>(); 

        ArkCrossEngine.FileReaderProxy.MakeSureAllHandlerRegistered();
        ArkCrossEngine.PlayerConfigProvider.Instance.Clear();
        ArkCrossEngine.PlayerConfigProvider.Instance.LoadPlayerConfig(Application.dataPath + "\\StreamingAssets\\Public\\PlayerConfig.txt", "PlayerConfig");
        var players = ArkCrossEngine.PlayerConfigProvider.Instance.PlayerConfigMgr.GetData();
        foreach (var p in players)
        {
            ArkCrossEngine.Data_PlayerConfig cfg = (ArkCrossEngine.Data_PlayerConfig)p.Value;
            
            if (cfg.m_Model.Contains(gameObject.name))
            {
                id = cfg.m_ActionList[0];
                normalAttackSkillId = cfg.m_FixedSkillList[1];
                skillList = cfg.m_PreSkillList;
                break;
            }
        }
        // Get npc id
        if (id == -1)
        {
            ArkCrossEngine.NpcConfigProvider.Instance.Clear();
            ArkCrossEngine.NpcConfigProvider.Instance.LoadNpcConfig(Application.dataPath + "\\StreamingAssets\\Public\\NpcConfig.txt", "NPCConfig");

            var npcs = ArkCrossEngine.NpcConfigProvider.Instance.NpcConfigMgr.GetData();
            foreach (var npc in npcs)
            {
                ArkCrossEngine.Data_NpcConfig cfg = (ArkCrossEngine.Data_NpcConfig)npc.Value;

                if (cfg.m_Model.Contains(gameObject.name))
                {
                    id = cfg.m_ActionList[0];
                    break;
                }
            }
        }

        // Error
        if (id == -1)
        {
            UnityEditor.EditorUtility.DisplayDialog("SkillEdit", "没有找到当前选中角色的id，请联系程序！", "OK");
            return;
        }

        // Read ActionConfig.txt
        string[] lineTexts = File.ReadAllLines(Application.dataPath + "\\StreamingAssets\\Public\\ActionConfig.txt");
        for(int i=1; i<lineTexts.Length; ++i)
        {
            string newLine;
            // idle
            if(_SetActionAnim(lineTexts[i], 角色行动动画配置.idleAnim, id, new int[] { 8, 9, 10, 11, 12, 16 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // walk
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.walkAnim, id, new int[] { 13 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // run
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.runAnim, id, new int[] { 14,15,17 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // hurt
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.hurtAnim, id, new int[] { 18, 19, 20 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // dead
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.deadAnim, id, new int[] { 22 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // getup
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.getupAnim, id, new int[] { 28, 29 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // airup
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.airupAnim, id, new int[] { 30 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // airdown
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.airdownAnim, id, new int[] { 31 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // airhurt
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.airhurtAnim, id, new int[] { 32 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
            // airground
            if (_SetActionAnim(lineTexts[i], 角色行动动画配置.airgroundAnim, id, new int[] { 33 }, out newLine))
            {
                lineTexts[i] = newLine;
            }
        }


        // Save
        StreamWriter sw = new StreamWriter(Application.dataPath + "\\StreamingAssets\\Public\\ActionConfig.txt", false, Encoding.UTF8);

        foreach (string s in lineTexts)
        {
            sw.WriteLine(s);
        }
        sw.Flush();
        sw.Close();

        // Load skill files
        ArkCrossEngine.SkillConfigProvider.Instance.Clear();
        ArkCrossEngine.SkillConfigProvider.Instance.CollectData(ArkCrossEngine.SkillConfigType.SCT_SKILL, "Assets\\StreamingAssets\\Public\\Skill\\SkillData.txt", "SkillConfig");

        // Normal attack skill
        _GenerateSkillFile(normalAttackSkillId, 普通攻击第一段.技能动画, 普通攻击第一段.攻击类型, true);

        // Other skills
        int[] activeSkillLst = new int[6];
        foreach (int skillId in skillList)
        {
            var cat = _GetSkillCategory(skillId);
            if (cat >= ArkCrossEngine.SkillCategory.kSkillA && cat <= ArkCrossEngine.SkillCategory.kSkillE)
            {
                activeSkillLst[cat- ArkCrossEngine.SkillCategory.kSkillA] = skillId;
            }
        }

        int idx = 0;
        foreach (int skillId in activeSkillLst)
        {
            if (skillId > 0)
            {
                SkillCfg c = 技能配置[idx++];
                if (c.技能动画.Length > 0)
                {
                    _GenerateSkillFile(skillId, c.技能动画, c.攻击类型, false);
                }
            }
        }

        UnityEditor.EditorUtility.DisplayDialog("SkillEdit", "保存完毕！", "OK");
#endif
    }

    private ArkCrossEngine.SkillCategory _GetSkillCategory(int skillId)
    {
        ArkCrossEngine.SkillLogicData skillData = (ArkCrossEngine.SkillLogicData)ArkCrossEngine.SkillConfigProvider.Instance.ExtractData(ArkCrossEngine.SkillConfigType.SCT_SKILL, skillId);
        if (skillData == null)
        {
            return ArkCrossEngine.SkillCategory.kNone;
        }

        return skillData.Category;
    }

    private void _GenerateSkillImpl(StreamWriter sw, int skillId, string anim, eHitType hitType)
    {
        string str = "skill(" + skillId.ToString() + ")";
        sw.WriteLine(str);
        sw.WriteLine("{");
        sw.WriteLine("\tsection(450)");
        sw.WriteLine("\t{");

        str = "\t\t";
        str += "animation(\"";
        str += anim;
        str += "\");";
        sw.WriteLine(str);

        str = "\t\t";
        str += "areadamage(285, 0, 1.7, 1, 2.3, true)";
        sw.WriteLine(str);
        sw.WriteLine("\t\t{");
        if (hitType == eHitType.普通受击)
        {
            sw.WriteLine("\t\t\tstateimpact(\"kDefault\", 16000101);");
        }
        else
        {
            sw.WriteLine("\t\t\tstateimpact(\"kDefault\", 16020101);");
        }
        sw.WriteLine("\t\t};");

        sw.WriteLine("\t};");
        sw.WriteLine("};");
    }

    private void _GenerateSkillFile(int skillId, string anim, eHitType hitType, bool bNormalAttack)
    {
        const string skillfilePrefix = "Assets\\StreamingAssets\\Public\\SkillDsl\\";
        ArkCrossEngine.SkillLogicData skillData = (ArkCrossEngine.SkillLogicData)ArkCrossEngine.SkillConfigProvider.Instance.ExtractData(ArkCrossEngine.SkillConfigType.SCT_SKILL, skillId);
        if (skillData != null)
        {

            string skillFilePath = skillfilePrefix + skillData.SkillDataFile;
            StreamWriter skillFilesw = new StreamWriter(skillFilePath, false, Encoding.UTF8);

            _GenerateSkillImpl(skillFilesw, skillId, anim, hitType);

            // 三段普攻
            if (bNormalAttack)
            {
                // 第二段
                if (skillData.NextSkillId > 0)
                {
                    _GenerateSkillImpl(skillFilesw, skillData.NextSkillId, 普通攻击第二段.技能动画, 普通攻击第二段.攻击类型);
                } 
                else
                {
                    UnityEditor.EditorUtility.DisplayDialog("SkillEdit", "普攻第二段next skill id没配置，请联系程序！", "OK");
                }

                // 第三段
                skillData = (ArkCrossEngine.SkillLogicData)ArkCrossEngine.SkillConfigProvider.Instance.ExtractData(ArkCrossEngine.SkillConfigType.SCT_SKILL, skillData.NextSkillId);

                if (skillData.NextSkillId > 0)
                {
                    _GenerateSkillImpl(skillFilesw, skillData.NextSkillId, 普通攻击第三段.技能动画, 普通攻击第三段.攻击类型);
                }
                else
                {
                    UnityEditor.EditorUtility.DisplayDialog("SkillEdit", "普攻第三段next skill id没配置，请联系程序！", "OK");
                }
            }

            skillFilesw.Flush();
            skillFilesw.Close();
        }
    }
}


