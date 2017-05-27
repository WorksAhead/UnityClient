//普攻二连
skill(380901)
{
	section(2233)
	{
		movecontrol(true);
		animation("Attack_01");
		findmovetarget(0, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
        ////addimpacttoself(0, 88896, 1700);
        playsound(510, "skill0101", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Npc/RA/boss_RA_pugong_01", false);	
		playsound(1110, "skill0102", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Npc/RA/boss_RA_pugong_02", false);	
		areadamage(566, 0, 1, 2, 3.5, true) 
		{
			stateimpact("kLauncher", 38090102);
			stateimpact("kKnockDown", 38090103);
			stateimpact("kDefault", 38090101);
		};
		areadamage(1166, 0, 1, 2, 3.5, true) 
		{
			stateimpact("kLauncher", 38090112);
			stateimpact("kKnockDown", 38090113);
			stateimpact("kDefault", 38090111);
		};
		charactereffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_DaoGuang_01", 1000, "Bone_Root", 566, false);
		charactereffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_DaoGuang_02", 1000, "Bone_Root", 1166, false);
	};
	oninterrupt()
	{
		stopeffect(0);
	};
	onstop()
	{
		stopeffect(0);
	};
};

//变灯：红
skill(380902)
{
	section(900)
	{
		movecontrol(true);
		findmovetarget(0, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		animation("Skill_01A");
        playsound(510, "skill0101", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Npc/RA/boss_RA_pugong_01", false);	
	};
	section(400)
	{
		animation("Skill_01B");
	};
	section(1000)
	{
		animation("Skill_01C")
		{
			 wrapmode(2);
		};
		charactereffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_BianDeng_01", 4000, "ef_weapon", 0, false);
		charactereffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_BianDeng_02", 2000, "Bone_Root", 0, false);
        playsound(0, "skill0201", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_01", false);	
	};
	section(1333)
	{
		animation("Skill_01D");
		summonnpc(1000, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_YanGuan_01", 380912, vector3(0, 0, 0));
        playsound(0, "skill0202", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_02", false);	
        playsound(1000, "skill0203", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_03", false);	
		movechild(1000, "6_Mon_RA_Weapon_02", "ef_weapon");
		movechild(1000, "6_Mon_RA_Weapon_03", "ef_low");
		movechild(1000, "6_Mon_RA_Weapon_01", "ef_low");
		movechild(1000, "6_Mon_RA_Red_01", "Bone_Root");
		movechild(1000, "6_Mon_RA_Blue_01", "ef_low");
		movechild(1000, "6_Mon_RA_Yellow_01", "ef_low");
	};
	section(600)
	{
		animation("Skill_01E");
	};
	oninterrupt()
	{
	};
	onstop()
	{
	};
};

//变灯：蓝
skill(380903)
{
	section(900)
	{
		movecontrol(true);
		findmovetarget(0, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		animation("Skill_01A");
	};
	section(400)
	{
		animation("Skill_01B");
	};
	section(1000)
	{
		animation("Skill_01C")
		{
			 wrapmode(2);
		};
		charactereffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_BianDeng_01", 4000, "ef_weapon", 0, false);
		charactereffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_BianDeng_02", 2000, "Bone_Root", 0, false);
        playsound(0, "skill0301", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_01", false);	
	};
	section(1333)
	{
		animation("Skill_01D");
		summonnpc(1000, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_BingGuan_01", 380913, vector3(0, 0, 0));
        playsound(0, "skill0302", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_02", false);	
        playsound(1000, "skill0303", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_04", false);	
		movechild(1000, "6_Mon_RA_Weapon_03", "ef_weapon");
		movechild(1000, "6_Mon_RA_Weapon_02", "ef_low");
		movechild(1000, "6_Mon_RA_Weapon_01", "ef_low");
		movechild(1000, "6_Mon_RA_Blue_01", "Bone_Root");
		movechild(1000, "6_Mon_RA_Red_01", "ef_low");
		movechild(1000, "6_Mon_RA_Yellow_01", "ef_low");
	};
	section(600)
	{
		animation("Skill_01E");
	};
	oninterrupt()
	{
	};
	onstop()
	{
	};
};

//冲击波
skill(380904)
{
	section(2900)
	{
		movecontrol(true);
		animation("Skill_02");
		findmovetarget(0, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		//19
		setanimspeed(633, "Skill_02", 0.2, false);
		//25
		setanimspeed(1633, "Skill_02", 1, false);
		//63
        playsound(1000, "skill0401", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_jiguang_01", false);	
		summonnpc(633, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_ChongJiBo_01", 380914, vector3(0, 0, 0));
		summonnpc(633, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_ChongJiBo_01", 380915, vector3(0, 0, 0));
		summonnpc(633, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_ChongJiBo_01", 380916, vector3(0, 0, 0));
		summonnpc(633, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_ChongJiBo_01", 380917, vector3(0, 0, 0));
	};
	oninterrupt()
	{
		stopeffect(0);
		setenable(0, "Damage", false);
	};
	onstop()
	{
	};
};

//波浪
skill(380905)
{
	section(2800)
	{
		movecontrol(true);
		animation("Skill_02");
		//19
		setanimspeed(633, "Skill_02", 0.2, false);
		//25
		setanimspeed(1633, "Skill_02", 1, false);
		//63
		findmovetarget(0, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		summonnpc(900, 101, "Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_BoLang_01", 380906, vector3(0, 0, 0));
        playsound(1300, "skill0501", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_bolang_01", false);
	};
	oninterrupt()
	{
	};
	onstop()
	{
	};
};

//召唤物：大浪
skill(380906)
{
	section(3000)
	{
		movecontrol(true);
		settransform(0," ",vector3(0,0,-3),eular(0,0,0),"RelativeOwner",false);
		startcurvemove(0, false, 3, 0, 0, 5, 0, 0, 0);
		colliderdamage(0, 3000, true, true, 400, 1)
		{
			stateimpact("kDefault", 38090601);
			sceneboxcollider(vector3(7,4,5), vector3(0,0,0), eular(0,0,0), true, false);
		};
		destroyself(3000);
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

//陨石
skill(380907)
{
	section(2800)
	{
		movecontrol(true);
		animation("Skill_02");
		//19
		setanimspeed(633, "Skill_02", 0.2, false);
		//25
		setanimspeed(1633, "Skill_02", 1, false);
		//63
		findmovetarget(0, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		summonnpc(900, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_YunShi_01", 380908, vector3(0, 0, 0));
        playsound(1300, "skill0701", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_huoqiu_01", false);
	};
	oninterrupt()
	{
	};
	onstop()
	{
	};
};

//召唤物：陨石
skill(380908)
{
	section(1050)
	{
		movecontrol(true);
		settransform(0," ",vector3(0,20,-13),eular(0,0,0),"RelativeOwner",false);
		startcurvemove(0, false, 1, 0, -20.8, 20, 0, 0, 0);
		areadamage(1000, 0, 0.5, 0, 4, false) 
		{
			stateimpact("kDefault", 38090801);
		};
		sceneeffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_YunShi_02",4000,vector3(0,0,0),1000);
		destroyself(1020);
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

//阳光普照
skill(380909)
{
	section(5100)
	{
		movecontrol(true);
		animation("Skill_03");
		//22
		setanimspeed(733, "Skill_03", 0.25, false);
		//43
		setanimspeed(3533, "Skill_03", 1, false);
		//91
        playsound(733, "skill0901", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_01", false);
        playsound(1500, "skill0902", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/Npc/RA/boss_RA_yangguangpuzhao_01", false);
		sceneeffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_YangGuangPuZhao_01",5000,vector3(0,4,0),800);
		summonnpc(1000, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_YangGuangPuZhao_02", 380918, vector3(0, 0, 0));
		summonnpc(1000, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_YangGuangPuZhao_02", 380919, vector3(0, 0, 0));
		summonnpc(1000, 101, "Monster_FX/RA_TaiYangShen/6_Mon_RA_YangGuangPuZhao_02", 380920, vector3(0, 0, 0));
	};
	oninterrupt()
	{
	};
	onstop()
	{
	};
};

//如沐甘霖
skill(380910)
{
	section(5100)
	{
		movecontrol(true);
		animation("Skill_03");
		//22
		setanimspeed(733, "Skill_03", 0.25, false);
		//43
		setanimspeed(3533, "Skill_03", 1, false);
		//91
        playsound(733, "skill1001", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_01", false);
        playsound(1500, "skill1002", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/Npc/RA/boss_RA_rumuganlin_01", false);
		areadamage(800, 0, 0, 0, 10, true) 
		{
			stateimpact("kDefault", 38091002);
		};
		addimpacttoself(4000, 38091001);
		sceneeffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_RuMuGanLin_01",5000,vector3(0,4,0),800);
		sceneeffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_RuMuGanLin_02",5000,vector3(0,6,0),1500);
	};
	oninterrupt()
	{
		movechild(0, "6_Mon_RA_Weapon_01", "ef_weapon");
		movechild(0, "6_Mon_RA_Weapon_02", "ef_low");
		movechild(0, "6_Mon_RA_Weapon_03", "ef_low");
		movechild(0, "6_Mon_RA_Yellow_01", "Bone_Root");
		movechild(0, "6_Mon_RA_Red_01", "ef_low");
		movechild(0, "6_Mon_RA_Blue_01", "ef_low");
	};
	onstop()
	{
		movechild(0, "6_Mon_RA_Weapon_01", "ef_weapon");
		movechild(0, "6_Mon_RA_Weapon_02", "ef_low");
		movechild(0, "6_Mon_RA_Weapon_03", "ef_low");
		movechild(0, "6_Mon_RA_Yellow_01", "Bone_Root");
		movechild(0, "6_Mon_RA_Red_01", "ef_low");
		movechild(0, "6_Mon_RA_Blue_01", "ef_low");
	};
};

//十字炼狱
skill(380911)
{
	section(5100)
	{
		movecontrol(true);
		animation("Skill_03");
		//22
		setanimspeed(733, "Skill_03", 0.25, false);
		//43
		setanimspeed(3533, "Skill_03", 1, false);
		//91
        playsound(733, "skill1101", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RA/boss_RA_biandeng_01", false);
        playsound(1500, "skill1102", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/Npc/RA/boss_RA_shizilianyu_01", false);
		colliderdamage(1500, 4000, true, true, 100, 40)
		{
			stateimpact("kDefault", 38091101);
			sceneboxcollider(vector3(20,3,3), vector3(0,0,0), eular(0,45,0), false, false);
		};
		colliderdamage(1500, 4000, true, true, 100, 40)
		{
			stateimpact("kDefault", 38091101);
			sceneboxcollider(vector3(20,3,3), vector3(0,0,0), eular(0,-45,0), false, false);
		};
		sceneeffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_ShiZiLianYu_01",5000,vector3(0,8,0),800);
		charactereffect("Monster_FX/RA_TaiYangShen/6_Mon_RA_ShiZiLianYu_02", 5000, "Bone_Root", 1500, false);
	};
	oninterrupt()
	{
		movechild(0, "6_Mon_RA_Weapon_01", "ef_weapon");
		movechild(0, "6_Mon_RA_Weapon_02", "ef_low");
		movechild(0, "6_Mon_RA_Weapon_03", "ef_low");
		movechild(0, "6_Mon_RA_Yellow_01", "Bone_Root");
		movechild(0, "6_Mon_RA_Red_01", "ef_low");
		movechild(0, "6_Mon_RA_Blue_01", "ef_low");
	};
	onstop()
	{
		movechild(0, "6_Mon_RA_Weapon_01", "ef_weapon");
		movechild(0, "6_Mon_RA_Weapon_02", "ef_low");
		movechild(0, "6_Mon_RA_Weapon_03", "ef_low");
		movechild(0, "6_Mon_RA_Yellow_01", "Bone_Root");
		movechild(0, "6_Mon_RA_Red_01", "ef_low");
		movechild(0, "6_Mon_RA_Blue_01", "ef_low");
	};
};

//召唤物：炎棺
skill(380912)
{
	section(3200)
	{
		movecontrol(true);
		settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
		//startcurvemove(0, false, 5, 0, 0, 5, 0, 0, 0);
		colliderdamage(0, 100, true, true, 100, 1)
		{
			stateimpact("kDefault", 38090202);
			sceneboxcollider(vector3(3,3,7), vector3(-2.5,0,0), eular(0,0,0), true, false);
		};
		colliderdamage(200, 3000, true, true, 200, 15)
		{
			stateimpact("kDefault", 38090201);
			sceneboxcollider(vector3(3,3,7), vector3(-2.5,0,0), eular(0,0,0), true, false);
		};
		destroyself(3200);
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

//召唤物：冰棺
skill(380913)
{
	section(3800)
	{
		movecontrol(true);
		settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
		//startcurvemove(0, false, 5, 0, 0, 5, 0, 0, 0);
		colliderdamage(0, 100, true, true, 100, 1)
		{
			stateimpact("kDefault", 38090301);
			sceneboxcollider(vector3(3,3,7), vector3(2.5,0,0), eular(0,0,0), true, false);
		};
		colliderdamage(200, 3800, true, true, 400, 10)
		{
			stateimpact("kDefault", 38090302);
			sceneboxcollider(vector3(3,3,7), vector3(2.5,0,0), eular(0,0,0), true, false);
		};
		destroyself(3800);
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

//召唤物：冲击波1
skill(380914)
{
	section(2000)
	{
		movecontrol(true);
		settransform(0," ",vector3(-4,1.5,0),eular(0,-90,0),"RelativeOwner",false);
		//startcurvemove(500, false, 1.5, 2.66, 4.6, 0, 0, 0, 0);
		rotate(0, 2000, vector3(0, 75, 0));
		colliderdamage(0, 2000, true, true, 400, 1)
		{
			stateimpact("kLauncher", 38090402);
			stateimpact("kKnockDown", 38090403);
			stateimpact("kDefault", 38090401);
			sceneboxcollider(vector3(1,1,15), vector3(0,0,7.5), eular(0,0,0), true, false);
		};
		destroyself(2000);
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

//召唤物：冲击波2
skill(380915)
{
	section(2000)
	{
		movecontrol(true);
		settransform(0," ",vector3(4,1.5,0),eular(0,90,0),"RelativeOwner",false);
		//startcurvemove(500, false, 1.5, -5.33, 0, 0, 0, 0, 0);
		rotate(0, 2000, vector3(0, -75, 0));
		colliderdamage(0, 2000, true, true, 400, 1)
		{
			stateimpact("kLauncher", 38090402);
			stateimpact("kKnockDown", 38090403);
			stateimpact("kDefault", 38090401);
			sceneboxcollider(vector3(1,1,15), vector3(0,0,7.5), eular(0,0,0), true, false);
		};
		destroyself(2000);
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

//召唤物：冲击波3
skill(380916)
{
	section(2000)
	{
		movecontrol(true);
		settransform(0," ",vector3(-4,1.5,0),eular(0,-90,0),"RelativeOwner",false);
		//startcurvemove(500, false, 1.5, 2.66, 4.6, 0, 0, 0, 0);
		rotate(0, 2000, vector3(0, -75, 0));
		colliderdamage(0, 2000, true, true, 400, 1)
		{
			stateimpact("kLauncher", 38090402);
			stateimpact("kKnockDown", 38090403);
			stateimpact("kDefault", 38090401);
			sceneboxcollider(vector3(1,1,15), vector3(0,0,7.5), eular(0,0,0), true, false);
		};
		destroyself(2000);
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

//召唤物：冲击波4
skill(380917)
{
	section(2000)
	{
		movecontrol(true);
		settransform(0," ",vector3(4,1.5,0),eular(0,90,0),"RelativeOwner",false);
		//startcurvemove(500, false, 1.5, -5.33, 0, 0, 0, 0, 0);
		rotate(0, 2000, vector3(0, 75, 0));
		colliderdamage(0, 2000, true, true, 400, 1)
		{
			stateimpact("kLauncher", 38090402);
			stateimpact("kKnockDown", 38090403);
			stateimpact("kDefault", 38090401);
			sceneboxcollider(vector3(1,1,15), vector3(0,0,7.5), eular(0,0,0), true, false);
		};
		destroyself(2000);
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

//召唤物：阳光普照1
skill(380918)
{
	section(3000)
	{
		movecontrol(true);
		settransform(0," ",vector3(0,0,6),eular(0,0,0),"RelativeOwner",false);
		startcurvemove(0, false, 2, 6, 0, 0, -3, 0, -3, 0.66, 0, 0, -6, -3, 0, 3);
		colliderdamage(0, 2000, true, true, 100, 1)
		{
			stateimpact("kDefault", 38090901);
			sceneboxcollider(vector3(2.4,10,2.4), vector3(0,0,0), eular(0,0,0), true, false);
		};
		destroyself(3000);
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

//召唤物：阳光普照2
skill(380919)
{
	section(3000)
	{
		movecontrol(true);
		settransform(0," ",vector3(5.2,0,-3),eular(0,0,0),"RelativeOwner",false);
		startcurvemove(0, false, 1.33, -2, 0, -4, -3, 0, 3, 1.33, -6, 0, 0, 3, 0, 3);
		colliderdamage(0, 2000, true, true, 100, 1)
		{
			stateimpact("kDefault", 38090901);
			sceneboxcollider(vector3(2.4,10,2.4), vector3(0,0,0), eular(0,0,0), true, false);
		};
		destroyself(3000);
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

//召唤物：阳光普照3
skill(380920)
{
	section(3000)
	{
		movecontrol(true);
		settransform(0," ",vector3(-5.2,0,-3),eular(0,0,0),"RelativeOwner",false);
		startcurvemove(0, false, 0.66, -2, 0, 4, 3, 0, 3, 2, 0, 0, 6, 3, 0, -3);
		colliderdamage(0, 2000, true, true, 100, 1)
		{
			stateimpact("kDefault", 38090901);
			sceneboxcollider(vector3(2.4,10,2.4), vector3(0,0,0), eular(0,0,0), true, false);
		};
		destroyself(3000);
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