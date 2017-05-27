story(1)
{
  local
  {
    @chain_npc(1001);
    @batch_count(1);
    @is_create_npc(0);
    @max_batch(2);
  };
  onmessage("start")
  {
    // 创建锁链
    createnpcbyscore(@chain_npc, @@MaxScore, 0, @@MaxLevel);
    createnpcbyscore(9001, @@MaxScore, 0, @@MaxLevel);
    createnpcbyscore(9002, @@MaxScore, 0, @@MaxLevel);
    createnpcbyscore(9003, @@MaxScore, 0, @@MaxLevel);
    createnpcbyscore(9004, @@MaxScore, 0, @@MaxLevel);
    wait(1000);
    npccastskill(9001, 460001);
    npccastskill(9002, 460001);
    npccastskill(9003, 460001);
    npccastskill(9004, 460001);
    // 创建第一波怪物
    localmessage("createnextbatchnpc");
  };
  onmessage("objkilled")
  {
    if(@chain_npc == objid2unitid($0))
    {
      // 锁链被打断
      changescene(0);
    }
    else
    {
      if(1 == $1)
      {
        // 当前批次怪物被杀光
        if(@is_create_npc == 0)
        {
          if(@batch_count == @max_batch)
          {
            // 胜利
            missioncompleted(0);
            changescene(0);
          }
          else
          {
            localmessage("createnextbatchnpc");
          };
        }
      };
    };
  };
  onmessage("createnextbatchnpc")
  {
    // start
    @is_create_npc = 1;
    @batch_count = @batch_count + 1;
    //
    ////////////////////////////////////////////////////////
    //
    ///////////////   第1大波   ///////////////
    //
    // 波数 1-1
    wait(3000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 1000 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    // 波数 1-2
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 1100 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    // 波数 1-3
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 1200 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    ///////////////   第2大波   ///////////////
    //
    // 波数 2-1
    wait(3000);
    loop(1)
    {
      createnpcbyscore(@batch_count + 2000 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    // 波数 2-2
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 2100 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    // 波数 2-3
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 2200 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    ///////////////   第3大波   ///////////////
    //
    // 波数 3-1
    wait(3000);
    loop(1)
    {
      createnpcbyscore(@batch_count + 3000 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    // 波数 3-2
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 3100 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    // 波数 3-3
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 3200 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    ///////////////   第4大波   ///////////////
    //
    // 波数 4-1
    wait(3000);
    loop(1)
    {
      createnpcbyscore(@batch_count + 4000 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    // 波数 4-2
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 4100 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    // 波数 4-3
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 4200 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    ///////////////   第5大波   ///////////////
    //
    // 波数 5-1
    wait(3000);
    loop(1)
    {
      createnpcbyscore(@batch_count + 5000 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    //
    // 波数 5-2
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 5100 + $$, @@MaxScore, 0, @@MaxLevel);
    };
    // 波数 5-3
    wait(10000);
    loop(20)
    {
      createnpcbyscore(@batch_count + 5200 + $$, @@MaxScore, 0, @@MaxLevel);
    };

    ////////////////////////////////////////////////////////
    // finish
    @is_create_npc = 0;
  };
  onmessage("npcleavearea")
  {
    npcaddimpact(objid2unitid($0), 88887);
  };
  onmessage("alluserkilled")//队灭
  {
    wait(1000);
    publishgfxevent("ge_mpve_misson_failed","ui");
    terminate();
  }; 
  onmessage("missionfailed")//用于回城处理
  {
    missionfailed();
    terminate();
  };
};
