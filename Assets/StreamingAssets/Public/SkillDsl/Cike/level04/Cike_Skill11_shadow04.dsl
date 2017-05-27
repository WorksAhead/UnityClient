

/****    暗器攻击4    ****/

skill(125504)
{
  section(1)//初始化
  {
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(-0.5, 1.4, 0),eular(0,0,0),"RelativeOwner",false);

    setlifetime(0, 300);
  };

  section(1000)//第一段
  {
     //目标选择
		findmovetarget(0, vector3(0, 0, 1), 10, 30, 0.1, 0.9, 0, 3);
    //
    //角色移动
    startcurvemove(10, true, 0.3, 0, 0, 50, 0, 0, 0);
    //
    //碰撞盒
    colliderdamage(0, 2000, true, true, 0, 1)
    {
      stateimpact("kDefault", 12110101);
      sceneboxcollider(vector3(1, 1, 1), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_02", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  onmessage("oncollide")  //处理事件, "oncollide"是默认碰撞事件
  {
    destroyself(0);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_FeiBiaoShouJi_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
};
