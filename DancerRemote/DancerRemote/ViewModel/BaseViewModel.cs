using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace DancerRemote.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _pageTitle;
        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseViewModel()
        {
            //PopupService = DependencyService.Get<IPopupService>();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }
    }
}