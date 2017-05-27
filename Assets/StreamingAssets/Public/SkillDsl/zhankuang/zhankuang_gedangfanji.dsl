skill(161701)
{
	//格挡起始
	section(300)
	{
		//格挡开始
		addbreaksection(0, 300, 100000)
        {
            sendmessage("break");
        };
		addbreaksection(1, 100000, 100000);
		addbreaksection(10, 100000, 100000);
		addbreaksection(20, 300, 100000);
		addbreaksection(30, 300, 100000);
        addimpacttoself(0, 88889);
		movecontrol(true);
		animation("zhankuang_gedang_01");
		parrycheck(0,300,0,"onparrystart","onparrystart",true);
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_gedangfanji_01", 300, "Bone_Root", 0, false);
        playsound(0, "skill1701", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_gedang_release", false);		
	};
	//格挡过程
	section(100000)
	{
		//格挡持续
        stopeffect(0);
		movecontrol(true);
		animation("zhankuang_gedang_02")
		{
			wrapmode(4);
		};
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_gedangfanji_02", 100000, "Bone_Root", 0, false);
		parrycheck(0,100000,0,"onparrytrue","onparryfalse",true);
		gotosection(99999, 5, 1);
	};
	//格挡反击
	section(500)
	{   
        stopeffect(0);
		removebreaksection(0);
		removebreaksection(1);
		removebreaksection(10);
		removebreaksection(20);
		removebreaksection(30);
        timescale(0, 0.1, 100);
		facetoattacker(0,1000);
        setcamerafollowspeed(0, 50, 0.5, 50, 2, 1);
        resetcamerafollowspeed(200);
        findmovetarget(20, vector3(0, 0, 0), 10, 30, 0.01, 0.99, 0, -0.7, true);
        shakecamera2(0, 100, false, false, vector3(0.4, 0, 0), vector3(50, 0, 0), vector3(32, 0, 0), vector3(90, 0, 0));
        startcurvemove(50, true, 0.05,0,0,30,0,0,0);
		animation("zhankuang_gedang_04");
        playsound(0, "skill1702", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_gedang_shove", false);
        playsound(200, "skill1703", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_gedang_attack", false);
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_gedangfanji_05", 1000, "Bone_Root", 0, false);
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_gedangfanji_06", 1000, "Bone_Root", 200, false);
        parrycheck(0,500,0,"","",true);
        parrycheckover(500);
		gotosection(500, 6, 1);
        areadamage(200, 0, 1, 1, 3, true)
		{
			stateimpact("kDefault", 16170102);
		};
	};
	//格挡受击
	section(400)
	{
        stopeffect(0);
		animation("zhankuang_gedang_06");
        shakecamera2(0, 200, false, false, vector3(0.4, 0, 0), vector3(100, 0, 0), vector3(32, 0, 0), vector3(90, 0, 0));
		facetoattacker(0,1000);
		parrycheck(0,400,0,"onparrytrue","onparryfalse",true);
        playsound(0, "skill1704", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_gedang_success", false);
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_gedangfanji_02", 400, "Bone_Root", 0, false);
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_gedangfanji_03", 1000, "Bone_Root", 0, false);
		gotosection(400, 1, 1);
	};
	//格挡被攻破
	section(1000)
	{
        stopeffect(0);
		parrycheckover(0);
		removebreaksection(0);
		removebreaksection(1);
		removebreaksection(10);
		removebreaksection(20);
		removebreaksection(30);
        animation("zhankuang_gedang_05");
        //֡15
        setanimspeed(500, "zhankuang_gedang_05", 0.5);
        //  23
        facetoattacker(0,2000);
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_gedangfanji_04", 1000, "Bone_Root", 0, false);
        startcurvemove(300, true, 0.1,0,0,0,0,0,-400, 0.1,0,0,-40,0,0,400);
        playsound(0, "skill1705", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_gedang_fail", false);
        shakecamera2(0, 400, false, false, vector3(0.8, 0, 0), vector3(133, 0, 0), vector3(64, 0, 0), vector3(90, 0, 0));
		facetoattacker(0,1000);
		gotosection(1000, 6, 1);
	};
	//格挡结束
	section(500)
	{
        parrycheckover(0);
        removebreaksection(0);
        stopeffect(0);
		animation("zhankuang_gedang_03");
        //֡1
        setanimspeed(33, "zhankuang_gedang_03", 0.5);
        //֡7
        setanimspeed(433, "zhankuang_gedang_03", 1);
        //  9
	};
	//技能结束
	section(1)
	{   
        parrycheckover(0);
        resetcamerafollowspeed(0);
	};
	onmessage("onparrystart")
	{
		gotosection(0, 2, 1);
	};
	onmessage("onparrytrue")
	{
		gotosection(0, 3, 1);
	};
	onmessage("onparryfalse")
	{
		gotosection(0, 4, 1);
	};
    onmessage("break")
    {
        gotosection(0, 5, 1);
    };
	oninterrupt()
	{
		parrycheckover(0);
        stopeffect(0);
		gotosection(0, 5, 1);
        resetcamerafollowspeed(0);
	};
	onstop()
	{
		parrycheckover(0);
        stopeffect(0);
        resetcamerafollowspeed(0);
	};
};

//格挡反击
skill(161702)
{
	section(700)
	{
		//全局参数
		addbreaksection(1, 600, 700);
		addbreaksection(10, 600, 700);
		addbreaksection(20, 600, 700);
		addbreaksection(30, 600, 700);
		movecontrol(true);
		animation("zhankuang_gedang_05");

		addimpacttoself(0, 16170299, 700);
		addimpacttoself(0, 16170104, 1000);

		areadamage(462, 0, 1, 1, 2.5, true) 
		{
			stateimpact("kDefault", 32020201);
		};
		
		shakecamera2(470, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,60));
	};
};