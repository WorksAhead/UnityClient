//召唤物出生技能
skill(3204001)
{
  section(500)
  {
    animation("Stand");
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_ChuSheng_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
};

//目标身后瞬身
skill(3204002)
{
  section(466)//起手
  {
    movecontrol(true);
    animation("Skill02_01");
    setenable(300, "Visible", false);
    sceneeffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_ShanShen_01", 3000, vector3(0, 0, 0), 290, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(100)//瞬移
  {
    addimpacttoself(1, 12990001, 500);
    findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, 5);
    startcurvemove(10, true, 0.05, 0, 0, 20, 0, 0, 0);
    settransform(90, " ", vector3(0, 0, 0), eular(0, 180, 0), "RelativeSelf", true);
  };

  section(966)//收招
  {
    animation("Skill02_02");
    setenable(0, "Visible", true);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setenable(0, "Visible", true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setenable(0, "Visible", true);
  };
};

//自身身后瞬身
skill(3204003)
{
  section(466)//起手
  {
    movecontrol(true);
    animation("Skill02_01");
    setenable(300, "Visible", false);
    sceneeffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_ShanShen_01", 3000, vector3(0, 0, 0), 290, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(100)//第一段
  {
    startcurvemove(1, true, 0.1, 0, 0, -30, 0, 0, 0);
  };

  section(966)//收招
  {
    animation("Skill02_02");
    setenable(0, "Visible", true);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setenable(0, "Visible", true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    setenable(0, "Visible", true);
  };
};

//瞬移
skill(3204004)
{
  section(466)
  {
    animation("Skill02_01");
    movecontrol(true);
    findmovetarget(5,vector3(0,0,0),10,90,0.5,0.5,0,1);
  };

  section(100)
  {
    setenable(0, "Visible", false);
    sceneeffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_ShanShen_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    settransform(10," ",vector3(3,0.2,-6),eular(0,0,0),"RelativeTarget",false,true);
  };

  section(966)
  {
    animation("Skill02_02");
    setenable(0, "Visible", true);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setenable(0, "Visible", true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    setenable(0, "Visible", true);
  };
};

//平移
skill(3204005)
{
  section(1200)
  {
    movecontrol(true);
    animation("Walk");
    startcurvemove(1, false, 1.2, 3, 0, 0.01, 0, 0, 0);
    findmovetarget(100,vector3(0,0,0),10,360,0.5,0.5,0,5);
    facetotarget(101,1000,300);
  };
};

//平移2
skill(3204006)
{
  section(1200)
  {
    movecontrol(true);
    animation("Walk");
    startcurvemove(1, false, 1.2, -3, 0, 0.01, 0, 0, 0);
    findmovetarget(100,vector3(0,0,0),10,360,0.5,0.5,0,5);
    facetotarget(101,1000,300);
  };
};


//暗影箭
skill(320407)
{
  section(600)//第一段
  {
    animation("Skill01_01");
  };

  section(1566)//第二段
  {
    animation("Skill01_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_AnYingJian_01", 3204071, vector3(0, 0, 0));
  };
};

//暗影箭飞行
skill(3204071)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,0,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 10, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040301);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_AnYingJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};




//召唤近战兵
skill(320408)
{
  section(466)//第一段
  {
    animation("Skill02_01");
  };

  section(1500)//第二段
  {
    animation("Skill02_02");
    //召唤NPC
    summonnpc(0, 3014, "", 3204001, vector3(0, 0, 2), eular(0, 0, 0), 20001, false);
  };
};

//召唤远程兵
skill(320409)
{
  section(466)//第一段
  {
    animation("Skill02_01");
  };

  section(1500)//第二段
  {
    animation("Skill02_02");
    //召唤NPC
    summonnpc(0, 3015, "", 3204001, vector3(0, 0, 2), eular(0, 0, 0), 20002, false);
  };
};

//召唤精英近战兵
skill(320410)
{
  section(466)//第一段
  {
    animation("Skill02_01");
  };

  section(1500)//第二段
  {
    animation("Skill02_02");
    //召唤NPC
    summonnpc(0, 3024, "", 3204001, vector3(0, 0, 2), eular(0, 0, 0), 20001, false);
  };
};

//召唤精英远程兵
skill(320411)
{
  section(466)//第一段
  {
    animation("Skill02_01");
  };

  section(1500)//第二段
  {
    animation("Skill02_02");
    //召唤NPC
    summonnpc(0, 3025, "", 3204001, vector3(0, 0, 2), eular(0, 0, 0), 20001, false);
  };
};

//召唤大壮
skill(320412)
{
  section(466)//第一段
  {
    animation("Skill02_01");
  };

  section(1500)//第二段
  {
    animation("Skill02_02");
    //召唤NPC
    summonnpc(0, 1063, "", 3204001, vector3(0, 0, 2), eular(0, 0, 0), 20001, false);
  };
};



