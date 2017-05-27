//冲刺突进
skill(320211)
{
  section(2367)
  {
    movecontrol(true);
    animation("Skill_02");
    setanimspeed(500, "Skill_02", 0.3, true);
    setanimspeed(800, "Skill_02", 1, true);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1800, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);//场景粒子在特殊位置仍然有不同步方向问题
    sceneeffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Shan_01", 900, vector3(0, 1.2, 0), 50, eular(0, 0, 0), vector3(1, 1, 1), true);
    startcurvemove(875, true, 0.3, 0, 0, 35, 0, 0, 0);
    playsound(850, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_chongji", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Tuowei_01",1000,"Bone_Root",880,false);
    colliderdamage(880, 300, true, true, 200, 2)
    {
      stateimpact("kDefault", 32021101);
      stateimpact("kLauncher", 32021102);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(900, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//破防连击
skill(320212)
{
  section(2333)
  {
    movecontrol(true);
    animation("Skill_03");
    findmovetarget(100, vector3(0,0,0),5,50,0.5,0.5,0,-1.5);
    startcurvemove(300, true, 0.2, 0, 0, 5, 0, 0, 0);
    findmovetarget(980, vector3(0,0,0),5,50,0.5,0.5,0,-1.5);
    startcurvemove(1000, true, 0.2, 0, 0, 5, 0, 0, 0);
    areadamage(330, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 32021201);
    };
    cleardamagestate(800);
    areadamage(1070, 0, 1, 2, 3.5, true) {
      stateimpact("kDefault", 32021202);
    };
    sceneeffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_03", 3000, vector3(0,-0.2,1), 300, eular(0,0,0), vector3(1,1,1), true);
    sceneeffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_04", 3000, vector3(0,-0.2,1), 1040, eular(0,0,0), vector3(1,1,1), true);
    shakecamera2(340, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,80));
    shakecamera2(1080, 180, true, true, vector3(0,0,0.4), vector3(0,0,50),vector3(0,0,1.2),vector3(0,0,80));
    playsound(300, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(1040, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_02", false);
    playsound(340, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(1080, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//投掷飞轮
skill(320213)
{
  section(1900)
  {
    animation("Skill_01");
    setchildvisible(870,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 320214, vector3(0, 0, 0));
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 320215, vector3(0, 0, 0));
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 320216, vector3(0, 0, 0));
    setchildvisible(1850,"5_Mon_SWJFlywheel_01_w_01",true);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1800, vector3(0, 0.4, 0), 0, eular(0, -45, 0), vector3(1, 1, 1), true);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1800, vector3(0, 0.4, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1800, vector3(0, 0.4, 0), 0, eular(0, 45, 0), vector3(1, 1, 1), true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_SWJFlywheel_01_w_01",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_SWJFlywheel_01_w_01",true);
  };
};

//飞轮
skill(320214)
{
  section(1410)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(10, 1400, vector3(0, 2000, 0));
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Flywheel_01", 2000, "ef_weapon01", 0);
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0.2,1.1,0.2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 0.7,-16,0,16,0,0,0,0.7,16,0,-16,0,0,0);
    colliderdamage(10, 690, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020701);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020702);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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


//飞轮
skill(320215)
{
  section(1410)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(10, 1400, vector3(0, 2000, 0));
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Flywheel_01", 2000, "ef_weapon01", 0);
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0.2,1.1,0.2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 0.7,0,0,16,0,0,0,0.7,0,0,-16,0,0,0);
    colliderdamage(10, 690, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020701);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020702);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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


//飞轮
skill(320216)
{
  section(1410)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(10, 1400, vector3(0, 2000, 0));
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Flywheel_01", 2000, "ef_weapon01", 0);
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0.2,1.1,0.2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 0.7,16,0,16,0,0,0,0.7,-16,0,-16,0,0,0);
    colliderdamage(10, 690, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020701);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020702);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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


//解控制
skill(320217)
{
  section(1733)
  {
    movecontrol(true);
    animation("Skill_02");
    settransform(5," ",vector3(0,0,0),eular(0,0,0),"RelativeSelf",false,true);
    startcurvemove(50, true, 0.4,0,0,-10,0,0,-25);
    // charactereffect("Monster_FX/Boss/6_Mon_Xilie_ChongJi_01",1000,"Bone_Root",50,false) {
    //   transform(vector3(0,0.9,0),eular(0,0,180));
    // };
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Tuowei_01",1000,"Bone_Root",50,false) {
      transform(vector3(0,0.4,0),eular(0,180,0));
    };
    setchildvisible(265,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(260, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 320218, vector3(0, 0, 0));
    setchildvisible(1100,"5_Mon_SWJFlywheel_01_w_01",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_SWJFlywheel_01_w_01",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_SWJFlywheel_01_w_01",true);
  };
};



//飞轮
skill(320218)
{
  section(1410)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(10, 1400, vector3(0, 2000, 0));
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Flywheel_01", 2000, "ef_weapon01", 0);
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0.2,1.1,0.2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 0.7,0,0,14,0,0,0,0.7,0,0,-16,0,0,0);
    colliderdamage(10, 690, true, false, 0, 0)
    {
      stateimpact("kDefault", 32021701);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 32021702);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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