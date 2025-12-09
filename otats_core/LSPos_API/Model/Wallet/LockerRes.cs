using System;

namespace BusinessLayer.Model.Wallet
{
	public class LockerRes
	{
		public Guid ID { get; set; }

		public string Name { get; set; }

		public Guid? AccountDetailID { get; set; }

		public bool Status { get; set; }
	}
}
