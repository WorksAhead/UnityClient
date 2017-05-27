skill(160601)
{
	section(4066)
	{
		//全局参数
		addbreaksection(1, 3933, 3933);
		addbreaksection(10, 3933, 3933);
		addbreaksection(20, 0, 3933);
		addbreaksection(30, 3933, 3933);
		movecontrol(true);
		animation("zhankuang_jiantuoye_01",0);
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(233, true, 0.1, 0, 0, 0, 0, 0, 200, 0.05, 0, 0, 20, 0, 0, 0, 0.05, 0, 0, 20, 0, 0, -200);

		areadamage(250, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16060101);
		};

		colliderdamage(233, 1500, true, true, 1500, 1)
		{
			attachenemy("ef_body", eular(0, 0, 0), 16160201, -1, 16160202, -1, 16160203, -1);
			bonecollider("hero/5_zhankuang/jiantuoyecollider","ef_rightweapon01", true);
		};
		animation("zhankuang_jiantuoye_03",433);
		
		//帧1
		setanimspeed(466, "zhankuang_jiantuoye_03", 2);
		//帧21
		setanimspeed(800, "zhankuang_jiantuoye_03", 0.5);
		//帧28
		setanimspeed(1266, "zhankuang_jiantuoye_03", 2);
		//帧32
		setanimspeed(1333, "zhankuang_jiantuoye_03", 1);
		//帧48
		animation("zhankuang_jiantuoye_04", 1866);
		areadamage(2066, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16060101);
		};
		areadamage(2433, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16060101);
		};
		areadamage(2800, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16060101);
		};
		areadamage(3700, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16060101);
		};
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};