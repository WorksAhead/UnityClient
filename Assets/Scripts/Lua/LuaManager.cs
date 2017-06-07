using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaManager
{
    #region Singleton
    private static LuaManager s_instance_ = new LuaManager();
    public static LuaManager Instance
    {
        get { return s_instance_; }
    }
    #endregion

    /// GC counter
    internal float LastGCTime = 0;
    internal const float GCInterval = 1;

    private LuaEnv luaEnv;
    public LuaEnv Env
    {
        get { return luaEnv; }
        set { luaEnv = value; }
    }

    public void InitEnv()
    {
        luaEnv = new LuaEnv();
    }

    public void Tick()
    {
        if (Time.time - LastGCTime > GCInterval)
        {
            luaEnv.Tick();
            LastGCTime = Time.time;
        }
    }

    public void DisposeEnv()
    {

    }
}
