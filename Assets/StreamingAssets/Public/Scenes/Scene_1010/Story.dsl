story(1)
{  
  local
  {
    @targetPt(vector3(0,0,0));
    @dist(0);
  };
	onmessage("start")
	{
	  sendgfxmessage("Main Camera","StopSound",1);
	  wait(10);
	  sendgfxmessage("Main Camera","PlaySound",0);
	  wait(10);
	  showui(true);
	  lockframe(1.0);
	  wait(10);
	  camerayaw(0,10);
	  cameraheight(-1,10);
		cameradistance(-1,10);
	  camerafollow();
	  showwall("BtoC",false);
	  showwall("AtoB",false);
	  showwall("BDoor",true);
	  wait(10);
	  sendgfxmessage("UIPortal_Activity","PlayParticle");
	  sendgfxmessage("UIPortal_PVE","PlayParticle");
	  sendgfxmessage("UIPortal_PVP","PlayParticle");
	  sendgfxmessage("UIPortal_Gateway","PlayParticle");
	  wait(10);
	  loop(8)
	  {
	    createnpc(1005+$$);
	  };
	  //wait(1000);
	  //setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("npcstore")
	{
	  if($0==1){
	    log("open npcstore");
	  } else {
	    log("close npcstore");	    
	  };
	};
	//虚拟玩家移动
	onmessage("cityusermove")
	{
	  log("cityusermove:{0} {1} {2}",$0,$1,$2);
	  objmove($0,vector3($1,0,$2));
	};
	onmessage("objarrived")
	{
	  log("objarrived, adjust face {0}", $0);
	  objface($0,-1);
	};
	//0——单人PVE，1——多人活动，2——PVP
	onmessage("cityplayermove")
	{
	  if($0==0)
	  {	
	    @targetPt = vector3(64.81986,172.11,31.4238);
	    @dist = vector3dist(getposition(playerselfid()),@targetPt);
	    if(@dist<=1)
	    {
	      sendgfxmessage("GfxGameRoot","ShowWindow","SceneSelect");
	    }
	    else
	    {
  	    playerselfmove(@targetPt);
  	  };
	  };
	  if($0==1)
	  {
	    @targetPt = vector3(63.29525,172.11,42.55972);
	    @dist = vector3dist(getposition(playerselfid()),@targetPt);
	    if(@dist<=1)
	    {
	      sendgfxmessage("GfxGameRoot","ShowWindow","Trial");
	    }
	    else
	    {
  	    playerselfmove(@targetPt);
  	  };
	  };
	  if($0==2)
	  {
	    @targetPt = vector3(40.99107,172.11,45.77026);
	    @dist = vector3dist(getposition(playerselfid()),@targetPt);
	    if(@dist<=1)
	    {
	      sendgfxmessage("GfxGameRoot","ShowWindow","PvPEntrance");
	    }
	    else
	    {
  	    playerselfmove(@targetPt);
  	  };
	  };
	};
	onmessage("playermovetopos")
	{
	  playerselfmove(vector3($0,$1,$2));
	};
    onmessage("aimovestopped")
    {
      log("666");
    };
};
story(2)
{  
};
story(3)
{  
};
