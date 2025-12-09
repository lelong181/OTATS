using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Reports;
using BusinessLayer.Model.Sell;
using BusinessLayer.Repository;
using LSPos_API.Model.API;
using LSPos_API.Model.RequestModel;
using LSPos_Data.Data;
using LSPos_Data.DataAccess;
using LSPos_Data.Models;
using LSPosMVC.App_Start;
using LSPosMVC.Common;
using NPOI.OpenXmlFormats.Vml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Ticket;
using Ticket.Business;
using RazorEngine;
using RazorEngine.Templating;
using Model.ResponseModel;
using Microsoft.Ajax.Utilities;
using System.Xml.Linq;
using static LSPosMVC.Common.VnPayUtils;
using Ticket.Utils;
using Newtonsoft.Json;

namespace LSPosMVC.Controllers
{
    public class BookingController : Controller
    {
        private SellRepository _sellRepository;
        private BookingRepository _bookingRepository;
        private CommonRepository _commonRepository;

        public BookingController()
        {
            _sellRepository = new SellRepository();
            _bookingRepository = new BookingRepository();
            _commonRepository = new CommonRepository();
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetServicesByDate(DateTime date)
        {
            List<ServiceRateModel> listServicePackageRate = new List<ServiceRateModel>();
            //Trang An
            string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_TrangAn"];
            string ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            string ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "leanhtest", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, date);
            listServicePackageRate.AddRange(_sellRepository.GetListServicePackageRate());
            //Lscloud
            ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_LSCloud"];
            ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "pchl", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, date);
            listServicePackageRate.AddRange(_sellRepository.GetListServicePackageRate());
            return Json(listServicePackageRate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetListPaymentType(DateTime date)
        {
            List<PaymentTypeModel> ListPaymentType = new List<PaymentTypeModel>();
            //Trang An
            string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_TrangAn"];
            string ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            string ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "leanhtest", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, date);
            ListPaymentType.AddRange(_sellRepository.GetListPaymentType());
            //Lscloud
            ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_LSCloud"];
            ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "pchl", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, date);
            ListPaymentType.AddRange(_sellRepository.GetListPaymentType());
            return Json(ListPaymentType, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult SyncServicesFromLocalApi()
        {
            List<ServiceRateModel> listServicePackageRate = new List<ServiceRateModel>();
            //Trang An
            string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_TrangAn"];
            string ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            string ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "leanhtest", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, DateTime.Now.Date);
            listServicePackageRate.AddRange(_sellRepository.GetListServicePackageRate());
            //Lscloud
            ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_LSCloud"];
            ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "pchl", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, DateTime.Now.Date);
            listServicePackageRate.AddRange(_sellRepository.GetListServicePackageRate());

            return Json(listServicePackageRate, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ListServices()
        {
            return View();
        }

        [Authorize]
        public ActionResult Booking(string ServiceRateID)
        {
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, DateTime.Now);
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
            cart.CheckInDate = DateTime.Now;
            cart.CartTime = DateTime.Now.ToShortTimeString();
            cart.MemberId = profile.MemberId;
            cart.profile = profile;
            cart.Note = "B2B_API";
            cart.customer.Add(new CustomerB2BSelectedModel
            {
                CustomerID = Guid.NewGuid(),
                Address = cart.profile.Address,
                CustomerType = "TA_CUSTOMER",
                Email = cart.profile.Email,
                IdOrPPNum = cart.profile.IdentityCard,
                Name = cart.profile.Name,
                PhoneNumber = cart.profile.PhoneNumber
            });

            //Dich vu trong gio hang
            ServiceSelectedModel item = new ServiceSelectedModel();
            ServiceRateModel servicePackageRate = _sellRepository.GetListServicePackageRate().FirstOrDefault((ServiceRateModel m) => m.ServiceRateID == ServiceRateID);
            if (servicePackageRate != null)
            {
                item = new ServiceSelectedModel
                {
                    Amount = Convert.ToInt64(servicePackageRate.SellPrice),
                    Quantity = 1,
                    Title = servicePackageRate.Title,
                    ServiceRateID = servicePackageRate.ServiceRateID,
                    SellPrice = servicePackageRate.SellPrice
                };
                cart.listServiceSelected.Add(item);
            }

            //Phuong thuc thanh toan
            cart.listPaymentType.Add(new PaymentTypeModel
            {
                Amount = cart.listServiceSelected.Sum(x => x.Amount),
                //"b0465aba-7e5b-4726-a434-f2fb4366f9ca" chuyển khoản
                PaymentTypeID = "8b9cbccd-5094-46ab-a758-3f2fb3c2d893", /*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                PaymentTypeName = "Công nợ",/*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                IsNewPayment = true,
                IsPaymentDeposit = false,
                BookingPaymentID = Guid.NewGuid()
            }); ;

            SaveBookingModel saveBooking = _sellRepository.SaveBookingB2BExportTicket(cart, 0L);
            return Json(saveBooking, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ThongBaoBookingThanhCong()
        {
            RateAvailableReq m = new RateAvailableReq();
            m.CheckIn = "2023-03-09";
            m.ProfileId = "620A7319-28D7-4FA0-B8AB-19A98E5C7D6E";
            m.SiteId = "7D5F0AD1-EC01-4771-8969-C61F7CA694CF";
            Req<RateAvailableReq> d = new LSPos_API.Model.RequestModel.Req<RateAvailableReq>();
            d.Value = m;
            new GlobalUtilities().SetGlobal("http://dstabt.ddns.net:8889", "b2b_online", "123", "TRANGAN", "620A7319-28D7-4FA0-B8AB-19A98E5C7D6E", "2700276575");
            GetAllServiceRateResponse data = _commonRepository.B2B_GetAllServiceRate(d);
            return View();
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult PrintTicket(int idDonHang)
        {
            DonHangData dh = new DonHangData();
            List<ChiTietDonHangModels> dsctdh = dh.LayChiTietDonHang(idDonHang, "vi");
            return View(dsctdh);
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult PrintTicketByBookingCode(int ID_ChiTietDonHang, int ID_DonHang, int ID_MatHang, int ID_DichVu, string BookingCode, string SiteCode)
        {

            DonHangData dhdt = new DonHangData();
            DonHang_DichVuRequestAPIModel dhdv = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).Where(x => (x.BookingCode == BookingCode || BookingCode == "") && (x.ID_DichVu == ID_DichVu || ID_DichVu == 0)).FirstOrDefault();
            dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVu(ID_ChiTietDonHang, ID_DonHang, ID_MatHang, ID_DichVu, BookingCode, SiteCode, "");
            dhdv.lstTicket = dhdv.lstTicket.Distinct().OrderBy(x => x.TenHienThi).ToList();

            if (dhdv.lstTicket.Count == 0)
            {
                return Redirect("http://muave.disantrangan.vn/vi/print?booking_code=" + BookingCode);
            }
            return View(dhdv);
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult OnlineTicketByBookingCode(string BookingCode, string SiteCode, int? ID_MatHang, string GroupLink)
        {

            DonHangData dhdt = new DonHangData();
            List<ChiTietMatHangDonHangModels> lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVuV2(ID_MatHang, BookingCode, SiteCode, GroupLink);
            DonHang_DichVuRequestAPIModel dhdv = null;
            if (lstTicket.Count == 0)
            {
                return Redirect("http://muave.disantrangan.vn/vi/print?booking_code=" + BookingCode);
            }
            else
            {
                dhdv = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(lstTicket[0].ID_DonHang).Where(x => (x.BookingCode == BookingCode || BookingCode == "")).FirstOrDefault();
                dhdv.lstTicket = lstTicket.Distinct().OrderBy(x => x.TenHienThi).ToList();
            }
            return View(dhdv);
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult PrintTicketBySubBookingCode(int ID_DonHang, string GroupLink)
        {

            DonHangData dhdt = new DonHangData();
            DonHang_DichVuRequestAPIModel dhdv = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).FirstOrDefault();
            if (!string.IsNullOrEmpty(GroupLink))
            {
                dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVu(0, ID_DonHang, 0, 0, "", "", GroupLink);
                dhdv.lstTicket = dhdv.lstTicket.Distinct().OrderBy(x => x.TenHienThi).ToList();
            }
            if (dhdv.lstTicket.Count == 0)
            {
                return Redirect("https://tourshopping.vn/");
            }
            return View(dhdv);
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult PrintQRTicketByBookingCode(int ID_ChiTietDonHang, int ID_DonHang, int ID_MatHang, int ID_DichVu, string BookingCode, string SiteCode)
        {
            LSPos_Data.Utilities.Log.Info("Print: " + ID_DonHang + " - " + BookingCode + " - " + SiteCode);
            DonHangData dhdt = new DonHangData();
            DonHang_DichVuRequestAPIModel dhdv = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).Where(x => x.BookingCode == BookingCode && x.ID_DichVu == ID_DichVu).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(SiteCode))
            {
                dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVu(ID_ChiTietDonHang, ID_DonHang, ID_MatHang, ID_DichVu, BookingCode, SiteCode);
            }
            else
            {
                dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVuV2(0, BookingCode, "", "");
            }
            dhdv.lstTicket = dhdv.lstTicket.Distinct().OrderBy(x => x.TenHienThi).ToList();

            if (dhdv.lstTicket.Count == 0)
            {
                return Redirect("http://muave.disantrangan.vn/vi/print?booking_code=" + BookingCode);
            }
            return View(dhdv);
        }

        public class TamCoc_PrintObject
        {
            public List<ChiTietMatHangDonHangModels> lstTicket { get; set; }
            public string ThuNgan { get; set; }
            public DateTime Ngay { get; set; }
            public string MaThamChieu { get; set; }
            public List<ChiTietDonHangModels> lstChiTiet { get; set; }
            public List<LichSuThanhToanModel> lstThanhToan { get; set; }
            public object MaQRThanhToan { get; set; }
        }

        public ActionResult CheckThanhToan(int ID_DonHang)
        {
            DonHangData dhdt = new DonHangData();
            DonHangModels donhang = dhdt.LayDonHang(ID_DonHang);
            return Json(new { success = true, data = donhang }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintQRTicketByBookingCodeForTamCoc(int ID_DonHang, string BookingCode, string ThuNgan, string SiteCode)
        {
            LSPos_Data.Utilities.Log.Info("Print TamCoc : " + ID_DonHang + " - " + BookingCode + " - " + ThuNgan + " - " + SiteCode);
            DonHangData dhdt = new DonHangData();
            DonHangModels donhang = dhdt.LayDonHang(ID_DonHang);
            DonHang_DichVuRequestAPIModel dhdv = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).Where(x => string.IsNullOrWhiteSpace(BookingCode) || x.BookingCode == BookingCode).FirstOrDefault();
            //dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVu(0, ID_DonHang, 0, 0, string.IsNullOrWhiteSpace(BookingCode) ? "" : BookingCode, SiteCode);
            dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVuV2(0, BookingCode, "", "");
            dhdv.lstTicket = dhdv.lstTicket.DistinctBy(p => p.MaVeDichVu).OrderBy(x => x.TenHienThi).ToList();

            DonHangData dhData = new DonHangData();
            List<ChiTietDonHangModels> data = dhData.LayChiTietDonHangForPrint(ID_DonHang, "vi");
            List<LichSuThanhToanModel> lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(ID_DonHang);
            TamCoc_PrintObject tamCoc_PrintObject = new TamCoc_PrintObject();
            tamCoc_PrintObject.lstThanhToan = lichsuthanhtoan;
            tamCoc_PrintObject.lstTicket = dhdv.lstTicket;
            tamCoc_PrintObject.lstChiTiet = data;
            tamCoc_PrintObject.ThuNgan = ThuNgan;
            tamCoc_PrintObject.Ngay = dhdv.Ngay;
            tamCoc_PrintObject.MaThamChieu = donhang.mathamchieu;
            return View(tamCoc_PrintObject);
        }

        public ActionResult PrintQRTicketByBookingCodeForBaiDinh(int ID_DonHang, string BookingCode, string ThuNgan, string SiteCode)
        {
            LSPos_Data.Utilities.Log.Info("Print BaiDinh: " + ID_DonHang + " - " + BookingCode + " - " + ThuNgan + " - " + SiteCode);
            DonHangData dhdt = new DonHangData();
            DonHangModels donhang = dhdt.LayDonHang(ID_DonHang);
            DonHang_DichVuRequestAPIModel dhdv = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).Where(x => !string.IsNullOrWhiteSpace(x.BookingCode) && (string.IsNullOrWhiteSpace(BookingCode) || x.BookingCode == BookingCode)).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(SiteCode))
            {
                dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVu(0, ID_DonHang, 0, 0, string.IsNullOrWhiteSpace(BookingCode) ? "" : BookingCode, SiteCode);
            }
            else
            {
                dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVuV2(0, string.IsNullOrWhiteSpace(BookingCode) ? dhdv.BookingCode : BookingCode, "", "");
            }

            dhdv.lstTicket = dhdv.lstTicket.DistinctBy(p => p.MaVeDichVu).OrderBy(x => x.TenHienThi).ToList();

            DonHangData dhData = new DonHangData();
            List<ChiTietDonHangModels> data = dhData.LayChiTietDonHangForPrint(ID_DonHang, "vi");
            List<LichSuThanhToanModel> lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(ID_DonHang);
            TamCoc_PrintObject tamCoc_PrintObject = new TamCoc_PrintObject();
            tamCoc_PrintObject.lstThanhToan = lichsuthanhtoan;
            tamCoc_PrintObject.lstTicket = dhdv.lstTicket;
            tamCoc_PrintObject.lstChiTiet = data;
            tamCoc_PrintObject.ThuNgan = ThuNgan;
            tamCoc_PrintObject.Ngay = dhdv.Ngay;
            tamCoc_PrintObject.MaThamChieu = donhang.mathamchieu;
            return View(tamCoc_PrintObject);
        }



        public ActionResult VnpayWaitPayment(int ID_DonHang, string ThuNgan)
        {
            DonHangData dhdt = new DonHangData();
            DonHangModels donhang = dhdt.LayDonHang(ID_DonHang);
            VnPayOrder order = new VnPayUtils().CreateOrder(donhang);
            string resultQR = ApiUtility.CreateQRVnpay(JsonConvert.SerializeObject(order));
            DonHangData dhData = new DonHangData();
            List<ChiTietDonHangModels> data = dhData.LayChiTietDonHangForPrint(ID_DonHang, "vi");
            TamCoc_PrintObject tamCoc_PrintObject = new TamCoc_PrintObject();
            tamCoc_PrintObject.lstChiTiet = data;
            tamCoc_PrintObject.ThuNgan = ThuNgan;
            tamCoc_PrintObject.Ngay = DateTime.Now;
            tamCoc_PrintObject.MaThamChieu = donhang.mathamchieu;
            tamCoc_PrintObject.MaQRThanhToan = JsonConvert.DeserializeObject(resultQR);
            return View(tamCoc_PrintObject);
        }

        public ActionResult VnpayWaitPayment_KC(int ID_DonHang, string ThuNgan)
        {
            DonHangData dhdt = new DonHangData();
            DonHangModels donhang = dhdt.LayDonHang(ID_DonHang);
            VnPayOrder order = new VnPayUtils().CreateOrder_KheCoc(donhang);
            string resultQR = ApiUtility.CreateQRVnpay(JsonConvert.SerializeObject(order));
            DonHangData dhData = new DonHangData();
            List<ChiTietDonHangModels> data = dhData.LayChiTietDonHangForPrint(ID_DonHang, "vi");
            TamCoc_PrintObject tamCoc_PrintObject = new TamCoc_PrintObject();
            tamCoc_PrintObject.lstChiTiet = data;
            tamCoc_PrintObject.ThuNgan = ThuNgan;
            tamCoc_PrintObject.Ngay = DateTime.Now;
            tamCoc_PrintObject.MaThamChieu = donhang.mathamchieu;
            tamCoc_PrintObject.MaQRThanhToan = JsonConvert.DeserializeObject(resultQR);
            return View(tamCoc_PrintObject);
        }

        //public ActionResult VnpayWaitPaymentPCHL(int ID_DonHang, string ThuNgan)
        //{
        //    DonHangData dhdt = new DonHangData();
        //    DonHangv2 dh = dhdt.GetDonHangTheoID_v2(ID_DonHang, 1);
        //    dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang);
        //    dh.NhanVien = new NhanVien_dl().GetNVTheoID(dh.ID_NhanVien);
        //    DonHangModels donhang = dhdt.LayDonHang(ID_DonHang);
        //    VnPayOrder order = new VnPayUtils().CreateOrder(donhang);
        //    string resultQR = ApiUtility.CreateQRVnpay(JsonConvert.SerializeObject(order));
        //    dh.MaThanhToan = JsonConvert.DeserializeObject(resultQR);
        //    return View(dh);
        //}

        public ActionResult PrintQRTicketByBookingCodeForPCHL(int ID_DonHang, string BookingCode, string ThuNgan, string SiteCode)
        {
            LSPos_Data.Utilities.Log.Info("Print PCHL: " + ID_DonHang + " - " + BookingCode + " - " + ThuNgan + " - " + SiteCode);
            DonHangData dhdt = new DonHangData();
            DonHangModels donhang = dhdt.LayDonHang(ID_DonHang);
            DonHang_DichVuRequestAPIModel dhdv = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(ID_DonHang).Where(x => string.IsNullOrWhiteSpace(BookingCode) || x.BookingCode == BookingCode).FirstOrDefault();
            if (dhdv != null)
            {
                dhdv.lstTicket = dhdt.GetDSChiTietMatHangDonHang_DichVu(0, ID_DonHang, 0, 0, string.IsNullOrWhiteSpace(BookingCode) ? "" : BookingCode, SiteCode);
                dhdv.lstTicket = dhdv.lstTicket.DistinctBy(p => p.MaVeDichVu).OrderBy(x => x.TenHienThi).ToList();
            }

            DonHangData dhData = new DonHangData();
            List<ChiTietDonHangModels> data = dhData.LayChiTietDonHangForPrint(ID_DonHang, "vi");
            List<LichSuThanhToanModel> lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(ID_DonHang);
            TamCoc_PrintObject tamCoc_PrintObject = new TamCoc_PrintObject();
            tamCoc_PrintObject.lstThanhToan = lichsuthanhtoan;
            tamCoc_PrintObject.lstTicket = dhdv != null ? dhdv.lstTicket : new List<ChiTietMatHangDonHangModels>();
            tamCoc_PrintObject.lstChiTiet = data;
            tamCoc_PrintObject.ThuNgan = ThuNgan;
            tamCoc_PrintObject.Ngay = dhdv != null ? dhdv.Ngay : DateTime.Now;
            tamCoc_PrintObject.MaThamChieu = donhang.mathamchieu;
            return View(tamCoc_PrintObject);
        }

        //[Authorize]
        //public ActionResult InsertAccountReceivable(int ID_NhaCungCap, decimal Amount, string Note, string ImgUrl)
        //{
        //    NhaCungCapDB nccdb = new NhaCungCapDB();
        //    NhaCungCapOBJ ncc = nccdb.GetById(ID_NhaCungCap);
        //    SiteModel site = new SiteDAO().GetSite(ncc.SiteCode);
        //    new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, ncc.ProfileID, ncc.ProfileCode);

        //    string returnID2 = new ARRepository().InsertTransAR(ncc.AccountReceivableNo, ncc.PaymentTypeID, Amount, 0m, Note);
        //    Guid arPaymentID3 = Guid.Parse(returnID2);
        //    new ARRepository().Allocate("2", string.Empty, Guid.Parse(ncc.AccountReceivableNo), arPaymentID3);

        //    List<ARModel> data = new ARRepository().GetDataAr("", ncc.AccountReceivableNo);
        //    LichSuNapVi_NhomTaiKhoanModel napvi = new LichSuNapVi_NhomTaiKhoanModel();
        //    napvi.ID_NhaCungCap = ID_NhaCungCap;
        //    napvi.NgayTao = new DateTime();
        //    napvi.SoTien = Amount;
        //    napvi.TongSoDu = data.FirstOrDefault().SoDu;
        //    napvi.ImgUrl = ImgUrl;
        //    nccdb.ThemLichSuNapVi(napvi);
        //    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetAccountReceivable(int ID_NhaCungCap)
        {
            NhaCungCapOBJ ncc = new NhaCungCapDB().GetById(ID_NhaCungCap);
            SiteModel site = new SiteDAO().GetSite(ncc.SiteCode);
            new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, ncc.ProfileID, ncc.ProfileCode);

            List<ARModel> data = new ARRepository().GetDataAr("", ncc.AccountReceivableNo);
            return Json(new { success = true, data = data.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProcessPayment(int iddh)
        {
            try
            {
                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_AccessCode = "";
                string vpc_ReturnURL = "";
                string vpc_Currency = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["VI_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["VI_vpc_Version"];
                vpc_Command = "pay";
                vpc_Merchant = ConfigurationManager.AppSettings["VI_vpc_Merchant"];
                vpc_AccessCode = ConfigurationManager.AppSettings["VI_vpc_AccessCode"];
                vpc_ReturnURL = Request.Url.Scheme + "://" + Request.Url.Host + ConfigurationManager.AppSettings["VI_vpc_ReturnURL"] + "/" + iddh;
                vpc_Currency = ConfigurationManager.AppSettings["VI_vpc_Currency"];
                SECURE_SECRET = ConfigurationManager.AppSettings["VI_vpc_Hascode"];
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(iddh);
                string vpc_MerchTxnRef = iddh + "_" + donhang.thoigiantao.Ticks.ToString();
                string vpc_OrderInfo = donhang.mathamchieu + " " + donhang.tenvilspay;
                string vpc_Amount = (donhang.tongtien * 100).ToString();

                string vpc_TicketNo = "";

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        vpc_TicketNo = ip.ToString();
                    }
                }
                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                // Add the Digital Order Fields for the functionality you wish to use
                conn.SetSecureSecret(SECURE_SECRET);

                // Core Transaction Fields
                conn.AddDigitalOrderField("Title", "onepay paygate");
                conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
                conn.AddDigitalOrderField("vpc_Version", vpc_Version);
                conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                conn.AddDigitalOrderField("vpc_OrderInfo", vpc_OrderInfo);
                conn.AddDigitalOrderField("vpc_Amount", vpc_Amount);
                conn.AddDigitalOrderField("vpc_Currency", vpc_Currency);
                conn.AddDigitalOrderField("vpc_ReturnURL", vpc_ReturnURL);
                // Thong tin them ve khach hang. De trong neu khong co thong tin
                conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
                conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
                conn.AddDigitalOrderField("vpc_SHIP_City", "");
                conn.AddDigitalOrderField("vpc_SHIP_Country", "");
                conn.AddDigitalOrderField("vpc_Customer_Phone", "");
                conn.AddDigitalOrderField("vpc_Customer_Email", "");
                conn.AddDigitalOrderField("vpc_Customer_Id", "");
                // Dia chi IP cua khach hang
                conn.AddDigitalOrderField("vpc_TicketNo", vpc_TicketNo);
                // Chuyen huong trinh duyet sang cong thanh toan
                String url = conn.Create3PartyQueryString();
                return Redirect(url);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProcessPaymentB2C(int iddh)
        {
            try
            {
                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_AccessCode = "";
                string vpc_ReturnURL = "";
                string vpc_Currency = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["B2C_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["B2C_vpc_Version"];
                vpc_Command = "pay";
                vpc_Merchant = ConfigurationManager.AppSettings["B2C_vpc_Merchant"];
                vpc_AccessCode = ConfigurationManager.AppSettings["B2C_vpc_AccessCode"];
                vpc_ReturnURL = Request.Url.Scheme + "://" + Request.Url.Host + ConfigurationManager.AppSettings["B2C_vpc_ReturnURL"] + "/" + iddh;
                vpc_Currency = ConfigurationManager.AppSettings["B2C_vpc_Currency"];
                SECURE_SECRET = ConfigurationManager.AppSettings["B2C_vpc_Hascode"];
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(iddh);
                string vpc_MerchTxnRef = iddh + "_" + donhang.thoigiantao.Ticks.ToString();
                string vpc_OrderInfo = donhang.mathamchieu + " " + donhang.tenvilspay;
                string vpc_Amount = (donhang.tongtien * 100).ToString();

                string vpc_TicketNo = "";

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        vpc_TicketNo = ip.ToString();
                    }
                }
                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                // Add the Digital Order Fields for the functionality you wish to use
                conn.SetSecureSecret(SECURE_SECRET);

                // Core Transaction Fields
                conn.AddDigitalOrderField("Title", "onepay paygate");
                conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
                conn.AddDigitalOrderField("vpc_Version", vpc_Version);
                conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                conn.AddDigitalOrderField("vpc_OrderInfo", vpc_OrderInfo);
                conn.AddDigitalOrderField("vpc_Amount", vpc_Amount);
                conn.AddDigitalOrderField("vpc_Currency", vpc_Currency);
                conn.AddDigitalOrderField("vpc_ReturnURL", vpc_ReturnURL);
                // Thong tin them ve khach hang. De trong neu khong co thong tin
                conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
                conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
                conn.AddDigitalOrderField("vpc_SHIP_City", "");
                conn.AddDigitalOrderField("vpc_SHIP_Country", "");
                conn.AddDigitalOrderField("vpc_Customer_Phone", "");
                conn.AddDigitalOrderField("vpc_Customer_Email", "");
                conn.AddDigitalOrderField("vpc_Customer_Id", "");
                // Dia chi IP cua khach hang
                conn.AddDigitalOrderField("vpc_TicketNo", vpc_TicketNo);
                // Chuyen huong trinh duyet sang cong thanh toan
                String url = conn.Create3PartyQueryString();
                return Redirect(url);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProcessPaymentLspay(int ID_NhomTaiKhoan, decimal SoTien)
        {
            try
            {
                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_AccessCode = "";
                string vpc_ReturnURL = "";
                string vpc_Currency = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["VI_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["VI_vpc_Version"];
                vpc_Command = "pay";
                vpc_Merchant = ConfigurationManager.AppSettings["VI_vpc_Merchant"];
                vpc_AccessCode = ConfigurationManager.AppSettings["VI_vpc_AccessCode"];
                vpc_ReturnURL = Request.Url.Scheme + "://" + Request.Url.Host + "/Booking/PaymentSuccessLspay/" + ID_NhomTaiKhoan;
                vpc_Currency = ConfigurationManager.AppSettings["VI_vpc_Currency"];
                SECURE_SECRET = ConfigurationManager.AppSettings["VI_vpc_Hascode"];
                LichSuNapVi_NhomTaiKhoanDB db = new LichSuNapVi_NhomTaiKhoanDB();
                NhomOBJ nhom = NhomDB.get_NhomById(ID_NhomTaiKhoan);
                LichSuNapVi_NhomTaiKhoanModel model = new LichSuNapVi_NhomTaiKhoanModel();
                model.NgayTao = DateTime.Now;
                model.CongThanhToan = "ONEPAY";
                model.TrangThai = 0;
                model.ImgUrl = "";
                model.ID_NhomTaiKhoan = ID_NhomTaiKhoan;
                model.SoTien = SoTien;
                model.DuLieuThanhToan = "";
                int idlsunap = db.ThemLichSuNapVi(model);
                if (idlsunap > 0)
                {
                    string vpc_MerchTxnRef = "lspay_" + ID_NhomTaiKhoan + "_" + idlsunap;
                    string vpc_OrderInfo = "lspay_" + ID_NhomTaiKhoan + "_" + nhom.MaNhom;
                    string vpc_Amount = (SoTien * 100).ToString();

                    string vpc_TicketNo = "";

                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            vpc_TicketNo = ip.ToString();
                        }
                    }
                    // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                    VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                    // Add the Digital Order Fields for the functionality you wish to use
                    conn.SetSecureSecret(SECURE_SECRET);

                    // Core Transaction Fields
                    conn.AddDigitalOrderField("Title", "onepay paygate");
                    conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
                    conn.AddDigitalOrderField("vpc_Version", vpc_Version);
                    conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                    conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                    conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                    conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                    conn.AddDigitalOrderField("vpc_OrderInfo", vpc_OrderInfo);
                    conn.AddDigitalOrderField("vpc_Amount", vpc_Amount);
                    conn.AddDigitalOrderField("vpc_Currency", vpc_Currency);
                    conn.AddDigitalOrderField("vpc_ReturnURL", vpc_ReturnURL);
                    // Thong tin them ve khach hang. De trong neu khong co thong tin
                    conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
                    conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
                    conn.AddDigitalOrderField("vpc_SHIP_City", "");
                    conn.AddDigitalOrderField("vpc_SHIP_Country", "");
                    conn.AddDigitalOrderField("vpc_Customer_Phone", "");
                    conn.AddDigitalOrderField("vpc_Customer_Email", "");
                    conn.AddDigitalOrderField("vpc_Customer_Id", "");
                    // Dia chi IP cua khach hang
                    conn.AddDigitalOrderField("vpc_TicketNo", vpc_TicketNo);
                    // Chuyen huong trinh duyet sang cong thanh toan
                    String url = conn.Create3PartyQueryString();
                    return Redirect(url);
                }
                else
                {
                    return Json(new { success = false, data = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateVnPayQRPayment(int ID_DonHang)
        {
            try
            {
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(ID_DonHang);

                VnPayOrder order = new VnPayUtils().CreateOrder(donhang);
                string resultQR = ApiUtility.CreateQRVnpay(JsonConvert.SerializeObject(order));
                return Json(new { success = true, checksum = order.checksum, qr = resultQR }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateVnPayQRPayment_KheCoc(int ID_DonHang)
        {
            try
            {
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(ID_DonHang);

                VnPayOrder order = new VnPayUtils().CreateOrder_KheCoc(donhang);
                string resultQR = ApiUtility.CreateQRVnpay(JsonConvert.SerializeObject(order));
                return Json(new { success = true, checksum = order.checksum, qr = resultQR }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PaymentSuccess(string vpc_Amount, string vpc_CardNum, string vpc_Command, string vpc_MerchTxnRef, string vpc_Merchant, string vpc_Message, string vpc_OrderInfo, string vpc_PayChannel, string vpc_TransactionNo, string vpc_TxnResponseCode, string vpc_Version, string vpc_SecureHash)
        {
            string dataipn = vpc_Amount + "/" + vpc_CardNum + "/" + vpc_Command + "/" + vpc_MerchTxnRef + "/" + vpc_Merchant + "/" + vpc_Message + "/" + vpc_OrderInfo + "/" + vpc_PayChannel + "/" + vpc_TransactionNo + "/" + vpc_TxnResponseCode + "/" + vpc_Version + "/" + vpc_SecureHash;
            string param = "";
            DonHangModels donhang = new DonHangModels();
            DonHangv2 dh = new DonHangv2();
            dh.chitietdonhang = new List<DonHang_DichVuRequestAPIModel>();
            if (vpc_TxnResponseCode == "0")
            {
                if (Request.RequestContext.RouteData.Values.Count == 3)
                {
                    param = Request.RequestContext.RouteData.Values["id"].ToString();
                    int iddh = int.Parse(param);
                    DonHangData dhData = new DonHangData();
                    donhang = dhData.LayDonHang(iddh);
                    dh = dhData.GetDonHangTheoID_v2(iddh, 0);
                    if (donhang.trangthaithanhtoan == 4 && donhang.tongtien > 0)
                    {
                        return View(dh);
                    }
                    else
                    {
                        if (dhData.ThanhToanDonHang(iddh, donhang.tongtien, 0, 0, "ONEPAY", dataipn, 3))
                        {
                            donhang = dhData.LayDonHang(iddh);
                            if (donhang.trangthaithanhtoan == 4)
                            {
                                try
                                {
                                    dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(iddh);
                                    dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(iddh);

                                    EmailHelper helper = new EmailHelper();
                                    string path = Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                    string bodyTemplate = System.IO.File.ReadAllText(path);
                                    var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                    //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN HÀNG " + dh.MaThamChieu + " THANH TOÁN!");
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    LSPos_Data.Utilities.Log.Info("Start");
                                    DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                                    List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                                    if (lstdv.Count > 0)
                                    {
                                        List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                        List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                        foreach (SiteModel s in lstSite)
                                        {
                                            if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                            {
                                                lstSitedistinct.Add(s);
                                            }
                                        }
                                        SaveBookingModel saveBooking = null;
                                        foreach (SiteModel site in lstSitedistinct)
                                        {
                                            List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                            var data = BookingUtilities.XuLyXuatVe(models, site, donhang.iddonhang, 0, _bookingRepository, _sellRepository);

                                        }
                                        //}
                                    }
                                    else
                                    {
                                        dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }

                }
            }
            return View(dh);
        }

        public ActionResult PaymentSuccessB2C(string vpc_Amount, string vpc_CardNum, string vpc_Command, string vpc_MerchTxnRef, string vpc_Merchant, string vpc_Message, string vpc_OrderInfo, string vpc_PayChannel, string vpc_TransactionNo, string vpc_TxnResponseCode, string vpc_Version, string vpc_SecureHash)
        {
            string dataipn = vpc_Amount + "/" + vpc_CardNum + "/" + vpc_Command + "/" + vpc_MerchTxnRef + "/" + vpc_Merchant + "/" + vpc_Message + "/" + vpc_OrderInfo + "/" + vpc_PayChannel + "/" + vpc_TransactionNo + "/" + vpc_TxnResponseCode + "/" + vpc_Version + "/" + vpc_SecureHash;
            string param = "";
            DonHangModels donhang = new DonHangModels();
            DonHangv2 dh = new DonHangv2();
            dh.chitietdonhang = new List<DonHang_DichVuRequestAPIModel>();
            if (vpc_TxnResponseCode == "0")
            {
                if (Request.RequestContext.RouteData.Values.Count == 3)
                {
                    param = Request.RequestContext.RouteData.Values["id"].ToString();
                    int iddh = int.Parse(param);
                    DonHangData dhData = new DonHangData();
                    donhang = dhData.LayDonHang(iddh);
                    dh = dhData.GetDonHangTheoID_v2(iddh, 0);
                    if (donhang.trangthaithanhtoan == 4 && donhang.tongtien > 0)
                    {
                        return View(dh);
                    }
                    else
                    {
                        if (dhData.ThanhToanDonHang(iddh, donhang.tongtien, 0, 0, "ONEPAY-B2C", dataipn, 3))
                        {
                            donhang = dhData.LayDonHang(iddh);
                            if (donhang.trangthaithanhtoan == 4)
                            {
                                try
                                {
                                    dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(iddh);
                                    dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(iddh);

                                    EmailHelper helper = new EmailHelper();
                                    string path = Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                    string bodyTemplate = System.IO.File.ReadAllText(path);
                                    var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                    //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN HÀNG " + dh.MaThamChieu + " THANH TOÁN!");
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    LSPos_Data.Utilities.Log.Info("Start");
                                    DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                                    List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                                    if (lstdv.Count > 0)
                                    {
                                        List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                        List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                        foreach (SiteModel s in lstSite)
                                        {
                                            if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                            {
                                                lstSitedistinct.Add(s);
                                            }
                                        }
                                        SaveBookingModel saveBooking = null;
                                        foreach (SiteModel site in lstSitedistinct)
                                        {
                                            List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                            var data = BookingUtilities.XuLyXuatVe(models, site, donhang.iddonhang, 0, _bookingRepository, _sellRepository);
                                            try
                                            {
                                                dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(iddh);
                                                dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(iddh);

                                                EmailHelper helper = new EmailHelper();
                                                string path = Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                                string bodyTemplate = System.IO.File.ReadAllText(path);
                                                var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                                //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN HÀNG " + dh.MaThamChieu + " THANH TOÁN!");
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        //}
                                    }
                                    else
                                    {
                                        dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }

                }
            }
            return View(dh);
        }

        public ActionResult PaymentSuccessLspay(string vpc_Amount, string vpc_CardNum, string vpc_Command, string vpc_MerchTxnRef, string vpc_Merchant, string vpc_Message, string vpc_OrderInfo, string vpc_PayChannel, string vpc_TransactionNo, string vpc_TxnResponseCode, string vpc_Version, string vpc_SecureHash)
        {
            LSPos_Data.Utilities.Log.Info(vpc_Amount + "/" + vpc_CardNum + "/" + vpc_Command + "/" + vpc_MerchTxnRef + "/" + vpc_Merchant + "/" + vpc_Message + "/" + vpc_OrderInfo + "/" + vpc_PayChannel + "/" + vpc_TransactionNo + "/" + vpc_TxnResponseCode + "/" + vpc_Version + "/" + vpc_SecureHash);
            string param = "";
            LichSuNapVi_NhomTaiKhoanModel item = new LichSuNapVi_NhomTaiKhoanModel();
            if (vpc_TxnResponseCode == "0")
            {
                if (Request.RequestContext.RouteData.Values.Count == 3)
                {
                    param = Request.RequestContext.RouteData.Values["id"].ToString();
                    int ID_NhomTaiKhoan = int.Parse(param);
                    LichSuNapVi_NhomTaiKhoanDB db = new LichSuNapVi_NhomTaiKhoanDB();
                    item = db.GetLichSuNapViByID(int.Parse(vpc_MerchTxnRef.Split('_')[2].ToString()));
                    item.TrangThai = 1;
                    item.ImgUrl = "";
                    item.SoTien = decimal.Parse(vpc_Amount) / 100m;
                    item.DuLieuThanhToan = vpc_Amount + "/" + vpc_CardNum + "/" + vpc_Command + "/" + vpc_MerchTxnRef + "/" + vpc_Merchant + "/" + vpc_Message + "/" + vpc_OrderInfo + "/" + vpc_PayChannel + "/" + vpc_TransactionNo + "/" + vpc_TxnResponseCode + "/" + vpc_Version + "/" + vpc_SecureHash;
                    item.CongThanhToan = "ONEPAY-DCOM";
                    if (db.UpdateThanhCong_LichSuNapVi(item))
                    {
                        return View(item);
                    }
                    else
                    {
                        return View(item);
                    }
                }
            }
            return View();
        }

        public ActionResult PaymentSuccessVnPay(string code, string message, string msgType, string txnId, string qrTrace, string bankCode, string mobile, string accountNo, string amount, string payDate, string merchantCode, string terminalId, string name, string phone, string province_id, string district_id, string address, string email, string addData, string checksum)
        {
            LSPos_Data.Utilities.Log.Info(code + "/" + message + "/" + msgType + "/" + txnId + "/" + qrTrace + "/" + bankCode + "/" + mobile + "/" + accountNo + "/" + amount + "/" + payDate + "/" + merchantCode + "/" + terminalId + "/" + name + "/" + phone + "/" + province_id + "/" + district_id + "/" + address + "/" + address + "/" + addData + "/" + checksum);
            string param = "";
            return View();
        }
    }
}