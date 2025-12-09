using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.Locker
{
	public class Locker
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int Zone_id { get; set; }

		public int Line_id { get; set; }

		public int Size_id { get; set; }

		public int Status { get; set; }

		public string StatusStr { get; set; }

		public int Del_flag { get; set; }

		public DateTime Created_at { get; set; }

		public DateTime Updated_at { get; set; }

		public string Revision_number { get; set; }

		public string Address { get; set; }

		public string Position { get; set; }

		public string LineAddress { get; set; }

		public string ZoneAddress { get; set; }

		public string ZoneName { get; set; }

		public string LineName { get; set; }

		public List<string> Card_id { get; set; }

		public string Transaction_status { get; set; }
	}
}
