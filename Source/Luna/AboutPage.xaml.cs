using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace Luna {
    public partial class AboutPage : ContentPage {
        public AboutPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            versionLabel.Text = $" v{AppInfo.VersionString}";
        }
    }
}
