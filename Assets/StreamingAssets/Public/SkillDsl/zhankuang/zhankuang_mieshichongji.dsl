skill(161401)
{
	section(1100)
	{
		//全局参数
		addbreaksection(1, 800, 4000);
		addbreaksection(10, 800, 4000);
		addbreaksection(20, 0, 4000);
		addbreaksection(30, 800, 4000);
		movecontrol(true);
		animation("zhankuang_mieshikuangwu_01");
		setanimspeed(100, "zhankuang_mieshikuangwu_01", 1.5);
		areadamage(666, 0, 0, 0, 4, false) 
		{
			stateimpact("kDefault", 16080101);
		};
		charactereffect("Hero_FX/5_zhankuang/5_Hero_zhankuang_wumangshazhen_02_02",1000,"Bone_Root",666);
	};
	section(1100)
	{
		animation("zhankuang_mieshikuangwu_01");
		areadamage(666, 0, 0, 0, 4, false) 
		{
			stateimpact("kDefault", 16080101);
		};
		charactereffect("Hero_FX/5_zhankuang/5_Hero_zhankuang_wumangshazhen_02_02",1000,"Bone_Root",666);
	};
	section(1800)
	{
		animation("zhankuang_mieshikuangwu_01");
		//6
		setanimspeed(200, "zhankuang_mieshikuangwu_01", 0.25);
		//12
		setanimspeed(1000, "zhankuang_mieshikuangwu_01", 1);
		areadamage(1266, 0, 0, 0, 4, false) 
		{
			stateimpact("kDefault", 16080101);
		};
		charactereffect("Hero_FX/5_zhankuang/5_Hero_zhankuang_wumangshazhen_02_02",1000,"Bone_Root",1266);
	};
};
