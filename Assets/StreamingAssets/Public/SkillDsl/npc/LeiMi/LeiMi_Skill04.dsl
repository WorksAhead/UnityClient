

/****    蕾咪 连击    ****/

skill(440401)
{

  section(1)
  {
    movecontrol(true);
    //目标选择
    findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, -3);
  };

  section(266)//第一段
  {
    animation("Skill04_01");
    //
    //角色移动
    startcurvemove(100, true, 0.15, 0, 0, 12, 0, 0, 0);
  };

  section(500)//第二段
  {
    animation("Skill04_02");
    //
    //角色移动
    startcurvemove(100, true, 0.4, 0, 0, 8, 0, 0, 0);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.6, true)
		{
			stateimpact("kDefault", 44010401);
			stateimpact("kLauncher", 44010402);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill04_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_TuJi_01", false);
    //
    //伤害判定
    areadamage(490, 0, 1.5, 1.5, 2.6, true)
		{
			stateimpact("kDefault", 44010403);
			stateimpact("kLauncher", 44010404);
			stateimpact("kKnockDown", 12990000);
		};
    //
    //特效
    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill04_02", 500, "Bone_Root", 470);
    //
    //音效
    playsound(490, "Hit2", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_TuJi_02", false);
  };

  section(633)//第三段
  {
    animation("Skill04_03");
    //
    //角色移动
    startcurvemove(0, true, 0.2, 0, 0, 6, 0, 0, 0);
    startcurvemove(200, true, 0.6, 0, 0, 3, 0, 0, 0);
  };

  section(566)//第四段
  {
    animation("Skill04_04");
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kDefault", 44010405);
		};
    //
    //特效
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill04_04", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit3", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_TuJi_03", false);
  };

};
