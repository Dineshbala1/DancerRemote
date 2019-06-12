using System;
using DancerRemote.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DancerRemote
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage
    {
        
        private string _fromMessage = string.Empty;

        public EditorPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string, string>("SongFilePath", "SongFilePathMessage",
                (s, args) => { _fromMessage = args; });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!string.IsNullOrEmpty(_fromMessage))
            {
                ((EditorPageViewModel) BindingContext).SongFilePath = _fromMessage;
            }
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new SongsPage());
        }
    }
}