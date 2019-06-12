using System.Windows.Input;
using Xamarin.Forms;

namespace DancerRemote
{
    public class LongPressImageButton : ImageButton
    {

        public static readonly BindableProperty LongPressCommandProperty =
            BindableProperty.Create(nameof(LongPressCommand), typeof(ICommand), typeof(LongPressImageButton), null);

        public ICommand LongPressCommand
        {
            get { return (ICommand) GetValue(LongPressCommandProperty); }
            set { SetValue(LongPressCommandProperty, value); }
        }
    }
}
