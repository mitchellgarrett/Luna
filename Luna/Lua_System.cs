using System;
using System.Threading;
using MoonSharp.Interpreter;

namespace Luna {

    [MoonSharpUserData]
    public static class Lua_System {

        public static Action ClearAction;
        public static Action<string> WriteAction;
        public static Action<string> PrintAction;
        public static Func<string> ReadAction;

        public static void clear() {
            ClearAction?.Invoke();
        }

        public static void write(string s) {
            WriteAction?.Invoke(s);
        }

        public static void print(string s) {
            PrintAction?.Invoke(s);
        }

        public static string read() {
            return ReadAction?.Invoke();
        }

        public static void sleep(int ms) {
            Thread.Sleep(ms);
        }

    }
}
