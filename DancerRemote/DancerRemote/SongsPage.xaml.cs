using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Plugin.FilePicker.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DancerRemote
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongsPage
    {
        private ObservableCollection<FileData> _files;

        public SongsPage()
        {
            InitializeComponent();
            BindingContext = this;
            Files = new ObservableCollection<FileData>();

            if (Xamarin.Essentials.Preferences.ContainsKey("Files"))
            {
                Files = JsonConvert.DeserializeObject<ObservableCollection<FileData>>(
                    Xamarin.Essentials.Preferences.Get("Files", ""));
            }
        }

        public ObservableCollection<FileData> Files
        {
            get => _files;
            set
            {
                _files = value;
                OnPropertyChanged();
            }
        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            this.activityIndicator.IsRunning = true;
            var result = await DependencyService.Get<DancerRemote.IFilePicker>().PickFile();
            if (result != null)
            {
                Files.Add(result);

                var newData = new FileData(Xamarin.Essentials.FileSystem.AppDataDirectory, result.FileName,
                    () => result.GetStream(), null);

                await DependencyService.Get<DancerRemote.IFilePicker>().SaveFile(newData);
            }

            this.activityIndicator.IsRunning = false;

            var settings = new JsonSerializerSettings
            {
                ContractResolver = ShouldSerializeContractResolver.Instance
            };

            Xamarin.Essentials.Preferences.Set("Files", JsonConvert.SerializeObject(Files, settings));
        }

        private async void SongsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.SelectedFilePath = ((sender as CollectionView).SelectedItem as FileData).FilePath;
            MessagingCenter.Send("SongFilePath", "SongFilePathMessage", App.SelectedFilePath);
            await this.Navigation.PopAsync();
        }
    }

    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        public static ShouldSerializeContractResolver Instance { get; } = new ShouldSerializeContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (typeof(FileData).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(FileData.DataArray))
            {
                property.Ignored = true;
            }
            return property;
        }
    }
}