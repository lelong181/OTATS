using System;

namespace BusinessLayer.Model
{
	public class Card
	{
		public Guid ID { get; set; }

		public string CardCode { get; set; }

		public Guid ServiceID { get; set; }

		public string Description { get; set; }

		public bool? IsSold { get; set; }

		public string Type { get; set; }

		public bool Inactive { get; set; }

		public bool IsDelete { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
	}
}
