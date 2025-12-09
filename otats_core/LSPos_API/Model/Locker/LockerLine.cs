using System.Collections.Generic;

namespace BusinessLayer.Model.Locker
{
	public class LockerLine
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int Zone_id { get; set; }

		public int Address { get; set; }

		public string Created_at { get; set; }

		public string Updated_at { get; set; }

		public List<Locker> Lockers { get; set; }

		public LockerLine()
		{
			Lockers = new List<Locker>();
		}
	}
}
