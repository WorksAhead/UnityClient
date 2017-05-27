

/****    紫衣刃旋    ****/

skill(430201)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(666)//第三段
  {
    animation("Skill02_01_04")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.6, 0, 0, 8, 0, 0, 0);
    //
    //伤害判定
    areadamage(90, 0, 1.5, 0, 2, true)
		{
			stateimpact("kDefault", 43020105);
			stateimpact("kLauncher", 43020106);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(150, 0, 1.5, 0, 2, true)
		{
			stateimpact("kDefault", 43020105);
			stateimpact("kLauncher", 43020106);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(210, 0, 1.5, 0, 2, true)
		{
			stateimpact("kDefault", 43020105);
			stateimpact("kLauncher", 43020106);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(270, 0, 1.5, 0, 2, true)
		{
			stateimpact("kDefault", 43020105);
			stateimpact("kLauncher", 43020106);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
   areadamage(330, 0, 1.5, 0, 2, true)
		{
			stateimpact("kDefault", 43020105);
			stateimpact("kLauncher", 43020106);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
   areadamage(390, 0, 1.5, 0, 2, true)
		{
			stateimpact("kDefault", 43020107);
			stateimpact("kLauncher", 43020108);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_XuanZhuan_01", 2000, "Bone_Root", 100);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(466)//第四段
  {
    animation("Skill02_01_05")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kDefault", 43020109);
			stateimpact("kLauncher", 43020109);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    sceneeffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_ShangTiao_01", 3000, vector3(0, 0, 1), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

};
