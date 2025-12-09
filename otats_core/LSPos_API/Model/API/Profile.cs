using System;

namespace BusinessLayer.Model.API
{
	public class Profile
	{
		public Guid ID { get; set; }

		public string IdentityCard { get; set; }

		public string Email { get; set; }

		public string ProfileCode { get; set; }

		public string Name { get; set; }

		public string Gender { get; set; }

		public string PhoneNumber { get; set; }

		public string Address { get; set; }
	}
}
