//精英哥布林挥砍
skill(381401)
{
  section(1400)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_DaoGuang_03", 2000, "Bone_Root", 100);
    startcurvemove(630, true, 0.05, 0, 0, 6, 0, 0, 0);
    playsound(630, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    areadamage(680, 0, 1, 0.5, 3, false) {
      stateimpact("kDefault", 38140101);
    };
    playsound(690, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(740, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};


//精英哥布林旋风斩
skill(381402)
{
  section(1333)
  {
    movecontrol(true);
    animation("SwordS_01") {
      speed(1.5);
    };
    addimpacttoself(0, 38140200);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1500, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);//场景粒子在特殊位置仍然有不同步方向问题
    startcurvemove(266, true, 0.4, 0, 0, -0.07, 0, 0, -0.5);
  };
  section(1500)
  {
    animation("SwordS_02")
    {
      wrapmode(2);
    };
    startcurvemove(0, true, 1.5, 0, 0, 3.5, 0, 0, 0);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_SwordS_01", 1550, "Bone_Root", 0);
    playsound(10, "huiwu", "Sound/Npc/Mon_Loop", 1500, "Sound/Npc/guaiwu_xuanfengzhan_01", false);

    areadamage(100, 0, 1, 0.5, 2.5, true) {
      stateimpact("kDefault", 38140201);
    };
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    //shakecamera2(150, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));

    cleardamagestate(650);
    areadamage(700, 0, 1, 0.5, 2.5, true) {
      stateimpact("kDefault",38140201);
    };
    playsound(710, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    //shakecamera2(750, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));

    cleardamagestate(1250);
    areadamage(1300, 0, 1, 0.5, 2.5, true) {
      stateimpact("kDefault", 38140201);
    };
    playsound(1310, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    //shakecamera2(1350, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
    stopsound(1495, "huiwu");
  };
  section(1833)
  {
    animation("SwordS_03");
    startcurvemove(0, true, 0.8, 0, 0, 1.5, 0, 0, -2.5);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "huiwu");
  };
};


//投掷飞斧
skill(381403)
{
  section(1633)
  {
    animation("Skill_01");
    setchildvisible(770,"5_Mon_Bluelf_03_w_01",false);
    summonnpc(770, 101, "Monster/Campaign_Wild/01_Bluelf/5_Mon_Bluelf_03_w_01", 381404, vector3(0, 0, 0));
    setchildvisible(1600,"5_Mon_Bluelf_03_w_01",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_Bluelf_03_w_01",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_Bluelf_03_w_01",true);
  };
};


//飞斧
skill(381404)
{
  section(1410)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(10, 1400, vector3(0, 720, 0));
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Axe_01", 2000, "Bone011", 0)
    {
     transform(vector3(0,0,0),eular(0,0,0));
    };
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0.1,1.5,0.2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 0.7,0,0,14,0,0,0,0.7,0,0,-14,0,0,0);
    colliderdamage(10, 690, true, false, 0, 0)
    {
      stateimpact("kDefault", 38140301);
      stateimpact("kLauncher", 38140302);
      boneboxcollider(vector3(2, 1, 2), "Bone011", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu1", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38140301);
      stateimpact("kLauncher", 38140302);
      boneboxcollider(vector3(2, 1, 2), "Bone011", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1410);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
  };
};


//咆哮
skill(381405)
{
  //一段出招
  section(600)
  {
    animation("Skill_02A")
    {
      speed(0.7,true);
    };
    playsound(400, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_nuhou_01", false);
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_NuHou_01", 3000, vector3(0, 0.1, 0), 405, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(420, 0, 1, 0, 8, false) {
      stateimpact("kDefault", 38010401);
    };
  };
  section(367)
  {
    animation("Skill_02B")
    {
      speed(0.5,true);
    };
  };
};


//砸地
skill(381406)
{
  section(3433)
  {
    movecontrol(true);
    animation("Skill_03");
    setanimspeed(100,"Skill_03",0.2,true);
    setanimspeed(200,"Skill_03",1,true);
    findmovetarget(125, vector3(0,0,0),8,50,0.5,0.5,0,-1);
    startcurvemove(200, true, 0.43, 0, 6, 12, 0, -13, 0);
    playsound(667, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
    areadamage(660, 0, 1, 2, 4, false) {
      stateimpact("kDefault", 30011201);
    };
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_ZaDi_01", 1200, vector3(0,0,2), 667, eular(0,0,0), vector3(1,1,1));
    shakecamera2(670, 200, false, true, vector3(0.12,0.4,0.12), vector3(40,80,40),vector3(0.15,0.8,0.15),vector3(60,50,60));
  };
};


//出生技能
skill(381407)
{
	section(1200)
	{
		addimpacttoself(0, 38032199);
		movecontrol(true);
		animation("Skill_04")
		{
			wrapmode(2);
		};
		settransform(0, " ", vector3(0, 10, -5), eular(0, 0, 0), "RelativeSelf", false);
		startcurvemove(0, true, 1, 0, 0, 5, 0, -20, 0);

	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};