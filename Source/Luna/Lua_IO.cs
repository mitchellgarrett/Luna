using System;
using MoonSharp.Interpreter;

namespace Luna.Library {

    [MoonSharpUserData]
    public static class Lua_IO {

        public static Action<object> WriteAction;
        public static Action<object> PrintAction;
        public static Func<string> ReadAction;

        public static void write(object s) {
            WriteAction?.Invoke(s);
        }

        public static void print(object s) {
            PrintAction?.Invoke(s);
        }

        public static string read() {
            return ReadAction?.Invoke();
        }
    }
}
