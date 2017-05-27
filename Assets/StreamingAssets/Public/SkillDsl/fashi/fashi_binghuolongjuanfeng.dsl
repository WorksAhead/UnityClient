skill(130901)
{
	section(2700)
	{
        addbreaksection(0, 300, 2700);
        addbreaksection(1, 2100, 2700);
        addbreaksection(10, 2100, 2700);
        addbreaksection(20, 0, 2700);
        addbreaksection(30, 2000, 2700);
        enablechangedir(0, 2100);
		movecontrol(true);
		animation("fashi_longjuanfeng_01"); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",700,"Bip001 L Forearm",0, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",900,"Bip001 R Forearm",700, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",1000,"Bip001 L Forearm",1600, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",1000,"Bip001 R Forearm",1600, true);
        summonnpc(333, 101, "Hero/6_fashi/base_ef", 130911, vector3(0, 0, 0.5));  
        summonnpc(866, 101, "Hero/6_fashi/base_ef", 130912, vector3(0, 0, 0.5));  
        summonnpc(1800, 101, "Hero/6_fashi/base_ef", 130913, vector3(-1, 0, 0.5));  
        summonnpc(1800, 101, "Hero/6_fashi/base_ef", 130914, vector3(0, 0, 0.5));  
        summonnpc(1800, 101, "Hero/6_fashi/base_ef", 130915, vector3(1, 0, 0.5));  
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_05",1500,"Bone_Root",333, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_04",1500,"Bone_Root",866, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_06",1500,"Bone_Root",1800, true);
		startcurvemove(266, true, 0.05, 0, 0, 8, 0, 0, 160, 0.05, 0, 0, 16, 0, 0, -160);
		startcurvemove(800, true, 0.05, 0, 0, 8, 0, 0, 160, 0.05, 0, 0, 16, 0, 0, -160);
		startcurvemove(1433, true, 0.05, 0, 0, -4, 0, 0, -80, 0.05, 0, 0, -8, 0, 0, 80);
		startcurvemove(1666, true, 0.05, 0, 0, 12, 0, 0, 240, 0.05, 0, 0, 24, 0, 0, -240);
	};
	oninterrupt()
	{
		stopeffect(100);
	};
	
	onstop()
	{
		stopeffect(100);
	};
};

skill(130902)
{
	section(1233)
	{
        addbreaksection(1, 1000, 1233);
        addbreaksection(10, 1000, 1233);
        addbreaksection(20, 0, 1233);
        addbreaksection(30, 900, 1233);
        enablechangedir(0, 2100);
		movecontrol(true);
		animation("fashi_longjuanfeng_02"); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",1200,"Bip001 R Forearm",0, true);
        summonnpc(666, 101, "Hero/6_fashi/base_ef", 130917, vector3(0, 0, 0.5));  
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_04",1500,"Bone_Root",600, true);
		startcurvemove(66, true, 0.05, 0, 0, -4, 0, 0, -80, 0.05, 0, 0, -8, 0, 0, 80);
        
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

//寒冰旋风
skill(130911)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0.5),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_02",2000,"ef_base",10, true);

         colliderdamage(0, 500, true, false, 50, 10)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
         colliderdamage(500, 1500, false, false, 200, 5)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
		startcurvemove(0, true, 0.1, -40, 0, 0, 400, 0, 400, 0.4, 0, 0, 10, 10, 0, -20, 0.8, 4, 0, 2, -5, 0, -4);
        destroyself(1500);
    };
    //onmessage("oncollide"){};
	oninterrupt()
	{
        destroyself(0);
	};
	onstop()
	{
        destroyself(0);
	};
};

//赤炎旋风
skill(130912)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0.5),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_01",2000,"ef_base",10, true);

         colliderdamage(0, 500, true, false, 50, 10)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
         colliderdamage(500, 1500, false, false, 200, 5)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
		startcurvemove(0, true, 0.1, 40, 0, 0, -400, 0, 400, 0.4, 0, 0, 10, -10, 0, -20, 0.8, -4, 0, 2, 5, 0, -4);
        destroyself(1500);
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

//寒冰旋风
skill(130913)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(-1,0,0.5),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_02",2000,"ef_base",10, true);

         colliderdamage(0, 500, true, false, 50, 10)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
         colliderdamage(500, 1500, false, false, 200, 5)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
		startcurvemove(0, true, 0.1, -40, 0, 0, 400, 0, 400, 0.4, 0, 0, 10, 0, 0, -20, 0.8, 0, 0, 2, 0, 0, -4);
		destroyself(1500);
    };
    //onmessage("oncollide"){};
	oninterrupt()
	{
        destroyself(0);
	};
	onstop()
	{
        destroyself(0);
	};
};

//寒冰旋风
skill(130914)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0.5),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_02",2000,"ef_base",10, true);

         colliderdamage(0, 500, true, false, 50, 10)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
         colliderdamage(500, 1500, false, false, 200, 5)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
		startcurvemove(0, true, 0.1, -40, 0, 0, 400, 0, 400, 0.4, 0, 0, 10, 20, 0, -20, 0.8, 8, 0, 2, -20, 0, -4);
        destroyself(1500);
    };
    //onmessage("oncollide"){};
	oninterrupt()
	{
        destroyself(0);
	};
	onstop()
	{
        destroyself(0);
	};
};

//赤炎旋风
skill(130915)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(1,0,0.5),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_01",2000,"ef_base",10, true);

         colliderdamage(0, 500, true, false, 50, 10)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
         colliderdamage(500, 1500, false, false, 200, 5)
        {
            stateimpact("kKnockDown", 13090103);
            stateimpact("kLauncher", 13090102);
            stateimpact("kDefault", 13090101);
            bonecollider("Hero/6_fashi/longjuanfengcollider1","ef_base", true);
        };
		startcurvemove(0, true, 0.1, 40, 0, 0, -400, 0, 400, 0.4, 0, 0, 10, 0, 0, -20, 0.8, 0, 0, 2, 0, 0, -4);
        destroyself(1500);
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

//灭炎龙卷风底座
skill(130917)
{
    section(2000)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0.5),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_08",1900,"ef_base",10, true);
        colliderdamage(0, 1900, false, false, 1900, 1)
        {
			attachenemy("ef_body", eular(0, 0, 0), 13090211, -1, 13090212, -1, 13090213, -1);
            bonecollider("Hero/6_fashi/longjuanfengcollider2","ef_base", true);
        };
		startcurvemove(0, true, 1, 4, 0, 3, -4, 0, 0, 1, 0, 0, 3, -4, 0, 0);
        summonnpc(1950, 101, "Hero/6_fashi/base_ef", 130918, vector3(0, 0, 0));  
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

//灭炎龙卷风
skill(130918)
{
    section(4000)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xuanfeng_07",4000,"ef_base",0, true);
        colliderdamage(0, 3750, true, false, 150, 25)
        {
            stateimpact("kKnockDown", 13090203);
            stateimpact("kLauncher", 13090202);
            stateimpact("kDefault", 13090201);
            bonecollider("Hero/6_fashi/longjuanfengcollider3","ef_base", true);
        };
        destroyself(4000);
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