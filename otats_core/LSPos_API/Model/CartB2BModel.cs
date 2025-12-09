using System;
using System.Collections.Generic;
using BusinessLayer.Model.Sell;
using Ticket;

namespace BusinessLayer.Model
{
	public class CartB2BModel
	{
		public int CartOrder { get; set; }

		public string CartTime { get; set; }

		public string Note { get; set; }

		public string OrderCode { get; set; }

		public bool IsBookingOnline { get; set; } = false;


		public string BookingCode { get; set; }

		public DateTime? CheckInDate { get; set; }

		public List<ServiceSelectedModel> listServiceSelected { get; set; }

		public List<PaymentTypeModel> listPaymentType { get; set; }

		public List<CustomerB2BSelectedModel> customer { get; set; }

		public ProfileSelectedModel profile { get; set; }

		public string MemberId { get; set; }

		public string EmailCC { get; set; }

		public string EmailTo { get; set; }

		public string EmailBCC { get; set; }

		public CartB2BModel()
		{
			listServiceSelected = new List<ServiceSelectedModel>();
			listPaymentType = new List<PaymentTypeModel>();
			customer = new List<CustomerB2BSelectedModel>();
			CheckInDate = DateTime.Now;
		}

		public void ClearData()
		{
			listServiceSelected.Clear();
			listPaymentType.Clear();
			customer.Clear();
			profile = null;
			IsBookingOnline = false;
			BookingCode = "";
			Note = "";
		}
	}
}
