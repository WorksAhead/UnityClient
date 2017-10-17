skill(161001)
{
	section(2030)
	{
		//全局参数
		addbreaksection(1, 1900, 2030);
		addbreaksection(10, 1900, 2030);
		addbreaksection(20, 1050, 2030);
		addbreaksection(30, 1900, 2030);
		movecontrol(true);
		animation("hero_skill01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		addimpacttoself(0, 16100199, -1);	
		//帧10
		setanimspeed(333, "hero_skill01", 0.5);
		//帧19
		setanimspeed(933, "hero_skill01", 2);
		//帧25
		setanimspeed(1033, "hero_skill01", 1);
		//帧33
		setanimspeed(1300, "hero_skill01", 0.2);
		//帧36
		setanimspeed(1930, "hero_skill01", 1);
		//帧39
		enablechangedir(0, 900);
		//findmovetarget(0, vector3(0, 0, 4), 5, 90, 0.8, 0.2, 0, -0.8);
        setlayer(0, 1300, "NoBlockCharacter");
		startcurvemove(0, false, 0.1, 0, 30, 30, 0, -100, -150, 0.1, 0, 20, 15, 0, -100, -120, 0.73, 0, 10, 3, 0, -20, 0);
		findmovetarget(920, vector3(0, 0, 3), 3, 90, 0.8, 0.2, 0, -0.8);
		startcurvemove(930, true, 0.1, 0, -50, 20, 0, -500, 0);
		//sceneeffect("Hero_FX/5_zhankuang/5_hero_hero_skill01_01",1000,vector3(0,0,0),0,eular(0,0,0),vector3(1,1,1),true);
		//sceneeffect("Hero_FX/5_zhankuang/5_hero_hero_skill01_02",2500,vector3(0,0,1),1030,eular(0,0,0),vector3(1,1,1),true);
                charactereffect("Aaron_fx_skill/Prefab/fashi_skill02",4000,"hero_coordinateCenter_refpoint",300, false);		
		//playsound(0, "skill1001", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/hero_skill01", false);		
		//playsound(1000, "skill1002", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/ZK_Voice_TiaoZa", false);		
		//playsound(1200, "skill1003", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/zhankuang/zhankuang_tiaozhan_02", false);
		colliderdamage(1030, 1000, true, true, 100, 10)
		{
			stateimpact("kLauncher", 16100101);
			stateimpact("kDefault", 16100102);
			bonecollider("hero/1_jianshi/shenguizhancollider","Bone_Root", true);
		};
		shakecamera2(1030, 200, false, true, vector3(0.5, 0.8, 0), vector3(40, 40, 0), vector3(24, 24, 0), vector3(80, 60, 0));		
	};
	oninterrupt()
	{
        setlayer(0, 10, "Player");
		stopeffect(0);
	};
	
	onstop()
	{
        setlayer(0, 10, "Player");
		stopeffect(300);
	};
};