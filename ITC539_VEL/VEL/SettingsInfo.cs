using System;
using SQLite;

namespace VEL
{
	public class SettingsInfo
	{
		[PrimaryKey, AutoIncrement]
		public int Id{get;set;}
		public string DriverName {get; set;}
		public string BusinessName{get;set;}
		public string DistanceUnitFormat{get;set;}
		public string CurrencySymbol{get;set;}
		public bool OdoDecimalPlaces{get;set;}
		public string DateFormat{get;set;}
		public DateTime FBTYear{get;set;}
		public DateTime TaxYear{get;set;}
			
	}
}

