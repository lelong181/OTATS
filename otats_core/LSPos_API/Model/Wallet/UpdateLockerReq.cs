using System;

namespace BusinessLayer.Model.Wallet
{
	public class UpdateLockerReq
	{
		public Guid LockerID { get; set; }

		public Guid AccountDetailID { get; set; }
	}
}
