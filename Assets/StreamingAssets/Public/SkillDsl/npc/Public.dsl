//怪物通用爆气脱控
skill(300001)
{
  section(1933)
  {
    animation("Stand");
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeSelf",false,true);
    addimpacttoself(0, 30010003);
    playsound(370, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_baoqi", false);
    areadamage(450, 0, 1, 0, 3, false) {
      stateimpact("kDefault", 38020201);
    };
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_BaoQi_01", 3000, vector3(0,1.2,0), 300, eular(0,0,0), vector3(1.5,1.5,1.5));
    shakecamera2(450,550,false,false,vector3(0.5,0.5,0.5),vector3(50,50,50),vector3(5,5,5),vector3(85,85,85));
  };
};
