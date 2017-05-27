//飞轮手横抡
skill(381501)
{
  section(2267)
  {
    movecontrol(true);
    animation("Attack_01");
    startcurvemove(627, true, 0.32, 0, 0, 10, 0, 0, 0);
    playsound(745, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Flywheel_01", 300, "ef_weapon01", 700);
    colliderdamage(700, 220, false, false, 0, 0)
    {
      stateimpact("kDefault", 38150101);
      boneboxcollider(vector3(3.5, 3.5, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    playsound(755, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(755, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,70));
  };
};


//投掷飞轮
skill(381502)
{
  section(1900)
  {
    animation("Skill_01");
    setchildvisible(870,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381503, vector3(0, 0, 0));
    setchildvisible(1850,"5_Mon_SWJFlywheel_01_w_01",true);
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
skill(381503)
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
      stateimpact("kDefault", 38150201);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150202);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1410);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
    shakecamera2(0, 100, true, true, vector3(0,0,0.2), vector3(0,0,50),vector3(0,0,0.5),vector3(0,0,70));
  };
};


//投掷飞轮
skill(381504)
{
  section(1900)
  {
    animation("Skill_01");
    setchildvisible(870,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381505, vector3(0, 0, 0));
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381506, vector3(0, 0, 0));
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381507, vector3(0, 0, 0));
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
skill(381505)
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
      stateimpact("kDefault", 38150401);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150402);
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
skill(381506)
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
      stateimpact("kDefault", 38150401);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150402);
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
skill(381507)
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
      stateimpact("kDefault", 38150401);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150402);
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
skill(381508)
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
    summonnpc(260, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381509, vector3(0, 0, 0));
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
skill(381509)
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
      stateimpact("kDefault", 38150201);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150202);
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



//投掷飞轮
skill(381510)
{
  section(1900)
  {
    animation("Skill_01");
    setchildvisible(870,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381511, vector3(0, 0, 0));
    setchildvisible(1850,"5_Mon_SWJFlywheel_01_w_01",true);
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
skill(381511)
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
      stateimpact("kDefault", 38150201);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150203);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1410);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
    shakecamera2(0, 100, true, true, vector3(0,0,0.2), vector3(0,0,50),vector3(0,0,0.5),vector3(0,0,70));
  };
};


//投掷飞轮
skill(381512)
{
  section(1900)
  {
    animation("Skill_01");
    setchildvisible(870,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381513, vector3(0, 0, 0));
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381514, vector3(0, 0, 0));
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381515, vector3(0, 0, 0));
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
skill(381513)
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
      stateimpact("kDefault", 38150401);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150403);
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
skill(381514)
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
      stateimpact("kDefault", 38150401);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150403);
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
skill(381515)
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
      stateimpact("kDefault", 38150401);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150403);
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
skill(381516)
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
    summonnpc(260, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 381516, vector3(0, 0, 0));
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
skill(381517)
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
      stateimpact("kDefault", 38150201);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 38150203);
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