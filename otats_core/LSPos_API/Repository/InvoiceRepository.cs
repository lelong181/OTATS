using System;
using System.Collections.Generic;
using System.Linq;
using Business.Model;
using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Sell;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class InvoiceRepository
	{
		public List<InvoiceResponse> GetData(InvoiceRequestModel model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<InvoiceResponse>("invoice/get-data", Method.POST, model);
		}

		public List<InvoiceAdjustRes> GetAdjust(InvoiceAdjustReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<InvoiceAdjustRes>("invoice/get-invoice-adjust", Method.POST, model);
		}

		public DetailAdjustServiceRes DetailAdjust(string invoiceID, string langCode = "vi")
		{
			return ApiUtility.CallApiSimple<DetailAdjustServiceRes>("invoice/detail-adjust-service?invoiceID=" + invoiceID + "&langCode=" + langCode, Method.GET);
		}

		public SaveBookingModel RepayServiceRate(RepayServiceRateReq model)
		{
			return ApiUtility.CallApiSimple<SaveBookingModel>("booking/repay-service-rate", Method.POST, model);
		}

		public List<InvoiceRepayRes> GetRepay(InvoiceRepayReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<InvoiceRepayRes>("invoice/get-invoice-repay", Method.POST, model);
		}

		public List<ServiceRateResponse> GetServiceRateByInvoice(string invoiceID)
		{
			List<ServiceRateResponse> data = ApiUtility.CallApi<ServiceRateResponse>("invoice/get-service-by-invoice?invoiceId=" + invoiceID, Method.GET);
			if (data.Count > 0)
			{
				data = data.Where((ServiceRateResponse m) => m.Quantity > 0).ToList();
			}
			return data;
		}

		public List<ServiceRateResponse> GetServiceRateByInvoice1(string invoiceID)
		{
			List<ServiceRateResponse> data = ApiUtility.CallApi<ServiceRateResponse>("invoice/get-service-by-invoice1?invoiceId=" + invoiceID, Method.GET);
			if (data.Count > 0)
			{
				data = data.Where((ServiceRateResponse m) => m.Quantity > 0).ToList();
			}
			return data;
		}

		public List<InvoicePaymentTypeResponse> GetInvoicePaymentType(string invoiceID)
		{
			return ApiUtility.CallApi<InvoicePaymentTypeResponse>("invoice/get-invoice-payment-type?invoiceId=" + invoiceID, Method.GET);
		}

		public List<BookingPaymentRes1> GetBookingPaymentByInvoice(string invoiceID)
		{
			return ApiUtility.CallApi<BookingPaymentRes1>("invoice/get-booking-payment-by-invoice?invoiceId=" + invoiceID, Method.GET);
		}

		public List<InvoiceResponse> GetListByDate(InvoiceRequestModel model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<InvoiceResponse>("invoice/get-list", Method.POST, model);
		}

		public InvoiceDetailResponse GetDetail(string invoiceId)
		{
			string siteCode = Global.SiteCode;
			string langCode = "vi";
			return ApiUtility.CallApiSimple<InvoiceDetailResponse>("invoice/get-detail-invoice?invoiceId=" + invoiceId + "&siteCode=" + siteCode + "&langCode=" + langCode, Method.GET);
		}

		public List<PaymentTypeModel> GetPaymentTypeByProfile(string profileId)
		{
			return ApiUtility.CallApi<PaymentTypeModel>("invoice/get-payment-type-by-profile?profileID=" + profileId, Method.GET);
		}

		public Res ChangePaymentType(List<ChangePaymentType> model)
		{
			return ApiUtility.CallApiSimple("invoice/change-payment-type", Method.POST, model);
		}

		public Res PrintInvoiceTax(string invoiceID)
		{
			return ApiUtility.CallApiSimple("invoice/print-invoice-tax?invoiceID=" + invoiceID, Method.GET);
		}

		public List<InvoicePrintTaxResponse> ExportInvoiceTax(DateTime invoiceDate)
		{
			return ApiUtility.CallApi<InvoicePrintTaxResponse>("invoice/export-invoice-tax", Method.POST, new GenerateInvoiceTax
			{
				InvoiceDate = invoiceDate
			});
		}

		public List<InvoiceResponse> GetListByInvoiceDate(InvoiceByDateRequest model)
		{
			return ApiUtility.CallApi<InvoiceResponse>("invoice/get-by-invoice-date", Method.POST, model);
		}

		public Res GenerateInvoiceTax(GenerateInvoiceTax model)
		{
			return ApiUtility.CallApiSimple("invoice/generate-invoice-tax", Method.POST, model);
		}

		public ListServiceRateUpgradeRes ServiceRateUpgrade(ListServiceRateUpgradeReq model)
		{
			return ApiUtility.CallApiSimple<ListServiceRateUpgradeRes>("common/get-service-rate-upgrade", Method.POST, model);
		}

		public List<m_ServiceSerialNumber> GetServiceSerialNumber(string siteCode)
		{
			return ApiUtility.CallApi<m_ServiceSerialNumber>("invoice/get-service-serial-number?siteCode=" + siteCode, Method.GET);
		}
	}
}
