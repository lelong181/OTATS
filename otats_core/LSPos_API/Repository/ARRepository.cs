using System;
using System.Collections.Generic;
using System.Linq;
using Business.Model;
using BusinessLayer.Model.API;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class ARRepository
	{
		public List<ARModel> GetDataAr(string keyword = "", string arNo = "", int CurrentPage = 1, int PageSize = 150)
		{
			return ApiUtility.CallApi<ARModel>("ar/get-data-ar", Method.POST, new
			{
				keyword = keyword,
				arNo = arNo,
				currentPage = CurrentPage,
				pageSize = PageSize
			});
		}

		public List<ARBookingCanAR> GetBookingCanAR(Guid profileID)
		{
			return ApiUtility.CallApi<ARBookingCanAR>("ar/get-booking-can-ar", Method.POST, new
			{
				ProfileID = profileID
			});
		}

		public List<ARTrans> GetTransAr(DateTime fromDate, DateTime toDate, int type, Guid AccountReceivable)
		{
			return ApiUtility.CallApi<ARTrans>("ar/get-ar-trans", Method.POST, new
			{
				FromDate = fromDate,
				ToDate = toDate,
				FilterType = type,
				AccountReceivable = AccountReceivable
			});
		}

		public ARModel GetARMaster(Guid id, int type)
		{
			return ApiUtility.CallApiSimple<ARModel>("ar/get-master-by-profile", Method.POST, new
			{
				ID = id,
				Type = type
			});
		}

		public List<ARPayment> GetPaymentAr(string siteID, string lang = "vi")
		{
			return ApiUtility.CallApi<ARPayment>("ar/get-payment", Method.POST, new { siteID, lang });
		}

		public string InsertTransAR(string accountReceivable, string paymentType, decimal amount, decimal allocatedAmount, string description = "")
		{
			Res data = ApiUtility.CallApiSimple("ar/insert-trans-ar", Method.POST, new
			{
				accountReceivable = accountReceivable,
				paymentType = paymentType,
				amount = amount,
				description = description,
				allocatedAmount = allocatedAmount,
				CreatedBy = Global.UserName
			});
			return data.Value.ToString();
		}

		public string InsertTransBooking(Guid bookingID)
		{
			Res data = ApiUtility.CallApiSimple("ar/insert-trans-booking", Method.POST, new { bookingID });
			return data.Value.ToString();
		}

		public string Allocate(string type, string trans, Guid accountReceivable, Guid? arPaymentID, List<Guid> ListARBookingID = null)
		{
			Res data = ApiUtility.CallApiSimple("ar/allocate", Method.POST, new
			{
				type = type,
				trans = trans,
				accountReceivable = accountReceivable,
				AccountReceivablePaymentID = arPaymentID,
				CreatedBy = Global.UserName,
				ListARBookingID = ListARBookingID
			});
			return data.Value.ToString();
		}

		public string Refund(string accountReceivableId, string _paymentID, decimal pRefundAmount, string pDescription)
		{
			Res data = ApiUtility.CallApiSimple("ar/refund", Method.POST, new
			{
				AccountReceivableID = accountReceivableId,
				RefundAmount = pRefundAmount,
				Description = pDescription,
				User = Global.UserName,
				paymentID = _paymentID
			});
			return data.Value.ToString();
		}

		public string CheckBooking(Guid bookingID)
		{
			Res data = ApiUtility.CallApiSimple("ar/check-booking", Method.POST, new
			{
				BookingID = bookingID
			});
			return data.Value.ToString();
		}

		public List<ARModel> RptArCurrent(string AccountNo, string ProfileInfo)
		{
			return ApiUtility.CallApi<ARModel>("ar/rpt-ar-current", Method.POST, new { AccountNo, ProfileInfo });
		}

		public RptARListDetailModel RptArDetail(string AccountNo, DateTime? Fromdate, DateTime? Todate, string ProfileInfo, int Type)
		{
			RptARDetailReq model = new RptARDetailReq
			{
				AccountNo = AccountNo,
				Fromdate = Fromdate,
				Todate = Todate,
				ProfileInfo = ProfileInfo,
				Type = Type
			};
			List<RptARListDetailModel> data = ApiUtility.CallApi<RptARListDetailModel>("ar/rpt-ar-detail", Method.POST, model);
			return data.FirstOrDefault();
		}

		public List<AccountReceivableAllocated_Extension> GetARAllocated(string arTransId, string bookingID)
		{
			return ApiUtility.CallApi<AccountReceivableAllocated_Extension>("ar/get-trans-allocated", Method.POST, new
			{
				arTransID = arTransId,
				BookingID = bookingID
			});
		}

		public string UnAllocated(ARUnallocatedTrans model)
		{
			Res data = ApiUtility.CallApiSimple("ar/unallocated-trans", Method.POST, model);
			return data.Value.ToString();
		}

		public DetailAR Detail(string arTransID)
		{
			List<DetailAR> data = ApiUtility.CallApi<DetailAR>("ar/detail-trans", Method.POST, new
			{
				ID = arTransID
			});
			return data.FirstOrDefault();
		}

		public List<ListARApproveRes> GetListARApprove(ListARApproveReq req)
		{
			return ApiUtility.CallApi<ListARApproveRes>("ar/list-ar-approve", Method.POST, req);
		}
	}
}
