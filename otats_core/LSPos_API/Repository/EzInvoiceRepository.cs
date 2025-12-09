using System;
using System.Collections.Generic;
using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.EzInv;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class EzInvoiceRepository
	{
		public List<KeyValueObject> GetListPaymentMethod()
		{
			return ApiUtility.CallApi<KeyValueObject>("ezInvoice/get-payment-method?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListSymbol()
		{
			return ApiUtility.CallApi<KeyValueObject>("ezInvoice/get-symbol?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListDenominator()
		{
			return ApiUtility.CallApi<KeyValueObject>("ezInvoice/get-denominator?siteId=" + Global.SiteID, Method.GET);
		}

		public KeyValueObject AddInvoice(EzInvoiceReq req)
		{
			req.SiteID = Guid.Parse(Global.SiteID);
			return ApiUtility.CallApiSimple<KeyValueObject>("ezInvoice/add-invoice", Method.POST, req);
		}

		public List<EzInvoiceList> GetListEzInvoiceTrans(EzInvoiceSearchReq model)
		{
			return ApiUtility.CallApi<EzInvoiceList>("ezInvoice/list-invoice", Method.POST, model);
		}

		public List<EzInvoiceTransactionDetailReq> GetEzInvoiceDetail(Guid id)
		{
			return ApiUtility.CallApi<EzInvoiceTransactionDetailReq>("ezInvoice/detail-invoice?id=" + id, Method.GET);
		}

		public List<ExportInvoice> GetInvoiceExport(Guid shiftID, string view, string invDate)
		{
			if (string.IsNullOrEmpty(invDate) || invDate == "null")
			{
				return ApiUtility.CallApi<ExportInvoice>(string.Concat("ezInvoice/invoice-export?shiftID=", shiftID, "&view=", view, "&invDate=null"), Method.GET);
			}
			return ApiUtility.CallApi<ExportInvoice>(string.Concat("ezInvoice/invoice-export?shiftID=", shiftID, "&view=", view, "&invDate=", invDate), Method.GET);
		}

		public KeyValueObject GetSignCmcInvoice(EzInvoiceReq req)
		{
			req.SiteID = Guid.Parse(Global.SiteID);
			return ApiUtility.CallApiSimple<KeyValueObject>("ezInvoice/get-sign-invoice", Method.POST, req);
		}
	}
}
