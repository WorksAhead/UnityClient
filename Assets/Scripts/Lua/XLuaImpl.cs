using ArkCrossEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLua;

namespace ScriptableData
{
    internal class XLuaImpl : LuaInterface
    {
        public override void Init( ScriptManager.CustomLoaderDelegate func, string luaDebuggerPath = "" )
        {
            NativeObject = Env_;
            LuaScriptPathForDebugger = luaDebuggerPath;
            Env_.AddLoader(( ref string file ) => 
            {
                if (func != null)
                {
                    return func(ref file);
                }
                return null;
            } );
        }

        public override void Tick()
        {
            long now = TimeUtility.GetLocalMilliseconds();
            if ( now - LastGCTime > GCInterval )
            {
                Env_.Tick();
                LastGCTime = now;
            }
        }

        public override void Destroy()
        {
            Env_.Dispose();
        }

        public override void ExecuteBuffer ( string scriptData, object extra = null )
        {
            if (scriptData != null)
            {
                // no check
                // if (extra is LuaTable)
                {
                    Env_.DoString(scriptData, "default", extra as LuaTable);
                }
            }
        }

        public override void ExecuteFile ( string file, object extra = null )
        {
            if ( file != null )
            {
                // no check
                // if (extra is LuaTable)
                {
                    Env_.DoString("require " + "\"" + file + "\"", "default", extra as LuaTable);
                }
            }
        }

        private LuaEnv Env_ = new LuaEnv();
        long LastGCTime = 0;
        const long GCInterval = 1000;
    }
}