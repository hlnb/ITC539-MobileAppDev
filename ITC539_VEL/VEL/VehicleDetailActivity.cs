
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
using Newtonsoft.Json;

namespace VEL
{
	[Activity (Label = "VehicleDetailActivity")]			
	public class VehicleDetailActivity : Activity
	{
		VehicleInfo _vehInfo;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.VehicleDetailsFragment);

			var detailFragment = new VehicleDetailFragment ();
			detailFragment.Arguments = new Bundle ();
			if (Intent.HasExtra("veh")){
				string vehJson = Intent.GetStringExtra ("veh");
				detailFragment.Arguments.PutString ("veh", vehJson);
			}

			FragmentTransaction ft = FragmentManager.BeginTransaction ();
			ft.Add (Resource.Id.vehicleScrollView, detailFragment);
			ft.Commit ();
		}
	}
}

