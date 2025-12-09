using System.Collections.Generic;

namespace BusinessLayer.Model.Locker
{
	public class LockerZone
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int Address { get; set; }

		public string Status { get; set; }

		public string Created_at { get; set; }

		public string Updated_at { get; set; }

		public List<LockerLine> LockerLines { get; set; }

		public LockerZone()
		{
			LockerLines = new List<LockerLine>();
		}
	}
}
