using System;

namespace Ticket
{
	public class CardModel : BaseModel
	{
		public int ID { get; set; }

		public string CardID { get; set; }

		public int ServiceTypeID { get; set; }

		public int InActive { get; set; }

		public bool CanSell { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

		public int SiteID { get; set; }

		public int OriginID { get; set; }
	}
}
