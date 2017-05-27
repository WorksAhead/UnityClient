
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


