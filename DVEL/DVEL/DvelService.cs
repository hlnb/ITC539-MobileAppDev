using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;

namespace DVEL
{
	public class DvelService
	{
		//trips
		private const string GET_TRIP = "http://127.0.0.1:8080/com.graphitedge.dvel/api/dvel/dvel";
		private const string CREATE_TRIP = "http://127.0.0.1:8080/com.graphitedge.dvel/api/create";
		private const string DELETE_TRIP = "http://127.0.0.1:8080/com.graphitedge.dvel/api/delete";


		public async Task<TripInfo> GetTripListAsync()
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

				//initialise vehicle list
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

		public bool isConnected(Context activity){
			var connectivityManager = (ConnectivityManager)activity.GetSystemService (Context.ConnectivityService);
			var activeConnection = connectivityManager.ActiveNetworkInfo;
			return (null != activeConnection && activeConnection.IsConnected);
		}

		//create or update 
		public async Task<String> CreateOrUpdateVehAsync(TripInfo trip, Activity activity)
		{
			var settings = new JsonSerializerSettings ();
			settings.ContractResolver = new DVELContractResolver ();
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
		public class DVELContractResolver : DefaultContractResolver{

			protected string ResolverPropertyName(string key)
			{
				return key.ToLower ();
			}

		}


		//delete 
		public async Task<String> DeleteVEHAsync (int tripId)
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

		//vehicles

		private const string GET_VEH = "http://127.0.0.1:8080/com.graphitedge.dvel/api/dvel/dvel";
		private const string CREATE_VEH = "http://127.0.0.1:8080/com.graphitedge.dvel/api/dvel/create";
		private const string DELETE_VEH = "http://127.0.0.1:8080/com.graphitedge.dvel/api/dvel/delete";

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

		//create or update 
		public async Task<String> CreateOrUpdateVehAsync(VehicleInfo veh, Activity activity)
		{
			var settings = new JsonSerializerSettings ();
			settings.ContractResolver = new DVELContractResolver ();
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


		//delete 
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
	}
}

