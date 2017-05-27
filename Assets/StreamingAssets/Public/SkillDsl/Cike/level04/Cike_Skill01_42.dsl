

/****    影刃 四阶    ****/

skill(120142)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(200)//起手
  {
    animation("Cike_Skill01_02_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(30, true, 0.2, 0, 0, 12, 0, 0, -40);
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 500);
    //
    //模型消失
    setenable(30, "Visible", false);
    //模型显示
    setenable(120, "Visible", true);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);

    //翅膀
    playpartanimation(0, "CiBang_02", "Open");
  };

  section(166)//第一段
  {
    animation("Cike_Skill01_02_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //召唤NPC  仅用于延迟伤害
    summonnpc(30, 101, "Hero/3_Cike/None", 120143, vector3(0, 0, 3));
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_TuZhanYiDuan_02", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_TuZhanYiDuan_02_01", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_01", 3000, vector3(-0.6, 0, 1.5), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill01_TuZhan_03", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Voice_RenXuan_02", false)
    {
	    audiogroup("None");
    };
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(266)//硬直
  {
    animation("Cike_Skill01_02_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 266, 3000);
    addbreaksection(10, 266, 3000);
    addbreaksection(21, 266, 3000);
    addbreaksection(100, 266, 3000);
  };

  section(500)//收招
  {
    animation("Cike_Skill01_02_99")
    {
      speed(1);
    };
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
    //翅膀
    playpartanimation(0, "CiBang_02", "Idle", 2);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
    //翅膀
    playpartanimation(0, "CiBang_02", "Idle", 2);
  };
};


skill(120143)
{
  section(450)
  {
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    //
    //伤害判定
    areadamage(1, 0, 1.5, 2, 3, true)
		{
			stateimpact("kDefault", 12010301);
			stateimpact("kLauncher", 12010303);
			stateimpact("kKnockDown", 12010303);
      //showtip(200, 0, 1, 1);
		};
    shakecamera2(40, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
    areadamage(60, 0, 1.5, 2, 2.8, true)
		{
			stateimpact("kDefault", 12010301);
			stateimpact("kLauncher", 12010303);
			stateimpact("kKnockDown", 12010303);
      //showtip(200, 0, 1, 1);
		};
    areadamage(120, 0, 1.5, 2, 2.8, true)
		{
			stateimpact("kDefault", 12010301);
			stateimpact("kLauncher", 12010303);
			stateimpact("kKnockDown", 12010303);
      //showtip(200, 0, 1, 1);
		};
    areadamage(180, 0, 1.5, 2, 3, true)
		{
			stateimpact("kDefault", 12010301);
			stateimpact("kLauncher", 12010303);
			stateimpact("kKnockDown", 12010303);
      //showtip(200, 0, 1, 1);
		};
    shakecamera2(220, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
    areadamage(240, 0, 1.5, 2.5, 3.2, true)
		{
			stateimpact("kDefault", 12010302);
			stateimpact("kLauncher", 12010304);
			stateimpact("kKnockDown", 12010304);
      //showtip(200, 0, 1, 1);
		};
  };
};
