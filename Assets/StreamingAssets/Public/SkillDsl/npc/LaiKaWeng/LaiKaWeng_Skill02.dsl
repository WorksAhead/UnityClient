
/****    莱卡翁 跳跃攻击    ****/

skill(450201)
{
  section(1)//
  {
    movecontrol(true);
    //目标选择
    findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, -3);
  };

  section(533)//起手
  {
    animation("Skill02_01");
    {
      speed(1.5);
    };
    //角色移动
    startcurvemove(10, true, 0.4, 0, 0, 8, 0, 0, 0);
  };

  section(333)//第一段
  {
    animation("Skill02_02");
    {
      speed(1.5);
    };
    areadamage(10, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45020101);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(20, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_02", 1000, "Bone_Root", 10);
    //音效
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(20, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };

  section(577)//第二段
  {
    animation("Skill02_03");
    {
      speed(1.5);
    };
    areadamage(10, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45020102);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(20, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_03", 1000, "Bone_Root", 60);
    //音效
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(20, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };
};


/****    莱卡翁 跳跃攻击强化    ****/

skill(450202)
{
  section(1)//
  {
    movecontrol(true);
    //目标选择
    findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, -3);
  };

  section(320)//起手
  {
    animation("Skill02_01");
    {
      speed(2.5);
    };
    //角色移动
    startcurvemove(10, true, 0.3, 0, 0, 10, 0, 0, 0);
  };

  section(200)//第一段
  {
    animation("Skill02_02");
    {
      speed(2.5);
    };
    areadamage(10, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45020101);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(20, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_02", 1000, "Bone_Root", 10);
    //音效
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(20, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };

  section(346)//第二段
  {
    animation("Skill02_03");
    {
      speed(2.5);
    };
    areadamage(10, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 45020102);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(20, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_03", 1000, "Bone_Root", 60);
    //音效
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(20, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_PuGong_01", false);
  };
};