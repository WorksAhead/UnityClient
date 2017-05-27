

/****    死亡莲华飞镖1    ****/

skill(4206011)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1,0),eular(0,0,0),"RelativeOwner",false, false, vector3(0, 60, 0));

    //设定生命时间
    setlifetime(0, 2000);
  };

  section(2000)//第一段
  {
    //
    //角色移动
    startcurvemove(0, true, 2, 0, 0, 20, 0, 0, 0);
    //
    //碰撞盒
    colliderdamage(0, 2000, true, true, 0, 1)
    {
      stateimpact("kDefault", 40050101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};

skill(4206012)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1,0),eular(0,144,0),"RelativeOwner",false, false, vector3(0, 60, 0));

    //设定生命时间
    setlifetime(0, 2000);
  };

  section(2000)//第一段
  {
    //
    //角色移动
    startcurvemove(0, true, 2, 0, 0, 20, 0, 0, 0);
    //
    //碰撞盒
    colliderdamage(0, 2000, true, true, 0, 1)
    {
      stateimpact("kDefault", 40050101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};

skill(4206013)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1,0),eular(0,288,0),"RelativeOwner",false, false, vector3(0, 60, 0));

    //设定生命时间
    setlifetime(0, 2000);
  };

  section(2000)//第一段
  {
    //
    //角色移动
    startcurvemove(0, true, 2, 0, 0, 20, 0, 0, 0);
    //
    //碰撞盒
    colliderdamage(0, 2000, true, true, 0, 1)
    {
      stateimpact("kDefault", 40050101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};

skill(4206014)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1,0),eular(0,432,0),"RelativeOwner",false, false, vector3(0, 60, 0));

    //设定生命时间
    setlifetime(0, 2000);
  };

  section(2000)//第一段
  {
    //
    //角色移动
    startcurvemove(0, true, 2, 0, 0, 20, 0, 0, 0);
    //
    //碰撞盒
    colliderdamage(0, 2000, true, true, 0, 1)
    {
      stateimpact("kDefault", 40050101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};

skill(4206015)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,1,0),eular(0,576,0),"RelativeOwner",false, false, vector3(0, 60, 0));

    //设定生命时间
    setlifetime(0, 2000);
  };

  section(2000)//第一段
  {
    //
    //角色移动
    startcurvemove(0, true, 2, 0, 0, 20, 0, 0, 0);
    //
    //碰撞盒
    colliderdamage(0, 2000, true, true, 0, 1)
    {
      stateimpact("kDefault", 40050101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};


