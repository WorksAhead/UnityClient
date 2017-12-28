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

        public override void ExecuteBuffer ( string scriptData, object extra = null, string chunk = "default" )
        {
            if (scriptData != null)
            {
                // no check
                // if (extra is LuaTable)
                {
                    Env_.DoString(scriptData, chunk, extra as LuaTable);
                }
            }
        }

        public override void ExecuteFile ( string file, object extra = null, string chunk = "default" )
        {
            if ( file != null )
            {
                // no check
                // if (extra is LuaTable)
                {
                    Env_.DoString("require " + "\"" + file + "\"", chunk, extra as LuaTable);
                }
            }
        }

        public override object SetupNewEnv ( string scriptEnv )
        {
            LuaTable table = Env_.NewTable();
            LuaTable metaTable = Env_.NewTable();
            metaTable.Set("__index", Env_.Global);
            table.SetMetaTable(metaTable);
            metaTable.Dispose();
            return table;
        }

        public override Action QueryAction ( string action, object env )
        {
            Action act;
            LuaTable table = (LuaTable)env;
            table.Get(action, out act);
            return act;
        }

        public override Action<object> QueryAction_1 ( string action, object env )
        {
            Action<object> act;
            LuaTable table = (LuaTable)env;
            table.Get(action, out act);
            return act;
        }

        public override Action[] QueryActions ( string[] actions, object env )
        {
            Action[] acts = new Action[actions.Length];
            for ( int i = 0; i < actions.Length; ++i )
            {
                acts[i] = QueryAction(actions[i], env);
            }
            return acts;
        }

        public override Action<object>[] QueryActions_1 ( string[] actions, object env )
        {
            Action<object>[] acts = new Action<object>[actions.Length];
            for ( int i = 0; i < actions.Length; ++i )
            {
                acts[i] = QueryAction_1(actions[i], env);
            }
            return acts;
        }

        private LuaEnv Env_ = new LuaEnv();
        long LastGCTime = 0;
        const long GCInterval = 1000;
    }
}