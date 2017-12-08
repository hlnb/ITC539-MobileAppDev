using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace VEL
{
	[Activity (Label ="SplashScreen", MainLauncher=true, NoHistory=true, Theme ="@style/Theme.SplashActivity")]
	public class SplashActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			//wait for 2 seconds

			Thread.Sleep (2000);

			//Moving to next activity
			StartActivity(typeof(MainActivity));
		}
	}
}

