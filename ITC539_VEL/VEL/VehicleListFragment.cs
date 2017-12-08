
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace VEL
{
	public class VehicleListFragment : ListFragment
	{
		private ProgressBar progressBar;
		private List<VehicleInfo> vehListData;
		private VEHListViewAdapter vehListAdapter;
		private Activity activity;

		int scrollPostion;

		public override void OnAttach(Activity activity)
		{
			base.OnAttach (activity);
			this.activity = activity;
		}


		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here

			if (null != savedInstanceState) {
				scrollPostion = savedInstanceState.GetInt ("scroll_position");
			}
		}

		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			int currentPosition = ListView.FirstVisiblePosition;
			outState.PutInt ("scroll_position", currentPosition);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View view = inflater.Inflate (Resource.Layout.VehicleListFragment, container, false);
			progressBar = view.FindViewById<ProgressBar> (Resource.Id.progressBar);

			SetHasOptionsMenu (true);

			return view;
		}

		public override void OnResume()
		{
			DownloadVehiclesListAsync();
			base.OnResume ();
		}

		public async void DownloadVehiclesListAsync(){
			VELService service = new VELService ();
			if (!service.isConnected (activity)) {
				Toast toast = Toast.MakeText (activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
				toast.Show ();
				vehListData = DBManager.Instance.GetVehicleListFromCache ();
			} else {
				progressBar.Visibility = ViewStates.Visible;
				vehListData = await service.GetVehListAsync ();

				//Clear cache data
				DBManager.Instance.ClearVEHCache ();

				//Save updated Vehicle data
				DBManager.Instance.InsertAll (vehListData);
				progressBar.Visibility = ViewStates.Gone;
			}

			vehListAdapter = new VEHListViewAdapter (activity, vehListData);
			this.ListAdapter = vehListAdapter;
			ListView.Post(() =>{ListView.SetSelection (scrollPostion);
			});
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.VehicleDetailMenu, menu);
			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.actionNew:
				if (DVELActivity.isDualMode) {
					var detailFragment = new VehicleDetailFragment ();
					FragmentTransaction ft = FragmentManager.BeginTransaction ();
					ft.Replace (Resource.Id.vehicleExpRelativeLayout, detailFragment);
					ft.Commit ();
				} else {
					Intent intent = new Intent (activity, typeof(VehicleDetailActivity));
					StartActivity (intent);
				}
				return true;
			case Resource.Id.actionRefresh:
				DownloadVehiclesListAsync ();
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		public override void OnListItemClick(ListView l, View v, int position, long id)
		{
			VehicleInfo veh = vehListData [position];
			if(DVELActivity.isDualMode){
				var detailFragment = new VehicleDetailFragment ();
				detailFragment.Arguments = new Bundle ();
				detailFragment.Arguments.PutString ("veh", JsonConvert.SerializeObject (veh));

				FragmentTransaction ft = FragmentManager.BeginTransaction ();
				ft.Replace (Resource.Id.vehicleExpRelativeLayout, detailFragment);
				ft.Commit ();
			} else {
				Intent vehicleDetailIntent = new Intent (activity, typeof(VehicleDetailActivity));
				vehicleDetailIntent.PutExtra ("veh", JsonConvert.SerializeObject (veh));
				StartActivity (vehicleDetailIntent);
			}

		}
	}
}

