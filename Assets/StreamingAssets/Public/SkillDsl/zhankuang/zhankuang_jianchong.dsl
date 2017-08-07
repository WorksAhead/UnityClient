skill(160801)
{
	section(900)
	{
		//全局参数
		addbreaksection(1, 700, 900);
		addbreaksection(10, 700, 900);
		addbreaksection(20, 0, 900);
		addbreaksection(30, 600, 900);
		movecontrol(true);
		animation("hero_skill01");
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");

		//帧1
		setanimspeed(33, "hero_skill01", 0.5);
		
		//帧4
		setanimspeed(233, "hero_skill01", 3);
		
		//帧13
		setanimspeed(333, "hero_skill01", 1);
		//帧49
		
		findmovetarget(0, vector3(0, 0, 0), 2, 180, 0.8, 0.2, 0, 0, false);
		startcurvemove(10, true, 0.1, 0, 0, -2, 0, 0, 10);
		summonnpc(300, 101, "Hero/6_fashi/base", 160811, vector3(0,0,0));
        summonnpc(300, 101, "Hero/6_fashi/base", 160812, vector3(0,0,0));
        summonnpc(300, 101, "Hero/6_fashi/base", 160813, vector3(0,0,0));
		playsound(300, "skill08012", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/ZK_Voice_JiaoSha_01", false);
		//shakecamera2(1050, 100, false, true, vector3(0.3, 0.4, 0), vector3(40, 40, 0), vector3(16, 16, 0), vector3(80, 60, 0));
	};

	
	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		stopeffect(0);
	};
	
	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		stopeffect(800);
	};
};

skill(160802)
{
	section(1050)
	{
		//全局参数
		addbreaksection(1, 900,1050);
		addbreaksection(10, 900, 1050);
		addbreaksection(20, 0, 1050);
		addbreaksection(30, 0, 200);
		addbreaksection(30, 700, 1050);

		movecontrol(true);
        enablechangedir(0, 200);
		animation("zhankuang_jiaosha_02");
		movechild(100, "1_JianShi_w_01", "ef_rightweapon01");

		//帧1
		setanimspeed(33, "zhankuang_jiaosha_02", 2);

		//帧9
		setanimspeed(166, "zhankuang_jiaosha_02", 1);

		//sound("Sound/JianShi/JianChong_02",233);
		playsound(250, "skill0802", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/ZK_Voice_JiaoSha_02", false);
		
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_jianchong_01_02",1000,"Bone_Root",266, false);
	
		colliderdamage(250, 950, true, true, 50, 14)
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kDefault", 16080201);
			bonecollider("hero/5_zhankuang/jianchongcollider","Bone_Root", true);
		};

        playsound(250, "skill08024", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_jiaosha_04", false);
		playsound(300, "hit08021", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
		playsound(550, "hit08022", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
		playsound(800, "hit08023", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	};
	oninterrupt()
	{
		stopeffect(300);
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	};
	
	onstop()
	{
		stopeffect(300);
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	};
};

//绞杀：近
skill(160811)
{
    section(1000)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,1.5),eular(0,0,0),"RelativeOwner",false); 
        areadamage(33, 0, 0, 0, 2.5, false) 
		{
			stateimpact("kDefault", 16120201);
		};        
        sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_03",1000,vector3(0,0,0),33);
        playsound(0, "skill08011", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/hero_skill01", false);
		destroyself(1000);
    };
	oninterrupt()
	{
        destroyself(0);
	};
	onstop()
	{
        destroyself(0);
	};
};


//绞杀：中
skill(160812)
{
    section(1000)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,3),eular(0,0,0),"RelativeOwner",false); 
        areadamage(233, 0, 0, 0, 2.5, false) 
		{
			stateimpact("kDefault", 16120202);
		};        
        sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_03",1000,vector3(0,0,0),233);
        playsound(200, "skill08012", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_jiaosha_02", false);
        destroyself(1000);
    };
	oninterrupt()
	{
        destroyself(0);
	};
	onstop()
	{
        destroyself(0);
	};
};

//绞杀：远
skill(160813)
{
    section(1000)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,4.5),eular(0,0,0),"RelativeOwner",false); 
        areadamage(533, 0, 0, 5.5, 2.5, false) 
		{
			stateimpact("kDefault", 16120201);
		};        
        sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_03",1000,vector3(0,0,0),533);
        playsound(500, "skill08013", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_jiaosha_03", false);
        destroyself(1000);
    };
	oninterrupt()
	{
        destroyself(0);
	};
	onstop()
	{
        destroyself(0);
	};
};