//剧毒箭
skill(320413)
{
  section(600)//第一段
  {
    animation("Skill01_01");
  };

  section(1566)//第二段
  {
    animation("Skill01_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_01", 3204131, vector3(0, 0, 0));
  };
};

//剧毒箭飞行
skill(3204131)
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
      stateimpact("kDefault", 32040401);
      sceneboxcollider(vector3(0.7, 0.7, 0.7), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};

//剧毒新星
skill(320414)
{
  section(466)//第一段
  {
    animation("Skill02_01");
  };

  section(1500)//第二段
  {
    animation("Skill02_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204140, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204141, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204142, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204143, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204144, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204145, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204146, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204147, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204148, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_04", 3204149, vector3(0, 0, 0));
  };
};

//剧毒新星飞行
skill(3204140)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204141)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,36,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204142)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,72,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204143)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,108,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204144)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,144,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204145)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,180,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204146)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,216,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204147)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,252,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204148)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,288,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};
skill(3204149)
{
  section(3000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1.5,0),eular(0,324,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    //角色移动
    startcurvemove(10, true, 3, 0, 0, 5, 0, 0, 0);
    //碰撞盒
    colliderdamage(0, 3000, true, true, 0, 1)
    {
      stateimpact("kDefault", 32040402);
      sceneboxcollider(vector3(0.4, 0.4, 0.4), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //销毁
    destroyself(2990);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_02", 320400, vector3(0, 0, 1.2));
    destroyself(0);
  };
};







//剧毒地爆
skill(320415)
{
  section(466)//第一段
  {
    animation("Skill02_01");
  };

  section(1500)//第二段
  {
    animation("Skill02_02");
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_01", 3204151, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_01", 3204152, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_01", 3204153, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_01", 3204154, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_01", 3204155, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_01", 3204156, vector3(0, 0, 0));
  };
};

//剧毒地爆飞行
skill(3204151)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,15,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1050);
  };
};
skill(3204152)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,-15,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1050);
  };
};
skill(3204153)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,45,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1050);
  };
};
skill(3204154)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,-45,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1050);
  };
};
skill(3204155)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,75,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1050);
  };
};
skill(3204156)
{
  section(2000)//第一段
  {
    movecontrol(true);
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,-75,0),"RelativeOwner",false);
    //角色移动
    startcurvemove(10, true, 0.5, 0, 0, 5, 0, -10, 0);
    startcurvemove(500, true, 2.7, 0, 0, 5, 0, 0, 0);
    //伤害判定
    areadamage(500, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(750, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    areadamage(1000, 0, 0, 0, 1, true)
		{
			stateimpact("kDefault", 32040403);
		};
    sceneeffect("Monster_FX/Campaign_Desert/04_Wizard/6_Mon_Wizard_DuJian_03", 3000, vector3(0, 0, 0), 1000, eular(0, 0, 0), vector3(1, 1, 1), true);
    //销毁
    destroyself(1050);
  };
};
