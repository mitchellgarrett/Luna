using System;
using System.Threading;
using MoonSharp.Interpreter;

namespace Luna.Library {

    [MoonSharpUserData]
    public static class Lua_System {

        public static Action ClearAction;

        public static void clear() {
            ClearAction?.Invoke();
        }

        public static void sleep(int ms) {
            Thread.Sleep(ms);
        }
    }
}
