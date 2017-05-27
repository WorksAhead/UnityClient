

/****    莱卡翁 皮毛强化    ****/

skill(450301)
{
  section(100)//第一段
  {
    addimpacttoself(0, 45030101, 500000);
  };
};


/****    莱卡翁 利爪强化    ****/

skill(450401)
{
  section(100)//第一段
  {
    //addimpacttoself(0, 12990001, 5000);
    //addimpacttoself(0, 12990003, 5000);
    //
    //特效
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaZi_L", 500000, "ef_lefthand", 1);
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaZi_R", 500000, "ef_righthand", 1);
    //
  };
};
