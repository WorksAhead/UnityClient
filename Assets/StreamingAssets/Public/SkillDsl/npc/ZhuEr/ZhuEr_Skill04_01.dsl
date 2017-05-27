//普攻
skill(410101)
{
  section(1)
  {
    movecontrol(true);
  };
  section(766)//第一段
  {
    movecontrol(true);
    animation("Skill01_01");
  };
  section(533)//第二段
  {
    animation("Skill01_02");
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_01", 4101011, vector3(1, 0.8, 0));
  };
  section(533)
  {
    animation("Skill01_02");
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_01", 4101011, vector3(1, 0.8, 0));
  };
  section(533)
  {
    animation("Skill01_02");
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_01", 4101011, vector3(1, 0.8, 0));
  };

  section(800)//第二段
  {
    animation("Skill01_03");
  };
  section(866)
  {
    animation("Skill01_04");
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_01", 4101012, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_01", 4101013, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_01", 4101014, vector3(0, 0, 0));
  };
};

//普攻飞行
skill(4101011)
{
  section(3000)//第一段
  {
    setlifetime(0, 300);
    movecontrol(true);
    //设定方向为施法者方向
    //目标选择
    findmovetarget(0, vector3(0, 0, 1), 10, 30, 0.1, 0.9, 0, 3);
    //设定方向为施法者方向
    settransform(1," ",vector3(0, 1.6, 0),eular(0,0,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.3, 0, 0, 50, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 41010101);
      stateimpact("kLauncher", 41010102);
      stateimpact("kKnockDown", 12990000);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

skill(4101012)
{
  section(3000)//第一段
  {
    setlifetime(0, 300);
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(-0.6,1.8,0.3),eular(0,-15,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 0.3, 0, 0, 50, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 41010103);
      stateimpact("kLauncher", 41010104);
      stateimpact("kKnockDown", 12990000);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

skill(4101013)
{
  section(3000)//第一段
  {
    setlifetime(0, 300);
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(-0.6,1.8,0.3),eular(0,15,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 0.3, 0, 0, 50, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 41010103);
      stateimpact("kLauncher", 41010104);
      stateimpact("kKnockDown", 12990000);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

skill(4101014)
{
  section(3000)//第一段
  {
    setlifetime(0, 300);
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(-0.6,1.8,0.3),eular(0,0,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 0.3, 0, 0, 50, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 41010103);
      stateimpact("kLauncher", 41010104);
      stateimpact("kKnockDown", 12990000);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_PuGong_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};