
using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Content;

namespace SmartHeatStation.Droid
{
    [Activity(Label = "�ǻۻ���վ" ,
        Icon = "@drawable/icon", 
        MainLauncher = true, 
        NoHistory = true,
        Theme = "@style/Theme.Splash",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InvokeMainActivity();
        }

        /// <summary>
        /// ����������Activity
        /// </summary>
        private void InvokeMainActivity()
        {
            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            StartActivity(mainActivityIntent);
            this.Finish();

        }

    }
}