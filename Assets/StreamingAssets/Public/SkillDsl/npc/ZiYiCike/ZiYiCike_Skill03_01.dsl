

/****    紫衣护盾    ****/

skill(430301)
{
  section(100)//第一段
  {

    animation("Cike_Skill08_02_99")
    {
      speed(1.2);
    };
    addimpacttoself(0, 12990001, 5000);
    addimpacttoself(0, 12990003, 5000);
    //
    //特效
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_HuDunJuji_01", 5000, "Bone_Root", 1);
    charactereffect("Monster_FX/ZiYiCiKe/3_Hero_ZiYiCiKe_HuDun_01", 5000, "Bone_Root", 1);
    //
  };
};
