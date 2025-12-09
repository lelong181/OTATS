namespace BusinessLayer.Model.Locker
{
	public class ReleaseLockerReq
	{
		public string cardID { get; set; }

		public int? lockerID { get; set; }

		public int actionType { get; set; }

		public string lockerName { get; set; }

		public string createdBy { get; set; }

		public string LockerName { get; set; }
	}
}
