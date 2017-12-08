using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace VEL
{
	public enum Purpose{
		Business, Private
	}
	[Table("TripTable")]
	public class TripInfo

	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set;}
		[NotNull]
		public long StartOdo { get; set;}
		[NotNull]
		public long EndOdo {get; set;}
		public long Distance { get; set;}
		public string Description {get; set;}

		public DateTime Date{get; set;}
		public Purpose Purpose {get; set;}

		[ForeignKey(typeof(VehicleInfo))] // many to one relationship with vehicle
		public int VehicleInfo_Id{get; set;}

		[ManyToOne]
		public VehicleInfo VehicleInfo{ get; set; }



	}


}

