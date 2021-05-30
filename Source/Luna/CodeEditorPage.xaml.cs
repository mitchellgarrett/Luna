using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using MoonSharp.Interpreter;
using Luna.Library;

namespace Luna {

    public partial class CodeEditorPage : ContentPage { 

        readonly string FILE_PATH;
        readonly string FILE_NAME;

        bool programRunning;
        bool inputCompleted;
        
        public CodeEditorPage(string file) {
            InitializeComponent();
            FILE_PATH = file;
            FILE_NAME = Path.GetFileName(file);

            page.Title = FILE_NAME;
            filename.Text = FILE_NAME;
            editor.TextChanged += OnEditorTextChanged;
            LoadLibraries();
            OnLoad(null, null);
        }

        void OnSave(object sender, EventArgs e) {
            File.WriteAllText(Config.GetFilePath(FILE_PATH), editor.Text);
        }

        void OnLoad(object sender, EventArgs e) {
            if (File.Exists(Config.GetFilePath(FILE_PATH))) {
                editor.Text = File.ReadAllText(Config.GetFilePath(FILE_PATH));
            }
        }

        void OnRun(object sender, EventArgs e) {
            ConsoleClear();
            InputClear();

            string src = @editor.Text;
            if (string.IsNullOrEmpty(src)) {
                return;
            }

            Script script = GenerateLuaScript();
            Task.Run(() => {

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
                programRunning = false;
                Device.BeginInvokeOnMainThread(() => { run.IsEnabled = true; });
            });

            programRunning = true;
            run.IsEnabled = false;
        }

        Script GenerateLuaScript() {
            CoreModules modules = CoreModules.Preset_Default;
            Script script = new Script(modules);

            script.Options.DebugPrint = s => ConsolePrint(s);

            script.Globals["io"] = typeof(Lua_IO);
            script.Globals["sys"] = typeof(Lua_System);

            return script;
        }

        void LoadLibraries() {
            UserData.RegisterType(typeof(Lua_IO));
            UserData.RegisterType(typeof(Lua_System));

            Lua_IO.WriteAction += ConsoleWrite;
            Lua_IO.PrintAction += ConsolePrint;
            Lua_IO.ReadAction += ConsoleRead;
            Lua_System.ClearAction += ConsoleClear;
        }

        void OnEditorTextChanged(object sender, TextChangedEventArgs args) {
            if (string.IsNullOrEmpty(args.NewTextValue)) {
                return;
            }

            string text = args.NewTextValue;

            /*for (int i = 0; i < text.Length; ++i) {
                switch (text[i]) {
                    case (char)8220: text = text.Substring(0, i) + DOUBLE_QUOTE + text.Substring(i, text.Length - i); break;
                    case (char)8221: text = text.Substring(0, i) + DOUBLE_QUOTE + text.Substring(i, text.Length - i); break;
                }
            }*/

            text = text.Replace((char)8220, Config.DOUBLE_QUOTE);
            text = text.Replace((char)8221, Config.DOUBLE_QUOTE);

            text = text.Replace((char)8216, Config.SINGLE_QUOTE);
            text = text.Replace((char)8217, Config.SINGLE_QUOTE);
            bool valueChanged = true;
            /*char c = text.Last();
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
            }*/

            if (valueChanged) {
                (sender as Editor).Text = text;
            }
        }

        void ConsoleClear() {
            Device.BeginInvokeOnMainThread(() => { console.Text = string.Empty; });
        }

        void InputClear() {
            Device.BeginInvokeOnMainThread(() => { input.Text = string.Empty; });
        }

        void OnInput(object sender, EventArgs e) {
            inputCompleted = true;
        }

        void ConsoleWrite(object s) {
            Device.BeginInvokeOnMainThread(() => { console.Text += s.ToString(); });
        }

        void ConsolePrint(object s) {
            ConsoleWrite(s.ToString() + "\n");
        }

        string ConsoleRead() {
            while (!inputCompleted);
            inputCompleted = false;
            string text = input.Text;
            InputClear();
            return text;
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
