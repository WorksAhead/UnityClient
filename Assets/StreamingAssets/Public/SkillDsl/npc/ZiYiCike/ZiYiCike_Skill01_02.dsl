

/****    紫衣升龙刃    ****/

skill(430102)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(1000)//第一段
  {
    animation("Skill01_02_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(200, true, 0.5, 0, 15, 0, 0, -50, 0);
    startcurvemove(700, true, 0.2, 0, 0, 0, 0, -100, 0);
    //
    //伤害判定
    areadamage(200, 0, 1.5, 0, 2.8, true)
		{
			stateimpact("kDefault", 43010201);
			stateimpact("kLauncher", 43010201);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(300, 0, 1.5, 0, 2.8, true)
		{
			stateimpact("kDefault", 43010201);
			stateimpact("kLauncher", 43010201);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(400, 0, 1.5, 0, 2.8, true)
		{
			stateimpact("kDefault", 43010201);
			stateimpact("kLauncher", 43010201);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(500, 0, 1.5, 0, 2.8, true)
		{
			stateimpact("kDefault", 43010201);
			stateimpact("kLauncher", 43010201);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(600, 0, 1.5, 0, 2.8, true)
		{
			stateimpact("kDefault", 43010201);
			stateimpact("kLauncher", 43010201);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_ShengLongRen_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};
