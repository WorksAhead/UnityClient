story(1)
{ 
	local
  {
    @Stage(0);
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
		cameradistance(16,10);
		cameralookatimmediately(27.12691,159.9652,10.15036);
		publishgfxevent("ge_set_indicator_invisible","indicator");
		wait(10);
	  sendgfxmessage("Main Camera","LightScreen",2000);
		wait(1000);
		showdlg(102501);
		showwall("AirWall_1025A1",false);
		showwall("AirWall_1025A2",false);
		wait(3000);
		//publishgfxevent("ge_show_skip","ui",1);
	};
	onmessage("dialogover",102501)
	{
		wait(10);
		npcmovewithwaypoints(10002,vector2list("35.42746 17.06449 44.77301 25.49537"));
		wait(100);
		showdlg(102502);
	};
	onmessage("npcarrived",10002)
	{
		destroynpc(10002);
	};
	onmessage("dialogover",102502)
	{
		wait(10);
		npcmovewithwaypoints(10003,vector2list("15.15939 1.936505"));
		wait(10);
		showdlg(102503);
	};
	onmessage("dialogover",102503)
	{
		wait(10);
		publishgfxevent("ge_show_skip","ui",0);
		sendgfxmessage("Main Camera","DimScreen",1000);
		wait(1000);
		loop(3)
	  {
	    destroynpc(10001+$$);
	  };
	  wait(10);
	  sendgfxmessage("StoryObj_1025","SetVisible",0);
	  cameraheight(-1,10);
		cameradistance(-1,10);
		camerafollowimmediately();
		wait(10);
		createnpc(10004);
		wait(10);
		loop(5)
	  {
	    createnpc(1001+$$);
	  };
		showwall("AirWall_1025A1",true);
		showwall("AirWall_1025A2",true);
		wait(10);
		sendgfxmessage("Main Camera","LightScreen",2000);
		wait(800);
		publishgfxevent("pve_checkpoint_begin","ui_effect","潜入军营",2,5);
		wait(1500);
		showui(true);
		wait(10);
		publishgfxevent("ge_show_skip","ui",0);
		sendgfxmessage("GfxGameRoot","DisableBloom");
		wait(10);
		loop(3)
	  {
	    createnpc(1101+$$);
	  };
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
		if(@Tencount == 2 && @Alive < 6)
		{
		  loop(4)
		  {
		    createnpc(1301+$$);
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
		};
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
	  loop(4)
	  {
	    createnpc(2001+$$);
	  };
	  wait(1000);
	  loop(4)
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
		if(@Tencount == 1 && @Alive < 5)
		{
		  loop(4)
		  {
		    createnpc(2201+$$);
		  };
		  wait(500);
			@Tencount = 2;
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
		if(@Tencount == 2 && @Alive < 5)
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
		wait(10);
	  loop(5)
	  {
	    createnpc(3001+$$);
	  };
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	  wait(4000);
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
		if(@Tencount == 1 && @Alive < 5)
		{
		  loop(3)
		  {
		    createnpc(3201+$$);
		  };
		  wait(500);
			@Tencount = 2;
			wait(500);
			setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
		};
		if(@Tencount == 2 && @Alive < 5)
		{
		  loop(4)
		  {
		    createnpc(3301+$$);
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
		showui(false);
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
	  wait(10);
		sendgfxmessage("Main Camera","DimScreen",1000);
		wait(1000);
		publishlogicevent("ge_set_story_state","game",1);
		publishgfxevent("ge_set_indicator_invisible","indicator");
		wait(10);
		destroynpc(10004);
		wait(10);
		sendgfxmessage("StoryObj_1025","SetVisible",1);
		sendgfxmessage("StoryObj_1025","SetPlayerselfPosition",42.44723,155.8971,56.57144);
		wait(10);
		playerselfface(4.0399207);
		createnpc(11001);
		createnpc(11002);
		cameraheight(11,10);
		cameradistance(18,10);
		cameralookatimmediately(40.37395,155.9771,52.13464);
		sendgfxmessage("Main Camera","LightScreen",2000);
		wait(800);
		showdlg(102504);
		showwall("AirWall_1025B1",false);
		showwall("AirWall_1025B2",false);
		showwall("AirWall_1025B3",false);
		wait(10);
		publishlogicevent("ge_change_npc_movemode","game",unitid2objid(11001),1);
		publishlogicevent("ge_change_npc_movemode","game",unitid2objid(11002),1);
	  publishlogicevent("ge_change_player_movemode","game",1);
		};
	};
	onmessage("dialogover",102504)
	{
		wait(10);
		npcmovewithwaypoints(11002,vector2list("33.93243 57.1408 28.93591 57.01743 23.66195 57.35776 17.06144 60.99615"));
		playerselfmovewithwaypoints(vector2list("38.59093 57.44765 34.36547 55.90551 29.92411 56.39899 25.32853 57.23175 17.587 60.74783"));
		npcmovewithwaypoints(11001,vector2list("35.9892 51.39572 32.07217 53.52387 27.44575 54.97348 24.63906 55.86792 16.83584 59.90833"));
		wait(10);
		showdlg(102505);
		wait(1000);
		camerafollow(11002);
		wait(2500);
		sendgfxmessage("Main Camera","DimScreen",3000);
	  wait(2800);
	  publishlogicevent("ge_area_clear", "game",1);
	  wait(10);
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
    @reconnectCount(0);
  };
	onmessage("start")
	{	
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
  onmessage("missionfailed")
  {
    changescene(0);
    terminate();
  };
};
