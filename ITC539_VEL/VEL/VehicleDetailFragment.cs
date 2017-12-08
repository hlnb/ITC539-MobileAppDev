
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
	public class VehicleDetailFragment : Fragment
	{

		VehicleInfo _vehInfo;

		EditText _makeEditText;
		EditText _modelEditText;
		EditText _engineEditText;
		EditText _odoEditText;
		EditText _regoEditText;
		EditText _descrEditText;


		private Activity activity;
		public override void OnAttach(Activity activity)
		{
			base.OnAttach (activity);
			this.activity = activity;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			if (Arguments!=null && Arguments.ContainsKey("veh")) {
				string vehJson = Arguments.GetString ("veh");
				_vehInfo = JsonConvert.DeserializeObject<VehicleInfo>(vehJson);
			} else {
				_vehInfo = new VehicleInfo ();
			}
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View view = inflater.Inflate(Resource.Layout.VehicleDetailsFragment, container, false);

			_makeEditText = view.FindViewById<EditText>(Resource.Id.makeEditText);
			_modelEditText = view.FindViewById<EditText>(Resource.Id.modelEditText);
			_engineEditText = view.FindViewById<EditText>(Resource.Id.engineEditText);
			_odoEditText = view.FindViewById<EditText>(Resource.Id.odoEditText);
			_regoEditText = view.FindViewById<EditText>(Resource.Id.regoEditText);
			_descrEditText = view.FindViewById<EditText> (Resource.Id.descrEditText);



			SetHasOptionsMenu (true);

			if (Arguments!=null && Arguments.ContainsKey("veh")) {
				string vehJson = Arguments.GetString ("veh");
				_vehInfo = JsonConvert.DeserializeObject<VehicleInfo>(vehJson);
			} else {
				_vehInfo = new VehicleInfo ();
			}

			UpdateUI ();

			return view;
		}

		protected void UpdateUI()
		{

			_makeEditText.Text = _vehInfo.Make;
			_modelEditText.Text = _vehInfo.Model;
			_engineEditText.Text = _vehInfo.EngType.ToString();
			_odoEditText.Text = _vehInfo.OdometerReading.ToString ();
			_regoEditText.Text = _vehInfo.Registration;
			_descrEditText.Text = _vehInfo.Description;
		
		}

		protected void SaveVEH()
		{
			bool errors = false;
			if (String.IsNullOrEmpty (_makeEditText.Text)) {
				_makeEditText.Error = "Vehicle Make cannot be empty";
				errors = true;
			} else {
				_makeEditText.Error = null;
				errors = false;
			}

			if (String.IsNullOrEmpty (_modelEditText.Text)) {
				_modelEditText.Error = "Vehicle Model cannot be empty";
				errors = true;
			} else {
				_modelEditText.Error = null;
			}

			if (String.IsNullOrEmpty (_regoEditText.Text)) {
				_regoEditText.Error = "Vehicle record requires registration details";
				errors = true;
			} else {
				_regoEditText.Error = null;
			}

			double? tempEngine = null;
			if(!String.IsNullOrEmpty(_engineEditText.Text))
			{
				try{
					tempEngine = Double.Parse(_engineEditText.Text);
					if((tempEngine > 5000) | (tempEngine < 50)){
						_engineEditText.Error = "Engine type / size must be between 50 and 5000";
						errors= true;
					}else{
						_engineEditText.Error = null;
					}
				}
				catch{
					_engineEditText.Error = "Engine type must be a valid whole number";
					errors = true;
				}
			}

			double? tempOdometer = null;
			if(!String.IsNullOrEmpty(_odoEditText.Text))
			{
				try{
					tempOdometer = Double.Parse(_odoEditText.Text);
					if((tempOdometer > 999999) | (tempOdometer < 0)){
						_odoEditText.Error = "Odometer Reading must be between 0 and 999999";
						errors= true;
					}else{
						_odoEditText.Error = null;
					}
				}catch{
						_odoEditText.Error = "Odometer Reading must be a valid whole number";
						errors = true;
				}
			}

			_vehInfo.Make = _makeEditText.Text;
			_vehInfo.Model = _modelEditText.Text;
			_vehInfo.EngType = (double)tempEngine;
			_vehInfo.OdometerReading = (double)tempOdometer;
			_vehInfo.Registration = _regoEditText.Text;
			_vehInfo.Description = _descrEditText.Text ;


			CreateOrUpdateVehAsync (_vehInfo);
		}

		private async void CreateOrUpdateVehAsync(VehicleInfo veh){
			VELService service = new VELService ();
			if (!service.isConnected(activity)) {
				Toast toast = Toast.MakeText (activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
				toast.Show ();
				return;
			}

			string response = await service.CreateOrUpdateVehAsync(_vehInfo);
			if (!string.IsNullOrEmpty (response)) {
				Toast toast = Toast.MakeText (activity, String.Format ("{0} saved.", _vehInfo.Registration), ToastLength.Short);
				toast.Show();

				DBManager.Instance.SaveVehicle (veh);

				if(!DVELActivity.isDualMode)
					activity.Finish ();
			} else {
				Toast toast = Toast.MakeText (activity, "Something went Wrong!", ToastLength.Short);
				toast.Show();
			}
		}

		protected void DeleteVEH()
		{
			AlertDialog.Builder alertConfirm = new AlertDialog.Builder(activity);
			alertConfirm.SetTitle("Confirm delete");
			alertConfirm.SetCancelable(false);
			alertConfirm.SetPositiveButton("OK", ConfirmDelete);
			alertConfirm.SetNegativeButton("Cancel", delegate {});
			alertConfirm.SetMessage(String.Format("Are you sure you want to delete {0}?", _vehInfo.Registration));
			alertConfirm.Show();

		}

		protected void ConfirmDelete(object sender, EventArgs e)
		{
			DeleteVEHAsync ();
		}

		public async void DeleteVEHAsync(){
			VELService service = new VELService ();
			if (!service.isConnected(activity)) {
				Toast toast = Toast.MakeText (activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
				toast.Show ();
				return;
			}

			string response = await service.DeleteVEHAsync (_vehInfo.Id);
			if (!string.IsNullOrEmpty	(response)) {
				Toast toast = Toast.MakeText (activity, String.Format ("{0} deleted.", _vehInfo.Registration), ToastLength.Short);
				toast.Show();

				DBManager.Instance.DeleteVeh (_vehInfo.Id);

				if(!DVELActivity.isDualMode)
					activity.Finish ();
			} else {
				Toast toast = Toast.MakeText (activity, "Something went Wrong!", ToastLength.Short);
				toast.Show();
			}
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate(Resource.Menu.VehicleDetailMenu, menu);
			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.actionSave:
				SaveVEH();
				return true;
			case Resource.Id.actionDelete: 
				DeleteVEH ();
				return true;
			default:
				return base.OnOptionsItemSelected(item);
			}
		}

		public override void OnPrepareOptionsMenu (IMenu menu)
		{
			base.OnPrepareOptionsMenu (menu);
			if (_vehInfo.Id <= 0) {
				IMenuItem item = menu.FindItem (Resource.Id.actionDelete);
				item.SetEnabled (false);
				item.SetVisible(false);
			}
		}
	}
}

