

/****    达克尼斯飞镖    ****/

skill(420501)
{
  section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
    //
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);
  };

  section(166)//起手
  {
    animation("Skill05_01")
    {
      speed(3);
    };
    //
    //角色移动
    //startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
  };

  section(733)//第一段
  {
    animation("Skill05_02")
    {
      speed(1);
    };
    //
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/DaKeNiSi/6_Mon_DaKeNiSi_FeiDao_01", 4205011, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/DaKeNiSi/6_Mon_DaKeNiSi_FeiDao_01", 4205012, vector3(30, 0, 0));
    summonnpc(0, 101, "Monster_FX/DaKeNiSi/6_Mon_DaKeNiSi_FeiDao_01", 4205013, vector3(30, 0, 0));
    //
    summonnpc(30, 101, "Monster_FX/DaKeNiSi/6_Mon_DaKeNiSi_FeiDao_01", 4205014, vector3(0, 0, 0));
    summonnpc(30, 101, "Monster_FX/DaKeNiSi/6_Mon_DaKeNiSi_FeiDao_01", 4205015, vector3(0, 0, 0));
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/YiNa/boss_YiNa_FeiBiao_01", false);
  };

  section(1)//硬直
  {
    animation("YiNa_Skill05_01_99")
    {
      speed(1);
    };
  };
};
