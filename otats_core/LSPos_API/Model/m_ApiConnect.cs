using System;

namespace BusinessLayer.Model
{
	public class m_ApiConnect
	{
		public Guid ID { get; set; }

		public string ConnectName { get; set; }

		public string ConnectID { get; set; }

		public string ConnectKey { get; set; }

		public bool? Inactive { get; set; }

		public string Note { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? UpdateDate { get; set; }
	}
}
