
/****    银盾剑盾兵、长枪兵技能    ****/

//剑盾兵上挑
skill(300413)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(1000)//起手
  {
    animation("Skill_02_01")
    {
      speed(1);
    };
  };

  section(500)//第一段
  {
    animation("Skill_02_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.2, true)
		{
			stateimpact("kDefault", 30040303);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_ShangTiao_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(866)//收招
  {
    animation("Skill_02_99")
    {
      speed(1);
    };
  };
};

//剑盾兵普攻
skill(300403)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(1066)//起手
  {
    animation("Attack_01_01")
    {
      speed(1);
    };
  };

  section(466)//第一段
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.2, true)
		{
			stateimpact("kDefault", 30040301);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_PuGong_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(400)//收招
  {
    animation("Attack_01_99")
    {
      speed(1);
    };
  };
};

//剑盾兵重击
skill(300404)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(800)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1500);
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_XuLi_01", 3000, "ef_righthand", 1);
  };

  section(600)//起手2
  {
    animation("Skill_01_02")
    {
      speed(0.5);
    };
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1500);
  };

  section(500)//第一段
  {
    animation("Skill_01_03")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.1, 0, 0, 16, 0, 0, -30);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kDefault", 30040302);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSLancer_ZhongJi_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(333)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
  };
};


//长枪兵普攻
skill(300405)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(366)//起手
  {
    animation("Attack_01_01")
    {
      speed(1);
    };
  };

  section(300)//起手2
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
  };

  section(466)//第一段
  {
    animation("Attack_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040401);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_PuGong_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(366)//收招
  {
    animation("Attack_01_99")
    {
      speed(1);
    };
  };
};

//长枪兵重击
skill(300406)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(266)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
  };

  section(400)//起手2
  {
    animation("Skill_01_02")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    //
    //角色移动
    startcurvemove(1, true, 0.4 , 0, 0, 5 , 0, 0, 0);
    //
  };

  section(333)//第一段
  {
    animation("Skill_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040402);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_ZhongJi_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(433)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
  };
};

//长枪兵普攻 无前摇
skill(300407)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(300)//起手2
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
  };

  section(466)//第一段
  {
    animation("Attack_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040401);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_PuGong_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(366)//收招
  {
    animation("Attack_01_99")
    {
      speed(1);
    };
  };
};

//长枪兵重击 无前摇
skill(300408)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(400)//起手2
  {
    animation("Skill_01_02")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    //
    //角色移动
    startcurvemove(1, true, 0.4 , 0, 0, 5 , 0, 0, 0);
    //
  };

  section(333)//第一段
  {
    animation("Skill_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040402);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_ZhongJi_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(433)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
  };
};

//长枪兵普攻 无后摇
skill(300409)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(366)//起手
  {
    animation("Attack_01_01")
    {
      speed(1);
    };
  };

  section(300)//起手2
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
  };

  section(466)//第一段
  {
    animation("Attack_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040401);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_PuGong_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};

//长枪兵重击 无后摇
skill(300410)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(266)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
  };

  section(400)//起手2
  {
    animation("Skill_01_02")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    //
    //角色移动
    startcurvemove(1, true, 0.4 , 0, 0, 5 , 0, 0, 0);
    //
  };

  section(333)//第一段
  {
    animation("Skill_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040402);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_ZhongJi_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};

//长枪兵普攻 无前后摇
skill(300411)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(300)//起手2
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
  };

  section(466)//第一段
  {
    animation("Attack_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040401);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_PuGong_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

};

//长枪兵重击 无前后摇
skill(300412)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(400)//起手2
  {
    animation("Skill_01_02")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    //
    //角色移动
    startcurvemove(1, true, 0.4 , 0, 0, 5 , 0, 0, 0);
    //
  };

  section(333)//第一段
  {
    animation("Skill_01_03")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 30040402);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/5_Mon_SSPike_ZhongJi_01", 500, "Bone_Root", 1);
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};