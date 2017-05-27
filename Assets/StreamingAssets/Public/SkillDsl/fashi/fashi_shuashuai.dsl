skill(130005)
{
	section(500000)
	{
		addbreaksection(1, 500, 500000);
		addbreaksection(10, 500, 500000);
		addbreaksection(30, 500, 500000);
		movecontrol(true);
		animation("fashi_shenglongquan_01");
		storepos(0);
		settransform(1, "", vector3(0, 0, 0), eular(0, 220, 0), "RelativeWorld", false);
		restorepos(2);
		movecamera(0, true,  0.4, 0, -25, 16, 0, 0, 0, 0.1, 0, 0, 0, 0, 0, 0);
		movecamera(450, false,  0.2, 20, 0, 0, -120, 0, 0, 0.1, 0, 0, 0, 0, 0, 0, 0.2, -30, 0, 0, 180, 0, 0, 0.15, 0, 0, 0, 0, 0, 0);
		movecamera(1100, true,  0.2, 0, -4, -27.5, 0, 0, 0, 0.3, -3.3, -4, 25.7, 0, 0, 0, 4990, 0, 0, 0, 0, 0, 0);	
		cullingmask(0,500000,"Monster","Player");
		//rotatecamera(0, 200, vector3(-165, 0, 0));
		//rotatecamera(200, 499800, vector3(0, 0, 0));
  };
  oninterrupt()
  {
  };
};