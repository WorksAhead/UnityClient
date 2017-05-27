

/****    影袭影子攻击    ****/

skill(4203011)
{
  section(1)//初始化
  {
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
  };

  section(200)//起手
  {
    animation("Skill07_04")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(10, true, 0.18, 0, 0, 25, 0, 0, -20);
    //
    //
    areadamage(0, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(20, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(40, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(60, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(80, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(100, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(120, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(140, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(160, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(180, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(200, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/DaKeNiSi/6_Mon_DaKeNiSi_ShadowXian_01", 2000, "Bone_Root", 1);
  };

  section(200)//起手
  {
    movecontrol(true);
    animation("Skill02_01")
    {
        speed(1.5);
    };
    //角色移动
    startcurvemove(0, true, 0.18, 0, 0, 25, 0, 0, -20);
areadamage(0, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(20, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(40, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(60, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(80, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(100, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(120, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(140, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    areadamage(160, 0, 1.5, 0, 1.2, true)
		{
			stateimpact("kDefault", 12990002);
			//stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
  };

  section(288)//第一段
  {
    animation("Skill02_02")
    {
        speed(1.5);
    };
    areadamage(10, 0, 1.5, 1.5, 2, true)
    {
        stateimpact("kDefault", 42010201);
        stateimpact("kKnockDown", 40000001);
    };
    shakecamera2(1, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
    //特效
    charactereffect("Monster_FX/DaKeNiSi/6_Mon_DaKeNiSi_ZhongJi_03", 1000, "Bone_Root", 1);
    //音效
    playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型消失
    setenable(0, "Visible", true);
    destroyself(0);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 1.5), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型消失
    setenable(0, "Visible", true);
    destroyself(0);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 1.5), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
};
