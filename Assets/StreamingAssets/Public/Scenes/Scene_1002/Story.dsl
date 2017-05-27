story(1)
{ 
	onmessage("start")
	{
		wait(1900);
		//createnpc(10001);
	  //wait(10);
	  sendgfxmessage("GfxGameRoot","EnableBloom");
		showui(false);
		publishlogicevent("ge_set_story_state","game",1);
		sendgfxmessage("Main Camera","DimScreen",10);
		wait(10);
		sendgfxmessage("Main Camera","LightScreen",3000);
		wait(1000);
		//publishgfxevent("ge_show_skip","ui",1);
    publishgfxevent("ge_set_indicator_invisible","indicator");
	  showdlg(100201);
	};
	onmessage("dialogover",100201)
	{
		wait(10);
		sendgfxmessage("GfxGameRoot","DisableBloom");
		showui(true);
		cameraheight(10.5,10);
		cameradistance(11,10);
		publishgfxevent("ge_show_skip","ui",0);
		wait(30);
		//publishlogicevent("ge_set_story_state","game",0);
	  loop(4)
	  {
	    createnpc(1001+$$);
	  };
	  wait(500);
    publishgfxevent("ge_set_indicator_visible","indicator");
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("SkipStory")
  {
  	wait(10);
  	sendgfxmessage("Main Camera","DimScreen",500);
  	sendgfxmessage("GfxGameRoot","StopCurrentStory",0);
  	wait(500);
  	sendgfxmessage("Main Camera","LightScreen",1000);
  	wait(300);
  	showui(true);
  	cameraheight(10.5,10);
		cameradistance(11,10);
		publishgfxevent("ge_show_skip","ui",0);
		wait(30);
		loop(4)
	  {
	    createnpc(1001+$$);
	  };
	  publishlogicevent("ge_set_story_state","game",0);
	  wait(500);
	  sendgfxmessage("GfxGameRoot","DisableBloom");
    publishgfxevent("ge_set_indicator_visible","indicator");
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
  };
	onmessage("npckilled",1001)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("npckilled",1002)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("npckilled",1003)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("npckilled",1004)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("allnpckilled")
	{
		wait(100);
		npcpatrol(10001,vector2list("51.17324 28.24513"),isnotloop);
		wait(600);
		publishlogicevent("ge_area_clear", "game",0);
		wait(1500);
		showwall("AtoB",false);
		wait(100);
		publishlogicevent("ge_set_story_state","game",0);
		restartareamonitor(2);
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
	onmessage("start")
	{
		wait(10);
	  loop(10)
	  {
	    createnpc(2001+$$);
	  };
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("npckilled",2001)
	{
	  npcpatrol(10001,vector2list("47.03958 38.17242"),isnotloop);
	};
	onmessage("npckilled",2002)
	{
	  npcpatrol(10001,vector2list("47.03958 38.17242"),isnotloop);
	};
	onmessage("npckilled",2005)
	{
	  npcpatrol(10001,vector2list("38.67462 44.81231"),isnotloop);
	};
	onmessage("npckilled",2006)
	{
	  npcpatrol(10001,vector2list("38.67462 44.81231"),isnotloop);
	};
	onmessage("npckilled",2009)
	{
	  npcpatrol(10001,vector2list("38.67462 44.81231"),isnotloop);
	};
	onmessage("npckilled",2003)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2004)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2007)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2008)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2010)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("allnpckilled")
	{
		wait(100);
		npcpatrol(10001,vector2list("47.16977 70.23268"),isnotloop);
		wait(600);
		publishlogicevent("ge_area_clear", "game",0);
		wait(2000);
		showwall("BtoC",false);
		wait(100);
		restartareamonitor(3);
	};
	onmessage("anyuserenterarea",3)
	{
		npcpatrol(10001,vector2list("48.47208 73.41793"),isnotloop);
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
    @reconnectCount(0);
    @rnd(0);
  };
	onmessage("start")
	{	
		/* showui(false);
		enableai(10001,false);
		sendgfxmessage("GfxGameRoot","EnableBloom");
		sendgfxmessage("Main Camera","DimScreen",10);
		createnpc(3001);
		enableai(3001,false);
		camerafollowimmediately(3001);
		wait(10);
		cameraheight(2.3,10);
	  cameradistance(7.6,10);
	  wait(10);
	  sendgfxmessage("Main Camera","LightScreen",500);
	  wait(100);
	  objanimation(unitid2objid(3001),99,2500);
	  wait(1500);
	  //lockframe(0.01);
	  publishgfxevent("pve_boss_enter","ui_effect","驻军千人长");
    wait(1000);
    sendgfxmessage("Main Camera","DimScreen",300);
    wait(290);
    //lockframe(1.0);
    camerafollowimmediately();
    cameraheight(-1,100);
	  cameradistance(-1,100);
		sendgfxmessage("Main Camera","LightScreen",1000);
		wait(300);
		showui(true); */
		wait(10);
		@rnd=rndfloat();
	  loop(8)
	  {
  	  createnpc(3001+$$,@rnd);
  	};
  	//enableai(10001,true);
  	//enableai(3001,true);
  	wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("npckilled",3002)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3003)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3004)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3005)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3006)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3007)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3008)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3009)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("allnpckilled")
	{
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
	  publishlogicevent("ge_area_clear", "game",1);
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
