skill(160001)
{
	section(450)
	{
		addbreaksection(1, 350, 850);
		addbreaksection(10, 300, 850);
		addbreaksection(20, 0, 850);
		addbreaksection(30, 0, 120);
		addbreaksection(30, 210, 850);
		movecontrol(true);
		animation("hero_attack_0");	
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		//findmovetarget(100, vector3(0, 0, 0), 3, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(166, true, 0.05, 0, 0, 4, 0, 0, 80, 0.05, 0, 0, 8, 0, 0, -80);
		//֡1
		setanimspeed(33, "hero_attack_0", 2);
		//֡3
		setanimspeed(66, "hero_attack_0", 1);
		//֡5
		//setanimspeed(133, "hero_attack_0", 1);
		//֡6
		//setanimspeed(166, "hero_attack_0", 1.5);
		//֡10.5
		//setanimspeed(266, "zhankuang_julitiaokong_01", 1);
		//֡11
		//playsound(285, "skill0101", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/hero_attack_0_new", false);	
		//playsound(235, "skill0102", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/ZK_Voice_PuGong_01", false)
		//{
		//	audiogroup("Sound/zhankuang/ZK_Voice_PuGong_01", "Sound/zhankuang/ZK_Voice_PuGong_05");
		//}; 
		charactereffect("Hero_FX/5_zhankuang/5_hero_hero_attack_0_01", 500, "Bone_Root", 285, false);
		areadamage(285, 0, 1.7, 1, 2.3, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000102);
			stateimpact("kDefault", 16000101);
		};
		//playsound(300, "hit0001", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		//{
		//	audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		//};   
		//lockframe(111, "hero_attack_0", true, 0, 100, 1, 150);
		//shakecamera2(200, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};
	
	oninterrupt()
	{
		stopeffect(300);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};

skill(160002)
{
	section(450)
	{
		addbreaksection(1, 420, 1050);
		addbreaksection(10, 370, 1050);
		addbreaksection(20, 0, 1050);
		addbreaksection(30, 0, 50);
		addbreaksection(30, 130, 1050);
		movecontrol(true);
		animation("hero_attack_1");
		
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(166, true, 0.05, 0, 0, 8, 0, 0, 80, 0.05, 0, 0, 12, 0, 0, -160);
		
		//playsound(300, "skill0201", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/hero_attack_1", false);
		//playsound(250, "skill0202", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/ZK_Voice_PuGong_02", false)
		//{
		//	audiogroup("Sound/zhankuang/ZK_Voice_PuGong_02", "Sound/zhankuang/ZK_Voice_PuGong_06");
		//}; 
		charactereffect("Hero_FX/5_zhankuang/5_hero_hero_attack_1_01", 500, "Bone_Root", 300, false);
    
		areadamage(300, 0, 1.7, 1, 2.3, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000202);
			stateimpact("kDefault", 16000201);
		};
		
		//playsound(310, "hit0002", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		//{
		//	audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		//};
		//lockframe(111, "hero_attack_1", true, 0, 100, 1, 150);
		//shakecamera2(100, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};
	oninterrupt()
	{
		stopeffect(300);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};

skill(160003)
{
	section(800)
	{
		addbreaksection(1, 616, 800);
		addbreaksection(10, 566, 800);
		addbreaksection(20, 0, 800);
		addbreaksection(30, 0, 233);
		addbreaksection(30, 330, 480);
		addbreaksection(30, 530, 800);
		movecontrol(true);
		animation("hero_attack_2");
		
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(100, true, 0.05, 0, 0, 8, 0, 0, 80, 0.05, 0, 0, 12, 0, 0, -160 );
		
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(433, true, 0.05, 0, 0, 8, 0, 0, 80, 0.05, 0, 0, 12, 0, 0, -160 );
		
		//֡1
		setanimspeed(33, "hero_attack_2", 2);

		//֡7
		setanimspeed(133, "hero_attack_2", 1);
		
		//playsound(250, "skill0301", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/hero_attack_2", false);
		
		//playsound(200, "skill0302", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/ZK_Voice_PuGong_03", false)
		//{
		//	audiogroup("Sound/zhankuang/ZK_Voice_PuGong_03", "Sound/zhankuang/ZK_Voice_PuGong_07");
		//}; 
		charactereffect("Hero_FX/5_zhankuang/5_hero_hero_attack_2_01", 500, "Bone_Root", 233, false);
		
		//playsound(550, "skill0303", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/hero_attack_0", false);

		charactereffect("Hero_FX/5_zhankuang/5_hero_hero_attack_2_02", 500, "Bone_Root", 550, false);
    
		areadamage(250, 0, 1.7, 1, 2.3, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000302);
			stateimpact("kDefault", 16000301);
		};
		
		areadamage(550, 0, 1.7, 1, 2.3, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000312);
			stateimpact("kDefault", 16000311);
		};
		
		//playsound(260, "hit00031", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		//{
		//	audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		//};
		
		//playsound(560, "hit00032", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		//{
		//	audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		//};
		//lockframe(111, "hero_attack_2", true, 0, 100, 1, 150);
		//shakecamera2(100, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};
	oninterrupt()
	{
		stopeffect(300);
	};

	onstop()
	{
		stopeffect(300);
	};
};

skill(160004)
{
	section(600)
	{
		addbreaksection(1, 600, 600);
		addbreaksection(10, 600, 600);
		addbreaksection(20, 0, 600);
		addbreaksection(30, 0, 200);
		addbreaksection(30, 400, 600);
		movecontrol(true);
		animation("hero_attack_0");
		
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(0, true, 0.1, 0, 0, 0, 0, 0, 80, 0.13, 0, 0, 8, 0, 0, -40, 0.05, 0, 0, 2, 0, 0, 320, 0.05, 0, 0, 10, 0, 0, -160);
		
		//֡1
		setanimspeed(33, "hero_attack_0", 1.5);
		
		//֡4
		setanimspeed(100, "hero_attack_0", 0.5);

		//֡5
		setanimspeed(166, "hero_attack_0", 1);
		
		//֡8
		setanimspeed(266, "hero_attack_0", 2);

		//֡12
		setanimspeed(343, "hero_attack_0", 1);
		
		//playsound(300, "skill0401", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/zhankuang_pugong_05", false);
		
		//playsound(250, "skill0402", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/ZK_Voice_PuGong_04", false)
		//{
		//	audiogroup("Sound/zhankuang/ZK_Voice_PuGong_04", "Sound/zhankuang/ZK_Voice_PuGong_08");
		//}; 
		charactereffect("Hero_FX/5_zhankuang/5_hero_hero_attack_0_01", 500, "Bone_Root", 300, false);
    
		areadamage(300, 0, 1.7, 1, 2.5, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000402);
			stateimpact("kDefault", 16000401);
		};

		//playsound(310, "hit0004", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		//{
		//	audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		//};
		//lockframe(111, "hero_attack_1", true, 0, 100, 1, 150);
		//shakecamera2(370, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};

	oninterrupt()
	{
		stopeffect(300);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};