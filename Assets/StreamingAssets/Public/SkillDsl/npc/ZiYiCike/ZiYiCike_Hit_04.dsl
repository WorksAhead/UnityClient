

/****    普攻四段    ****/

skill(430004)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
  };

  section(200)//第一段
  {
    animation("Cike_Hit_04_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.12, 0, 0, 8, 0, 0, 0);
    //
    //翅膀
    playpartanimation(0, "CiBang_02", "Open", 1);
    //
  };
  section(166)//第二段
  {
    animation("Cike_Hit_04_02")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.1, 0, 0, 16, 0, 0, -30);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 43000401);
			stateimpact("kLauncher", 43000402);
      //showtip(200, 0, 1, 0);
		};
    //
    //震屏
    shakecamera2(40, 250, true, true, vector3(0.2,0.2,0.2), vector3(50,50,50),vector3(1,1,1),vector3(50,50,50));
    //
    //特效
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_PuGong_04_001", 500, "Bone_Root", 1);
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_PuGong_04_002", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_08", false);
    playsound(20, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    playsound(20, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_Voice_01", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //残影
    //createshadow(1, 50, 1, 200, 600, "3_Cike_02", "Transparent/Cutout/Soft Edge Unlit", 0.8)
    //{
    //   ignorelist(3_Hero_CiKe);
    //};
    //
    //模型消失
    //setenable(0, "Visible", false);
    //setenable(100, "Visible", true);
  };

  section(333)//硬直
  {
    animation("Cike_Hit_04_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(10, 300, 2000);
    addbreaksection(21, 300, 2000);
    addbreaksection(100, 300, 2000);
  };

  section(100)//收招
  {
    animation("Cike_Hit_04_99")
    {
      speed(1);
    };
    //
  };

  oninterrupt()
  {
		//movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
    //翅膀
    playpartanimation(0, "CiBang_02", "Idle", 2);
  };

  onstop()
  {
		//movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
    //翅膀
    playpartanimation(0, "CiBang_02", "Idle", 2);
  };

};
