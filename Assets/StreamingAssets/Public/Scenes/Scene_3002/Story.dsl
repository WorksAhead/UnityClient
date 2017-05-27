story(1)
{
  onmessage("start")
  {
    wait(2000);
    publishgfxevent("ge_show_marsloading", "ui");
    wait(6500);
    publishgfxevent("ge_pause_or_resume_backgroud_music","music",0,1);

    //restarttimeout(1);
    //updatecoefficient();
  };
  onmessage("onbeginfight")
  {
    startcountdown(180);
  };
  onmessage("onenemykilled")
  {
    missioncompleted(0);
    lockframe(0.1);
    wait(3000);
    lockframe(1.0);
    objanimation(winuserid(),72);
    objanimation(lostuserid(),73);
    wait(1000);
    publishlogicevent("check_pvap_result", "pvap");
  };
  onmessage("timeout")
  {
    missioncompleted(0);
    lockframe(0.1);
    wait(3000);
    lockframe(1.0);
    objanimation(winuserid(),72);
    objanimation(lostuserid(),73);
    wait(2000);
    publishlogicevent("check_pvap_result", "pvap");
  };
};