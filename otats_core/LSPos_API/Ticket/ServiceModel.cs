using System;

namespace Ticket
{
	public class ServiceModel : BaseModel
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int ServiceTypeID { get; set; }

		public int NumberOfExpirationDay { get; set; }

		public bool Inactive { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

		public int ServiceID { get; set; }

		public string ServiceName { get; set; }

		public short Type { get; set; }

		public bool IsCard { get; set; }

		public bool HasAllotment { get; set; }

		public int ZoneID { get; set; }

		public int Price { get; set; }

		public string PriceStr { get; set; }

		public bool IsDisabledDiscount { get; set; }

		public string HasDiscount { get; set; }

		public int SlotAvailability { get; set; }

		public int VATPercent { get; set; }

		public int VATAmount { get; set; }
	}
}
