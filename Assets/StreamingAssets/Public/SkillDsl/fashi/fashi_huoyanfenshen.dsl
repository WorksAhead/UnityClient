skill(130401)
{
	section(933)
	{   
        addbreaksection(0, 600, 950)
        {   
            sendmessage("break");
        };
        addbreaksection(0, 1200, 1500)
        {   
            sendmessage("break");
        };
        addbreaksection(0, 1800, 1867)
        {   
            sendmessage("break");
        };
        addbreaksection(1, 1800, 1867);
        addbreaksection(10, 1800, 1867);
        addbreaksection(20, 0, 1867);
        addbreaksection(30, 1800, 1867);
		movecontrol(true);
		animation("fashi_huoyanfenshen_01"); 
        startcurvemove(366, true, 0.1, 0, 0, 60, 0, 0, 0); 
        enablechangedir(0, 350);
        enablechangedir(500, 933);
        summonnpc(330, 106, "Hero/6_fashi/6_fashi_03", 130411, vector3(0, 0, 0))
        {
            signforskill(401);
        };  
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",1800,"Bip001 R Forearm",0, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_01",1500,"Bone_Root",100, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_03_01",1000,"Bone_Root",366, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_03_02",1000,"Bone_Root",366, false);
        setenable(733, "Visible", false);
	};
    section(566)
	{   
        setenable(0, "Visible", true);
        summonnpc(0, 107, "Hero/6_fashi/6_fashi_03", 130411, vector3(0, 0, 0))
        {
            signforskill(402);
        };  
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_03_01",1000,"Bone_Root",0, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_03_02",1000,"Bone_Root",0, false);
        enablechangedir(300, 566);
        startcurvemove(133, true, 0.1, 0, 0, 60, 0, 0, 0); 
		animation("fashi_huoyanfenshen_01_02"); 
        setenable(366, "Visible", false);
	};
    section(366)
	{   
        setenable(0, "Visible", true);
        summonnpc(0, 108, "Hero/6_fashi/6_fashi_03", 130411, vector3(0, 0, 0))
        {
            signforskill(403);
        };  
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_03_01",1000,"Bone_Root",0, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_03_02",1000,"Bone_Root",0, false);
        summonnpc(250, 109, "Hero/6_fashi/6_fashi_03", 130411, vector3(0, 0, 0))
        {
            signforskill(404);
        };  
        startcurvemove(133, true, 0.1, 0, 0, 60, 0, 0, 0); 
		animation("fashi_huoyanfenshen_01_02"); 
	};
    section(1)
	{   
        setenable(0, "Visible", true);
	};
    onmessage("break")
    {
         removebreaksection(0);
         gotosection(0, 3, 0);
    };
	oninterrupt()
	{
        setenable(0, "Visible", true);
		stopeffect(100);
	};
	
	onstop()
	{
        setenable(0, "Visible", true);
		stopeffect(100);
	};
};

skill(130402)
{
	section(500)
	{
        addbreaksection(1, 1200, 1466);
        addbreaksection(10, 1200, 1466);
        addbreaksection(20, 0, 1466);
        addbreaksection(30, 1200, 1466);
		movecontrol(true);
        
		animation("fashi_huoyanfenshen_02"); 
        findmovetarget(0, vector3(0, 0, 0), 12, 360, 0.5, 0.5, -1, 6, false,"sons")
        {
             signforskill(404,false);
        };
        facetotarget(100, 400);
        sonreleaseskill(0,404,130415);
        sonreleaseskill(0,403,130414);
        sonreleaseskill(0,402,130413);
        sonreleaseskill(0,401,130412);
        findmovetarget(400, vector3(0, 0, 0), 12, 360, 0.5, 0.5, -1, 6, false,"sons")
        {
             signforskill(404,false);
        };
        settransform(500," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false); 
	};
	section(700)
	{
        findmovetarget(0, vector3(0, 0, 0), 12, 360, 0.5, 0.5, 0, 0, true,"sons")
        {
             signforskill(403,false);
        };
        startcurvemove(100, false, 0.15, 0, 0, 30, 0, 0, -200);
        findmovetarget(200, vector3(0, 0, 0), 12, 360, 0.5, 0.5, 0, 0, true,"sons")
        {
             signforskill(402,false);
        };
        startcurvemove(300, false, 0.15, 0, 0, 30, 0, 0, -200);
        findmovetarget(400, vector3(0, 0, 0), 12, 360, 0.5, 0.5, 0, 0, true,"sons")
        {
             signforskill(401,false);
        };
        startcurvemove(500, false, 0.15, 0, 0, 30, 0, 0, -200);
        animation("fashi_binghuozoulang_02_03")
        {
            wrapmode(2);
        }; 
    };
	section(266)
	{
        
		animation("fashi_binghuozoulang_02_04"); 
    };
	oninterrupt()
	{
	};
	
	onstop()
	{
	};
};

//火焰分身
skill(130411)
{
	section(5000)
	{
        addbreaksection(11, 0, 4500);
		movecontrol(true);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        animation("fashi_huoyanfenshen_01_01")
        {
            wrapmode(2);
        }; 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_01",1000,"Bone_Root",0, false);
        areadamage(4900, 0, 1, 0, 2.4, true) 
        {
            stateimpact("kKnockDown", 13040103);
            stateimpact("kLauncher", 13040102);
            stateimpact("kDefault", 13040101);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_02_huanying02",2000,"Bone_Root",4900, false);
        destroyself(5000);
	};
	oninterrupt()
	{
		stopeffect(300);
        destroyself(0);
	};
	
	onstop()
	{
		stopeffect(300);
        destroyself(0);
	};
};

//火焰分身连线
skill(130412)
{
	section(1300)
	{
		movecontrol(true);
        animation("fashi_huoyanfenshen_01_01")
        {
            wrapmode(2);
        }; 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_01",1300,"Bone_Root",200, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_02",1300,"Bone_Root",0, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_03",2000,"Bone_Root",1000, false);
        colliderdamage(1000, 200, true, false, 200, 1)
        {
            stateimpact("kKnockDown", 13040203);
            stateimpact("kLauncher", 13040202);
            stateimpact("kDefault", 13040201); 
            bonecollider("Hero/6_fashi/huoyanfenshencollider1","Bone_Root", false);
        };
        areadamage(1200, 0, 1, 0, 2.4, true) 
        {
            stateimpact("kKnockDown", 13040103);
            stateimpact("kLauncher", 13040102);
            stateimpact("kDefault", 13040101);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_02_huanying02",2000,"Bone_Root",1200, false);
        destroyself(1300);
	};
	oninterrupt()
	{
		stopeffect(300);
        destroyself(0);
	};
	
	onstop()
	{
		stopeffect(300);
        destroyself(0);
	};
};

//火焰分身连线
skill(130413)
{
	section(1100)
	{
		movecontrol(true);
        animation("fashi_huoyanfenshen_01_01")
        {
            wrapmode(2);
        }; 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_01",1100,"Bone_Root",200, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_02",1100,"Bone_Root",0, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_03",2000,"Bone_Root",800, false);
        colliderdamage(800, 200, true, false, 200, 1)
        {
            stateimpact("kKnockDown", 13040203);
            stateimpact("kLauncher", 13040202);
            stateimpact("kDefault", 13040201); 
            bonecollider("Hero/6_fashi/huoyanfenshencollider1","Bone_Root", false);
        };
        areadamage(1000, 0, 1, 0, 2.4, true) 
        {
            stateimpact("kKnockDown", 13040103);
            stateimpact("kLauncher", 13040102);
            stateimpact("kDefault", 13040101);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_02_huanying02",2000,"Bone_Root",1000, false);
        destroyself(1100);
	};
	oninterrupt()
	{
		stopeffect(300);
        destroyself(0);
	};
	
	onstop()
	{
		stopeffect(300);
        destroyself(0);
	};
};

//火焰分身连线
skill(130414)
{
	section(900)
	{
		movecontrol(true);
        animation("fashi_huoyanfenshen_01_01")
        {
            wrapmode(2);
        }; 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_01",900,"Bone_Root",200, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_02",900,"Bone_Root",0, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_03",2000,"Bone_Root",600, false);
        colliderdamage(600, 200, true, false, 200, 1)
        {
            stateimpact("kKnockDown", 13040203);
            stateimpact("kLauncher", 13040202);
            stateimpact("kDefault", 13040201); 
            bonecollider("Hero/6_fashi/huoyanfenshencollider1","Bone_Root", false);
        };
        areadamage(800, 0, 1, 0, 2.4, true) 
        {
            stateimpact("kKnockDown", 13040103);
            stateimpact("kLauncher", 13040102);
            stateimpact("kDefault", 13040101);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_02_huanying02",2000,"Bone_Root",800, false);
        destroyself(900);
	};
	oninterrupt()
	{
		stopeffect(300);
        destroyself(0);
	};
	
	onstop()
	{
		stopeffect(300);
        destroyself(0);
	};
};

//火焰分身消失
skill(130415)
{
	section(700)
	{
		movecontrol(true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_04_01",700,"Bone_Root",0, false);
        areadamage(600, 0, 1, 0, 2.4, true) 
        {
            stateimpact("kKnockDown", 13040103);
            stateimpact("kLauncher", 13040102);
            stateimpact("kDefault", 13040101);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_huoyanfenshen_02_huanying02",2000,"Bone_Root",600, false);
        destroyself(700);
	};
	oninterrupt()
	{
		stopeffect(300);
        destroyself(0);
	};
	
	onstop()
	{
		stopeffect(300);
        destroyself(0);
	};
};
