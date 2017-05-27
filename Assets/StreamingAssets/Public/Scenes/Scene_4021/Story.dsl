story(1)
{
	local
  {
     @Count(0);         //刷怪计数
     @Money(0);        //可掉总钱数量
     @Point(vector2list("51 71 70 77 83 69 89 52 70 54"));   //场景中5个点
     @Patrolpath(vector2list("51 71 70 77"));					   //怪物跑动路径
	 @MosterList(intlist("1001 1002 1002 1003 1003 1003"));
	 @PosList(intlist("0 1 2 3 4 5"));
     @tempobjId(0);
     @unitid(0);
	 @deltime(3000);
	 @theLevel(1);
  };
	onmessage("start")
	{	
		wait(1000);
	   showwall("BDoor",true);
	   wait(200);
		
		//thelevel是调钱的放大系数
	  if(@@MaxLevel>10 && @@MaxLevel<20 )
	  {
		   @theLevel = 0.9;
	  };

	  if(@@MaxLevel>20 && @@MaxLevel<40 )
	  {
		   @theLevel = 0.95;
	  };

	  if(@@MaxLevel>=40)
	  {
		   @theLevel = 1;
	  };



      //通知UI显示计时
      restarttimeout(2);
      publishgfxevent("ge_just_counttime","ui", 120);

     //循环刷怪消息
      loop(100)
      {
            wait(@deltime);
			inc(@Count);
            @unitid = rndfromlist(@MosterList);
			//确定多少钱

			@Money = 100;

            if(@unitid == 1001)
			{
				@Money = @theLevel*360;	
			};

			if(@unitid == 1002)
			{
                		@Money = @theLevel*163;
			};

			if(@unitid == 1003)
			{
				@Money =@theLevel*108;
			};
	
			if(@Count>20)
			{
				//如果刷新波数大于25(总计75秒)以后。每波间隔变成1.5秒
				@deltime = 1500;
			};

			loop(3)
			{
			  @bornIndex=rndint(0,5);
			  @rdposition = listget(@Point,@bornIndex);
				createnpcbyscore(@unitid,@@MaxScore,1,@@MaxLevel,vector2to3(@rdposition),0)objid("@tempobjId");
        //createnpc(@unitid,vector2to3(@rdposition),21)objid("@tempobjId");
           
			    wait(100);
			    objset(@tempobjId,"mymaxmoney",@Money);
				objset(@tempobjId,"mymoney", @Money);	//把怪的钱存身上
             objenableai(@tempobjId,"false");
	         objlisten(@tempobjId,"damage","true");
             localmessage("move",@tempobjId,@bornIndex);
			   wait(300);
			};
				
			
      };
     

	 
	};

    onmessage("move")
    {
          // log("coming move");
          @beginpoint =   listget(@Point,$1);
          @endIndex =  0;

           if($1==0)
		   {
				 @endIndex =  rndfromlist(intlist("2 3"));
			};

		   if($1==1)
			{
                 @endIndex =  rndfromlist(intlist("3 4"));
			};

			 if($1==2)
			{
                @endIndex =  rndfromlist(intlist("0 4"));
			};

			 if($1==3)
			{
                  @endIndex =  rndfromlist(intlist("0 1"));
			};

			 if($1==4)
			{
                 @endIndex =  rndfromlist(intlist("1 2"));
			};

          @endpoint   =    listget(@Point,@endIndex);
			
           listset(@Patrolpath,0,@beginpoint);
		   listset(@Patrolpath,1,@endpoint);
        
          npcpatrolwithobjid($0,@Patrolpath,noloop);
     };

	onmessage("objpatrolfinish")
	{
         destroynpcwithobjid($0);
	};

     onmessage("objdamage")
     {

		 @MaxHp = getmaxhp($0);
		 @CurHp  =  gethp($0);
		 @damage  =  $2;

		 @dropmoney = 0;
		 @maxmoney =  objget($0,"mymaxmoney");
		 @percent  = 0;
		 if( @MaxHp > 1 )
		 {
			   @percent =  (0-@damage)*1.0/@MaxHp;
				if( @percent >1)
				{
					@percent = 1;
				};
				
				if( @percent<0.05)
				{
					@percent = 0;
				};

		       @dropmoney = @maxmoney*@percent;

			    @leftmoney = objget($0,"mymoney");
		      
				if( @leftmoney>=@dropmoney)
				{
					objset($0,"mymoney", @leftmoney-@dropmoney);	
				}
				else
				{
					@dropmoney = 0;
				};
     	 };
		//log("dropmoney:"+@dropmoney);
		 if(@dropmoney>1)
		{
          dropnpc($1,$0,3,"Monster_FX/Public/Gold","",@dropmoney);
	    };
          
     };


	onmessage("objkilled")
	{
		 @maxmoney =  objget($0,"mymaxmoney");
		 @leftmoney = objget($0,"mymoney");
		 if(@leftmoney>0)
		{
			  dropnpc(0,$0,3,"Monster/Public/jinPhy","",@leftmoney);
		};

	};

  onmessage("timeout",2)
  {
    wait(1000);
    missioncompleted(0);
    terminate();
  };

  onmessage("missionfailed")
  {
    changescene(0);
    terminate();
  };
};
