using Xamarin.Forms;

namespace Luna {

    public partial class MainPage : ContentPage {

        public MainPage() {
            InitializeComponent();
            //image.Source = Xamarin.Forms.ImageSource.FromStream(() => (UIImage.GetSystemImage("file.text")).AsPNG().AsStream());
        }

        void LoadNewPage(System.Object sender, System.EventArgs e) {
            Navigation.PushAsync(new CodeEditorPage(Config.GetFilePath("untitled.lua")));
        }

        void LoadEditPage(System.Object sender, System.EventArgs e) {
            Navigation.PushAsync(new FileExplorerPage());
        }

        void LoadAboutPage(System.Object sender, System.EventArgs e) {
            Navigation.PushAsync(new AboutPage());
        }
    }
}
