
/****    莱卡翁 普攻    ****/

skill(450101)
{
  section(588)//起手
  {
    movecontrol(true);
    animation("Skill01_01");
    {
      speed(3);
    };
    //目标选择
    findmovetarget(500, vector3(0, 0, 1), 3, 60, 0.1, 0.9, 0, -2);
  };

  section(244)//第一段
  {
    animation("Skill01_02");
    {
      speed(1.5);
    };
    //角色移动
    startcurvemove(0, true, 0.1, 0, 0, 8, 0, 0, 0);
    areadamage(10, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45010101);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(20, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_01", 1000, "Bone_Root", 10);
    //音效
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(20, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };

  section(422)//第二段
  {
    animation("Skill01_03");
    {
      speed(1.5);
    };
    areadamage(300, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45010102);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(300, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_03", 1000, "Bone_Root", 300);
    //音效
    playsound(300, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(300, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };

  section(266)//收手
  {
    animation("Skill01_04");
    {
      speed(1.5);
    };
  };
};


/****    莱卡翁 普攻强化    ****/

skill(450102)
{
  section(441)//起手
  {
    movecontrol(true);
    animation("Skill01_01");
    {
      speed(4);
    };
    //目标选择
    findmovetarget(400, vector3(0, 0, 1), 3, 60, 0.1, 0.9, 0, -2);
  };

  section(122)//第一段
  {
    animation("Skill01_02");
    {
      speed(3);
    };
    //角色移动
    startcurvemove(0, true, 0.1, 0, 0, 8, 0, 0, 0);
    areadamage(10, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45010101);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(20, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_01", 1000, "Bone_Root", 10);
    //音效
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(20, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };

  section(211)//第二段
  {
    animation("Skill01_03");
    {
      speed(3);
    };
    areadamage(150, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45010102);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(150, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_03", 1000, "Bone_Root", 150);
    //音效
    playsound(150, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(150, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };

  section(133)//收手
  {
    animation("Skill01_04");
    {
      speed(3);
    };
  };
};
