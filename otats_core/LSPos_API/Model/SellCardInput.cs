namespace BusinessLayer.Model
{
	public class SellCardInput
	{
		public string CardID { get; set; }

		public string CardType { get; set; }

		public string CardTypeStr { get; set; }

		public string ServiceID { get; set; }

		public string ServiceRateID { get; set; }

		public string ServiceRateName { get; set; }

		public string ServiceName { get; set; }

		public bool AutoCreateWallet { get; set; } = false;


		public string serviceWalletID { get; set; }
	}
}
