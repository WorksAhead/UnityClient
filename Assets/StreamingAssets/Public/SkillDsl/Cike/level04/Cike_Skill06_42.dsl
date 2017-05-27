

/****    影袭二段 四阶    ****/

skill(120642)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
    //
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);
  };

  section(166)//起手
  {
    animation("Cike_Skill06_02_01")
    {
      speed(1);
    };
  };

  section(167)//第一段
  {
    animation("Cike_Skill06_02_02")
    {
      speed(2);
    };
    //
    //召唤NPC
    summonnpc(10, 101, "Hero/3_Cike/3_Cike_02", 125102, vector3(0, 0, 0));
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_001", 500, "Bone_Root", 1);
  };

  section(66)//第二段
  {
    animation("Cike_Skill06_02_03")
    {
      speed(1);
    };
    //
    //召唤NPC
    summonnpc(10, 101, "Hero/3_Cike/3_Cike_02", 125103, vector3(0, 0, 0));
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_002", 500, "Bone_Root", 1);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(66)//硬直
  {
    animation("Cike_Skill06_02_04")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 66, 2000);
    addbreaksection(10, 66, 2000);
    addbreaksection(21, 66, 2000);
    addbreaksection(100, 66, 2000);
  };
  /*
  section(433)//收招
  {
    animation("Cike_Skill06_01_99")
    {
      speed(1);
    };
    */
    //
  };
};
