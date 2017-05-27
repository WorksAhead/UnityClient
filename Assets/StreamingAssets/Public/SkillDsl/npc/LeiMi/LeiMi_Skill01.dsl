
/****    普攻一段    ****/
skill(440101)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(150)//起手
  {
    animation("Skill01_01_01")
    {
        speed(2);
    };
    //角色移动
    startcurvemove(80, true, 0.06, 0, 0, 12, 0, 0, 0);
  };

  section(166)//第一段
  {
    animation("Skill01_01_02");
     //角色移动
    startcurvemove(0, true, 0.1, 0, 0, 15, 0, 0, -100);
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.2, true)
		{
			stateimpact("kDefault", 44010101);
			stateimpact("kLauncher", 44010103);
			stateimpact("kKnockDown", 12990000);
		};
    //特效
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_01", 2000, "Bone_Root", 1);
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);

    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true)
    {
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
    };
  };

  section(200)//第二段
  {
    animation("Skill01_01_03");
     //角色移动
    startcurvemove(10, true, 0.03, 0, 0, 10, 0, 0, 0);
    //伤害判定
    areadamage(1, 0, 1.5, 0.8, 2.2, true)
		{
			stateimpact("kDefault", 44010102);
			stateimpact("kLauncher", 44010104);
			stateimpact("kKnockDown", 12990000);
		};
    //特效
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_02", 2000, "Bone_Root", 1);
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);

    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true)
    {
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
    };
  };
};

/****    普攻二段    ****/
skill(440102)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(100)//起手
  {
    animation("Skill01_02_01")
    {
        speed(2);
    };
    //角色移动
    startcurvemove(80, true, 0.06, 0, 0, 12, 0, 0, 0);
  };

  section(1166)//第一段
  {
    animation("Skill01_02_02");
     //角色移动
    startcurvemove(0, true, 0.1, 0, 0, 15, 0, 0, -100);
    //伤害判定
    areadamage(10, 0, 1.5, 1.8, 2, true)
		{
			stateimpact("kDefault", 44010105);
			stateimpact("kLauncher", 44010106);
			stateimpact("kKnockDown", 12990000);
		};
    areadamage(200, 0, 1.5, 1.8, 2, true)
		{
			stateimpact("kDefault", 44010105);
			stateimpact("kLauncher", 44010106);
			stateimpact("kKnockDown", 12990000);
		};
    areadamage(400, 0, 1.5, 1.8, 2, true)
		{
			stateimpact("kDefault", 44010105);
			stateimpact("kLauncher", 44010106);
			stateimpact("kKnockDown", 12990000);
		};
    areadamage(600, 0, 1.5, 1.8, 2, true)
		{
			stateimpact("kDefault", 44010105);
			stateimpact("kLauncher", 44010106);
			stateimpact("kKnockDown", 12990000);
		};
    areadamage(800, 0, 1.5, 1.8, 2, true)
		{
			stateimpact("kDefault", 44010105);
			stateimpact("kLauncher", 44010106);
			stateimpact("kKnockDown", 12990000);
		};
    areadamage(950, 0, 1.5, 1.8, 2, true)
		{
			stateimpact("kDefault", 44010105);
			stateimpact("kLauncher", 44010106);
			stateimpact("kKnockDown", 12990000);
		};
    areadamage(1100, 0, 1.5, 1.8, 2, true)
		{
			stateimpact("kDefault", 44010107);
			stateimpact("kLauncher", 44010108);
			stateimpact("kKnockDown", 12990000);
		};
    //特效
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03", 200, "Bone_Root", 10);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04", 200, "Bone_Root", 100);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03_1", 200, "Bone_Root", 200);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04_2", 200, "Bone_Root", 300);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03", 200, "Bone_Root", 400);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04_2", 200, "Bone_Root", 500);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03_2", 200, "Bone_Root", 600);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04_1", 200, "Bone_Root", 700);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03_1", 200, "Bone_Root", 800);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04", 200, "Bone_Root", 900);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03_2", 200, "Bone_Root", 1000);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04", 200, "Bone_Root", 1100);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03", 200, "Bone_Root", 50);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04_1", 200, "Bone_Root", 150);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03_1", 200, "Bone_Root", 250);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04", 200, "Bone_Root", 350);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03", 200, "Bone_Root", 450);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04_2", 200, "Bone_Root", 550);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03_2", 200, "Bone_Root", 650);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04", 200, "Bone_Root", 750);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03_1", 200, "Bone_Root", 850);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04_2", 200, "Bone_Root", 950);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_03", 2000, "Bone_Root", 1050);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_04", 2000, "Bone_Root", 1150);
    //音效
    playsound(10, "Hit2", "Sound/Cike/CikeSkillSound01", 3000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_02", false);
  };
};

/****    普攻三段    ****/
skill(440103)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(466)//起手
  {
    animation("Skill01_03_01");
    //角色移动
    startcurvemove(80, true, 0.06, 0, 0, 12, 0, 0, 0);
  };

  section(633)//第一段
  {
    animation("Skill01_03_02");
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //伤害判定
    areadamage(10, 0, 1.5, 1.2, 2.4, true)
		{
			stateimpact("kDefault", 44010109);
			stateimpact("kLauncher", 44010110);
		};
    //特效
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_PuGong_05", 2000, "Bone_Root", 1);
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 3000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_03", false);

    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true)
    {
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
    };
  };
};

