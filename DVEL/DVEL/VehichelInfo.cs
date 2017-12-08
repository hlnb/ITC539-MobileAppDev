using System;
using SQLite;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

namespace DVEL
{
	[Table("VehicleTable")]
	public class VehicleInfo
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id{get;set;}
		[NotNull]
		public string Make{get;set;}
		[NotNull]
		public string Model{get; set;}
		public string EngType{ get; set;}
		[NotNull]
		public string Registration{get; set;}
		public long OdometerReading { get; set;}
		[MaxLength(1000)]
		public string Description{get;set;}
		public bool ClubRegistration {get;set;}

		[OneToMany(CascadeOperations = CascadeOperation.All)] // One to many relationship with Trips
		public List<TripInfo> Trips{ get; set;}
	}
}

