using System.Collections.Generic;

namespace BusinessLayer.Model.Locker
{
	public class LockerMapModel
	{
		public List<LockerZone> LockerZones { get; set; }

		public LockerMapModel()
		{
			LockerZones = new List<LockerZone>();
		}
	}
}
