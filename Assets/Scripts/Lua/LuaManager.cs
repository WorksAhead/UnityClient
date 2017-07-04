using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using ArkCrossEngine;

#if ENABLE_LUA
using XLua;
#endif

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

#if ENABLE_LUA
    private LuaEnv luaEnv;
    public LuaEnv Env
    {
        get { return luaEnv; }
        set { luaEnv = value; }
    }
#endif

    public void InitEnv()
    {
#if ENABLE_LUA
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(CustomLoader);
#endif
    }

    public void Tick()
    {
#if ENABLE_LUA
        if (UnityEngine.Time.time - LastGCTime > GCInterval)
        {
            luaEnv.Tick();
            LastGCTime = UnityEngine.Time.time;
        }
#endif 
    }

    public void DisposeEnv()
    {
#if ENABLE_LUA
        luaEnv.Dispose();
#endif  
    }

#if ENABLE_LUA
    /// note: debugger can`t capture dostring calls
    public void DoString(string luaText, LuaTable table = null )
    {
        Env.DoString(luaText, "Lua", table);
    }

    public void DoFile(string path, LuaTable table = null)
    {
        Env.DoString("require " + "\"" + path + "\"", "Lua");
    }
#endif

    // input:  argument of lua require call, require full path if a remote debugger attached
    // output: null if no file found or UTF-8 string
    public byte[] CustomLoader(ref string filepath)
    {
        string fileName = filepath + ".lua";
        TextAsset text = ResourceSystem.GetSharedResource(fileName) as TextAsset;
        if (text != null)
        {
            // for debugger
            filepath = GetLuaPathFroDebugger(filepath);
            return System.Text.Encoding.UTF8.GetBytes(text.text);
        }
        else
        {
            return null;
        }
    }

    private string GetLuaPathFroDebugger(string path)
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        return UnityEngine.Application.dataPath + "/Scripts/Lua/LuaScripts/Resources/" + path + ".lua.txt";
#else
        // use your own native lua file path
        return "F:/ArkCrossEngine/Client/Publish/Assets/Scripts/Lua/LuaScripts/Resources/" + path + ".lua.txt";
#endif
    }

    public string LoadLuaFromFile(string name)
    {
        string fileName = name + ".lua";
        TextAsset text = ResourceSystem.GetSharedResource(fileName) as TextAsset;
        if (text != null)
        {
            return text.text;
        }
        else
        {
            return null;
        }
    }
}
