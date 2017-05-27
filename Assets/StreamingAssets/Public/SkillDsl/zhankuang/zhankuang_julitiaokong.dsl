skill(160401)
{
	section(1000)
	{
		//全局参数
		addbreaksection(1, 900, 1000);
		addbreaksection(10, 900, 1000);
		addbreaksection(20, 0, 1000);
		addbreaksection(30, 900, 1000);
		movecontrol(true);
		animation("zhankuang_julitiaokong_01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		//findmovetarget(0, vector3(0, 0, 0), 5, 60, 0.8, 0.2, 0, -2.5);
		startcurvemove(0, true, 0.1, 0, 0, 3, 0, 0, 20, 0.05, 0, 0, 5, 0, 0, -40);
		
		addimpacttoself(0, 16040199, -1);	
		startcurvemove(300, true, 0.17, 0, 0, 20, 0, 0, -60);
				
		addimpacttoself(0, 16040102, 0);
		//帧1
		setanimspeed(33, "zhankuang_julitiaokong_01", 2.5);

		//帧11
		setanimspeed(200, "zhankuang_julitiaokong_01", 1.5);

		//帧14
		setanimspeed(266, "zhankuang_julitiaokong_01", 2);

		//帧18
		setanimspeed(333, "zhankuang_julitiaokong_01", 1.5);

		//帧24
		setanimspeed(466, "zhankuang_julitiaokong_01", 0.125);
		
		//帧25
		setanimspeed(733, "zhankuang_julitiaokong_01", 0.5);

		//帧27
		setanimspeed(900, "zhankuang_julitiaokong_01", 1);
		//帧48
		
		playsound(350, "skill0401", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_julitiaokong_01", false);
		playsound(300, "skill0402", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/ZK_Voice_JuLiTiaoKong_01", false);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_julitiaokong_01_01",1000,"Bone_Root",400);

		areadamage(420, 0, 1, 0, 4, true) 
		{
			stateimpact("kLauncher", 16040112);
			stateimpact("kDefault", 16040111);
		};
		
		enablechangedir(900, 1000);

		playsound(430, "hit0401", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};

		//lockframe(433, "zhankuang_julitiaokong_01", true, 0, 100, 1, 10, true, 1, 1, 1);
		//shakecamera2(433, 200, false, true, vector3(0.5, 0.8, 0), vector3(40, 40, 0), vector3(24, 24, 0), vector3(80, 60, 0));
	};
	oninterrupt()
	{
		stopeffect(0);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};

skill(160402)
{
	section(1933)
	{
		//全局参数
		addbreaksection(1, 700, 1933);
		addbreaksection(10, 600, 1933);
		addbreaksection(20, 0, 1933);
		addbreaksection(30, 600, 1933);
		movecontrol(true);
		animation("zhankuang_julitiaokong_02");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		enablechangedir(0, 200);
		addimpacttoself(0, 16040299, -1);	
		//startcurvemove(0, true, 0.2, 0, 0, 1, 0, 0, 10, 0.15, 0, 0, 3, 0, 0, -20, 0.2, 0, 0, 0, 0, 0, 0, 0.3, 0, 0, 20, 0, 0, -60);
		
		//帧1
		setanimspeed(33, "zhankuang_julitiaokong_02", 2.5);

		//帧11
		setanimspeed(166, "zhankuang_julitiaokong_02", 1);

		//帧14
		setanimspeed(266, "zhankuang_julitiaokong_02", 2);
		
		//帧22
		setanimspeed(400, "zhankuang_julitiaokong_02", 1);

		//帧24
		setanimspeed(466, "zhankuang_julitiaokong_02", 0.1);
		
		//帧26
		setanimspeed(1133, "zhankuang_julitiaokong_02", 1);
		//帧50
		
		playsound(300, "skill0411", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/zhankuang_julitiaokong_02", false);

		playsound(250, "skill0412", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/ZK_Voice_JuLiTiaoKong_02", false);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_julitiaokong_02_01",1000,"Bone_Root",266);
		
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_julitiaokong_02_02",1000,vector3(0,0,0),333,eular(0,0,0),vector3(1,1,1),true);
		
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_julitiaokong_02_05",1000,vector3(0,0,2.5),333,eular(0,0,0),vector3(1,1,1),true);
	
		areadamage(333, 0, 1, 0, 4, true) 
		{
			stateimpact("kDefault", 16040201);
		};

		playsound(310, "hit0402", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};

		//lockframe(366, "zhankuang_julitiaokong_02", true, 0, 100, 1, 10, true, 1, 1, 1);
		shakecamera2(366, 200, false, true, vector3(1, 1.6, 0), vector3(40, 40, 0), vector3(36, 36, 0), vector3(80, 60, 0));
	};
	
	oninterrupt()
	{
		stopeffect(0);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};