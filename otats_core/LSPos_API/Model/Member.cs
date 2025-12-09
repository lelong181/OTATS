using System;
using Newtonsoft.Json;

namespace BusinessLayer.Model
{
	public class Member
	{
		public Guid? ID { get; set; }

		public string Fullname { get; set; }

		public string Gender { get; set; }

		[JsonIgnore]
		public string GenderStr { get; set; }

		public string Address { get; set; }

		public DateTime? BirthDay { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string IdentityCard { get; set; }

		public string MemberType { get; set; }

		public byte[] Avatar { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? ActiveDate { get; set; }

		public DateTime? EndActiveDate { get; set; }
	}
}
