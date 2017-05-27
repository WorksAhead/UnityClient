//火焰箭
skill(320404)
{
  section(766)//第一段
  {
    animation("Skill01_01");
  };

  section(1033)//第二段
  {
    animation("Skill01_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204041, vector3(0, 0, 0));
  };
};

//火焰箭飞行
skill(3204041)
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
      stateimpact("kDefault", 32040201);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

//火焰螺旋
skill(320405)
{
  section(766)//第一段
  {
    animation("Skill02_01");
  };

  section(966)//第二段
  {
    animation("Skill02_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204050, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204051, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204052, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204053, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204054, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204055, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204056, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204057, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204058, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204059, vector3(0, 0, 0));
  };
};

//火焰螺旋飞行
skill(3204050)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, 12, 0, 0, -20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204051)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, -12, 0, 0, 20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204052)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,72,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, 12, 0, 0, -20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204053)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,72,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, -12, 0, 0, 20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204054)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,144,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, 12, 0, 0, -20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204055)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,144,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, -12, 0, 0, 20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204056)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,216,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, 12, 0, 0, -20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204057)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,216,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, -12, 0, 0, 20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204058)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,288,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, 12, 0, 0, -20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204059)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,288,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 10, 0, 0, -10);
    startcurvemove(1010, true, 2, -12, 0, 0, 20, 0, -10);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040202);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

//火焰地爆
skill(320406)
{
  section(766)//第一段
  {
    animation("Skill02_01");
  };

  section(1033)//第二段
  {
    animation("Skill02_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204061, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204062, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_01", 3204063, vector3(0, 0, 0));
  };
};

//火焰地爆飞行
skill(3204061)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,0,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1250, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1250, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1500, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1550);
  };
};
skill(3204062)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,30,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1250, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1250, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1500, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1550);
  };
};
skill(3204063)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,-30,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1250, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1250, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040203);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_HuoYanJian_03", 3000, vector3(0, 0, 0), 1500, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1550);
  };
};
