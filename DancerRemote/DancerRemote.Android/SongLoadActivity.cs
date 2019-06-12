
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Com.Semantive.Waveformandroid.Waveform;
using Com.Semantive.Waveformandroid.Waveform.Soundfile;
using Com.Semantive.Waveformandroid.Waveform.View;
using Java.IO;
using Java.Lang;
using Console = System.Console;

namespace DancerRemote.Droid
{
    [Activity(Label = "Songload", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    class SongLoadActivity : AppCompatActivity, WaveformView.IWaveformListener, CheapSoundFile.IProgressListener
    {
        public static string SongFilePath = string.Empty;

        private WaveformView _waveformView;

        private CheapSoundFile _soundFile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = this.Intent;

            if (intent != null && intent.HasExtra("FileName"))
            {
                SongFilePath = intent.GetStringExtra("FileName");
            }

            _waveformView = new WaveformView(this, null);
            _waveformView.SetListener(this);
            _waveformView.SetSegments(getSegments());

            SetContentView(Resource.Layout.layout_songload);

            this.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, new CustomWaveFormFragment())
                .Commit();
        }

        private async Task LoadFile()
        {
            var file = new File(SongFilePath);

            var progressDialog = new ProgressDialog(this);

            await Task.Run(() =>
            {
                try
                {
                    _soundFile = CheapSoundFile.Create(SongFilePath, this);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                finally
                {
                    _waveformView.SetSoundFile(_soundFile);
                    _waveformView.RecomputeHeights(2);

                    var mPos = _waveformView.MaxPos();
                }

            });

        }

        protected List<Segment> getSegments()
        {
            return null;
        }

        public void WaveformDraw()
        {
            throw new System.NotImplementedException();
        }

        public void WaveformFling(float p0)
        {
            throw new System.NotImplementedException();
        }

        public void WaveformTouchEnd()
        {
            throw new System.NotImplementedException();
        }

        public void WaveformTouchMove(float p0)
        {
            throw new System.NotImplementedException();
        }

        public void WaveformTouchStart(float p0)
        {
            throw new System.NotImplementedException();
        }

        public void WaveformZoomIn()
        {
            throw new System.NotImplementedException();
        }

        public void WaveformZoomOut()
        {
            throw new System.NotImplementedException();
        }

        public bool ReportProgress(double p0)
        {
            throw new System.NotImplementedException();
        }
    }

    public class CustomWaveFormFragment : WaveformFragment
    {
        protected override string FileName { get; }

        public CustomWaveFormFragment()
        {
            FileName = App.SelectedFilePath;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}