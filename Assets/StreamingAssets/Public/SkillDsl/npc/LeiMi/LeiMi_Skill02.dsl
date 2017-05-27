

/****    蕾咪 小冲    ****/

skill(440201)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(300)//起手
  {
    animation("Skill02_01");
  };

  section(200)//第一段
  {
    animation("Skill02_02");
    //
    //模型消失
    setenable(30, "Visible", false);
    //
    //角色移动
    startcurvemove(0, true, 0.2, 0, 0, 20, 0, 0, 0);
    //
    //召唤NPC  仅用于延迟伤害
    summonnpc(0, 101, "Hero/3_Cike/None", 4402011, vector3(0, 0, 0));
    summonnpc(150, 101, "Hero/3_Cike/None", 4402011, vector3(0, 0, 0));
    //
    //特效
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill02_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/LeiMi/boss_LeiMi_XiaoChong_01", false);
  };

  section(600)//收招
  {
    settransform(0, " ", vector3(0, 0, 0), eular(0, 180, 0), "RelativeSelf", true);
    setenable(30, "Visible", true);
    animation("Skill02_03")
    {
        speed(2);
    };
    //特效
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill02_03", 3000, vector3(0, 0, 0), 60, eular(0, 0, 0), vector3(1, 1, 1), true);
    //角色移动
    startcurvemove(0, true, 0.4, 0, 0, -20, 0, 0, 50);
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


skill(4402011)
{
  section(450)
  {
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    //
    //伤害判定
    areadamage(30, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
      //showtip(200, 0, 1, 1);
		};
    areadamage(90, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
		};
    areadamage(150, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
		};
    areadamage(210, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
		};
    areadamage(210, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
		};
    areadamage(330, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
		};
    areadamage(390, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
		};
    areadamage(420, 0, 1, 0, 2.4, true)
		{
			stateimpact("kDefault", 44010201);
		};
    //特效
    sceneeffect("Monster_FX/LeiMi/6_Mon_LeiMi_Skill02_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
};
