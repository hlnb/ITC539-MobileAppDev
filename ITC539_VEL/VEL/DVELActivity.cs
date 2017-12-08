using Android.App;
using Android.OS;
using Android.Views;


namespace VEL
{
	[Activity (Label = "The D.V.E.L", MainLauncher = true, Icon = "@drawable/icon")]
	public class DVELActivity : Activity
	{
		public static bool isDualMode = false;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.DVELHome);

			var detailsLayout = FindViewById (Resource.Id.tripDetailLayout);
			if((detailsLayout != null) && detailsLayout.Visibility == ViewStates.Visible)
			{
				isDualMode = true;
			}else{
				isDualMode = false;
			}

			DBManager.Instance.CreateTable ();

		}


			
	}
		
}


