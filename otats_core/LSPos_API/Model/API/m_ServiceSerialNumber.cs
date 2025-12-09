using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class m_ServiceSerialNumber
	{
		public Guid ID { get; set; }

		public string Name { get; set; }

		public Guid ServiceID { get; set; }

		public string PrefixCode { get; set; }

		public int? NumberCode { get; set; }

		public int? StartValue { get; set; }

		public int? EndValue { get; set; }

		public int? IncrementValue { get; set; }

		public bool? Cycle { get; set; }

		public string Description { get; set; }

		public string SiteCode { get; set; }

		public bool? Inactive { get; set; }

		public IEnumerable<m_ServiceSerialNumberConfig> ListConfig { get; set; }
	}
}
