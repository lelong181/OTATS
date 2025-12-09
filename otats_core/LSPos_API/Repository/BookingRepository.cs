using System;
using System.Collections.Generic;
using Business.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Sell;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class BookingRepository
	{
		private const string langCodeDefault = "vi";

		public BookingListPosResult GetData(string memberId, string bookingCode, DateTime startDate, DateTime endDate, string channel, string keyword = "", string bookingStatus = "")
		{
			return ApiUtility.CallApiSimple<BookingListPosResult>("booking/booking-list-pos", Method.POST, new
			{
				value = new
				{
					Type = "checkInDate",
					KeyWord = keyword,
					BookingCode = bookingCode,
					MemberId = memberId,
					ProfileID = ((memberId != "") ? memberId : ""),
					Limit = 250,
					Page = 1,
					Channel = channel,
					SiteId = Global.SiteID,
					AccountCode = "",
					BookingStatus = bookingStatus,
					StartDate = startDate.ToString("yyyy-MM-dd"),
					EndDate = endDate.ToString("yyyy-MM-dd")
				}
			});
		}

		public BookingListPosResult GetDataB2B(string memberId, string bookingCode, DateTime startDate, DateTime endDate, string channel, string keyword = "", string bookingStatus = "")
		{
			return ApiUtility.CallApiSimple<BookingListPosResult>("booking/booking-list-b2b-detail", Method.POST, new
			{
				value = new
				{
					Type = "checkInDate",
					KeyWord = keyword,
					BookingCode = bookingCode,
					MemberId = memberId,
					ProfileID = "",
					Limit = 250,
					Page = 1,
					Channel = "B2B",
					SiteId = Global.SiteID,
					AccountCode = "",
					BookingStatus = bookingStatus,
					StartDate = startDate.ToString("yyyy-MM-dd"),
					EndDate = endDate.ToString("yyyy-MM-dd")
				}
			});
		}

		public BookingListPosResult GetBooking(string memberId, string bookingCode, DateTime startDate, DateTime endDate, string channel, string keyword = "", string bookingStatus = "")
		{
			return ApiUtility.CallApiSimple<BookingListPosResult>("booking/booking-list-b2b", Method.POST, new
			{
				value = new
				{
					Type = "checkInDate",
					KeyWord = keyword,
					BookingCode = bookingCode,
					MemberId = memberId,
					ProfileID = "",
					Limit = 250,
					Page = 1,
					Channel = channel,
					SiteId = Global.SiteID,
					AccountCode = "",
					BookingStatus = bookingStatus,
					StartDate = startDate.ToString("yyyy-MM-dd"),
					EndDate = endDate.ToString("yyyy-MM-dd")
				}
			});
		}

		public BookingListB2bDetail GetBookingDetail(string memberId, string bookingCode, DateTime startDate, DateTime endDate, string channel, string keyword = "", string bookingStatus = "")
		{
			return ApiUtility.CallApiSimple<BookingListB2bDetail>("booking/booking-list-b2b-detail", Method.POST, new
			{
				value = new
				{
					Type = "checkInDate",
					KeyWord = keyword,
					BookingCode = bookingCode,
					MemberId = memberId,
					ProfileID = "",
					Limit = 250,
					Page = 1,
					Channel = channel,
					SiteId = Global.SiteID,
					AccountCode = "",
					BookingStatus = bookingStatus,
					StartDate = startDate.ToString("yyyy-MM-dd"),
					EndDate = endDate.ToString("yyyy-MM-dd")
				}
			});
		}

		public BookingRes Detail(string bookingCode)
		{
			return ApiUtility.CallApiSimple<BookingRes>("booking/detail-booking", Method.POST, new
			{
				value = new
				{
					BookingCode = bookingCode
				}
			});
		}

        public BookingCustomerDetailResponse DetailCustomer(string bookingCode)
        {
            return ApiUtility.CallApiSimple<BookingCustomerDetailResponse>("booking/detail-customer", Method.POST, new
            {
                value = new
                {
                    BookingCode = bookingCode
                }
            });
        }

        public BookingDetailInfoResponse DetailInfo(BookingDetaiInfolReq model)
		{
			return ApiUtility.CallApiSimple<BookingDetailInfoResponse>("booking/detail-booking-info", Method.POST, new
			{
				value = model
			});
		}

		public BookingRes DetailPos(string bookingCode)
		{
			return ApiUtility.CallApiSimple<BookingRes>("booking/detail-booking-pos", Method.POST, new
			{
				value = new
				{
					BookingCode = bookingCode,
					langCode = "vi"
				}
			});
		}

		public BookingResponse DetailBookingPos(string bookingCode)
		{
			return ApiUtility.CallApiSimple<BookingResponse>("booking/detail-booking-pos", Method.POST, new
			{
				value = new
				{
					BookingCode = bookingCode,
					langCode = "vi"
				}
			});
		}

		public Res CancelBookingB2b(string bookingCode, string note, string cancelBy)
		{
			return ApiUtility.CallApiSimple("booking/cancel-booking-b2b", Method.POST, new
			{
				Value = new
				{
					BookingCode = bookingCode,
					CancelBy = cancelBy,
					Note = note
				}
			});
		}

		public SaveBookingModel CancelService(RepayServiceRequest repay)
		{
			repay.Channel = "POS";
			repay.ComputerId = Global.ComputerID;
			repay.SiteId = new Guid(Global.SiteID);
			repay.SiteCode = Global.SiteCode;
			repay.CreatedBy = Global.UserName;
			repay.ZoneId = Global.ZoneID;
			repay.RepayFullBooking = false;
			repay.SessionId = new Guid(Global.SessionID);
			repay.SetBookingDetail();
			BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/cancel-service", Method.POST, new
			{
				Value = repay
			});
			if (data != null)
			{
				return new SaveBookingModel
				{
					Success = true,
					InvoiceCode = data.BookingCode
				};
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel CancelInvoice(RepayServiceRequest repay)
		{
			repay.Channel = "POS";
			repay.ComputerId = Global.ComputerID;
			repay.SiteId = new Guid(Global.SiteID);
			repay.SiteCode = Global.SiteCode;
			repay.CreatedBy = Global.UserName;
			repay.RepayFullBooking = true;
			repay.ZoneId = Global.ZoneID;
			repay.ProfileId = Global.ProfileID;
			repay.SessionId = new Guid(Global.SessionID);
			repay.SetBookingDetail();
			BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/cancel-invoice", Method.POST, new
			{
				Value = repay
			});
			if (data != null)
			{
				return new SaveBookingModel
				{
					Success = true,
					InvoiceCode = data.BookingCode
				};
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public CancellationRuleResponse GetCancellationRule(string profileId, DateTime usingDate)
		{
			return ApiUtility.CallApiSimple<CancellationRuleResponse>("booking/cancellation-rule?profileId=" + profileId + "&siteId=" + Global.SiteID + "&usingDate=" + usingDate, Method.GET);
		}

		public List<AccountResponse> ExportTicketByBooking(string bookingCode)
		{
			return ApiUtility.CallApi<AccountResponse>("booking/export-ticket?bookingCode=" + bookingCode, Method.GET);
		}

		public Res ConfirmAndExportTicketByBooking(string bookingCode)
		{
			return ApiUtility.CallApiSimple("booking/confirm-and-export-ticket?bookingCode=" + bookingCode + "&sessionID=" + Global.SessionID, Method.GET);
		}

		public Res RefundBookingPayment(RefundBookingPaymentReq model)
		{
			model.SessionId = Global.SessionID;
			return ApiUtility.CallApiSimple("booking/refund-booking-payment", Method.POST, model);
		}

		public Res ActiveTicketBookingB2B(ActiveTicketBookingB2BReq model)
		{
			return ApiUtility.CallApiSimple("booking/active-ticket-booking-b2b", Method.POST, model);
		}
	}
}
