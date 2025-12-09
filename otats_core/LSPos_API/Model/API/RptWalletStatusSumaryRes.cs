namespace BusinessLayer.Model.API
{
	public class RptWalletStatusSumaryRes
	{
		public decimal TotalMoney { get; set; }

		public decimal Deposit { get; set; }

		public decimal Balance { get; set; }

		public int TotalCreated { get; set; }

		public int TotalNotUse { get; set; }

		public int TotalUse { get; set; }

		public int TotalExpiration { get; set; }

		public int TotalUsed { get; set; }

		public int TotalCancel { get; set; }
	}
}
