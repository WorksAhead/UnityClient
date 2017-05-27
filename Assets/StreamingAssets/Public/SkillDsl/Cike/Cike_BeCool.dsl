skill(120005)
{
	section(30000)
	{
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器

		animation("Cike_Show_01");
		movecontrol(true);

		storepos(0);
		settransform(1, "", vector3(0, 0, 0), eular(0, 180, 0), "RelativeWorld", false);
		restorepos(2);
    setenable(0,  "CameraFollow", false);

		//movecamera(0.06, true, 0.56, 0, -18, 8, 0, 0, 0);
		movecamera(0, true, 0.56, 0, -18, 10, 0, 0, 0);
		movecamera(566, false,  0.1, 10, 10, 0, 0, 0, 0);
		movecamera(666, false,  0.33, 0, -3, 0, 0, 0, 0);
		movecamera(1000, false,  0.1, 0, 10, 0, 0, 0, 0);
		movecamera(1100, false,  0.33, 0, -3, 0, 0, 0, 0);

    //1066
    //
    //模型消失
    setenable(1066, "Visible", false);
    //模型显示
    setenable(2000, "Visible", true);
    //
		animation("Cike_Show_02", 2000);



    //主角位置修正1
    startcurvemove(566, true, 0.1, 0, 0, -10, 0, 0, 0);
    startcurvemove(666, true, 0.1, 0, 0, -10, 0, 0, 0);

    //主角位置修正2
    startcurvemove(1200, true, 0.5, -0.8, 1.7, 3, 0, 0, 0);
    settransform(1200, " ", vector3(0, 0, 0), eular(0, -15, 0), "RelativeSelf", true);



		movecamera(1400, false, 0.6, 5, 0, 6, 0, 0, 0);


		movecamera(2000, false, 0.1, -30, 0, 0, 0, 0, 0);

    blackscene(2000, "Hero/3_Cike/BlackScene", 0.8, 200, 20000, 200, "Character");

    setenable(29000,  "CameraFollow", true);
  };

  oninterrupt()
	{
    setenable(0,  "CameraFollow", true);
    setenable(0, "Visible", true);
	};

  onstop()
	{
    setenable(0,  "CameraFollow", true);
    setenable(0, "Visible", true);
	};
};