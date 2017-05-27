skill(130501)
{
	section(466)
	{
        addbreaksection(0, 466, 1766)
        {   
            sendmessage("break");
        };
        addbreaksection(1, 1466, 1766);
        addbreaksection(10, 1466, 1766);
        addbreaksection(20, 0, 1766);
        addbreaksection(30, 1366, 1766);
		movecontrol(true);
		animation("fashi_bingqiang_01_01"); 
        //֡5
        setanimspeed(166, "fashi_bingqiang_01_01", 2);
        //֡23
        summonnpc(150, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130511, vector3(1.27,2.95,0))
        {
            signforskill(511);
        };
        summonnpc(250, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130512, vector3(-4.19,1.21,0))
        {
            signforskill(512);
        };
        summonnpc(350, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130513, vector3(0.84,2.35,0))
        {
            signforskill(513);
        };
        summonnpc(450, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130514, vector3(-1.98,1.38,0))
        {
            signforskill(514);
        };
        enablechangedir(0, 100);
	};
    section(900)
    {
        
        animation("fashi_bingqiang_01_02")
        {
            speed(2);
            wrapmode(2);
        }; 
        summonnpc(50, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130515, vector3(3.36,2.39,0))
        {
            signforskill(515);
        };
        summonnpc(150, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130516, vector3(-0.65,2.85,0))
        {
            signforskill(516);
        };
        summonnpc(250, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130517, vector3(1.64,2.13,0))
        {
            signforskill(517);
        };
        summonnpc(350, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130518, vector3(-4.33,1.97,0))
        {
            signforskill(518);
        };
        summonnpc(450, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130519, vector3(0.7,1.98,0))
        {
            signforskill(519);
        };
        summonnpc(550, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130520, vector3(-3.49,1.76,0))
        {
            signforskill(520);
        };
        summonnpc(600, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130521, vector3(3.4,1.06,0))
        {
            signforskill(521);
        };
        summonnpc(650, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130522, vector3(-4.19,2.12,0))
        {
            signforskill(522);
        };
        summonnpc(700, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130523, vector3(1.31,2.03,0))
        {
            signforskill(523);
        };
        summonnpc(750, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130524, vector3(-3.95,1.08,0))
        {
            signforskill(524);
        };
        summonnpc(800, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130525, vector3(2.91,1.69,0))
        {
            signforskill(525);
        };
        summonnpc(850, 101, "Hero_FX/6_Fashi/6_hero_fashi_bingqiang_03", 130526, vector3(-3.17,1.46,0))
        {
            signforskill(526);
        };
    }; 
    section(400)
    {//攒满
        removebreaksection(0);
		animation("fashi_bingqiang_01_03"); 
        //֡12
        gotosection(400, 4, 0);
    };
    section(100)
    {//攒未满释放
        removebreaksection(0);
		stopeffect(0);
		
        gotosection(100, 4, 0);
    };
    section(1)
    {
    };
    onmessage("break")
    {
         removebreaksection(0);
         gotosection(0, 3, 0);
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

skill(130502)
{
	section(500)
	{
		movecontrol(true);
		animation("fashi_bingqiang_02"); 
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

//冰枪
skill(130511)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(1.27,2.95,0),eular(0,-8.17,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.37,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130512)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-4.19,1.21,0),eular(0,49.1,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.15,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130513)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(0.84,2.35,0),eular(0,-6.79,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.29,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130514)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-1.98,1.38,0),eular(0,25.56,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.17,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130515)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(3.36,2.39,0),eular(0,-25.11,0),"RelativeOwner",false); 
        startcurvemove(1500, true,0.30,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130516)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-0.65,2.85,0),eular(0,4.35,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.36,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130517)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(1.64,2.13,0),eular(0,-14.39,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.27,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130518)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-4.33,1.97,0),eular(0,36.23,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.25,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130519)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(0.7,1.98,0),eular(0,-6.72,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.25,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130520)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-3.49,1.76,0),eular(0,33.46,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.22,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130521)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(3.4,1.06,0),eular(0,-46.92,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.13,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130522)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-4.19,2.12,0),eular(0,33.38,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.27,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130523)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(1.31,2.03,0),eular(0,-12.14,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.25,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130524)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-3.95,1.08,0),eular(0,50.64,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.14,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130525)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(2.91,1.69,0),eular(0,-29.85,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.21,0,-15,60,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

//冰枪
skill(130526)
{
    section(5500)
    {
        addbreaksection(11, 0, 5500);
        movecontrol(true);
        settransform(0," ",vector3(-3.17,1.46,0),eular(0,35.89,0),"RelativeOwner",false); 
        startcurvemove(1500, true, 0.18,0,-7,30,0,0,0);
        colliderdamage(1500, 300, false, false, 0, 0)
        {
            stateimpact("kDefault", 13050101);
            boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
        };
        destroyself(5500);
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

