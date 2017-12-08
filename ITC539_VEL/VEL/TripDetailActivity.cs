
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace VEL
{
	[Activity (Label = "TripDetailActivity")]			
	public class TripDetailActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.TripDetails);

			var detailFragment = new TripDetailFragment ();
			detailFragment.Arguments = new Bundle ();
			if (Intent.HasExtra("trip")){
				string tripJson = Intent.GetStringExtra ("trip");
				detailFragment.Arguments.PutString ("trip", tripJson);
			}

			FragmentTransaction ft = FragmentManager.BeginTransaction ();
			ft.Add (Resource.Id.tripDetailLayout, detailFragment);
			ft.Commit ();
		}
	}
}

