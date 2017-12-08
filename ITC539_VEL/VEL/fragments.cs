
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
	public class TripListFragment : ListFragment
	{

		private ProgressBar progressBar;
		private List<TripInfo> tripListData;
		private TripListViewAdapter tripListAdpater;

		private Activity activity;

		int scrollPosition;

		public override void OnAttach(Activity activity)
		{
			base.OnAttach (activity);
			this.activity = activity;
		}


		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			if (null != savedInstanceState) {
				scrollPosition = savedInstanceState.GetInt ("scroll_position");
			}
		}

		public override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			int currentPosition = ListView.FirstVisiblePosition;
			outState.PutInt ("scroll_position", currentPosition);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View view = inflater.Inflate (Resource.Layout.TripListFragment, container, false);
			progressBar = view.FindViewById<ProgressBar> (Resource.Id.progressBar);

			SetHasOptionsMenu (true);

			return view;
		}

		public override void OnResume()
		{
			DownloadTripListAsync();
			base.OnResume ();
		}

		public async void DownloadTripListAsync(){
			VELService service = new VELService ();

			if (!service.isConnected (activity)) {
				Toast toast = Toast.MakeText (activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
				toast.Show ();
				tripListData = DBManager.Instance.GetTripListFromCache ();
			} else {
				progressBar.Visibility = ViewStates.Visible;
				tripListData = await service.GetTripListAsync();

				//Clear cache data
				DBManager.Instance.ClearTripCache ();

				//Save updated Vehicle data
				DBManager.Instance.InsertAllTrips(tripListData);
				progressBar.Visibility = ViewStates.Gone;
			}

			tripListAdpater = new TripListViewAdapter (activity, tripListData);
			this.ListAdapter = tripListAdpater;
			ListView.Post(() =>{ListView.SetSelection (scrollPosition);
			});
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater)
		{
			menuInflater.Inflate (Resource.Menu.menu_fragement_list, menu);
			base.OnCreateOptionsMenu (menu, menuInflater);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.actionNew:
				if (DVELActivity.isDualMode) {
					var detailFragment = new TripDetailFragment();
					FragmentTransaction ft = FragmentManager.BeginTransaction ();
					ft.Replace (Resource.Id.tripDetailLayout, detailFragment);
					ft.Commit ();
				} else {
					Intent intent = new Intent (activity, typeof(TripDetailActivity));
					StartActivity (intent);
				}
				return true;
			case Resource.Id.actionRefresh:
				DownloadTripListAsync ();
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		public override void OnListItemClick(ListView l, View v, int position, long id)
		{
			TripInfo trip = tripListData [position];



		}
	}

		public class TripDetailFragment :Fragment
		{
				TripInfo _tripInfo;

			EditText _descrEditText;
			EditText _starttripEditText;
			EditText _endtripEditText;
			EditText _dateEditText;
			EditText _regoEditText;

			
			

			private Activity activity;

			public override void OnAttach(Activity activity)
			{
				base.OnAttach (activity);
				this.activity = activity;
			}

			public override void OnCreate (Bundle savedInstanceState)
			{
				base.OnCreate (savedInstanceState);
				if (Arguments!=null && Arguments.ContainsKey("trip")) {
					string tripJson = Arguments.GetString ("trip");
					_tripInfo = JsonConvert.DeserializeObject<TripInfo>(tripJson);
				} else {
					_tripInfo = new TripInfo ();
				}
			}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View view = inflater.Inflate(Resource.Layout.VehicleDetailsFragment, container, false);

			_descrEditText = view.FindViewById<EditText>(Resource.Id.descrEditText);
			_starttripEditText = view.FindViewById<EditText>(Resource.Id.odoStartEditText);
			_endtripEditText = view.FindViewById<EditText>(Resource.Id.odoEndEditText);
			_dateEditText = view.FindViewById<EditText>(Resource.Id.dateEditText);
			_regoEditText = view.FindViewById<EditText>(Resource.Id.regoEditText);


			SetHasOptionsMenu (true);

			if (Arguments!=null && Arguments.ContainsKey("trip")) {
				string tripJson = Arguments.GetString ("trip");
				_tripInfo = JsonConvert.DeserializeObject<TripInfo>(tripJson);
			} else {
				_tripInfo = new TripInfo ();
			}

			UpdateTripUI ();

			return view;
		}


		protected void UpdateTripUI()
		{

			_descrEditText.Text = _tripInfo.Description;
			_starttripEditText.Text = _tripInfo.StartOdo.ToString ();
			_endtripEditText.Text = _tripInfo.EndOdo.ToString();
			_dateEditText.Text = _tripInfo.Date.ToString();
			_regoEditText.Text = _tripInfo.VehicleInfo.Registration;

		}

		protected void SaveTrip()
		{
			bool errors = false;
			if (String.IsNullOrEmpty (_starttripEditText.Text)) {
				_starttripEditText.Error = "Name cannot be empty";
				errors = true;
			} else {
				_starttripEditText.Error = null;
			}

			if (errors) {
				return;
			}

			var startOdoString = _tripInfo.StartOdo.ToString ();
			var endOdoString = _tripInfo.EndOdo.ToString();
			var dateString = _tripInfo.Date.ToString();

			_tripInfo.Description = _descrEditText.Text;
			 startOdoString= _starttripEditText.Text;
			endOdoString= _endtripEditText.Text;
			dateString = _dateEditText.Text;
			_tripInfo.VehicleInfo.Registration = _regoEditText.Text;

			CreateOrUpdateTripAsync (_tripInfo);
		}

		private async void CreateOrUpdateTripAsync(TripInfo trip){
			VELService service = new VELService ();
			if (!service.isConnected(activity)) {
				Toast toast = Toast.MakeText (activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
				toast.Show ();
				return;
			}

			string response = await service.CreateOrUpdateTripAsync(_tripInfo);
			if (!string.IsNullOrEmpty (response)) {
				Toast toast = Toast.MakeText (activity, String.Format ("{0} saved.", _tripInfo.VehicleInfo.Registration), ToastLength.Short);
				toast.Show();

				DBManager.Instance.SaveTrips(trip);

				if(!DVELActivity.isDualMode)
					activity.Finish ();
			} else {
				Toast toast = Toast.MakeText (activity, "Something went Wrong!", ToastLength.Short);
				toast.Show();
			}
		}

		protected void DeleteTrip()
		{
			AlertDialog.Builder alertConfirm = new AlertDialog.Builder(activity);
			alertConfirm.SetTitle("Confirm delete");
			alertConfirm.SetCancelable(false);
			alertConfirm.SetPositiveButton("OK", ConfirmDelete);
			alertConfirm.SetNegativeButton("Cancel", delegate {});
			alertConfirm.SetMessage(String.Format("Are you sure you want to delete {0}?", _tripInfo.Description));
			alertConfirm.Show();

		}

		protected void ConfirmDelete(object sender, EventArgs e)
		{
			DeleteTripAsync ();
		}

		public async void DeleteTripAsync(){
			VELService service = new VELService ();
			if (!service.isConnected(activity)) {
				Toast toast = Toast.MakeText (activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
				toast.Show ();
				return;
			}

			string response = await service.DeleteTripAsync (_tripInfo.Id);
			if (!string.IsNullOrEmpty	(response)) {
				Toast toast = Toast.MakeText (activity, String.Format ("{0} deleted.", _tripInfo.Description), ToastLength.Short);
				toast.Show();

				DBManager.Instance.DeleteTrip (_tripInfo.Id);

				if(!DVELActivity.isDualMode)
					activity.Finish ();
			} else {
				Toast toast = Toast.MakeText (activity, "Something went Wrong!", ToastLength.Short);
				toast.Show();
			}
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate(Resource.Menu.menu_fragement_list, menu);
			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.actionSave:
				SaveTrip();
				return true;
			case Resource.Id.actionDelete: 
				DeleteTrip ();
				return true;
			default:
				return base.OnOptionsItemSelected(item);
			}
		}

		public override void OnPrepareOptionsMenu (IMenu menu)
		{
			base.OnPrepareOptionsMenu (menu);
			if (_tripInfo.Id <= 0) {
				IMenuItem item = menu.FindItem (Resource.Id.actionDelete);
				item.SetEnabled (false);
				item.SetVisible(false);
			}
		}
		}

		
		public class CreateTripFragment : Fragment
		{

		}

		public class VehicleExpensesFragment: Fragment
		{

				
		}
}

