//召唤特效
skill(320400)
{
  section(3000)
  {
    destroyself(2990);
  };
};

//寒冰箭
skill(320401)
{
  section(766)//第一段
  {
    animation("Skill01_01");
  };

  section(1033)//第二段
  {
    animation("Skill01_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_01", 3204011, vector3(0, 0, 0));
  };
};

//寒冰箭飞行
skill(3204011)
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
      stateimpact("kDefault", 32040101);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

//多重寒冰箭
skill(320402)
{
  section(766)//第一段
  {
    animation("Skill02_01");
  };

  section(966)//第二段
  {
    animation("Skill02_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_01", 3204021, vector3(0, 0, 0));
    summonnpc(100, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_01", 3204022, vector3(0, 0, 0));
    summonnpc(200, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_01", 3204023, vector3(0, 0, 0));
  };
};

//多重寒冰箭飞行
skill(3204021)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,-15,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 10, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040102);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204022)
{
  section(3000)//第一段
  {
     movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 10, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040101);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204023)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,15,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 10, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040101);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

//寒冰地爆
skill(320403)
{
  section(766)//第一段
  {
    animation("Skill02_01");
  };

  section(1033)//第二段
  {
    animation("Skill02_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_01", 3204031, vector3(0, 0, 0));
  };
};

//寒冰地爆飞行
skill(3204031)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,0,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(2500, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(2500, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 1250, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1500, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 1500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(2500, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 1750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(2000, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 2000, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(2500, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 2250, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(2500, 0, 0, 0, 1.5, true)
		{
			stateimpact("kDefault", 32040103);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HanBingJian_03", 3000, vector3(0, 0, 0), 2500, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(2990);
  };
};
