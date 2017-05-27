
/****    影刃部队技能    ****/

//双刀普攻
skill(320301)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(266)//起手
  {
    animation("Attack_01_01")
    {
      speed(1);
    };
  };

  section(133)//第一段
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2, true)
		{
			stateimpact("kDefault", 32020801);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_PuGong_01", 500, "Bone_Root", 1);
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

//双刀兵闪身
skill(320302)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(333)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
    //
    //模型消失
    setenable(300, "Visible", false);
    //
    //特效
    sceneeffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_ShanShen_01", 3000, vector3(0, 0, 0), 290, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(100)//第一段
  {
    //角色移动
    startcurvemove(1, true, 0.1, 0, 0, -30, 0, 0, 0);
  };

  section(233)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
    //
    //模型显示
    setenable(0, "Visible", true);
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

//双刀背刺
skill(320303)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(333)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
    //
    //模型消失
    setenable(300, "Visible", false);
    //
    //特效
    sceneeffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_ShanShen_01", 3000, vector3(0, 0, 0), 290, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(100)//第一段
  {
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    //
    //目标选择
		findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, 2);
    //
    //角色移动
    startcurvemove(10, true, 0.05, 0, 0, 20, 0, 0, 0);
    //
    //传送
    settransform(90, " ", vector3(0, 0, 0), eular(0, 180, 0), "RelativeSelf", true);
  };


  section(1)//初始化
  {
    movecontrol(true);
  };

  section(266)//起手
  {
    animation("Attack_01_01")
    {
      speed(1);
    };
    //
    //模型显示
    setenable(0, "Visible", true);
  };

  section(133)//第一段
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2, true)
		{
			stateimpact("kDefault", 32020802);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_PuGong_01", 500, "Bone_Root", 1);
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

//单刀普攻
skill(320304)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(266)//起手
  {
    animation("Attack_01_01")
    {
      speed(1);
    };
  };

  section(133)//第一段
  {
    animation("Attack_01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2, true)
		{
			stateimpact("kDefault", 32020803);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
	  shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //
    //特效
    charactereffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_PuGong_01", 500, "Bone_Root", 1);
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

//单刀兵闪身
skill(320305)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(333)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
    //
    //模型消失
    setenable(300, "Visible", false);
    //
    //特效
    sceneeffect("Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_ShanShen_01", 3000, vector3(0, 0, 0), 290, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(100)//第一段
  {
    //角色移动
    startcurvemove(1, true, 0.1, 0, 0, -30, 0, 0, 0);
  };

  section(233)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
    //
    //模型显示
    setenable(0, "Visible", true);
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

//单刀飞镖
skill(320306)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(333)//起手
  {
    animation("Skill_01_01")
    {
      speed(1);
    };
  };

  section(266)//第一段
  {
    animation("Skill_01_02")
    {
      speed(1);
    };
    //
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/Campaign_Desert/03_Assassin/5_Mon_Assassin_FeiBiao_01", 320307, vector3(0, 0, 0));
    //
    //音效
	  playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  section(233)//收招
  {
    animation("Skill_01_99")
    {
      speed(1);
    };
  };
};

//飞镖
skill(320307)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
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
      stateimpact("kDefault", 32020804);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
  };
};

//左徘徊
skill(320308)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(2000)//起手
  {
    animation("Walk")
    {
      speed(0.5);
    };
    //
    //角色移动
    startcurvemove(1, true, 2, 0.5, 0, 0.3, 0, 0, 0);
  };
};

//右徘徊
skill(320309)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(2000)//起手
  {
    animation("Walk")
    {
      speed(0.5);
    };
    //
    //角色移动
    startcurvemove(1, true, 2, -0.5, 0, 0.3, 0, 0, 0);
  };
};


