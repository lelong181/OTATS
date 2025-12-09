using System;
using System.Threading;
using SmartLockerSDKNet45;
using SmartLockerSDKNet45.Entities;

namespace BusinessLayer.Repository
{
	public class LockerAndLock
	{
		public static string customerID = "001";

		public static string vendorID = "001";

		public static string keyCardInfo = "";

		private static LockerInfo lockerInfor;

		private static KeyInfo keyInfo;

		private static CardInfo cardInfo;

		public static Thread readCom = null;




		public static void Init()
		{
			try
			{
				if (!string.IsNullOrEmpty(keyCardInfo))
				{
					lockerInfor = new LockerInfo();
					keyInfo = new KeyInfo();
					cardInfo = new CardInfo();
					keyInfo.customerID = customerID;
					keyInfo.vendorID = vendorID;
					keyInfo.keyCardInfo = keyCardInfo;
					lockerInfor.customerID = customerID;
					lockerInfor.vendorID = vendorID;
					lockerInfor.keyCardInfo = keyCardInfo;
					SmartLockerSDKLib.SmartLockerInitDE620Comm(out var _);
					SmartLockerSDKLib.SmartLockerRegKeyInfo(keyInfo, out var _);
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message, "ERROR_INIT_READER_LOCKER");
			}
		}

		public static void InitReader()
		{
			try
			{
				lockerInfor = new LockerInfo();
				keyInfo = new KeyInfo();
				cardInfo = new CardInfo();
				keyInfo.customerID = customerID;
				keyInfo.vendorID = vendorID;
				keyInfo.keyCardInfo = keyCardInfo;
				lockerInfor.customerID = customerID;
				lockerInfor.vendorID = vendorID;
				lockerInfor.keyCardInfo = keyCardInfo;
				SmartLockerSDKLib.SmartLockerInitDE620Comm(out var _);
				SmartLockerSDKLib.SmartLockerRegKeyInfo(keyInfo, out var _);
				if (readCom != null)
				{
					readCom.Interrupt();
					readCom.Abort();
					readCom.Join();
					readCom = null;
				}
				readCom = new Thread(GetDataFormCard);
				readCom.Start();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public static void InitReaderWrite()
		{
			try
			{
				lockerInfor = new LockerInfo();
				keyInfo = new KeyInfo();
				cardInfo = new CardInfo();
				keyInfo.customerID = customerID;
				keyInfo.vendorID = vendorID;
				keyInfo.keyCardInfo = keyCardInfo;
				lockerInfor.customerID = customerID;
				lockerInfor.vendorID = vendorID;
				lockerInfor.keyCardInfo = keyCardInfo;
				SmartLockerSDKLib.SmartLockerInitDE620Comm(out var _);
				SmartLockerSDKLib.SmartLockerRegKeyInfo(keyInfo, out var _);
				if (readCom != null)
				{
					readCom.Interrupt();
					readCom.Abort();
					readCom.Join();
					readCom = null;
				}
				readCom = new Thread(GetDataFormCard);
				readCom.Start();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static void GetDataFormCard()
		{
			string msg = "";
			while (true)
			{
				try
				{
					Thread.Sleep(100);
					cardInfo = SmartLockerSDKLib.SmartLockerCSNCard(out var _msg);
					SmartLockerSDKLib.SmartLockerCheckCard(out var _);
					if (cardInfo != null && !string.IsNullOrEmpty(cardInfo.cardRFID))
					{
						//DisplayData(cardInfo.cardRFID);
						msg = _msg;
					}
				}
				catch (Exception ex)
				{
					msg = ex.Message;
				}
			}
		}


		public static string ReadCard()
		{
			string msg = "";
			try
			{
				cardInfo = SmartLockerSDKLib.SmartLockerCSNCard(out msg);
				return cardInfo.cardRFID;
			}
			catch (Exception ex)
			{
				return ex.Message.ToString() + ", msg:" + msg;
			}
		}

		public static string WriteCard(string cardId, string zoneAddress, string lineAddress, string lockerAddress, DateTime dateStart, DateTime dateEnd, string revision_number = "")
		{
			try
			{
				DateTime newdateStart = dateStart;
				DateTime newdateEnd = dateEnd;
				lockerInfor.keyCardInfo = keyCardInfo;
				lockerInfor.zone_address = zoneAddress;
				lockerInfor.line_address = lineAddress;
				lockerInfor.locker_address = lockerAddress;
				lockerInfor.dateStart = newdateStart;
				lockerInfor.dateEnd = newdateEnd;
				if (!string.IsNullOrEmpty(revision_number))
				{
					lockerInfor.revision_number = revision_number;
				}
				lockerInfor.dateStartHour = newdateStart.ToString("HH");
				lockerInfor.dateStartMin = newdateStart.ToString("mm");
				lockerInfor.dateStartSec = newdateStart.ToString("ss");
				lockerInfor.dateEndHour = newdateEnd.ToString("HH");
				lockerInfor.dateEndMin = newdateEnd.ToString("mm");
				lockerInfor.dateEndSec = newdateEnd.ToString("ss");
				SmartLockerSDKLib.SmartLockerIssueCard(lockerInfor, out var writeMsg);
				string msg1;
				CardInfo info = SmartLockerSDKLib.SmartLockerCheckCard(out msg1);
				return writeMsg;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
