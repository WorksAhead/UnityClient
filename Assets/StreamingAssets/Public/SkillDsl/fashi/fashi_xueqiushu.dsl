skill(130301)
{
	section(233)
	{
        addbreaksection(0, 300, 1600);
        addbreaksection(0, 2000, 2050);
        addbreaksection(1, 2000, 2050);
        addbreaksection(10, 2000, 2050);
        addbreaksection(20, 0, 2050);
        addbreaksection(30, 2000, 2050);
		movecontrol(true);
		animation("fashi_xueqiu_01_01"); 
        //֡1
        setanimspeed(33, "fashi_xueqiu_01_01", 2);
        //֡9
        setanimspeed(166, "fashi_xueqiu_01_01", 1);
        //֡11
        summonnpc(200, 101, "Hero/6_fashi/base", 130311, vector3(0, 0, 0));  
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",2000,"Bip001 L Forearm",0, true);
        //findmovetarget(200, vector3(0, 0, 0), 5.5, 60, 0.1, 0.9, 0, 0, false);
        //gotosection(210, 1, 0) 
        //{
        //    targetcondition(true);
        //};
        //findmovetarget(260, vector3(0, 0, 0), 10, 60, 0.1, 0.9, 0, 0, false);
        //gotosection(270, 2, 0) 
        //{
        //    targetcondition(true);
        //};
	};
    section(1800)
	{
        animation("fashi_xueqiu_01_03"); 
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_03",1000,vector3(-0.2,1.5,3),1500);
        summonnpc(1600, 104, "Hero_FX/6_Fashi/6_hero_fashi_xueqiu_02", 130312, vector3(0,0,0))
        {
            signforskill(301);
        };
        summonnpc(1600, 105, "Hero/6_fashi/base_ef", 130314, vector3(0,0,0))  
        {
            signforskill(302);
        };
    };
    //section(1800)
    //{
    //    animation("fashi_xueqiu_01_03"); 
    //    sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_03",1000,vector3(-0.2,1.5,7),1500);
    //    summonnpc(1600, 104, "Hero_FX/6_Fashi/6_hero_fashi_xueqiu_02", 130313, vector3(0,0,0))
    //    {
    //        signforskill(301);
    //    };  
    //    summonnpc(1600, 105, "Hero/6_fashi/base_ef", 130315, vector3(0,0,0))
    //    {
    //        signforskill(302);
    //    };
    //    gotosection(1800, 3, 0);
    //};
    section(1)
	{
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

skill(130302)
{
	section(1030)
	{
        addbreaksection(1, 300, 1030);
        addbreaksection(10, 300, 1030);
        addbreaksection(20, 0, 1030);
        addbreaksection(30, 300, 1030);
		movecontrol(true);
		animation("fashi_xueqiu_02"); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",1000,"Bip001 L Forearm",0, true);
        findmovetarget(0, vector3(0, 0, 0), 12, 360, 0.5, 0.5, -1, 6, false,"sons")
        {
             signforskill(301,false);
        };
        facetotarget(50, 400);
        sonreleaseskill(50,301,130316);
        sonreleaseskill(50,302,130317);
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

//雪漩涡
skill(130311)
{
    section(20)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        findmovetarget(10, vector3(0, 0, 0), 5.5, 60, 0.1, 0.9, 0, 0, false);
        //gotosection(20, 1, 0) 
        //{
        //    targetcondition(true);
        //};
        //findmovetarget(60, vector3(0, 0, 0), 10, 60, 0.1, 0.9, 0, 0, false);
        //gotosection(70, 2, 0) 
        //{
        //    targetcondition(true);
        //};
        //gotosection(50, 1, 0);
    };
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(-0.2,0.8,3),eular(0,0,0),"RelativeSelf",false); 
        colliderdamage(50, 1400, true, false, 200, 0)
        {
            stateimpact("kKnockDown", 13020103);
            stateimpact("kLauncher", 13020102);
            stateimpact("kDefault", 13020101);
            scenecollider("Hero/6_fashi/xueqiucollider1",vector3(0,0,0));
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_01",1400,vector3(0,0,0),50);
        gotosection(1500, 3, 0);
    };
    //section(1500)
    //{
    //    movecontrol(true);
    //    settransform(0," ",vector3(-0.2,0.8,7),eular(0,0,0),"RelativeSelf",false); 
    //    colliderdamage(50, 1400, true, false, 200, 0)
    //    {
    //        stateimpact("kKnockDown", 13020103);
    //        stateimpact("kLauncher", 13020102);
    //        stateimpact("kDefault", 13020101);
    //        scenecollider("Hero/6_fashi/xueqiucollider1",vector3(0,0,0));
    //    };
    //    sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_01",1400,vector3(0,0,0),50);
    //    gotosection(1500, 3, 0);
    //};
    section(1)
    {
        destroyself(1); 
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

//雪球:近
skill(130312)
{
    section(4000)
    {
        addbreaksection(11, 0, 3900);
        movecontrol(true);
        settransform(0," ",vector3(-0.2,1.5,3),eular(0,0,0),"RelativeOwner",false); 
        setlifetime(0, 4000);
        areadamage(0, 0, 0, 0, 2, true) 
        {
            stateimpact("kKnockDown", 13020113);
            stateimpact("kLauncher", 13020112);
            stateimpact("kDefault", 13020111);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_05",4000,vector3(0,-1.5,0),4000);
        areadamage(4000, 0, 0, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13020203);
            stateimpact("kLauncher", 13020202);
            stateimpact("kDefault", 13020201);
        };
        //gotosection(50, 1, 0);
    };
    section(1)
    {
        destroyself(1); 
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

//雪球:远
skill(130313)
{
    section(4000)
    {
        addbreaksection(11, 0, 3900);
        movecontrol(true);
        settransform(0," ",vector3(-0.2,1.5,7),eular(0,0,0),"RelativeOwner",false); 
        setlifetime(0, 4000);
        areadamage(0, 0, 0, 0, 1.6, true) 
        {
            stateimpact("kKnockDown", 13020113);
            stateimpact("kLauncher", 13020112);
            stateimpact("kDefault", 13020111);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_05",4000,vector3(0,-1.5,0),4000);
        areadamage(4000, 0, 0, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13020203);
            stateimpact("kLauncher", 13020202);
            stateimpact("kDefault", 13020201);
        };
        //gotosection(50, 1, 0);
    };
    section(1)
    {
        destroyself(1); 
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

//雪球地面效果:近
skill(130314)
{
    section(4000)
    {
        addbreaksection(11, 0, 3900);
        movecontrol(true);
        settransform(0," ",vector3(-0.2,0,3),eular(0,0,0),"RelativeOwner",false); 
        setlifetime(0, 4000);
    };
    section(1)
    {
        destroyself(1); 
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

//雪球地面效果:远
skill(130315)
{
    section(4000)
    {
        addbreaksection(11, 0, 3900);
        movecontrol(true);
        settransform(0," ",vector3(-0.2,0,7),eular(0,0,0),"RelativeOwner",false); 
        setlifetime(0, 4000); 
    };
    section(1)
    {
        destroyself(1); 
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

//雪球:推动
skill(130316)
{   
    section(50)
    {
        movecontrol(true);
        findmovetarget(0, vector3(0, 0, 0), 5.5, 360, 0.5, 0.5, 0, 0, false,"owner");
        gotosection(10, 1, 0) 
        {
            targetcondition(true);
        };
        findmovetarget(40, vector3(0, 0, 0), 12, 360, 0.5, 0.5, 0, 0, false,"owner");
        gotosection(50, 2, 0) 
        {
            targetcondition(true);
        };
    };
    section(310)
    {
        rotate(20, 310, vector3(-1080, 0, 0));
        findmovetarget(0, vector3(0, 0, 0), 5.5, 360, 0.5, 0.5, -1, -6, true,"owner");
        facetotarget(10, 100);
        startcurvemove(10, true, 0.3, 0, 0, -20, 0, 0, 0);
        areadamage(10, 0, 0, 0, 1.6, true) 
        {
            stateimpact("kKnockDown", 13020223);
            stateimpact("kLauncher", 13020222);
            stateimpact("kDefault", 13020221);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_05",4000,vector3(0,-1.5,0),310);
        areadamage(310, 0, 0, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13020203);
            stateimpact("kLauncher", 13020202);
            stateimpact("kDefault", 13020201);
        };
        gotosection(310, 3, 0);
    };
    section(310)
    {
        rotate(20, 310, vector3(1080, 0, 0));
        findmovetarget(0, vector3(0, 0, 0), 12, 360, 0.5, 0.5, -1, 6, true,"owner");
        facetotarget(10, 100);
        startcurvemove(10, true, 0.3, 0, 0, 20, 0, 0, 0);
        areadamage(10, 0, 0, 0, 1.6, true) 
        {
            stateimpact("kKnockDown", 13020213);
            stateimpact("kLauncher", 13020212);
            stateimpact("kDefault", 13020211);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_05",4000,vector3(0,-1.5,0),310);
        areadamage(310, 0, 0, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13020203);
            stateimpact("kLauncher", 13020202);
            stateimpact("kDefault", 13020201);
        };
        gotosection(310, 3, 0);
    };
    section(1)
    {
        destroyself(1); 
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

//雪球地面滚动
skill(130317)
{   
    section(50)
    {
        movecontrol(true);
        findmovetarget(0, vector3(0, 0, 0), 5.5, 360, 0.5, 0.5, 0, 0, false,"owner");
        gotosection(10, 1, 0) 
        {
            targetcondition(true);
        };
        findmovetarget(40, vector3(0, 0, 0), 12, 360, 0.5, 0.5, 0, 0, false,"owner");
        gotosection(50, 2, 0) 
        {
            targetcondition(true);
        };
    };
    section(310)
    {  
        findmovetarget(0, vector3(0, 0, 0), 5.5, 360, 0.5, 0.5, -1, -6, true,"owner");
        facetotarget(10, 100);
        startcurvemove(10, true, 0.3, 0, 0, -20, 0, 0, 0);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_06",300,"ef_base",10, true);
        gotosection(310, 3, 0);
    };
    section(310)
    {
        findmovetarget(0, vector3(0, 0, 0), 12, 360, 0.5, 0.5, -1, 6, true,"owner");
        facetotarget(10, 100);
        startcurvemove(10, true, 0.3, 0, 0, 20, 0, 0, 0);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_xueqiu_04",300,"ef_base",10, true);
        gotosection(310, 3, 0);
    };
    section(1)
    {
        destroyself(1); 
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