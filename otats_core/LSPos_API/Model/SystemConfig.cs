using System;

namespace BusinessLayer.Model
{
	public class SystemConfig
	{
		public Guid ID { get; set; }

		public string ConfigKey { get; set; }

		public string ConfigValue { get; set; }

		public bool Inactive { get; set; }

		public bool IsDelete { get; set; }
	}
}
