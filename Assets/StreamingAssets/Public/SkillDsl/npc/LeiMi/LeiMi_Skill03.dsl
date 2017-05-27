
/****    蕾咪 旋转    ****/

skill(440301)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(100)//起手
  {
    animation("Skill03_01")
    {
        speed(2);
    }
  };

  section(533)//第一段
  {
    animation("Skill03_02");
    //伤害判定
    areadamage(0, 0, 1.5, 0, 3.5, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(50, 0, 1.5, 0, 3.2, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(100, 0, 1.5, 0, 3.2, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(150, 0, 1.5, 0, 3.2, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(200, 0, 1.5, 0, 3.2, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(250, 0, 1.5, 0, 3.2, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(300, 0, 1.5, 0, 3.2, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(350, 0, 1.5, 0, 3.2, true)
		{
			stateimpact("kDefault", 44010301);
			stateimpact("kLauncher", 44010302);
		};
    areadamage(400, 0, 1.5, 0, 3.5, true)
		{
			stateimpact("kDefault", 44010303);
			stateimpact("kLauncher", 44010302);
		};

    charactereffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill03_01", 2000, "Bone_Root", 0);
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_XuanZhuan_01", false);
  };

  section(533)//收招
  {
    animation("Skill03_03");
  };
};

