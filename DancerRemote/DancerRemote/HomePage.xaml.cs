using System;
using Plugin.FilePicker.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DancerRemote
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {

        public static FileData FilePath = null;

        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new EditorPage(),true);
        }
    }

    public interface INavigationAndroid
    {
        void Navigate(string filePath);
        
    }
}