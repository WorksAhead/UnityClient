

/****    普攻三段    ****/

skill(430003)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
  };

  section(55)//起手
  {
    animation("Cike_Hit_03_01")
    {
      speed(1.2);
    };
    //
    //角色移动
    //startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(110)//第一段
  {
    animation("Cike_Hit_03_02")
    {
      speed(1.2);
    };
     //角色移动
    //startcurvemove(0, true, 0.15, 0, 0, 10, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 0.8, 2, true)
		{
			stateimpact("kDefault", 43000301);
			stateimpact("kLauncher", 43000304);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_PuGong_03_001", 500, "Bone_Root", 1);
    //
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_06", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(110)//第二段
  {
    animation("Cike_Hit_03_03")
    {
      speed(1.2);
    };
    //
    //角色移动
    //startcurvemove(1, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 0.8, 2, true)
		{
			stateimpact("kDefault", 43000302);
			stateimpact("kLauncher", 43000305);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_PuGong_03_002", 500, "Bone_Root", 1);
    //
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_07", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(83)//第三段
  {
    animation("Cike_Hit_03_04")
    {
      speed(1.2);
    };
     //角色移动
    //startcurvemove(1, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 0.8, 2, true)
		{
			stateimpact("kDefault", 43000303);
			stateimpact("kLauncher", 43000306);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_PuGong_03_003", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_06", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(110)//硬直
  {
    animation("Cike_Hit_03_05")
    {
      speed(1.2);
    };
    //
    //角色移动
    //startcurvemove(66, true, 0.12, 0, 0, 20, 0, 0, 0);
  };
};
