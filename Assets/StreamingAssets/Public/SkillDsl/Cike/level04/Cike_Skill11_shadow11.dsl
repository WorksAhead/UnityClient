

/****    大风轮攻击    ****/

skill(125511)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0, 1.5, 0),eular(0,0,0),"RelativeOwner",false);

    setlifetime(0, 3000);

    //目标选择
		findmovetarget(0, vector3(0, 0, 1), 10, 30, 0.1, 0.9, 0, 3);
  };

  section(2000)//第一段
  {
    //
    //角色移动
    startcurvemove(0, true, 0.1, 0, 0, 50, 0, 0, 0);
    startcurvemove(100, true, 2, 0, 0, 0, 0, 0, 0);

    //
    //碰撞盒
    colliderdamage(0, 2000, true, true, 0, 1)
    {
      stateimpact("kDefault", 40050101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };

    areadamage(0, 0, 1.5, 0, 3, true)
		{
			stateimpact("kDefault", 12990002);
      //showtip(200, 0, 1, 0);
		};
    areadamage(60, 0, 1.5, 0, 3, true)
		{
			stateimpact("kDefault", 12990002);
      //showtip(200, 0, 1, 0);
		};

    //
    //伤害判定
    areadamage(200, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(260, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(320, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(380, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(440, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(500, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(560, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(620, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(680, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(740, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(800, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(860, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(920, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(980, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1040, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1100, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1160, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1220, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1280, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1340, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1400, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1460, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1520, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1580, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1640, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1700, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1760, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1820, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1880, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(1940, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    areadamage(2000, 0, 0, 0, 4.5, true)
		{
			stateimpact("kDefault", 12110102);
      //showtip(200, 0, 1, 0);
		};
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};
