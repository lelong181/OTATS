using LSPosMVC.Common;
using LSPos_Data.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/bando")]
    public class BanDoController : ApiController
    {
        [HttpGet]
        [Route("getlistdaily")]
        public HttpResponseMessage getlistdaily([FromUri] string kinhdo, string vido, int idtinh, int idquan, int idloaikhachhang)
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
                    List<ToaDoOBJ> data = new List<ToaDoOBJ>();
                    double kinhDo = 0;
                    double viDo = 0;
                    try
                    {
                        kinhDo = (kinhdo != null && kinhdo != "") ? double.Parse(kinhdo) : 0;
                        viDo = (vido != null && vido != "") ? double.Parse(vido) : 0;
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }

                    if (kinhDo > 0)
                    {
                        data = KhachHang_dl.TatCaKhachHangTheoIDCT_ToaDo(userinfo.ID_QLLH, userinfo.ID_QuanLy, kinhDo, viDo, idtinh, idquan, idloaikhachhang);
                    }
                    else
                    {
                        data = KhachHang_dl.TatCaKhachHangTheoIDCT(userinfo.ID_QLLH, userinfo.ID_QuanLy, idtinh, idquan, idloaikhachhang);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getvitridaily")]
        public HttpResponseMessage getvitridaily([FromUri] int idkhachhang)
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
                    ToaDoOBJ data = new ToaDoOBJ();
                    data = KhachHang_dl.ViTriKhachHang(idkhachhang);

                    KhachHang kh = new KhachHang();
                    kh = KhachHang_dl.GetKhachHangTheoID(idkhachhang);
                    if (kh == null)
                    {
                        kh = new KhachHang();
                        kh.IDKhachHang = 0;
                        kh.Ten = "{{bindTextI18n('label_khongxacdinh')}}";
                        kh.DiaChi = "";
                        kh.SoDienThoai = "";
                        kh.Email = "";
                        kh.TenLoaiKhachHang = "";
                    }

                    string img = ((kh.Imgurl != null && kh.Imgurl != "") ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + kh.Imgurl : "assets/img/noimage.png");
                    string text = "";
                    text += "<div>" +
                            "<h5 id='heading' class='heading'> {{bindTextI18n('label_khachhang')}}: " +
                                "<a href = '/#!/editkhachhang/" + kh.IDKhachHang.ToString() + "'> " +
                                kh.Ten +
                                "</a>" +
                            "</h5>" +
                            "<p> {{bindTextI18n('label_diachi')}}: " + kh.DiaChi + "</p>" +
                            "<p> {{bindTextI18n('label_dienthoai')}}: " + kh.SoDienThoai + "</p>" +
                            "<p> {{bindTextI18n('label_email')}}: " + kh.Email + "</p>" +
                            "<p> {{bindTextI18n('label_loaikhachhang')}}: " + kh.TenLoaiKhachHang + "</p>";

                    text += "<p> {{bindTextI18n('label_anhdaidien')}}: " +
                        "<div class='text-center' style=\"overflow-x:auto; \"><img style=\"width:140px; height:140px\" src = '" + img + "' /></div></p>";
                    text += "</div>";

                    response = Request.CreateResponse(HttpStatusCode.OK, new { toado = data, info = text });
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getchitietdaily")]
        public HttpResponseMessage getchitietdaily([FromUri] int idkhachhang)
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
                    KhachHang kh = new KhachHang();
                    kh = KhachHang_dl.GetKhachHangTheoID(idkhachhang);
                    if (kh == null)
                    {
                        kh = new KhachHang();
                        kh.IDKhachHang = 0;
                        kh.Ten = "{{bindTextI18n('label_khongxacdinh')}}";
                        kh.DiaChi = "";
                        kh.SoDienThoai = "";
                        kh.Email = "";
                        kh.TenLoaiKhachHang = "";
                    }
                    try
                    {
                        kh.Imgurl = kh.danhsachanh[0].path;
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }

                    string img = ((kh.Imgurl != null && kh.Imgurl != "") ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + kh.Imgurl : "assets/img/noimage.png");
                    string text = "";
                    text += "<div>" +
                            "<h5 id='heading' class='heading'> {{bindTextI18n('label_khachhang')}}: " +
                                "<a href = '/#!/editkhachhang/" + kh.IDKhachHang.ToString() + "'> " +
                                kh.Ten +
                                "</a>" +
                            "</h5>" +
                            "<p> {{bindTextI18n('label_diachi')}}: " + kh.DiaChi + "</p>" +
                            "<p> {{bindTextI18n('label_dienthoai')}}: " + kh.SoDienThoai + "</p>" +
                            "<p> {{bindTextI18n('label_email')}}: " + kh.Email + "</p>" +
                            "<p> {{bindTextI18n('label_loaikhachhang')}}: " + kh.TenLoaiKhachHang + "</p>";

                    text += "<p> {{bindTextI18n('label_anhdaidien')}}: " +
                        "<div class='text-center' style=\"overflow-x:auto; \"><img style=\"width:140px; height:140px\" src = '" + img + "' /></div></p>";
                    text += "</div>";

                    response = Request.CreateResponse(HttpStatusCode.OK, text);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getthongketructuyen")]
        public HttpResponseMessage getthongketructuyen([FromUri] int idnhom)
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
                    ThongKeTrucTuyenOBJ data = new ThongKeTrucTuyenOBJ();
                    data = BaoCao_dl.ThongKeTrucTuyenIDCT_v2(userinfo.ID_QLLH, userinfo.ID_QuanLy, idnhom);

                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getdanhsachnhanvientheotrangthai")]
        public HttpResponseMessage getdanhsachnhanvientheotrangthai([FromUri] string trangthai)
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
                    List<NhanVien> data = new List<NhanVien>();
                    data = NhanVien_dl.DSNhanVienTheoTrangThai(userinfo.ID_QLLH, trangthai, userinfo.ID_QuanLy);

                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getlistnhanvien")]
        public HttpResponseMessage getlistnhanvien([FromUri] int idnhom, int loctrangthai)
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
                    List<gpsnhanvien> data = new List<gpsnhanvien>();

                    data = LoTrinhDB.GetViTriTatCaNV(userinfo.ID_QLLH, userinfo.ID_QuanLy, idnhom, loctrangthai);

                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getvitrihientainhanvien")]
        public HttpResponseMessage getvitrihientainhanvien([FromUri] int idnhanvien)
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
                    NhanVien nv = new NhanVien();
                    if ((nv = NhanVien_dl.ChiTietNhanVienTheoIDNV(idnhanvien)) == null)
                    {
                        nv = new NhanVien();
                        nv.IDNV = 0;
                        nv.TenDayDu = "{{bindTextI18n('label_khongxacdinh')}}";
                        nv.TrucTuyen = 2;
                    }

                    CheckInOut_dl ckdl = new CheckInOut_dl();
                    CheckIn chk = ckdl.GetCheckInMoiNhat_TheoIDNV_Latest(idnhanvien);

                    gpsnhanvien data = new gpsnhanvien();
                    data = LoTrinhDB.GetViTriHienTaiNV(idnhanvien);

                    string img = ((nv.AnhDaiDien_thumbnail_small != null && nv.AnhDaiDien_thumbnail_small != "") ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + nv.AnhDaiDien_thumbnail_small : "assets/img/noimage.png");
                    string text = "";
                    text += "<div>" +
                            "<h5 id='heading' class='heading'> {{bindTextI18n('label_nhanvien')}}: " +
                                "<a href = '/#!/editnhanvien/" + nv.IDNV.ToString() + "'> " +
                                nv.TenDayDu +
                                "</a>" +
                            "</h5>" +
                            "<p> {{bindTextI18n('label_tendangnhap')}}: " + nv.TenDangNhap + "</p>" +
                            "<p> {{bindTextI18n('label_trangthai')}}: " + ((nv.TrucTuyen == 1) ? "{{bindTextI18n('label_dangtructuyen')}}" : (nv.TrucTuyen == 2) ? "{{bindTextI18n('label_mattinhieu')}}" : "{{bindTextI18n('label_chuadangnhap')}}") + "</p>" +
                            "<p> {{bindTextI18n('label_thoigianhoatdongcuoicung')}}: " + nv.ThoiGianHoatDong.ToString("dd/MM/yyyy HH:mm:ss") + "</p>" +
                            "<p> {{bindTextI18n('label_thoigianguitoadocuoicung')}}: " + nv.ThoiGianGuiBanTinCuoiCung.ToString("dd/MM/yyyy HH:mm:ss") + "</p>" +
                            "<p> {{bindTextI18n('label_tinhtrangpin')}}: " + nv.TinhTrangPin + "</p>";

                    if (chk != null && chk.ID_CheckIn > 0)
                    {
                        text += "<p> {{bindTextI18n('label_dangvaodiemtaikhachhang')}}: " + chk.TenKhachHang + "</p>" +
                            "<p> {{bindTextI18n('label_diachi')}}: " + chk.DiaChi + "</p>" +
                            "<p> {{bindTextI18n('label_thoigiantaidiem')}}: " + chk.ThoiGianTaiDiem + "</p>";
                    }

                    text += "<p> {{bindTextI18n('label_anhdaidien')}}: " +
                        "<div class='text-center' style=\"overflow-x:auto; \"><img style=\"width:140px; height:140px\" src = '" + img + "' /></div></p>";

                    text += "</div>";

                    response = Request.CreateResponse(HttpStatusCode.OK, new { toado = data, info = text });
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getchitietnhanvien")]
        public HttpResponseMessage getchitietnhanvien([FromUri] int idnhanvien)
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
                    NhanVien nv = new NhanVien();
                    if ((nv = NhanVien_dl.ChiTietNhanVienTheoIDNV(idnhanvien)) == null)
                    {
                        nv.TenDayDu = "{{bindTextI18n('label_khongxacdinh')}}";
                    }

                    CheckInOut_dl ckdl = new CheckInOut_dl();
                    CheckIn chk = ckdl.GetCheckInMoiNhat_TheoIDNV_Latest(nv.IDNV);

                    string img = ((nv.AnhDaiDien_thumbnail_small != null && nv.AnhDaiDien_thumbnail_small != "") ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + nv.AnhDaiDien_thumbnail_small : "assets/img/noimage.png");
                    string text = "";
                    text += "<div>" +
                            "<h5 id='heading' class='heading'> {{bindTextI18n('label_nhanvien')}}: " +
                                "<a href = '/#!/editnhanvien/" + nv.IDNV.ToString() + "'> " +
                                nv.TenDayDu +
                                "</a>" +
                            "</h5>" +
                            "<p> {{bindTextI18n('label_tendangnhap')}}: " + nv.TenDangNhap + "</p>" +
                            "<p> {{bindTextI18n('label_trangthai')}}: " + ((nv.TrucTuyen == 1) ? "{{bindTextI18n('label_dangtructuyen')}}" : (nv.TrucTuyen == 2) ? "{{bindTextI18n('label_mattinhieu')}}" : "{{bindTextI18n('label_chuadangnhap')}}") + "</p>" +
                            "<p> {{bindTextI18n('label_thoigianhoatdongcuoicung')}}: " + nv.ThoiGianHoatDong.ToString("dd/MM/yyyy HH:mm:ss") + "</p>" +
                            "<p> {{bindTextI18n('label_thoigianguitoadocuoicung')}}: " + nv.ThoiGianGuiBanTinCuoiCung.ToString("dd/MM/yyyy HH:mm:ss") + "</p>" +
                            "<p> {{bindTextI18n('label_tinhtrangpin')}}: " + nv.TinhTrangPin + "</p>";

                    if (chk != null && chk.ID_CheckIn > 0)
                    {
                        text += "<p> {{bindTextI18n('label_dangvaodiemkhachhang')}}: " + chk.TenKhachHang + "</p>" +
                            "<p> {{bindTextI18n('label_diachi')}}: " + chk.DiaChi + "</p>" +
                            "<p> {{bindTextI18n('label_thoigiantaidiem')}}: " + chk.ThoiGianTaiDiem + "</p>";
                    }



                    text += "<p> {{bindTextI18n('label_anhdaidien')}}: " +
                        "<div class='text-center' style=\"overflow-x:auto; \"><img style=\"width:140px; height:140px\" src = '" + img + "' /></div></p>";

                    text += "</div>";


                    response = Request.CreateResponse(HttpStatusCode.OK, text);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        #region lotrinhdichuyen
        [HttpGet]
        [Route("chitietdungdo")]
        public HttpResponseMessage chitietdungdo([FromUri] int idnhanvien, string kinhdostr, string vidostr, string from, string to)
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    double kinhdo = (kinhdostr != null) ? double.Parse(kinhdostr.ToString()) : 0;
                    double vido = (vidostr != null) ? double.Parse(vidostr.ToString()) : 0;
                    DateTime thoigianbatdau = DateTime.ParseExact(from, new string[] { "dd/MM/yyyy HH:mm:ss" }, null, DateTimeStyles.None);
                    DateTime thoigianketthuc = DateTime.ParseExact(to, new string[] { "dd/MM/yyyy HH:mm:ss" }, null, DateTimeStyles.None);
                    string KHGanNhat = (lang == "vi") ? "Không có" : "Not available";

                    if (kinhdo > 0)
                    {
                        SqlParameter[] par = new SqlParameter[]{
                            new SqlParameter("@kinhdo", kinhdo),
                            new SqlParameter("@vido", vido),
                            new SqlParameter("@idnhanvien",  idnhanvien)
                        };
                        SqlDataHelper helper = new SqlDataHelper();
                        DataSet dsKH = helper.ExecuteDataSet("sp_App_VaoDiemCuaHangGanNhat_KhongTheoKeHoach_GomKhachHangCuaNhanVienTrongNhom", par);
                        if (dsKH.Tables.Count > 0)
                        {
                            DataTable dtKH = dsKH.Tables[0];
                            if (dtKH.Rows.Count > 0)
                            {
                                KHGanNhat = dtKH.Rows[0]["TenKhachHang"].ToString() + "(" + dtKH.Rows[0]["DiaChi"].ToString() + ")";
                            }
                        }
                    }

                    TimeSpan ts = thoigianketthuc - thoigianbatdau;
                    string text = "";

                    string diemdungtrong = (lang == "vi") ? "Không xác định" : "Not available";
                    LoTrinhData loTrinhData = new LoTrinhData();
                    string diemdung = loTrinhData.GetDiaDiemTheoToaDo(vido, kinhdo);

                    text += "<h6 class='color-infor'>" + ((diemdung == "") ? diemdungtrong : diemdung) + "</h6>";

                    if ((lang == "vi"))
                        text += "<li>Thời gian bắt đầu dừng: " + from + "</li>"
                            + "<li>Thời gian kết thúc dừng: " + thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") + " </li>"
                            + "<li>Thời gian dừng: " + ts.ToString(@"hh\:mm\:ss") + "</li> "
                            + "<li>Khách hàng gần nhất: " + KHGanNhat + "</li> ";
                    else
                        text += "<li>Time of starting stop: " + from + "</li>"
                            + "<li>Time of end stop: " + thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") + " </li>"
                            + "<li>Total stop time: " + ts.ToString(@"hh\:mm\:ss") + "</li> "
                            + "<li>Nearest customer: " + KHGanNhat + "</li> ";

                    response = Request.CreateResponse(HttpStatusCode.OK, text);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        [HttpGet]
        [Route("getdatalotrinhnhanvien")]
        public HttpResponseMessage getdatalotrinhnhanvien([FromUri] int idnhanvien, int loaiLoTrinh, int khongnoidiem, DateTime tungayF, DateTime denngayF)
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
                    int minkhoangcach = 100;
                    int thamsohienthi = 1;
                    int thoigianlaybantin = 20000;
                    double tocDoHopLeToiDa = 300;

                    OBJdata OBJ = new OBJdata();
                    OBJ.status = false;
                    OBJ.msg = "Không có dữ liệu";
                    OBJ.datalotrinh = null;
                    OBJ.databanglotrinh = null;

                    if (tungayF.Year > 1900)
                    {
                        if ((denngayF - tungayF).TotalDays > 5)
                        {
                            OBJ.msg = "label_khoangcachgiua2ngaykhongduocqua5ngay";

                        }
                        else if (tungayF > denngayF)
                        {
                            OBJ.msg = "label_khoangcachtungaykhongduoclonhondenngay";

                        }
                        else
                        {
                            KeHoachDiChuyen_dl khdc = new KeHoachDiChuyen_dl();
                            List<KeHoachDiChuyenObj> lstKeHoach = khdc.GetKeHoachTheoNhanVien_Moi(idnhanvien, tungayF, denngayF, userinfo.ID_QLLH, userinfo.ID_QuanLy);

                            DataTable dt = LoTrinhDB.LichSuDiChuyenTheoNhanVien_Online_Offline(idnhanvien, tungayF, denngayF, userinfo.ID_QLLH, userinfo.ID_QuanLy);

                            List<lotrinhmattinhieuOBJ> lotrinhmattinhieu_new = new List<lotrinhmattinhieuOBJ>();
                            List<Point> lpoint = new List<Point>();
                            List<Point> lpoint_Offline = new List<Point>();
                            int x = 0;
                            int j = 0;
                            int dem = 0;

                            DataTable dtLoTrinh = new DataTable();
                            dtLoTrinh.Columns.Add("STT");
                            dtLoTrinh.Columns.Add("Nhan vien");
                            dtLoTrinh.Columns.Add("Thoi gian");
                            dtLoTrinh.Columns.Add("Kinh do");
                            dtLoTrinh.Columns.Add("Vi do");

                            string strDataPoint = "";
                            List<lotrinhOBJ> lotrinh_napvaoduong = new List<lotrinhOBJ>();
                            List<string> lstDataPoint = new List<string>();
                            List<string> lstDataPoint_Offline = new List<string>();
                            List<Point> lpointDiChuyen = new List<Point>();
                            List<Point> lpointDiChuyen_Offline = new List<Point>();
                            List<Point> lpointVaoRaDiem = new List<Point>();
                            List<lotrinhOBJ> lotrinhdt = new List<lotrinhOBJ>();
                            List<lotrinhmattinhieuOBJ> lotrinhmattinhieu = new List<lotrinhmattinhieuOBJ>();
                            List<banglotrinhOBJ> banglotrinhdt = new List<banglotrinhOBJ>();
                            List<lotrinhtrinhvaodiemOBJ> lotrinhtrinhvaodiem = new List<lotrinhtrinhvaodiemOBJ>();
                            List<lotrinhtrinhvaodiemOBJ> lotrinhtrinhradiem = new List<lotrinhtrinhvaodiemOBJ>();
                            foreach (DataRow i in dt.Rows)
                            {

                                if (x % thamsohienthi == 1 || thamsohienthi == 1 || khongnoidiem == 0)
                                {
                                    Point.PointType t = Point.PointType.Move;

                                    if (i["ghichu"].ToString() == "Vào điểm")
                                    {
                                        t = Point.PointType.CheckIn;
                                    }
                                    else if (i["ghichu"].ToString() == "Ra điểm")
                                    {
                                        t = Point.PointType.CheckOut;
                                    }
                                    else if (i["ghichu"].ToString() == "Ngoại tuyến")
                                    {
                                        t = Point.PointType.Offline;
                                    }
                                    strDataPoint += i["vido"].ToString() + "," + i["kinhdo"].ToString();
                                    Point p = null;
                                    try
                                    {
                                        p = new Point { tinhtrangpin = i["tinhtrangpin"].ToString(), tennhanvien = i["nhanvien"].ToString(), idnhanvien = int.Parse(i["idnhanvien"].ToString()), Lat = double.Parse(i["vido"].ToString()), Lng = double.Parse(i["kinhdo"].ToString()), thoigianbantin = DateTime.Parse(i["thoigian"].ToString()), Time = DateTime.Parse(i["thoigian"].ToString()), Type = t, OrigIndex = j, accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = i["tenkhachhang"].ToString(), diachikhachhang = i["diachikhachhang"].ToString(), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()) : new DateTime(1900, 01, 01) };

                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                    }
                                    lpoint.Add(p);
                                    j++;
                                    dem++;
                                    if (p.Type == Point.PointType.Move || p.Type == Point.PointType.Offline)
                                    {
                                        lpointDiChuyen.Add(p);
                                    }
                                    else if (p.Type == Point.PointType.CheckIn || p.Type == Point.PointType.CheckOut)
                                    {
                                        string gc = "label_tructuyen";

                                        if (p.Type == Point.PointType.CheckIn)
                                        {
                                            gc = "header_vaodiem";
                                            lotrinhtrinhvaodiem.Add(new lotrinhtrinhvaodiemOBJ { idnhanvien = int.Parse(i["idnhanvien"].ToString()), vido = double.Parse(i["vido"].ToString()), kinhdo = double.Parse(i["kinhdo"].ToString()), ghichu = gc, thoigian = DateTime.Parse(i["thoigian"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = (i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0 ? "Vào điểm tự do" : i["tenkhachhang"].ToString()), diachikhachhang = ((i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0) ? "Vào điểm tự do" : i["diachikhachhang"].ToString()), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()).ToString("HH:mm:ss") : new DateTime(1900, 01, 01).ToString("HH:mm:ss"), thoigianvaodiem = DateTime.Parse(i["thoigianvaodiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), thoigianradiem = (i["thoigianradiem"].ToString() == "" || DateTime.Parse(i["thoigianradiem"].ToString()).Year == 1900) ? "chưa ra điểm" : DateTime.Parse(i["thoigianradiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });
                                        }
                                        else if (p.Type == Point.PointType.CheckOut)
                                        {
                                            gc = "label_radiem";
                                            lotrinhtrinhradiem.Add(new lotrinhtrinhvaodiemOBJ { idnhanvien = int.Parse(i["idnhanvien"].ToString()), vido = double.Parse(i["vido"].ToString()), kinhdo = double.Parse(i["kinhdo"].ToString()), ghichu = gc, thoigian = DateTime.Parse(i["thoigian"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = (i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0 ? "Vào điểm tự do" : i["tenkhachhang"].ToString()), diachikhachhang = ((i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0) ? "Vào điểm tự do" : i["diachikhachhang"].ToString()), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()).ToString("HH:mm:ss") : new DateTime(1900, 01, 01).ToString("HH:mm:ss"), thoigianvaodiem = DateTime.Parse(i["thoigianvaodiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), thoigianradiem = (i["thoigianradiem"].ToString() == "" || DateTime.Parse(i["thoigianradiem"].ToString()).Year == 1900) ? "chưa ra điểm" : DateTime.Parse(i["thoigianradiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });

                                        }

                                        lpointVaoRaDiem.Add(p);
                                    }

                                    try
                                    {
                                        if (dem == 100)
                                        {
                                            lstDataPoint.Add(strDataPoint);
                                            strDataPoint = "";
                                            dem = 0;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                        throw ex;
                                    }

                                    if (j != dt.Rows.Count && dem != 0)
                                    {
                                        strDataPoint += "|";
                                    }
                                }
                                x++;
                            }
                            if (strDataPoint != "")
                                lstDataPoint.Add(strDataPoint);

                            if (loaiLoTrinh > 0)
                            {
                                lotrinh_napvaoduong = NapVaoDuong(lstDataPoint);
                            }

                            List<Point> filteredPoints = null;
                            List<Point> filteredPoints_Offline = null;
                            if (loaiLoTrinh == -1)
                            {
                                //khong loai diem trung
                                filteredPoints = lpointDiChuyen;
                                filteredPoints_Offline = lpointDiChuyen_Offline;
                            }
                            else
                            {
                                //loc lo trinh
                                LoTrinhGPSFilter filter = new LoTrinhGPSFilter(lpointDiChuyen);
                                LoTrinhGPSFilter filter_Offline = new LoTrinhGPSFilter(lpointDiChuyen_Offline);
                                filteredPoints = filter.FilterAnhTrungNangCap(lpointDiChuyen, minkhoangcach, thoigianlaybantin, tocDoHopLeToiDa);
                                filteredPoints_Offline = filter_Offline.FilterAnhTrungNangCap(lpointDiChuyen_Offline, minkhoangcach, thoigianlaybantin, tocDoHopLeToiDa);

                            }

                            List<LichSuDiChuyenOBJ> ldc = new List<LichSuDiChuyenOBJ>();
                            foreach (Point p in filteredPoints)
                            {
                                string gc = "label_tructuyen";

                                if (p.Type == Point.PointType.CheckIn)
                                {
                                    gc = "header_vaodiem";
                                }
                                else if (p.Type == Point.PointType.CheckOut)
                                {
                                    gc = "label_radiem";
                                }
                                else if (p.Type == Point.PointType.Offline)
                                {
                                    gc = "label_ngoaituyen";
                                }
                                else if (p.Type == Point.PointType.Stop)
                                {
                                    gc = "label_dungdo";
                                }
                                ldc.Add(new LichSuDiChuyenOBJ { nhanvien = p.tennhanvien, tinhtrangpin = p.tinhtrangpin, idnhanvien = p.idnhanvien, kinhdo = p.Lng, vido = p.Lat, thoigian = p.thoigianbantin, ghichu = gc, accuracy = p.accuracy, speed = p.speed, thoigianketthuc = p.thoigianketthuc });

                            }

                            foreach (Point p in lpointVaoRaDiem)
                            {
                                string gc = "label_tructuyen";

                                if (p.Type == Point.PointType.CheckIn)
                                {
                                    gc = "header_vaodiem";
                                }
                                else if (p.Type == Point.PointType.CheckOut)
                                {
                                    gc = "label_radiem";
                                }
                                else if (p.Type == Point.PointType.Offline)
                                {
                                    gc = "label_ngoaituyen";
                                }
                                ldc.Add(new LichSuDiChuyenOBJ { nhanvien = p.tennhanvien, tinhtrangpin = p.tinhtrangpin, idnhanvien = p.idnhanvien, kinhdo = p.Lng, vido = p.Lat, thoigian = p.thoigianbantin, ghichu = gc, accuracy = p.accuracy, speed = p.speed, thoigianketthuc = p.thoigianketthuc });

                            }


                            List<LichSuDiChuyenOBJ> newList = ldc.OrderBy(o => o.thoigian).ToList();
                            int k = 0;
                            foreach (LichSuDiChuyenOBJ item in newList)
                            {
                                k++;
                                if (!item.kinhdo.ToString().Trim().Equals("nan") && !item.vido.ToString().ToLower().Equals("nan"))
                                {
                                    lotrinhOBJ lt = new lotrinhOBJ { tennhanvien = item.nhanvien, tinhtrangpin = item.tinhtrangpin, idnhanvien = item.idnhanvien, thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, kinhdo = item.kinhdo, vido = item.vido, accuracy = item.accuracy, speed = item.speed, thoigianketthuc = item.thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") };
                                    lotrinhdt.Add(lt);

                                    if (k == 1)
                                    {
                                        lotrinhOBJ firt = new lotrinhOBJ { tennhanvien = item.nhanvien, tinhtrangpin = item.tinhtrangpin, idnhanvien = item.idnhanvien, thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, kinhdo = item.kinhdo, vido = item.vido, accuracy = item.accuracy, speed = item.speed, thoigianketthuc = item.thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") };
                                        OBJ.LoTrinhDauTien = firt;
                                    }
                                    if (k == newList.Count)
                                    {
                                        lotrinhOBJ last = new lotrinhOBJ { tennhanvien = item.nhanvien, tinhtrangpin = item.tinhtrangpin, idnhanvien = item.idnhanvien, thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, kinhdo = item.kinhdo, vido = item.vido, accuracy = item.accuracy, speed = item.speed, thoigianketthuc = item.thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") };
                                        OBJ.LoTrinhCuoiCung = last;
                                    }

                                    DataRow dr = dtLoTrinh.NewRow();
                                    dr["STT"] = k;
                                    dr["Nhan vien"] = item.nhanvien;
                                    dr["Thoi gian"] = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss");
                                    dr["Kinh do"] = item.kinhdo;
                                    dr["Vi do"] = item.vido;
                                    dtLoTrinh.Rows.Add(dr);


                                    banglotrinhdt.Add(new banglotrinhOBJ { tenhanvien = item.nhanvien, tinhtrangpin = item.tinhtrangpin, idnhanvien = item.idnhanvien, thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, accuracy = item.accuracy, kinhdo = item.kinhdo, vido = item.vido, speed = item.speed, thoigianketthuc = item.thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") });
                                }
                            }

                            for (int i = 1; i < ldc.Count; i++)
                            {
                                if (!ldc[i - 1].kinhdo.ToString().Trim().Equals("nan")
                                    && !ldc[i - 1].vido.ToString().ToLower().Equals("nan")
                                    && !ldc[i].kinhdo.ToString().ToLower().Equals("nan")
                                    && !ldc[i].vido.ToString().ToLower().Equals("nan")
                                    )
                                {
                                    if (ldc[i].ghichu == "Ngoại tuyến" || ldc[i].ghichu == "label_ngoaituyen")
                                    {
                                        try
                                        {
                                            lotrinhmattinhieu.Add(new lotrinhmattinhieuOBJ
                                            {
                                                diemdau = new lotrinhOBJ { kinhdo = ldc[i - 1].kinhdo, vido = ldc[i - 1].vido },
                                                diemcuoi = new lotrinhOBJ { kinhdo = ldc[i].kinhdo, vido = ldc[i].vido }
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                        }
                                    }
                                }
                            }

                            OBJ.datalotrinh = lotrinhdt;
                            OBJ.datavaodiem = lotrinhtrinhvaodiem;
                            OBJ.dataradiem = lotrinhtrinhradiem;
                            OBJ.datalotrinh_suydien = lotrinh_napvaoduong;
                            OBJ.databanglotrinh = banglotrinhdt;
                            OBJ.datamattinhieu = lotrinhmattinhieu;
                            OBJ.dataKeHoachDiChuyen = lstKeHoach;
                            OBJ.status = true;
                            OBJ.msg = "thành công";
                        }
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getdatalotrinhnhanvienphienlamviec")]
        public HttpResponseMessage getdatalotrinhnhanvienphienlamviec([FromUri] int idnhanvien, int loaiLoTrinh, int khongnoidiem, DateTime tungayF, DateTime denngayF)
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
                    int minkhoangcach = 100;
                    int thamsohienthi = 1;
                    int thoigianlaybantin = 20000;
                    double tocDoHopLeToiDa = 300;

                    OBJdataTheoPhien OBJ = new OBJdataTheoPhien();
                    OBJ.status = false;
                    OBJ.msg = "Không có dữ liệu";
                    OBJ.datalotrinh = null;
                    OBJ.databanglotrinh = null;

                    if (tungayF.Year > 1900)
                    {
                        if ((denngayF - tungayF).TotalDays > 5)
                        {
                            OBJ.msg = "Khoảng cách giữa 2 ngày không được vượt quá 5 ngày";

                        }
                        else if (tungayF > denngayF)
                        {
                            OBJ.msg = "Khoảng thời gian từ ngày không được phép lớn hơn đến ngày, vui lòng thử lại";

                        }
                        else
                        {
                            DataSet ds = NhanVien_dl.PhienLamViec(userinfo.ID_QLLH, idnhanvien, tungayF, denngayF, userinfo.ID_QuanLy);
                            OBJ.datalotrinh_suydien = new List<List<lotrinhOBJ>>();

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DateTime dtThoiGianDangNhap = DateTime.Parse(dr["thoigiandangnhap"].ToString());
                                DateTime dtThoiGianDangXuat = dr["thoigiandangxuatphien"].ToString() != "" ? DateTime.Parse(dr["thoigiandangxuatphien"].ToString()) : (dr["thoigiandangnhaptieptheo"].ToString() != "" ? DateTime.Parse(dr["thoigiandangnhaptieptheo"].ToString()) : DateTime.Now);
                                DataTable dt_New = LoTrinhDB.LichSuDiChuyenTheoNhanVien_Online_Offline(idnhanvien, dtThoiGianDangNhap, dtThoiGianDangXuat, userinfo.ID_QLLH, userinfo.ID_QuanLy);

                                int j = 0;
                                int dem = 0;


                                string strDataPoint = "";
                                List<string> lstDataPoint = new List<string>();
                                foreach (DataRow i in dt_New.Rows)
                                {
                                    strDataPoint += i["vido"].ToString() + "," + i["kinhdo"].ToString();
                                    j++;
                                    dem++;
                                    try
                                    {
                                        if (dem == 100)
                                        {
                                            lstDataPoint.Add(strDataPoint);
                                            strDataPoint = "";
                                            dem = 0;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                        throw ex;
                                    }

                                    if (j != dt_New.Rows.Count && dem != 0)
                                    {
                                        strDataPoint += "|";
                                    }
                                }
                                if (strDataPoint != "")
                                    lstDataPoint.Add(strDataPoint);
                                List<lotrinhOBJ> lotrinh_napvaoduong = new List<lotrinhOBJ>();
                                if (loaiLoTrinh > 0)
                                {
                                    lotrinh_napvaoduong = NapVaoDuong(lstDataPoint);
                                }

                                if (lotrinh_napvaoduong.Count > 0)
                                    OBJ.datalotrinh_suydien.Add(lotrinh_napvaoduong);
                            }
                            DataTable dt = LoTrinhDB.LichSuDiChuyenTheoNhanVien_Online_Offline(idnhanvien, tungayF, denngayF, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                            List<lotrinhmattinhieuOBJ> lotrinhmattinhieu_new = new List<lotrinhmattinhieuOBJ>();
                            List<Point> lpoint = new List<Point>();
                            List<Point> lpoint_Offline = new List<Point>();



                            List<string> lstDataPoint_Offline = new List<string>();
                            List<Point> lpointDiChuyen = new List<Point>();
                            List<Point> lpointDiChuyen_Offline = new List<Point>();
                            List<Point> lpointVaoRaDiem = new List<Point>();
                            List<lotrinhOBJ> lotrinhdt = new List<lotrinhOBJ>();
                            List<lotrinhmattinhieuOBJ> lotrinhmattinhieu = new List<lotrinhmattinhieuOBJ>();
                            List<banglotrinhOBJ> banglotrinhdt = new List<banglotrinhOBJ>();
                            List<lotrinhtrinhvaodiemOBJ> lotrinhtrinhvaodiem = new List<lotrinhtrinhvaodiemOBJ>();
                            List<lotrinhtrinhvaodiemOBJ> lotrinhtrinhradiem = new List<lotrinhtrinhvaodiemOBJ>();
                            int k = 0;
                            foreach (DataRow i in dt.Rows)
                            {
                                int x = 0;

                                if (x % thamsohienthi == 1 || thamsohienthi == 1 || khongnoidiem == 0)
                                {
                                    k++;
                                    Point.PointType t = Point.PointType.Move;

                                    if (i["ghichu"].ToString() == "Vào điểm")
                                    {
                                        t = Point.PointType.CheckIn;
                                    }
                                    else if (i["ghichu"].ToString() == "Ra điểm")
                                    {
                                        t = Point.PointType.CheckOut;
                                    }
                                    else if (i["ghichu"].ToString() == "Ngoại tuyến")
                                    {
                                        t = Point.PointType.Offline;
                                    }

                                    Point p = null;
                                    try
                                    {
                                        p = new Point { Lat = double.Parse(i["vido"].ToString()), Lng = double.Parse(i["kinhdo"].ToString()), Time = DateTime.Parse(i["thoigian"].ToString()), Type = t, OrigIndex = k, accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = i["tenkhachhang"].ToString(), diachikhachhang = i["diachikhachhang"].ToString(), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()) : new DateTime(1900, 01, 01) };

                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                    }
                                    lpoint.Add(p);

                                    if (p.Type == Point.PointType.Move || p.Type == Point.PointType.Offline)
                                    {
                                        lpointDiChuyen.Add(p);
                                    }
                                    else if (p.Type == Point.PointType.CheckIn || p.Type == Point.PointType.CheckOut)
                                    {
                                        string gc = "Trực tuyến";

                                        if (p.Type == Point.PointType.CheckIn)
                                        {
                                            gc = "Vào điểm";
                                            lotrinhtrinhvaodiem.Add(new lotrinhtrinhvaodiemOBJ { vido = double.Parse(i["vido"].ToString()), kinhdo = double.Parse(i["kinhdo"].ToString()), ghichu = gc, thoigian = DateTime.Parse(i["thoigian"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = (i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0 ? "Vào điểm tự do" : i["tenkhachhang"].ToString()), diachikhachhang = ((i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0) ? "Vào điểm tự do" : i["diachikhachhang"].ToString()), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()).ToString("HH:mm:ss") : new DateTime(1900, 01, 01).ToString("HH:mm:ss"), thoigianvaodiem = DateTime.Parse(i["thoigianvaodiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), thoigianradiem = (i["thoigianradiem"].ToString() == "" || DateTime.Parse(i["thoigianradiem"].ToString()).Year == 1900) ? "chưa ra điểm" : DateTime.Parse(i["thoigianradiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });

                                        }
                                        else if (p.Type == Point.PointType.CheckOut)
                                        {
                                            gc = "Ra điểm";
                                            lotrinhtrinhradiem.Add(new lotrinhtrinhvaodiemOBJ { vido = double.Parse(i["vido"].ToString()), kinhdo = double.Parse(i["kinhdo"].ToString()), ghichu = gc, thoigian = DateTime.Parse(i["thoigian"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = (i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0 ? "Vào điểm tự do" : i["tenkhachhang"].ToString()), diachikhachhang = ((i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0) ? "Vào điểm tự do" : i["diachikhachhang"].ToString()), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()).ToString("HH:mm:ss") : new DateTime(1900, 01, 01).ToString("HH:mm:ss"), thoigianvaodiem = DateTime.Parse(i["thoigianvaodiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), thoigianradiem = (i["thoigianradiem"].ToString() == "" || DateTime.Parse(i["thoigianradiem"].ToString()).Year == 1900) ? "chưa ra điểm" : DateTime.Parse(i["thoigianradiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });

                                        }

                                        lpointVaoRaDiem.Add(p);
                                    }
                                }
                                x++;
                            }

                            List<Point> filteredPoints = null;
                            List<Point> filteredPoints_Offline = null;
                            if (loaiLoTrinh == -1)
                            {
                                //khong loai diem trung
                                filteredPoints = lpointDiChuyen;
                                filteredPoints_Offline = lpointDiChuyen_Offline;
                            }
                            else
                            {
                                //loc lo trinh
                                LoTrinhGPSFilter filter = new LoTrinhGPSFilter(lpointDiChuyen);
                                LoTrinhGPSFilter filter_Offline = new LoTrinhGPSFilter(lpointDiChuyen_Offline);
                                filteredPoints = filter.FilterAnhTrungNangCap(lpointDiChuyen, minkhoangcach, thoigianlaybantin, tocDoHopLeToiDa);
                                filteredPoints_Offline = filter_Offline.FilterAnhTrungNangCap(lpointDiChuyen_Offline, minkhoangcach, thoigianlaybantin, tocDoHopLeToiDa);

                            }

                            List<LichSuDiChuyenOBJ> ldc = new List<LichSuDiChuyenOBJ>();
                            foreach (Point p in filteredPoints)
                            {
                                string gc = "Trực tuyến";

                                if (p.Type == Point.PointType.CheckIn)
                                {
                                    gc = "Vào điểm";
                                }
                                else if (p.Type == Point.PointType.CheckOut)
                                {
                                    gc = "Ra điểm";
                                }
                                else if (p.Type == Point.PointType.Offline)
                                {
                                    gc = "Ngoại tuyến";
                                }
                                try
                                {
                                    ldc.Add(new LichSuDiChuyenOBJ { kinhdo = p.Lng, vido = p.Lat, thoigian = DateTime.Parse(dt.Rows[p.OrigIndex - 1]["thoigian"].ToString()), ghichu = gc, accuracy = dt.Rows[p.OrigIndex - 1]["accuracy"].ToString() != "" ? double.Parse(dt.Rows[p.OrigIndex - 1]["accuracy"].ToString()) : 0 });

                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                            }

                            foreach (Point p in lpointVaoRaDiem)
                            {
                                string gc = "Trực tuyến";

                                if (p.Type == Point.PointType.CheckIn)
                                {
                                    gc = "Vào điểm";
                                }
                                else if (p.Type == Point.PointType.CheckOut)
                                {
                                    gc = "Ra điểm";
                                }
                                else if (p.Type == Point.PointType.Offline)
                                {
                                    gc = "Ngoại tuyến";
                                }
                                ldc.Add(new LichSuDiChuyenOBJ { kinhdo = p.Lng, vido = p.Lat, thoigian = DateTime.Parse(dt.Rows[p.OrigIndex - 1]["thoigian"].ToString()), ghichu = gc, accuracy = dt.Rows[p.OrigIndex - 1]["accuracy"].ToString() != "" ? double.Parse(dt.Rows[p.OrigIndex - 1]["accuracy"].ToString()) : 0 });

                            }

                            List<LichSuDiChuyenOBJ> newList = ldc.OrderBy(o => o.thoigian).ToList();
                            int m = 0;
                            foreach (LichSuDiChuyenOBJ item in newList)
                            {
                                m++;
                                if (!item.kinhdo.ToString().Trim().Equals("nan") && !item.vido.ToString().ToLower().Equals("nan"))
                                {
                                    lotrinhOBJ lt = new lotrinhOBJ { thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, kinhdo = item.kinhdo, vido = item.vido, accuracy = item.accuracy };
                                    lotrinhdt.Add(lt);
                                    banglotrinhdt.Add(new banglotrinhOBJ { thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, accuracy = item.accuracy, kinhdo = item.kinhdo, vido = item.vido });

                                    if (m == 1)
                                    {
                                        lotrinhOBJ firt = new lotrinhOBJ { thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, kinhdo = item.kinhdo, vido = item.vido, accuracy = item.accuracy };
                                        OBJ.LoTrinhDauTien = firt;
                                    }
                                    if (m == newList.Count)
                                    {
                                        lotrinhOBJ last = new lotrinhOBJ { thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, kinhdo = item.kinhdo, vido = item.vido, accuracy = item.accuracy };
                                        OBJ.LoTrinhCuoiCung = last;
                                    }

                                }
                            }

                            for (int i = 1; i < ldc.Count; i++)
                            {
                                if (!ldc[i - 1].kinhdo.ToString().Trim().Equals("nan")
                                    && !ldc[i - 1].vido.ToString().ToLower().Equals("nan")
                                    && !ldc[i].kinhdo.ToString().ToLower().Equals("nan")
                                    && !ldc[i].vido.ToString().ToLower().Equals("nan")
                                    )
                                {
                                    if (ldc[i].ghichu == "Ngoại tuyến")
                                    {
                                        try
                                        {
                                            lotrinhmattinhieu.Add(new lotrinhmattinhieuOBJ
                                            {
                                                diemdau = new lotrinhOBJ { kinhdo = ldc[i - 1].kinhdo, vido = ldc[i - 1].vido },
                                                diemcuoi = new lotrinhOBJ { kinhdo = ldc[i].kinhdo, vido = ldc[i].vido }
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                        }
                                    }
                                }
                            }

                            OBJ.datalotrinh = lotrinhdt;
                            OBJ.datavaodiem = lotrinhtrinhvaodiem;
                            OBJ.dataradiem = lotrinhtrinhradiem;

                            OBJ.databanglotrinh = banglotrinhdt;
                            OBJ.datamattinhieu = lotrinhmattinhieu;
                            OBJ.status = true;
                            OBJ.msg = "thành công";
                        }
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        List<lotrinhOBJ> NapVaoDuong(List<string> data)
        {
            List<lotrinhOBJ> lotrinhdt = new List<lotrinhOBJ>();

            try
            {
                foreach (string s in data)
                {
                    string strLinkServer = "https://roads.googleapis.com/v1/snapToRoads";
                    Hashtable htParam = new Hashtable();
                    htParam.Add("interpolate", "true");
                    htParam.Add("path", s);
                    //htParam.Add("path", "-35.27801,149.12958|-35.28032,149.12907|-35.28099,149.12929|-35.28144,149.12984|-35.28194,149.13003|-35.28282,149.12956|-35.28302,149.12881|-35.28473,149.12836");
                    htParam.Add("key", "AIzaSyCtSL8GLck0Y_GyuvvjlFn1-tYyKQ_A6WY");
                    Utils ut = new Utils();
                    string sJsonKetQua = ut.CallHTTP(strLinkServer, htParam);
                    var results = JsonConvert.DeserializeObject<dynamic>(sJsonKetQua);
                    if (results.snappedPoints != null)
                    {
                        for (int i = 0; i < results.snappedPoints.Count; i++)
                        {
                            lotrinhdt.Add(new lotrinhOBJ { kinhdo = results.snappedPoints[i].location.longitude, vido = results.snappedPoints[i].location.latitude });
                        }
                    }
                    else
                    {
                        string[] lsttoado = s.Split(new string[] { "|" }, StringSplitOptions.None);
                        foreach (string str in lsttoado)
                        {
                            string[] sToaDO = str.Split(',');
                            lotrinhdt.Add(new lotrinhOBJ { kinhdo = double.Parse(sToaDO[1]), vido = double.Parse(sToaDO[0]) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                //throw new Exception("Đã xảy ra lỗi trong việc lấy lộ trình.");
            }
            return lotrinhdt;
        }
        public class OBJdataTheoPhien
        {
            public OBJdataTheoPhien() { }

            public bool status { get; set; }

            public string msg { get; set; }
            public lotrinhOBJ LoTrinhDauTien { get; set; }
            public lotrinhOBJ LoTrinhCuoiCung { get; set; }
            public List<List<lotrinhOBJ>> datalotrinh_suydien { get; set; }
            public List<lotrinhOBJ> datalotrinh_suydien_offline { get; set; }
            public List<lotrinhOBJ> datalotrinh { get; set; }
            public List<banglotrinhOBJ> databanglotrinh { get; set; }
            public List<lotrinhtrinhvaodiemOBJ> datavaodiem { get; set; }
            public List<lotrinhtrinhvaodiemOBJ> dataradiem { get; set; }
            public List<lotrinhmattinhieuOBJ> datamattinhieu { get; set; }
        }

        #endregion
    }
}
