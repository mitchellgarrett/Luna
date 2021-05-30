using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;

namespace Luna {

    public partial class FileExplorerPage : ContentPage {

        public FileExplorerPage() {
            InitializeComponent();
            LoadFiles(Config.RootFolder);
        }

        void LoadFiles(string folder) {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            File.Create(Config.GetFilePath("test.lua"));
            //File.WriteAllText(Path.Combine(folder, "test.lua"), "print(1)");
            File.Create(Config.GetFilePath("test2.lua"));

            string[] filenames = Directory.GetFiles(folder, "*.lua", SearchOption.TopDirectoryOnly);
            for (int i = filenames.Length - 1; i >= 0; --i) {
                Console.WriteLine(filenames[i]);
                Button button = new Button();
                int index = i;
                button.Clicked += (s, e) => { LoadFile(filenames[index]); };
                button.Text = Path.GetFileName(filenames[i]);
                fileList.Children.Add(button);
            }
        }

        void LoadFile(string file) {
            Navigation.PushAsync(new CodeEditorPage(file));
        }
    }
}
