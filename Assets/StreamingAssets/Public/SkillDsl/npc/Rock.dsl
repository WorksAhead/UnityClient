//普攻
skill(320601)
{
  section(2700)
  {
    animation("Attack_01");
    setanimspeed(300, "Attack_01", 0.4, true);
    setanimspeed(800, "Attack_01", 1, true);
    areadamage(1000, 0, 1, 2, 3.5, true) {
      stateimpact("kDefault", 32060101);
    };
    shakecamera2(1530, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,100));
    playsound(970, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(1010, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};

//普攻2
skill(320602)
{
  section(2067)
  {
    animation("Attack_02");
    setanimspeed(167, "Attack_02", 0.4, true);
    setanimspeed(667, "Attack_02", 1, true);
    areadamage(880, 0, 1, 2, 3.5, true) {
      stateimpact("kDefault", 32060201);
    };
    shakecamera2(890, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,100));
    playsound(850, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(890, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//踩地板
skill(320603)
{
  section(2167)
  {
    animation("Skill_03");
    areadamage(1150, 0, 1, 0, 4.8, true) {
      stateimpact("kDefault", 32060301);
    };
    sceneeffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_YuJing_01", 1400, vector3(0,0.3,0), 100, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_Zhendangbo_01", 3000, vector3(0,0.3,0), 1150, eular(0,0,0), vector3(1,1,1));
    shakecamera2(1150, 150, false, true, vector3(0,1,0), vector3(0,50,0),vector3(0,1,0),vector3(0,100,0));
    playsound(1120, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
  };
};


//砸地
skill(320604)
{
  section(3800)
  {
    movecontrol(true);
    animation("Skill_02");
    lockframe(1060, "Skill_02", false, 0, 1000, 1, 50, false);
    areadamage(1060, 0, 1, 3, 4.5, true) {
      stateimpact("kDefault", 32060401);
    };
    areadamage(2060, 0, 1, 3, 7, true) {
      stateimpact("kDefault", 32060402);
    };
    sceneeffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_YuJing_02", 2500, vector3(0,0.3,3), 60, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_Zhendangbo_01", 3000, vector3(0,0.3,3), 1060, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_Zhendangbo_02", 3000, vector3(0,0.3,3), 2060, eular(0,0,0), vector3(1,1,1));
    shakecamera2(1060, 100, false, true, vector3(0,1,0), vector3(0,50,0),vector3(0,0.5,0),vector3(0,100,0));
    shakecamera2(2060, 200, false, true, vector3(0,2.5,0), vector3(0,50,0),vector3(0,1.5,0),vector3(0,100,0));
    playsound(1030, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
    playsound(2030, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadida_01", false);
  };
};


//普攻加快版
skill(320605)
{
  section(2700)
  {
    animation("Attack_01");
    setanimspeed(300, "Attack_01", 1.5, true);
    setanimspeed(800, "Attack_01", 1, true);
    areadamage(1000, 0, 1, 2, 3.5, true) {
      stateimpact("kDefault", 32060501);
    };
    shakecamera2(1530, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,100));
    playsound(970, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(1010, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};

//普攻2加快版
skill(320606)
{
  section(2067)
  {
    animation("Attack_02");
    setanimspeed(167, "Attack_02", 1.5, true);
    setanimspeed(667, "Attack_02", 1, true);
    areadamage(880, 0, 1, 2, 3.5, true) {
      stateimpact("kDefault", 32060601);
    };
    shakecamera2(890, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,100));
    playsound(850, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(890, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//砸地
skill(320607)
{
  section(3800)
  {
    movecontrol(true);
    animation("Skill_02");
    lockframe(1060, "Skill_02", false, 0, 1000, 1, 50, false);
    summonnpc(1000, 101, "Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_YuJing_03", 320608, vector3(0, 0, 0));
    shakecamera2(1060, 100, false, true, vector3(0,1,0), vector3(0,50,0),vector3(0,0.5,0),vector3(0,100,0));
    playsound(1030, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
  };
};

//砸地召唤物
skill(320608)
{
  section(3600)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(15," ",vector3(0,0.5,0),eular(0,0,0),"RelativeTarget",false,true);
    setenable(20, "Visible", true);
    playsound(2030, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadida_01", false);
    sceneeffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_Zhendangbo_01", 3000, vector3(0,0.3,0), 2030, eular(0,0,0), vector3(1,1,1));
    areadamage(2060, 0, 1.3, 0, 4.5, true) {
      stateimpact("kDefault", 32060701);
    };
    destroyself(3500);
    setenable(2060, "Visible", false);
    shakecamera2(2060, 200, false, true, vector3(0,2.5,0), vector3(0,50,0),vector3(0,1.5,0),vector3(0,100,0));
  };
};


//投掷巨石
skill(320609)
{
  section(2700)
  {
    animation("Attack_01");
    summonnpc(990, 101, "Monster/Campaign_Wild/07_OEOrc/5_Mon_OneyeOrc_02_w_02", 320610, vector3(0, 0, 0));
    setanimspeed(1300, "Attack_01", 2, true);
  };
};

//巨石飞行
skill(320610)
{
  section(2500)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    setenable(5, "Visible", true);
    rotate(10, 1500, vector3(400, 100, 0));
    settransform(0," ",vector3(-0.4, 1.5, 4.5),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5,vector3(0,-1.7,0),16,180,0.5,0.5,0,0);
    startcurvemove(10, true, 0.3,0,1,20,0,-12,0);
    colliderdamage(5, 500, false, false, 0, 0)
    {
      stateimpact("kDefault", 32060901);
      boneboxcollider(vector3(1.8, 1.8, 1.8), "Bone001", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    setenable(310, "CurveMove", false);
    setenable(310, "Rotate", false);
    setenable(310, "Damage", false);
    setenable(310, "Visible", false);
    settransform(311," ",vector3(0, 0, 0),eular(0,0,0),"RelativeSelf",false, true);
    areadamage(310, 0, 1, 0, 3, false) {
      stateimpact("kDefault", 32060902);
    };
    destroyself(3000);
    shakecamera2(310, 150, false, true, vector3(0, 0.8, 0), vector3(0, 50, 0), vector3(0, 1, 0), vector3(0, 80, 0));
    playsound(310, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_zadijifei_02", false);
    sceneeffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Rock_Zadi_01", 1500, vector3(0,0,0), 315);
  };
  onmessage("oncollide")
  {
    setenable(5, "Visible", false);
    destroyself(3000);
  };
};