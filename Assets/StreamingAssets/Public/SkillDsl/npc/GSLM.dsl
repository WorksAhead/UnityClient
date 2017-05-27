//普攻
skill(381101)
{
  section(1867)
  {
    movecontrol(true);
    animation("Attack_01");
    findmovetarget(400, vector3(0,0,0),4,50,0.5,0.5,0,-2);
    startcurvemove(470, true, 0.15, 0, 0, 7, 0, 0, 0);
    findmovetarget(1002, vector3(0,0,0),5,50,0.5,0.5,0,2);
    startcurvemove(1007, true, 0.15, 0, 0, 7, 0, 0, 0);
    areadamage(530, 0, 1, 1.2, 3.5, true) {
      stateimpact("kDefault", 38110101);
      stateimpact("kLauncher", 38110102);
    };
    cleardamagestate(800);
    areadamage(1030, 0, 1, 1.2, 3.5, true) {
      stateimpact("kDefault", 38110103);
      stateimpact("kLauncher", 38110104);
    };
    charactereffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_DaoGuang_01", 1000, "Bone_Root", 490);
    charactereffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_DaoGuang_02", 1000, "Bone_Root", 970);
    //sceneeffect("Monster_FX/Boss/6_Mon_Xilie_DaoGuang_01_01", 3000, vector3(0,-0.2,1), 367, eular(0,0,0), vector3(1,1,1), true);
    //sceneeffect("Monster_FX/Boss/6_Mon_Xilie_DaoGuang_01_02", 3000, vector3(0,-0.2,1), 967, eular(0,0,0), vector3(1,1,1), true);
    shakecamera2(540, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,80));
    shakecamera2(1040, 180, true, true, vector3(0,0,0.4), vector3(0,0,50),vector3(0,0,1.2),vector3(0,0,80));
    playsound(515, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/BOSS_GUSILANMA_PUGONG_1", false);
    playsound(1025, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/BOSS_GUSILANMA_PUGONG_2", false);
    playsound(540, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1040, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//火焰刀
skill(381102)
{
  section(1600)
  {
    animation("Skill_01");
    setanimspeed(500, "Skill_03", 0.4, true);
    setanimspeed(1000, "Skill_03", 1, true);
    charactereffect("Monster_FX/Boss/YanShou/6_Mon_YanShou_PenQi_01",1550,"ef_mouth",500,false);
    playsound(520, "huiwu", "Sound/Npc/Mon", 1500, "Sound/Npc/BOSS_GUSILANMA_HUOYANREN_BAOPO_1", false);
    areadamage(530, 0, 1, 1, 2.5, false) {
      stateimpact("kDefault", 38110201);
    };
    shakecamera2(530, 200, false, true, vector3(0.4,2,0.4), vector3(75,75,75),vector3(0.4,2.5,0.4),vector3(70,90,70));
    summonnpc(520, 101, "Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_ranshao_01", 381103, vector3(0, 0, 0));
  };
};


//火焰蔓延
skill(381103)
{
  section(5550)
  {
    movecontrol(true);
    settransform(0," ",vector3(0, 0.3, 1.5),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(30, true, 1, 0, 0, 6, 0, 0, 0);
    playsound(20, "huiwu", "Sound/Npc/Mon", 5000, "Sound/Npc/BOSS_GUSILANMA_HUOYANREN", false);
    colliderdamage(400, 5310, true, true, 800, 10)
    {
      stateimpact("kDefault", 38110201);
      stateimpact("kLauncher", 38110202);
      sceneboxcollider(vector3(4.5, 4, 4.5), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_01", 1000, vector3(0,0.2,0), 390, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_01", 1000, vector3(0,0.2,0), 1190, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_01", 1000, vector3(0,0.2,0), 1990, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_01", 1000, vector3(0,0.2,0), 2790, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_01", 1000, vector3(0,0.2,0), 3590, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_01", 1000, vector3(0,0.2,0), 4390, eular(0,0,0), vector3(1,1,1));
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_01", 1000, vector3(0,0.2,0), 5190, eular(0,0,0), vector3(1,1,1));
    destroyself(5400);
  };
  onmessage("oncollide")
  {
    setenable(10, "CurveMove", false);
  };
};


//释放火雨
skill(381104)
{
  section(2733)
  {
    animation("Skill_03");
    addimpacttoself(0,30030001,5000);
    setanimspeed(1500, "Skill_03", 0.2, true);
    setanimspeed(2000, "Skill_03", 1, true);
    setchildvisible(615,"5_IP_Gusilanma_01_w_01",false);
    setchildvisible(615,"5_IP_Gusilanma_01_w_02",false);
    setchildvisible(615,"6_Mon_GSLM_Weapon_01",false);
    setchildvisible(615,"6_Mon_GSLM_Weapon_02",false);
    charactereffect("Monster_FX/Campaign_Wild/Public/6_Mon_Baodian_01", 800, "ef_righthand", 610, false);
    charactereffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Xuli_01", 3600, "ef_lefthand", 630);
    summonnpc(1500, 101, "Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 381105, vector3(0, 0, 0));
    summonnpc(1700, 101, "Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 381105, vector3(0, 0, 0));
    summonnpc(1900, 101, "Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 381105, vector3(0, 0, 0));
    setchildvisible(2615,"5_IP_Gusilanma_01_w_01",true);
    setchildvisible(2615,"6_Mon_GSLM_Weapon_01",true);
    setchildvisible(2615,"6_Mon_GSLM_Weapon_02",true);
    setchildvisible(2615,"5_IP_Gusilanma_01_w_02",true);
    shakecamera2(1300, 3500, false, true, vector3(0.08,0.2,0.08), vector3(60,40,60),vector3(0.08,0.2,0.08),vector3(95,100,95));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_IP_Gusilanma_01_w_01",true);
    setchildvisible(0,"5_IP_Gusilanma_01_w_02",true);
    setchildvisible(0,"6_Mon_GSLM_Weapon_01",true);
    setchildvisible(0,"6_Mon_GSLM_Weapon_02",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_IP_Gusilanma_01_w_01",true);
    setchildvisible(0,"5_IP_Gusilanma_01_w_02",true);
    setchildvisible(0,"6_Mon_GSLM_Weapon_01",true);
    setchildvisible(0,"6_Mon_GSLM_Weapon_02",true);
  };
};


//火雨召唤物
skill(381105)
{
  section(6000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    setenable(20, "Visible", true);
    //循环段
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0.4,2),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/YanShou/6_Mon_YanShou_HuoYu_01", 5000, vector3(0,0,0),1000);
    colliderdamage(1000, 5000, true, true, 1000, 5)
    {
      stateimpact("kDefault", 38110401);
      sceneboxcollider(vector3(7, 5, 7), vector3(0, 2.5, 0), eular(0, 0, 0), true, false);
    };
    playsound(1000, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_ranshao_01", false);
    destroyself(5000);
  };
};


//连打
skill(381106)
{
  section(3000)
  {
    movecontrol(true);
    animation("Skill_04");
    findmovetarget(5,vector3(0,0,0),5,50,0.5,0.5,0,-1);
    startcurvemove(520, true, 0.1, 0, 0, 6, 0, 0, 0);
    findmovetarget(1500,vector3(0,0,0),5,50,0.5,0.5,0,2);
    startcurvemove(1560, true, 0.12, 0, 0, 10, 0, 0, 0);
    colliderdamage(510,50,true,false,0,1)
    {
      stateimpact("kDefault", 38110601);
      stateimpact("kLauncher", 38110602);
      sceneboxcollider(vector3(5, 2, 6), vector3(0, 1, 1), eular(0, 0, 0), true, false);
    };
    areadamage(894, 0, 1, 1.5, 3.5, true) {
      stateimpact("kDefault", 38110603);
      stateimpact("kLauncher", 38110604);
    };
    areadamage(1185, 0, 1, 1.5, 3.5, true) {
      stateimpact("kDefault", 38110603);
      stateimpact("kLauncher", 38110604);
    };
    colliderdamage(1587,50,true,false,0,1)
    {
      stateimpact("kDefault", 38110605);
      stateimpact("kLauncher", 38110606);
      sceneboxcollider(vector3(5, 3, 6), vector3(0, 1, 1), eular(0, 0, 0), true, false);
    };
//刀光
    charactereffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_DaoGuang_01", 1000, "Bone_Root", 470);
    charactereffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_DaoGuang_02", 1000, "Bone_Root", 834);
    charactereffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_DaoGuang_03", 1000, "Bone_Root", 1125);
    charactereffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_DaoGuang_03", 1000, "Bone_Root", 1527);
//
    playsound(480, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/BOSS_GUSILANMA_LIANJI_1", false);
    playsound(854, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/BOSS_GUSILANMA_LIANJI_2", false);
    playsound(1145, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/BOSS_GUSILANMA_LIANJI_3", false);
    playsound(1550, "huiwu3", "Sound/Npc/Mon", 1000, "Sound/Npc/BOSS_GUSILANMA_LIANJI_4", false);
//
    playsound(530, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(900, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1190, "hit2", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1600, "hit3", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//
    shakecamera2(530, 180, true, true, vector3(0,0.8,0.8), vector3(0,50,50),vector3(0,0,1.2),vector3(0,0,80));
    shakecamera2(900, 180, true, true, vector3(0,0,0.4), vector3(0,0,50),vector3(0,0,1.2),vector3(0,0,80));
    shakecamera2(1190, 180, true, true, vector3(0,0,0.4), vector3(0,0,50),vector3(0,0,1.2),vector3(0,0,80));
    shakecamera2(1600, 180, true, true, vector3(0,0,0.4), vector3(0,40,40),vector3(0,0,1.2),vector3(0,0,80));
//
    cleardamagestate(800);
    cleardamagestate(1000);
    cleardamagestate(1500);
  };
};

//变身
skill(381107)
{
  section(1800)
  {
    animation("Skill_02");
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeSelf",false,true);
    addimpacttoself(0, 30010003);
    playsound(400, "huiwu", "Sound/Npc/Mon", 2000, "Sound/Npc/BOSS_GUSILANMA_BAOQI", false);
    areadamage(410, 0, 1, 0, 6, false) {
      stateimpact("kDefault", 38110701);
    };
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_02", 3000, vector3(0,0.3,0), 380, eular(0,0,0), vector3(1.5,1.5,1.5));
    shakecamera2(380,550,false,false,vector3(0.5,0.5,0.5),vector3(50,50,50),vector3(5,5,5),vector3(85,85,85));
  };
};


//变身
skill(381108)
{
  section(1800)
  {
    animation("Skill_02");
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeSelf",false,true);
    addimpacttoself(0, 30010003);
    playsound(400, "huiwu", "Sound/Npc/Mon", 2000, "Sound/Npc/BOSS_GUSILANMA_BAOQI", false);
    areadamage(410, 0, 1, 0, 6, false) {
      stateimpact("kDefault", 38110701);
    };
    sceneeffect("Monster_FX/Boss/GSLM/6_Mon_GSLM_Baozha_02", 3000, vector3(0,0.3,0), 380, eular(0,0,0), vector3(1.5,1.5,1.5));
    shakecamera2(380,550,false,false,vector3(0.5,0.5,0.5),vector3(50,50,50),vector3(5,5,5),vector3(85,85,85));
    summonnpc(400, 5021, "", 381109, vector3(0, 0, 0), eular(0, 0, 0), 20005, true, ",,300006");
    setenable(401,Visible, false);
    destroyself(1500);
  };
};

//召唤物出生技能
skill(381109)
{
  section(3000)
  {
    animation("Ridicule");
    addimpacttoself(0, 30010003, 3000);
  };
};