//抡击
skill(310501)
{
  section(1500)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Dungeon/05_Carrion/6_Mon_Carrion_DaoGuang_01", 2000, "Bone_Root", 660);
    startcurvemove(550, true, 0.1, 0, 0, 10, 0, 0, 20);
    playsound(680, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    areadamage(750, 0, 1, 1.5, 2.6, true) {
      stateimpact("kDefault", 31050101);
    };
    playsound(760, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(770, 250, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,40,60));
  };
};


//冲撞
skill(310502)
{
  section(1533)
  {
    movecontrol(true);
    addimpacttoself(0, 30010100, 3500);
    animation("Skill_01");
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 2500, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);//场景粒子在特殊位置仍然有不同步方向问题
    startcurvemove(500, true, 0.3, 0, 0, 18, 0, 0, 0);
    playsound(500, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_chongji", false);
    charactereffect("Monster_FX/Campaign_Dungeon/05_Carrion/6_Mon_Carrion_ShanShen_01",1000,"Bone_Root",500,true) {
      transform(vector3(0,0.5,0));
    };
    colliderdamage(510, 300, false, false, 0, 0)
    {
      stateimpact("kDefault", 31050201);
      sceneboxcollider(vector3(3, 2, 4), vector3(0, 1, 0.2), eular(0, 0, 0), true, false);
    };
    playsound(550, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(540, 250, true, true, vector3(0,0,0.3), vector3(0,0,150),vector3(0,0,0.6),vector3(0,0,80));
    setanimspeed(100,"Skill_01",0.2,true);
    setanimspeed(500,"Skill_01",1,true);
  };
};


//抡打
skill(310503)
{
  section(1500)
  {
    movecontrol(true);
    animation("Skill_02");
    charactereffect("Monster_FX/Campaign_Dungeon/05_Carrion/6_Mon_Carrion_DaoGuang_02", 2000, "Bone_Root", 450);
    startcurvemove(400, true, 0.54, 0, 0, 5, 0, 0, 0);
    playsound(420, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    areadamage(538, 0, 1, 0.5, 3.5, true) {
      stateimpact("kDefault", 31050302);
      stateimpact("kStand", 31050301);
    };
    playsound(540, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(540, 250, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,40,60));

    areadamage(862, 0, 1, 0.5, 3.5, true) {
      stateimpact("kDefault", 31050302);
      stateimpact("kStand", 31050301);
    };
    playsound(865, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(865, 250, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,40,60));
    setanimspeed(10,"Skill_02",0.4,true);
    setanimspeed(310,"Skill_02",1,true);
  };
};