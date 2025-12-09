using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Sell;
using BusinessLayer.Repository;
using LSPos_Data.Data;
using LSPos_Data.DataAccess;
using LSPos_Data.Models;
using LSPosMVC.Common;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ticket;

namespace LSPosMVC.App_Start
{
    public static class BookingUtilities
    {

        public static async Task<bool> XyLuTaoBooking(List<DonHang_DichVuRequestAPIModel> models, SiteModel site, SellRepository sr, KhachHang kh)
        {
            ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
            DonHang_DichVuRequestAPIModel dichvu = models.Where(x => !string.IsNullOrEmpty(x.ProfileCode)).FirstOrDefault();
            if (dichvu != null)
            {
                new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, dichvu.ProfileID, dichvu.ProfileCode, dichvu.MemberID);
                LSPos_Data.Utilities.Log.Info("CreateBookingToLocalAPI");
                SaveBookingModel saveBooking = await CreateBookingToLocalAPI(models, sr, kh);
                if (saveBooking != null)
                {
                    //LSPos_Data.Utilities.Log.Info("ChiTietMatHang_DichVuModel Insert Start");
                    foreach (DonHang_DichVuRequestAPIModel model in models)
                    {
                        ChiTietMatHang_DichVuModel ct = new ChiTietMatHang_DichVuModel();
                        ct.ID = model.ID;
                        ct.BookingCode = saveBooking.BookingCode;
                        ct.InvoiceCode = saveBooking.InvoiceCode;
                        ct.SoLuong = model.SoLuong;
                        ctmhdao.InsertOrUpdate(ct);
                    }
                    //LSPos_Data.Utilities.Log.Info("ChiTietMatHang_DichVuModel Insert End");
                }
                return true;
            }
            return false;
        }
        public static async Task<bool> XuLyXuatVe(List<DonHang_DichVuRequestAPIModel> models, SiteModel site, int ID_DonHang, int ID_QuanLy, BookingRepository br, SellRepository sr)
        {
            ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
            DonHangData dhData = new DonHangData();
            DonHangv2 dh = dhData.GetDonHangTheoID_v2(ID_DonHang, ID_QuanLy);
            DonHang_DichVuRequestAPIModel dichvudacbiet = models.Where(x => !string.IsNullOrEmpty(x.ProfileCode)).FirstOrDefault();
            if (dichvudacbiet != null)
            {
                new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, dichvudacbiet.ProfileID, dichvudacbiet.ProfileCode, dichvudacbiet.MemberID);
            }
            SaveBookingModel saveBooking = await ConfirmBookingToLocalAPI(models, sr, dh);
            LSPos_Data.Utilities.Log.Info("Booking: " + JsonConvert.SerializeObject(saveBooking));
            if (saveBooking != null)
            {
                //LSPos_Data.Utilities.Log.Info("ChiTietMatHang_DichVuModel Insert Start " + ID_DonHang);
                foreach (DonHang_DichVuRequestAPIModel dhdv in models)
                {
                    ChiTietMatHang_DichVuModel ct = new ChiTietMatHang_DichVuModel();
                    ct.ID = dhdv.ID;
                    ct.BookingCode = saveBooking.BookingCode;
                    ct.InvoiceCode = saveBooking.InvoiceCode;
                    ctmhdao.InsertOrUpdate(ct);

                    List<ChiTietMatHangDonHangModels> lstct = new List<ChiTietMatHangDonHangModels>();
                    //BookingRes bk = br.Detail(saveBooking.BookingCode);
                    //saveBooking.Accounts = bk.Account;
                    foreach (BookingAccountResponse account in saveBooking.Accounts.Where(x => x.ServiceRateID.ToUpper() == dhdv.DichVu.MaDichVu.ToUpper()).ToList())
                    {
                        try
                        {
                            ChiTietMatHangDonHangModels m = new ChiTietMatHangDonHangModels();
                            m.ID_MatHang = dhdv.ID_MatHang;
                            m.ID_DonHang = ID_DonHang;
                            m.MaVeDichVu = account.AccountCode;
                            m.MaBookingDichVu = saveBooking.BookingCode;
                            m.MaDonHangDichVu = saveBooking.InvoiceCode;
                            m.HanSuDung = account.ExpirationDate;
                            m.ID_DichVu = dhdv.ID_DichVu;
                            lstct.Add(m);

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //LSPos_Data.Utilities.Log.Info("SL Ve:" + lstct.Count);
                    dhData.TaoDSChiTietMatHangDonHang(dhdv.ID_ChiTietDonHang, ID_DonHang, lstct.Where(x => x.ID_MatHang == dhdv.ID_MatHang).ToList());
                }
                //LSPos_Data.Utilities.Log.Info("ChiTietMatHang_DichVuModel Insert End " + ID_DonHang);
                // Send email
                try
                {
                    dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                    //dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(DHThemOBJ.iddonhang);
                    EmailHelper helper = new EmailHelper();
                    string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoiceExportTicket.html");
                    string bodyTemplate = System.IO.File.ReadAllText(path);
                    var html = Engine.Razor.RunCompile(bodyTemplate, "InvoiceExportTicket", dh.GetType(), dh);
                    helper.SendEmail(html.ToString(), dh.Email, null, "[TB] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " XUẤT VÉ - " + site.SiteName);
                    //if (!string.IsNullOrEmpty(dh.EmailHDV))
                        //helper.SendEmail("Bạn có đơn hàng cần hỗ trợ: OTA-Booking số " + dh.MaThamChieu, dh.EmailHDV, null, "[THÔNG BÁO] YÊU CẦU HỖ TRƠ KHÁCH HÀNG " + dh.MaThamChieu + " - " + site.SiteName);
                }
                catch (Exception ex)
                {

                }
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool ResendEmail(SiteModel site, int ID_DonHang, int ID_QuanLy)
        {
            DonHangData dhData = new DonHangData();
            DonHangv2 dh = dhData.GetDonHangTheoID_v2(ID_DonHang, ID_QuanLy);
            // Send email
            try
            {
                dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                EmailHelper helper = new EmailHelper();
                string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoiceExportTicket.html");
                string bodyTemplate = System.IO.File.ReadAllText(path);
                var html = Engine.Razor.RunCompile(bodyTemplate, "InvoiceExportTicket", dh.GetType(), dh);
                helper.SendEmail(html.ToString(), dh.Email, null, "[TB] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " XUẤT VÉ - " + site.SiteName);
                //if (!string.IsNullOrEmpty(dh.EmailHDV))
                    //helper.SendEmail("Bạn có đơn hàng cần hỗ trợ: OTA-Booking số " + dh.MaThamChieu, dh.EmailHDV, null, "[THÔNG BÁO] YÊU CẦU HỖ TRƠ KHÁCH HÀNG " + dh.MaThamChieu + " - " + site.SiteName);
            }
            catch (Exception ex)
            {

            }
            return true;
        }



        public static async Task<bool> XuLyXuatVePOS(List<DonHang_DichVuRequestAPIModel> models, SiteModel site, int ID_DonHang, int ID_QuanLy, BookingRepository br, SellRepository sr)
        {
            ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
            DonHangData dhData = new DonHangData();
            DonHangv2 dh = dhData.GetDonHangTheoID_v2(ID_DonHang, ID_QuanLy);
            DonHang_DichVuRequestAPIModel dichvudacbiet = models.Where(x => !string.IsNullOrEmpty(x.ProfileCode)).FirstOrDefault();
            if (dichvudacbiet != null)
            {
                new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, dichvudacbiet.ProfileID, dichvudacbiet.ProfileCode, dichvudacbiet.MemberID);
            }
            SaveBookingModel saveBooking = await ConfirmBookingToLocalAPI(models, sr, dh);
            LSPos_Data.Utilities.Log.Info("Booking: " + JsonConvert.SerializeObject(saveBooking));
            if (saveBooking != null)
            {
                //LSPos_Data.Utilities.Log.Info("ChiTietMatHang_DichVuModel Insert Start " + ID_DonHang);
                foreach (DonHang_DichVuRequestAPIModel dhdv in models)
                {
                    ChiTietMatHang_DichVuModel ct = new ChiTietMatHang_DichVuModel();
                    ct.ID = dhdv.ID;
                    ct.BookingCode = saveBooking.BookingCode;
                    ct.InvoiceCode = saveBooking.InvoiceCode;
                    ctmhdao.InsertOrUpdate(ct);

                    List<ChiTietMatHangDonHangModels> lstct = new List<ChiTietMatHangDonHangModels>();
                    if (saveBooking.Accounts.Count == 0)
                    {
                        BookingRes bk = br.Detail(saveBooking.BookingCode);
                        saveBooking.Accounts = bk.Account;
                    }
                    foreach (BookingAccountResponse account in saveBooking.Accounts.Where(x => x.ServiceRateID.ToUpper() == dhdv.DichVu.MaDichVu.ToUpper()).ToList())
                    {
                        try
                        {
                            ChiTietMatHangDonHangModels m = new ChiTietMatHangDonHangModels();
                            m.ID_MatHang = dhdv.ID_MatHang;
                            m.ID_DonHang = ID_DonHang;
                            m.MaVeDichVu = account.AccountCode;
                            m.MaBookingDichVu = saveBooking.BookingCode;
                            m.MaDonHangDichVu = saveBooking.InvoiceCode;
                            m.HanSuDung = account.ExpirationDate;
                            m.ID_DichVu = dhdv.ID_DichVu;
                            lstct.Add(m);

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    dhData.TaoDSChiTietMatHangDonHang(dhdv.ID_ChiTietDonHang, ID_DonHang, lstct.Where(x => x.ID_MatHang == dhdv.ID_MatHang).ToList());
                }
                //LSPos_Data.Utilities.Log.Info("ChiTietMatHang_DichVuModel Insert End " + ID_DonHang);
                return true;
            }
            else
            {
                return false;

            }
        }

        public static async Task<SaveBookingModel> CreateBookingToLocalAPI(List<DonHang_DichVuRequestAPIModel> serviceRateModel, SellRepository sr, KhachHang kh)
        {
            ProfileSelectedModel profile = new ProfileSelectedModel
            {
                Name = Global._Profile.Name,
                ProfileId = new Guid(Global._Profile.ProfileId),
                Address = Global._Profile.Address,
                PhoneNumber = Global._Profile.Phone,
                Email = Global._Profile.Email,
                AlternativeEmail = Global._Profile.AlternativeEmail,
                IdentityCard = Global._Profile.IdentityCard,
                MemberId = Global._Profile.MemberId,
                BookingCode = Global._Profile.ProfileCode
            };
            CartB2BModel cart = new CartB2BModel();
            cart.OrderCode = "1";
            cart.CheckInDate = serviceRateModel.First().Ngay;
            cart.CartTime = DateTime.Now.ToShortTimeString();
            cart.MemberId = profile.MemberId;
            cart.profile = profile;
            cart.Note = "B2B_API";
            cart.customer.Add(new CustomerB2BSelectedModel
            {
                CustomerID = Guid.NewGuid(),
                Address = !string.IsNullOrWhiteSpace(kh.DiaChiXuatHoaDon) ? kh.DiaChiXuatHoaDon : cart.profile.Address,
                CustomerType = "TA_CUSTOMER",
                Email = !string.IsNullOrWhiteSpace(kh.Email) ? kh.Email : cart.profile.IdentityCard,
                IdOrPPNum = !string.IsNullOrWhiteSpace(kh.MaSoThue) ? kh.MaSoThue : cart.profile.IdentityCard,
                Name = !string.IsNullOrWhiteSpace(kh.NguoiLienHe) ? kh.NguoiLienHe : cart.profile.Name,
                PhoneNumber = cart.profile.PhoneNumber
            });

            cart.EmailTo = cart.profile.Email;

            //Dich vu trong gio hang
            ServiceSelectedModel item = new ServiceSelectedModel();
            foreach (DonHang_DichVuRequestAPIModel serviceRate in serviceRateModel)
            {

                item = new ServiceSelectedModel
                {
                    Amount = Convert.ToInt64(serviceRate.GiaBan),
                    Quantity = (int)serviceRate.SoLuong,
                    Title = serviceRate.DichVu.TenDichVu,
                    ServiceRateID = serviceRate.DichVu.MaDichVu,
                    SellPrice = serviceRate.GiaBan
                };
                cart.listServiceSelected.Add(item);

            }
            SaveBookingModel saveBooking;
            //LSPos_Data.Utilities.Log.Info("SaveBookingB2B - start");
            if (Global.SiteID != "BDNewway")
            {
                saveBooking = sr.SaveBookingB2B(cart, 0L);
            }
            else
            {
                saveBooking = new SaveBookingModel();
                saveBooking.BookingCode = "";
                saveBooking.InvoiceCode = "";
            }
            //LSPos_Data.Utilities.Log.Info("SaveBookingB2B - end");

            return saveBooking;
        }

        public static async Task<SaveBookingModel> ConfirmBookingToLocalAPI(List<DonHang_DichVuRequestAPIModel> serviceRateModel, SellRepository sr, DonHangv2 dh)
        {
            //_sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, DateTime.Now);
            ProfileSelectedModel profile = new ProfileSelectedModel
            {
                Name = Global._Profile.Name,
                ProfileId = new Guid(Global._Profile.ProfileId),
                Address = Global._Profile.Address,
                PhoneNumber = Global._Profile.Phone,
                Email = Global._Profile.Email,
                AlternativeEmail = Global._Profile.AlternativeEmail,
                IdentityCard = Global._Profile.IdentityCard,
                MemberId = Global._Profile.MemberId,
                BookingCode = Global._Profile.ProfileCode
            };
            CartB2BModel cart = new CartB2BModel();
            cart.BookingCode = serviceRateModel.FirstOrDefault().BookingCode;
            cart.IsBookingOnline = true;
            cart.OrderCode = "1";
            cart.CheckInDate = serviceRateModel.First().Ngay;
            cart.CartTime = DateTime.Now.ToShortTimeString();
            cart.MemberId = profile.MemberId;
            cart.profile = profile;
            cart.Note = "B2B_API";
            //cart.customer.Add(new CustomerB2BSelectedModel
            //{
            //    CustomerID = Guid.NewGuid(),
            //    Address = "",//cart.profile.Address,
            //    CustomerType = "TA_CUSTOMER",
            //    Email = "",//cart.profile.Email,
            //    IdOrPPNum = "",//cart.profile.IdentityCard,
            //    Name = "Khach le",//cart.profile.Name,
            //    PhoneNumber = ""//cart.profile.PhoneNumber
            //});
            cart.EmailTo = cart.profile.Email;

            //Dich vu trong gio hang
            ServiceSelectedModel item = new ServiceSelectedModel();
            NWRequestBooking model = new NWRequestBooking();
            model.listticket = new List<Service>();
            //ServiceRateModel servicePackageRate = _sellRepository.GetListServicePackageRate().FirstOrDefault((ServiceRateModel m) => m.ServiceRateID == ServiceRateID);
            foreach (DonHang_DichVuRequestAPIModel serviceRate in serviceRateModel)
            {

                item = new ServiceSelectedModel
                {
                    Amount = Convert.ToInt64(serviceRate.ThanhTien),
                    Quantity = (int)serviceRate.SoLuong,
                    Title = serviceRate.DichVu.TenDichVu,
                    ServiceRateID = serviceRate.DichVu.MaDichVu,
                    SellPrice = (decimal)serviceRate.GiaBan
                };
                cart.listServiceSelected.Add(item);

                //NW
                Service service = new Service();
                service.ticket_code = serviceRate.DichVu.MaDichVu;
                service.quantity = serviceRate.SoLuong;
                service.num_people = serviceRate.SoLuong;
                service.price = (float)serviceRate.GiaBan;
                model.listticket.Add(service);
            }

            //Phuong thuc thanh toan
            cart.listPaymentType.Add(new PaymentTypeModel
            {
                Amount = cart.listServiceSelected.Sum(x => x.Amount),
                PaymentTypeID = serviceRateModel.FirstOrDefault().PaymentTypeID, /*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                PaymentTypeName = serviceRateModel.FirstOrDefault().PaymentTypeName,/*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                IsNewPayment = true,
                IsPaymentDeposit = false,
                BookingPaymentID = Guid.NewGuid()
            });




            model.invoice_code = dh.MaThamChieu;
            model.create_date = DateTime.Now.ToString("dd/MM/yyyy");
            model.in_date = DateTime.Now.ToString("dd/MM/yyyy");
            //model.in_date = serviceRateModel.First().Ngay.ToString("dd/MM/yyyy");
            model.booker = dh.TenKhachHang;
            model.phone_booker = dh.DienThoai;
            model.email_booker = dh.Email;
            model.site_code = "LS";

            if (Global.SiteID == "BDNewway")
            {
                NWUtil nWUtil = new NWUtil();
                Metadata_addticket meta = nWUtil.addTicket(Global._Profile.ProfileCode, model);
                LSPos_API.Utils.Log.Info("ID_ChiTietDonHang: " + serviceRateModel.FirstOrDefault().ID_ChiTietDonHang);
                SaveBookingModel saveBookingf = new SaveBookingModel();
                saveBookingf.BookingCode = meta.invoicecode;
                saveBookingf.InvoiceCode = meta.invoicecode;
                saveBookingf.Accounts = new List<BookingAccountResponse>();
                foreach (TicketAccount a in meta.listticket.Values)
                {
                    foreach (Ticket t in a.listqrcode.Values)
                    {
                        BookingAccountResponse accountResponse = new BookingAccountResponse();
                        accountResponse.ServiceRateID = a.ticket_code;
                        accountResponse.AccountCode = t.qrcode;
                        saveBookingf.Accounts.Add(accountResponse);
                    }
                }
                return saveBookingf;
            }
            else
            {
                LSPos_API.Utils.Log.Info("ID_ChiTietDonHang: " + serviceRateModel.FirstOrDefault().ID_ChiTietDonHang);
                SaveBookingModel saveBooking = sr.SaveBookingB2BExportTicket(cart, 0L);
                return saveBooking;
            }
        }

        public static async Task<SaveBookingModel> ConfirmFOCBookingRequestToLocalAPI(List<DonHang_DichVuRequestAPIModel> serviceRateModel, SellRepository sr)
        {
            ProfileSelectedModel profile = new ProfileSelectedModel
            {
                Name = Global._Profile.Name,
                ProfileId = new Guid(Global._Profile.ProfileId),
                Address = Global._Profile.Address,
                PhoneNumber = Global._Profile.Phone,
                Email = Global._Profile.Email,
                AlternativeEmail = Global._Profile.AlternativeEmail,
                IdentityCard = Global._Profile.IdentityCard,
                MemberId = Global._Profile.MemberId,
                BookingCode = Global._Profile.ProfileCode
            };
            CartB2BModel cart = new CartB2BModel();
            cart.BookingCode = serviceRateModel.FirstOrDefault().BookingCode;
            cart.IsBookingOnline = true;
            cart.OrderCode = "1";
            cart.CheckInDate = serviceRateModel.First().Ngay;
            cart.CartTime = DateTime.Now.ToShortTimeString();
            cart.MemberId = profile.MemberId;
            cart.profile = profile;
            cart.Note = "B2B_API";
            cart.EmailTo = cart.profile.Email;

            //Dich vu trong gio hang
            ServiceSelectedModel item = new ServiceSelectedModel();
            //ServiceRateModel servicePackageRate = _sellRepository.GetListServicePackageRate().FirstOrDefault((ServiceRateModel m) => m.ServiceRateID == ServiceRateID);
            foreach (DonHang_DichVuRequestAPIModel serviceRate in serviceRateModel)
            {

                item = new ServiceSelectedModel
                {
                    Amount = Convert.ToInt64(serviceRate.ThanhTien),
                    Quantity = (int)serviceRate.SoLuong,
                    Title = serviceRate.DichVu.TenDichVu,
                    ServiceRateID = serviceRate.DichVu.MaDichVu,
                    SellPrice = (decimal)serviceRate.GiaBan
                };
                cart.listServiceSelected.Add(item);


            }

            //Phuong thuc thanh toan
            cart.listPaymentType.Add(new PaymentTypeModel
            {
                Amount = cart.listServiceSelected.Sum(x => x.Amount),
                PaymentTypeID = serviceRateModel.FirstOrDefault().PaymentTypeID, /*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                PaymentTypeName = serviceRateModel.FirstOrDefault().PaymentTypeName,/*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                IsNewPayment = true,
                IsPaymentDeposit = false,
                BookingPaymentID = Guid.NewGuid()
            });

            SaveBookingModel saveBooking = sr.SaveBookingB2BExportTicket(cart, 0L);
            return saveBooking;
        }
    }
}
