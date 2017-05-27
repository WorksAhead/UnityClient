//来福枪兵
skill(300301)
{
  section(1000)
  {
    animation("Attack_01A");
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
//音效
    playsound(0, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/monster_shotgun", false);
    colliderdamage(5,100,true,true,150,1) {
    stateimpact("kDefault",30030101);
    boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2233)
  {
    animation("Attack_01C");
  };
};

//火枪兵
skill(300302)
{
  section(3867)
  {
    movecontrol(true);
    animation("Attack_01");
    playsound(1300, "yujing", "Sound/Npc/Mon", 1230, "Sound/Npc/guaiwu_shoothongwaixian_01", false);
    charactereffect("Monster_FX/Campaign_Wild/Public/6_Monster_YuJing_RedLine_01", 1200, "ef_weapon01", 1300);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDM_QiangKou_01", 500, "ef_weapon01", 2500, false);
    playsound(2495, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/monster_shotgun", false);
    summonnpc(2500, 101, "Monster/Campaign_Wild/03_KDArmy/6_Mon_KDM_Bullet_01", 390004, vector3(0, 0, 0));
    startcurvemove(2530, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//火枪兵扔雷
skill(300303)
{
  section(1333)
  {
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 1200, vector3(0,0.2,6.7));
    animation("Skill_01");
//    startcurvemove(2100, true, 0.1, 0, 0, 3, 0, 0, 0);
    summonnpc(590, 101, "Monster/Campaign_Wild/03_KDArmy/6_Mon_KDM_Bomb_01", 390005, vector3(0, 0, 0));
  };
};

//来福枪兵连喷
skill(300304)
{
  section(1000)
  {
    animation("Attack_01A");
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    playsound(0, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/monster_shotgun", false);
    colliderdamage(5,100,true,true,150,1) {
      stateimpact("kDefault",30030401);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    playsound(0, "kaipao1", "Sound/Npc/Mon", 1000, "Sound/Npc/monster_shotgun", false);
    colliderdamage(5,100,true,true,150,1) {
      stateimpact("kDefault",30030401);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2233)
  {
    animation("Attack_01C");
  };
};



//来福枪兵连喷三下
skill(300305)
{
  section(1000)
  {
    animation("Attack_01A");
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    playsound(0, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/monster_shotgun", false);
    colliderdamage(5,100,true,true,150,1) {
      stateimpact("kDefault",30030501);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    playsound(0, "kaipao1", "Sound/Npc/Mon", 1000, "Sound/Npc/monster_shotgun", false);
    colliderdamage(5,100,true,true,150,1) {
      stateimpact("kDefault",30030501);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    playsound(0, "kaipao2", "Sound/Npc/Mon", 1000, "Sound/Npc/monster_shotgun", false);
    colliderdamage(5,100,true,true,150,1) {
      stateimpact("kDefault",30030501);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2233)
  {
    animation("Attack_01C");
  };
};


//长矛兵劈砍
skill(300306)
{
  section(933)
  {
    movecontrol(true);
    animation("Attack_01");
    startcurvemove(431, true, 0.1, 0, 0, 3, 0, 0, 0);
    playsound(435, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDPike_DaoGuang_01", 1000, "Bone_Root", 430);
    areadamage(468, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 30030601);
    };
    playsound(470, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(470, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
  };
};

//长矛兵劈砍
skill(300307)
{
  section(1133)
  {
    animation("Skill_01");
    playsound(455, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDPike_DaoGuang_02", 1000, "Bone_Root", 460);
    areadamage(489, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 30030701);
    };
    playsound(495, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(495, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
  };
};

//剑兵劈砍
skill(300308)
{
  section(2200)
  {
    movecontrol(true);
    animation("Attack_01");
    startcurvemove(1200, true, 0.1, 0, 0, 3, 0, 0, 0);
    playsound(1266, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDJ_DaoGuang_01", 1000, "Bone_Root", 1266);
    areadamage(1266, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 30030801);
    };
    playsound(1266, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1300, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
  };
};

//剑兵劈砍
skill(300309)
{
  section(1666)
  {
    animation("Skill_01");
    playsound(700, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDJ_DaoGuang_02", 1000, "Bone_Root", 700);
    areadamage(700, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 30030901);
    };
    playsound(700, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(760, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
  };
};