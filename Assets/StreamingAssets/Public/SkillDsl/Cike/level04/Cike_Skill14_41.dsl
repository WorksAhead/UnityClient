

/****    爆气    ****/

skill(121441)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(166)//起手
  {
    animation("Cike_Skill14_01_01")
    {
      speed(1);
    };
    //自身增加解除控制buff
    addimpacttoself(1, 12990003, 500);
    addimpacttoself(1, 88889, 500);
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 3000);
    //
    //角色移动
    startcurvemove(0, true, 0.13, 0, 2, 0, 0, 0, 0);
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_JuGuai", 2000, "Bone_Root", 1);
  };

  section(300)//第一段
  {
    animation("Cike_Skill14_01_02")
    {
      speed(1);
    };
    //角色移动
    startcurvemove(0, true, 0.3, 0, 0, 0, 0, -50, 0);
    //
    //伤害判定
    areadamage(10, 0, 0, 0, 4, true)
    {
        stateimpact("kDefault", 12140101);
        //showtip(200, 0, 1, 0);
    };
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_BaoQi_01", 2000, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill03_FengRenZhan_01", false);
    //
    //震屏
    shakecamera2(20, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(666)//硬直
  {
    animation("Cike_Skill14_01_03")
    {
      speed(1);
    };
    //角色移动
    startcurvemove(0, true, 0.7, 0, 0, 0, 0, -50, 0);
    //
    //打断
    addbreaksection(1, 400, 3000);
    addbreaksection(10, 400, 3000);
    addbreaksection(21, 400, 3000;);
    addbreaksection(100, 400, 3000;);
  };
};
