

/****    伊娜前冲    ****/

skill(400401)
{
  section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
  };

  section(333)//第一段
  {
    animation("YiNa_Skill04_01_01")
    {
      speed(1);
    };
    //
    //目标选择
		findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, -2);
    //
     //角色移动
    startcurvemove(30, true, 0.25, 0, 0, 40, 0, 0, -100);
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_ShanShen_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Npc/YiNa/boss_YiNa_ShunShen_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };
};
