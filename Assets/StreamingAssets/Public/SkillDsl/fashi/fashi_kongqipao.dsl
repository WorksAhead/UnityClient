//skill(130101)
//{
//    section(1600)
//    {
//        addbreaksection(1, 1400, 1600);
//        addbreaksection(10, 1400, 1600);
//        addbreaksection(20, 0, 1600);
//        addbreaksection(30, 1300, 1600);
//        movecontrol(true);
//        animation("fashi_kongqipao_01"); 
//        //֡4
//        setanimspeed(133, "fashi_kongqipao_01", 2);
//        //֡12
//        setanimspeed(266, "fashi_kongqipao_01", 1);
//        //֡16
//        setanimspeed(400, "fashi_kongqipao_01", 2);
//        //֡20
//        setanimspeed(466, "fashi_kongqipao_01", 1);
//        //֡22
//        setanimspeed(533, "fashi_kongqipao_01", 2);
//        //֡28
//        setanimspeed(633, "fashi_kongqipao_01", 1);
//        //֡29
//        setanimspeed(666, "fashi_kongqipao_01", 2);
//        //֡39
//        setanimspeed(833, "fashi_kongqipao_01", 1);
//        //֡44
//        setanimspeed(1000, "fashi_kongqipao_01", 2);
//        //֡50
//        setanimspeed(1100, "fashi_kongqipao_01", 1);
//        //  65
//        enablechangedir(200, 800);
//        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),233,eular(0,0,0),vector3(1,1,1),true);
//        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(0.2,0.4,0),466,eular(0,0,0),vector3(1,1,1),true);
//        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),633,eular(0,0,0),vector3(1,1,1),true);
//        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(0.2,0.4,0),766,eular(0,0,0),vector3(1,1,1),true);
//        summonnpc(233, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
//        summonnpc(466, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130112, vector3(0, 0, 0));
//        summonnpc(633, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
//        summonnpc(766, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130112, vector3(0, 0, 0));
//        summonnpc(1000, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_03", 130113, vector3(0, 0, 0));

//    };
//    oninterrupt()
//    {
//        stopeffect(300);
//    };
//    onstop()
//    {
//        stopeffect(300);
//    };
//};

skill(130101)
{
    section(466)
    {
        addbreaksection(0, 466, 3166)
        {   
            sendmessage("break");
        };
        addbreaksection(1, 3166, 3166);
        addbreaksection(10, 3166, 3166);
        addbreaksection(20, 0, 3166);
        addbreaksection(30, 3166, 3166);
        movecontrol(true);
        animation("fashi_kongqipao_01_01"); 
        //֡4
        setanimspeed(133, "fashi_kongqipao_01_01", 2);
        //֡12
        setanimspeed(266, "fashi_kongqipao_01_01", 1);
        //֡16
        setanimspeed(400, "fashi_kongqipao_01_01", 2);
        //֡20
        enablechangedir(200, 466);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",3166,"Bip001 L Forearm",0, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",3166,"Bip001 R Forearm",0, true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),233,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),466,eular(0,0,0),vector3(1,1,1),true);
        summonnpc(233, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(466, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));
        //startcurvemove(233, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(466, true, 0.05, 0, 0, -5, 0, 0, 0);
    };
    section(2100)
    {
        animation("fashi_kongqipao_01_02")
        {
            wrapmode(2);
        }; 
        enablechangedir(0, 2100);
        //֡12
        setanimspeed(66, "fashi_kongqipao_01_02", 2);
        //֡8
        setanimspeed(166, "fashi_kongqipao_01_02", 1);
        //֡9
        setanimspeed(200, "fashi_kongqipao_01_02", 2);
        //֡15
        setanimspeed(300, "fashi_kongqipao_01_02", 1);
        //֡22
        setanimspeed(366, "fashi_kongqipao_01_02", 2);
        //֡8
        setanimspeed(466, "fashi_kongqipao_01_02", 1);
        //֡9
        setanimspeed(500, "fashi_kongqipao_01_02", 2);
        //֡15
        setanimspeed(600, "fashi_kongqipao_01_02", 1);
        //֡32
        setanimspeed(666, "fashi_kongqipao_01_02", 2);
        //֡8
        setanimspeed(766, "fashi_kongqipao_01_02", 1);
        //֡9
        setanimspeed(800, "fashi_kongqipao_01_02", 2);
        //֡15
        setanimspeed(900, "fashi_kongqipao_01_02", 1);
        //֡42
        setanimspeed(966, "fashi_kongqipao_01_02", 2);
        //֡8
        setanimspeed(1066, "fashi_kongqipao_01_02", 1);
        //֡9
        setanimspeed(1100, "fashi_kongqipao_01_02", 2);
        //֡15
        setanimspeed(1200, "fashi_kongqipao_01_02", 1);
        //֡52
        setanimspeed(1266, "fashi_kongqipao_01_02", 2);
        //֡8
        setanimspeed(1366, "fashi_kongqipao_01_02", 1);
        //֡9
        setanimspeed(1400, "fashi_kongqipao_01_02", 2);
        //֡15
        setanimspeed(1500, "fashi_kongqipao_01_02", 1);
        //֡62
        setanimspeed(1566, "fashi_kongqipao_01_02", 2);
        //֡8
        setanimspeed(1666, "fashi_kongqipao_01_02", 1);
        //֡9
        setanimspeed(1700, "fashi_kongqipao_01_02", 2);
        //֡15
        setanimspeed(1800, "fashi_kongqipao_01_02", 1);
        //֡72
        setanimspeed(1866, "fashi_kongqipao_01_02", 2);
        //֡8
        setanimspeed(1966, "fashi_kongqipao_01_02", 1);
        //֡9
        setanimspeed(2000, "fashi_kongqipao_01_02", 2);
        //֡15
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),166,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),300,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),466,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),600,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),766,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),900,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),1066,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),1200,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),1366,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),1500,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),1666,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),1800,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_01",1000,vector3(-0.2,0.4,0),1966,eular(0,0,0),vector3(1,1,1),true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_09",1000,vector3(0.2,0.4,0),2100,eular(0,0,0),vector3(1,1,1),true);
        summonnpc(166, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(300, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));
        summonnpc(466, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(600, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));
        summonnpc(766, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(900, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));
        summonnpc(1066, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(1200, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));
        summonnpc(1366, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(1500, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));
        summonnpc(1666, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(1800, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));
        summonnpc(1966, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_02", 130111, vector3(0, 0, 0));
        summonnpc(2100, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_10", 130112, vector3(0, 0, 0));  
        //startcurvemove(166, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(300, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(466, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(600, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(766, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(900, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(1066, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(1200, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(1366, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(1500, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(1666, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(1800, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(1966, true, 0.05, 0, 0, -5, 0, 0, 0);
        //startcurvemove(2100, true, 0.05, 0, 0, -5, 0, 0, 0);
    };
    section(600)
    {
        removebreaksection(0);
        animation("fashi_kongqipao_01_03"); 
        //֡1
        setanimspeed(33, "fashi_kongqipao_01_03", 2);
        //֡9
        setanimspeed(166, "fashi_kongqipao_01_03", 1);
        //֡11
        setanimspeed(233, "fashi_kongqipao_01_03", 2);
        //֡15
        setanimspeed(300, "fashi_kongqipao_01_03", 1);
        //  30
          summonnpc(200, 101, "Hero/6_fashi/base", 130113, vector3(0, 0, 0));

    };
    onmessage("break")
    {
         removebreaksection(0);
         gotosection(0, 2, 0);
    };
    oninterrupt()
    {
		stopeffect(100);
        removebreaksection(0);
        gotosection(0, 2, 0);
    };
    onstop()
    {
		stopeffect(100);
    };
};

skill(130102)
{
	section(2166)
	{
        addbreaksection(1, 1200, 2166);
        addbreaksection(10, 1200, 2166);
        addbreaksection(20, 0, 2166);
        addbreaksection(30, 1200, 2166);
		movecontrol(true);
        enablechangedir(200, 500);
		animation("fashi_kongqipao_02");
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",1400,"Bip001 L Forearm",0, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_youshou_02",1400,"Bip001 R Forearm",0, true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_06",1000,vector3(0,3.5,0),100,eular(0,0,0),vector3(1,1,1),true);
        summonnpc(600, 101, "Hero_FX/6_Fashi/6_hero_fashi_kongqipao_07", 130121, vector3(0, 3.5, 0));
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

//空气炮:左1
skill(130111)
{
    section(1050)
    {
        movecontrol(true);
        settransform(0," ",vector3(-0.4,1.75,0.8),eular(0,0,0),"RelativeOwner",false); 
        findmovetarget(20, vector3(0, 0, 0), 10, 30, 0.1, 0.9, 0, 20, true)
        {
            filtstate("kKnockDown");
        };
        startcurvemove(50, true, 1,0,0,30,0,0,0);
        colliderdamage(0, 1000, false, false, 0, 0)
        {
            stateimpact("kKnockDown", 13010103);
            stateimpact("kLauncher", 13010102);
            stateimpact("kDefault", 13010101);
            oncollidelayer("Terrains", "onterrain");
            oncollidelayer("Default", "onterrain");
            boneboxcollider(vector3(0.8, 0.8, 0.8), "paoti", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(1000);
    };
    onmessage("onterrain")
    {
        setenable(20, "CurveMove", false);
        setenable(20, "Rotate", false);
        setenable(10, "Damage", false);
        destroyself(50);
        //sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_02", 1000, vector3(0,0,0));
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

//空气炮:右1
skill(130112)
{
    section(1050)
    {
        movecontrol(true);
        settransform(0," ",vector3(0.4,1.75,0.8),eular(0,0,0),"RelativeOwner",false); 
        findmovetarget(20, vector3(0, 0, 0), 10, 30, 0.1, 0.9, 0, 20, true)
        {
            filtstate("kKnockDown");
        };
        startcurvemove(50, true, 1,0,0,30,0,0,0);
        colliderdamage(0, 1000, false, false, 0, 0)
        {
            stateimpact("kKnockDown", 13010103);
            stateimpact("kLauncher", 13010102);
            stateimpact("kDefault", 13010101);
            oncollidelayer("Terrains", "onterrain");
            oncollidelayer("Default", "onterrain");
            boneboxcollider(vector3(0.8, 0.8, 0.8), "paoti", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(1000);
    };
    onmessage("onterrain")
    {
        setenable(20, "CurveMove", false);
        setenable(20, "Rotate", false);
        setenable(10, "Damage", false);
        destroyself(50);
        //sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_02", 1000, vector3(0,0,0));
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

//空气炮:瞬发中型
skill(130113)
{
    section(100)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
        findmovetarget(10, vector3(0, 0, 0), 3.5, 45, 0.1, 0.9, 0, 0, false);
        gotosection(20, 1, 0) 
        {
            targetcondition(true);
        };
        findmovetarget(50, vector3(0, 0, 0), 6.5, 45, 0.1, 0.9, 0, 0, false);
        gotosection(60, 2, 0) 
        {
            targetcondition(true);
        };
        findmovetarget(90, vector3(0, 0, 0), 10, 45, 0.1, 0.9, 0, 0, false);
        gotosection(100, 3, 0)
        {
            targetcondition(true);
        };
    };
    section(1200)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,3),eular(0,0,0),"RelativeSelf",false); 
        areadamage(50, 0, 0, 0, 3, true) 
        {
            stateimpact("kKnockDown", 13010113);
            stateimpact("kLauncher", 13010112);
            stateimpact("kDefault", 13010111);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_03",1500,vector3(0,0,0),50);
        gotosection(1200, 4, 0);
    };
    section(1200)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,5),eular(0,0,0),"RelativeSelf",false); 
        areadamage(50, 0, 0, 0, 3, true) 
        {
            stateimpact("kKnockDown", 13010113);
            stateimpact("kLauncher", 13010112);
            stateimpact("kDefault", 13010111);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_03",1500,vector3(0,0,0),50);
        gotosection(1200, 4, 0);
    };
    section(1200)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,0,7),eular(0,0,0),"RelativeSelf",false); 
        areadamage(50, 0, 0, 0, 3, true) 
        {
            stateimpact("kKnockDown", 13010113);
            stateimpact("kLauncher", 13010112);
            stateimpact("kDefault", 13010111);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_03",1500,vector3(0,0,0),50);
        gotosection(1200, 4, 0);
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

//空气炮:超大
skill(130121)
{
    section(200)
    {
        movecontrol(true);
        settransform(0," ",vector3(0,3.5,0),eular(0,0,0),"RelativeOwner",false);
        findmovetarget(100, vector3(0, 0, 0), 3.5, 45, 0.1, 0.9, 0, 0, false);
        gotosection(110, 1, 0)
        {
            targetcondition(true);
        };
        findmovetarget(150, vector3(0, 0, 0), 6.5, 45, 0.1, 0.9, 0, 0, false);
        gotosection(160, 2, 0)
        {
            targetcondition(true);
        };
        findmovetarget(190, vector3(0, 0, 0), 10, 45, 0.1, 0.9, 0, 0, false);
        gotosection(200, 3, 0)
        {
            targetcondition(true);
        };
    };
    section(870)
    {
        startcurvemove(50, true, 0.8,0,11.25,3.75,0,-37.5,0);
        areadamage(850, 0, 0, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13010203);
            stateimpact("kLauncher", 13010202);
            stateimpact("kDefault", 13010201);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_05",3000,vector3(0,-0.5,0),850);
        gotosection(870, 4, 0);
    };
    section(870)
    {
        startcurvemove(50, true, 0.8,0,11.25,6.25,0,-37.5,0);
        areadamage(850, 0, 0, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13010203);
            stateimpact("kLauncher", 13010202);
            stateimpact("kDefault", 13010201);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_05",3000,vector3(0,-0.5,0),850);
        gotosection(870, 4, 0);
    };
    section(870)
    {
        startcurvemove(50, true, 0.8,0,11.25,8.75,0,-37.5,0);
        areadamage(850, 0, 0, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13010203);
            stateimpact("kLauncher", 13010202);
            stateimpact("kDefault", 13010201);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_kongqipao_05",3000,vector3(0,-0.5,0),850);
        gotosection(870, 4, 0);
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