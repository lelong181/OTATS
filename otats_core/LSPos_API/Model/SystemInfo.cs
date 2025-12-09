using System;

namespace BusinessLayer.Model
{
	public class SystemInfo
	{
		public Guid ID { get; set; }

		public string KeyName { get; set; }

		public string KeyValue { get; set; }

		public string Description { get; set; }

		public int? Status { get; set; }

		public bool? IsAllowEdit { get; set; }

		public bool? Inactive { get; set; }

		public bool? IsDelete { get; set; }

		public DateTime? CreatedDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }

		public string UpdatedBy { get; set; }
	}
}
