using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;

using MoonSharp.Interpreter;
using System.IO;

namespace Luna {
    public partial class MainPage : ContentPage {

        const char DOUBLE_QUOTE = '\"';
        const char SINGLE_QUOTE = '\'';

        public MainPage() {
            InitializeComponent();
            //input.Completed += (sender, e) => { ConsolePrint(((Label)sender).Text); };

            editor.TextChanged += OnEditorTextChanged;
            LoadLibraries();
        }

        void OnSave(System.Object sender, System.EventArgs e) {
            File.WriteAllText(GetFileName(filename.Text), editor.Text);
        }

        void OnLoad(System.Object sender, System.EventArgs e) {
            if (File.Exists(GetFileName(filename.Text))) {
                editor.Text = File.ReadAllText(GetFileName(filename.Text));
            }
        }

        void OnRun(System.Object sender, System.EventArgs e) {
            ConsoleClear();

            string src = @editor.Text;
            if (string.IsNullOrEmpty(src)) {
                return;
            }

            //Debug.WriteLine(src);

            Script script = GenerateLuaScript();
            try {
                DynValue val = script.DoString(src);
                if (val.IsNotVoid()) {
                    ConsolePrint(val.ToString());
                }
            } catch (InterpreterException ie) {
                string error_msg = FormatExceptionMessage(ie);
                Debug.WriteLine(error_msg);
                ConsolePrint(error_msg);
            }

        }

        Script GenerateLuaScript() {

            CoreModules modules = CoreModules.Preset_Default;
            UserData.RegisterAssembly();
            Script script = new Script(modules);

            script.Options.DebugPrint = s => ConsolePrint(s);
            script.Options.DebugInput = s => { return ConsoleRead(); };

            script.Globals["sys"] = typeof(Lua_System);

            return script;
        }

        void LoadLibraries() {
            Lua_System.ClearAction += ConsoleClear;
            Lua_System.WriteAction += ConsoleWrite;
            Lua_System.PrintAction += ConsolePrint;
            Lua_System.ReadAction += ConsoleRead;
        }

        void OnEditorTextChanged(object sender, TextChangedEventArgs args) {
            if (string.IsNullOrEmpty(args.NewTextValue)) {
                return;
            }
            

            string text = args.NewTextValue;
            char c = text.Last();
            bool valueChanged = false;
            switch ((int)c) {
                case 8220:
                case 8221:
                    valueChanged = true;
                    text = text.Substring(0, text.Length - 1) + DOUBLE_QUOTE;
                    break;

                case 8216:
                case 8217:
                    valueChanged = true;
                    text = text.Substring(0, text.Length - 1) + SINGLE_QUOTE;
                    break;
            }

            if (valueChanged) {
                (sender as Editor).Text = text;
            }
        }

        void ConsoleClear() {
            console.Text = "";
            input.Text = "";
        }

        void ConsoleWrite(string s) {
            console.Text += s;
        }
        void ConsolePrint(string s) {
            ConsoleWrite(s + "\n");
        }

        string ConsoleRead() {
            return "hello";
            return input.Text;
        }

        string GetFileName(string file) {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
        }

        string FormatExceptionMessage(InterpreterException e) {

            string msg = "";
            if (e is InternalErrorException) {
                msg += "Internal Error ";
            } else if (e is SyntaxErrorException) {
                msg += "Syntax Error ";
            } else if (e is DynamicExpressionException) {
                msg += "Dynamic Error ";
            } else if (e is ScriptRuntimeException) {
                msg += "Runtime Error ";
            } else {
                msg += "Interpreter Error";
            }

            string error_msg = e.DecoratedMessage;
            int pos = error_msg.IndexOf('(');
            int length = error_msg.LastIndexOf(')') - pos + 1;

            msg += error_msg.Substring(pos, length) + ": ";
            msg += error_msg.Substring(error_msg.IndexOf(' '));

            return msg;
        }

    }
}
