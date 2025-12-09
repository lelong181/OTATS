using System.Collections.Generic;
using BusinessLayer.Model.Sell;
using BusinessLayer.Model.Wallet;
using Ticket;

namespace BusinessLayer.Model
{
	public class CartModel
	{
		public int CartOrder { get; set; }

		public string CartTime { get; set; }

		public string Note { get; set; }

		public bool IsBookingOnline { get; set; } = false;


		public string BookingCode { get; set; }

		public List<ServiceSelectedModel> listServiceSelected { get; set; }

		public List<PaymentTypeModel> listPaymentType { get; set; }

		public CustomerSelectedModel customer { get; set; }

		public WalletInforRes wallet { get; set; }

		public CartModel()
		{
			listServiceSelected = new List<ServiceSelectedModel>();
			listPaymentType = new List<PaymentTypeModel>();
			customer = new CustomerSelectedModel();
		}
	}
}
