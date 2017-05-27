

/****    大飞镖一段    ****/

skill(121141)
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

  section(366)//第一段
  {
    animation("Cike_Skill11_01_01")
    {
      speed(1);
    };
    //
    //召唤飞镖
    summonnpc(50, 101, "Hero_FX/3_Cike/3_Hero_Cike_FeiBiao_01", 125501, vector3(0, 0, 0));
    summonnpc(100, 101, "Hero_FX/3_Cike/3_Hero_Cike_FeiBiao_01", 125502, vector3(0, 0, 0));
    summonnpc(150, 101, "Hero_FX/3_Cike/3_Hero_Cike_FeiBiao_01", 125503, vector3(0, 0, 0));
  };

  section(733)//第二段
  {
    animation("Cike_Skill11_01_02")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.3, 0, 0, -8, 0, 0, 0);
    startcurvemove(300, true, 0.2, 0, 0, -5, 0, 0, 0);
    startcurvemove(500, true, 0.1, 0, 0, -2, 0, 0, 0);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //召唤飞镖
    summonnpc(0, 101, "Hero_FX/3_Cike/3_Hero_Cike_FeiBiao_01", 125504, vector3(0, 0, 0));
    summonnpc(50, 101, "Hero_FX/3_Cike/3_Hero_Cike_FeiBiao_01", 125505, vector3(0, 0, 0));
    summonnpc(100, 101, "Hero_FX/3_Cike/3_Hero_Cike_FeiBiao_01", 125506, vector3(0, 0, 0));
  };


  section(566)//硬直
  {
    animation("Cike_Skill11_01_03")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.05, 0, 0, -30, 0, 0, 0);
    startcurvemove(50, true, 0.1, 0, 0, -5, 0, 0, 0);
    startcurvemove(150, true, 0.1, 0, 0, -2, 0, 0, 0);
    //
    //召唤大飞镖
    summonnpc(0, 101, "Hero_FX/3_Cike/3_Hero_Cike_FeiBiaoDa_01", 125511, vector3(0, 0, 0));
  };

  section(166)//收招
  {
    animation("Cike_Skill11_01_99")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 1, 2000);
    addbreaksection(10, 1, 2000);
    addbreaksection(21, 1, 2000);
    addbreaksection(100, 1, 2000);
    //
  };
};
