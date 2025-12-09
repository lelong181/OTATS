using System;

namespace BusinessLayer.Model
{
	public class HistoryImageACMResponse
	{
		public int ID { get; set; }

		public DateTime UsingTime { get; set; }

		public string ACMName { get; set; }

		public string AccountCode { get; set; }

		public string EmployeeCode { get; set; }

		public string EmployeeName { get; set; }

		public string Department { get; set; }

		public byte[] Image { get; set; }

		public string ImageType { get; set; }

		public string Description { get; set; }

		public string DataType { get; set; }

		public string ServiceName { get; set; }

		public int TotalServiceUsing { get; set; }

		public int TotalUsing { get; set; }
	}
}
