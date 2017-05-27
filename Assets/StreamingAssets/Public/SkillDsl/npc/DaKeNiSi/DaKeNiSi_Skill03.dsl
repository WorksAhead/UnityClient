

/****    达克尼斯影袭    ****/

skill(420301)
{
  section(1)//起手1
  {
    movecontrol(true);
/*
    animation("Skill07_01")
    {
      speed(1);
    };
*/
  };

  section(215)//起手2
  {
    animation("Skill07_02")
    {
      speed(2);
    };
  };

  section(400)//第一段
  {
    animation("Skill07_03")
    {
      speed(2);
    };
    //
    //
    enablechangedir(0, 2000);
    //
    //召唤NPC
    summonnpc(0, 101, "Monster/Boss/5_Boss_DaKeNiSi_02", 4203011, vector3(0, 0, 0));
  };
};
