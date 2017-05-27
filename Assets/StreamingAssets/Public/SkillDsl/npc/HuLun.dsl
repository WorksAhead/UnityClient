
//连击
skill(380301)
{
	section(2100)
	{
		addimpacttoself(0, 38030199);
		movecontrol(true);
		animation("Attack_01A");

		//֡6
		setanimspeed(200, "Attack_01A", 0.25);

		//֡10
		setanimspeed(733, "Attack_01A", 1);

		//֡17
		setanimspeed(966, "Attack_01A", 0.25);

		//֡21
		setanimspeed(1500, "Attack_01A", 1);
		//֡39

        playsound(810, "skill0101", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
		startcurvemove(733, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_01", 700, "Bone010", 0, false);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_01", 1000, "Bone010", 866, false);
    
		areadamage(866, 0, 1, 2, 3, true) 
		{
			stateimpact("kLauncher", 38030102);
			stateimpact("kKnockDown", 38030103);
			stateimpact("kDefault", 38030101);
		};
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//2连击
skill(380302)
{
	section(2700)
	{
		addimpacttoself(0, 38030299);
		movecontrol(true);
		animation("Attack_01B");

		//֡6
		setanimspeed(200, "Attack_01B", 0.25);

		//֡10
		setanimspeed(733, "Attack_01B", 1);

		//֡19
		setanimspeed(1033, "Attack_01B", 0.25);

		//֡23
		setanimspeed(1566, "Attack_01B", 1);

		//֡32
		setanimspeed(1866, "Attack_01B", 0.25);

		//֡36
		setanimspeed(2400, "Attack_01B", 1);
		//֡45

		startcurvemove(733, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);

		startcurvemove(1700, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);

		areadamage(833, 0, 1, 2, 3, true) 
		{
			stateimpact("kLauncher", 38030202);
			stateimpact("kKnockDown", 38030203);
			stateimpact("kDefault", 38030201);
		};
		
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_01", 700, "Bone010", 0, false);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_01", 1000, "Bone010", 800, false);

		areadamage(1800, 0, 1, 2, 3, true) 
		{
			stateimpact("kLauncher", 38030212);
			stateimpact("kKnockDown", 38030213);
			stateimpact("kDefault", 38030211);
		};
		
        playsound(760, "skill0201", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
        playsound(1750, "skill0202", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_02", 1000, "Bone010", 1766, false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//3连击
skill(380303)
{
	section(4300)
	{
		addimpacttoself(0, 38030399);
		movecontrol(true);
		animation("Attack_01C");

		//֡6
		setanimspeed(200, "Attack_01C", 0.25);

		//֡10
		setanimspeed(733, "Attack_01C", 1);

		//֡19
		setanimspeed(1033, "Attack_01C", 0.25);

		//֡23
		setanimspeed(1566, "Attack_01C", 1);

		//֡32
		setanimspeed(1866, "Attack_01C", 0.25);

		//֡36
		setanimspeed(2400, "Attack_01C", 1);

		//֡48
		setanimspeed(2800, "Attack_01C", 0.25);

		//֡52
		setanimspeed(3333, "Attack_01C", 1);
		//֡81

		startcurvemove(733, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);

		startcurvemove(1700, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);
		
		
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_01", 700, "Bone010", 0, false);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_01", 1000, "Bone010", 800, false);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_02", 1000, "Bone010", 1766, false);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_03", 1000, "Bone010", 3433, false);

		areadamage(833, 0, 1, 2, 3, true) 
		{
			stateimpact("kLauncher", 38030302);
			stateimpact("kKnockDown", 38030303);
			stateimpact("kDefault", 38030301);
		};

		areadamage(1800, 0, 1, 2, 3, true) 
		{
			stateimpact("kLauncher", 38030312);
			stateimpact("kKnockDown", 38030313);
			stateimpact("kDefault", 38030311);
		};
		
		areadamage(3500, 0, 1, 2, 3, true) 
		{
			stateimpact("kKnockDown", 38030322);
			stateimpact("kDefault", 38030321);
		};
        playsound(760, "skill0301", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
        playsound(1750, "skill0302", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
        playsound(3450, "skill0303", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_05", false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//挣扎
skill(380304)
{
	section(8633)
	{
		addimpacttoself(0, 38030499);
		addimpacttoself(0, 38030498);
		movecontrol(true);
		animation("Struggle");

		//֡9
		setanimspeed(300, "Struggle", 0.5);

		//֡85
		setanimspeed(5366, "Struggle", 1);

		//֡158
		setanimspeed(7800, "Struggle", 2.5);

		//֡168
		setanimspeed(7933, "Struggle", 0.25);

		//֡171
		setanimspeed(8333, "Struggle", 1);
		//֡180

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Struggle_01", 5000, "Bone010", 3300, false);

		areadamage(7933, 0, 0, 0, 8, true) 
		{
			stateimpact("kKnockDown", 38030402);
			stateimpact("kDefault", 38030401);
		};
		
        playsound(7800, "skill0401", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_huoyanbaopo_explosion", false);
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_02", 2200, "Bone010", 5300, false);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Struggle_02", 1000, "Bone010", 7900, false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//怒吼
skill(380305)
{
	section(2433)
	{
		movecontrol(true);
		animation("Roar");
		
		addimpacttoself(0, 38030599);
		//֡14
		setanimspeed(466, "Roar", 0.25);

		//֡18
		setanimspeed(1000, "Roar", 3);

		//֡30
		setanimspeed(1133, "Roar", 1);
		//֡69

        playsound(970, "skill0501", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/YiNa/boss_YiNa_TuCi_01", false);
		areadamage(1033, 0, 0, 0, 4, true) 
		{
			stateimpact("kDefault", 38030501);
		};
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Roar_01", 2000, "Bone010", 1000);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//龙卷风
skill(380306)
{
	section(1866)
	{
		addimpacttoself(0, 38030699);
		//全局参数
		movecontrol(true);
		animation("Skill_01A");
		
		//֡8
		setanimspeed(233, "Skill_01A", 0.25);

		//֡18
		setanimspeed(1566, "Skill_01A", 1);
		//֡27

        playsound(1500, "skill0601", "Sound/zhankuang/zhankuang_sound", 5000, "Sound/zhankuang/zhankuang_xuanfengzhan_02", false);
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Whirlwind_01", 5000, "Bone010", 1700);
	};
	section(4000)
	{
		movecontrol(false);
		enablechangedir(0, 4000);
		animation("Skill_01B")
		{
			wrapmode(2);
		};

		colliderdamage(0, 4000, true, true, 0, 10)
		{
			stateimpact("kDefault", 38030601);
			bonecollider("Monster/Boss/dafengchecollider","Bone010", true);
		};

	};
	section(900)
	{
		movecontrol(true);
		animation("Skill_01C");
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//怒劈
skill(380307)
{
	section(3433)
	{
		addimpacttoself(0, 38030799);
		movecontrol(true);
		animation("Skill_02");
        playsound(2500, "skill0701", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_02", false);
        playsound(2600, "skill0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_01", false);
        playsound(2700, "skill0703", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_02", false);
        playsound(2800, "skill0704", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_03", false);
		
		areadamage(2600, 0, 1, 3, 4, true) 
		{
			stateimpact("kKnockDown", 38030702);
			stateimpact("kDefault", 38030701);
		};

		areadamage(2700, 0, 1, 6, 4, true) 
		{
			stateimpact("kKnockDown", 38030702);
			stateimpact("kDefault", 38030701);
		};

		areadamage(2800, 0, 1, 9, 4, true) 
		{
			stateimpact("kKnockDown", 38030702);
			stateimpact("kDefault", 38030701);
		};
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Sword_01", 1000, "5_IP_HuLun_01_w_01", 1800);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_04",2200,vector3(0,0,3),0,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_04",2200,vector3(0,0,6),0,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_04",2200,vector3(0,0,9),0,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_02",2000,vector3(0,0,4.5),2633,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_02",2000,vector3(0,0,6),2733,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_02",2000,vector3(0,0,9),2833,eular(0,0,0),vector3(1,1,1),true);
	
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Split_01", 1000, "Bone010", 2466);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//跳斩
skill(380308)
{
	section(3933)
	{
		addimpacttoself(0, 38030899);
		movecontrol(true);
		animation("Skill_03");

		findmovetarget(3000, vector3(0, 0, 0), 8, 60, 0.8, 0.2, 0, 0);
		startcurvemove(3100, true, 0.15, 0, 40, 30, 0, -200, 0, 0.05, 0, -100, 20, 0, -200, -300);

		//֡10
		setanimspeed(333, "Skill_03", 0.25);

		//֡28
		setanimspeed(2733, "Skill_03", 2);

		//֡34
		setanimspeed(2833, "Skill_03", 0.5);

		//֡38
		setanimspeed(3100, "Skill_03", 4);

		//֡62
		setanimspeed(3300, "Skill_03", 1);
		//֡81

		areadamage(3300, 0, 1, 1, 3, true) 
		{
			stateimpact("kKnockDown", 38030802);
			stateimpact("kDefault", 38030801);
		};
		
        playsound(3100, "skill0801", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_huoyanzhuaqu_crush", false);
		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_05",2000,vector3(0,0,0),1000,eular(0,0,0),vector3(1,1,1),true);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Sword_01", 1500, "5_IP_HuLun_01_w_01", 2000);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Smash_01", 2000, "Bone010", 3300, false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};




//连击
skill(380311)
{
	section(600)
	{
		movecontrol(true);
		animation("Attack_01A");

		//֡10
		setanimspeed(333, "Attack_01A", 3);

		//֡16
		setanimspeed(400, "Attack_01A", 1);
		//֡39

		startcurvemove(333, true, 0.05, 0, 0, 10, 0, 0, 400, 0.05, 0, 0, 30, 0, 0, -560);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_01", 1000, "Bone010", 350, false);
    
		areadamage(370, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kLauncher", 38031102);
			stateimpact("kKnockDown", 38031103);
			stateimpact("kDefault", 38031101);
		};

        playsound(310, "skill0101", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//2连击
skill(380312)
{
	section(900)
	{
		movecontrol(true);
		animation("Attack_01B");

		//֡10
		setanimspeed(333, "Attack_01B", 3);

		//֡19
		setanimspeed(433, "Attack_01B", 1);

		//֡23
		setanimspeed(566, "Attack_01B", 3);

		//֡32
		setanimspeed(666, "Attack_01B", 1);
		//֡45

		startcurvemove(333, true, 0.05, 0, 0, 10, 0, 0, 400, 0.05, 0, 0, 30, 0, 0, -560);

		startcurvemove(583, true, 0.05, 0, 0, 10, 0, 0, 400, 0.05, 0, 0, 30, 0, 0, -560);

		areadamage(283, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kLauncher", 38031202);
			stateimpact("kKnockDown", 38031203);
			stateimpact("kDefault", 38031201);
		};

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_01", 1000, "Bone010", 353, true);

		areadamage(613, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kLauncher", 38031212);
			stateimpact("kKnockDown", 38031213);
			stateimpact("kDefault", 38031211);
		};
		
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_02", 1000, "Bone010", 583, true);
        playsound(230, "skill0201", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
        playsound(563, "skill0202", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//3连击
skill(380313)
{
	section(1600)
	{
		movecontrol(true);
		animation("Attack_01C");

		//֡10
		setanimspeed(333, "Attack_01C", 3);

		//֡19
		setanimspeed(433, "Attack_01C", 1);

		//֡23
		setanimspeed(566, "Attack_01C", 3);

		//֡32
		setanimspeed(666, "Attack_01C", 1);

		//֡52
		setanimspeed(1333, "Attack_01C", 3);

		//֡58
		setanimspeed(1400, "Attack_01C", 1);
		//֡81

		startcurvemove(333, true, 0.05, 0, 0, 10, 0, 0, 400, 0.05, 0, 0, 30, 0, 0, -560);

		startcurvemove(483, true, 0.05, 0, 0, 10, 0, 0, 400, 0.05, 0, 0, 30, 0, 0, -560);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_01", 1000, "Bone010", 353, true);
		
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_02", 1000, "Bone010", 583, true);

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_03", 1000, "Bone010", 1350, true);

		areadamage(383, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kLauncher", 38031302);
			stateimpact("kKnockDown", 38031303);
			stateimpact("kDefault", 38031301);
		};

		areadamage(610, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kLauncher", 38031312);
			stateimpact("kKnockDown", 38031313);
			stateimpact("kDefault", 38031311);
		};
		
		areadamage(1380, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031322);
			stateimpact("kDefault", 38031321);
		};
        playsound(333, "skill0301", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
        playsound(560, "skill0302", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
        playsound(1330, "skill0303", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_05", false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//龙卷风
skill(380316)
{
	section(700)
	{
		//全局参数
		movecontrol(true);
		animation("Skill_01A");
		
		//֡1
		setanimspeed(33, "Skill_01A", 3);

		//֡10
		setanimspeed(133, "Skill_01A", 1);

		//֡18
		setanimspeed(400, "Skill_01A", 1);
		//֡27

        playsound(533, "skill0601", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_xuanfengzhan_02", false);
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Whirlwind_02", 2000, "Bone010", 533);
	};
	section(1500)
	{
		movecontrol(true);
		startcurvemove(0, true, 0.75, 0, 0, 10, 0, 0, 0, 0.75, 0, 0, -10, 0, 0, 0);

		animation("Skill_01B")
		{
			wrapmode(2);
		};

		colliderdamage(0, 1500, true, true, 100, 15)
		{
			stateimpact("kDefault", 38031601);
			bonecollider("Monster/Boss/dafengchecollider","Bone010", true);
		};

	};
	section(500)
	{
		movecontrol(true);
		animation("Skill_01C");
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//怒劈
skill(380317)
{
	section(1400)
	{
		movecontrol(true);
		animation("Skill_02");
		
		//֡1
		setanimspeed(33, "Skill_02", 3);

		//֡79
		setanimspeed(900, "Skill_02", 1);
		//֡102

		areadamage(900, 0, 1, 2.5, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		areadamage(1000, 0, 1, 4, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		areadamage(1200, 0, 1, 6, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(0,0,2.5),900,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(0,0,4),1000,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(0,0,6),1200,eular(0,0,0),vector3(1,1,1),true);
	
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Split_01", 1000, "Bone010", 866);
        playsound(800, "skill0701", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_02", false);
        playsound(900, "skill0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_01", false);
        playsound(1000, "skill0703", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_02", false);
        playsound(1200, "skill0704", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_03", false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//跳斩
skill(380318)
{
	section(1000)
	{
		movecontrol(true);
		animation("Skill_03");

		findmovetarget(700, vector3(0, 0, 0), 8, 60, 0.8, 0.2, 0, 0);
		startcurvemove(733, true, 0.15, 0, 40, 30, 0, -200, 0, 0.05, 0, -100, 20, 0, -200, -300);
		
		//֡1
		setanimspeed(33, "Skill_03", 2);

		//֡33
		setanimspeed(566, "Skill_03", 1);

		//֡38
		setanimspeed(733, "Skill_03", 4);

		//֡62
		setanimspeed(933, "Skill_03", 1);
		//֡81

		areadamage(900, 0, 1, 1, 3, true) 
		{
			stateimpact("kKnockDown", 38031802);
			stateimpact("kDefault", 38030801);
		};

        playsound(700, "skill0801", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_huoyanzhuaqu_crush", false);
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Smash_02", 2000, "Bone010", 933, false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//出场
skill(380321)
{
	section(500)
	{
		addimpacttoself(0, 38032199);
		movecontrol(true);
		animation("PLand")
		{
			wrapmode(2);
		};

		settransform(0, " ", vector3(-10, 1, -10), eular(0, 0, 0), "RelativeSelf", false);
		startcurvemove(0, true, 0.5, 18, 30, 16.5, 0, -120, 0);
	};
	section(1500)
	{
		startcurvemove(0, true, 0.5, 0, -30, 0, 0, 0, 0);

		sceneeffect("Monster_FX/Campaign_Wild/07_OEOrc/6_Mon_OEOrc_ZhenDangBo_01",2500,vector3(0,0,0),100);
		
		movecontrol(true);
		animation("Land");
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//龙卷风:加强
skill(380326)
{
	section(700)
	{
		//全局参数
		movecontrol(true);
		animation("Skill_01A");
		
		//֡1
		setanimspeed(33, "Skill_01A", 3);

		//֡10
		setanimspeed(133, "Skill_01A", 1);

		//֡18
		setanimspeed(400, "Skill_01A", 1);
		//֡27

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Whirlwind_02", 2000, "Bone010", 533);
        playsound(533, "skill0601", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_xuanfengzhan_02", false);
	};
	section(1500)
	{
		movecontrol(true);
		startcurvemove(0, true, 0.375, 7.5, 0, 15, -40, 0, 0, 0.375, -7.5, 0, -15, 40, 0, 0, 0.375, 7.5, 0, 15, -40, 0, 0, 0.375, -7.5, 0, -15, 40, 0, 0);

		animation("Skill_01B")
		{
			wrapmode(2);
		};

		colliderdamage(0, 1500, true, true, 100, 15)
		{
			stateimpact("kDefault", 38031601);
			bonecollider("Monster/Boss/dafengchecollider","Bone010", true);
		};

	};
	section(500)
	{
		movecontrol(true);
		animation("Skill_01C");
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//怒劈：加强
skill(380327)
{
	section(1400)
	{
		movecontrol(true);
		animation("Skill_02");
		
		//֡1
		setanimspeed(33, "Skill_02", 3);

		//֡79
		setanimspeed(900, "Skill_02", 1);
		//֡102

		areadamage(900, 0, 1, 2.5, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		areadamage(1000, -1.5, 1, 4, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		areadamage(1000, 1.5, 1, 4, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		areadamage(1200, -3, 1, 6, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		areadamage(1200, 0, 1, 6, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

		areadamage(1200, 3, 1, 6, 2.2, true) 
		{
			stateimpact("kKnockDown", 38031702);
			stateimpact("kDefault", 38031701);
		};

        playsound(800, "skill0701", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_02", false);
        playsound(900, "skill0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_01", false);
        playsound(1000, "skill0703", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_02", false);
        playsound(1200, "skill0704", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_jiaosha_03", false);
		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(0,0,2.5),900,eular(0,0,0),vector3(1,1,1),true);
		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(-1.5,0,4),1000,eular(0,0,0),vector3(1,1,1),true);
		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(1.5,0,4),1000,eular(0,0,0),vector3(1,1,1),true);
		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(-3,0,6),1200,eular(0,0,0),vector3(1,1,1),true);
		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(0,0,6),1200,eular(0,0,0),vector3(1,1,1),true);
		sceneeffect("Monster_FX/HuLun/6_Mon_HuLun_Split_03",2000,vector3(3,0,6),1200,eular(0,0,0),vector3(1,1,1),true);
	
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Split_01", 1000, "Bone010", 866);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//跳斩：加强
skill(380328)
{
	section(1000)
	{
		movecontrol(true);
		animation("Skill_03");

		findmovetarget(700, vector3(0, 0, 0), 8, 60, 0.8, 0.2, 0, 0);
		startcurvemove(733, true, 0.15, 0, 40, 30, 0, -200, 0, 0.05, 0, -100, 20, 0, -200, -300);
		
		//֡1
		setanimspeed(33, "Skill_03", 2);

		//֡33
		setanimspeed(566, "Skill_03", 1);

		//֡38
		setanimspeed(733, "Skill_03", 4);

		//֡62
		setanimspeed(933, "Skill_03", 1);
		//֡81

		areadamage(900, 0, 1, 1, 3, true) 
		{
			stateimpact("kKnockDown", 38031802);
			stateimpact("kDefault", 38030801);
		};

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Smash_02", 2000, "Bone010", 933, false);
	};
	section(1000)
	{
		movecontrol(true);
		animation("Skill_03");

		findmovetarget(700, vector3(0, 0, 0), 8, 60, 0.8, 0.2, 0, 0);
		startcurvemove(733, true, 0.15, 0, 40, 30, 0, -200, 0, 0.05, 0, -100, 20, 0, -200, -300);
		
		//֡1
		setanimspeed(33, "Skill_03", 2);

		//֡33
		setanimspeed(566, "Skill_03", 1);

		//֡38
		setanimspeed(733, "Skill_03", 4);

		//֡62
		setanimspeed(933, "Skill_03", 1);
		//֡81

		areadamage(900, 0, 1, 1, 3, true) 
		{
			stateimpact("kKnockDown", 38031802);
			stateimpact("kDefault", 38030801);
		};

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Smash_02", 2000, "Bone010", 933, false);
        playsound(700, "skill0801", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_huoyanzhuaqu_crush", false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

//剧情用2连击——1022
skill(380392)
{
	section(900)
	{
		movecontrol(true);
		animation("Attack_01B");

		//֡10
		setanimspeed(333, "Attack_01B", 3);

		//֡19
		setanimspeed(433, "Attack_01B", 1);

		//֡23
		setanimspeed(566, "Attack_01B", 3);

		//֡32
		setanimspeed(666, "Attack_01B", 1);
		//֡45

		startcurvemove(333, true, 0.05, 0, 0, 10, 0, 0, 400, 0.05, 0, 0, 30, 0, 0, -560);

		startcurvemove(583, true, 0.05, 0, 0, 10, 0, 0, 400, 0.05, 0, 0, 30, 0, 0, -560);

		areadamage(283, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kLauncher", 38031202);
			stateimpact("kKnockDown", 38031203);
			stateimpact("kDefault", 38039201);
		};

		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_01", 1000, "Bone010", 353, true);

		areadamage(613, 0, 1.5, 1, 2.2, true) 
		{
			stateimpact("kLauncher", 38031212);
			stateimpact("kKnockDown", 38031213);
			stateimpact("kDefault", 38039211);
		};
		
		charactereffect("Monster_FX/HuLun/6_Mon_HuLun_Attack_02", 1000, "Bone010", 583, true);
        playsound(233, "skill0201", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
        playsound(563, "skill0202", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};
