skill(160501)
{
	section(1400)
	{
		//全局参数
		addbreaksection(1, 1300, 1400);
		addbreaksection(10, 1200, 1400);
		addbreaksection(20, 0, 1400);
		addbreaksection(30, 1100, 1400);
		movecontrol(true);
		animation("zhankuang_jujianchongji_01",0);
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		startcurvemove(500, true, 0.15, 0, 0, 0, 0, 0, 200, 0.15, 0, 0, 30, 0, 0, -40);
		//19
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_jianchong_01_01",1000,"Bone_Root",500, true);
		colliderdamage(500, 300, true, true, 300, 1)
		{
			attachenemy("ef_body", eular(0, 0, 0), 16160201, -1, 16160202, -1, 16160203, -1);
			bonecollider("hero/5_zhankuang/jujianchongjicollider","ef_rightweapon01", true);
		};
		animation("zhankuang_jujianchongji_03",800);
		areadamage(1000, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16060101);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_jujianchongji_04",1000,"Bone_Root",950, false);
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_03_02",1000,vector3(0,0,1.8),1000);
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160502)
{
	section(1933)
	{
		//全局参数
		addbreaksection(10, 1200, 1933);
		addbreaksection(30, 1200, 1933);
		movecontrol(true);
		animation("zhankuang_xuanfengzhan_02");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

	};
	
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160599)
{
	section(2000)
	{
		movecontrol(true);

		settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
   
		startcurvemove(0, false, 2, 0, 0, 12, 0, 0, 0);

		colliderdamage(0, 2000, true, true, 200, 10)
		{
			stateimpact("kDefault", 16050101);
			stateimpact("kLauncher", 16050102);
			sceneboxcollider(vector3(9,3,2.4), vector3(0,2,0), eular(0,0,0), true, false);
		};
		destroyself(2000);
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};