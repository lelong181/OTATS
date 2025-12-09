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
	public class SellRepository
	{
		private List<ServiceRateGroup> listServiceRateGroup = new List<ServiceRateGroup>();

		private List<ServiceRateModel> listServiceRates = new List<ServiceRateModel>();

		private List<PaymentTypeModel> listPaymentType = new List<PaymentTypeModel>();

		private List<ServiceTicket> listServiceTicket = new List<ServiceTicket>();

		private const string langCode = "vi";

		public void ClearData()
		{
			listServiceRateGroup.Clear();
			listServiceRates.Clear();
			listPaymentType.Clear();
			listServiceTicket.Clear();
		}

		public List<ServiceRateGroup> GetListRateServiceGroup()
		{
			return listServiceRateGroup;
		}

		public List<ServiceTicket> GetListServiceTicket()
		{
			return listServiceTicket;
		}

		public List<ServiceRateModel> GetListServicePackageRate()
		{
			return listServiceRates;
		}

		public List<PaymentTypeModel> GetListPaymentType()
		{
			return listPaymentType;
		}

		public SaveBookingModel SaveBooking(CartModel cart, long redundancyMoney)
		{
			if (cart != null)
			{
				BookingReq br = new BookingReq
				{
					Checkin = DateTime.Now.ToString("yyyy-MM-dd"),
					SiteId = Global.SiteID,
					SiteCode = Global.SiteCode,
					Note = cart.Note,
					Channel = "POS",
					SessionId = Global.SessionID,
					CreatedBy = Global.UserName,
					ProfileId = Global.ProfileID,
					ComputerId = Global.ComputerID,
					ZoneId = Global.ZoneID
				};
                foreach (ServiceSelectedModel item in cart.listServiceSelected)
				{
					if (item.Quantity <= 0)
					{
						continue;
					}
					if (item.ListSellCard != null)
					{
						foreach (SellCardInput sellCard in item.ListSellCard)
						{
							if (!string.IsNullOrEmpty(sellCard.CardID))
							{
								br.listSellCard.Add(sellCard);
							}
						}
					}
					BookingDetailReq sr = new BookingDetailReq
					{
						Checkin = DateTime.Now.ToString("yyyy-MM-dd"),
						Id = item.ServiceRateID,
						Quantity = item.Quantity,
						PromotionID = item.PromotionID,
						Discount = item.Discount,
						PromotionLinkID = item.PromotionLinkID
					};
					br.Details.Add(sr);
				}
				long paidAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					paidAmount += payment.Amount;
					br.PaymentTypes.Add(new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount - redundancyMoney,
						Voucher = payment.voucher
					});
				}
				br.PaidAmount = paidAmount;
				br.ReturnAmount = redundancyMoney;
				if (cart.customer != null && cart.customer.CustomerID != Guid.Empty)
				{
					br.BookingCustomers.Add(new CustomerBookingReq
					{
						Id = cart.customer.CustomerID.ToString(),
						Name = cart.customer.Name,
						Address = cart.customer.Address,
						PhoneNumber = cart.customer.PhoneNumber,
						Email = cart.customer.Email
					});
				}
				BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/pos", Method.POST, new
				{
					Value = br
				});
				if (data != null)
				{
					return new SaveBookingModel
					{
						Success = true,
						InvoiceCode = data.InvoiceCode,
						BookingCode = data.BookingCode,
						Accounts = data.BookingAccounts
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel SaveBookingSeat(CartSeatModel cart, long redundancyMoney)
		{
			if (cart != null)
			{
				BookingReq br = new BookingReq
				{
					Checkin = DateTime.Now.ToString("yyyy-MM-dd"),
					SiteId = Global.SiteID,
					SiteCode = Global.SiteCode,
					Note = cart.Note,
					Channel = "POS",
					SessionId = Global.SessionID,
					CreatedBy = Global.UserName,
					ProfileId = Global.ProfileID,
					ComputerId = Global.ComputerID,
					ZoneId = Global.ZoneID,
					ShiftID = cart.ShiftID
				};
				foreach (ServiceSelectedModel item in cart.listServiceSelected)
				{
					if (item.Quantity <= 0)
					{
						continue;
					}
					if (item.ListSellCard != null)
					{
						foreach (SellCardInput sellCard in item.ListSellCard)
						{
							if (!string.IsNullOrEmpty(sellCard.CardID))
							{
								br.listSellCard.Add(sellCard);
							}
						}
					}
					BookingDetailReq dt = new BookingDetailReq
					{
						Checkin = DateTime.Now.ToString("yyyy-MM-dd"),
						Id = item.ServiceRateID,
						Quantity = item.Quantity,
						PromotionID = item.PromotionID,
						Discount = item.Discount,
						PromotionLinkID = item.PromotionLinkID
					};
					if (item._ShiftID.HasValue)
					{
						dt.ShiftID = item._ShiftID;
					}
					if (item._ZoneID.HasValue)
					{
						dt._ZoneID = item._ZoneID;
					}
					if (item.listSeatLock != null && item.listSeatLock.Count > 0)
					{
						dt.listSeatLock.Clear();
						foreach (Ticket.LockSeat ss in item.listSeatLock)
						{
							dt.listSeatLock.Add(new BusinessLayer.Model.LockSeat
							{
								hasSetSeat = false,
								SeatID = ss.SeatID,
								ShiftSeatID = ss.ShiftSeatID
							});
						}
					}
					br.Details.Add(dt);
				}
				long paidAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					paidAmount += payment.Amount;
					br.PaymentTypes.Add(new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount - redundancyMoney,
						Voucher = payment.voucher
					});
				}
				br.PaidAmount = paidAmount;
				br.ReturnAmount = redundancyMoney;
				if (cart.customer != null && cart.customer.CustomerID != Guid.Empty)
				{
					br.BookingCustomers.Add(new CustomerBookingReq
					{
						Id = cart.customer.CustomerID.ToString(),
						Name = cart.customer.Name,
						Address = cart.customer.Address,
						PhoneNumber = cart.customer.PhoneNumber,
						Email = cart.customer.Email
					});
				}
				BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/pos-seat", Method.POST, new
				{
					Value = br
				});
				if (data != null)
				{
					return new SaveBookingModel
					{
						Success = true,
						InvoiceCode = data.InvoiceCode,
						BookingCode = data.BookingCode,
						Accounts = data.BookingAccounts
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel SaveBookingB2B(CartB2BModel cart, long redundancyMoney)
		{
			if (cart != null && cart.profile != null && cart.profile.ProfileId != Guid.Empty)
			{
				DateTime? checkinDate = cart.CheckInDate;
				if (!checkinDate.HasValue || checkinDate <= DateTime.MinValue)
				{
					checkinDate = DateTime.Now;
				}
				BookingReq br = new BookingReq
				{
					Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
					SiteId = Global.SiteID,
					MemberId = cart.MemberId,
					SiteCode = Global.SiteCode,
					Note = cart.Note,
					Channel = "B2B",
					SessionId = Global.SessionID,
					CreatedBy = Global.UserName,
					ProfileId = cart.profile.ProfileId.ToString(),
					ComputerId = Global.ComputerID,
					ZoneId = Global.ZoneID,
					BookingCode = cart.BookingCode,
					OrderCode = cart.OrderCode,
					EmailTo = cart.EmailTo,
					EmailCC = cart.EmailCC,
					EmailBCC = cart.EmailBCC
				};
				foreach (ServiceSelectedModel item in cart.listServiceSelected)
				{
					if (item.Quantity > 0)
					{
						br.Details.Add(new BookingDetailReq
						{
							Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
							Id = item.ServiceRateID,
							Quantity = item.Quantity,
							PromotionID = item.PromotionID,
							PromotionLinkID = item.PromotionLinkID,
							Discount = item.Discount
						});
					}
				}
				long paidAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					paidAmount += payment.Amount;
					br.PaymentTypes.Add(new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount,
						BookingPaymentID = payment.BookingPaymentID
					});
				}
				br.PaidAmount = paidAmount;
				br.ReturnAmount = redundancyMoney;
				br.Customers = cart.customer;
				//LSPos_API.Utils.Log.Info("booking-b2b start");
                BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/booking-b2b", Method.POST, new
				{
					Value = br
				});
                //LSPos_API.Utils.Log.Info("booking-b2b end");
                if (data != null)
				{
					return new SaveBookingModel
					{
						IsPaymentFull = data.IsPaymentFull,
						Success = true,
						BookingCode = data.BookingCode,
						Accounts = data.BookingAccounts
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel SaveBookingB2BNonExportTicket(CartB2BModel cart, long redundancyMoney, bool isAutoCreateWallet = false)
		{
			if (cart != null && cart.profile != null && cart.profile.ProfileId != Guid.Empty)
			{
				DateTime? checkinDate = cart.CheckInDate;
				if (!checkinDate.HasValue || checkinDate <= DateTime.MinValue)
				{
					checkinDate = DateTime.Now;
				}
				BookingReq br = new BookingReq
				{
					Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
					SiteId = Global.SiteID,
					SiteCode = Global.SiteCode,
					MemberId = cart.MemberId,
					Note = cart.Note,
					Channel = "B2B",
					SessionId = Global.SessionID,
					CreatedBy = Global.UserName,
					ProfileId = cart.profile.ProfileId.ToString(),
					ComputerId = Global.ComputerID,
					ZoneId = Global.ZoneID,
					BookingCode = cart.BookingCode,
					IsExportTicket = true,
					OrderCode = cart.OrderCode,
					EmailTo = cart.EmailTo,
					EmailCC = cart.EmailCC,
					EmailBCC = cart.EmailBCC
				};
				foreach (ServiceSelectedModel item in cart.listServiceSelected)
				{
					if (item.Quantity <= 0)
					{
						continue;
					}
					if (item.ListSellCard != null)
					{
						foreach (SellCardInput sellCard in item.ListSellCard)
						{
							if (!string.IsNullOrEmpty(sellCard.CardID))
							{
								sellCard.AutoCreateWallet = isAutoCreateWallet;
								br.listSellCard.Add(sellCard);
							}
						}
					}
					br.Details.Add(new BookingDetailReq
					{
						Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
						Id = item.ServiceRateID,
						Quantity = item.Quantity,
						PromotionID = item.PromotionID,
						PromotionLinkID = item.PromotionLinkID,
						Discount = item.Discount
					});
				}
				long paidAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					paidAmount += payment.Amount;
					PaymentTypeRequest tempPayment = new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount,
						Voucher = payment.voucher,
						BookingPaymentID = payment.BookingPaymentID,
						IsNewPayment = payment.IsNewPayment,
						IsPaymentDeposit = payment.IsPaymentDeposit
					};
					br.PaymentTypes.Add(tempPayment);
				}
				br.PaidAmount = paidAmount;
				br.ReturnAmount = redundancyMoney;
				br.Customers = cart.customer;
				BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/booking-b2b-non-export-ticket", Method.POST, new
				{
					Value = br
				});
				if (data != null)
				{
					return new SaveBookingModel
					{
						IsPaymentFull = data.IsPaymentFull,
						Success = true,
						InvoiceCode = data.InvoiceCode,
						BookingCode = data.BookingCode,
						Accounts = data.BookingAccounts
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel SaveBookingB2BExportTicket(CartB2BModel cart, long redundancyMoney, bool isAutoCreateWallet = false)
		{
			if (cart != null && cart.profile != null && cart.profile.ProfileId != Guid.Empty)
			{
				DateTime? checkinDate = cart.CheckInDate;
				if (!checkinDate.HasValue || checkinDate <= DateTime.MinValue)
				{
					checkinDate = DateTime.Now;
				}
				BookingReq br = new BookingReq
				{
					Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
					SiteId = Global.SiteID,
					MemberId = cart.MemberId,
					SiteCode = Global.SiteCode,
					Note = cart.Note,
					Channel = "B2B",
					SessionId = Global.SessionID,
					CreatedBy = Global.UserName,
					ProfileId = cart.profile.ProfileId.ToString(),
					ComputerId = Global.ComputerID,
					ZoneId = Global.ZoneID,
					BookingCode = cart.BookingCode,
					IsExportTicket = true,
					OrderCode = cart.OrderCode,
					EmailTo = cart.EmailTo,
					EmailCC = cart.EmailCC,
					EmailBCC = cart.EmailBCC
				};
				foreach (ServiceSelectedModel item in cart.listServiceSelected)
				{
					if (item.Quantity <= 0)
					{
						continue;
					}
					if (item.ListSellCard != null)
					{
						foreach (SellCardInput sellCard in item.ListSellCard)
						{
							if (!string.IsNullOrEmpty(sellCard.CardID))
							{
								sellCard.AutoCreateWallet = isAutoCreateWallet;
								br.listSellCard.Add(sellCard);
							}
						}
					}
					br.Details.Add(new BookingDetailReq
					{
						Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
						Id = item.ServiceRateID,
						Quantity = item.Quantity,
						PromotionID = item.PromotionID,
						PromotionLinkID = item.PromotionLinkID,
						Discount = item.Discount
					});
				}
				long paidAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					paidAmount += payment.Amount;
					br.PaymentTypes.Add(new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount,
						Voucher = payment.voucher,
						IsPaymentDeposit = payment.IsPaymentDeposit,
						IsNewPayment = payment.IsNewPayment,
						BookingPaymentID = payment.BookingPaymentID
					});
				}
				br.PaidAmount = paidAmount;
				br.ReturnAmount = redundancyMoney;
				br.Customers = cart.customer;
                //LSPos_API.Utils.Log.Info("booking-b2b-export-ticket start");
                BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/booking-b2b-export-ticket", Method.POST, new
				{
					Value = br
				});
				LSPos_API.Utils.Log.Info("BookingCode: " + data.BookingCode);
				//LSPos_API.Utils.Log.Info("booking-b2b-export-ticket end");
				if (data != null)
				{
					return new SaveBookingModel
					{
						IsPaymentFull = data.IsPaymentFull,
						Success = true,
						InvoiceCode = data.InvoiceCode,
						BookingCode = data.BookingCode,
						Accounts = data.BookingAccounts
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel SaveBooking(CartB2BModel cart, long redundancyMoney)
		{
			if (cart != null && cart.profile != null && cart.profile.ProfileId != Guid.Empty)
			{
				DateTime? checkinDate = cart.CheckInDate;
				if (!checkinDate.HasValue || checkinDate <= DateTime.MinValue)
				{
					checkinDate = DateTime.Now;
				}
				BookingReq br = new BookingReq
				{
					Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
					SiteId = Global.SiteID,
					SiteCode = Global.SiteCode,
					Note = cart.Note,
					Channel = "B2B",
					SessionId = Global.SessionID,
					CreatedBy = Global.UserName,
					ProfileId = cart.profile.ProfileId.ToString(),
					ComputerId = Global.ComputerID,
					ZoneId = Global.ZoneID,
					BookingCode = cart.BookingCode
				};
				foreach (ServiceSelectedModel item in cart.listServiceSelected)
				{
					if (item.Quantity > 0)
					{
						br.Details.Add(new BookingDetailReq
						{
							Checkin = checkinDate.Value.ToString("yyyy-MM-dd"),
							Id = item.ServiceRateID,
							Quantity = item.Quantity,
							PromotionID = item.PromotionID,
							PromotionLinkID = item.PromotionLinkID,
							Discount = item.Discount
						});
					}
				}
				long paidAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					paidAmount += payment.Amount;
					br.PaymentTypes.Add(new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount
					});
				}
				br.PaidAmount = paidAmount;
				br.ReturnAmount = redundancyMoney;
				br.Customers = cart.customer;
				BookingCreateRes data = ApiUtility.CallApiSimple<BookingCreateRes>("booking/b2b", Method.POST, new
				{
					Value = br
				});
				if (data != null)
				{
					return new SaveBookingModel
					{
						IsPaymentFull = data.IsPaymentFull,
						Success = true,
						InvoiceCode = data.BookingCode,
						Accounts = data.BookingAccounts
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel ConfirmBooking(CartModel cart, string siteID, string memberID, string sessionID, long redundancyMoney)
		{
			if (cart != null && cart.IsBookingOnline)
			{
				PosBookingConfirmReq dataConfirm = new PosBookingConfirmReq
				{
					BookingCode = cart.BookingCode,
					Note = cart.Note,
					SessionId = sessionID,
					SiteId = siteID,
					ReturnAmount = redundancyMoney
				};
				long prepayAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					prepayAmount += payment.Amount;
					dataConfirm.PaymentTypes.Add(new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount,
						IsNewPayment = payment.IsNewPayment
					});
				}
				dataConfirm.PrepayAmount = prepayAmount;
				BookingCreateRes confirmResponse = ApiUtility.CallApiSimple<BookingCreateRes>("booking/confirm-pos", Method.POST, new
				{
					Value = dataConfirm
				});
				if (confirmResponse != null)
				{
					return new SaveBookingModel
					{
						Success = true,
						InvoiceCode = confirmResponse.InvoiceCode,
						BookingCode = confirmResponse.BookingCode,
						Accounts = confirmResponse.BookingAccounts
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public SaveBookingModel ConfirmBooking(CartB2BModel cart, string siteID, string memberID, string sessionID, long redundancyMoney)
		{
			if (cart != null && cart.IsBookingOnline)
			{
				PosBookingConfirmReq dataConfirm = new PosBookingConfirmReq
				{
					BookingCode = cart.BookingCode,
					Note = cart.Note,
					SessionId = sessionID,
					SiteId = siteID,
					ReturnAmount = redundancyMoney
				};
				long prepayAmount = 0L;
				foreach (PaymentTypeModel payment in cart.listPaymentType)
				{
					prepayAmount += payment.Amount;
					dataConfirm.PaymentTypes.Add(new PaymentTypeRequest
					{
						Name = payment.PaymentTypeName,
						PaymentID = new Guid(payment.PaymentTypeID),
						Amount = payment.Amount,
						IsNewPayment = payment.IsNewPayment
					});
				}
				dataConfirm.PrepayAmount = prepayAmount;
				BookingCreateRes confirmResponse = ApiUtility.CallApiSimple<BookingCreateRes>("booking/confirm-pos", Method.POST, new
				{
					Value = dataConfirm
				});
				if (confirmResponse != null)
				{
					return new SaveBookingModel
					{
						Success = true,
						InvoiceCode = confirmResponse.BookingCode,
						Accounts = confirmResponse.BookingAccounts,
						IsPaymentFull = confirmResponse.IsPaymentFull
					};
				}
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public void GetSellData(string siteId, string memberID, int days = 0, bool getPaymentTypeOnly = false)
		{
			PosSellModel data = ApiUtility.CallApiSimple<PosSellModel>("service-rate/get-all-pos", Method.POST, new
			{
				value = new
				{
					siteId = siteId,
					memberId = memberID,
					checkIn = ((!getPaymentTypeOnly) ? DateTime.Now.AddDays(days).ToString("yyyy-MM-dd") : DateTime.MinValue.ToString("yyyy-MM-dd")),
					Channel = "B2B",
					ComputerID = Global.ComputerID
				}
			});
			if (data == null)
			{
				return;
			}
			listServiceTicket = data.ServiceTickets;
			listServiceRateGroup.Add(new ServiceRateGroup
			{
				ServiceRateGroupID = Guid.Empty,
				ServiceRateGroupName = "Tất cả"
			});
			if (data.ServiceRateGroups != null && data.ServiceRateGroups.Count > 0)
			{
				listServiceRateGroup.AddRange(data.ServiceRateGroups);
			}
			foreach (ServiceRateModel item in data.ServiceRates)
			{
				if (item.DailyQuantity.HasValue && (item.DailyQuantity.Value > 0 || item.DailyQuantity.Value == -1))
				{
					if (item.DailyQuantity.Value != -1)
					{
						item.QuantityStr = "SL: " + item.DailyQuantity.Value.ToString("n0");
					}
					else
					{
						item.QuantityStr = "SL: ++";
					}
					item.ColorDisplay = item.Color;
					listServiceRates.Add(item);
				}
			}
			listPaymentType = data.PaymentTypes;
		}

		public List<ServiceRateSeatRes> GetServiceRateSeat(ServiceRateSeatReq model)
		{
			return ApiUtility.CallApi<ServiceRateSeatRes>("service-rate/get-service-rate-seat", Method.POST, model);
		}

		public List<ZoneGroupAvailableRes> GetZoneGroupAvailable(Guid ShiftID)
		{
			string langCode = "vi";
			return ApiUtility.CallApi<ZoneGroupAvailableRes>(string.Concat("service-rate/get-zone-group-available?ShiftID=", ShiftID, "&langCode=", langCode, "&profileID=", Global.ProfileID), Method.GET);
		}

		public void GetSellB2BData(string siteId, string memberID, DateTime checkin)
		{
			PosSellModel data = ApiUtility.CallApiSimple<PosSellModel>("service-rate/get-all-pos", Method.POST, new
			{
				value = new
				{
					siteId = siteId,
					memberId = memberID,
					checkIn = checkin.ToString("yyyy-MM-dd"),
					Channel = "B2B"
				}
			});
			if (data == null)
			{
				return;
			}
			listServiceTicket = data.ServiceTickets;
			listServiceRateGroup.Add(new ServiceRateGroup
			{
				ServiceRateGroupID = Guid.Empty,
				ServiceRateGroupName = "Tất cả"
			});
			if (data.ServiceRateGroups != null && data.ServiceRateGroups.Count > 0)
			{
				listServiceRateGroup.AddRange(data.ServiceRateGroups);
			}
			foreach (ServiceRateModel item in data.ServiceRates)
			{
				if (item.DailyQuantity.HasValue && (item.DailyQuantity.Value > 0 || item.DailyQuantity.Value == -1))
				{
					if (item.DailyQuantity.Value != -1)
					{
						item.QuantityStr = "SL: " + item.DailyQuantity.Value.ToString("n0");
					}
					else
					{
						item.QuantityStr = "";
					}
					item.ZoneName = Global.SiteCode;
					listServiceRates.Add(item);
				}
			}
			listPaymentType = data.PaymentTypes;
		}

		public List<PromotionResponse> GetPromotionByProfile(GetDataByProfileRequest model)
		{
			return ApiUtility.CallApi<PromotionResponse>("promotion/get-data-by-profile", Method.POST, model);
		}

		public List<SeatAvailableRes> GetSeatByZone(SeatAvailableReq model)
		{
			model.Type = "SERVICERATE";
			model.LangCode = "vi";
			return ApiUtility.CallApi<SeatAvailableRes>("service-rate/get-seat-by-zone", Method.POST, model);
		}

		public SaveBookingModel SaveBookingUpgrade(List<ServiceRateUpgradeSelected> listServiceRateUpgrade, List<PaymentTypeModel> listPaymentTypeSelected, string note, ListServiceRateUpgradeRes data, bool isExportTicket = false)
		{
			if (data == null || listServiceRateUpgrade == null || listServiceRateUpgrade.Count == 0)
			{
				return null;
			}
			UpgradeServiceRateReq request = new UpgradeServiceRateReq
			{
				CheckInDate = DateTime.Now,
				SiteID = Global.SiteID,
				SiteCode = Global.SiteCode,
				Note = note,
				OriginInvoiceID = data.InvoiceID.Value,
				SessionID = Global.SessionID,
				Username = Global.UserName,
				ProfileID = data.ProfileID.Value,
				ComputerID = Global.ComputerID,
				ZoneID = Global.ZoneID,
				OriginBookingID = data.BookingID.Value,
				IsExportTicket = isExportTicket,
				OriginInvoiceCode = data.InvoiceCode
			};
			foreach (ServiceRateUpgradeSelected item in listServiceRateUpgrade)
			{
				if (!item.Price.HasValue)
				{
					continue;
				}
				UgradeServiceRateDetailReq detail = new UgradeServiceRateDetailReq
				{
					NewPrice = item.UpgradeServiceRatePrice,
					OldPrice = item.Price.Value,
					OriginServiceRateID = item.ServiceRateID,
					UpgradeServiceRateID = item.UpgradeServiceRateID,
					PromotionID = item.PromotionID
				};
				if (item.ListAccount != null && item.ListAccount.Count > 0)
				{
					foreach (AccountRepaySelected acc in item.ListAccount)
					{
						detail.ListAccount.Add(new UgradeAccountDetailReq
						{
							AccountCode = acc.AccountCode,
							AccountID = acc.AccountID,
							CardID = acc.CardID
						});
					}
				}
				if (item.ListSellCard != null && item.ListSellCard.Count() > 0)
				{
					request.listSellCard.AddRange(item.ListSellCard);
				}
				request.UpgradeDetails.Add(detail);
			}
			foreach (PaymentTypeModel payment in listPaymentTypeSelected)
			{
				request.PaymentTypes.Add(new PaymentTypeReq
				{
					Amount = payment.Amount,
					Name = payment.PaymentTypeName,
					PaymentID = payment.PaymentTypeID
				});
			}
			BookingCreateRes dataRes = ApiUtility.CallApiSimple<BookingCreateRes>("booking/upgrade-service-rate", Method.POST, request);
			if (dataRes != null)
			{
				return new SaveBookingModel
				{
					Success = true,
					InvoiceCode = dataRes.InvoiceCode,
					BookingCode = dataRes.BookingCode,
					Accounts = dataRes.BookingAccounts,
					InvoiceCodeRepay = dataRes.InvoiceCodeRepay
				};
			}
			return new SaveBookingModel
			{
				Success = false
			};
		}

		public ShiftSellRes GetSellShift(ShiftSellReq model)
		{
			model.LangCode = "vi";
			model.ByGroup = "DAY";
			return ApiUtility.CallApiSimple<ShiftSellRes>("service-rate/get-shift", Method.POST, new
			{
				value = model
			});
		}

		public BookingRes LockSeat(BookingLockSeatReq model)
		{
			return ApiUtility.CallApiSimple<BookingRes>("booking/lock-seat", Method.POST, new
			{
				value = model
			});
		}

		public void ClearLock(string bookingCode)
		{
			Res data = ApiUtility.CallApiSimple("booking/clear-lock-seat?bookingCode=" + bookingCode, Method.GET);
		}
	}
}
