skill(130801)
{
	section(233)
	{
        addbreaksection(0, 300, 1500)
        {   
            sendmessage("break");
        };
        addbreaksection(0, 1900, 2050)
        {   
            sendmessage("break");
        };
        addbreaksection(1, 1900, 2050);
        addbreaksection(10, 1900, 2050);
        addbreaksection(20, 0, 2050);
        addbreaksection(30, 1900, 2050);
		movecontrol(true);
		animation("fashi_binghuozoulang_01_01"); 
        //֡7
        summonnpc(200, 101, "Hero/6_fashi/base_ef", 130811, vector3(0, 0, 0));
        summonnpc(200, 101, "Hero/6_fashi/base_ef", 130812, vector3(0, 0, 2));
        summonnpc(200, 101, "Hero/6_fashi/base_ef", 130813, vector3(0, 0, 5));
        summonnpc(200, 101, "Hero/6_fashi/base_ef", 130814, vector3(0, 0, 8));
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",2000,"Bip001 L Forearm",0, true);
	};
    section(1400)
	{
        animation("fashi_binghuozoulang_01_02")
        {
            wrapmode(2);
        }; 
        summonnpc(1366, 110, "Hero/6_fashi/hanbingzoulang", 130815, vector3(0,0,0))
        {
            signforskill(801);
        };
        summonnpc(1366, 111, "Hero/6_fashi/6_fashi_04", 130817, vector3(0,0,0))
        {
            signforskill(802);
        };
        summonnpc(1366, 112, "Hero/6_fashi/6_fashi_04", 130818, vector3(0,0,11.5))
        {
            signforskill(803);
        };
    };
    section(433)
	{
        animation("fashi_binghuozoulang_01_03"); 
    };
    onmessage("break")
    {
         removebreaksection(0);
         gotosection(0, 2, 0);
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

skill(130802)
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
        findmovetarget(0, vector3(0, 0, 0), 12, 360, 0.5, 0.5, -1, 6, false,"sons");
        {
             signforskill(801,false);
        };
        facetotarget(50, 400);
        sonreleaseskill(50,801,130816);
        sonreleaseskill(50,802,130819);
        sonreleaseskill(50,803,130820);
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

//寒冰走廊未形成
skill(130811)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_01",1400,"ef_base",0);
    };
    section(1)
    {
        destroyself(1); 
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

//寒冰吸引1
skill(130812)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,2),eular(0,0,0),"RelativeOwner",false); 
        colliderdamage(0, 1400, true, false, 200, 0)
        {
            stateimpact("kKnockDown", 13080103);
            stateimpact("kLauncher", 13080102);
            stateimpact("kDefault", 13080101);
            scenecollider("Hero/6_fashi/binghuozoulangcollider1",vector3(0,0,0));
        };
    };
    section(1)
    {
        destroyself(1); 
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

//寒冰吸引2
skill(130813)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,5),eular(0,0,0),"RelativeOwner",false); 
        colliderdamage(100, 1400, true, false, 200, 0)
        {
            stateimpact("kKnockDown", 13080103);
            stateimpact("kLauncher", 13080102);
            stateimpact("kDefault", 13080101);
            scenecollider("Hero/6_fashi/binghuozoulangcollider1",vector3(0,0,0));
        };
    };
    section(1)
    {
        destroyself(1); 
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

//寒冰吸引3
skill(130814)
{
    section(1500)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,8),eular(0,0,0),"RelativeOwner",false); 
        colliderdamage(200, 1400, true, false, 200, 0)
        {
            stateimpact("kKnockDown", 13080103);
            stateimpact("kLauncher", 13080102);
            stateimpact("kDefault", 13080101);
            scenecollider("Hero/6_fashi/binghuozoulangcollider1",vector3(0,0,0));
        };
    };
    section(1)
    {
        destroyself(1); 
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

//召唤物：寒冰走廊
skill(130815)
{
    section(4000)
    {
        movecontrol(true);
        addbreaksection(11, 0, 3900);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_02",4000,"ef_base",0);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_04",2000,vector3(0,-1.5,0),4000,eular(0,0,0),vector3(1,1,1),true);
        areadamage(4000, 0, 1, 2, 2, true) 
        {
            stateimpact("kKnockDown", 13080203);
            stateimpact("kLauncher", 13080202);
            stateimpact("kDefault", 13080201);
        };
        areadamage(4000, 0, 1, 5, 2, true) 
        {
            stateimpact("kKnockDown", 13080203);
            stateimpact("kLauncher", 13080202);
            stateimpact("kDefault", 13080201);
        };
        areadamage(4000, 0, 1, 8, 2, true) 
        {
            stateimpact("kKnockDown", 13080203);
            stateimpact("kLauncher", 13080202);
            stateimpact("kDefault", 13080201);
        };
    };
    section(1)
    {
        destroyself(1); 
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

//寒冰走廊破碎
skill(130816)
{
    section(1433)
    {
        movecontrol(true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_04",2000,vector3(0,-1.5,0),1433,eular(0,0,0),vector3(1,1,1),true);
        areadamage(1433, 0, 1, 2, 2, true) 
        {
            stateimpact("kKnockDown", 13080203);
            stateimpact("kLauncher", 13080202);
            stateimpact("kDefault", 13080201);
        };
        areadamage(1433, 0, 1, 5, 2, true) 
        {
            stateimpact("kKnockDown", 13080203);
            stateimpact("kLauncher", 13080202);
            stateimpact("kDefault", 13080201);
        };
        areadamage(1433, 0, 1, 8, 2, true) 
        {
            stateimpact("kKnockDown", 13080203);
            stateimpact("kLauncher", 13080202);
            stateimpact("kDefault", 13080201);
        };
    };
    section(1)
    {
        destroyself(1); 
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

//寒冰分身1
skill(130817)
{
    section(4000)
    {
        addbreaksection(11, 0, 3900);
        movecontrol(true);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        animation("fashi_binghuozoulang_01_02")
        {
            wrapmode(2);
        }; 
    };
    section(1)
    {
        destroyself(1); 
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

//寒冰分身2
skill(130818)
{
    section(4000)
    {
        addbreaksection(11, 0, 3900);
        movecontrol(true);
        settransform(0," ",vector3(0,0,11.5),eular(0,180,0),"RelativeOwner",false); 
        animation("fashi_binghuozoulang_01_02")
        {
            wrapmode(2);
        }; 
    };
    section(1)
    {
        destroyself(1); 
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


//寒冰分身陨落1
skill(130819)
{
    section(433)
    {
        movecontrol(true);
        animation("fashi_binghuozoulang_01_03"); 
        //shadercolor(0,433,"1_FaShi_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,255,73),Color(593,0,-593,-170));
        //setenable(400, "Visible", false);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_01",2000,vector3(0,0,0),0);
        summonnpc(400, 101, "Hero/6_fashi/6_fashi_03", 130821, vector3(0,0,0));
    };
    section(1)
    {
        destroyself(1); 
    };
	oninterrupt()
	{
        //shadercolor(0,0,"1_FaShi_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,255,73),Color(0,0,0,0));
        destroyself(0);
	};
	onstop()
	{
        //shadercolor(0,0,"1_FaShi_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,255,73),Color(0,0,0,0));
        destroyself(0);
	};
};

//寒冰分身陨落2
skill(130820)
{
    section(433)
    {
        movecontrol(true);
        animation("fashi_binghuozoulang_01_03"); 
        //shadercolor(0,433,"1_FaShi_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,1,0.7),Color(593,0,-593,-170));
        //setenable(400, "Visible", false);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_01",2000,vector3(0,0,0),0);
        summonnpc(400, 101, "Hero/6_fashi/6_fashi_03", 130821, vector3(0,0,0));
    };
    section(1)
    {
        destroyself(1); 
    };
	oninterrupt()
	{
        //shadercolor(0,0,"1_FaShi_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,255,73),Color(0,0,0,0));
        destroyself(0);
	};
	onstop()
	{
        //shadercolor(0,0,"1_FaShi_01","DFM/AlphaBlendDisorderWithLight",Color(0,0,255,73),Color(0,0,0,0));
        destroyself(0);
	};
};


//烈焰分身
skill(130821)
{
    section(233)
    {
		animation("fashi_huoyanfenshen_01_03"); 
        movecontrol(true);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
    };
    section(800)
    {
        animation("fashi_huoyanfenshen_01_01")
        {
            wrapmode(2);
        }; 
        summonnpc(0, 101, "Hero/6_fashi/6_fashi_03", 130822, vector3(0,0,0));
        summonnpc(200, 101, "Hero/6_fashi/6_fashi_03", 130822, vector3(0,0,0));
        summonnpc(400, 101, "Hero/6_fashi/6_fashi_03", 130822, vector3(0,0,0));
        summonnpc(600, 101, "Hero/6_fashi/6_fashi_03", 130822, vector3(0,0,0));
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_03",2000,"Bone_Root",0, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_03",2000,"Bone_Root",200, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_03",2000,"Bone_Root",400, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_hanbingzoulang_03",2000,"Bone_Root",600, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_02_huanying02",2000,"Bone_Root",800, false);
    };
    section(1)
    {
        destroyself(1); 
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

//烈焰分身：冲击
skill(130822)
{
    section(200)
    {
        movecontrol(true);
        
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        startcurvemove(0, true, 0.2, 0, 0, 50, 0, 0, 0); 
        animation("fashi_binghuozoulang_02_03")
        {
            wrapmode(2);
        }; 
        colliderdamage(0, 200, true, true, 200, 0)
        {
            stateimpact("kKnockDown", 13080213);
            stateimpact("kLauncher", 13080212);
            stateimpact("kDefault", 13080211);
			bonecollider("Hero/6_fashi/binghuozoulangcollider1","Bone_Root", true);
        };
    };
    section(1)
    {
        destroyself(1); 
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