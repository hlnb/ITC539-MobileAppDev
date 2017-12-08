using System;
using Android.Content;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Android.Net;
using Android.App;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace VEL
{
	public class VELService
	{
		//vehicles
		private const string GET_VEH = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/vels";
		private const string CREATE_VEH = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/create";
		private const string DELETE_VEH = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/delete";
		private const string UPLOAD_VEH = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/upload";
		//trips
		private const string GET_TRIP = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/trip";
		private const string CREATE_TRIP = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/create";
		private const string DELETE_TRIP = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/delete";
		private const string UPLOAD_TRIP = "http://127.0.0.1:8080/com.helenburgess.vel/api/vel/upload";


		//vehicles list
		public async Task<List<VehicleInfo>> GetVehListAsync()
		{
			HttpClient httpClient = new HttpClient ();

			//Adding Accept-Type as applicaiton/json header

			httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
			HttpResponseMessage response = await httpClient.GetAsync (GET_VEH);
			if (response != null || response.IsSuccessStatusCode) {

				//await! control returns to the caller and the task continues
				string content = await response.Content.ReadAsStringAsync ();

				//printing response body in console
				Console.Out.WriteLine ("Response Body: \r\n {0}", content);

				//initialise vehicle list
				var vehicleListData = new List<VehicleInfo> ();

				//load a JObject from response string
				JObject jsonResponse = JObject.Parse (content);
				IList<JToken> results = jsonResponse ["vehicles"].ToList ();
				foreach (JToken token in results) {
					VehicleInfo vehicle = token.ToObject<VehicleInfo> ();
					vehicleListData.Add (vehicle);
				}
				return vehicleListData;
			} else {
				Console.Out.WriteLine ("Failed to fetch data. Try again later!");
				return null;
			}
				
		}

		//get trips list
		public async Task<List<TripInfo>> GetTripListAsync()
		{
			HttpClient httpClient = new HttpClient ();

			//Adding Accept-Type as applicaiton/json header

			httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
			HttpResponseMessage response = await httpClient.GetAsync (GET_TRIP);
			if (response != null || response.IsSuccessStatusCode) {

				//await! control returns to the caller and the task continues
				string content = await response.Content.ReadAsStringAsync ();

				//printing response body in console
				Console.Out.WriteLine ("Response Body: \r\n {0}", content);

				//initialise Trip list
				var tripListData = new List<TripInfo> ();

				//load a JObject from response string
				JObject jsonResponse = JObject.Parse (content);
				IList<JToken> results = jsonResponse ["trips"].ToList ();
				foreach (JToken token in results) {
					TripInfo trip = token.ToObject<TripInfo> ();
					tripListData.Add (trip);
				}
				return tripListData;
			} else {
				Console.Out.WriteLine ("Failed to fetch data. Try again later!");
				return null;
			}
		}

		//connection checking.
		public bool isConnected(Context activity){
			var connectivityManager = (ConnectivityManager)activity.GetSystemService (Context.ConnectivityService);
			var activeConnection = connectivityManager.ActiveNetworkInfo;
			return (null != activeConnection && activeConnection.IsConnected);
		}

		//create or update Vehicles
		public async Task<String> CreateOrUpdateVehAsync(VehicleInfo veh)
		{
			var settings = new JsonSerializerSettings ();
			settings.ContractResolver = new VELContractResolver ();
			var vehJson = JsonConvert.SerializeObject (veh, Formatting.Indented, settings);

			HttpClient httpClient = new HttpClient ();
			StringContent jsonContent = new StringContent (vehJson, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await httpClient.PostAsync (CREATE_VEH, jsonContent);

			if (response != null || response.IsSuccessStatusCode) {
				string content = await response.Content.ReadAsStringAsync ();
				Console.Out.WriteLine ("{0} saved.", veh.Registration);
				return content;
			}
			return null;

		}

		//create or update trips 
		public async Task<String> CreateOrUpdateTripAsync(TripInfo trip)
		{
			var settings = new JsonSerializerSettings ();
			settings.ContractResolver = new VELContractResolver ();
			var tripJson = JsonConvert.SerializeObject (trip, Formatting.Indented, settings);

			HttpClient httpClient = new HttpClient ();
			StringContent jsonContent = new StringContent (tripJson, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await httpClient.PostAsync (CREATE_TRIP, jsonContent);

			if (response != null || response.IsSuccessStatusCode) {
				string content = await response.Content.ReadAsStringAsync ();
				Console.Out.WriteLine ("{0} saved.", trip.VehicleInfo.Registration);
				return content;
			}
			return null;

		}


		/**
		*Converts all json keys into lowercase
		*/
		public class VELContractResolver : DefaultContractResolver{

			protected string ResolverPropertyName(string key)
			{
				return key.ToLower ();
			}

		}


		//delete Vehicles
		public async Task<String> DeleteVEHAsync (int vehId)
		{
			HttpClient httpClient = new HttpClient ();
			String url = String.Format (DELETE_VEH, vehId);
			HttpResponseMessage response = await httpClient.DeleteAsync (url);

			if (response != null || response.IsSuccessStatusCode) {
				string content = await response.Content.ReadAsStringAsync ();
				Console.Out.WriteLine ("One record Deleted");
				return content;
			}
			return null;
				
		}

		//delete trips
		//delete 
		public async Task<String> DeleteTripAsync (int tripId)
		{
			HttpClient httpClient = new HttpClient ();
			String url = String.Format (DELETE_TRIP, tripId);
			HttpResponseMessage response = await httpClient.DeleteAsync (url);

			if (response != null || response.IsSuccessStatusCode) {
				string content = await response.Content.ReadAsStringAsync ();
				Console.Out.WriteLine ("One record Deleted");
				return content;
			}
			return null;

		}
	}
}

