using System;

namespace BusinessLayer.Model.API
{
	public class m_ServiceSerialNumberConfig
	{
		public Guid ID { get; set; }

		public Guid ServiceSerialNumberID { get; set; }

		public string LangCode { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public bool? Inactive { get; set; }
	}
}
