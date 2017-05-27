

/****    伊娜防御    ****/

skill(400601)
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
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 2500);
    addimpacttoself(0, 40000003, 2500);
    addimpacttoself(0, 40000002, 2500);
  };

  section(100)//起手
  {
    animation("YiNa_Skill06_01_01")
    {
      speed(1);
    };
  };

  section(2000)//第一段
  {
    addimpacttoself(0, 40000003, 2500);
    addimpacttoself(0, 40000002, 2500);
    animation("YiNa_Skill06_01_02")
    {
      speed(0.4);
    };
    //
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 0, 0, -30, 0);
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 1);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 700);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 1400);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill13_GeDang_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//硬直
  {
    animation("YiNa_Skill06_01_99")
    {
      speed(1);
    };
  };
};

/****    伊娜防御2秒反击    ****/

skill(4006011)
{
  section(1)//初始化 0
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
    //
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 2500);
    addimpacttoself(0, 40000003, 200);
    addimpacttoself(0, 40000002, 200);
  };

  section(100)//起手 1
  {
    animation("YiNa_Skill06_01_01");
  };

  section(2000)//第一段 2
  {
    //受击检测
    parrycheck(1,1900,0,"onparry1", "onparry1", true);
    //
    animation("YiNa_Skill06_01_02")
    {
      speed(0.4);
    };
    //
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 0, 0, -30, 0);
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 1);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 700);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 1400);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill13_GeDang_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);


     //判断是否有目标
    findmovetarget(1990, vector3(0, 0, 0), 5, 360, 0.8, 0.2, 0, 0, false)
    {
      filtsupperarmer();
    };
    //判断1
    gotosection(1990, 7, 0)
    {
	    targetcondition(false);
    };

    addimpacttoself(1990, 40000003, 200);
    addimpacttoself(1990, 40000002, 200);
  };

 section(1)//初始化 3
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 1000);
    addimpacttoself(0, 40000003, 1000);
    addimpacttoself(0, 40000002, 1000);
  };

  section(133)//起手 4
  {
    animation("YiNa_Skill03_01_01")
    {
      speed(1);
    };
    //目标选择
    findmovetarget(0, vector3(0, 0, 1), 5, 360, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(300)//第一段 5
  {
    animation("YiNa_Skill03_01_02")
    {
      speed(1);
    };
     //角色移动
     //startcurvemove(5, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.1, 2.6, true)
		{
			stateimpact("kDefault", 40030102);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //震屏
    shakecamera2(30, 200, true, true, vector3(1, 1, 0), vector3(30, 30, 0), vector3(30, 30, 0), vector3(40, 30, 0));
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_XuanFeng_01", 2000, "Bone_Root", 1);
    //
    //音效
    playsound(0, "Hit11", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill01_TuZhan_03", false);
    playsound(0, "Hit1102", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//收招 6
  {
    animation("YiNa_Skill03_01_99")
    {
      speed(1);
    };
  };

  section(1)//结束 7
  {
    movecontrol(true);
    parrycheckover(0);
  };

  onmessage("onparry1")
  {
       //facetoattacker(0,2000);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
        parrycheckover(0);
        setenable(0, "Visible", true);
        stopeffect(0);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
        parrycheckover(0);
        setenable(0, "Visible", true);
        stopeffect(0);
  };
};



/****    伊娜防御秒反击    ****/

skill(4006012)
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
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 2000);
    addimpacttoself(0, 40000003, 2000);
    addimpacttoself(0, 40000002, 2000);
  };

  section(100)//起手
  {
    animation("YiNa_Skill06_01_01")
    {
      speed(1);
    };
  };

  section(50)//第一段
  {
    animation("YiNa_Skill06_01_02")
    {
      speed(0.4);
    };
    //
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 0, 0, -30, 0);
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 100, "Bone_Root", 1);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 100, "Bone_Root", 700);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 100, "Bone_Root", 1400);
    //
    //音效
    playsound(10, "Hit11", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill13_GeDang_01", false);
    playsound(10, "Hit1102", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/guaiwu_shouji_01", true);
  };

 section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 1000);
    addimpacttoself(0, 40000003, 1000);
    addimpacttoself(0, 40000002, 1000);
  };

  section(133)//起手
  {
    animation("YiNa_Skill03_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(300)//第一段
  {
    animation("YiNa_Skill03_01_02")
    {
      speed(1);
    };
     //角色移动
     //startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(0, 0, 1.5, 1.1, 4.2, true)
		{
			stateimpact("kDefault", 40030101);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //震屏
		shakecamera2(30, 200, true, true, vector3(1, 1, 0), vector3(30, 30, 0), vector3(30, 30, 0), vector3(40, 30, 0));
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_XuanFeng_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(0, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill01_TuZhan_03", false);
    playsound(0, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//收招
  {
    animation("YiNa_Skill03_01_99")
    {
      speed(1);
    };
  };
};


/****    伊娜防御1秒反击    ****/

skill(4006013)
{
  section(1)//初始化 0
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
    //
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 2500);
    addimpacttoself(0, 40000003, 200);
    addimpacttoself(0, 40000002, 200);
  };

  section(100)//起手 1
  {
    animation("YiNa_Skill06_01_01");
  };

  section(1000)//第一段 2
  {
    //受击检测
    parrycheck(1,900,0,"onparry1", "onparry1", true);
    //
    animation("YiNa_Skill06_01_02")
    {
      speed(0.4);
    };
    //
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 0, 0, -30, 0);
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 1);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 2000, "Bone_Root", 700);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill13_GeDang_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);


     //判断是否有目标
    findmovetarget(990, vector3(0, 0, 0), 5, 360, 0.8, 0.2, 0, 0, false)
    {
      filtsupperarmer();
    };
    //判断1
    gotosection(990, 7, 0)
    {
	    targetcondition(false);
    };

    addimpacttoself(990, 40000003, 200);
    addimpacttoself(990, 40000002, 200);
  };

 section(1)//初始化 3
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 1000);
    addimpacttoself(0, 40000003, 1000);
    addimpacttoself(0, 40000002, 1000);
  };

  section(133)//起手 4
  {
    animation("YiNa_Skill03_01_01")
    {
      speed(1);
    };
    //目标选择
    findmovetarget(0, vector3(0, 0, 1), 5, 360, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(300)//第一段 5
  {
    animation("YiNa_Skill03_01_02")
    {
      speed(1);
    };
     //角色移动
     //startcurvemove(5, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.1, 2.6, true)
		{
			stateimpact("kDefault", 40030102);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //震屏
    shakecamera2(30, 200, true, true, vector3(1, 1, 0), vector3(30, 30, 0), vector3(30, 30, 0), vector3(40, 30, 0));
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_XuanFeng_01", 2000, "Bone_Root", 1);
    //
    //音效
    playsound(0, "Hit11", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill01_TuZhan_03", false);
    playsound(0, "Hit1102", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//收招 6
  {
    animation("YiNa_Skill03_01_99")
    {
      speed(1);
    };
  };

  section(1)//结束 7
  {
    movecontrol(true);
        parrycheckover(0);
  };

  onmessage("onparry1")
  {
       //facetoattacker(0,2000);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
        parrycheckover(0);
        setenable(0, "Visible", true);
        stopeffect(0);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
        parrycheckover(0);
        setenable(0, "Visible", true);
        stopeffect(0);
  };
};


/****    伊娜防御0.3秒反击    ****/

skill(4006014)
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
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 1000);
    addimpacttoself(0, 40000003, 1000);
    addimpacttoself(0, 40000002, 1000);
  };

  section(100)//起手
  {
    animation("YiNa_Skill06_01_01")
    {
      speed(1);
    };
  };

  section(300)//第一段
  {
    addimpacttoself(0, 40000003, 1000);
    addimpacttoself(0, 40000002, 1000);
    animation("YiNa_Skill06_01_02")
    {
      speed(0.4);
    };
    //
    //角色移动
    startcurvemove(10, true, 1, 0, 0, 0, 0, -30, 0);
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 300, "Bone_Root", 1);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 300, "Bone_Root", 700);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 300, "Bone_Root", 1400);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill13_GeDang_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

 section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
    //自身增加无敌霸体buff, 受击可释放
    addimpacttoself(0, 12990004, 1000);
    addimpacttoself(0, 40000003, 1000);
    addimpacttoself(0, 40000002, 1000);
  };

  section(133)//起手
  {
    animation("YiNa_Skill03_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(300)//第一段
  {
    animation("YiNa_Skill03_01_02")
    {
      speed(1);
    };
     //角色移动
     //startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(0, 0, 1.5, 1.1, 2.6, true)
		{
			stateimpact("kDefault", 40030102);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //震屏
		shakecamera2(30, 200, true, true, vector3(1, 1, 0), vector3(30, 30, 0), vector3(30, 30, 0), vector3(40, 30, 0));
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_XuanFeng_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(0, "Hit11", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill01_TuZhan_03", false);
    playsound(0, "Hit1102", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//收招
  {
    animation("YiNa_Skill03_01_99")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 1, 2000);
    addbreaksection(10, 1, 2000);
    addbreaksection(100, 1, 2000);
  };
};