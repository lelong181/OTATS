using System.Collections.Generic;
using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Reports;
using Model.StoredProcedure;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class ReportsRepository
	{
		public List<RptRevenueSummaryByShift> RevenueSummaryByShift(RevenueSummaryByShiftRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueSummaryByShift>("reports/revenue-summary-by-shift", Method.POST, model);
		}	
		
		public List<RptRevenueSummaryByPaymentType> RevenueSummaryByPaymenttype(RevenueSummaryByShiftRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueSummaryByPaymentType>("reports/revenue-summary-by-paymenttype", Method.POST, model);
		}

		public List<RptRevenueDetail> RevenueDetail(RevenueDetailRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueDetail>("reports/revenue-detail", Method.POST, model);
		}

		public List<RptSeatRevenueDetailRes> RevenueDetailSeat(RevenueDetailReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptSeatRevenueDetailRes>("reports/revenue-detail-seat", Method.POST, model);
		}

		public List<RptRevenueSummary> RevenueSummary(RevenueDetailRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueSummary>("reports/revenue-summary", Method.POST, model);
		}

		public List<RptBookingSummaryRevenueB2B> RevenueSummaryBookingB2B(RevenueSummaryBookingB2B model)
		{
			model.SiteId = Global.SiteID;
			return ApiUtility.CallApi<RptBookingSummaryRevenueB2B>("reports/revenue-summary-booking-b2b", Method.POST, model);
		}

		public List<RptBookingDetailByProfile> BookingDetailByProfile(RevenueSummaryBookingB2B model)
		{
			model.SiteId = Global.SiteID;
			return ApiUtility.CallApi<RptBookingDetailByProfile>("reports/booking-detail-by-profile", Method.POST, model);
		}

		public List<RptDetailUseServiceByACM> DetailUseServiceByAcm(DetailUseServiceByAcmRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptDetailUseServiceByACM>("reports/detail-use-service-by-acm", Method.POST, model);
		}

		public List<RptRefundService> RefundService(RefundServiceModel model)
		{
			return ApiUtility.CallApi<RptRefundService>("reports/refund-service", Method.POST, model);
		}

		public List<RptTcpLog> TcpLog(TcpLogModel model)
		{
			return ApiUtility.CallApi<RptTcpLog>("reports/tcp-log", Method.POST, model);
		}

		public List<RptServiceSales> ServiceSales(string sessionID)
		{
			return ApiUtility.CallApi<RptServiceSales>("reports/service-sales?sessionID=" + sessionID, Method.GET);
		}

		public List<RptRevenueB2BSummaryByShift> RevenueB2BSummaryByShift(string sessionID)
		{
			return ApiUtility.CallApi<RptRevenueB2BSummaryByShift>("reports/revenue-summary-b2b-by-shift?sessionID=" + sessionID, Method.POST);
		}

		public List<RptRevenueSummaryByShift> PaymentByCashier(string sessionID)
		{
			return ApiUtility.CallApi<RptRevenueSummaryByShift>("reports/payment-by-cashier?sessionID=" + sessionID, Method.GET);
		}

		public List<RptDetailsService> DetailsService(DetailsServiceRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptDetailsService>("reports/details-service", Method.POST, model);
		}

		public List<RptRevenueByService> RevenueByService(RevenueByServiceRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueByService>("reports/revenue-by-service", Method.POST, model);
		}

		public List<RptRevenueByMonth> RevenueByMonth(RevenueByMonthRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueByMonth>("reports/revenue-by-month", Method.POST, model);
		}

		public List<RptPaymentByMonth> PaymentByMonth(PaymentByMonthRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptPaymentByMonth>("reports/payment-by-month", Method.POST, model);
		}

		public List<RptRevenueByServiceDetail> RevenueByServiceDetail(RevenueByServiceDetailRequest model)
		{
			model.SiteID = Global.SiteID;
			return ApiUtility.CallApi<RptRevenueByServiceDetail>("reports/revenue-by-service-detail", Method.POST, model);
		}

		public List<RptRevenueByServicePostingJournal> RevenueByServicePostingJournal(RevenueByServicePostingJournalRequest model)
		{
			model.SiteID = Global.SiteID;
			return ApiUtility.CallApi<RptRevenueByServicePostingJournal>("reports/revenue-by-service-posting-journal", Method.POST, model);
		}

		public List<RptTicketPrintHistory> TicketPrintHistory(RptTicketPrintHistoryRequest model)
		{
			return ApiUtility.CallApi<RptTicketPrintHistory>("reports/ticket-print-history", Method.POST, model);
		}

		public List<RptInvoicePrintHistory> InvoicePrintHistory(RptInvoicePrintHistoryRequest model)
		{
			return ApiUtility.CallApi<RptInvoicePrintHistory>("reports/invoice-print-history", Method.POST, model);
		}

		public List<RptBookingDetail> BookingDetail(BookingDetailRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptBookingDetail>("reports/booking-detail", Method.POST, model);
		}

		public List<RevenueServiceByCollaborator> RevenueServiceByCollaborator(RevenueServiceByCollaboratorRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RevenueServiceByCollaborator>("reports/revenue-service-by-collaborator", Method.POST, model);
		}

		public List<RptLoginLogsRes> LoginLog(RptLoginLogsReq model)
		{
			return ApiUtility.CallApi<RptLoginLogsRes>("reports/login-logs", Method.POST, model);
		}

		public List<RptRevenueDetailModel> RevenueDetail2(RptRevenueDetailReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueDetailModel>("reports/revenue-detail2", Method.POST, model);
		}

		public IEnumerable<dynamic> InvoiceByPayment(RptInvoiceByPaymentReq model)
		{
			model.Site = Global.SiteCode;
			model.Lang = "vi";
			return ApiUtility.CallApi<object>("reports/invoice-by-payment", Method.POST, model);
		}

		public List<RptExtendTicketModel> ExtendTicket(RptExtendTicketReq model)
		{
			model.LangCode = "vi";
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptExtendTicketModel>("reports/extend-ticket", Method.POST, model);
		}

		public List<RptBookingUpgradeRes> SummaryBookingUpgrade(RptBookingUpgradeReq model)
		{
			model.LangCode = "vi";
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptBookingUpgradeRes>("reports/summary-booking-upgrade", Method.POST, model);
		}

		public List<RptAllInOneModel> RptAllInOne(RptAllInOneReq model)
		{
			List<RptAllInOneModel> data = ApiUtility.CallApi<RptAllInOneModel>("reports/all-in-one", Method.POST, model);
			foreach (RptAllInOneModel item in data)
			{
				if (item.AccountStatus == "ACTIVE")
				{
					item.AccountStatus = "Chờ sử dụng";
				}
				else if (item.AccountStatus == "LOCK")
				{
					item.AccountStatus = "Bị khóa";
				}
				else if (item.AccountStatus == "CANCEL")
				{
					item.AccountStatus = "Hủy";
				}
				else if (item.AccountStatus == "CLOSE")
				{
					item.AccountStatus = "Ngừng sử dụng";
				}
				else if (item.AccountStatus == "INACTIVE")
				{
					item.AccountStatus = "Chờ kích hoạt";
				}
				else
				{
					item.AccountStatus = "";
				}
				_ = item.HasUsing;
				if (item.HasUsing)
				{
					item.AccountStatus = "Đã sử dụng";
				}
			}
			return data;
		}

		public RptDescription ReportDescription(ReportDescriptionRequest model)
		{
			return ApiUtility.CallApiSimple<RptDescription>("reports/description", Method.POST, model);
		}

		public List<RptBookingStatus> BookingStatus(RptBookingStatusRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptBookingStatus>("reports/booking-status", Method.POST, model);
		}

		public List<RptBookingRevenue> PaymentBookingSumary(RptBookingStatusRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptBookingRevenue>("reports/booking-payment-sumary", Method.POST, model);
		}

		public List<RptCoupon> Coupon(RptCouponRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptCoupon>("reports/rpt-coupon", Method.POST, model);
		}

		public List<RptCouponConfig> CouponConfig(RptCouponRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptCouponConfig>("reports/rpt-coupon-config", Method.POST, model);
		}

		public List<RptCouponTrans> CouponTrans(RptCouponDateTimeRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptCouponTrans>("reports/rpt-coupon-trans", Method.POST, model);
		}

		public List<RptRevenueTotal> RptRevenueTotal(RevenueDetailRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueTotal>("reports/revenue-total", Method.POST, model);
		}

		public List<RptShiftSeatTotal> RptShiftSeatTotal(ShiftReq model)
		{
			return ApiUtility.CallApi<RptShiftSeatTotal>("reports/shift-seat-total", Method.POST, model);
		}

		public List<RptWalletRes> RptWallet(RptWalletReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptWalletRes>("reports/wallet", Method.POST, model);
		}

		public List<RptWalletDepositRes> RptWalletDeposit(RptWalletReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptWalletDepositRes>("reports/wallet-deposit", Method.POST, model);
		}

		public List<RptWalletTransSumaryRes> RptWalletTransSumary(RptWalletReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptWalletTransSumaryRes>("reports/wallet-trans", Method.POST, model);
		}

		public List<RptWalletStatusSumaryRes> RptWalletStatusSumary(RptWalletReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptWalletStatusSumaryRes>("reports/wallet-status", Method.POST, model);
		}

		public List<RevenueTotalByCashierRes> RevenueTotalByCashier(RevenueTotalByCashierReq model)
		{
			model.SiteCode = Global.SiteCode;
			model.LangCode = "vi";
			return ApiUtility.CallApi<RevenueTotalByCashierRes>("reports/revenue-total-by-cashier", Method.POST, model);
		}

		public List<RptRevenueReals> RevenueReals(RevenueDetailRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptRevenueReals>("reports/revenue-reals", Method.POST, model);
		}

		public List<RptChangeTicketHistory> ChangeTicketHistory(RptChangeTicketReq model)
		{
			return ApiUtility.CallApi<RptChangeTicketHistory>("reports/change-ticket-history", Method.POST, model);
		}

		public List<RptDetailBookingUpgradeRes> DetailBookingUpgrade(RptBookingUpgradeReq model)
		{
			model.LangCode = "vi";
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptDetailBookingUpgradeRes>("reports/detail-booking-upgrade", Method.POST, model);
		}

		public List<RptLockerFurthest> GetLockerFurthest(RptLockerFurthestRequest model)
		{
			return ApiUtility.CallApi<RptLockerFurthest>("reports/locker-furthest", Method.POST, model);
		}

		public List<RptDetailPaymentTypeRes> DetailPaymentType(RptDetailPaymentTypeReq model)
		{
			model.LangCode = "vi";
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<RptDetailPaymentTypeRes>("reports/detail-payment-type", Method.POST, model);
		}

		public List<RptAcmCountByDateRes> AcmCountByDate(RptAcmCountByDateReq model)
		{
			model.LangCode = "vi";
			return ApiUtility.CallApi<RptAcmCountByDateRes>("reports/acm-count-by-date", Method.POST, model);
		}
	}
}
