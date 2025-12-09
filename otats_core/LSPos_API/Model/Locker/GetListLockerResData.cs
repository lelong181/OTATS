using System.Collections.Generic;

namespace BusinessLayer.Model.Locker
{
	public class GetListLockerResData
	{
		public List<LockerZone> Zones { get; set; }

		public List<LockerLine> Lines { get; set; }

		public List<Locker> Lockers { get; set; }
	}
}
