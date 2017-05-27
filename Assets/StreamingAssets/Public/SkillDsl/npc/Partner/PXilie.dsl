//爆气
skill(380501)
{
  section(1933)
  {
    animation("Skill_01");
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeSelf",false,true);
    addimpacttoself(0, 30010003, 3000);
    playsound(370, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_baoqi", false);
    areadamage(450, 0, 1, 0, 3, false) {
      stateimpact("kDefault", 38050101);
    };
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_BaoQi_01", 3000, vector3(0,1.2,0), 300, eular(0,0,0), vector3(1.5,1.5,1.5));
    shakecamera2(450,550,false,false,vector3(0.5,0.5,0.5),vector3(50,50,50),vector3(5,5,5),vector3(85,85,85));
  };
};

//开炮
skill(380502)
{
  section(1333)
  {
    movecontrol(true);
    animation("Attack_01");
    summonnpc(10, 101, "Monster/Campaign_Wild/01_Bluelf/SkillEmptyPrefab", 380503, vector3(0, 0, 0));
    playsound(780, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_kaipao", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 798, false);
    startcurvemove(798, true, 0.1, 0, 0, -4, 0, 0, 0);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    destroysummonnpc(0);
  };
};


//单发炮弹
skill(380503)
{
  section(1000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5,vector3(0,0,0),25,70,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false, true);
    setenable(20, "Visible", true);
    playsound(790, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    areadamage(800, 0, 1.2, 0, 3, false) {
      stateimpact("kDefault", 38050201);
      stateimpact("kLauncher", 38050202);
    };
    destroyself(4000);
    setenable(850, "Visible", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_03p", 800, vector3(0,0,0),800);
  };
};


//巨炮打击
skill(380504)
{
  //一段出招
  section(933)
  {
    animation("Skill_02A");
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_Xuli_01", 1900, "ef_weapon01", 0, true);
    shakecamera2(100, 3600, false, false, vector3(0.08,0.08,0.08), vector3(25,25,25),vector3(1.5,1.5,1.5),vector3(100,100,100));
    setanimspeed(10, "Skill_02A", 0.5, true);
    setanimspeed(930, "Skill_02A", 1, true);
  };
  //二段开炮
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    summonnpc(10, 101, "Monster/Campaign_Wild/01_Bluelf/SkillEmptyPrefab", 380505, vector3(0, 0, 0));
    playsound(140, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_jupaodaji_01", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
    shakecamera2(165, 200, false, false, vector3(0.3,0.3,0.3), vector3(100,100,100),vector3(2,2,2),vector3(60,60,60));
  };
  //三段收招
  section(400)
  {
    animation("Skill_02C");
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//巨炮落地
skill(380505)
{
  section(4000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5, vector3(0,0,0), 25, 360, 0.5, 0.5, 0, 0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    setenable(20, "Visible", true);
    areadamage(3000, 0, 1.5, 0, 4, false) {
      stateimpact("kDefault", 38050401);
      stateimpact("kLauncher", 38050402);
    };
    shakecamera2(3010,550,false,false,vector3(0.5,1.3,0.5),vector3(50,80,50),vector3(5,7,5),vector3(85,85,85));
    destroyself(4000);
    setenable(3005, "Visible", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_01p", 1500, vector3(0,0,0),2800);
    playsound(2950, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_jupaodaji_02", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_01p", 800, vector3(0,0,0),3000);
  };
};


//连发炮弹
skill(380506)
{
  //一段出招
  section(470)
  {
    animation("Skill_02A");
    setanimspeed(5,"Skill_02A",2);
  };
  //二段开炮
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    summonnpc(110, 101, "Monster/Campaign_Wild/01_Bluelf/SkillEmptyPrefab", 380507, vector3(0, 0, 0));
    playsound(146, "kaipao1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    playsound(146, "kaipao2", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    playsound(146, "kaipao3", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  //三段收招
  section(400)
  {
    animation("Skill_02C");
  };
};


//炮弹轰击
skill(380507)
{
  section(4000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    setenable(20, "Visible", true);
    //循环段
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),70);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),310);
    areadamage(320, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),750);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),990);
    areadamage(1000, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    //
    findmovetarget(1005,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(1010," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),1550);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),1790);
    areadamage(1800, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    //
    findmovetarget(1805,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(1810," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),2350);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),2590);
    areadamage(2600, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    //
    shakecamera2(320,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(1000,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(1800,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(2600,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    playsound(310, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(990, "baozha1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(1790, "baozha2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(2590, "baozha3", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    setenable(2600, "Visible", false);
    destroyself(3000);
  };
};



//霸体巨炮打击
skill(380508)
{
  //一段出招
  section(933)
  {
    animation("Skill_02A");
    addimpacttoself(0, 30010003, 2500);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_Xuli_01", 1900, "ef_weapon01", 0, true);
    shakecamera2(100, 3600, false, false, vector3(0.08,0.08,0.08), vector3(25,25,25),vector3(1.5,1.5,1.5),vector3(100,100,100));
    setanimspeed(10, "Skill_02A", 0.5, true);
    setanimspeed(930, "Skill_02A", 1, true);
  };
  //二段开炮
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    summonnpc(10, 101, "Monster/Campaign_Wild/01_Bluelf/SkillEmptyPrefab", 380509, vector3(0, 0, 0));
    playsound(140, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_jupaodaji_01", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
    shakecamera2(165, 200, false, false, vector3(0.3,0.3,0.3), vector3(100,100,100),vector3(2,2,2),vector3(60,60,60));
  };
  //三段收招
  section(400)
  {
    animation("Skill_02C");
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//霸体巨炮落地
skill(380509)
{
  section(4000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5, vector3(0,0,0), 25, 360, 0.5, 0.5, 0, 0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    setenable(20, "Visible", true);
    areadamage(3000, 0, 1.5, 0, 4, false) {
      stateimpact("kDefault", 38050401);
      stateimpact("kLauncher", 38050402);
    };
    shakecamera2(3010,550,false,false,vector3(0.5,1.3,0.5),vector3(50,80,50),vector3(5,7,5),vector3(85,85,85));
    destroyself(4000);
    setenable(3005, "Visible", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_01p", 1500, vector3(0,0,0),2800);
    playsound(2950, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_jupaodaji_02", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_01p", 800, vector3(0,0,0),3000);
  };
};


//霸体连发炮弹
skill(380510)
{
  //一段出招
  section(470)
  {
    animation("Skill_02A");
    setanimspeed(5,"Skill_02A",2);
    addimpacttoself(0, 30010003, 2000);
  };
  //二段开炮
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    summonnpc(110, 101, "Monster/Campaign_Wild/01_Bluelf/SkillEmptyPrefab", 380511, vector3(0, 0, 0));
    playsound(146, "kaipao1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    playsound(146, "kaipao2", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    playsound(146, "kaipao3", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  //三段收招
  section(400)
  {
    animation("Skill_02C");
  };
};


//霸体炮弹轰击
skill(380511)
{
  section(4000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    setenable(20, "Visible", true);
    //循环段
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),70);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),310);
    areadamage(320, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),750);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),990);
    areadamage(1000, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    //
    findmovetarget(1005,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(1010," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),1550);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),1790);
    areadamage(1800, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    //
    findmovetarget(1805,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(1810," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02p", 1500, vector3(0,0,0),2350);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02p", 800, vector3(0,0,0),2590);
    areadamage(2600, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38050601);
      stateimpact("kLauncher", 38050602);
    };
    //
    shakecamera2(320,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(1000,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(1800,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(2600,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    playsound(310, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(990, "baozha1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(1790, "baozha2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(2590, "baozha3", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    setenable(2600, "Visible", false);
    destroyself(3000);
  };
};


//出场
skill(380512)
{
	section(500)
	{
		addimpacttoself(0, 38032199);
		movecontrol(true);
		animation("PLand")
		{
			wrapmode(2);
		};

		settransform(0, " ", vector3(-10, 1, -10), eular(0, 0, 0), "RelativeSelf", false);
		startcurvemove(0, true, 0.5, 18, 30, 16.5, 0, -120, 0);
	};
	section(1500)
	{
		startcurvemove(0, true, 0.5, 0, -30, 0, 0, 0, 0);

		sceneeffect("Monster_FX/Campaign_Wild/07_OEOrc/6_Mon_OEOrc_ZhenDangBo_01",2500,vector3(0,0,0),200);
		playsound(190, "Land", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_baoqi", false);
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