using System;

namespace BusinessLayer.Model
{
	public class PromotionLink
	{
		public Guid ID { get; set; }

		public Guid ProfileLevelID { get; set; }

		public Guid ServiceRateID { get; set; }

		public Guid? PromotionID { get; set; }

		public Guid? ServiceID { get; set; }

		public bool Inactive { get; set; }

		public bool IsDelete { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }

		public virtual Promotion Promotion { get; set; }
	}
}
