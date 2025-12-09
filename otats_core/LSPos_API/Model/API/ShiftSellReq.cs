using System;

namespace BusinessLayer.Model.API
{
	public class ShiftSellReq
	{
		public string ProfileID { get; set; }

		public string LangCode { get; set; }

		public DateTime DateSell { get; set; }

		public string ByGroup { get; set; } = "DAY";

	}
}
