using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Com.Semantive.Waveformandroid.Waveform.Soundfile;
using DancerRemote;
using DancerRemote.Droid;
using Java.IO;
using Java.Lang;
using WaveFormBindings;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;
using AView = Android.Views.View;
using Color = Xamarin.Forms.Color;

[assembly: Dependency(typeof(AddCuePointService))]
[assembly: ExportRenderer(typeof(WaveView), typeof(WaveViewRenderer))]

namespace DancerRemote.Droid
{
    public class WaveViewRenderer : ViewRenderer<WaveView, AView>
    {
        public static WaveformViewExtended _waveformView;
        public static FrameLayout fragmentContainer;
        private AView centralMedianView;
        public static WaveView _WaveView;

        public WaveViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<WaveView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var activity = (AppCompatActivity) Context;
                fragmentContainer = new FrameLayout(Context)
                {
                    Id = GenerateViewId()
                };
                _waveformView = new WaveformViewExtended(Context, null);
                _waveformView.setListener(new WaveFormTouchListener(_waveformView, fragmentContainer, Element));
                fragmentContainer.AddView(_waveformView);
                centralMedianView = new AView(Context);
                centralMedianView.SetBackgroundColor(Color.Transparent.ToAndroid());
                fragmentContainer.AddView(centralMedianView);
                SetNativeControl(fragmentContainer);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == WaveView.SongPathProperty.PropertyName)
            {
                if (!_waveformView.hasSoundFile())
                {
                    loadFromFile(Element.SongPath);
                }
            }
        }

        private ProgressDialog mProgressDialog;
        private File mFile;
        private CheapSoundFile _cheapSoundFile;
        protected long mLoadingLastUpdateTime;

        protected async void loadFromFile(string mFilename)
        {
            mFile = new File(mFilename);
            mLoadingLastUpdateTime = JavaSystem.CurrentTimeMillis();
            mProgressDialog = new ProgressDialog(Context);
            mProgressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            mProgressDialog.SetTitle("progress_dialog_loading");
            mProgressDialog.SetCancelable(true);

            mProgressDialog.Show();

            await Task.Run(() =>
            {
                _cheapSoundFile = CheapSoundFile.Create(App.SelectedFilePath, new ProgressListener()
                {
                    ReportProgressCallback = fractionComplete =>
                    {
                        long now = JavaSystem.CurrentTimeMillis();
                        if (now - mLoadingLastUpdateTime > 100)
                        {
                            mProgressDialog.Progress =
                                (int) (mProgressDialog.Max * fractionComplete);
                            mLoadingLastUpdateTime = now;
                        }
                    }
                });

                new Handler(Looper.MainLooper).Post(() => { finishOpeningSoundFile(); });
            });
        }

        protected void finishOpeningSoundFile()
        {
            _waveformView.setSoundFile(_cheapSoundFile);
            _waveformView.Invalidate();
            mProgressDialog.Dismiss();
            centralMedianView.LayoutParameters = new FrameLayout.LayoutParams(fragmentContainer.Width, 2)
                {TopMargin = fragmentContainer.Height / 2};
            centralMedianView.SetBackgroundColor(Color.Black.ToAndroid());
        }

       
    }
}

public class AddCuePointService : IAddCuePointService
{
    private int height = 30, width = 30;

    public int AddCuePoint(int cuePointIndex, int cuePosition)
    {
        var circleView = new AView(Application.Context)
        {
            LayoutParameters = new FrameLayout.LayoutParams(width, height)
            {
                TopMargin = (WaveViewRenderer.fragmentContainer.Height / 2) - height / 2,
                LeftMargin = WaveViewRenderer._waveformView.millisecsToPixels(cuePosition) - width / 2
            }
        };

        var circleDrawable = new ShapeDrawable(new OvalShape());
        circleDrawable.Paint.Color = Android.Graphics.Color.DeepPink;
        circleView.SetBackground(circleDrawable);
        WaveViewRenderer.fragmentContainer.AddView(circleView);

        return cuePosition;
    }
}

public class ProgressListener : Java.Lang.Object, CheapSoundFile.IProgressListener
{
    public Action<double> ReportProgressCallback;

    public bool ReportProgress(double p0)
    {
        this.ReportProgressCallback.Invoke(p0);
        return true;
    }
}

public class WaveFormTouchListener : WaveformListener
{
    private float _touchStartX = 0;
    protected long mWaveformTouchStartMsec;

    private WaveformViewExtended mWaveformView;
    private FrameLayout _fragmentContainer;
    private AView seekableBackgroundView;
    private WaveView _element;

    public WaveFormTouchListener()
    {

    }

    public WaveFormTouchListener(WaveformViewExtended waveformView, FrameLayout fragmentContainer, WaveView element) :
        this()
    {
        mWaveformView = waveformView;
        _fragmentContainer = fragmentContainer;
        _element = element;
        seekableBackgroundView = new AView(Application.Context);
        seekableBackgroundView.SetBackgroundColor(Android.Graphics.Color.ParseColor("#85000000"));
    }

    public void waveformTouchStart(float x)
    {
        _touchStartX = x;
        mWaveformTouchStartMsec = JavaSystem.CurrentTimeMillis();
    }

    public void waveformTouchMove(float x)
    {
        //throw new NotImplementedException();
    }

    public void waveformTouchEnd()
    {
        long elapsedMillisecond = JavaSystem.CurrentTimeMillis() - mWaveformTouchStartMsec;
        if (elapsedMillisecond < 300)
        {
            int seekMillisecond = mWaveformView.pixelsToMillisecs((int) (_touchStartX));
            _element.CurrentRangeIndex = seekMillisecond;
            seekableBackgroundView.LayoutParameters =
                new FrameLayout.LayoutParams((int) _touchStartX, _fragmentContainer.Height);
            if (seekableBackgroundView.Parent != null)
            {
                return;
            }

            _fragmentContainer.AddView(seekableBackgroundView);
        }
    }

    public void waveformFling(float x)
    {
        //throw new NotImplementedException();
    }

    public void waveformDraw()
    {
        //throw new NotImplementedException();
    }

    public void waveformZoomIn()
    {
        //throw new NotImplementedException();
    }

    public void waveformZoomOut()
    {
        //throw new NotImplementedException();
    }

}