//剑盾兵上挑
skill(381301)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(1000)//起手
  {
    animation("Skill_02_01")
    {
      speed(1);
    };
  };

  section(500)//第一段
  {
    animation("Skill_02_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.2, true)
		{
			stateimpact("kDefault", 38130101);
			stateimpact("kKnockDown", 38130102);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_ShangTiao_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(866)//收招
  {
    animation("Skill_02_99")
    {
      speed(1);
    };
  };
};

//剑盾兵普攻
skill(381302)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(1066)//起手
  {
    animation("Attack_01_01")
    {
      speed(1);
    };
  };

  section(466)//第一段
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.2, true)
		{
			stateimpact("kDefault", 38130201);
			stateimpact("kKnockDown", 38130102);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_PuGong_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(400)//收招
  {
    animation("Attack_01_99")
    {
      speed(1);
    };
  };
};

//剑盾兵重击
skill(381303)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(800)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 1500);
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_XuLi_01", 3000, "ef_righthand", 1);
  };

  section(600)//起手2
  {
    animation("Skill_01_02")
    {
      speed(0.5);
    };
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 1500);
  };

  section(500)//第一段
  {
    animation("Skill_01_03")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.1, 0, 0, 16, 0, 0, -30);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kDefault", 38130301);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_ZhongJi_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(333)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
  };
};


//穿刺
skill(381304)
{
  section(1333)
  {
    movecontrol(true);
    animation("Skill_04");
    setanimspeed(50, "Skill_04", 0.3, true);
    setanimspeed(200, "Skill_04", 1, true);
    findmovetarget(40,vector3(0,0,0),8,30,0.5,0.5,0,0.3,false);
    facetotarget(50, 50, 0);
    startcurvemove(300, true, 0.3, 0, 0, 30, 0, 0, 0);
    playsound(290, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_chongji", false);
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_Tuowei_01",1000,"Bone_Root",300,false);
    colliderdamage(300, 320, true, true, 150, 3)
    {
      stateimpact("kDefault", 38130401);
      stateimpact("kLauncher", 38130401);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(320, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//格挡
skill(381305)
{
  section(367)
  {
    animation("Skill_03A");
    addimpacttoself(0, 30010003, 5033);
    addimpacttoself(0, 38130501, 4633);
    addimpacttoself(0, 38130502, 4633);
    findmovetarget(0, vector3(0,0,0),10,360,0.5,0.5,0,0,false,"friend");
    addimpacttotarget(30, 30010003, 5000);
    addimpacttotarget(30, 38130501, 5000);
  };
  section(4267)
  {
    movecontrol(true);
    findmovetarget(0, vector3(0,0,0),10,360,0.5,0.5,0,0);
    animation("Skill_03B")
    {
      wrapmode(2);
    };
    facetotarget(5, 4200, 100);
  };
  section(533)
  {
    animation("Skill_03C");
  };
};



//剑盾兵重击
skill(381306)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(800)//起手
  {
    animation("Skill_01_01")
    {
      speed(2);
    };
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 1500);
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_XuLi_01", 3000, "ef_righthand", 1);
  };

  section(600)//起手2
  {
    animation("Skill_01_02")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 1500);
  };

  section(500)//第一段
  {
    animation("Skill_01_03")
    {
      speed(1);
    };
    startcurvemove(1, true, 0.15, 0, 0, 20, 0, 0, -30);
    areadamage(10, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kDefault", 38130601);
		};
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_ZhongJi_01", 500, "Bone_Root", 1);
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
  section(333)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
  };
};


//出场
skill(381307)
{
	section(1000)
	{
		addimpacttoself(0, 38032199);
		movecontrol(true);
		animation("PLand")
		{
			wrapmode(2);
		};

		settransform(0, " ", vector3(0, 10, -5), eular(0, 0, 0), "RelativeSelf", false);
		startcurvemove(0, true, 1, 0, 0, 5, 0, -20, 0);

	};
	section(1533)
	{
		movecontrol(true);
		animation("Land");
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};