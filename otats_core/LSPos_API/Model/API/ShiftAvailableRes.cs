using System;

namespace BusinessLayer.Model.API
{
	public class ShiftAvailableRes
	{
		public Guid ShiftID { get; set; }

		public string ShiftName { get; set; }

		public string ShiftTypeName { get; set; }

		public DateTime? CheckInDate { get; set; }

		public DateTime? FromDate { get; set; }

		public DateTime? ToDate { get; set; }

		public string DateTimeShiftStr { get; set; }
	}
}
