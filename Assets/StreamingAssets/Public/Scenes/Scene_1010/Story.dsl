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
		camerayaw(3.14159,10);
		cameraheight(11.5,10);
		cameradistance(12,10);
		camerafollow();
		wait(10);

		sendgfxmessage("Portal_PVE","PlayParticle");
		sendgfxmessage("Portal_PVP","PlayParticle");
		sendgfxmessage("Portal_World","PlayParticle");
		wait(10);

		loop(3)
		{
		  createnpc(1006+$$);
		};
	};

	//虚拟玩家移动
	onmessage("cityusermove")
	{
	  objmove($0,vector3($1,0,$2));
	};
	onmessage("objarrived")
	{
	  objface($0,-1);
	};

	//0——单人PVE，1——多人活动，2——PVP
	onmessage("cityplayermove")
	{
		if($0==0)
		{	
			@targetPt = vector3(64.54072 172.08 39.34027);
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
			@targetPt = vector3(71.92841 0.07996559 -6.046996);
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
			@targetPt = vector3(103.5865 0.07999742 -6.541862);
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
		
		message("playermovetopos")
		playerselfmove(vector3($0,$1,$2));
		
		message("aimovestopped")
		
	};
};
story(2)
{  
};
story(3)
{  
};
