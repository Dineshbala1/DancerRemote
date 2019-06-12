using Android.Content;
using DancerRemote;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ImageButton = Xamarin.Forms.ImageButton;
using ImageButtonRenderer = DancerRemote.Droid.ImageButtonRenderer;

[assembly:ExportRenderer(typeof(LongPressImageButton), typeof(ImageButtonRenderer))]
namespace DancerRemote.Droid
{
    public class ImageButtonRenderer : Xamarin.Forms.Platform.Android.ImageButtonRenderer
    {

        private LongPressImageButton _element;

        public ImageButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<ImageButton> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _element = Element as LongPressImageButton;
                LongClick += ImageButtonRenderer_LongClick;
            }
        }

        private void ImageButtonRenderer_LongClick(object sender, LongClickEventArgs e)
        {
            _element.LongPressCommand?.Execute(null);
        }
    }
}