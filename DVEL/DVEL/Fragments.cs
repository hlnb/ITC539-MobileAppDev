
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

namespace DVEL
{

	public class TripFragment : Fragment
	{
		private ProgressBar progressBar;
		private List<TripInfo> tripListData;
		private TripListViewAdapter tripListAdapater;

		private Activity activity;

		public override void OnAttach(Activity activity)
		{
			base.OnAttach (activity);
			this.activity = activity;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			View view = inflater.Inflate(Resource.Layout.TripFragment, null);
			view.FindViewById<TextView>(Resource.Id).SetText(Resource.String.triplist_tab_label);
			return view;
		}

		public override void OnResume()
		{
			DownloadTripListAsync();
			base.OnResume ();
		}

		public async void DownloadTripListAsync(){
			DvelService service = new DvelService ();
			if (!service.isConnected (activity)) {
				Toast toast = Toast.MakeText (activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
				toast.Show ();
				tripListData = DBManager.Instance.GetTripListFromCache ();
			} else {
				progressBar.Visibility = ViewStates.Visible;
				tripListData = await service.GetTripListAsync ();

				//Clear cache data
				DBManager.Instance.ClearVEHCache ();

				//Save updated Vehicle data
				DBManager.Instance.InsertAll (tripListData);
				progressBar.Visibility = ViewStates.Gone;
			}

			tripListAdapater = new TripListViewAdapter (activity, tripListData);
			this.listAdapter = tripListAdapater;
			ListView.Post(() =>{ListView.SetSelection (ScrollState);
			});
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.TripListMenu, menu);
			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected(IMenu item)
		{
			switch (item.ItemId) {
			case Resource.Id.actionNew:
				if (TripActivity.isDualMode) {
					var detailFragment = new CreateTripFragment ();
					FragmentTransaction ft = FragmentManager.BeginTransaction ();
					ft.Replace (Resource.Id.CreateTripDetail, detailFragment);
					ft.Commit ();
				} else {
					Intent intent = new Intent (activity, typeof(CreateTripActivity));
					StartActivity (intent);
				}
				return true;
			case Resource.Id.actionRefresh:
				DownloadTripListAsync ();
				return true;
			default:
				return base.OnOptionsItemSelected ();
			}
		}

		public override void OnListItemClick(ListView l, View v, int position, long id)
		{
			TripInfo trip = tripListData [position];
			if(TripActivity.isDualMode){
				var detailFragment = new CreateTripFragment ();
				detailFragment.Arguments = new Bundle ();
				detailFragment.Arguments.PutString ("trip", JsonConvert.SerializeObject (trip));

				FragmentTransaction ft = FragmentManager.BeginTransaction ();
				ft.Replace (Resource.Id.CreateTripDetail, detailFragment);
				ft.Commit ();
			} else {
				Intent CreateTripDetailIntent = new Intent (activity, typeof(CreateTripActivity));
				CreateTripDetailIntent.PutExtra ("trip", JsonConvert.SerializeObject (trip));
				StartActivity (CreateTripDetailIntent);
			}

		}
	}

	public class CreateTripFragment : Fragment
	{

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.CreateTripFragment, null);
			view.FindViewById<TextView>(Resource.Id).SetText(Resource.String.createtrip_tab_label);
			return view;
		}
	}

	public class VehicleExpenseFragment : Fragment
	{

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.VehicleExpenseFragment, null);
			view.FindViewById<TextView>(Resource.Id).SetText(Resource.String.vehicleexpenses_tab_label);
			return view;
		}
	}
}

