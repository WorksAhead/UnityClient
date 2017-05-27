

/****    瞬身二段 四阶    ****/

skill(120442)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //自身增加霸体buff
    addimpacttoself(0, 12990001, 1000);
    addimpacttoself(0, 12990003, 1000);
  };

  section(600)//第一段
  {
    animation("Cike_Skill04_02_01")
    {
      speed(0.5);
    };
    //
    //召唤NPC
    summonnpc(0, 101, "Hero/3_Cike/3_Cike_02", 125301, vector3(0, 0, 0));
    //
    //角色移动
    startcurvemove(0, true, 0.6, 0, 10, 27, 0, -50, -50);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 400);
    //

    summonnpc(100, 101, "Hero/3_Cike/3_Cike_02", 125302, vector3(0, 0, 0));
    summonnpc(200, 101, "Hero/3_Cike/3_Cike_02", 125303, vector3(0, 0, 0));
    summonnpc(300, 101, "Hero/3_Cike/3_Cike_02", 125304, vector3(0, 0, 0));
    summonnpc(400, 101, "Hero/3_Cike/3_Cike_02", 125305, vector3(0, 0, 0));

    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, vector3(0, 0, 0), 80, eular(0, 0, 0), vector3(1, 1, 1), true);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, "Bone_Root", 80);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill04_ShunShen_01", false);

    playsound(20, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Voice_ShunShen_03", false)
    {
	    audiogroup("Sound/Cike/Cike_Voice_ShunShen_04");
    };
    //打断
    addbreaksection(1, 500, 1000);
    addbreaksection(10, 500, 1000);
    addbreaksection(11, 500, 1000);
    addbreaksection(21, 500, 1000);
    addbreaksection(100, 500, 1000);
  };

  section(133)//收招
  {
    animation("Cike_Skill04_02_99")
    {
      speed(1);
    };
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };
};
