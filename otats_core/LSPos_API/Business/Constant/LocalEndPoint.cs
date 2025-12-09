namespace Business.Constant
{
	public class LocalEndPoint
	{
		public const string prefix = "pos/";

		public const string ServiceGroup_GetData = "service-group/get-data";

		public const string ServicePackageRate_GetData = "service-package-rate/get-data";

		public const string Promotion_GetData = "promotion/get-data";

		public const string PosSell_GetData = "service-rate/get-all-pos";

		public const string ServiceRate_Seat = "service-rate/get-service-rate-seat";

		public const string ServiceRate_GetZoneGroup = "service-rate/get-zone-group-available";

		public const string PosSellShift_GetData = "service-rate/get-shift";

		public const string LockSeat = "booking/lock-seat";

		public const string ClearSeatLock = "booking/clear-lock-seat";

		public const string B2BSell_GetData = "service-rate/get-all";

		public const string Customer_GetData = "customer/get-all";

		public const string Customer_AddNew = "customer/create";

		public const string Customer_Update = "customer/update";

		public const string Profile_GetData = "profile/get-data";

		public const string Promotion_GetByProfile = "promotion/get-data-by-profile";

		public const string ServiceRate_GetSeatByZone = "service-rate/get-seat-by-zone";

		public const string AR_GetData = "ar/get-data-ar";

		public const string AR_GetTrans = "ar/get-ar-trans";

		public const string AR_GetByID = "ar/get-master-by-profile";

		public const string AR_GetPayment = "ar/get-payment";

		public const string AR_InsertAR = "ar/insert-trans-ar";

		public const string AR_InsertBooking = "ar/insert-trans-booking";

		public const string AR_GetBookingCanAR = "ar/get-booking-can-ar";

		public const string AR_Allocate = "ar/allocate";

		public const string AR_Refund = "ar/refund";

		public const string AR_Unallocated = "ar/unallocated-trans";

		public const string AR_CheckBooking = "ar/check-booking";

		public const string AR_RptCurrent = "ar/rpt-ar-current";

		public const string AR_RptListDetail = "ar/rpt-ar-detail";

		public const string AR_GetAllocated = "ar/get-trans-allocated";

		public const string AR_GetDetail = "ar/detail-trans";

		public const string AR_GetListARApprove = "ar/list-ar-approve";

		public const string Login = "session/login";

		public const string Role_GetByUser = "session/get-roles-pos";

		public const string Session_Check = "session/check-status";

		public const string Session_Open = "session/open-session";

		public const string Session_Close = "session/close-session";

		public const string Session_CloseAll = "session/close-session-master";

		public const string Session_Detail = "session/details";

		public const string Session_GetSessionMasterOpen = "session/get-session-master-open";

		public const string Session_GetData = "session/get-data";

		public const string Session_AdminCloseSession = "session/admin-close-session";

		public const string Invoice_GetData = "invoice/get-data";

		public const string Invoice_Adjust = "invoice/get-invoice-adjust";

		public const string Invoice_DetailAdjust = "invoice/detail-adjust-service";

		public const string Invoice_GetRepay = "invoice/get-invoice-repay";

		public const string Invoice_GetList = "invoice/get-list";

		public const string Invoice_GetListByInvoiceDate = "invoice/get-by-invoice-date";

		public const string Invoice_GetService = "invoice/get-service-by-invoice";

		public const string Invoice_GetService1 = "invoice/get-service-by-invoice1";

		public const string InvoicePaymentType_GetData = "invoice/get-invoice-payment-type";

		public const string Invoice_GetDetail = "invoice/get-detail-invoice";

		public const string InvoicePaymentType_GetByProfile = "invoice/get-payment-type-by-profile";

		public const string Invoice_PrintInvoiceTax = "invoice/print-invoice-tax";

		public const string Invoice_ExportInvoiceTax = "invoice/export-invoice-tax";

		public const string Invoice_GenerateInvoiceTax = "invoice/generate-invoice-tax";

		public const string Invoice_ChangePaymentType = "invoice/change-payment-type";

		public const string Invoice_BookingPayment = "invoice/get-booking-payment-by-invoice";

		public const string Invoice_GetServiceSerialNumber = "invoice/get-service-serial-number";

		public const string Common_UpgradeServiceRate = "common/get-service-rate-upgrade";

		public const string Wallet_Create = "wallet/create-new";

		public const string Wallet_RecentRepay = "wallet/get-booking-recent";

		public const string Wallet_CheckUsing = "wallet/check-using";

		public const string Wallet_UpdateMember = "wallet/update-member";

		public const string Wallet_GetServicWalletID = "wallet/get-service-wallet-id";

		public const string Wallet_GetList = "wallet/get-list";

		public const string Wallet_Infor = "wallet/information";

		public const string Wallet_RechargeMoney = "wallet/recharge-money";

		public const string Wallet_Payment = "wallet/payment";

		public const string Wallet_Checkout = "wallet/check-out";

		public const string Wallet_UpdateLocker = "wallet/update-locker";

		public const string Wallet_GetLocker = "wallet/get-locker";

		public const string Wallet_GetResInvInfor = "wallet/get-res-invinfo";

		public const string Booking_CancelService = "booking/cancel-service";

		public const string Booking_CancelInvoice = "booking/cancel-invoice";

		public const string Booking_CancelBookingB2B = "booking/cancel-booking-b2b";

		public const string Booking_CancellationRule = "booking/cancellation-rule";

		public const string Booking_CancelList = "booking/cancel-bookings-list";

		public const string Booking_Create = "booking/create";

		public const string Booking_ConfirmPos = "booking/confirm-pos";

		public const string Booking_Pos = "booking/pos";

		public const string Booking_PosSeat = "booking/pos-seat";

		public const string Booking_GetData = "booking/booking-list-pos";

		public const string Booking_GetDataB2B = "booking/booking-list-b2b";

		public const string Booking_GetDataB2BDetail = "booking/booking-list-b2b-detail";

		public const string Booking_Detail = "booking/detail-booking";

		public const string Booking_DetailPos = "booking/detail-booking-pos";

		public const string Booking_DetailInfo = "booking/detail-booking-info";

		public const string Booking_ExportTicket = "booking/export-ticket";

		public const string Booking_ConfirmAndExportTicket = "booking/confirm-and-export-ticket";

		public const string Booking_UpgradeServiceRate = "booking/upgrade-service-rate";

		public const string Booking_RefundPayment = "booking/refund-booking-payment";

		public const string Booking_ActiveTicketBookingB2B = "booking/active-ticket-booking-b2b";

		public const string Booking_RepayServiceRate = "booking/repay-service-rate";

		public const string Member_GetList = "member/get-list";

		public const string Member_Create = "member/create";

		public const string Member_Edit = "member/edit";

		public const string Member_GetByID = "member/get-detail";

		public const string Member_GetTicket = "member/get-ticket";

		public const string Repay_ServiceRate = "common/detail-repay-service-rate";

		public const string AutoGenQR = "common/auto-gen-qrcode";

		public const string HistoryImageACM = "common/history-image-by-acm";

		public const string Computer_GetId = "common/get-computer-pos";

		public const string GetCardReaderByComputer = "common/get-card-reader-by-computer";

		public const string GetComputerCamera = "common/get-computer-camera";

		public const string Site_GetData = "common/get-sites";

		public const string SystemConfig = "common/get-system-config";

		public const string SystemInfo = "common/get-system-info";

		public const string CheckCard = "common/check-card";

		public const string Site_GetInfo = "common/get-site-info";

		public const string ServerDatetime = "common/get-server-date-time";

		public const string Cashier_GetData = "common/get-cashier";

		public const string SaleInCharge_GetData = "common/get-sale";

		public const string Service_GetData = "common/get-service";

		public const string ServiceRate_GetData = "common/get-service-rate";

		public const string Profile_GetDataAll = "common/get-profile";

		public const string ServiceRate_GetByPos = "common/get-service-by-pos";

		public const string ServiceRateFOC_GetByPos = "common/get-service-rate-FOC-by-pos";

		public const string Computer_GetACM = "common/get-acm";

		public const string ServiceSubGroup_GetData = "common/get-service-sub-group";

		public const string ServiceGroup_GetData1 = "common/get-service-group";

		public const string GetListComputer = "common/get-list-computer";

		public const string Shift_GetData = "common/get-shift";

		public const string Wallet_found = "common/found-wallet-info";

		public const string GetCollaborator = "common/get-collaborator";

		public const string Versions_GetLast = "common/get-last-version";

		public const string User_ChangePassword = "common/change-password";

		public const string Common_ChangeTicketCode = "common/change-ticket-code";

		public const string Common_GetTicketInfor = "common/get-ticket-info";

		public const string Common_GetShiftName = "common/GetShiftName";

		public const string Common_InvoiceCMC = "common/ExportInvoiceCMC";

		public const string AppSettings_Update = "common/update-app-settings";

		public const string AppSettings_GetData = "common/get-app-settings";

		public const string Account_CheckServiceCode = "common/check-service-code";

		public const string PrintTicket = "common/print-ticket";

		public const string PrintInvoice = "common/print-invoice";

		public const string ExtendTicket = "common/extend-ticket";

		public const string DashboardTicket = "dashboard/ticket";

		public const string LoginLog = "common/login-log";

		public const string HistoryImageByACM = "common/history-image-by-acm";

		public const string CheckVoucher = "common/check-voucher-using";

		public const string GetTicketUpdateStatus = "common/get-ticket-update-status";

		public const string UpdateTicketStatus = "common/update-ticket-status";

		public const string GetAccount = "common/get-account";

		public const string GetApiConnectPOS = "common/get-api-connect-pos";

		public const string AccountUsing = "common/account-using";

		public const string SendEmail = "common/send-mail";

		public const string FormEmail = "common/get-email-sent";

		public const string UpdateMemberAccount = "common/update-member-account";

		public const string Report_RevenueSummaryByShift = "reports/revenue-summary-by-shift";

		public const string Report_RevenueDetail = "reports/revenue-detail";

		public const string Report_RevenueDetailSeat = "reports/revenue-detail-seat";

		public const string Report_RevenueSummary = "reports/revenue-summary";

		public const string Report_RevenueSummaryB2B = "reports/revenue-summary-booking-b2b";

		public const string Report_BookingDetailByProfile = "reports/booking-detail-by-profile";

		public const string Report_DetailUseServiceByACM = "reports/detail-use-service-by-acm";

		public const string Report_RefundService = "reports/refund-service";

		public const string Report_TcpLog = "reports/tcp-log";

		public const string Report_ServiceSales = "reports/service-sales";

		public const string Report_RevenueB2BSummaryByShift = "reports/revenue-summary-b2b-by-shift";

		public const string Report_PaymentByCashier = "reports/payment-by-cashier";

		public const string Report_DetailsService = "reports/details-service";

		public const string Report_RevenueByService = "reports/revenue-by-service";

		public const string Report_RevenueByServiceDetail = "reports/revenue-by-service-detail";

		public const string Report_RevenueByServicePostingJournal = "reports/revenue-by-service-posting-journal";

		public const string Report_PrintTicketHistory = "reports/ticket-print-history";

		public const string Report_PrintInvoiceHistory = "reports/invoice-print-history";

		public const string Report_ChangeTicketHistory = "reports/change-ticket-history";

		public const string Report_BookingDetail = "reports/booking-detail";

		public const string Report_RevenueServiceByCollaborator = "reports/revenue-service-by-collaborator";

		public const string Report_BookingTotal = "reports/booking-total";

		public const string Report_BookingTotalByProfile = "reports/booking-total-by-profile";

		public const string Report_RevenueByMonth = "reports/revenue-by-month";

		public const string Report_PaymentByMonth = "reports/payment-by-month";

		public const string Report_LoginLogs = "reports/login-logs";

		public const string Report_RevenueDetail2 = "reports/revenue-detail2";

		public const string Report_ExtendTicket = "reports/extend-ticket";

		public const string Report_BookingUpgrade = "reports/summary-booking-upgrade";

		public const string Report_InvoiceByPayment = "reports/invoice-by-payment";

		public const string Report_Wallet = "reports/wallet";

		public const string Report_WalletDeposit = "reports/wallet-deposit";

		public const string Report_WalletTransSumary = "reports/wallet-trans";

		public const string Report_WalletStatusSumary = "reports/wallet-status";

		public const string Report_RevenueTotalByCashier = "reports/revenue-total-by-cashier";

		public const string Report_RevenueReals = "reports/revenue-reals";

		public const string Report_AllInOne = "reports/all-in-one";

		public const string Report_Description = "reports/description";

		public const string Report_BookingStatus = "reports/booking-status";

		public const string Report_PaymentBookingSumary = "reports/booking-payment-sumary";

		public const string Report_Coupon = "reports/rpt-coupon";

		public const string Report_CouponConfig = "reports/rpt-coupon-config";

		public const string Report_CouponTrans = "reports/rpt-coupon-trans";

		public const string Report_RevenueTotal = "reports/revenue-total";

		public const string Report_ShiftSeatTotal = "reports/shift-seat-total";

		public const string Report_DetailBookingUpgrade = "reports/detail-booking-upgrade";

		public const string Report_DetailPaymentType = "reports/detail-payment-type";

		public const string Report_AcmCountByDate = "reports/acm-count-by-date";

		public const string Report_GetLockerFurthest = "reports/locker-furthest";

		public const string Kios_CheckIn = "kios/check-in";

		public const string Assign_Locker = "locker/assignLocker";

		public const string Reassign_Locker = "locker/reassign";

		public const string Release_Locker = "locker/releaseLocker";

		public const string Check_Card_Locker = "locker/checkCard";

		public const string Get_All_Zones = "locker/getAllZones";

		public const string LockerAndLock_Login = "locker/login";

		public const string Auto_Assign = "locker/autoAssign";

		public const string GetKeyCard = "locker/get-keycard";

		public const string Get_All_Line = "locker/getAllLine";

		public const string Get_All_Locker = "locker/getAllLocker";

		public const string EzInv_GetListPaymentMethod = "ezInvoice/get-payment-method";

		public const string EzInv_GetListSymbol = "ezInvoice/get-symbol";

		public const string EzInv_GetListDenominator = "ezInvoice/get-denominator";

		public const string EzInv_Addinvoice = "ezInvoice/add-invoice";

		public const string EzInv_GetListInvoice = "ezInvoice/list-invoice";

		public const string EzInv_GetInvoiceDetail = "ezInvoice/detail-invoice";

		public const string EzInv_InvoiceExport = "ezInvoice/invoice-export";

		public const string EzInv_GetSignInvoice = "ezInvoice/get-sign-invoice";
	}
}
