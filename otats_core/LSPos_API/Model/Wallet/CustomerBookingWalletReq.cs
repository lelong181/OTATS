using System;

namespace BusinessLayer.Model.Wallet
{
	public class CustomerBookingWalletReq
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public byte[] Avatar { get; set; }

		public string ImageType { get; set; }

		public string CustomerType { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string CountryInPP { get; set; }

		public string IdOrPPNum { get; set; }

		public string NationalityId { get; set; }

		public string Address { get; set; }

		public Guid GetCustomerId()
		{
			Guid parsedId;
			return Guid.TryParse(Id, out parsedId) ? parsedId : Guid.Empty;
		}

		public Guid? GetNationalityId()
		{
			if (Guid.TryParse(NationalityId, out var parsed))
			{
				return parsed;
			}
			return null;
		}
	}
}
