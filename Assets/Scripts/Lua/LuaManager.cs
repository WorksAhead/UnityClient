using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using ArkCrossEngine;

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
        luaEnv.AddLoader(CustomLoader);
    }

    public void Tick()
    {
        if (UnityEngine.Time.time - LastGCTime > GCInterval)
        {
            luaEnv.Tick();
            LastGCTime = UnityEngine.Time.time;
        }
    }

    public void DisposeEnv()
    {
        luaEnv.Dispose();
    }

    /// note: debugger can`t capture dostring calls
    public void DoString(string luaText, LuaTable table = null )
    {
        Env.DoString(luaText, "Lua", table);
    }

    public void DoFile(string path, LuaTable table = null)
    {
        Env.DoString("require " + "\"" + path + "\"", "Lua");
    }

    // input:  argument of lua require call, require full path if a remote debugger attached
    // output: null if no file found or UTF-8 string
    public byte[] CustomLoader(ref string filepath)
    {
        string fileName = filepath + ".lua";
        ArkCrossEngine.Object obj = ResourceSystem.GetSharedResource(fileName);
        TextAsset text = CrossObjectHelper.TryCastObject<UnityEngine.TextAsset>(obj);
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
        ArkCrossEngine.Object obj = ResourceSystem.GetSharedResource(fileName);
        TextAsset text = CrossObjectHelper.TryCastObject<UnityEngine.TextAsset>(obj);
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
