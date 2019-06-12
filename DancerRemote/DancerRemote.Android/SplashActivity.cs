
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace DancerRemote.Droid
{
    [Activity(Label = "DancerRemote", Icon = "@mipmap/icon", Theme = "@style/SplashTheme", MainLauncher = true,
        NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_splash);
        }

        protected override async void OnResume()
        {
            base.OnResume();

            await Task.Delay(3000);

            await Task.Run(() =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            });
        }
    }
}