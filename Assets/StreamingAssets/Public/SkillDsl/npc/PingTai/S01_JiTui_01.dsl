

/****    普攻一段    ****/

skill(460001)
{
  section(1)//初始化
  {
    movecontrol(true);
  };

  section(3000000)//起手
  {
    colliderdamage(0, 3000000, true, true, 0, 1)
    {
      stateimpact("kDefault", 88887);
      sceneboxcollider(vector3(40, 20, 10), vector3(0, 0, 6.2), eular(0, 0, 0), true, false);
    };
  };
};
