//影子出生
skill(4204011)
{
  section(433)
  {
    animation("Skill06_02");
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_ChuSheng_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    setlifetime(10, 15000);
  };
};

//影子出生2
skill(4204012)
{
  section(433)
  {
    settransform(0," ",vector3(-2,0,0),eular(0,0,0),"RelativeOwner",false, false, vector3(0, 0, 0));
    animation("Skill06_02");
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_ChuSheng_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    setlifetime(10, 15000);
  };
};

//召唤影分身
skill(420401)
{
  section(566)//第一段
  {
    animation("Skill06_01");
    movecontrol(true);
  };

  section(433)//第二段
  {
    animation("Skill06_02");
    //召唤NPC
    summonnpc(0, 5028, "", 4204011, vector3(2, 0, 0), eular(0, 0, 0), 20005, false, "0,,231001");
    summonnpc(0, 5028, "", 4204011, vector3(-2, 0, 0), eular(0, 0, 0), 20005, false, "0,,231001");
  };
};