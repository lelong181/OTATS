using System;

namespace BusinessLayer.Model
{
	public class ChangeCardInput : SellCardInput
	{
		public string OldCode { get; set; }

		public Guid AccountID { get; set; }

		public string Status { get; set; }

		public string StatusStr { get; set; }
	}
}
