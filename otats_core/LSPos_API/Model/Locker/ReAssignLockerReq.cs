namespace BusinessLayer.Model.Locker
{
	public class ReAssignLockerReq
	{
		public string cardIDOld { get; set; }

		public string cardNew { get; set; }

		public string startDate { get; set; }

		public string endDate { get; set; }

		public string lockerID { get; set; }

		public string lockerName { get; set; }

		public string CreatedBy { get; set; }
	}
}
