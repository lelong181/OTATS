using LSPosMVC.Common;
using LSPos_Data.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BusinessLayer.Repository;
using LSPos_Data.DataAccess;
using LSPos_Data.Models;
using LSPosMVC.App_Start;
using LSPosMVC.Common.Paycollect;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNet.SignalR;
using LSPosMVC.Hubs;
using Ticket;

namespace LSPosMVC.Controllers
{
    [System.Web.Mvc.Authorize]
    [RoutePrefix("api/ota")]
    public class OtaController : ApiController
    {
        private SellRepository _sellRepository;
        private BookingRepository _bookingRepository;

        public OtaController()
        {
            _sellRepository = new SellRepository();
            _bookingRepository = new BookingRepository();
        }

        [HttpGet]
        [Route("products")]
        public HttpResponseMessage danhsachmathang()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DonHangData dh = new DonHangData();
                    List<MatHang> dsmh = new List<MatHang>();
                    if (userinfo.IsAdmin)
                    {
                        dsmh = dh.getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoQuanLy(0, userinfo.ID_QLLH, userinfo.ID_QuanLy, -1);
                    }
                    else
                    {
                        dsmh = dh.getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoNhanVien(0, userinfo.ID_QLLH, userinfo.ID_QuanLy, -1);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, dsmh);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }


        [HttpPost]
        [Route("book-payment-exportticket")]
        public HttpResponseMessage addAndPaymentNewway([FromBody] DonHangModels paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            LSPos_Data.Utilities.Log.Info(JsonConvert.SerializeObject(paramDH));
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {

                    //Kiểm tra phân quyền bán
                    DonHangData dhData = new DonHangData();
                    //List<MatHang> dsmh = dhData.getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoNhanVien(0, userinfo.ID_QLLH, userinfo.ID_QuanLy, -1);
                    //foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                    //{
                    //    bool allowSale = true;
                    //    if (!dsmh.Any(x => x.IDMatHang == ctdhobj.idhanghoa))
                    //    {
                    //        allowSale = false;
                    //    }
                    //    response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, err = 4, msg = "Không được cấp phép bán sản phẩm đã chọn!" });
                    //    return response;
                    //}
                    // bắt đầu tạo đơn
                    paramDH.hinhthucban = 2;
                    paramDH.idnhanvientao = userinfo.ID_QuanLy;
                    paramDH.idct = userinfo.ID_QLLH; // id công ty
                    paramDH.thoigiantao = DateTime.Now; // thời gian tao
                    paramDH.idcuahang = 63;
                    paramDH.diachitao = paramDH.echotoken;
                    NhanVienApp nvApp = new NhanVienApp();
                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                    //NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(paramDH.idnhanvien);
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoTenDangNhap(userinfo.Username, userinfo.ID_QLLH);
                    paramDH.idnhanvien = nhanVienInfor.idnhanvien;
                    paramDH.idnhanvientao = nhanVienInfor.idnhanvien;
                    int sttDonHang = 0;
                    string macuoicung = dhData.GetMaThamChieu(paramDH.idct, "");
                    paramDH.mathamchieu = utilsCommon.genMaDonHang(nhanVienInfor, macuoicung, nhanVienInfor.idnhom.ToString(), nhanVienInfor.MaNhom, nhanVienInfor.STT_DonHang);
                    int exited_id = dhData.CheckEchotoken(paramDH.echotoken);
                    if (exited_id > 0)
                    {
                        LSPos_Data.Utilities.Log.Info("Trùng echotoken: " + paramDH.echotoken);
                        try
                        {
                            List<ChiTietDonHangModels> data = dhData.LayChiTietDonHang(exited_id, "vi");
                            string linkPrintTicket = "https://server.lscloud.vn/Booking/PrintQRTicketByBookingCodeForBaiDinh?ID_ChiTietDonHang=" + data[0].idchitietdonhang + "&ID_DonHang=" + data[0].dschitietmathang[0].ID_DonHang + "&ID_MatHang=" + data[0].dschitietmathang[0].ID_MatHang + "&ID_DichVu=" + data[0].dschitietmathang[0].ID_DichVu + "&BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu + "&SiteCode=TRANGAN";
                            string onlinePrintTicket = "https://server.lscloud.vn/Booking/PrintTicketByBookingCode?ID_ChiTietDonHang=0&ID_DonHang=" + data[0].dschitietmathang[0].ID_DonHang + "&ID_MatHang=0&ID_DichVu=0&BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu + "&SiteCode=TRANGAN";
                            response = Request.CreateResponse(HttpStatusCode.Created, new { msg = "Echotoken " + paramDH.echotoken + " bị trùng", iddonhang = data[0].dschitietmathang[0].MaDonHangDichVu, printTicketUrl = linkPrintTicket, onlinePrintTicket = onlinePrintTicket });
                        }
                        catch
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Xảy ra lỗi trong quá trình xuất vé");
                        }
                    }
                    else
                    {
                        DonHangModels DHThemOBJ = dhData.TaoDonHang(paramDH);
                        if (DHThemOBJ.iddonhang > 0 && DHThemOBJ.tongtien >= 0)
                        {
                            dhData.CapNhatSTT_DonHang(nhanVienInfor.idnhom.ToString(), sttDonHang + 1);
                            List<string> lstSiteCode = new List<string>();
                            foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                            {
                                try
                                {
                                    ctdhobj.iddonhang = DHThemOBJ.iddonhang;
                                    int ID_ChiTietDonHang = dhData.TaoChiTietDonHang(ctdhobj);
                                    if (ID_ChiTietDonHang > 0)
                                    {
                                        ctdhobj.idchitietdonhang = ID_ChiTietDonHang;
                                        if (ctdhobj.dschitietmathang != null)
                                            dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, DHThemOBJ.iddonhang, ctdhobj.dschitietmathang);
                                        if (ctdhobj.lstDichVu != null)
                                        {
                                            foreach (HangHoa_DichVuModel hhdv in ctdhobj.lstDichVu)
                                            {
                                                ChiTietMatHang_DichVuModel it = new ChiTietMatHang_DichVuModel();
                                                it.ID_DonHang = DHThemOBJ.iddonhang;
                                                it.ID_ChiTietDonHang = ctdhobj.idchitietdonhang;
                                                it.ID_MatHang = hhdv.ID_HangHoa;
                                                it.ID_DichVu = hhdv.ID_DichVu;
                                                it.SoLuong = hhdv.SoLuong;
                                                it.GiaBan = hhdv.GiaBan;
                                                it.Loai = hhdv.Loai;
                                                ctmhdao.InsertOrUpdate(it);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                            }
                            DonHangModels donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                            //kết thúc tạo đơn

                            //bắt đầu thanh toán
                            bool paymentSuccess = false;
                            if (paramDH.hinhthucthanhtoan != 4)
                            {
                                if (dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                                {
                                    paymentSuccess = true;
                                }
                            }
                            else
                            {
                                if (dhData.BienDongSoDuNhomTaiKhoan(nhanVienInfor.idnhom, DateTime.Now, -1, DHThemOBJ.iddonhang, (float)paramDH.tongtien, 0, ""))
                                {
                                    float sodu = dhData.BienDongSoDuNhom_TongTienHienTai(nhanVienInfor.idnhom);
                                    float gioihangcn = NhomData.getNhomByID(nhanVienInfor.idnhom).CongNoGioiHan;
                                    if (sodu - gioihangcn >= paramDH.tongtien)
                                    {
                                        if (dhData.BienDongSoDuNhom_UpdateTrangThaiThanhToan(userinfo.ID_QuanLy, DHThemOBJ.iddonhang)
                                            && dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                                        {
                                            paymentSuccess = true;
                                        }
                                    }
                                    else
                                    {
                                        response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, err = 2, msg = "Số dư trong ví không đủ để thực hiện thanh toán! Vui lòng nạp thêm!" });
                                        return response;
                                    }
                                }
                                else
                                {
                                    response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, err = 3, msg = "Không thể sử dụng ví" });
                                    return response;
                                }
                            }
                            donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                            //kết thúc thanh toán

                            if (paymentSuccess)
                            {
                                //bắt đầu xuất vé
                                string bookingcode = "";
                                if (donHangCu.trangthaithanhtoan == 4 && donHangCu.trangthaidonhang == 2)
                                {
                                    DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                    List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donHangCu.iddonhang);
                                    List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                    List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                    foreach (SiteModel s in lstSite)
                                    {
                                        if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                        {
                                            lstSitedistinct.Add(s);
                                        }
                                    }
                                    foreach (SiteModel site in lstSitedistinct)
                                    {
                                        List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                        BookingUtilities.XuLyXuatVePOS(models, site, donHangCu.iddonhang, 0, _bookingRepository, _sellRepository);
                                    }

                                }
                                //kết thúc xuất vé
                                LSPos_Data.Utilities.Log.Info(nhanVienInfor.tendangnhap + " - " + donHangCu.iddonhang + " - " + donHangCu.mathamchieu);
                                try
                                {
                                    List<ChiTietDonHangModels> data = dhData.LayChiTietDonHang(DHThemOBJ.iddonhang, "vi");
                                    string linkPrintTicket = "https://svtour.lscloud.vn/Booking/PrintQRTicketByBookingCodeForBaiDinh?ID_ChiTietDonHang=" + data[0].idchitietdonhang + "&ID_DonHang=" + data[0].dschitietmathang[0].ID_DonHang + "&ID_MatHang=" + data[0].dschitietmathang[0].ID_MatHang + "&ID_DichVu=" + data[0].dschitietmathang[0].ID_DichVu + "&BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu + "&SiteCode=TRANGAN";
                                    string onlinePrintTicket = "https://svtour.lscloud.vn/Booking/PrintTicketByBookingCode?ID_ChiTietDonHang=0&ID_DonHang=" + data[0].dschitietmathang[0].ID_DonHang + "&ID_MatHang=0&ID_DichVu=0&BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu + "&SiteCode=TRANGAN";
                                    response = Request.CreateResponse(HttpStatusCode.Created, new { iddonhang = data[0].dschitietmathang[0].MaDonHangDichVu, printTicketUrl = linkPrintTicket, onlinePrintTicket = onlinePrintTicket });
                                }
                                catch
                                {
                                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Xảy ra lỗi trong quá trình xuất vé");
                                }

                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Thanh toán đơn hàng không thành công");
                            }
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }


        [HttpGet]
        [Route("get-detail-order-ls")]
        public HttpResponseMessage getDetailOrder([FromUri] int iddonhang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DonHangData dhData = new DonHangData();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    List<ChiTietDonHangModels> data = dhData.LayChiTietDonHang(iddonhang, "vi");
                    //if (data[0].idnhanvien == userinfo.ID_QuanLy)
                    //{
                    string linkPrintTicket = "https://server.lscloud.vn/Booking/PrintQRTicketByBookingCode?ID_ChiTietDonHang=" + data[0].idchitietdonhang + "&ID_DonHang=" + data[0].dschitietmathang[0].ID_DonHang + "&ID_MatHang=" + data[0].dschitietmathang[0].ID_MatHang + "&ID_DichVu=" + data[0].dschitietmathang[0].ID_DichVu + "&BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu + "&SiteCode=TRANGAN";
                    string onlinePrintTicket = "https://server.lscloud.vn/Booking/OnlineTicketByBookingCode?BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu;
                    response = Request.CreateResponse(HttpStatusCode.Created, new { iddonhang = iddonhang, bookingcode = data[0].dschitietmathang[0].MaBookingDichVu, printTicketUrl = linkPrintTicket, onlinePrintTicket = onlinePrintTicket });
                    //}
                    //else
                    //{
                    //    response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Không tìm thấy đơn hàng");
                    //}
                }
            }
            catch
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Không tìm thấy đơn hàng");
            }
            return response;
        }


        [HttpPost]
        [Route("book-payment-exportticket-ls")]
        public HttpResponseMessage addAndPaymentLsTour([FromBody] List<ChiTietDonHangModels> chitiet, string ghichu = "")
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DonHangModels paramDH = new DonHangModels();
            paramDH.ghichu = ghichu;
            paramDH.chitietdonhang = chitiet;
            LSPos_Data.Utilities.Log.Info(JsonConvert.SerializeObject(paramDH));
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    // bắt đầu tạo đơn
                    paramDH.hinhthucban = 2;
                    paramDH.hinhthucthanhtoan = 4;
                    paramDH.idnhanvientao = userinfo.ID_QuanLy;
                    paramDH.idct = userinfo.ID_QLLH; // id công ty
                    paramDH.thoigiantao = DateTime.Now; // thời gian tao
                    paramDH.idcuahang = 48;
                    NhanVienApp nvApp = new NhanVienApp();
                    DonHangData dhData = new DonHangData();
                    MatHang_dl matHangData = new MatHang_dl();
                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoTenDangNhap(userinfo.Username, userinfo.ID_QLLH);
                    paramDH.idnhanvien = nhanVienInfor.idnhanvien;
                    paramDH.idnhanvientao = nhanVienInfor.idnhanvien;
                    int sttDonHang = 0;
                    string macuoicung = dhData.GetMaThamChieu(paramDH.idct, "");
                    paramDH.mathamchieu = utilsCommon.genMaDonHang(nhanVienInfor, macuoicung, nhanVienInfor.idnhom.ToString(), nhanVienInfor.MaNhom, nhanVienInfor.STT_DonHang);
                    paramDH.tongtien = 0;
                    foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                    {
                        MatHang mh = matHangData.GetMatHangTheoID(ctdhobj.idhanghoa);
                        paramDH.tongtien += (double)mh.GiaLe * ctdhobj.soluong;
                    }
                    DonHangModels DHThemOBJ = dhData.TaoDonHang(paramDH);
                    if (DHThemOBJ.iddonhang > 0)
                    {
                        dhData.CapNhatSTT_DonHang(nhanVienInfor.idnhom.ToString(), sttDonHang + 1);
                        List<string> lstSiteCode = new List<string>();
                        foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                        {
                            try
                            {
                                ctdhobj.iddonhang = DHThemOBJ.iddonhang;
                                int ID_ChiTietDonHang = dhData.TaoChiTietDonHangLS(ctdhobj);
                                if (ID_ChiTietDonHang > 0)
                                {
                                    ctdhobj.idchitietdonhang = ID_ChiTietDonHang;
                                    if (ctdhobj.dschitietmathang != null)
                                        dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, DHThemOBJ.iddonhang, ctdhobj.dschitietmathang);
                                    ctmhdao.InsertLS(ctdhobj.idchitietdonhang);
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        DonHangModels donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                        //kết thúc tạo đơn

                        //bắt đầu thanh toán
                        bool paymentSuccess = false;
                        if (paramDH.hinhthucthanhtoan != 4)
                        {
                            if (dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                            {
                                paymentSuccess = true;
                            }
                        }
                        else
                        {
                            if (dhData.BienDongSoDuNhomTaiKhoan(nhanVienInfor.idnhom, DateTime.Now, -1, DHThemOBJ.iddonhang, (float)paramDH.tongtien, 0, ""))
                            {
                                float sodu = dhData.BienDongSoDuNhom_TongTienHienTai(nhanVienInfor.idnhom);
                                float gioihangcn = NhomData.getNhomByID(nhanVienInfor.idnhom).CongNoGioiHan;
                                if (sodu - gioihangcn >= paramDH.tongtien)
                                {
                                    if (dhData.BienDongSoDuNhom_UpdateTrangThaiThanhToan(userinfo.ID_QuanLy, DHThemOBJ.iddonhang)
                                        && dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                                    {
                                        paymentSuccess = true;
                                    }
                                }
                                else
                                {
                                    response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, err = 2, msg = "Số dư trong ví không đủ để thực hiện thanh toán! Vui lòng nạp thêm!" });
                                    return response;
                                }
                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, err = 3, msg = "Không thể sử dụng ví" });
                                return response;
                            }
                        }
                        donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                        //kết thúc thanh toán

                        if (paymentSuccess)
                        {
                            //bắt đầu xuất vé
                            string bookingcode = "";
                            if (donHangCu.trangthaithanhtoan == 4 && donHangCu.trangthaidonhang == 2)
                            {
                                DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donHangCu.iddonhang);
                                List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                foreach (SiteModel s in lstSite)
                                {
                                    if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                    {
                                        lstSitedistinct.Add(s);
                                    }
                                }
                                foreach (SiteModel site in lstSitedistinct)
                                {
                                    List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                    BookingUtilities.XuLyXuatVePOS(models, site, donHangCu.iddonhang, 0, _bookingRepository, _sellRepository);
                                }

                            }
                            //kết thúc xuất vé
                            try
                            {
                                List<ChiTietDonHangModels> data = dhData.LayChiTietDonHang(DHThemOBJ.iddonhang, "vi");
                                string linkPrintTicket = "https://svtour.lscloud.vn/Booking/PrintQRTicketByBookingCode?ID_ChiTietDonHang=" + data[0].idchitietdonhang + "&ID_DonHang=" + data[0].dschitietmathang[0].ID_DonHang + "&ID_MatHang=" + data[0].dschitietmathang[0].ID_MatHang + "&ID_DichVu=" + data[0].dschitietmathang[0].ID_DichVu + "&BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu + "&SiteCode=TRANGAN";
                                string onlinePrintTicket = "https://svtour.lscloud.vn/Booking/OnlineTicketByBookingCode?BookingCode=" + data[0].dschitietmathang[0].MaBookingDichVu;
                                response = Request.CreateResponse(HttpStatusCode.Created, new { iddonhang = DHThemOBJ.iddonhang, bookingcode = data[0].dschitietmathang[0].MaBookingDichVu, printTicketUrl = linkPrintTicket, onlinePrintTicket = onlinePrintTicket });
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Xảy ra lỗi trong quá trình xuất vé");
                            }
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Thanh toán đơn hàng không thành công");
                        }
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("paycollect_createpaymentlspay")]
        public HttpResponseMessage PayCollect_CreatePaymentLsPay(int ID_NhomTaiKhoan, decimal SoTien)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                //DonHangData dhData = new DonHangData();
                //DonHangModels donhang = dhData.LayDonHang(ID_DonHang);
                NhomOBJ nhom = NhomDB.get_NhomById(ID_NhomTaiKhoan);
                LichSuNapVi_NhomTaiKhoanDB db = new LichSuNapVi_NhomTaiKhoanDB();
                LichSuNapVi_NhomTaiKhoanModel model = new LichSuNapVi_NhomTaiKhoanModel();
                model.NgayTao = DateTime.Now;
                model.CongThanhToan = "ONEPAY";
                model.TrangThai = 0;
                model.ImgUrl = "";
                model.ID_NhomTaiKhoan = ID_NhomTaiKhoan;
                model.SoTien = SoTien;
                model.DuLieuThanhToan = "";
                int idlsunap = db.ThemLichSuNapVi(model);

                //PaycollectTest pc = new PaycollectTest();
                RequestUserOP user = new RequestUserOP();
                user.name = "DISANTRANGAN  LSPAY  " + StringHelper.RemoveVietNameseSign(nhom.TenNhom);
                user.gender = "male";
                user.address = "";
                user.mobile_number = "";
                user.email = "";
                user.id_card = "";
                user.issue_date = "";
                user.issue_by = "";
                user.bank_id = "";
                user.description = "";
                user.reference = "lspay_" + ID_NhomTaiKhoan + "_" + idlsunap;
                RequestInvoiceOP inv = new RequestInvoiceOP();
                inv.amount = SoTien.ToString();
                inv.description = "lspay_" + ID_NhomTaiKhoan + "_" + idlsunap;

                //CreateUserInvoiceResponse result = pc.CallCreateUserInvoice(user, inv);
                response = Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("paycollect_createpayment_trangan")]
        public HttpResponseMessage PayCollect_CreatePayment_TrangAn(string Code, int ID_DonHang, decimal SoTien)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(ID_DonHang);

                PaycollectTest pc = new PaycollectTest(Code + "PC");
                RequestUserOP user = new RequestUserOP();
                user.name = "DI SAN TRANG AN " + donhang.mathamchieu.Replace("-", "");
                user.gender = "male";
                user.address = "";
                user.mobile_number = "";
                user.email = "";
                user.id_card = "";
                user.issue_date = "";
                user.issue_by = "";
                user.bank_id = "";
                user.description = "";
                user.reference = Code + "PC_" + ID_DonHang;
                RequestInvoiceOP inv = new RequestInvoiceOP();
                inv.amount = SoTien.ToString();
                inv.description = "ota_" + ID_DonHang;
                CreateUserInvoiceResponse result = pc.CallCreateUserInvoice(user, inv);
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }


        [AllowAnonymous]
        [HttpPut]
        [Route("onepay-dynamic-qr-ipn")]
        public HttpResponseMessage OnepayDynamicQRIpn([FromBody] OnePayIPNRequest oprequest)
        {
            LSPos_Data.Utilities.Log.Info(JsonConvert.SerializeObject(oprequest));
            byte[] payload = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(oprequest));
            string uri = HttpContext.Current.Request.Url.AbsoluteUri;
            string timeStamp = HttpContext.Current.Request.Headers["X-OP-Date"].ToString();
            string expDate = HttpContext.Current.Request.Headers["X-OP-Expires"].ToString();
            string reference = oprequest.user.reference;
            LSPosMVC.Common.Paycollect.Authorization auth = new PaycollectTest(reference.Split('_')[0]).CreateSign(timeStamp, expDate, uri, payload);
            string signHeader = HttpContext.Current.Request.Headers["X-OP-Authorization"].ToString();
            if (signHeader.Equals(auth.ToString()))
            {
                int iddonhang = int.Parse(reference.Split('_')[1].ToString());
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(iddonhang);
                if (dhData.ThanhToanDonHang(iddonhang, double.Parse(oprequest.amount), 0, 0, "ONEPAY", JsonConvert.SerializeObject(oprequest), 11))
                {
                    //bắt đầu xuất vé
                    string bookingcode = "";
                    DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                    List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(iddonhang);
                    List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                    List<SiteModel> lstSitedistinct = new List<SiteModel>();
                    foreach (SiteModel s in lstSite)
                    {
                        if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                        {
                            lstSitedistinct.Add(s);
                        }
                    }
                    foreach (SiteModel site in lstSitedistinct)
                    {
                        List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                        BookingUtilities.XuLyXuatVePOS(models, site, iddonhang, 0, _bookingRepository, _sellRepository);
                    }
                    //kết thúc xuất vé
                    return Request.CreateResponse(HttpStatusCode.Created, new { error_code = 0, message = "Success" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Created, new { error_code = 1, message = "Failed" });
                }
            }
            else
            {
                LSPos_Data.Utilities.Log.Info("Not Passed Sign");
                return Request.CreateResponse(HttpStatusCode.Created, new { error_code = 1, message = "Failed" });
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("gethanghoabyidnhomota")]
        public HttpResponseMessage gethanghoabyidnhomota([FromUri] int ID_DANHMUC, string username, string macongty)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            UserInfo userinfo = new User_dl().GetUserInfo(username, macongty);

            MatHang_dl tdt = new MatHang_dl();
            List<MatHang> data = tdt.getDS_HangHoa_ByIdDanhMuc(ID_DANHMUC, userinfo.ID_QLLH);
            HangHoa_DichVuDAO hhdvdao = new HangHoa_DichVuDAO();
            foreach (MatHang mh in data)
            {
                mh.lstDichVu = hhdvdao.GetAllByHangHoa(mh.IDMatHang);
            }
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("getnhommathangtheophanquyenota")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetNhomMatHangTheoPhanQuyenOTA([FromUri] string username, string company)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = new User_dl().GetUserInfo(username, company);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();

                    lstDanhMuc = DanhMucDB.getDS_DanhMuc_TheoPhanQuyen_Admin(0, userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    response = Request.CreateResponse(HttpStatusCode.OK, lstDanhMuc);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;

        }
    }
}
