story(1)
{
	local
  {
    @Count(0);
    @RollBoss1(0);
    @RollBoss2(0);
    @RollBoss3(0);
    @RollBoss4(0);
    @RollBoss5(0);
  };
	onmessage("start")//初始化
	{	
		
		wait(10);
		 showui(false);//隐藏UI
		wait(10);
		
		//关灯
		 sendgfxmessage("GfxGameRoot","EnableBloom");
		 wait(10);
		 sendgfxmessage("Main Camera","DimScreen",10);
		 wait(10); 
		 
	  
	  wait(500);
	  
	  //初始摄像头定位
	  camerayaw(30,10);
	  cameralookat(31,168,22);
	  
	  wait(1000);
	   sendgfxmessage("Main Camera","LightScreen",2000);//开灯
	   wait(10);
	   camerayaw(60,1000);//旋转展示
	    wait(500);
	   publishgfxevent("pve_boss_enter","ui_effect","大战开启");//字幕
		 wait(2500);
	  
	  //关灯
	  sendgfxmessage("GfxGameRoot","EnableBloom");
		 wait(10);
		 sendgfxmessage("Main Camera","DimScreen",10);
		 wait(10); 
		 camerayaw(270,10);//回镜头
		 //cameralookat(73.42709,178.9598,38.68758); 
		
		  wait(10);  
	   camerafollowimmediately();
	  
	  
	   wait(500); 
		 sendgfxmessage("Main Camera","LightScreen",2000);//开灯
	   wait(10);
	   
	   showui(true);//显示UI
	   wait(10);
	 
	  
	  publishgfxevent("ge_mpve_progress","ui",@Count,5);//修改界面击杀boss数显示
	  //inc(@Count);
	  wait(2000);
	  
	  
	  if(0==@Count)//打第一波怪的时候
	  {
	  	
	  	@RollBoss1=rndfromlist(intlist("1003 1005 1006 1017 1038 1039 1019 1020 1031 1060 1061"));//roll第1个怪
	  	
	  	 wait(10);
	  	createnpcbyscore(@RollBoss1,@@MaxScore*0.6,0,@@MaxLevel);//刷第1个怪，AverageScore后跟系数，用于放大战斗难度
	  	wait(300);
	  	
	  };	
			
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);//设置主角半透
	};
	
	onmessage("allnpckilled")//每打完一波怪
	{
    
    wait(10);
		inc(@Count);
		wait(10);
    publishgfxevent("ge_mpve_progress","ui",@Count,5);//修改界面击杀boss数显示
		wait(10);
		if(@Count<=4)
			{
	      publishgfxevent("pve_area_clear","ui_effect",0);//显示clear动效
	  }else
		{
			publishgfxevent("pve_area_clear","ui_effect",1);//显示stage clear动效
      
		};
	  
	   //子弹效果
	    lockframe(0.01);
      wait(500);
      lockframe(0.05);
      wait(1800);
      lockframe(0.08);
      wait(300);
      lockframe(0.2);
      wait(500);
      lockframe(1.0);
		  wait(300);
	  
	  wait(1000);
	  
	 
	  if(1==@Count)//打第2波怪的时候
	  {
		  
		  	publishgfxevent("pve_boss_enter","ui_effect","即将来袭");
		  	wait(5000);
		  	@RollBoss2=rndfromlist(intlist("1056 1057 1059 1040 1051"));//roll第2个怪
		  	 wait(10);
		  	createnpcbyscore(@RollBoss2,@@MaxScore*0.7,0,@@MaxLevel);//刷第2个怪，AverageScore后跟系数，用于放大战斗难度
		  	wait(500);
		 
		  
		};
		if(2==@Count)//打第3波怪的时候
	  {
		    publishgfxevent("pve_boss_enter","ui_effect","即将来袭");
		  	wait(5000);
		    @RollBoss3=rndfromlist(intlist("1042 1045 1041 1049"));//roll第3个怪
		     wait(10);
		    createnpcbyscore(@RollBoss3,@@MaxScore*0.8,0,@@MaxLevel);//刷第3个怪，，AverageScore后跟系数，用于放大战斗难度
		  	wait(500);
		 
		};
		if(3==@Count)//打第4波怪的时候
	  {
		    publishgfxevent("pve_boss_enter","ui_effect","即将来袭");
		  	wait(5000);
		    @RollBoss4=rndfromlist(intlist("1043 1047 1054"));//roll第4个怪
		     wait(10);
		    createnpcbyscore(@RollBoss4,@@MaxScore*1.1,0,@@MaxLevel);//刷第4个怪，AverageScore后跟系数，用于放大战斗难度
		  	wait(500);
		 
		};
		if(4==@Count)//打第5波怪的时候
	  {
	  	  publishgfxevent("pve_boss_enter","ui_effect","即将来袭");
		  	wait(5000);
	  	  @RollBoss5=rndfromlist(intlist("1048 1055"));//roll第5个怪
	  	   wait(10);
	  	  createnpcbyscore(@RollBoss5,@@MaxScore*1.375,0,@@MaxLevel);//刷第5个怪，AverageScore后跟系数，用于放大战斗难度
		  	wait(500); 
		 
    };
    if(5==@Count)//5波都打完了
    {  
     
      
	  	missioncompleted(1010);	//通关5个boss
		  terminate();
		
	  };
	   
	}; 
	
	onmessage("alluserkilled")//队灭
	{
		  wait(1000);
		 if(0==@Count)//第一个boss都没打死
		 	{
		 		
		 		publishgfxevent("ge_mpve_misson_failed","ui");

		    terminate();
		 	}else{//至少打死了一个boss，就算胜利
		 		missioncompleted(1010);
		 		terminate();
		 	};
		 
	};
  onmessage("missionfailed")//用于回城处理
  {
    missionfailed(1010);
    terminate();
  };
  
   onmessage("userenterscene")//掉线再上或连接延迟的话
  {
    wait(10);
    publishgfxevent("ge_mpve_progress","ui",@Count,5);//修改界面击杀boss数显示
    wait(10);
  };
};
