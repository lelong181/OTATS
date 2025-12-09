using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Contexts;
using Business.Model;
using BusinessLayer.Model;
using BusinessLayer.Model.API;
using RestSharp;
using Ticket;
using Ticket.Utils;
using LSPos_API.Model.API;
using System.Reflection;
using LSPos_API.Model.RequestModel;

namespace BusinessLayer.Repository
{
	public class CommonRepository
	{
		public List<TicketInfoRes> GetTicketInfo(TicketInfoReq model)
		{
			model.LangCode = "vi";
			return ApiUtility.CallApi<TicketInfoRes>("common/get-ticket-info", Method.POST, model);
		}

		public Res CheckTicketCode(ChangeTicketRes model)
		{
			return ApiUtility.CallApiSimple("common/change-ticket-code", Method.POST, model);
		}

		public Res GetShiftName(string sessionID)
		{
			string langCode = "vi";
			return ApiUtility.CallApiSimple("common/GetShiftName?sessionID=" + sessionID + "&langCode=" + langCode, Method.GET);
		}

		public List<ReportCMC> GetInvoiceCMC(Guid shiftID)
		{
			return ApiUtility.CallApi<ReportCMC>("common/ExportInvoiceCMC?shiftID=" + shiftID, Method.GET);
		}

		public Res CheckCard(string cardID)
		{
			return ApiUtility.CallApiSimple("common/check-card?cardID=" + cardID, Method.GET);
		}

		public Res AutoGenQR()
		{
			return ApiUtility.CallApiSimple("common/auto-gen-qrcode?siteCode=" + Global.SiteCode, Method.GET);
		}

		public SystemConfig GetSystemConfig(string key)
		{
			return ApiUtility.CallApiSimple<SystemConfig>("common/get-system-config?key=" + key, Method.GET);
		}

		public DetailRepayServiceRateRes DetailRepayServiceRate(DetailRepayServiceRateReq model)
		{
			return ApiUtility.CallApiSimple<DetailRepayServiceRateRes>("common/detail-repay-service-rate", Method.POST, model);
		}

		public SystemInfo GetSystemInfo(string key, string localApi = "")
		{
			return ApiUtility.CallApiSimple<SystemInfo>("common/get-system-info?key=" + key, Method.GET, localApi);
		}

		public List<Site> GetSitesData()
		{
			return ApiUtility.CallApi<Site>("common/get-sites", Method.GET);
		}

		public SendEmailTask GetEmailSent(string bookingCode)
		{
			return ApiUtility.CallApiSimple<SendEmailTask>("common/get-email-sent?bookingcode=" + bookingCode, Method.GET);
		}

		public Site GetSiteInfo(string siteID)
		{
			return ApiUtility.CallApiSimple<Site>("common/get-site-info?siteID=" + siteID, Method.GET);
		}

		public DateTime GetServerDatetime(string localApi = "")
		{
			DateTime dt = DateTime.MinValue;
			try
			{
				KeyValueObject data = ApiUtility.CallApiSimple<KeyValueObject>("common/get-server-date-time", Method.GET, localApi);
				if (!string.IsNullOrEmpty(data.Value))
				{
					dt = DateTime.ParseExact(data.Value, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
					if (dt <= DateTime.MinValue)
					{
						dt = DateTime.Now;
					}
				}
				else
				{
					dt = DateTime.Now;
				}
			}
			catch
			{
				dt = DateTime.Now;
			}
			return dt;
		}

		public CardReaderByComputerRes GetCardReaderByComputer(string computerID)
		{
			return ApiUtility.CallApiSimple<CardReaderByComputerRes>("common/get-card-reader-by-computer?computerID=" + computerID, Method.GET);
		}

		public ComputerCameraRes GetComputerCamera(string computerID)
		{
			return ApiUtility.CallApiSimple<ComputerCameraRes>("common/get-computer-camera?computerID=" + computerID, Method.GET);
		}

		public HistoryImageACMResponse ImageAcm(HistoryImageACMReq model)
		{
			return ApiUtility.CallApiSimple<HistoryImageACMResponse>("common/history-image-by-acm", Method.POST, model);
		}

		public List<KeyValueObject> GetListCashier()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-cashier?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListSale()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-sale", Method.GET);
		}

		public List<KeyValueObject> GetListService()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-service?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListServiceRate()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-service-rate?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListProfile()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-profile", Method.GET);
		}

		public List<KeyValueObject> GetListServiceSubGroup()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-service-sub-group?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListServiceGroup()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-service-group?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListComputer()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-list-computer", Method.GET);
		}

		public List<KeyValueObject> GetCollaborator()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-collaborator", Method.GET);
		}

		public Res ChangePassword(ChangePasswordRequest model)
		{
			return ApiUtility.CallApiSimple("common/change-password", Method.POST, model);
		}

		public Versions GetLastVersionPOS()
		{
			return ApiUtility.CallApiSimple<Versions>("common/get-last-version", Method.GET);
		}

		public List<KeyValueObject> GetListServiceByPos()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-service-by-pos?siteId=" + Global.SiteID, Method.GET);
		}

		public List<KeyValueObject> GetListServiceRateFOCByPos()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-service-rate-FOC-by-pos?siteId=" + Global.SiteID, Method.GET);
		}

		public Ticket.AppSettings GetAppSettings(string ComputerID)
		{
			List<Ticket.AppSettings> data = ApiUtility.CallApi<Ticket.AppSettings>("common/get-app-settings?computerID=" + Global.ComputerID, Method.GET);
			return data.FirstOrDefault();
		}

		public Res UpdateAppSettings(Ticket.AppSettings model)
		{
			return ApiUtility.CallApiSimple("common/update-app-settings", Method.POST, model);
		}

		public Res CheckServiceCode(string serviceCode)
		{
			return ApiUtility.CallApiSimple("common/check-service-code?serviceCode=" + serviceCode, Method.GET);
		}

		public Res PrintTicket(string invoiceID, string accountID)
		{
			return ApiUtility.CallApiSimple("common/print-ticket?accountID=" + accountID + "&invoiceID=" + invoiceID + "&userPrint=" + Global.UserID, Method.GET);
		}

		public Res PrintInvoice(string type, string invoiceID)
		{
			return ApiUtility.CallApiSimple("common/print-invoice?type=" + type + "&invoiceID=" + invoiceID + "&userPrint=" + Global.UserID, Method.GET);
		}

		public List<ComputerModel> GetACM()
		{
			return ApiUtility.CallApi<ComputerModel>("common/get-acm?siteID=" + Global.SiteID, Method.GET);
		}

		public TicketDashboardResponse DashboardTicket(TicketDashboardRequest model)
		{
			return ApiUtility.CallApiSimple<TicketDashboardResponse>("dashboard/ticket", Method.POST, model);
		}

		public Res LoginLogs(LoginLogsRequest model, string localApi = "")
		{
			return ApiUtility.CallApiSimple("common/login-log", Method.POST, model, localApi);
		}

		public HistoryImageACMResponse GetHistoryImageByACM(HistoryImageACMReq model)
		{
			return ApiUtility.CallApiSimple<HistoryImageACMResponse>("common/history-image-by-acm", Method.POST, model);
		}

		public Res ExtendTicket(List<ExtendTicketReq> model)
		{
			return ApiUtility.CallApiSimple("common/extend-ticket", Method.POST, model);
		}

		public VoucherRes CheckVoucher(VoucherReq model)
		{
			return ApiUtility.CallApiSimple<VoucherRes>("common/check-voucher-using", Method.POST, model);
		}

		public Account GetAccount(string code)
		{
			return ApiUtility.CallApiSimple<Account>("common/get-account?accountCode=" + code, Method.GET);
		}

		public List<AccountTicketRes> GetTicketUpdateStatus(AccountTicketReq model)
		{
			return ApiUtility.CallApi<AccountTicketRes>("common/get-ticket-update-status", Method.POST, model);
		}

		public Res UpdateTicketStatus(List<AccountTicketRes> model, string note, string username, Guid computerID, string statusNew, KeyValueObject computer)
		{
			List<TicketUpdateStatusReq> lst = new List<TicketUpdateStatusReq>();
			foreach (AccountTicketRes item in model)
			{
				if (computer != null)
				{
					lst.Add(new TicketUpdateStatusReq
					{
						AccountID = item.AccountID,
						ComputerID = computerID,
						Note = note,
						StatusNew = statusNew,
						AccountCode = item.AccountCode,
						StatusOld = item.Status,
						UpdateBy = username,
						ComputerUsingTicketID = computer.Value,
						ServiceID = item.ServiceID.ToString(),
						SiteCode = Global.SiteCode,
						TotalMoney = item.TotalMoney
					});
				}
			}
			return ApiUtility.CallApiSimple("common/update-ticket-status", Method.POST, lst);
		}

		public AccountUsingRes AccountUsing(TicketUpdateStatusReq model)
		{
			return ApiUtility.CallApiSimple<AccountUsingRes>("common/account-using", Method.POST, model);
		}

		//public Res SendEmail(SendEmailTask email)
		//{
		//	return ApiUtility.CallApiSimple("common/send-mail", Method.POST, email);
		//}

		public Res SendEmail(SendEmailTask email)
		{
			try
			{
				MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("noreplyticketsystem2020@gmail.com");
                string[] emailInfoTo = email.ToList.Split(';');
                string[] array = emailInfoTo;
                foreach (string cc in array)
                {
                    if (cc != null && cc != "")
                    {
                        message.To.Add(new MailAddress(cc));
                    }
                }
                message.Subject = email.EmailSubject;
				message.IsBodyHtml = true;
                message.Body = "<b>Cảm ơn bạn đã đặt vé</b>";
                //string[] emailCC = array 
                //string[] array2 = emailCC;
                //foreach (string cc2 in array2)
                //{
                //    if (cc2 != null && cc2 != "")
                //    {
                //        message.CC.Add(new MailAddress(cc2));
                //    }
                //}
                if (email.Cclist != null)
                {
                    string[] emailInfoCC = email.Cclist.Split(';');
                    string[] array3 = emailInfoCC;
                    foreach (string cc3 in array3)
                    {
                        if (cc3 != null && cc3 != "")
                        {
                            message.CC.Add(new MailAddress(cc3));
                        }
                    }
                }
                if (email.Bcclist != null)
                {
                    string[] emailInfoBCC = email.Bcclist.Split(';');
                    string[] array4 = emailInfoBCC;
                    foreach (string bcc in array4)
                    {
                        if (bcc != null && bcc != "")
                        {
                            message.Bcc.Add(new MailAddress(bcc));
						}
					}
				}
    //            smtp.Port = Convert.ToInt32("587");
    //            smtp.Host = "smtp.gmail.com";
				//smtp.EnableSsl = true;
				//smtp.UseDefaultCredentials = false;
				//smtp.Credentials = new NetworkCredential("noreplyticketsystem2020@gmail.com", "Hihi@12345");
				//smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				//smtp.Send(message);
                email.SentDate = DateTime.Now;
                email.IsSent = true;
            }
            catch (Exception ex)
            {
                email.SentDate = DateTime.Now;
                email.SentErrMsg = ex.Message;
                email.IsSent = false;
            }
            if (email.BookingCode == null)
            {
                email.Id = Guid.NewGuid();
                email.BookingCode = "";
                email.CreatedDate = DateTime.Now;
            }
            Res res = new Res
            {
                Status = "SUCCESS"
            };
            res.Value = "1";
            return ApiUtility.CallApiSimple("common/send-mail", Method.POST, email);
		}

		public Res UpdateAccountMember(string memberID, string accountID)
		{
			return ApiUtility.CallApiSimple("common/update-member-account?accountID=" + accountID + "&memberID=" + memberID, Method.GET);
		}

		public List<KeyValueObject> GetListShift()
		{
			return ApiUtility.CallApi<KeyValueObject>("common/get-shift?siteId=" + Global.SiteID, Method.GET);
		}

		public List<FoundWalletInfoRes> WalletFound(FoundWalletInfoReq model)
		{
			return ApiUtility.CallApi<FoundWalletInfoRes>("common/found-wallet-info", Method.POST, model);
		}

		public GetAllServiceRateResponse B2B_GetAllServiceRate(Req<RateAvailableReq> model)
		{
            return ApiUtility.CallB2BApiSimple<GetAllServiceRateResponse>("b2b/get-ticket", Method.POST, model);
        }
    }
}
