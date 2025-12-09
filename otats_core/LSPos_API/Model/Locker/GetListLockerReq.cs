namespace BusinessLayer.Model.Locker
{
	public class GetListLockerReq
	{
		public string dataType { get; set; }

		public int? zoneID { get; set; }

		public int? lineID { get; set; }

		public int? lockerID { get; set; }

		public int status { get; set; }
	}
}
