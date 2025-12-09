using System.Collections.Generic;
using Business.Model;
using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Wallet;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class WalletRepository
	{
		public Res CheckWalletUsing(string code)
		{
			return ApiUtility.CallApiSimple("wallet/check-using?code=" + code, Method.GET);
		}

		public List<WalletRepayRes> GetWalletRepay(WalletRepayReq model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<WalletRepayRes>("wallet/get-booking-recent", Method.POST, model);
		}

		public Res UpdateMember(string accountID, string memberID)
		{
			return ApiUtility.CallApiSimple("wallet/update-member?accountID=" + accountID + "&memberID=" + memberID, Method.GET);
		}

		public string GetServicWalletID()
		{
			Res data = ApiUtility.CallApiSimple("wallet/get-service-wallet-id?computerID=" + Global.ComputerID.ToString(), Method.GET);
			if (data != null && data.Value != null)
			{
				return data.Value.ToString();
			}
			return "";
		}

		public List<ListWalletRes> GetListWallet(ListWalletReq model)
		{
			return ApiUtility.CallApi<ListWalletRes>("wallet/get-list", Method.POST, new
			{
				Value = model
			});
		}

		public CreateWalletRes CreateWallet(CreatedWalletReq model)
		{
			return ApiUtility.CallApiSimple<CreateWalletRes>("wallet/create-new", Method.POST, new
			{
				Value = model
			});
		}

		public WalletInforRes WalletInfor(WalletInforReq model)
		{
			return ApiUtility.CallApiSimple<WalletInforRes>("wallet/information", Method.POST, new
			{
				Value = model
			});
		}

		public Res RechargeMoneyWallet(RechargeMoneyWalletReq model)
		{
			return ApiUtility.CallApiSimple("wallet/recharge-money", Method.POST, new
			{
				Value = model
			});
		}

		public BookingCreateRes Payment(PaymentWalletReq model)
		{
			return ApiUtility.CallApiSimple<BookingCreateRes>("wallet/payment", Method.POST, new
			{
				Value = model
			});
		}

		public BookingCreateRes CheckOut(CheckoutWalletReq model)
		{
			return ApiUtility.CallApiSimple<BookingCreateRes>("wallet/check-out", Method.POST, new
			{
				Value = model
			});
		}

		public Res UpdateLocker(UpdateLockerReq model)
		{
			return ApiUtility.CallApiSimple<Res>("wallet/update-locker", Method.POST, new
			{
				Value = model
			});
		}

		public List<LockerRes> GetLocker()
		{
			return ApiUtility.CallApi<LockerRes>("wallet/get-locker", Method.GET);
		}

		public List<ResInvRes> GetResInvInfor(WalletInvInforReq model)
		{
			return ApiUtility.CallApiSimple<List<ResInvRes>>("wallet/get-res-invinfo", Method.POST, model);
		}
	}
}
