using System;
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

            fileList.Root = new TableRoot();
            
            TableSection section = new TableSection("Documents");
            string[] filenames = Directory.GetFiles(folder, "*.lua", SearchOption.TopDirectoryOnly);
            for (int i = filenames.Length - 1; i >= 0; --i) {
                ImageCell cell = new ImageCell();
                int index = i;
                cell.Tapped += (s, e) => { LoadFile(filenames[index]); };
                cell.Text = Path.GetFileName(filenames[i]);
                cell.Detail = $"{new FileInfo(filenames[i]).Length} bytes";


                section.Add(cell);
            }
            fileList.Root.Add(section);
        }

        void LoadFile(string file) {
            Console.WriteLine(file);
            Navigation.PushAsync(new CodeEditorPage(file));
        }
    }
}
