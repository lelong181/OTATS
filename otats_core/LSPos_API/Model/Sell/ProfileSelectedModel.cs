using System;

namespace BusinessLayer.Model.Sell
{
	public class ProfileSelectedModel
	{
		public string MemberId { get; set; }

		public Guid ProfileId { get; set; }

		public Guid ID { get; set; }

		public string IdentityCard { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string AlternativeEmail { get; set; }

		public string PhoneNumber { get; set; }

		public string Address { get; set; }
		public string BookingCode { get; set; }
	}
}
