//普攻
skill(380701)
{
  section(1400)
  {
    animation("Attack_01");
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_DaoGuang_01", 2000, "Bone_Root", 720);
    playsound(715, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_safeile_pugong", false);
    areadamage(760, 0, 1, 0.5, 3, false) {
      stateimpact("kDefault", 38070101);
      stateimpact("kLauncher", 38070102);
    };
    playsound(770, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(770, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};

//暗影裂隙
skill(380702)
{
  section(2100)
  {
    animation("Attack_02");
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_DaoGuang_02", 2000, "Bone_Root", 1290);
    sceneeffect("Monster_FX/Boss/SFL/6_Mon_SFL_Diliezhan_01", 3000, vector3(0,0.1,2.5), 1320, eular(0,0,0), vector3(1,1,1), true);
    playsound(1280, "huiwu", "Sound/Npc/Mon", 2000, "Sound/Npc/boss_safeile_anyingliexi", false);
    // playsound(1310, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadida_01", false);
    // playsound(1335, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadida_01", false);
    // playsound(1360, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadida_01", false);
    colliderdamage(1330, 100, false, false, 0, 0)
    {
      stateimpact("kDefault", 38070201);
      stateimpact("kLauncher", 38070202);
      sceneboxcollider(vector3(2, 4, 12), vector3(0, 2, 6), eular(0, 0, 0), false, false);
    };
    shakecamera2(1340, 250, false, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};

//召唤黑洞
skill(380703)
{
  section(1567)
  {
    animation("Skill_02");
    setanimspeed(832, "Skill_02", 0.2, true);
    setanimspeed(1142, "Skill_02", 1, true);
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_Xuli_01", 2500, "ef_lefthand", 600);
    playsound(600, "huiwu", "Sound/Npc/Mon", 2000, "Sound/Npc/boss_safeile_zhaohuanheidong(zhaohuan)", false);
    summonnpc(1142, 103, "", 380704, vector3(0, 0, 0), eular(0, 0, 0), 20005, true, ",,300004");
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "huiwu");
  };
};


//黑洞出生
skill(380704)
{
  section(50)
  {
    movecontrol(true);
    findmovetarget(1, vector3(0, 0, 0), 25, 360, 0.5, 0.5, 0, 1);
    setenable(0, "Visible", false);
    setenable(20, "Visible", true);
    settransform(10, " ", vector3( 0, 0.1, -6), eular(0, 0, 0), "RelativeTarget", false, true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 2000, "Sound/Npc/boss_safeile_zhaohuanheidong(heidongchusheng)", false);
  };
};

//破军
skill(380705)
{
  section(970)
  {
    movecontrol(true);
    animation("Skill_03");
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 3500, vector3(0,0.2,0), 100, eular(0, 0, 0), vector3(1,1,1),true);
    setanimspeed(100, "Skill_03", 0.3, true);
    setanimspeed(400, "Skill_03", 1, true);
    findmovetarget(20,vector3(0,0,0),10,40,0.5,0.5,0,0.3,false);
    facetotarget(50, 50, 0);
    startcurvemove(500, true, 0.45, 0, 0, 20, 0, 0, 0);
    playsound(490, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_chongji", false);
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_Tuowei_01",1000,"Bone_Root",500,false);
    colliderdamage(500, 320, true, true, 150, 3)
    {
      stateimpact("kDefault", 38070501);
      stateimpact("kLauncher", 38070502);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(520, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
  section(520)
  {
    movecontrol(true);
    animation("Skill_03A");
    settransform(0,"", vector3(0,0,0),eular(0,180,0), "RelativeSelf", false, true);
    findmovetarget(10,vector3(0,0,0),10,40,0.5,0.5,0,0.3,false);
    facetotarget(15, 50, 0);
    startcurvemove(100, true, 0.4, 0, 0, 20, 0, 0, 0);
    playsound(90, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_chongji", false);
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_Tuowei_01",1000,"Bone_Root",100,false);
    colliderdamage(100, 320, true, true, 150, 3)
    {
      stateimpact("kDefault", 38070501);
      stateimpact("kLauncher", 38070502);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(120, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
  section(1467)
  {
    movecontrol(true);
    animation("Skill_03A");
    settransform(0,"", vector3(0,0,0),eular(0,180,0), "RelativeSelf", false, true);
    findmovetarget(10,vector3(0,0,0),10,40,0.5,0.5,0,0.3,false);
    facetotarget(15, 50, 0);
    startcurvemove(100, true, 0.50, 0, 0, 20, 0, 0, 0);
    playsound(90, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_chongji", false);
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_Tuowei_01",1000,"Bone_Root",100,false);
    colliderdamage(100, 420, true, true, 150, 3)
    {
      stateimpact("kDefault", 38070501);
      stateimpact("kLauncher", 38070502);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(120, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//牵引
skill(380706)
{
  section(1567)
  {
    animation("Skill_02");
    setanimspeed(780, "Skill_02", 0.2, true);
    setanimspeed(1180, "Skill_02", 1, true);
    playsound(700, "huiwu", "Sound/Npc/Mon", 2000, "Sound/Npc/boss_safeile_qianyin", false);
    charactereffect("Monster_FX/Boss/SFL/6_Mon_SFL_Qianyin_01", 2300, "ef_lefthand", 600);
    colliderdamage(780, 1100, true, true, 1000, 2)
    {
      stateimpact("kDefault", 38070601);
      sceneboxcollider(vector3(6, 3, 6), vector3(0, 1.5, 1), eular(0, 0, 0), true, false);
    };
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "huiwu");
  };
};

//召唤黑暗球
skill(380707)
{
  section(2867)
  {
    addimpacttoself(0, 30010003, 4000);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 3500, vector3(0,0.2,2), 500, eular(0, -45, 0), vector3(1,1,1),true);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 3500, vector3(0,0.2,2), 500, eular(0, 0, 0), vector3(1,1,1),true);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 3500, vector3(0,0.2,2), 500, eular(0, 45, 0), vector3(1,1,1),true);
    animation("Skill_01");
    playsound(0, "huiwu", "Sound/Npc/Mon", 6000, "Sound/Npc/boss_safeile_zhaohuanheiqiu", false);
    setanimspeed(650, "Skill_01", 0.4, true);
    setanimspeed(1450, "Skill_01", 1, true);
    summonnpc(650, 101, "Monster_FX/Boss/SFL/6_Mon_SFL_Heidong_02", 380709, vector3(0, 0, 0));
    summonnpc(1500, 101, "Monster_FX/Boss/SFL/6_Mon_SFL_Heidong_02", 380710, vector3(0, 0, 0));
    summonnpc(1500, 101, "Monster_FX/Boss/SFL/6_Mon_SFL_Heidong_02", 380711, vector3(0, 0, 0));
    summonnpc(1500, 101, "Monster_FX/Boss/SFL/6_Mon_SFL_Heidong_02", 380712, vector3(0, 0, 0));
    shakecamera2(650, 2000, false, true, vector3(0.05,0.05,0.05), vector3(50,50,50),vector3(0.1,0.1,0.1),vector3(100,100,100));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "huiwu");
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
  };
};

//黑洞脉冲
skill(380708)
{
  section(1000)
  {
    sceneeffect("Monster_FX/Boss/SFL/6_Mon_SFL_Maichong_01", 1500, vector3(0,2,0), 0);
    areadamage(10, 0, 1.5, 0, 12, false) {
      stateimpact("kDefault", 38070801);
    };
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_safeile_heidongqianyin", false);
    shakecamera2(15, 150, true, true, vector3(0.5,0,0.5), vector3(75,0,75),vector3(1,0,1),vector3(50,0,50));
  };
};

//黑球出生
skill(380709)
{
  section(2201)
  {
    movecontrol(true);
    setenable(5, "Visible", true);
    settransform(0," ",vector3(0,-3,2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 2, 0, 2, 0, 0, 0, 0);
    // colliderdamage(10, 2000, false, false, 400, 0)
    // {
    //   stateimpact("kDefault", 38070801);
    //   sceneboxcollider(vector3(3, 3, 3), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    // };
    destroyself(2200);
  };
};


//移动黑球
skill(380710)
{
  section(5500)
  {
    movecontrol(true);
    setenable(5, "Visible", true);
    settransform(0," ",vector3(0, 1, 2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(330, true, 5, -3, 0, 3, 0, 0, 0);
    colliderdamage(330, 5000, true, true, 500, 10)
    {
      stateimpact("kDefault", 38070701);
      sceneboxcollider(vector3(2, 3, 2), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(5500);
  };
};

//移动黑球
skill(380711)
{
  section(5500)
  {
    movecontrol(true);
    setenable(5, "Visible", true);
    settransform(0," ",vector3(0, 1, 2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(330, true, 5, 0, 0, 3, 0, 0, 0);
    colliderdamage(330, 5000, true, true, 500, 10)
    {
      stateimpact("kDefault", 38070701);
      sceneboxcollider(vector3(2, 3, 2), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(5500);
  };
};

//移动黑球
skill(380712)
{
  section(5500)
  {
    movecontrol(true);
    setenable(5, "Visible", true);
    settransform(0," ",vector3(0, 1, 2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(330, true, 5, 3, 0, 3, 0, 0, 0);
    colliderdamage(330, 5000, true, true, 500, 10)
    {
      stateimpact("kDefault", 38070701);
      sceneboxcollider(vector3(2, 3, 2), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(5500);
  };
};