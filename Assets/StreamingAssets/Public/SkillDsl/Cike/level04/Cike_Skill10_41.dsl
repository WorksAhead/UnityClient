

/****    风神落 四阶    ****/

skill(121041)
{
  section(1)//初始化  0
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);

    addimpacttoself(0, 12990001, 700);
    //
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);
  };

  section(333)//起手  1
  {
    animation("Cike_Skill10_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(250, true, 0.05, 0, 0, 25, 0, 0, 0);
  };

  section(500)//第一段  2
  {
    animation("Cike_Skill10_01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1, 2.4, true)
		{
			stateimpact("kDefault", 12100101);
			stateimpact("kLauncher", 12100102);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_FengShenLuo_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill10_FengShenLuo_01", false);

    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true)
{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		}; 
    //
    //人工震屏
    movecamera(0, false, 0.05, 0, -24, 24, 0, 0, 0);
    setenable(0,  "CameraFollow", false);
    movecamera(50, false, 0.1, 0, 8, -8, 0, 0, 0);
    movecamera(150, false, 0.2, 0, 2, -2, 0, 0, 0);
    setenable(310,  "CameraFollow", true);
  };

  section(1)//第二段  3
  {
    //占位
  };

  section(433)//第三段  4
  {
    animation("Cike_Skill10_01_04")
    {
      speed(1);
    };
    //
    //人工震屏
    movecamera(0, false, 0.05, 0, -24, 24, 0, 0, 0);
    setenable(0,  "CameraFollow", false);
    movecamera(50, false, 0.1, 0, 8, -8, 0, 0, 0);
    movecamera(150, false, 0.2, 0, 2, -2, 0, 0, 0);
    setenable(310,  "CameraFollow", true);
    //
    //伤害判定
    areadamage(0, 0, 1.5, 1, 2, true)
		{
			stateimpact("kDefault", 12100105);
      //showtip(200, 0, 1, 0);
		};
    areadamage(50, 0, 1.5, 1, 2, true)
		{
			stateimpact("kDefault", 12100105);
      //showtip(200, 0, 1, 0);
		};
    areadamage(100, 0, 1.5, 1, 2, true)
		{
			stateimpact("kDefault", 12100105);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_FengShenLuo_03", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill10_FengShenLuo_02", false);
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Voice_FengShenLuo_01", false);

    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true)
{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		}; 
    //
  };

  section(1)//判断  5
  {
     //判断是否有目标
    findmovetarget(0, vector3(0, 1, 0.5), 2.5, 180, 0.8, 0.2, 0, 0, false)
    {
      filtsupperarmer();
    };
    //判断1
    gotosection(1, 8, 0)
    {
	    targetcondition(false);
    };
  };

  section(1901)//第四段 抓取  6
  {
    animation("Cike_Skill10_01_05")
    {
      speed(1);
    };
    //加霸体buff
    addimpacttoself(0, 12990001, 1800);
    //
    //镜头控制
    setenable(0,  "CameraFollow", false);
    movecamera(0, true, 0.1, 0, -20, 0, 0, 0, 0);
    rotatecamera(100, 200, vector3(-80, 0, 0));
    rotatecamera(300, 400, vector3(-5, 0, 0));
    rotatecamera(1100, 100, vector3(20, 0, 0));
    rotatecamera(1200, 100, vector3(160, 0, 0));
    //
    movecamera(1300, true, 0.05, 0, 40, 0, 0, 0, 0);
    //人工震屏
    //movecamera(1700, true, 0.2, 0, 10, 0, 0, 0, 0);
    movecamera(1350, false, 0.05, 0, -72, 72, 0, 0, 0);
    setenable(1350,  "CameraFollow", false);
    movecamera(1400, false, 0.1, 0, 24, -24, 0, 0, 0);
    movecamera(1500, false, 0.2, 0, 6, -6, 0, 0, 0);
    //movecamera(1700, true, 0.2, 0, 10, 0, 0, 0, 0);
    setenable(1900,  "CameraFollow", true);
    //
    //判断是否有目标
    findmovetarget(100, vector3(0, 1, 0.5), 3, 120, 0.8, 0.2, 0, 0, false)
    {
      filtsupperarmer();
    };
    //
    //给目标增加抓取buff
		addimpacttotarget(100, 12990006, 5000);
    //
    //抓取
		grabtarget(110, true, "ef_sign", "ef_body", 300);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kLauncher", 12100106);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_FengShenLuo_04", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit22", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Voice_FengShenLuo_03", false);

    playsound(10, "Hit23", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true)
{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		}; 

    playsound(1300, "Hit52", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Voice_FengShenLuo_02", false);
    playsound(1300, "Hit21", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill10_FengShenLuo_03", false);

    //伤害判定
    areadamage(1320, 0, 1.5, 0, 3.5, true)
    {
			stateimpact("kDefault", 12100107);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
    };

    //释放目标
		addimpacttotarget(1433, 12990007, 5000);
		grabtarget(1434, false);

    //伤害判定
    areadamage(1440, 0, 1.5, 0, 2, true)
    {
			stateimpact("kDefault", 12100108);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
    };

    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_FengShenLuo_05", 3000, vector3(0, 0, 0), 1230, eular(0, 0, 0), vector3(1, 1, 1), true);

    //震屏
		shakecamera2(1320, 200, false, true, vector3(1, 1.6, 0), vector3(40, 40, 0), vector3(36, 36, 0), vector3(80, 60, 0));
  };

  section(578)//收招    7
  {
    animation("Cike_Skill10_01_06")
    {
      speed(1.5);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.2, 0, 0, -15, 0, 0, 30);

    //
    //打断
    addbreaksection(1, 500, 1000);
    addbreaksection(10, 400, 1000);
    addbreaksection(11, 400, 1000);
    addbreaksection(21, 400, 1000);
    addbreaksection(100, 400, 1000);
    //
    gotosection(570, 9, 0);
  };

  section(578)//收招    8
  {
    animation("Cike_Skill10_01_06")
    {
      speed(1.5);
    };
    //
    //角色移动
    //startcurvemove(0, true, 0.2, 0, 0, -15, 0, 0, 30);

    //
    //打断
    addbreaksection(1, 500, 1000);
    addbreaksection(10, 400, 1000);
    addbreaksection(11, 400, 1000);
    addbreaksection(21, 400, 1000);
    addbreaksection(100, 400, 1000);
    //
  };

  section(1)//收招    9
  {
    //占位
  };

  oninterrupt()
	{
    setenable(0, "CameraFollow", true);
		addimpacttotarget(0, 12990007, -1);
		grabtarget(0, false);
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
	};

	onstop()
	{
    setenable(0, "CameraFollow", true);
		addimpacttotarget(0, 12990007, -1);
		grabtarget(0, false);
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
	};
};
