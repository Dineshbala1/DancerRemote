using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DancerRemote
{
    public class WaveView : View
    {
        public static readonly BindableProperty SongPathProperty =
            BindableProperty.Create(nameof(SongPath), typeof(string), typeof(WaveView), string.Empty);

        public string SongPath
        {
            get => (string) GetValue(SongPathProperty);
            set => SetValue(SongPathProperty, value);
        }

        public static readonly BindableProperty CurrentRangeIndexProperty =
            BindableProperty.Create(nameof(CurrentRangeIndex), typeof(int), typeof(WaveView), 0);

        public int CurrentRangeIndex
        {
            get => (int) GetValue(CurrentRangeIndexProperty);
            set => SetValue(CurrentRangeIndexProperty, value);
        }

        public static readonly BindableProperty RangeIndexesProperty =
            BindableProperty.Create(nameof(RangeIndexes), typeof(IEnumerable<int>), typeof(WaveView),
                Enumerable.Empty<int>());

        public IEnumerable<int> RangeIndexes
        {
            get => (IEnumerable<int>) GetValue(RangeIndexesProperty);
            set => SetValue(RangeIndexesProperty, value);
        }
    }
}
