//冲撞
skill(381001)
{
  section(1667)
  {
    movecontrol(true);
    animation("Ridicule");
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 2000, vector3(0,0.2,0), 0, eular(0, 0, 0), vector3(1,1,1), true);
    playsound(90, "huiwu", "Sound/Npc/Mon", 5000, "Sound/Npc/BOSS_YANSHOU_CHONGHJI", false);
  };
  section(800)
  {
    animation("Skill_01")
    {
      wrapmode(4);
    };
    setanimspeed(400, "Skill_01", 0.2, true);
    setanimspeed(790, "Skill_01", 1, true);
    startcurvemove(240, true, 0.2, 0, 0, 40, 0, 0, 0);
    sceneeffect("Monster_FX/Boss/YanShou/6_Mon_YanShou_ChongZhuang_01", 1500, vector3(0,0,2), 220, eular(0, 0, 0), vector3(1,1,1), true);
    colliderdamage(240, 200, false, false, 0, 0)
    {
      stateimpact("kDefault", 38100101);
      sceneboxcollider(vector3(5, 3, 5), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(260, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(350, 100, false, true, vector3(1.5,0.5,1.5), vector3(50,20,50),vector3(1,0.3,1),vector3(70,70,70));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "huiwu");
  };
};


//喷火
skill(381002)
{
  section(833)
  {
    animation("Skill_02A");
    setanimspeed(100, "Skill_02A", 0.4, true);
    setanimspeed(450, "Skill_02A", 1, true);
    charactereffect("Monster_FX/Boss/YanShou/6_Mon_YanShou_PenQi_01",1500,"ef_mouth",30,false);
  };
  section(2800)
  {
    animation("Skill_02B")
    {
      wrapmode(4);
    };
    playsound(0, "kaipao", "Sound/Npc/Mon", 3500, "Sound/Npc/BOSS_YANSHOU_PENHUO", false);
    charactereffect("Monster_FX/Boss/YanShou/6_Mon_YanShou_TuXi_01", 2100, "ef_mouth", 0);//喷火
    colliderdamage(0, 2650, true, true, 300, 10)
    {
      stateimpact("kDefault", 38100201);
      boneboxcollider(vector3(3, 3, 15), "ef_mouth", vector3(0, 0, 0.5), eular(0, 0, 0), true, false);
    };
    shakecamera2(50, 2700, false, true, vector3(0.1,0.3,0.1), vector3(100,50,100),vector3(0.2,0.6,0.2),vector3(90,100,90));
  };
  section(700)
  {
    animation("Skill_02C");
  };
    oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    setenable(0, "Damage", false);
    stopsound(0, "yujing");
    stopsound(0, "kaipao");
  };
};


//践踏
skill(381003)
{
  section(2067)
  {
    animation("Skill_03");
        setanimspeed(500, "Skill_03", 0.4, true);
    setanimspeed(1000, "Skill_03", 1, true);
    charactereffect("Monster_FX/Boss/YanShou/6_Mon_YanShou_PenQi_01",1550,"ef_mouth",1000,false);
    sceneeffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_HuoLang_01", 2000, vector3(0,-0.7,2), 1250, eular(0,0,0), vector3(1,1,1), true);
    playsound(1280, "kaipao", "Sound/Npc/Mon", 4000, "Sound/Npc/BOSS_YANSHOU_JIANTA", false);
    areadamage(1280, 0, 1, 2, 5.5, false) {
      stateimpact("kDefault", 38100301);
    };
    shakecamera2(1250, 300, false, true, vector3(0.4,2.5,0.4), vector3(75,75,75),vector3(0.4,2.5,0.4),vector3(70,90,70));
    summonnpc(1270, 101, "Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_ranshao_01", 381008, vector3(0, 0, 0));
    summonnpc(1270, 101, "Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_ranshao_01", 381009, vector3(0, 0, 0));
    summonnpc(1270, 101, "Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_ranshao_01", 381010, vector3(0, 0, 0));
    summonnpc(1270, 101, "Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_ranshao_01", 381011, vector3(0, 0, 0));
  };
    oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "kaipao");
  };
};


//火雨
skill(381004)
{
  section(1400)
  {
    animation("Skill_04");
    playsound(0, "kaipao", "Sound/Npc/Mon", 6500, "Sound/Npc/BOSS_YANSHOU_HUOYU", false);
    setanimspeed(420, "Skill_04", 0.15, true);
    setanimspeed(1000, "Skill_04", 1, true);
    summonnpc(500, 101, "Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 381005, vector3(0, 0, 0));
    summonnpc(650, 101, "Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 381005, vector3(0, 0, 0));
    summonnpc(800, 101, "Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 381005, vector3(0, 0, 0));
    summonnpc(950, 101, "Monster_FX/Boss/YanShou/6_Mon_YuJing_01", 381005, vector3(0, 0, 0));
    shakecamera2(420, 3500, false, true, vector3(0.08,0.2,0.08), vector3(60,40,60),vector3(0.08,0.2,0.08),vector3(95,100,95));
  };
    oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "kaipao");
  };
};

//火雨召唤物
skill(381005)
{
  section(6000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    setenable(20, "Visible", true);
    //循环段
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/YanShou/6_Mon_YanShou_HuoYu_01", 5000, vector3(0,0,0),1000);
    colliderdamage(1000, 5000, true, true, 1000, 5)
    {
      stateimpact("kDefault", 38100401);
      sceneboxcollider(vector3(6, 5, 6), vector3(0, 2.5, 0), eular(0, 0, 0), true, false);
    };
    playsound(1000, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_ranshao_01", false);
    destroyself(5000);
  };
};


//点燃
skill(381006)
{
  section(1400)
  {
    animation("Skill_04");
    summonnpc(700, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 381007, vector3(0, 0, 0));
  };
};


//被点燃
skill(381007)
{
  section(6000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(5," ",vector3(0,0.1,0),eular(0,0,0),"RelativeTarget",true);
    setenable(20, "Visible", true);
    setenable(1000, "Visible", false);
    //循环段
    findmovetarget(2,vector3(0,0,0),30,360,0.5,0.5,0,0);
    addimpacttotarget(5,38100600,6000);
    sceneeffect("Monster_FX/ZhuLuoFu/6_Mon_ZhuLuoFu_HuoLang_01", 1000, vector3(0,-0.7,0),5000);
    colliderdamage(5000, 150, false, false, 0, 0)
    {
      stateimpact("kDefault", 38100601);
      sceneboxcollider(vector3(8, 4, 8), vector3(0, 2, 0), eular(0, 0, 0), false, false);
    };
    playsound(1000, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_baozha_01", false);
    destroyself(6000);
  };
};


//蔓延
skill(381008)
{
  section(8500)
  {
    movecontrol(true);
    settransform(0," ",vector3(0, 0.3, 1.5),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(30, true, 3, 0, 0, 4, 0, 0, 0);
    colliderdamage(30, 4000, true, true, 200, 20)
    {
      stateimpact("kDefault", 38100302);
      sceneboxcollider(vector3(3, 3, 3), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    destroyself(8400);
  };
};


//蔓延
skill(381009)
{
  section(8500)
  {
    movecontrol(true);
    settransform(0," ",vector3(0, 0.3, 1.5),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(30, true, 3, 0, 0, -4, 0, 0, 0);
    colliderdamage(30, 4000, true, true, 200, 20)
    {
      stateimpact("kDefault", 38100302);
      sceneboxcollider(vector3(3, 3, 3), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    destroyself(8400);
  };
};


//蔓延
skill(381010)
{
  section(8500)
  {
    movecontrol(true);
    settransform(0," ",vector3(0, 0.3, 1.5),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(30, true, 3, -4, 0, 0, 0, 0, 0);
    colliderdamage(30, 4000, true, true, 200, 20)
    {
      stateimpact("kDefault", 38100302);
      sceneboxcollider(vector3(3, 3, 3), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    destroyself(8400);
  };
};

//蔓延
skill(381011)
{
  section(8500)
  {
    movecontrol(true);
    settransform(0," ",vector3(0, 0.3, 1.5),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(30, true, 3, 4, 0, 0, 0, 0, 0);
    colliderdamage(30, 4000, true, true, 200, 20)
    {
      stateimpact("kDefault", 38100302);
      sceneboxcollider(vector3(3, 3, 3), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    destroyself(8400);
  };
};

//嘲讽
skill(381012)
{
  section(1667)
  {
    animation("Ridicule");
    setanimspeed(330, "Ridicule", 0.5, true);
    setanimspeed(1100, "Ridicule", 1, true);
  };
};