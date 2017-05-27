story(1)
{ 
	local
  {
    @Stage(0);
    @GuShow(0);
    @Tencount(0);
    @Alive(0);
  };
	onmessage("start")
	{
		wait(1900);
		sendgfxmessage("GfxGameRoot","EnableBloom");
		showui(false);
		publishlogicevent("ge_set_story_state","game",1);
		sendgfxmessage("Main Camera","DimScreen",10);
		wait(10);
		cameraheight(11,10);
		cameradistance(17,10);
		cameralookatimmediately(66.51428,165.1624,13.64116);
		showwall("AirWall_101501",false);
		showwall("AirWall_101502",false);
		showwall("AirWall_101503",false);
		showwall("AirWall_101504",false);
		showwall("AtoB",false);
		publishgfxevent("ge_set_indicator_invisible","indicator");
		wait(10);
		publishlogicevent("ge_change_player_movemode","game",1);
	  publishlogicevent("ge_change_npc_movemode","game",unitid2objid(10001),1);
	  wait(10);
	  sendgfxmessage("Main Camera","LightScreen",2000);
		wait(100);
		playerselfmovewithwaypoints(vector2list("64.57018 12.13312 68.31571 12.32799"));
		npcmovewithwaypoints(10001,vector2list("61.75694 12.562 64.37659 11.58491 68.23576 10.4111"));
		wait(3000);
		//publishgfxevent("ge_show_skip","ui",1);
	};
	onmessage("npcarrived",10001)
	{
		wait(50);
		npcface(10001,0.6315293);
	};
	onmessage("playerselfarrived")
	{
		wait(50);
		playerselfface(1.5707964);
		wait(1150);
		playerselfface(0.4523393);
		wait(500);
		showdlg(101501);
	};
	onmessage("dialogover",101501)
	{
		wait(10);
		npcface(10001,0.1);
		showdlg(101502);
	};
	onmessage("dialogover",101502)
	{
		wait(10);
		playerselfface(0.1);
		wait(450);
		cameralookat(70.19065,165.1624,53.8358);
		wait(500);
		npcmovewithwaypoints(10002,vector2list("72.40158 70.22399 76.32967 66.0602 75.77974 60.95367 72.83366 56.51493 69.96615 48.81585 69.21981 39.38841"));
		npcmovewithwaypoints(10010,vector2list("70.79421 72.58298 75.99223 68.69875 77.64874 62.98662 75.07829 58.18845 74.24719 53.7884"));
		npcmovewithwaypoints(10011,vector2list("68.96659 71.66775 74.05037 68.64033 75.36415 65.21307 72.67945 60.87187 71.65128 57.90157"));
		npcmovewithwaypoints(10012,vector2list("69.16962 72.60721 68.99825 68.95145 72.76825 67.00934"));
		npcmovewithwaypoints(10021,vector2list("69.19494 74.35301 75.93523 70.64014 78.5628 67.61272 79.02544 64.15398 76.2265 58.21338 71.7213 54.53439 70.2679 46.08898 71.17136 37.60429"));
		npcmovewithwaypoints(10022,vector2list("67.53843 73.26771 74.27872 69.55483 76.90629 66.52742 77.41604 62.99997 74.6171 57.05938 70.54399 53.85176 69.36556 45.7206 70.19047 36.8431"));
		npcmovewithwaypoints(10023,vector2list("65.31071 72.01105 72.05099 68.29817 74.67857 65.27076 75.24321 61.65067 72.44427 55.71008 68.95989 52.8293 68.48852 44.46244 69.11701 37.82396"));
		wait(2500);
		showdlg(101599);
		wait(9500);
		sendgfxmessage("Main Camera","DimScreen",800);
		wait(800);
		cameraheight(11.5,10);
		cameradistance(12,10);
		camerafollowimmediately();
		wait(10);
		destroynpc(10021);
		destroynpc(10022);
		destroynpc(10023);
		destroynpc(10010);
		destroynpc(10011);
		destroynpc(10012);
		destroynpc(10002);
		wait(10);
		sendgfxmessage("Main Camera","LightScreen",1500);
		wait(500);
		showdlg(101503);
	};
	onmessage("npcarrived",10021)
	{
		wait(50);
		destroynpc(10021);
	};
	onmessage("npcarrived",10022)
	{
		wait(50);
		destroynpc(10022);
	};
	onmessage("npcarrived",10023)
	{
		wait(50);
		destroynpc(10023);
	};
	onmessage("npcarrived",10002)
	{
		wait(50);
		destroynpc(10002);
	};
	onmessage("npcarrived",10010)
	{
		wait(150);
		objanimation(unitid2objid(10010),101,3267);
		wait(3500);
		objanimation(unitid2objid(10010),101,3267);
		wait(3300);
	};
	onmessage("npcarrived",10011)
	{
		wait(150);
		objanimation(unitid2objid(10011),101,3267);
		wait(3500);
		objanimation(unitid2objid(10011),101,3267);
		wait(3300);
	};
	onmessage("npcarrived",10012)
	{
		if(0==@GuShow)
		{
			inc(@GuShow);
			wait(150);
			objanimation(unitid2objid(10012),102,3000);
			wait(3300);
			npcmovewithwaypoints(10012,vector2list("76.86987 64.13005 75.78457 60.24582 71.10064 51.73476 68.8158 39.05388"));
			inc(@GuShow);
		}
		else
		{
			destroynpc(10012);
		};
	};
	onmessage("dialogover",101503)
	{
		wait(10);
		publishgfxevent("ge_show_skip","ui",0);
		sendgfxmessage("Main Camera","DimScreen",300);
		wait(300);
		//cameraheight(-1,10);
		//cameradistance(-1,10);
		destroynpc(10001);
		playerselfface(0.4523393);
		publishlogicevent("ge_change_player_movemode","game",2);
		wait(10);
		showwall("AirWall_101501",true);
		showwall("AirWall_101502",true);
		showwall("AirWall_101503",true);
		showwall("AirWall_101504",true);
		showwall("AtoB",true);
		wait(10);
		sendgfxmessage("Main Camera","LightScreen",2000);
		wait(800);
		publishgfxevent("pve_checkpoint_begin","ui_effect","博德遗迹Ⅰ",1,5);
		wait(400);
		createnpc(10003);
		wait(500);
		npcmovewithwaypoints(10003,vector2list("68.00211 15.78048"));
		wait(1000);
		showui(true);
		wait(10);
		publishgfxevent("ge_show_skip","ui",0);
		sendgfxmessage("GfxGameRoot","DisableBloom");
		wait(10);
		loop(5)
	  {
	    createnpc(1001+$$);
	    wait(10);
	    createnpc(1101+$$);
	  };
	  createnpc(1006);
	  publishlogicevent("ge_set_story_state","game",0);
	  publishgfxevent("ge_set_indicator_visible","indicator");
	  wait(500);
	  @Tencount = 1;
	  wait(500);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("objkilled")
	{
		@Alive = $1;
		if(@Tencount == 1 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(1201+$$);
			};
			wait(500);
			@Tencount = 2;
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
		if(@Tencount == 2 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(1301+$$);
			};
			wait(500);
			@Tencount = 3;
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
		if(@Tencount == 3 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(1401+$$);
			};
			@Tencount = 0;
			wait(500);
			inc(@Stage);
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
	};
	onmessage("allnpckilled")
	{
		if(@Stage > 0)
		{
		wait(600);
		publishlogicevent("ge_area_clear", "game",0);
		wait(1500);
		showwall("AtoB",false);
		wait(100);
		restartareamonitor(2);
		}；
	};
	onmessage("anyuserenterarea",2)
	{
		//showwall("BDoor",true);
		startstory(2);
		terminate();	  
	};
  onmessage("missionfailed")
  {
    changescene(0);
    terminate();
  };
};
story(2)
{
	local
  {
    @Stage(0);
    @Tencount(0);
    @Alive(0);
  };
	onmessage("start")
	{
		wait(10);
	  loop(6)
	  {
	    createnpc(2001+$$);
	  };
	  wait(10);
	  loop(3)
	  {
	    createnpc(2101+$$);
	  };
	  wait(500);
	  @Tencount = 1;
	  wait(500);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("objkilled")
	{
		@Alive = $1;
		if(@Tencount == 1 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(2201+$$);
			};
			wait(500);
			@Tencount = 2;
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
		if(@Tencount == 2 && @Alive < 6)
		{
			loop(4)
			{
				createnpc(2301+$$);
			};
			@Tencount = 0;
			wait(500);
			inc(@Stage);
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
	};
	onmessage("allnpckilled")
	{
		if(@Stage > 0)
		{
			wait(600);
			publishlogicevent("ge_area_clear", "game",0);
			wait(1500);
			showwall("BtoC",false);
			wait(100);
			restartareamonitor(3);
		};
	};
	onmessage("anyuserenterarea",3)
	{
		//showwall("CDoor",true);
		startstory(3);
		terminate();
	};
  onmessage("missionfailed")
  {
    changescene(0);
    terminate();
  };
};
story(3)
{
	local
  {
    @Stage(0);
    @Tencount(0);
    @Alive(0);
  };
	onmessage("start")
	{
		wait(1500);
	  loop(3)
	  {
	    createnpc(3101+$$);
	  };
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	  wait(2000);
	  loop(3)
	  {
	    createnpc(3201+$$);
	  };
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	  wait(2000);
	  loop(3)
	  {
	    createnpc(3101+$$);
	  };
	  wait(500);
	  @Tencount = 1;
	  wait(500);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("objkilled")
	{
		@Alive = $1;
		if(@Tencount == 1 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(3201+$$);
			};
			@Tencount = 0;
			wait(500);
			inc(@Stage);
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
	};
	onmessage("allnpckilled")
	{
		if(@Stage > 0)
		{
			wait(600);
			publishlogicevent("ge_area_clear", "game",0);
			wait(1500);
			showwall("CtoD",false);
			wait(100);
			restartareamonitor(4);
		};
	};
	onmessage("anyuserenterarea",4)
	{
		//showwall("CDoor",true);
		startstory(4);
		terminate();
	};
  onmessage("missionfailed")
  {
    changescene(0);
    terminate();
  };
};
story(4)
{
  local
  {
    @Stage(0);
    @Tencount(0);
    @Alive(0);
  };
	onmessage("start")
	{	
		wait(10);
	  loop(5)
	  {
  	  createnpc(4001+$$);
  	};  	
  	wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	  wait(3500);
	  loop(3)
	  {
  	  createnpc(4101+$$);
  	};  	
  	wait(500);
	  @Tencount = 1;
	  wait(500);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("objkilled")
	{
		@Alive = $1;
		if(@Tencount == 1 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(4201+$$);
			};
			wait(500);
			@Tencount = 2;
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
		if(@Tencount == 2 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(4101+$$);
			};
			wait(500);
			@Tencount = 3;
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
		if(@Tencount == 3 && @Alive < 7)
		{
			loop(3)
			{
				createnpc(4201+$$);
			};
			@Tencount = 0;
			wait(500);
			inc(@Stage);
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
	};
	onmessage("allnpckilled")
	{
		if(@Stage > 0)
		{
			@Stage = 0;
    //camerayaw(-80,3100);
    //wait(500);
    //cameraheight(2.3,10);
	  //cameradistance(7.6,10);
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
		//camerayaw(0,100);
	  //cameraheight(-1,100);
	  //cameradistance(-1,100);
	  wait(1000);
	  while(isplayerselfbusy()) 
	  {
	  	wait(1000);
	  };
	  sendgfxmessage("GfxGameRoot","EnableBloom");
	  localmessage("EndStory");
		};
	};
	onmessage("EndStory")
	{
		wait(10);
		sendgfxmessage("Main Camera","DimScreen",1000);
		wait(900);
		showui(false);
		publishlogicevent("ge_set_story_state","game",1);
		destroynpc(10003);
		wait(100);
		sendgfxmessage("StoryObj_1015","SetPlayerselfPosition",19.4623,156.3209,43.99448);
		wait(100);
		playerselfface(5.1770918);
		createnpc(11001);
		createnpc(11002);
		cameraheight(11.5,10);
		cameradistance(17,10);
		cameralookatimmediately(20.55713,156.4059,43.65404);
		sendgfxmessage("Main Camera","LightScreen",2000);
		wait(800);
		showdlg(101504);
		showwall("AirWall_101505",false);
		showwall("AirWall_101506",false);
		showwall("AirWall_101507",false);
	};
	onmessage("dialogover",101504)
	{
		wait(10);
		showdlg(101505);
		wait(1900);
		npcmovewithwaypoints(11002,vector2list("23.01643 45.79068 23.52709 50.77937 25.25401 53.63 28.08533 58.29634 31.20151 63.75754"));
	};
	onmessage("dialogover",101505)
	{
		wait(10);
		npcface(11001,0.8363775);
	  playerselfface(0.6616599);
	  wait(1500);
	  showdlg(101506);
	  wait(1800);
	  playerselfface(3.7736411);
	  publishlogicevent("ge_change_npc_movemode","game",unitid2objid(11001),2);
	  publishlogicevent("ge_change_player_movemode","game",2);
	};
	onmessage("dialogover",101506)
	{
		wait(100);
		playerselfmovewithwaypoints(vector2list("22.85967 45.91608 24.02419 49.01258 25.21655 53.32608"));
		npcmovewithwaypoints(11001,vector2list("20.9049 42.18459 22.74686 45.02726 24.04854 48.7323 25.18896 53.39988"));
		wait(300);
		sendgfxmessage("Main Camera","DimScreen",2500);
	  wait(10);
	  showdlg(101507);
	  wait(2300);
	  publishlogicevent("ge_area_clear", "game",1);
	  wait(10);
	  startstory(5);
		terminate();
	};
	onmessage("missionfailed")
  {
    changescene(0);
    terminate();
  };
};
story(5)
{
	local
  {
    @reconnectCount(0);
  };
	onmessage("start")
	{
		//publishlogicevent("ge_set_story_state","game",0);
	  wait(2000);
		loop(10) 
		{ 
		  //检测网络状态 
		  while(!islobbyconnected() && @reconnectCount<10) 
		  { 
		    reconnectlobby();
		    wait(3000);
		    inc(@reconnectCount);
		    loop(10)
		    {
		      if(islobbylogining())
		      {
		        wait(1000);
		      };
		    };
		    if(islobbylogining())
		    {
		      disconnectlobby();
		    };
		  };
		  if(islobbyconnected() && !islobbylogining()) 
		  { 
		    missioncompleted(0); 
		    wait(21000);
		    disconnectlobby(); 
		  } else {
		    wait(10000); 
		    //terminate(); 
		  };
		}; 
		changescene(0);
		terminate(); 
		};
	};
  onmessage("missionfailed")
  {
    changescene(0);
    terminate();
  };
};
