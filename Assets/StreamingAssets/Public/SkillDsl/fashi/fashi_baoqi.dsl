skill(131301)
{   
	section(866)
	{
		addbreaksection(1, 500, 866);
		addbreaksection(10, 500, 866);
		addbreaksection(20, 300, 866);
		addbreaksection(30, 500, 866);
		movecontrol(true);
        addimpacttoself(0, 13130102);
		animation("fashi_baoqi_01");
        areadamage(166, 0, 1, 0, 4, false) 
        {
            stateimpact("kDefault", 13130101);
        };
        startcurvemove(0, false, 0.1, 0, -120, 0, 0, 0, 0);
        charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_BaoQi_01", 1000, "Bone_Root", 0, true);
        playsound(0, "skill0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/ZK_Voice_FengKuangLianZhan_02", false);
		shakecamera2(150,200,false,false,vector3(0.5,0.5,0.5),vector3(50,50,50),vector3(5,5,5),vector3(85,85,85));	
	};
	oninterrupt()
	{
        addimpacttoself(0, 88889);
        stopeffect(0);
	};
	onstop()
	{
        addimpacttoself(0, 88889);
        stopeffect(0);
	};
};
