//连击两下
skill(320501)
{
  section(1600)
  {
    animation("Attack_01");
    areadamage(456, 0, 1, 0.5, 3, true) {
      stateimpact("kDefault", 32050101);
    };
    cleardamagestate(800);
    areadamage(888, 0, 1, 0.5, 3, true) {
      stateimpact("kDefault", 32050101);
    };
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/5_Mon_Stone_ZhuaJi_01", 1000, "Bone_Root", 430);
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/5_Mon_Stone_ZhuaJi_02", 1000, "Bone_Root", 850);
    shakecamera2(470, 100, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,100));
    shakecamera2(900, 100, true, true, vector3(0,0,0.4), vector3(0,0,50),vector3(0,0,1.2),vector3(0,0,100));
    playsound(427, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(850, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_02", false);
    playsound(470, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(900, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//普攻
skill(320502)
{
  section(2500)
  {
    animation("Attack_02");
    areadamage(1520, 0, 1, 0.5, 3, true) {
      stateimpact("kDefault", 32050201);
    };
    charactereffect("Monster_FX/Campaign_Desert/05_Monster/5_Mon_Stone_ZhuaJi_03", 1000, "Bone_Root", 1500);
    shakecamera2(1530, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,100));
    playsound(1480, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(1530, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//荆棘
skill(320503)
{
  section(2733)
  {
    animation("Skill_01");
    setanimspeed(30, "Skill_01", 0.5, true);
    setanimspeed(530, "Skill_01", 1, true);
    shakecamera2(600, 100, true, true, vector3(0,1,0), vector3(0,25,0),vector3(0,1,0),vector3(0,100,0));
    addimpacttoself(599,32050300,2000);
    findmovetarget(400, vector3(0,0,0),4,50,0.5,0.5,0,-2);
    facetotarget(600,5000,80);
    setanimspeed(600, "Skill_01", 0.05, true);
    setanimspeed(860, "Skill_01", 1, true);
  };
};