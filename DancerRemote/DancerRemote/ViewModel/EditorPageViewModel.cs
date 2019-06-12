using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DancerRemote.ViewModel
{
    class EditorPageViewModel : BaseViewModel
    {
        private int _currentCuePoint;
        private int _prevCuePoint;
        private string _songFilePath;
        private Dictionary<int, int> cuePointSource;

        public EditorPageViewModel()
        {
            AddCuePointCommand = new Command(ExecuteAddCuePointCommand);
            _prevCuePoint = 0;
            CuePointSource = new Dictionary<int, int>();
        }

        private void ExecuteAddCuePointCommand(object cuePointIndex)
        {
            if (CuePointSource.ContainsKey(Convert.ToInt32(cuePointIndex)))
            {
                return;
            }

            if (CurrentCuePoint > 0 && CurrentCuePoint != _prevCuePoint)
            {
                _prevCuePoint = CurrentCuePoint;
                var pixelToMillisecond = DependencyService.Get<IAddCuePointService>()
                    .AddCuePoint(Convert.ToInt32(cuePointIndex), CurrentCuePoint);
                CuePointSource.Add(Convert.ToInt32(cuePointIndex), pixelToMillisecond);
            }

            //TODO: Show some error or just leave it not executed.
        }

        public string SongFilePath
        {
            get => _songFilePath;
            set
            {
                _songFilePath = value;
                OnPropertyChanged();
            }
        }

        public int CurrentCuePoint
        {
            get => _currentCuePoint;
            set
            {
                _currentCuePoint = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<int, int> CuePointSource
        {
            get => cuePointSource;
            set
            {
                cuePointSource = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCuePointCommand { get; }
    }
}
