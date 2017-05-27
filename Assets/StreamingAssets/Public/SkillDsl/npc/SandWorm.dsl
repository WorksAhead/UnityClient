//远距离钻出
skill(320511)
{
  section(667)
  {
    movecontrol(true);
    animation("Skill_01");
    shakecamera2(30, 600, false, true, vector3(0.05,0.1,0.05), vector3(50,50,50),vector3(0.2,0.4,0.2),vector3(90,90,90));
    setanimspeed(1, "Skill_01", 0.6, true);
    setanimspeed(660, "Skill_01", 1, true);
  };
  section(3000)
  {
    setenable(0, "Visible", false);
    animation("Walk") {
      wrapmode(2);
      speed(1);
    };
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(30, " ", vector3(0,0,0), eular(0,0,0), "RelativeTarget", false, true);
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/5_Mon_Worm_FanGunYan_01", 3000, "Bone_Root", 100);
  };
  section(667)
  {
    animation("Skill_03");
    setanimspeed(180, "Skill_03", 0.5, true);
    setenable(30, "Visible", true);
    facetotarget(0,100,0);
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_ZhenDangBo_01", 850, vector3(0,0,0),50);
    colliderdamage(50, 100, true, true, 200, 1)
    {
      stateimpact("kDefault", 32051101);
      sceneboxcollider(vector3(4, 2, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(30, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_zadijifei_02", false);
    shakecamera2(50, 100, false, true, vector3(0.1,1,0.1), vector3(100,100,100),vector3(0.3,3,0.3),vector3(70,80,70));
  };
  oninterrupt()
  {
    setenable(0, "Visible", true);
  };
  onstop()
  {
    setenable(0, "Visible", true);
  };
};


//近距离钻出
skill(320512)
{
  section(667)
  {
    movecontrol(true);
    animation("Skill_01");
    shakecamera2(30, 600, false, true, vector3(0.05,0.1,0.05), vector3(50,50,50),vector3(0.2,0.4,0.2),vector3(90,90,90));
    setanimspeed(1, "Skill_01", 0.6, true);
    setanimspeed(660, "Skill_01", 1, true);
  };
  section(1000)
  {
    setenable(0, "Visible", false);
    findmovetarget(5,vector3(0,0,0),5,360,0.5,0.5,0,0);
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/5_Mon_Worm_FanGunYan_01", 1000, "Bone_Root", 50);
    startcurvemove(30, true, 1, 0, 0, 4, 0, 0, 0);
    animation("Walk") {
      wrapmode(2);
    };
    // findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    // settransform(30, " ", vector3(0,0,0), eular(0,0,0), "RelativeTarget", false, true);
    // sceneeffect("Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 3000, vector3(0,0,0),50);
  };
  section(667)
  {
    animation("Skill_03");
    setanimspeed(180, "Skill_03", 0.5, true);
    setenable(30, "Visible", true);
    facetotarget(0,100,0);
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_ZhenDangBo_01", 850, vector3(0,0,0),30);
    colliderdamage(50, 200, true, true, 100, 1)
    {
      stateimpact("kDefault", 32051101);
      sceneboxcollider(vector3(4, 2, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(30, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_zadijifei_02", false);
    shakecamera2(50, 100, false, true, vector3(0.1,1,0.1), vector3(100,100,100),vector3(0.3,3,0.3),vector3(70,80,70));
  };
  oninterrupt()
  {
    setenable(0, "Visible", true);
  };
  onstop()
  {
    setenable(0, "Visible", true);
  };
};

//喷吐
skill(320513)
{
  section(2000)
  {
    animation("Ridicule") {
      wrapmode(4);
    };
    setanimspeed(1, "Ridecule", 1.5, true);
    setanimspeed(1950, "Ridecule", 1, true);
  };
  section(1333)
  {
    animation("Skill_04");
    setanimspeed(1, "Skill_04", 0.35, true);
    playsound(0, "kaipao", "Sound/Npc/Mon", 0, "Sound/Npc/BOSS_YANSHOU_PENHUO", false);
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/6_Mon_Worm_Pentu_01", 1800, "ef_mouth", 300);//喷火
    colliderdamage(300, 1800, true, true, 500, 3)
    {
      stateimpact("kDefault", 32051301);
      boneboxcollider(vector3(3, 3, 6.5), "ef_mouth", vector3(0, 0, 2.8), eular(0, 0, 0), true, true);
    };
    shakecamera2(300, 2700, false, true, vector3(0.05,0.05,0.05), vector3(80,80,80),vector3(0.2,0.2,0.2),vector3(97,97,97));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    setenable(0, "Damage", false);
    stopsound(0, "yujing");
    stopsound(0, "kaipao");
  };
};


//追敌
skill(320514)
{
  section(667)
  {
    movecontrol(true);
    animation("Skill_01");
    shakecamera2(30, 600, false, true, vector3(0.05,0.1,0.05), vector3(50,50,50),vector3(0.2,0.4,0.2),vector3(90,90,90));
    setanimspeed(30, "Skill_01", 0.6, true);
    setanimspeed(630, "Skill_01", 1, true);
  };
  section(6600) 
  {
    animation("Walk") {
      wrapmode(2);
    };
    setenable(0, "Visible", false);
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0,false);
    facetotarget(500,5500,50);
    startcurvemove(500, true, 3, 0, 0, 4, 0, 0, 0);
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/5_Mon_Worm_FanGunYan_01", 6000, "Bone_Root", 100);
    colliderdamage(1500, 5000, true, true, 200, 1)
    {
      stateimpact("kDefault", 32051401);
      sceneboxcollider(vector3(5, 2, 5), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
  };
  section(1167)
  {
    setenable(0, "Visible", true);
    animation("Skill_02");
    shakecamera2(30, 600, false, true, vector3(0.05,0.1,0.05), vector3(50,50,50),vector3(0.2,0.4,0.2),vector3(90,90,90));
    facetotarget(0,100,0);
  };
  onmessage("oncollide")
  {
    setenable(0, "Visible", true);
    animation("Skill_02");
    facetotarget(0,100,0);
  };
  oninterrupt()
  {
    setenable(0, "Visible", true);
    stopeffect(0);
  };
  onstop()
  {
    setenable(0, "Visible", true);
  };
};

//随机移动
skill(320515)
{
  section(667)
  {
    movecontrol(true);
    animation("Skill_01");
    shakecamera2(30, 600, false, true, vector3(0.05,0.1,0.05), vector3(50,50,50),vector3(0.2,0.4,0.2),vector3(90,90,90));
    setanimspeed(30, "Skill_01", 0.6, true);
    setanimspeed(630, "Skill_01", 1, true);
  };
  section(1600)
  {
    setenable(0, "Visible", false);
    animation("Walk") {
      wrapmode(2);
    };
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(30, " ", vector3(3,0,-8), eular(0,0,0), "RelativeTarget", false, true);
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/5_Mon_Worm_FanGunYan_01", 1600, "Bone_Root", 100);
  };
  section(1167)
  {
    setenable(0, "Visible", true);
    animation("Skill_02");
    facetotarget(0,100,0);
    shakecamera2(30, 600, false, true, vector3(0.05,0.1,0.05), vector3(50,50,50),vector3(0.2,0.4,0.2),vector3(90,90,90));
  };
  oninterrupt()
  {
    setenable(0, "Visible", true);
    stopeffect(0);
  };
  onstop()
  {
    setenable(0, "Visible", true);
  };
};