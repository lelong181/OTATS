using System;

namespace BusinessLayer.Model
{
	public class PromotionI18n
	{
		public Guid ID { get; set; }

		public string LangCode { get; set; }

		public string Description { get; set; }

		public string Title { get; set; }

		public Guid PromotionID { get; set; }

		public virtual Promotion Promotion { get; set; }
	}
}
