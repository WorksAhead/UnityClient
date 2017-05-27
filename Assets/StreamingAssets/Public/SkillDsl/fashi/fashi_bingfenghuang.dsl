skill(130701)
{
	section(1133)
	{
        addbreaksection(0, 100, 700)
        {   
            sendmessage("break");
        };
        addbreaksection(0, 800, 1133);
        addbreaksection(1, 800, 1133);
        addbreaksection(10, 800, 1133);
        addbreaksection(20, 0, 1133);
        addbreaksection(30, 800, 1133);
		movecontrol(true);
		animation("fashi_bingfenghuang_01"); 
		//֡35
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_pugong_zuoshou_02",4000,"Bip001 L Forearm",1500, true);
        summonnpc(0, 113, "Hero/6_fashi/6_fenghuang_01", 130711, vector3(0, 0, -3))
        {
            signforskill(701);
        };
        summonnpc(750, 114, "Hero/6_fashi/base_ef", 130714, vector3(0, 0, 7))
        {
            signforskill(702);
        };  
	};
    onmessage("break")
    {
         summonnpc(0, 101, "Hero/6_fashi/6_fenghuang_01", 130713, vector3(0, 0, -3));  
         sonreleaseskill(0,701,130712);
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

skill(130702)
{
	section(866)
	{
        addbreaksection(1, 400, 866);
        addbreaksection(10, 400, 866);
        addbreaksection(20, 0, 866);
        addbreaksection(30, 400, 866);
		movecontrol(true);
		animation("fashi_bingfenghuang_02"); 
        sonreleaseskill(100,702,130716);
        findmovetarget(0, vector3(0, 0, 0), 12, 360, 0.5, 0.5, -1, 6, false,"sons")
        {
             signforskill(702,false);
        };
        facetotarget(50, 400);
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

//召唤物：冰凤凰
skill(130711)
{
    section(750)
    {
        addbreaksection(11, 0, 700);
        movecontrol(true);
        settransform(0," ",vector3(0,0,-3),eular(0,0,0),"RelativeOwner",false); 
		animation("fenghuang_zhunbei_01")
        {
            speed(2);
        };
    }; 
    section(400)
    {
       movecontrol(true);
		animation("fenghuang_chongci_01"); 
        colliderdamage(0, 400, true, false, 100, 4)
        {
            stateimpact("kKnockDown", 13070103);
            stateimpact("kLauncher", 13070102);
            stateimpact("kDefault", 13070101);
            bonecollider("Hero/6_fashi/bingfenghuangcollider1","Bone007", true);
        };
		startcurvemove(0, true, 0.4, 0, 0, 25, 0, 0, 0);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_04",400,"Bone007",0, true);
        destroyself(400);
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


//冰凤凰消失
skill(130712)
{
    section(10)
    {
        movecontrol(true);
        destroyself(10);
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


//冰凤凰冲刺
skill(130713)
{ 
    section(700)
    {
       movecontrol(true);
        settransform(0," ",vector3(0,0,-3),eular(0,0,0),"RelativeOwner",false); 
		animation("fenghuang_chongci_01"); 
        colliderdamage(0, 700, true, false, 100, 7)
        {
            stateimpact("kKnockDown", 13070103);
            stateimpact("kLauncher", 13070102);
            stateimpact("kDefault", 13070101);
            bonecollider("Hero/6_fashi/bingfenghuangcollider1","Bone007", true);
        };
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_04",700,"Bone007",0, true);
		startcurvemove(0, true, 0.4, 0, 0, 25, 0, 0, 0, 0.3, 0, 0, 25, 0, 120, 0);
        destroyself(700);
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


//召唤物：冰蛋
skill(130714)
{
    section(5000)
    {
        addbreaksection(11, 0, 4500);
        movecontrol(true);
        settransform(0," ",vector3(0,0,7),eular(0,0,0),"RelativeOwner",false); 
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_01",1500,"ef_base",400, false);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_02",4500,"ef_base",400, true);
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_03",1500,"ef_base",4900, false);
        areadamage(400, 0, 1, 0, 6, true) 
        {
            stateimpact("kKnockDown", 13070203);
            stateimpact("kLauncher", 13070202);
            stateimpact("kDefault", 13070201);
        };
        areadamage(4900, 0, 1, 0, 3, true) 
        {
            stateimpact("kKnockDown", 13070213);
            stateimpact("kLauncher", 13070212);
            stateimpact("kDefault", 13070211);
        };
        destroyself(5000);
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

//冰凤凰旋转
skill(130715)
{
    section(1800)
    {
        movecontrol(true);
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_04",2000,vector3(0,0,0),0);
        
        settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false); 
		animation("fenghuang_xuanzhuan_01"); 
        areadamage(0, 0, 1, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13070113);
            stateimpact("kLauncher", 13070112);
            stateimpact("kDefault", 13070111);
        };
        areadamage(1700, 0, 1, 0, 4, true) 
        {
            stateimpact("kKnockDown", 13070123);
            stateimpact("kLauncher", 13070122);
            stateimpact("kDefault", 13070121);
        };
        sceneeffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_06",1500,vector3(0,0,0),1700);
        destroyself(1800);
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

//冰蛋出生
skill(130716)
{
    section(100)
    {
        movecontrol(true);
        summonnpc(0, 101, "Hero/6_fashi/6_fenghuang_01", 130715, vector3(0, 0, 0));  
        charactereffect("Hero_FX/6_Fashi/6_hero_fashi_bingfenghuang_05",1500,"ef_base",0, false);
        destroyself(100);
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
