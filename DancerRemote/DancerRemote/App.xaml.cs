using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DancerRemote
{
    public partial class App : Application
    {

        public static string SelectedFilePath = string.Empty;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new HomePage()) {BackgroundImageSource = "background"};
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
