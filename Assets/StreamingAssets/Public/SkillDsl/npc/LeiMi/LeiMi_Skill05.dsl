
/****    蕾咪 旋转    ****/

skill(440501)
{
//////////     一阶段
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(166)//起手
  {
    animation("Skill05_01_01");
    startcurvemove(100, true, 0.66, 0, 0, 15, 0, 0, 0);
  };

  section(366)//第一段
  {
    animation("Skill05_01_02");
    //
    //角色移动
    startcurvemove(0, true, 0.1, 0, 15, 15, 0, 0, 0);
    startcurvemove(100, true, 0.1, 0, 6, 6, 0, 0, 0);
    startcurvemove(200, true, 0.1, 0, 2, 2, 0, 0, 0);
    startcurvemove(300, true, 0.06, 0, 1, 1, 0, 0, 0);
    //
    //伤害判定
    areadamage(0, 0, 1.5, 1, 2.2, true)
		{
			stateimpact("kDefault", 44010501);
		};
    areadamage(100, 0, 1.5, 1, 2.2, true)
		{
			stateimpact("kDefault", 44010501);
		};
    areadamage(200, 0, 1.5, 1, 2.2, true)
		{
			stateimpact("kDefault", 44010501);
		};
    //
    //特效
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_01", 2000, "Bone_Root", 0);
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_ShanYao_01", false);
  };



//////////     二阶段
  section(300)
  {
    animation("Skill05_02_01");
    //角色移动
    startcurvemove(0, true, 0.05, 0, 8, 0, 0, 0, 0);
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_02_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(0, 0, 1.5, 0, 3.5, true)
    {
        stateimpact("kDefault", 12990000);
        stateimpact("kLauncher", 44010502);
    };
    playsound(10, "Hit2", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_ShanYao_02", false);
    //模型消失
    setenable(0, "Visible", false);
    //模型显示
    setenable(200, "Visible", true);
    //
  };
  section(300)
  {
    animation("Skill05_02_02");
    //角色移动
    startcurvemove(0, true, 0.05, 0, 8, 0, 0, 0, 0);
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_02_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(0, 0, 1.5, 0, 3.5, true)
    {
        stateimpact("kDefault", 12990000);
        stateimpact("kLauncher", 44010502);
    };
    playsound(10, "Hit3", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_ShanYao_02", false);
    //模型消失
    setenable(0, "Visible", false);
    //模型显示
    setenable(200, "Visible", true);
    //
  };
  section(300)
  {
    animation("Skill05_02_03");
    //角色移动
    startcurvemove(0, true, 0.05, 0, 8, 0, 0, 0, 0);
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_02_03", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(0, 0, 1.5, 0, 3.5, true)
    {
        stateimpact("kDefault", 12990000);
        stateimpact("kLauncher", 44010502);
    };
    playsound(10, "Hit4", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_ShanYao_02", false);
    //模型消失
    setenable(0, "Visible", false);
    //模型显示
    setenable(200, "Visible", true);
    //
  };
  section(300)

  {
    animation("Skill05_02_04");
    //角色移动
    startcurvemove(0, true, 0.05, 0, 8, 0, 0, 0, 0);
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_02_04", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(0, 0, 1.5, 0, 3.5, true)
    {
        stateimpact("kDefault", 12990000);
        stateimpact("kLauncher", 44010502);
    };
    playsound(10, "Hit5", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_ShanYao_02", false);
    //模型消失
    setenable(0, "Visible", false);
    //模型显示
    setenable(200, "Visible", true);
    //
  };



//////////     三阶段
  section(733)
  {
    animation("Skill05_03")
    {
        speed(1.5);
    };
    //角色移动
    startcurvemove(0, true, 0.1, 0, 8, 0, 0, 0, 0);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_03", 2000, "Bone_Root", 0);
  };

  section(400)
  {
    animation("Skill05_04")
    {
        speed(1.5);
    };
    //角色移动
    startcurvemove(0, true, 0.05, 0, 10, -12, 0, 0, 0);
    startcurvemove(50, true, 0.1, 0, 5, -6, 0, 0, 0);
    startcurvemove(150, true, 0.25, 0, 5, -6, 0, 0, 0);
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_04", 2000, "Bone_Root", 0);
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_05", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(0, 0, 1.5, 0, 3.5, true)
    {
        stateimpact("kLauncher", 44010503);
    };
    areadamage(150, 0, -5, 0, 3.5, true)
    {
        stateimpact("kDefault", 44010504);
    };
    //音频
    playsound(10, "Hit6", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_ShanYao_03", false);
  };

  section(833)
  {
    animation("Skill05_05");
    //角色移动
    startcurvemove(0, true, 0.8, 0, 5, 0, 0, -40, 0);
   sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill05_06", 3000, vector3(0, 0, 0), 700, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };

};

