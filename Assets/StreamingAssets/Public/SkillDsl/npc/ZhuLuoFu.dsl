//抡击
skill(380401)
{
  section(2233)
  {
    movecontrol(true);
    animation("Attack_01");
    findmovetarget(1100, vector3(0,0,0),6,45,0.5,0.5,0,-3);
    startcurvemove(1199, false, 0.11, 0, 0, 10, 0, 0, 0);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_02", 1000, "Bone_Root", 1210);
    playsound(1230, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1260, 0, 1, 2, 3, false) {
      stateimpact("kDefault", 38040101);
    };
    playsound(1270, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1270, 200, false, true, vector3(0,0.1,0.3), vector3(0,50,50), vector3(0,1,3), vector3(0,60,70));
  };
};


//横抡
skill(380402)
{
  section(2567)
  {
    movecontrol(true);
    animation("Skill_01");
    findmovetarget(1100, vector3(0,0,0),6,45,0.5,0.5,0,-3);
    startcurvemove(1119, false, 0.15, 0, 0, 10, 0, 0, 0);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_03", 1000, "Bone_Root", 1170);
    playsound(1200, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1230, 0, 1, 2, 3, false) {
      stateimpact("kDefault", 38040201);
    };
    playsound(1240, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1240, 200, false, true, vector3(0,0.1,0.3), vector3(0,50,50), vector3(0,1,3), vector3(0,60,70));
  };
};


//跳砸
skill(380403)
{
  section(2267)
  {
    movecontrol(true);
    animation("Skill_02");
    setanimspeed(300, "Skill_02", 0.15, true);
    setanimspeed(400, "Skill_02", 1, true);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 2000, vector3(0, 0, 0), 100, eular(0, 0, 0), vector3(1, 1, 1), true);//场景粒子在特殊位置仍然有不同步方向问题
    startcurvemove(510, true, 0.7, 0, 0, 7, 0, 0, 0);
    playsound(1336, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_04", 1000, "Bone_Root", 1275, true);
    sceneeffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_HuoLang_01", 1500, vector3(0,0,4.2),1345);
    playsound(1376, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_zhuluofu_tiaoza", false);//砸地音效
    areadamage(1386, 0, 1.2, 4.2, 5.5, true) {
      stateimpact("kDefault", 38040302);
    };
    shakecamera2(1380, 300, false, true, vector3(0.3,0.9,0.3), vector3(100,100,100),vector3(0.6,1.5,0.6),vector3(80,60,80));
  };
};


//火焰旋风
skill(380404)
{
  section(1165)
  {
    movecontrol(true);
    animation("Skill_03A");
    setanimspeed(1, "Skill_03A", 0.2, false);
    addimpacttoself(0, 30010100);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1500, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);//场景粒子在特殊位置仍然有不同步方向问题
    startcurvemove(100, true, 0.13, 0, 0, 7, 0, 0, 0);
  };
  section(2000)
  {
    animation("Skill_03B")
    {
      wrapmode(2);
    };
    startcurvemove(0, true, 2.668, 0, 0, 5, 0, 0, 0);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_XuanFeng_01", 2000, "Bone_Root", 0);
    playsound(15, "huiwu", "Sound/Npc/Mon_Loop", 2000, "Sound/Npc/boss_zhuluofu_huoyanxuanfeng", false);

    // areadamage(100, 0, 1, 0.5, 2.5, true) {
    //   stateimpact("kDefault", 30010101);
    // };
    // playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    // shakecamera2(150, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));

    // cleardamagestate(650);
    // areadamage(700, 0, 1, 0.5, 2.5, true) {
    //   stateimpact("kDefault", 30010101);
    // };
    // playsound(710, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    // shakecamera2(750, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));

    // cleardamagestate(1250);
    // areadamage(1300, 0, 1, 0.5, 2.5, true) {
    //   stateimpact("kDefault", 30010101);
    // };
    // playsound(1310, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    // shakecamera2(1350, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
    colliderdamage(100, 1850 , true, true, 400, 5)
    {
      stateimpact("kDefault", 38040401);
      stateimpact("kLauncher", 38040402);
      sceneboxcollider(vector3(5, 6, 5), vector3(0, 2, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(110);
    cleardamagestate(510);
    cleardamagestate(910);
    cleardamagestate(1310);
    cleardamagestate(1710);
    playsound(111, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(511, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(911, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1311, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1711, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    stopsound(1995, "huiwu");
  };
  section(600)
  {
    animation("Skill_03C");
    startcurvemove(0, true, 0.3, 0, 0, 4, 0, 0, -12);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "huiwu");
  };
};


//挑空
skill(380405)
{
  section(1767)
  {
    movecontrol(true);
    animation("Skill_04");
    startcurvemove(1, false, 0.19, 0, 0, -1, 0, 0, 0);
    startcurvemove(660, true, 0.4, 0, 0, 8, 0, 0, -20);
    playsound(670, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_fukong", false);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_05", 800, "Bone_Root", 680, true);
    colliderdamage(700, 100, false, false, 0, 0)
    {
      stateimpact("kDefault", 38040501);
      stateimpact("kLauncher", 38040502);
      sceneboxcollider(vector3(4, 4, 4.5), vector3(0, 2, 2.2), eular(0, 0, 0), true, false);
    };
    shakecamera2(725, 200, false, false, vector3(0,0.6,0), vector3(0,100,0),vector3(0,3,0),vector3(0,80,0));
    playsound(725, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//连招1
skill(380406)
{
  section(800)
  {
    movecontrol(true);
    animation("Skill_04");
    setanimspeed(400,"Skill_04", 0.1, true);
    setanimspeed(500,"Skill_04", 1, true);
    startcurvemove(660, true, 0.4, 0, 0, 8, 0, 0, -20);
    playsound(670, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_fukong", false);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_05", 800, "Bone_Root", 680, true);
    colliderdamage(700, 100, true, false, 0, 0)
    {
      stateimpact("kDefault", 38040501);
      stateimpact("kLauncher", 38040502);
      sceneboxcollider(vector3(4, 4, 4.5), vector3(0, 2, 2.2), eular(0, 0, 0), true, false);
    };
    shakecamera2(725, 200, false, false, vector3(0,0.6,0), vector3(0,100,0),vector3(0,3,0),vector3(0,80,0));
    playsound(725, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
  section(2000)
  {
    animation("Skill_03B")
    {
      wrapmode(2);
    };
    startcurvemove(0, true, 2.668, 0, 0, 5, 0, 0, 0);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_XuanFeng_01", 2000, "Bone_Root", 0);
    playsound(15, "huiwu", "Sound/Npc/Mon_Loop", 2000, "Sound/Npc/boss_zhuluofu_huoyanxuanfeng", false);
    colliderdamage(100, 1850, true, true, 400, 5)
    {
      stateimpact("kDefault", 38040401);
      stateimpact("kLauncher", 38040402);
      sceneboxcollider(vector3(5, 6, 5), vector3(0, 2, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(110);
    cleardamagestate(510);
    cleardamagestate(910);
    cleardamagestate(1310);
    cleardamagestate(1710);
    playsound(111, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(511, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(911, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1311, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1711, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    stopsound(1995, "huiwu");
  };
  section(2267)
  {
    animation("Skill_02");
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1000, vector3(0, 0, 0), 520, eular(0, 0, 0), vector3(1, 1, 1), true);//场景粒子在特殊位置仍然有不同步方向问题
    findmovetarget(407, vector3(0,0,0),10,360,0.5,0.5,0,-3);
    startcurvemove(510, true, 0.7, 0, 0, 8, 0, 0, 0);
    playsound(1336, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_04", 1000, "Bone_Root", 1275, true);
    sceneeffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_HuoLang_01", 1500, vector3(0,0,4.2),1345);
    playsound(1376, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_zhuluofu_tiaoza", false);//砸地音效
    areadamage(1386, 0, 1.2, 4.2, 5.5, true) {
      stateimpact("kDefault", 38040302);
    };
    shakecamera2(1380, 300, false, true, vector3(0.3,0.9,0.3), vector3(100,100,100),vector3(0.6,1.5,0.6),vector3(80,60,80));
  };
};

//连招2
skill(380407)
{
  section(800)
  {
    movecontrol(true);
    animation("Skill_04");
    sceneeffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_YuJing_01", 1500, vector3(0, 1.2, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    shakecamera2(10, 200, false, false, vector3(0.4,0.4,0.4), vector3(50,50,50),vector3(1,1,1),vector3(90,90,90));
    setanimspeed(400,"Skill_04", 0.1, true);
    setanimspeed(500,"Skill_04", 1, true);
    startcurvemove(660, true, 0.4, 0, 0, 8, 0, 0, -20);
    playsound(670, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_fukong", false);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_05", 800, "Bone_Root", 680, true);
    colliderdamage(700, 100, true, false, 0, 0)
    {
      stateimpact("kDefault", 38040501);
      stateimpact("kLauncher", 38040502);
      sceneboxcollider(vector3(4, 4, 4.5), vector3(0, 2, 2.2), eular(0, 0, 0), true, false);
    };
    shakecamera2(725, 200, false, false, vector3(0,0.6,0), vector3(0,100,0),vector3(0,3,0),vector3(0,80,0));
    playsound(725, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
  section(1500)
  {
    animation("Skill_01");
    cleardamagestate(10);
    setanimspeed(100, "Skill_01", 3, true);
    setanimspeed(1000, "Skill_01", 1, true);
    findmovetarget(1100, vector3(0,0,0),6,45,0.5,0.5,0,-3);
    startcurvemove(1119, false, 0.15, 0, 0, 10, 0, 0, 0);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_03", 1000, "Bone_Root", 1170);
    playsound(1200, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1230, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 38040201);
      stateimpact("kLauncher", 38040202);
    };
    playsound(1240, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1240, 200, false, true, vector3(0,0.1,0.3), vector3(0,50,50), vector3(0,1,3), vector3(0,60,70));
  };
  section(1500)
  {
    animation("Attack_01");
    cleardamagestate(10);
    setanimspeed(100, "Attack_01", 3, true);
    setanimspeed(1000, "Attack_01", 1, true);
    findmovetarget(1100, vector3(0,0,0),6,45,0.5,0.5,0,-3);
    startcurvemove(1199, false, 0.11, 0, 0, 10, 0, 0, 0);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_02", 1000, "Bone_Root", 1210);
    playsound(1230, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1260, 0, 1, 2.1, 3, true) {
      stateimpact("kDefault", 38040101);
      stateimpact("kLauncher", 38040102);
    };
    playsound(1270, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1270, 200, false, true, vector3(0,0.1,0.3), vector3(0,50,50), vector3(0,1,3), vector3(0,60,70));
  };
  section(1500)
  {
    animation("Skill_01");
    cleardamagestate(10);
    setanimspeed(100, "Skill_01", 3, true);
    setanimspeed(1000, "Skill_01", 1, true);
    findmovetarget(1100, vector3(0,0,0),6,45,0.5,0.5,0,-3);
    startcurvemove(1119, false, 0.15, 0, 0, 10, 0, 0, 0);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_03", 1000, "Bone_Root", 1170);
    playsound(1200, "huiwu3", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1230, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 38040201);
      stateimpact("kLauncher", 38040202);
    };
    playsound(1240, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1240, 200, false, true, vector3(0,0.1,0.3), vector3(0,50,50), vector3(0,1,3), vector3(0,60,70));
  };
  section(1767)
  {
    animation("Skill_04");
    cleardamagestate(10);
    setanimspeed(200,"Skill_04", 2, true);
    setanimspeed(500,"Skill_04", 1, true);
    startcurvemove(660, true, 0.4, 0, 0, 8, 0, 0, -20);
    playsound(670, "huiwu4", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_fukong", false);
    charactereffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_DaoGuang_05", 800, "Bone_Root", 680, true);
    colliderdamage(700, 100, true, false, 0, 0)
    {
      stateimpact("kDefault", 38040501);
      stateimpact("kLauncher", 38040503);
      sceneboxcollider(vector3(4, 4, 4.5), vector3(0, 2, 2.2), eular(0, 0, 0), true, false);
    };
    shakecamera2(725, 200, false, false, vector3(0,0.6,0), vector3(0,100,0),vector3(0,3,0),vector3(0,80,0));
    playsound(725, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};

//自身霸体状态
skill(380408)
{
  section(10)
  {
    addimpacttoself(0, 30010003, 5000);
  };
};