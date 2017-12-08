using System;
using SQLite;

namespace VEL
{
	public class ExpenseInfo
	{
		[PrimaryKey, AutoIncrement]
		public int Id{get;set;}
		public string Description{get;set;}
		public DateTime ExpenseDate{get;set;}
		public bool Purpose{ get; set;}
	}
}

