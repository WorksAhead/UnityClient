//普攻
skill(381201)
{
  section(2533)
  {
    movecontrol(true);
    animation("Attack_01");
    setanimspeed(400, "Attack_01", 0.5, true);
    setanimspeed(800, "Attack_01", 1, true);
    findmovetarget(10,vector3(0,0,0),6,40,0.5,0.5,0,1);
    startcurvemove(850, false, 0.35, 0, 0, 10, 0, 0, 0);
    charactereffect("Monster_FX/Boss/SNK/6_Mon_SNK_DaoGuang_01", 2000, "Bone_Root", 1040);
    charactereffect("Monster_FX/Boss/SNK/6_Mon_SNK_Tuowei_01", 2000, "Bone_Root", 850, false);
    playsound(1040, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    // areadamage(1070, 0, 1, 1.5, 3.5, false) {
    //   stateimpact("kDefault", 38120101);
    // };
    colliderdamage(1000, 150, false, false, 0, 0)
    {
      stateimpact("kDefault", 38120101);
      sceneboxcollider(vector3(5, 3, 4), vector3(0, 1.5, 0), eular(0, 0, 0), true, false);
    };
    playsound(1080, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1080, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};


//普攻
skill(381202)
{
  section(2500)
  {
    animation("Attack_02");
    setanimspeed(330, "Attack_02", 0.5, true);
    setanimspeed(1000, "Attack_02", 1, true);
    charactereffect("Monster_FX/Boss/SNK/6_Mon_SNK_DaoGuang_02", 2000, "Bone_Root", 1070);
    playsound(1080, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    areadamage(1100, 0, 1, 1.5, 3.5, false) {
      stateimpact("kDefault", 38120201);
    };
    playsound(1110, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1110, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};


//天灾打击
skill(381203)
{
  section(2367)
  {
    animation("Skill_01");
    setanimspeed(500, "Skill_01", 0.5, true);
    setanimspeed(800, "Skill_01", 1, true);
    summonnpc(500, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 381211, vector3(0, 0, 0));
    charactereffect("Monster_FX/Boss/SNK/6_Mon_SNK_DaoGuang_03", 2000, "Bone_Root", 980);
    playsound(980, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    shakecamera2(1020, 200, false, false, vector3(0,3,0.5), vector3(0,100,50),vector3(0,2,0.5),vector3(0,80,50));
  };
};


//群体召唤
skill(381204)
{
  section(1933)
  {
    addimpacttoself(0, 30010003, 3000);
    charactereffect("Monster_FX/Boss/SNK/6_Mon_SNK_Zhaohuan_01", 1600, "ef_weapon", 720);
    animation("Skill_02");
    setanimspeed(667, "Skill_02", 0.2, true);
    setanimspeed(1000, "Skill_02", 1, true);
    summonnpc(667, 204206, "", 381207, vector3(0, 0, 0), eular(0, 0, 0), 20005, true, ",,300001");
    summonnpc(767, 204207, "", 381208, vector3(0, 0, 0), eular(0, 0, 0), 20005, true, ",,300002");
    summonnpc(867, 204207, "", 381209, vector3(0, 0, 0), eular(0, 0, 0), 20005, true, ",,300002");
    summonnpc(967, 204206, "", 381210, vector3(0, 0, 0), eular(0, 0, 0), 20005, true, ",,300001");
    shakecamera2(650, 1500, false, true, vector3(0.1,0.1,0.1), vector3(50,50,50),vector3(0.2,0.2,0.2),vector3(100,100,100));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
  };
};


//召唤法阵
skill(381205)
{
  section(1933)
  {
    animation("Skill_02");
    setanimspeed(667, "Skill_02", 0.5, true);
    setanimspeed(1000, "Skill_02", 1, true);
    charactereffect("Monster_FX/Boss/SNK/6_Mon_SNK_Zhaohuan_01", 600, "ef_weapon", 720);
    summonnpc(800, 101, "Monster_FX/Boss/SNK/6_Mon_SNK_Area_01", 381206, vector3(0, 0.3, 0), eular(0, 0, 0), 20005, true, ",,300007");
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
  };
};

//法阵技能
skill(381206)
{
  section(800)
  {
    findmovetarget(0, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 0, false, "friend");
    addimpacttotarget(100, 38120500);
    colliderdamage(100, 500, false, false, 0, 0)
    {
      stateimpact("kDefault", 38120501);
      sceneboxcollider(vector3(8, 3, 8), vector3(0, 1.5, 0), eular(0, 0, 0), false, false);
    };
  };
};


//骷髅出生技能
skill(381207)
{
  section(3000)
  {
    movecontrol(true);
    animation("Idle_01");
    setenable(0, Visible, false);
    settransform(10, " ", vector3( 0, 0.1, 3), eular(0, 0, 0), "RelativeOwner", false, true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_ChuSheng_01",1000,vector3(0,0.3,0),100);
    setenable(199,Visible, true);
  };
  oninterrupt()
  {
    setenable(0,Visible, true);
  };
};

skill(381208)
{
  section(3000)
  {
    movecontrol(true);
    animation("Idle_01");
    setenable(0, Visible, false);
    settransform(10, " ", vector3( 0, 0.1, -3), eular(0, 0, 0), "RelativeOwner", false, true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_ChuSheng_01",1000,vector3(0,0.3,0),100);
    setenable(199,Visible, true);
  };
  oninterrupt()
  {
    setenable(0,Visible, true);
  };
};

skill(381209)
{
  section(3000)
  {
    movecontrol(true);
    animation("Idle_01");
    setenable(0, Visible, false);
    settransform(10, " ", vector3( 3, 0.1, 0), eular(0, 0, 0), "RelativeOwner", false, true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_ChuSheng_01",1000,vector3(0,0.3,0),100);
    setenable(199,Visible, true);
  };
  oninterrupt()
  {
    setenable(0,Visible, true);
  };
};


skill(381210)
{
  section(3000)
  {
    movecontrol(true);
    animation("Idle_01");
    setenable(0, Visible, false);
    settransform(10, " ", vector3( -3, 0.1, 0), eular(0, 0, 0), "RelativeOwner", false, true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_ChuSheng_01",1000,vector3(0,0.3,0),100);
    setenable(199,Visible, true);
  };
  oninterrupt()
  {
    setenable(0,Visible, true);
  };
};


//天灾打击
skill(381211)
{
  section(1600)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5,vector3(0,0,0),15,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false, true);
    setenable(20, "Visible", true);
    areadamage(800, 0, 1.2, 0, 4, false) {
      stateimpact("kDefault", 38120301);
    };
    destroyself(1550);
    setenable(850, "Visible", false);
    sceneeffect("Monster_FX/Boss/SNK/6_Mon_SNK_Daji_01", 1500, vector3(0,0.2,0),780);
    playsound(770, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadida_01", false);
  };
};