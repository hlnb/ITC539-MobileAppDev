using System;
using Android.App;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;

namespace VEL
{
	public class TripListViewAdapter : BaseAdapter <TripInfo>
	{
		private readonly Activity context;
		private List<TripInfo> tripListData;

		public TripListViewAdapter (Activity _context, List<TripInfo> _tripList) : base()
		{
			this.context = _context;
			this.tripListData = _tripList;
		}

		public override int Count {
			get {
				return tripListData.Count;
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override TripInfo this [int index] {
			get {
				return tripListData [index];
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView; 

			// re-use an existing view, if one is available otherwise create a new one
			if (view == null) {
				view = context.LayoutInflater.Inflate (Resource.Layout.TripListItem, null, false);
			}

			TripInfo trip = this [position];
				
			view.FindViewById<TextView>(Resource.Id.dateTextView).Text = trip.Date.ToString ();

			if (String.IsNullOrEmpty (trip.Description)) {
				view.FindViewById<TextView> (Resource.Id.descrTextView).Visibility = ViewStates.Gone;
			} else {
				view.FindViewById<TextView>(Resource.Id.descrTextView).Text = trip.Description;
			}

			view.FindViewById<TextView> (Resource.Id.odoStartTextView).Text = trip.StartOdo.ToString();
			view.FindViewById<TextView> (Resource.Id.odoEndTextView).Text = trip.EndOdo.ToString ();

			view.FindViewById<TextView> (Resource.Id.regoTextView).Text = trip.VehicleInfo.Registration;

			var distanceTextView = view.FindViewById<TextView> (Resource.Id.distanceTextView);
						

			return view;
		}



	}
}

