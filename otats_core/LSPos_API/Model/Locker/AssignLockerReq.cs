namespace BusinessLayer.Model.Locker
{
	public class AssignLockerReq
	{
		public string cardID { get; set; }

		public string cardIDOrigin { get; set; }

		public int lockerID { get; set; }

		public int lineID { get; set; }

		public string lockerName { get; set; }

		public string startDate { get; set; }

		public string endDate { get; set; }

		public string createdBy { get; set; }
	}
}
