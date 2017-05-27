skill(130001)
{
	section(766)
	{
		addbreaksection(1, 500, 766);
		addbreaksection(10, 400, 766);
		addbreaksection(20, 0, 766);
		addbreaksection(30, 0, 200);
		addbreaksection(30, 400, 766);
		movecontrol(true);
		animation("fashi_pugong_03");
		//֡1
		setanimspeed(33, "fashi_pugong_03", 2);
		//֡7
		setanimspeed(100, "fashi_pugong_03", 0.5);
		//֡9
		setanimspeed(233, "fashi_pugong_03", 6);
		//֡15
		setanimspeed(266, "fashi_pugong_03", 1);
        //  30
		startcurvemove(0, true, 0.05, 0, 0, 4, 0, 0, 80, 0.05, 0, 0, 8, 0, 0, -80);
		startcurvemove(300, true, 0.1, 0, 0, 4, 0, 0, 40, 0.1, 0, 0, 8, 0, 0, -40);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",700,"Bip001 R Forearm",0, true);
        summonnpc(0, 101, "Hero/6_fashi/6_moshen_01", 130011, vector3(0, 0, 0));
        //areadamage(266, 0, 1, 1, 2.3, true) 
        //{
        //    stateimpact("kKnockDown", 13000103);
        //    stateimpact("kLauncher", 13000102);
        //    stateimpact("kDefault", 13000101);
        //};
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

skill(130002)
{
	section(500)
	{
		addbreaksection(1, 400, 500);
		addbreaksection(10, 400, 500);
		addbreaksection(20, 0, 500);
		addbreaksection(30, 0, 200);
		addbreaksection(30, 400, 500);
		movecontrol(true);
		animation("fashi_pugong_02");
		//֡1
		setanimspeed(33, "fashi_pugong_02", 0.5);
		//֡3
		setanimspeed(166, "fashi_pugong_02", 2);
		//֡9
		setanimspeed(266, "fashi_pugong_02", 1);
        //  13
		//startcurvemove(166, true, 0.05, 0, 0, 4, 0, 0, 80, 0.05, 0, 0, 8, 0, 0, -80);
        summonnpc(0, 101, "Hero/6_fashi/6_moshen_01", 130012, vector3(0, 0, 0));
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",500,"Bip001 L Forearm",0, true);
        //areadamage(266, 0, 1, 1, 2.3, true) 
        //{
        //    stateimpact("kKnockDown", 13000203);
        //    stateimpact("kLauncher", 13000202);
        //    stateimpact("kDefault", 13000201);
        //};
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

skill(130003)
{
	section(833)
	{
		addbreaksection(1, 450, 833);
		addbreaksection(10, 300, 833);
		addbreaksection(20, 0, 833);
		addbreaksection(30, 0, 200);
		addbreaksection(30, 300, 833);
		movecontrol(true);
		animation("fashi_pugong_04");
		//֡1
		setanimspeed(33, "fashi_pugong_04", 0.5);
		//֡2
		setanimspeed(100, "fashi_pugong_04", 1.5);
		//֡5
		setanimspeed(166, "fashi_pugong_04", 1);
        //  22
		startcurvemove(100, true, 0.05, 0, 0, 30, 0, 0, -200);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",833,"Bip001 R Forearm",0, true);
        summonnpc(0, 101, "Hero/6_fashi/6_moshen_01", 130013, vector3(0, 0, 0));
        //areadamage(166, 0, 1, 1, 2.3, true) 
        //{
        //    stateimpact("kKnockDown", 13000303);
        //    stateimpact("kLauncher", 13000302);
        //    stateimpact("kDefault", 13000301);
        //};
	};
	oninterrupt()
	{
		stopeffect(100);
        resetcamerafollowspeed(0);
	};
	onstop()
	{
		stopeffect(100);
        resetcamerafollowspeed(0);
	};
};

skill(130004)
{
	section(1166)
	{
		addbreaksection(1, 600, 1266);
		addbreaksection(10, 900, 1266);
		addbreaksection(20, 0, 1266);
		addbreaksection(30, 0, 400);
		addbreaksection(30, 600, 1266);
		movecontrol(true);
		animation("fashi_pugong_05");
		//֡1
		setanimspeed(33, "fashi_pugong_05", 3);
		//֡19
		setanimspeed(233, "fashi_pugong_05", 1);
        //  40
        summonnpc(0, 101, "Hero/6_fashi/6_moshen_01", 130014, vector3(0, 0, -3));
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",1166,"Bip001 L Forearm",0, true);
        //areadamage(233, 0, 1, 1, 2.5, true) 
        //{
        //    stateimpact("kKnockDown", 13000403);
        //    stateimpact("kLauncher", 13000402);
        //    stateimpact("kDefault", 13000401);
        //};
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

skill(130011)
{   
    section(500)
    {
        movecontrol(true);
        animation("moshen_pugong_01");
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        startcurvemove(10, false, 0.15, 0, 0, 6, 0, 0, 0, 0.05, 0, 0, 30, 0, 0, 0, 0.05, 0, 0, 30, 0, 0, -480);
        shadercolor(0,300,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,0,0),Color(0.666,0.666,0.666,3.33));
        shadercolor(300,200,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,1),Color(0,0,0,-2.5));
        areadamage(266, 0, 0, 3, 3.5, true) 
        { 
            showtip(1000, 0, 1, 0);
            stateimpact("kKnockDown", 13000103);
            stateimpact("kLauncher", 13000102);
            stateimpact("kDefault", 13000101);
            
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_moshen_pugong_01",700,"Bone_Root",233, true);
        destroyself(500); 
    };
    oninterrupt()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(0);
        setenable(0, "Visible", true);
    };
    onstop()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(0);
        setenable(0, "Visible", true);
    };
};

skill(130012)
{
   section(500)
    {
        movecontrol(true);
        animation("moshen_pugong_02");
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        startcurvemove(10, false, 0.15, 0, 0, 6, 0, 0, 0, 0.05, 0, 0, 30, 0, 0, 0, 0.05, 0, 0, 30, 0, 0, -480);
        shadercolor(0,300,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,0,0),Color(0.666,0.666,0.666,3.33));
        shadercolor(300,200,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,1),Color(0,0,0,-2.5));
        areadamage(266, 0, 0, 3, 3.5, true) 
        {
            showtip(1000, 1, 0, 0);
            stateimpact("kKnockDown", 13000203);
            stateimpact("kLauncher", 13000202);
            stateimpact("kDefault", 13000201);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_moshen_pugong_02",700,"Bone_Root",233, true);
        destroyself(500); 
    };
    oninterrupt()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(300);
        setenable(0, "Visible", true);
    };
    onstop()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(300);
        setenable(0, "Visible", true);
    };
};

skill(130013)
{
    section(600)
    {
        movecontrol(true);
        animation("moshen_pugong_03");
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        startcurvemove(10, false, 0.1, 0, 0, 9, 0, 0, 0, 0.05, 0, 0, 30, 0, 0, 0, 0.05, 0, 0, 30, 0, 0, -480);
        shadercolor(0,300,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,0,0),Color(0.666,0.666,0.666,3.33));
        shadercolor(300,300,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,1),Color(0,0,0,-1.5));
        areadamage(166, 0, 0, 3, 3.5, true) 
        {
            showtip(1000, 0, 0, 1);
            stateimpact("kKnockDown", 13000103);
            stateimpact("kLauncher", 13000102);
            stateimpact("kDefault", 13000101);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_moshen_pugong_03",700,"Bone_Root",133, true);
        destroyself(600); 
    };
    oninterrupt()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(300);
        setenable(0, "Visible", true);
    };
    onstop()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(300);
        setenable(0, "Visible", true);
    };
};

skill(130014)
{
   section(550)
    {
        movecontrol(true);
        animation("moshen_pugong_04");
        settransform(0," ",vector3(0,0,-3),eular(0,0,0),"RelativeOwner",false); 
        startcurvemove(10, false, 0.17, 0, 0, 4, 0, 0, 0, 0.05, 0, 0, 20, 0, 0, 0, 0.05, 0, 0, 20, 0, 0, -320);
        shadercolor(0,300,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,0,0),Color(0.666,0.666,0.666,3.33));
        shadercolor(300,250,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,1),Color(0,0,0,-2));
        areadamage(233, 0, 0, 6, 3.5, true) 
        {
            showtip(1000, 0, 0, 1);
            stateimpact("kKnockDown", 13000403);
            stateimpact("kLauncher", 13000402);
            stateimpact("kDefault", 13000401);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_moshen_pugong_04",1500,"Bone_Root",200, false);
        destroyself(550); 
    };
    oninterrupt()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(300);
        setenable(0, "Visible", true);
    };
    onstop()
    {
        shadercolor(0,0,"1_T_HunDunMoShen_01","DFM/AlphaBlendDisorderWithLight",Color(0.2,0.2,0.2,0.8),Color(0,0,0,0));
        destroyself(0); 
        stopeffect(300);
        setenable(0, "Visible", true);
    };
};
