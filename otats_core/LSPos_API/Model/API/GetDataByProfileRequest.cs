using System;

namespace BusinessLayer.Model.API
{
	public class GetDataByProfileRequest
	{
		public string ProfileID { get; set; }

		public DateTime SellDate { get; set; }
	}
}
