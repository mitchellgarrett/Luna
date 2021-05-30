using UIKit;

using Xamarin.Forms;

namespace Luna {

    public partial class MainPage : ContentPage {

        public MainPage() {
            InitializeComponent();
            image.Source = Xamarin.Forms.ImageSource.FromStream(() => (UIImage.GetSystemImage("file.text")).AsPNG().AsStream());
        }

        void OnNewFile(System.Object sender, System.EventArgs e) {
            Navigation.PushAsync(new CodeEditorPage("files/untitled.lua"));
        }

        void OnEditFile(System.Object sender, System.EventArgs e) {
            Navigation.PushAsync(new FileExplorerPage());
        }
    }
}
