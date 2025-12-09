using System;
using System.Collections.Generic;
using System.Data;
using BusinessLayer.Model;
using BusinessLayer.Model.API;

namespace Ticket
{
	public static class Global
	{
		private static string _AppUserID;

		private static int _AppGroupID;

		private static string _AppUserName;

		private static string _UserName;

		private static string _AppFullName;

		private static string _AppPassword;

		private static int _AppRegionID;

		private static bool _IsRoot = false;

		private static string _ConnectionString = "";

		private static string _AppUserCode;

		private static int _StoreID;

		private static decimal _ExchangeRate = default(decimal);

		private static string _CashierNo = "";

		private static string _SessionID = string.Empty;

		private static bool _isNotCreateSession = true;

		private static string _ServerName = "";

		private static string _DatabaseName = "";

		private static int _OutletID;

		private static string _ZoneID;

		private static string _ComputerID;

		private static bool _isTravelCardSell;

		private static int _SelectedDiscountGroupID;

		private static int _PMSCableServiceTypeID;

		private static int _PMSHotelServiceTypeID;

		private static int _PMSPOSServiceTypeID;

		private static int _PMSCampFireServiceTypeID;

		private static int _PMSMeetingServiceTypeID;

		private static int _PMSTransportServiceTypeID;

		private static int _PMSOtherServiceTypeID;

		private static int _ServiceCardPaymentID;

		private static int _ServiceClearDepositID;

		private static int _SaleTypeDepoisitExchangeRechargeID;

		private static int _PaymentTypeDepoisitExchangeID;

		private static int _SaleTypeDepositRechargeID;

		private static int _SellCardLoseTraveGroupID;

		private static int _FixBookingNgoaiGiaoID;

		private static int _CardID_Length;

		private static int _CardID_Reader;

		private static string _siteID;

		private static string _siteCode = "";

		private static string _siteAddess = "";

		private static DataTable _Property = null;

		private static int _PropertyID = 0;

		private static string _propertyname = "";

		private static int _Property_cID = 0;

		private static string _Property_cName = "";

		private static string _ConnectionStringtmp = "";

		private static string _ConnectionStringDiconnect = "";

		private static decimal _exchangePoint;

		private static int _vinidPaymentTypeID;

		private static int _vinidCardLength;

		private static int _readerType;

		private static int _brecus;

		private static string _MerchantId;

		private static string _TerminalId;

		private static string _VersionNo;

		public static string VersionNo = "1.6.4";

		private static string _AppVersion;

		private static string _TimeZone;

		private static string _ChannelId;

		private static string _OperationID;

		private static string _OperatorPIN;

		private static string _VinID_SiteName = "";

		private static bool _Is_VPLL = false;

		public static List<PermissionRole> Roles = new List<PermissionRole>();

		public static CardReaderByComputerRes cardReader { get; set; } = null;


		public static ComputerCameraRes ComputerCamera { get; set; } = null;


		public static bool RequiredLicense { get; set; } = false;


		public static bool B2B_HidePriceOnTicket { get; set; } = false;


		public static bool B2B_PrintInvoiceConfirmBooking { get; set; } = false;


		public static m_ApiConnect ApiConnect { get; set; }

		public static string ComputerID
		{
			get
			{
				return _ComputerID;
			}
			set
			{
				_ComputerID = value;
			}
		}

		public static string TypeTicketQR { get; set; } = "HEXA";


		public static int DefaultServiceID { get; set; }

		public static bool IsTravelCardSell
		{
			get
			{
				return _isTravelCardSell;
			}
			set
			{
				_isTravelCardSell = value;
			}
		}

		public static decimal ExchangeRate
		{
			get
			{
				return _ExchangeRate;
			}
			set
			{
				_ExchangeRate = value;
			}
		}

		public static bool RoleRepayAccountHadUsing { get; set; } = false;


		public static bool RoleUpgradeAccountHadUsing { get; set; } = false;


		public static string SessionID
		{
			get
			{
				return _SessionID;
			}
			set
			{
				_SessionID = value;
			}
		}

		public static string SessionNo { get; set; }

		public static string SessionMasterID { get; set; }

		public static int? SessionMasterNo { get; set; }

		public static DateTime? SessionMaster_StartTime { get; set; }

		public static bool IsNotCreateSession
		{
			get
			{
				return _isNotCreateSession;
			}
			set
			{
				_isNotCreateSession = value;
			}
		}

		public static bool IsProduct { get; set; }

		public static string CashierNo
		{
			get
			{
				return _CashierNo;
			}
			set
			{
				_CashierNo = value;
			}
		}

		public static string ServerName
		{
			get
			{
				return _ServerName;
			}
			set
			{
				_ServerName = value;
			}
		}

		public static string DatabaseName
		{
			get
			{
				return _DatabaseName;
			}
			set
			{
				_DatabaseName = value;
			}
		}

		public static bool IsCaptureCamera { get; set; } = false;


		public static string UserCode
		{
			get
			{
				return _AppUserCode;
			}
			set
			{
				_AppUserCode = value;
			}
		}

		public static string UserID
		{
			get
			{
				return _AppUserID;
			}
			set
			{
				_AppUserID = value;
			}
		}

		public static int RegionID
		{
			get
			{
				return _AppRegionID;
			}
			set
			{
				_AppRegionID = value;
			}
		}

		public static string ConnectionString
		{
			get
			{
				return _ConnectionString;
			}
			set
			{
				_ConnectionString = value;
			}
		}

		public static int AppGroupID
		{
			get
			{
				return _AppGroupID;
			}
			set
			{
				_AppGroupID = value;
			}
		}

		public static string AppUserName
		{
			get
			{
				return _AppUserName;
			}
			set
			{
				_AppUserName = value;
			}
		}

		public static string UserName
		{
			get
			{
				return _UserName;
			}
			set
			{
				_UserName = value;
			}
		}

		public static string Fullname { get; set; }

		public static bool SaleWallet { get; set; } = false;


		public static string AppFullName
		{
			get
			{
				return _AppFullName;
			}
			set
			{
				_AppFullName = value;
			}
		}

		public static string AppPassword
		{
			get
			{
				return _AppPassword;
			}
			set
			{
				_AppPassword = value;
			}
		}

		public static bool IsRoot
		{
			get
			{
				return _IsRoot;
			}
			set
			{
				_IsRoot = value;
			}
		}

		public static int StoreID
		{
			get
			{
				return _StoreID;
			}
			set
			{
				_StoreID = value;
			}
		}

		public static string SiteI18n_Name { get; set; }

		public static string SiteI18n_Description { get; set; }

		public static int OutlefID
		{
			get
			{
				return _OutletID;
			}
			set
			{
				_OutletID = value;
			}
		}

		public static string ZoneID
		{
			get
			{
				return _ZoneID;
			}
			set
			{
				_ZoneID = value;
			}
		}

		public static int SelectedDiscountGroupID
		{
			get
			{
				return _SelectedDiscountGroupID;
			}
			set
			{
				_SelectedDiscountGroupID = value;
			}
		}

		public static int PMSCableServiceTypeID
		{
			get
			{
				return _PMSCableServiceTypeID;
			}
			set
			{
				_PMSCableServiceTypeID = value;
			}
		}

		public static int PMSHotelServiceTypeID
		{
			get
			{
				return _PMSHotelServiceTypeID;
			}
			set
			{
				_PMSHotelServiceTypeID = value;
			}
		}

		public static int PMSPOSServiceTypeID
		{
			get
			{
				return _PMSPOSServiceTypeID;
			}
			set
			{
				_PMSPOSServiceTypeID = value;
			}
		}

		public static int PMSCampFireServiceTypeID
		{
			get
			{
				return _PMSCampFireServiceTypeID;
			}
			set
			{
				_PMSCampFireServiceTypeID = value;
			}
		}

		public static int PMSMeetingServiceTypeID
		{
			get
			{
				return _PMSMeetingServiceTypeID;
			}
			set
			{
				_PMSMeetingServiceTypeID = value;
			}
		}

		public static int PMSTransportServiceTypeID
		{
			get
			{
				return _PMSTransportServiceTypeID;
			}
			set
			{
				_PMSTransportServiceTypeID = value;
			}
		}

		public static int PMSOtherServiceTypeID
		{
			get
			{
				return _PMSOtherServiceTypeID;
			}
			set
			{
				_PMSOtherServiceTypeID = value;
			}
		}

		public static int ServiceCardPaymentID
		{
			get
			{
				return _ServiceCardPaymentID;
			}
			set
			{
				_ServiceCardPaymentID = value;
			}
		}

		public static int ServiceClearDepositID
		{
			get
			{
				return _ServiceClearDepositID;
			}
			set
			{
				_ServiceClearDepositID = value;
			}
		}

		public static int SaleTypeDepoisitExchangeRechargeID
		{
			get
			{
				return _SaleTypeDepoisitExchangeRechargeID;
			}
			set
			{
				_SaleTypeDepoisitExchangeRechargeID = value;
			}
		}

		public static int PaymentTypeDepoisitExchangeID
		{
			get
			{
				return _PaymentTypeDepoisitExchangeID;
			}
			set
			{
				_PaymentTypeDepoisitExchangeID = value;
			}
		}

		public static int SaleTypeDepositRechargeID
		{
			get
			{
				return _SaleTypeDepositRechargeID;
			}
			set
			{
				_SaleTypeDepositRechargeID = value;
			}
		}

		public static int SellCardLoseTraveGroupID
		{
			get
			{
				return _SellCardLoseTraveGroupID;
			}
			set
			{
				_SellCardLoseTraveGroupID = value;
			}
		}

		public static int FixBookingNgoaiGiaoID
		{
			get
			{
				return _FixBookingNgoaiGiaoID;
			}
			set
			{
				_FixBookingNgoaiGiaoID = value;
			}
		}

		public static int CardID_Length
		{
			get
			{
				return _CardID_Length;
			}
			set
			{
				_CardID_Length = value;
			}
		}

		public static int CardID_Reader
		{
			get
			{
				return _CardID_Reader;
			}
			set
			{
				_CardID_Reader = value;
			}
		}

		public static string SiteType { get; set; } = "NORMAL";


		public static string SiteID
		{
			get
			{
				return _siteID;
			}
			set
			{
				_siteID = value;
			}
		}

		public static string SiteCode
		{
			get
			{
				return _siteCode;
			}
			set
			{
				_siteCode = value;
			}
		}

		public static string SiteAddress
		{
			get
			{
				return _siteAddess;
			}
			set
			{
				_siteAddess = value;
			}
		}

		public static DataTable Property
		{
			get
			{
				return _Property;
			}
			set
			{
				_Property = value;
			}
		}

		public static int PropertyID
		{
			get
			{
				return _PropertyID;
			}
			set
			{
				_PropertyID = value;
			}
		}

		public static string PropertyName
		{
			get
			{
				return _propertyname;
			}
			set
			{
				_propertyname = value;
			}
		}

		public static int Property_cID
		{
			get
			{
				return _Property_cID;
			}
			set
			{
				_Property_cID = value;
			}
		}

		public static string Property_cName
		{
			get
			{
				return _Property_cName;
			}
			set
			{
				_Property_cName = value;
			}
		}

		public static string ConnectionStringtmp
		{
			get
			{
				return _ConnectionStringtmp;
			}
			set
			{
				_ConnectionStringtmp = value;
			}
		}

		public static string ConnectionStringDiconnect
		{
			get
			{
				return _ConnectionStringDiconnect;
			}
			set
			{
				_ConnectionStringDiconnect = value;
			}
		}

		public static decimal VinID_ExchangePoint
		{
			get
			{
				return _exchangePoint;
			}
			set
			{
				_exchangePoint = value;
			}
		}

		public static int VinID_PaymentTypeID
		{
			get
			{
				return _vinidPaymentTypeID;
			}
			set
			{
				_vinidPaymentTypeID = value;
			}
		}

		public static int VinID_CardLength
		{
			get
			{
				return _vinidCardLength;
			}
			set
			{
				_vinidCardLength = value;
			}
		}

		public static int CardReaderType
		{
			get
			{
				return _readerType;
			}
			set
			{
				_readerType = value;
			}
		}

		public static int Brecus
		{
			get
			{
				return _brecus;
			}
			set
			{
				_brecus = value;
			}
		}

		public static string MerchantId
		{
			get
			{
				return _MerchantId;
			}
			set
			{
				_MerchantId = value;
			}
		}

		public static string TerminalId
		{
			get
			{
				return _TerminalId;
			}
			set
			{
				_TerminalId = value;
			}
		}

		public static bool LicenseCheckEnable { get; set; }

		public static string AppName { get; set; }

		public static string AppVersion
		{
			get
			{
				return _AppVersion;
			}
			set
			{
				_AppVersion = value;
			}
		}

		public static string TimeZone
		{
			get
			{
				return _TimeZone;
			}
			set
			{
				_TimeZone = value;
			}
		}

		public static string ChannelId
		{
			get
			{
				return _ChannelId;
			}
			set
			{
				_ChannelId = value;
			}
		}

		public static string OperationID
		{
			get
			{
				return _OperationID;
			}
			set
			{
				_OperationID = value;
			}
		}

		public static string OperatorPIN
		{
			get
			{
				return _OperatorPIN;
			}
			set
			{
				_OperatorPIN = value;
			}
		}

		public static string VinID_SiteName
		{
			get
			{
				return _VinID_SiteName;
			}
			set
			{
				_VinID_SiteName = value;
			}
		}

		public static bool IsVinpearlland
		{
			get
			{
				return _Is_VPLL;
			}
			set
			{
				_Is_VPLL = value;
			}
		}

		public static string ProfileID { get; set; } = "620A7319-28D7-4FA0-B8AB-19A98E5C7D6E";
		public static string ProfileCode { get; set; } = "2700276575";


		public static void GetLogInfor()
		{
			try
			{
			}
			catch
			{
			}
		}

		public static ProfileModel _Profile { get; set; }
	}
}
