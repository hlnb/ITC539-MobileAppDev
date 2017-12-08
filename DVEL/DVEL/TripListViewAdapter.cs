using System;
using Android.App;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;

namespace DVEL
{
	public class TripListViewAdapter
	{
		private readonly Activity context;
		private List<TripInfo> tripListData;

		public TripListViewAdapter (Activity _context, List<TripInfo> _tripListData) : base ()
		{
			this.context = _context;
			this.tripListData = _tripListData;
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

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView;

			//re-use existing view, if one is available otherwise create a new one.
			if (view == null) {

				view = context.LayoutInflater.Inflate (Resource.Layout.TripListItem, null, false);
			} 

			TripInfo trip = this [position];
			view.FindViewById<TextView> (Resource.Id.dateTextView).Text = trip.Date;

			if (String.IsNullOrEmpty (trip.Description)) {
				view.FindViewById<TextView> (Resource.Id.tripDescrTextView).Visibility = ViewStates.Gone;
			} else {
				view.FindViewById<TextView> (Resource.Id.tripDescrTextView).Text = trip.Description;
			}

			view.FindViewById<TextView> (Resource.Id.startOdoTextView).Text = trip.StartOdo.ToString ();
			view.FindViewById<TextView> (Resource.Id.endOdoTextView).Text = trip.EndOdo.ToString ();
			view.FindViewById<TextView> (Resource.Id.distanceTextView).Text = trip.Distance + "km";

			if (String.IsNullOrEmpty (trip.Purpose)) {
				view.FindViewById<TextView> (Resource.Id.purposeTextView).Visibility = ViewStates.Gone;
			} else {
				view.FindViewById<TextView> (Resource.Id.purposeTextView).Text = trip.Purpose;
			}

			view.FindViewById<TextView> (Resource.Id.regoTextView).Text = trip.VehicleInfo.Registration;

		}
	}
}

