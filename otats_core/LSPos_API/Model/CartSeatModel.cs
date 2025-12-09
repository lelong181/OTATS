using System;
using System.Collections.Generic;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Sell;
using Ticket;

namespace BusinessLayer.Model
{
	public class CartSeatModel
	{
		public int CartOrder { get; set; }

		public string CartTime { get; set; }

		public string Note { get; set; }

		public Guid ShiftID { get; set; }

		public string BookingCode { get; set; }

		public string ShiftName { get; set; }

		public DateTime? ShiftFromTime { get; set; }

		public string ShiftStartTime { get; set; }

		public string ZoneGroupName { get; set; }

		public string ZoneName { get; set; }

		public string ShiftTypeName { get; set; }

		public List<ServiceSelectedModel> listServiceSelected { get; set; }

		public List<PaymentTypeModel> listPaymentType { get; set; }

		public CustomerSelectedModel customer { get; set; }

		public List<ServiceRateGroup> listServiceRateGroup { get; set; }

		public List<ServiceRateModel> listServiceRate { get; set; }

		public List<ZoneGroupAvailableRes> listServiceRateByZoneGroupAvailable { get; set; }

		public List<PromotionResponse> listAvailablePromotion { get; set; }

		public List<ServiceTicket> listServiceTicket { get; set; }

		public CartSeatModel()
		{
			listServiceSelected = new List<ServiceSelectedModel>();
			listPaymentType = new List<PaymentTypeModel>();
			customer = new CustomerSelectedModel();
			listServiceRateGroup = new List<ServiceRateGroup>();
			listServiceRate = new List<ServiceRateModel>();
			listAvailablePromotion = new List<PromotionResponse>();
			listServiceTicket = new List<ServiceTicket>();
			listServiceRateByZoneGroupAvailable = new List<ZoneGroupAvailableRes>();
		}
	}
}
