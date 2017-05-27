

/****    格挡 四阶    ****/

skill(121341)
{
  section(1)//初始化    0
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //打断
    addbreaksection(0, 500, 30000);
    addimpacttoself(1, 88889, 500);
  };

  section(1)//起手    1
  {
/*
    animation("Cike_Skill13_01_01")
    {
      speed(4);
    };
*/
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    addimpacttoself(1, 12990003, 500);
  };

  section(30000)//第一段    2
  {
    animation("Cike_Skill13_01_02")
    {
      speed(1);
    };
    //
    //受击检测
    parrycheck(0,500,0,"onparry1", "onparry1", true);
    //
    //受击检测
    parrycheck(500,30000,0,"onparry2", "onparryFalse", true);
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_GeDang_01", 30000, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill13_GeDang_01", false);
    //
    //跳转
     gotosection(29900, 6, 1);
  };


//////////     反击     ////////////

  section(200)//起手    3
  {
    movecontrol(true);
    removebreaksection(0);
    stopeffect(0);
    //
    //目标选择
    findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, -3);
    //
    animation("Cike_Skill13_02_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, 20, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1000);
    addimpacttoself(1, 12990003, 1000);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //模型消失
    setenable(10, "Visible", false);
    //模型显示
    setenable(150, "Visible", true);
    //
  };

  section(866)//第一段    4
  {
    animation("Cike_Skill13_02_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kDefault", 12130101);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_QianZhan_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_QianZhan_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_QianZhan_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill05_ShunShenZhan_01", false);
    playsound(10, "Hit01", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Call_01", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //跳转
     gotosection(860, 6, 1);
  };

/////////////////////

  section(1100)//被击硬直    5
  {
    playsound(0, "Hitd", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill13_GeDang_01", false);
    stopeffect(0);
    animation("Cike_Skill13_01_05");
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_GeDang_03", 2000, "Bone_Root", 0);
  };

//////////////////////

  section(1)//结束    6
  {
    movecontrol(true);
  };

//////////////////////

  onmessage("onparry1")
  {
        gotosection(0, 3, 1);
  };

  onmessage("onparry2")
  {
       facetoattacker(0,2000);
       animation("Cike_Skill13_01_04");
       charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_GeDang_02", 2000, "Bone_Root", 0);
  };

  onmessage("onparryFalse")
  {
       facetoattacker(0,2000);
       gotosection(0, 5, 1);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
		parrycheckover(0);
        setenable(0, "Visible", true);
        setcross2othertime(0, "stand", 200);
        stopeffect(0);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
		parrycheckover(0);
        setenable(0, "Visible", true);
        setcross2othertime(0, "stand", 200);
        stopeffect(0);
  };
};
