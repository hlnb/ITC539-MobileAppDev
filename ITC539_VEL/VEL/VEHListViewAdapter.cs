using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using SQLite;

namespace VEL
{
	public class VEHListViewAdapter : BaseAdapter <VehicleInfo>
	{
		private readonly Activity context;
		private List<VehicleInfo> vehicleListData;

		public VEHListViewAdapter (Activity _context, List<VehicleInfo> _vehicleListData): base()
		{
			this.context = _context;
			this.vehicleListData = _vehicleListData;
		}

		public override int Count{
			get{ return vehicleListData.Count; }
		}

		public override long GetItemId(int position){
			return position;
		}

		public override VehicleInfo this[int index]{
			get{
				return vehicleListData [index];
			}
		}

		public override View GetView(int position, View convertView, ViewGroup parent){

			var view = convertView;

			//re-use an existing view, if one is available otherwise create a new one
			if (view == null) {
				view = context.LayoutInflater.Inflate (Resource.Layout.VehicleList, null, false);
			}

			return view;
				

		}
	}
}

