
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DVEL
{
	[Activity (Label = "TripActivity")]			
	public class TripActivity : Activity
	{
		private ProgressBar progressBar;
		private List<TripInfo> tripListData;
		private TripListViewAdapter tripListAdapter;
		static readonly string Tag = "ActionBarTabsSupport";
		Android.App.Fragment _fragments;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.TripLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
			SetContentView (Resource.Layout.TripLayout);

			_fragments = new Android.App.Fragment[] {
				new TripFragment (),
				new CreateTripFragment (),
				new VehicleExpenseFragment ()
			};

			AddTabToActionBar (Resource.String.triplist_tab_label);
			AddTabToActionBar (Resource.String.createtrip_tab_label);
			AddTabToActionBar (Resource.String.vehicleexpenses_tab_label);

		}

		void AddTabToActionBar(int labelResourceId)
		{
			Android.App.ActionBar.Tab tab = ActionBar.NewTab ()
				.SetText (labelResourceId)
				.SetTabListener (this);

			tab.TabSelected += TabOnTabSelected;
			ActionBar.AddTab (tab);
		}

		void TabOnTabSelected(object sender, Android.App.ActionBar.TabEventArgs tabEventArgs)
		{
			Android.App.ActionBar.Tab tab = (Android.App.ActionBar.Tab)sender;

			Log.Debug (Tag, "The tab {0} has been selected.", tab.Text);
			Android.App.Fragment frag = _fragments [tab.Position];
			tabEventArgs.FragmentTransaction.Replace (Resource.Id.frameLayout1, frag);
		}
	}
}

