using System;
using NUnit.Framework;
using VEL;
using System.IO;
using System.Collections.Generic;

namespace VELTestApp
{
	[TestFixture]
	public class VELTestFixture
	{

		IVELDataService _velService;

		[SetUp]
		public void Setup ()
		{
			//setting up storage path
			string storagePath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			_velService = new VELJsonService (storagePath);

			//clear any existing json files
			foreach (string filename in Directory.EnumerateFiles(storagePath, "*.json")) {
				File.Delete (filename);
			}


		}

		
		[TearDown]
		public void Tear ()
		{
		}

		[Test]
		public void CreateVehicle ()
		{
			//setting up test data
			int testId=1091;
			VehicleInfo newVehicle = new VehicleInfo ();
			newVehicle.Id = testId;
			newVehicle.Make = "Honda";
			newVehicle.Model = "Insight";
			newVehicle.Registration = "1ECH993";
			newVehicle.EngType = "1300";
			newVehicle.OdometerReading = 489576;
			newVehicle.ClubRegistration = false;

			//saving Vehicle Record
			int recordsUpdated = DBManager.Instance.SaveVehicle(newVehicle);

			//verifiy if the newly created vehicle exists

			VehicleInfo vehicle = DBManager.Instance.GetVEH (testId);
			Assert.NotNull (vehicle);
			Assert.AreEqual (vehicle.Make, "Honda");

		}

		[Test]
		public void DeleteVehicle()
		{
			//setting up test data
			int testId = 1019;
			VehicleInfo testVehicle = new VehicleInfo ();
			testVehicle.Id = testId;
			testVehicle.Make = "Honda";
			testVehicle.Model = "Insight";
			testVehicle.Registration = "1ECH993";
			testVehicle.EngType = "1300";
			testVehicle.OdometerReading = 489576;
			testVehicle.Description = "Vehicle being saved so we can test delete";
			testVehicle.ClubRegistration = false;

			DBManager.Instance.SaveVehicle (testVehicle);

			//delete information
			VehicleInfo deleteVehicle = DBManager.Instance.GetVEH(testId);
			Assert.NotNull (deleteVehicle);

			DBManager.Instance.DeleteVeh (testId);

			VehicleInfo veh = DBManager.Instance.GetVEH (testId);
			Assert.Null (veh);

		}

		[Test]
		public void ClearCache()
		{
			DBManager.Instance.ClearVEHCache ();
			List<VehicleInfo> vehicleList = DBManager.Instance.GetVehicleListFromCache ();
			Assert.AreEqual (0, vehicleList.Count);
		}
			
	}
}

