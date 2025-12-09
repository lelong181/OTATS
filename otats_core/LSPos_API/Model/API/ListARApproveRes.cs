using System;

namespace BusinessLayer.Model.API
{
	public class ListARApproveRes
	{
		public string CreatedByFullname { get; set; }

		public DateTime? ApproveDate { get; set; }

		public decimal? CreditLimit { get; set; }

		public int? CreditDueDays { get; set; }

		public string Description { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public Guid? AccountReceivableID { get; set; }
	}
}
