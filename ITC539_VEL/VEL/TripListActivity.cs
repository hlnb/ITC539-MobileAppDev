using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Newtonsoft.Json;


namespace VEL
{
	[Activity (Label = "Trips")]
	public class TripListActivity: Activity
	{
		int scrollPosition;
		static readonly string Tag = "ActionBarTabSupport";


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			if (null != savedInstanceState) {
				scrollPosition = savedInstanceState.GetInt ("scroll_position");
			}
				
			this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			AddTabToActionBar("Trips", new TripListFragment());
			AddTabToActionBar("New Trip", new CreateTripFragment());
			AddTabToActionBar("Vehicle Expense", new VehicleExpensesFragment());

			if (savedInstanceState != null) {
				this.ActionBar.SelectTab (this.ActionBar.GetTabAt (savedInstanceState.GetInt ("tab")));
			}

		}

		protected override void OnSaveInstanceState (Bundle outState)
		{

			outState.PutInt ("tab", this.ActionBar.SelectedNavigationIndex);
			base.OnSaveInstanceState (outState);
		}

		void AddTabToActionBar(string tabText, Fragment view)
		{
			var tab = this.ActionBar.NewTab ();
			tab.SetText (tabText);

			tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
				
				var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
				if(fragment != null){
					e.FragmentTransaction.Remove(fragment);
					e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
				}
			};
			tab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e) {
				e.FragmentTransaction.Remove(view);
			};
		
				this.ActionBar.AddTab (tab);
		}

		void TabOnTabSelected(object sender, ActionBar.TabEventArgs tabEventArgs)
		{
			ActionBar.Tab tab = (ActionBar.Tab)sender;

			Log.Debug (Tag, "The Tab {0} has been selected.", tab.Text);


		}

	}

}			

