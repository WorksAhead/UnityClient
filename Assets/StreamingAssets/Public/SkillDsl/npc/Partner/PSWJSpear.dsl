//长矛兵横斩
skill(381601)
{
  section(1500)
  {
    movecontrol(true);
    animation("Attack_01");
    findmovetarget(550, vector3(0,0,0),3,45,0.5,0.5,0,-1.5);
    startcurvemove(562, true, 0.11, 0, 0, 5, 0, 0, 0);
    playsound(630, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_02_01", 2000, "Bone_Root", 630)
    {
      transform(vector3(0,0,0.5));
    };
    areadamage(660, 0, 1, 1, 2.5, true) {
      stateimpact("kDefault", 38160101);
    };
    playsound(665, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(665, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,70));
  };
};


//长矛兵连击
skill(381602)
{
  section(2033)
  {
    movecontrol(true);
    animation("Skill_01");
    findmovetarget(480, vector3(0, 0, 0), 3, 45, 0.5, 0.5, 0, -2);
    startcurvemove(487, true, 0.07, 0, 0, 8, 0, 0, 0);
    playsound(515, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_TuCi_01", 2000, "ef_weapon01", 515)
    {
      transform(vector3(0,0,0.5));
    };
    colliderdamage(550, 50, true, true, 0, 1)
    {
      stateimpact("kDefault", 38160201);
      boneboxcollider(vector3(1.5, 1.5, 5), "ef_weapon01", vector3(0, 0, 1), eular(0, 0, 0), true, false);
    };
    playsound(570, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(560, 200, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,2),vector3(0,0,70));
//第二下
    cleardamagestate(800);
    playsound(835, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_TuCi_01", 2000, "ef_weapon01", 835)
    {
      transform(vector3(0,0,0.5));
    };
    colliderdamage(870, 50, true, true, 0, 1)
    {
      stateimpact("kDefault", 38160202);
      boneboxcollider(vector3(1.5, 1.5, 5), "ef_weapon01", vector3(0, 0, 1), eular(0, 0, 0), true, false);
    };
    playsound(880, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(880, 200, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,2),vector3(0,0,70));
//第三下
    findmovetarget(1400, vector3(0,0,0),4,45,0.5,0.5,0,-1.5);
    startcurvemove(1406, true, 0.11, 0, 0, 5, 0, 0, 0);
    playsound(1440, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_02_02", 2000, "Bone_Root", 1440)
    {
     transform(vector3(0,0,0.5));
    };
    areadamage(1482, 0, 1, 1, 2.5, true) {
      stateimpact("kDefault", 32020502);
    };
    playsound(1490, "hit2", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1490, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,70));
  };
};


//冲刺突进
skill(381603)
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
      stateimpact("kDefault", 38160301);
      stateimpact("kLauncher", 38160302);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(900, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//破防连击
skill(381604)
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
      stateimpact("kDefault", 38160401);
    };
    cleardamagestate(800);
    areadamage(1070, 0, 1, 2, 3.5, true) {
      stateimpact("kDefault", 38160402);
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


//冲刺突进
skill(381605)
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
      stateimpact("kDefault", 38160501);
      stateimpact("kLauncher", 38160502);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(900, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//破防连击
skill(381606)
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
      stateimpact("kDefault", 38160601);
    };
    cleardamagestate(800);
    areadamage(1070, 0, 1, 2, 3.5, true) {
      stateimpact("kDefault", 38160602);
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