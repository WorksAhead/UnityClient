//蓄力箭
skill(410201)
{
  section(1)
  {
    movecontrol(true);
  };
  section(1633)//第一段
  {
    movecontrol(true);
    animation("Skill02_01");
    charactereffect("Monster_FX/zhuer/6_Mon_ZhuEr_XuLiJian_04", 2000, "Bone_Root", 1);
  };

  section(866)//第二段
  {
    animation("Skill02_02");
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_XuLiJian_01", 4102011, vector3(0, 0, 0));
  };
};

//蓄力箭飞行
skill(4102011)
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
      stateimpact("kDefault", 41020101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/zhuer/6_Mon_ZhuEr_XuLiJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
