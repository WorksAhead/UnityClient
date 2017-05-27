//普攻
skill(380601)
{
	section(1700)
	{
		movecontrol(true);
		animation("Attack_01");
        ////addimpacttoself(0, 88896, 1700);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		startcurvemove(500, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);
        playsound(600, "skill0101", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_pugong_01", false);	
		areadamage(700, 0, 1, 2, 3.2, true) 
		{
			stateimpact("kLauncher", 38060102);
			stateimpact("kKnockDown", 38060103);
			stateimpact("kDefault", 38060101);
		};
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_01", 1000, "Bone_Root", 670, false);
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

//闪身攻击
skill(380602)
{
	section(1833)
	{
		movecontrol(true);
        ////addimpacttoself(0, 88896, 1833);
		animation("Attack_02");
        setenable(0, "Visible", false);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 4);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_XiaoShi_01", 1000, "Bone_Root", 0, false);
		startcurvemove(40, true, 0.05, 0, 0, 5, 0, 0, 0);
		findmovetarget(120, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(180, 20);
        setenable(200, "Visible", true);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_01", 1000, "Bone_Root", 150, false);
		startcurvemove(700, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);
        playsound(0, "skill0201", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_xiaoshi_01", false);	
        playsound(200, "skill0202", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chuxian_01", false);	
        playsound(800, "skill0203", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_pugong_01", false);	
		areadamage(900, 0, 1, 2, 3.2, true) 
		{
			stateimpact("kLauncher", 38060202);
			stateimpact("kKnockDown", 38060203);
			stateimpact("kDefault", 38060201);
		};
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_02", 1000, "Bone_Root", 870, false);
	};
	oninterrupt()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
	onstop()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
};

//四连击
skill(380603)
{
	section(3166)
	{
		//addimpacttoself(0, 38030299);
        //addimpacttoself(0, 88896, 3166);
		movecontrol(true);
		animation("Skill_02");
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		startcurvemove(366, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);
		startcurvemove(933, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);
		startcurvemove(1566, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);
		startcurvemove(1866, true, 0.1, 0, 0, 5, 0, 0, 100, 0.1, 0, 0, 15, 0, 0, -140);
		areadamage(566, 0, 1, 2, 4, true) 
		{
			stateimpact("kLauncher", 38060302);
			stateimpact("kKnockDown", 38060303);
			stateimpact("kDefault", 38060301);
		};
		areadamage(1133, 0, 1, 2, 4, true) 
		{
			stateimpact("kLauncher", 38060312);
			stateimpact("kKnockDown", 38060313);
			stateimpact("kDefault", 38060311);
		};
		areadamage(1766, 0, 1, 2, 4, true) 
		{
			stateimpact("kLauncher", 38060322);
			stateimpact("kKnockDown", 38060323);
			stateimpact("kDefault", 38060321);
		};
		areadamage(2066, 0, 1, 2, 3, true) 
		{
			stateimpact("kLauncher", 38060332);
			stateimpact("kKnockDown", 38060333);
			stateimpact("kDefault", 38060331);
		};
		playsound(500, "skill0301", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_silianji_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_03", 1000, "Bone_Root", 530, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_04", 1000, "Bone_Root", 1100, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_05", 1000, "Bone_Root", 1730, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_06", 1000, "Bone_Root", 2030, false);
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

//闪身1
skill(380604)
{
	section(1000)
	{
		movecontrol(true);
        setenable(0, "Visible", false);
        //addimpacttoself(0, 88896, 1000);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, -4.5);
		startcurvemove(40, true, 0.9, 0, 0, -5, 0, 0, 0);
		findmovetarget(920, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(980, 20);
        setenable(1000, "Visible", true);
		playsound(0, "skill0401", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_xiaoshi_01", false);	
        playsound(950, "skill0402", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chuxian_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_XiaoShi_01", 1000, "Bone_Root", 0, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_01", 1000, "Bone_Root", 950, false);
	};
	oninterrupt()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
	onstop()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
};
//闪身2
skill(380605)
{
	section(1000)
	{
		movecontrol(true);
        setenable(0, "Visible", false);
        //addimpacttoself(0, 88896, 1000);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, -2.5);
		startcurvemove(40, true, 0.9, 0, 0, -5, 0, 0, 0);
		findmovetarget(920, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(980, 20);
        setenable(1000, "Visible", true);
		playsound(0, "skill0501", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_xiaoshi_01", false);	
        playsound(950, "skill0502", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chuxian_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_XiaoShi_01", 1000, "Bone_Root", 0, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_01", 1000, "Bone_Root", 950, false);
	};
	oninterrupt()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
	onstop()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
};
//闪身3
skill(380606)
{
	section(1000)
	{
		movecontrol(true);
        setenable(0, "Visible", false);
        //addimpacttoself(0, 88896, 1000);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 2.5);
		startcurvemove(40, true, 0.9, 0, 0, 5, 0, 0, 0);
		findmovetarget(920, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(980, 20);
        setenable(1000, "Visible", true);
		playsound(0, "skill0601", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_xiaoshi_01", false);	
        playsound(950, "skill0602", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chuxian_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_XiaoShi_01", 1000, "Bone_Root", 0, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_01", 1000, "Bone_Root", 950, false);
	};
	oninterrupt()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
	onstop()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
};
//闪身4
skill(380607)
{
	section(1000)
	{
		movecontrol(true);
        setenable(0, "Visible", false);
        //addimpacttoself(0, 88896, 1000);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 4.5);
		startcurvemove(40, true, 0.9, 0, 0, 5, 0, 0, 0);
		findmovetarget(920, vector3(0, 0, 0), 10, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(980, 20);
        setenable(1000, "Visible", true);
		playsound(0, "skill0701", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_xiaoshi_01", false);	
        playsound(950, "skill0702", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chuxian_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_XiaoShi_01", 1000, "Bone_Root", 0, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_01", 1000, "Bone_Root", 950, false);
	};
	oninterrupt()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
	onstop()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
};
//闪身5
skill(380608)
{
	section(1000)
	{
		movecontrol(true);
        setenable(0, "Visible", false);
        //addimpacttoself(0, 88896, 1000);
		findmovetarget(920, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(980, 20);
        setenable(1000, "Visible", true);
		playsound(0, "skill0801", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_xiaoshi_01", false);	
        playsound(950, "skill0802", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chuxian_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_XiaoShi_01", 1000, "Bone_Root", 0, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_01", 1000, "Bone_Root", 950, false);
	};
	oninterrupt()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
	onstop()
	{
		stopeffect(300);
        setenable(0, "Visible", true);
	};
};

//快速冲刺
skill(380609)
{
	section(1500)
	{
		animation("Skill_01");
        //addimpacttoself(0, 88896, 1500);
		movecontrol(true);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 5, false);
		facetotarget(50, 20);
        charactereffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_01", 1000, "Bone_Root", 0, false);
		startcurvemove(1000, true, 0.05, 0, 0, 0, 0, 0, 1600, 0.1, 0, 0, 80, 0, 0, -300);
		colliderdamage(1000, 150, true, true, 150, 1)
		{
			stateimpact("kLauncher", 38060902);
			stateimpact("kKnockDown", 38060903);
			stateimpact("kDefault", 38060901);
			boneboxcollider(vector3(2, 2, 4), "Bip001", vector3(0, 0, 0), eular(0, 0, 0), true, false);
		};
		playsound(900, "skill0901", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chongci_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChongFeng_01", 500, "ef_weapon", 900, true);
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

//冲刺
skill(380610)
{
	section(2833)
	{
		animation("Skill_01");
        //addimpacttoself(0, 88896, 2833);
		movecontrol(true);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 5, false);
		facetotarget(50, 20);
        charactereffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_01", 1000, "Bone_Root", 0, false);
		startcurvemove(1000, true, 0.05, 0, 0, 0, 0, 0, 1600, 0.1, 0, 0, 80, 0, 0, -300);
		colliderdamage(1000, 150, true, true, 150, 1)
		{
			stateimpact("kLauncher", 38060902);
			stateimpact("kKnockDown", 38060903);
			stateimpact("kDefault", 38060901);
			boneboxcollider(vector3(2, 2, 4), "Bip001", vector3(0, 0, 0), eular(0, 0, 0), true, false);
		};
		playsound(900, "skill1001", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chongci_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChongFeng_01", 500, "ef_weapon", 900, true);
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


     
//大浪
skill(380611)
{
	section(2500)
	{
        //addimpacttoself(0, 88896, 2000);
		findmovetarget(0, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 3, false);
		facetotarget(50, 20);
		animation("Skill_03");
		//֡21
		setanimspeed(700, "Skill_03", 0.2);
		//֡25
		setanimspeed(1366, "Skill_03", 1);
		//֡60
		movecontrol(true);
        charactereffect("Monster_FX/HuLun/6_Mon_HuLun_YuJing_03", 1500, "Bone_Root", 0, false);
		summonnpc(1666, 101, "Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_BoLang_01", 380612, vector3(0, 0, -3));
        playsound(1700, "skill1101", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_bolang_01", false);	
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

//召唤物：大浪
skill(380612)
{
	section(5000)
	{
		movecontrol(true);
		settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
		startcurvemove(0, false, 5, 0, 0, 8, 0, 0, 0);
		colliderdamage(0, 5000, true, true, 400, 10)
		{
			stateimpact("kLauncher", 38061102);
			stateimpact("kKnockDown", 38061103);
			stateimpact("kDefault", 38061101);
			sceneboxcollider(vector3(7,4,5), vector3(0,0,0), eular(0,0,0), true, false);
		};
		destroyself(5000);
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

//四连击
skill(380613)
{
	section(3166)
	{
		//addimpacttoself(0, 38030299);
        //addimpacttoself(0, 88896, 3166);
		movecontrol(true);
		animation("Skill_02");
		playsound(500, "skill0301", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_silianji_01", false);	
        charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_03", 1000, "Bone_Root", 530, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_04", 1000, "Bone_Root", 1100, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_05", 1000, "Bone_Root", 1730, false);
		charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_DaoGuang_06", 1000, "Bone_Root", 2030, false);
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


//下潜
skill(380614)
{
	section(200)
	{
		movecontrol(true);
    setenable(0, "Visible", false);
    //addimpacttoself(0, 88896, 1000);
    playsound(0, "skill0801", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_xiaoshi_01", false);
    charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_XiaoShi_01", 1000, "Bone_Root", 0, false);
	};
  section(6200)
  {
    setlayer(0, 6200, "NoBlockCharacter");
    findmovetarget(30, vector3(0, 0, 0), 30, 360, 0.5, 0.5, 0, 0, false);
    startcurvemove(300, false, 6.1, 0, 0, 2.5, 0, 0, 0.75);
    facetotarget(0, 6200, 180);
    charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_Water_01", 6200, "Bone_Root", 0, true);
    colliderdamage(800, 6100, false, false, 500, 1)
		{
			stateimpact("kLauncher", 38061402);
			stateimpact("kKnockDown", 38061403);
			stateimpact("kDefault", 38061401);
			sceneboxcollider(vector3(2.5,6,2.5), vector3(0,3,0), eular(0,0,0), true, false);
		};
    charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_01", 1000, "Bone_Root", 6190, false);
  };
  section(1700)
  {
    setenable(0, "Visible", true);
    animation("Attack_01");
    setanimspeed(30, "Attack_01", 2.5, true);
    setanimspeed(530, "Attack_01", 1, true);
    playsound(0, "skill0802", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/Npc/RenYuWangZi/boss_RenYuWangZi_chuxian_01", false);
  };
  onmessage("oncollide")
  {
    setenable(0, "Damage", false);
    gotosection(0, 2, 0);
    shakecamera2(0, 360, false, false, vector3(0.2,1.5,0.2), vector3(60,60,60),vector3(0.4,3,0.4),vector3(50,80,50));
    charactereffect("Monster_FX/RenYuWangZi/6_Mon_RenYuWangZi_ChuXian_02", 1000, "Bone_Root", 0, false);
  };
	oninterrupt()
	{
		stopeffect(300);
    setenable(0, "Visible", true);
	};
  onstop()
	{
		stopeffect(300);
    setenable(0, "Visible", true);
	};
};