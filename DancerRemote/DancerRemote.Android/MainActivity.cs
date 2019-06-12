
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using DancerRemote.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationAndroid))]
namespace DancerRemote.Droid
{
    [Activity(Label = "DancerRemote", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity 
    {
        public static MainActivity activity;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.SetFlags("CollectionView_Experimental");

            activity = this;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        
    }

    public class NavigationAndroid : INavigationAndroid
    {
        public void Navigate(string filePath)
        {
            var intent = new Intent(MainActivity.activity, typeof(SongLoadActivity));
            intent.PutExtra("FileName", filePath);
            MainActivity.activity.StartActivity(intent);
        }
    }
}