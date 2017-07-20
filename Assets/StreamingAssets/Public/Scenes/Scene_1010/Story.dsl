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
		cameraheight(6.5,10);
		cameradistance(6,10);
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
		
	};
};
story(2)
{  
};
story(3)
{  
};
