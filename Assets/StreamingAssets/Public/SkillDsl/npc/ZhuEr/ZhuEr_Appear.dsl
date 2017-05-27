
//出场
skill(430000)
{
	section(500)
	{
		addimpacttoself(0, 38032199);
		movecontrol(true);
		animation("Cike_Jump_01")
		{
			wrapmode(2);
			speed(0.2);
		};

		settransform(0, " ", vector3(-10, 1, -10), eular(0, 0, 0), "RelativeSelf", false);
		startcurvemove(0, true, 0.5, 18, 30, 16.5, 0, -120, 0);
	};
	section(500)
	{
		startcurvemove(0, true, 0.5, 0, -30, 0, 0, 0, 0);

		sceneeffect("Monster_FX/Campaign_Wild/07_OEOrc/6_Mon_OEOrc_ZhenDangBo_01",2500,vector3(0,0,0),100);
		
		movecontrol(true);
		animation("Cike_Jump_02");
	};
	oninterrupt()
	{
	};

	onstop()
	{
	};
};
