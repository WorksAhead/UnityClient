using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if ENABLE_LUA
using XLua;
#endif

[Serializable]
public class Injection
{
    public string Name;
    public object Object;
}
#if ENABLE_LUA
[LuaCallCSharp]
#endif
public class LuaBehaviour : MonoBehaviour
{
    public TextAsset LuaScript;
    public Injection[] Injections;

    /// common function call
    private Action LuafAwake;
    private Action LuafStart;
    private Action LuafUpdate;
    private Action LuafDestroy;

    /// self table
#if ENABLE_LUA
    private LuaTable ScriptEnv;
#endif
    
    void Awake()
    {
#if ENABLE_LUA
        LuaEnv env = LuaManager.Instance.Env;

        // set index meta table
        ScriptEnv = env.NewTable();
        LuaTable metaTable = env.NewTable();
        metaTable.Set("__index", env.Global);
        ScriptEnv.SetMetaTable(metaTable);
        metaTable.Dispose();

        // set self ptr
        ScriptEnv.Set("self", this);

        // register injections
        foreach( var injection in Injections )
        {
            ScriptEnv.Set(injection, injection.Object);
        }

        // execute lua impl
        ExecuteLua();

        // get common actions
        ScriptEnv.Get("awake", out LuafAwake);
        ScriptEnv.Get("start", out LuafStart);
        ScriptEnv.Get("update", out LuafUpdate);
        ScriptEnv.Get("ondestroy", out LuafDestroy);

        // fire awake in lua
        if (LuafAwake != null)
        {
            LuafAwake();
        }
#endif
    }

    void Start ()
    {
		if (LuafStart != null)
        {
            LuafStart();
        }
	}
	
	void Update ()
    {
		if (LuafUpdate != null)
        {
            LuafUpdate();
        }
	}

    void OnDestroy()
    {
        if (LuafDestroy != null)
        {
            LuafDestroy();
        }

        LuafAwake = null;
        LuafStart = null;
        LuafUpdate = null;
        LuafDestroy = null;
        Injections = null;
#if ENABLE_LUA
        ScriptEnv.Dispose();
#endif
    }

    void ExecuteLua()
    {
#if ENABLE_LUA
        string luaExecutable = LuaManager.Instance.LoadLuaFromFile(System.IO.Path.GetFileNameWithoutExtension(LuaScript.name));
        LuaManager.Instance.DoString(luaExecutable, ScriptEnv);
#endif
    }
}
