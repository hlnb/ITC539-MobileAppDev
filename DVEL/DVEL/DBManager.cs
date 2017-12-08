using System;
using SQLite;
using System.Collections.Generic;

namespace DVEL
{
	public class DBManager
	{
		private const string DB_NAME ="TheDVEL_DB.db3";
		private static readonly DBManager instance = new DBManager();
		SQLiteConnection dbConn;

		private DBManager ()
		{
		}

		public static DBManager Instance {
			get {
				return instance;
			}
		}

		//creating tables in database
		public void CreateTable()
		{
			var path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			dbConn = new SQLiteConnection (System.IO.Path.Combine (path, DB_NAME));
			dbConn.CreateTable<VehicleInfo> ();
			dbConn.CreateTable<TripInfo> ();
			//dbConn.CreateTable<ExpenseInfo> ();
			//dbConn.CreateTable<SettingsInfo> ();
		}

		//vehicleInfo table actions when creating a vehicle
		//inserting or updating a vehicle record
		public int SaveVehicle(VehicleInfo veh)
		{
			int result = dbConn.InsertOrReplace (veh);
			Console.WriteLine ("{0} record updated", result);
			return result;
		}

		//reading vehicle details from database
		public List<VehicleInfo> GetVehicleListFromCache()
		{
			var vehicleListData = new List<VehicleInfo> ();
			IEnumerable<VehicleInfo> table = dbConn.Table<VehicleInfo> ();
			foreach (VehicleInfo veh in table) {
				vehicleListData.Add (veh);
			}

			return vehicleListData;
		}

		//looking for a single vehicle
		public VehicleInfo GetVEH(int vehId)
		{
			VehicleInfo veh = dbConn.Table<VehicleInfo> ().Where (a => a.Id.Equals (vehId)).FirstOrDefault ();
			return veh;
		}

		//deleting vehicle from database

		public int DeleteVeh(int vehId)
		{
			int result = dbConn.Delete<VehicleInfo> (vehId);
			Console.WriteLine ("{0} record affected!", result);
			return result;
		}

		public int ClearVEHCache()
		{
			int result = dbConn.DeleteAll<VehicleInfo> ();
			Console.WriteLine ("{0} records affected!", result);
			return result;
		}

		//information for trips
		//inserting or updating trip details
		public int SaveTrips(TripInfo trip)
		{
			int result = dbConn.InsertOrReplace (trip);
			Console.WriteLine ("{0} record updated", result);
			return result;
		}

		//reading trip details from database
		public List<TripInfo> GetTripListFromCache()
		{
			var tripListData = new List<TripInfo> ();
			IEnumerable<TripInfo> table = dbConn.Table<TripInfo> ();
			foreach (TripInfo trip in table) {
				tripListData.Add (trip);
			}

			return tripListData;
		}

		//looking for a single Trip
		public TripInfo GetTrip(int tripId)
		{
			TripInfo veh = dbConn.Table<TripInfo> ().Where (a => a.Id.Equals (tripId)).FirstOrDefault ();
			return tripId;
		}

		//deleting vehicle from database

		public int DeleteTrip(int tripId)
		{
			int result = dbConn.Delete<TripInfo> (tripId);
			Console.WriteLine ("{0} record affected!", result);
			return result;
		}

		public int ClearTripCache()
		{
			int result = dbConn.DeleteAll<TripInfo> ();
			Console.WriteLine ("{0} records affected!", result);
			return result;
		}

		//information to add


	}
}



