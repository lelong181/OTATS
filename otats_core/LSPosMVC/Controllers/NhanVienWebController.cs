
using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using LSPos_Data.Models;


using LSPos_Data.Data;
using LSPosMVC.Models;
using RazorEngine.Compilation.ImpromptuInterface.Optimization;
using RazorEngine;
using RazorEngine.Templating;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/nhanvienweb")]
    public class NhanVienWebController : ApiController
    {
        [HttpPost]
        [Route("xoaquanly")]
        public HttpResponseMessage xoaquanly([FromBody] int idquanly)
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
                    User_dl u_dl = new User_dl();
                    if (userinfo.ID_QuanLy != idquanly)
                    {
                        if (userinfo.Level == 1) // quan ly moi duoc quyen xoa
                        {
                            if (u_dl.XoaUser(userinfo.ID_QLLH, idquanly))
                            {
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoataikhoanthanhcong" });
                            }
                        }
                        else
                        {
                            //kiểm tra xem quản lý đấy chỉ thuộc 1 nhóm hiện tại thì cho phép xóa, không thì thôi
                            List<NhomOBJ> lstNhom = u_dl.GetDanhSachNhomQuanLy(idquanly);
                            if (lstNhom.Count == 1)
                            {
                                if (u_dl.XoaUser(userinfo.ID_QLLH, idquanly))
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoataikhoanthanhcong" });
                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_bankhongcoquyenthuchienthaotac" });
                            }
                        }
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_bankhongthexoachinhtaikhoandangdangnhap" });
                    }
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
        [Route("resetPassword")]
        public HttpResponseMessage resetPassword([FromBody] ResetPassModelFilter obj)
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
                    UserinforData user = new UserinforData();
                    if (user.ChangeUserPassword(obj.username, obj.newpass, userinfo.ID_QLLH))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_capnhatmatkhauthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, new { success = false, msg = "label_capnhatmatkhaukhongthanhcongvuilongthulai" });
                    }
                            ;
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getbyid")]
        public HttpResponseMessage getbyid([FromUri] int idtaikhoan)
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
                    User_dl user_dl = new User_dl();
                    UserInfo user = user_dl.GetUserInfo(idtaikhoan);

                    response = Request.CreateResponse(HttpStatusCode.OK, user);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        [HttpGet]
        [Route("getlist")]
        public HttpResponseMessage getlist([FromUri] int idnhom)
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
                    User_dl user_dl = new User_dl();
                    List<UserInfo> dsuser = new List<UserInfo>();
                    dsuser = user_dl.GetListUserByNhom(idnhom, userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, dsuser);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public class DangKyOTAModel
        {
            public string username { get; set; }
            public string company { get; set; }
            public NhanVienWebModels quanly { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dangkyquanly")]
        public HttpResponseMessage dangkyquanly([FromBody] DangKyOTAModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = new User_dl().GetUserInfo(model.username, model.company);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    UserinforData u_dl = new UserinforData();
                    if (u_dl.CheckTonTaiUserWeb(model.quanly.Email, userinfo.ID_QLLH))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_quanlyvoitendangnhapnaydatontai" });
                    }
                    else
                    {
                        NhanVienWebData data = new NhanVienWebData();
                        model.quanly.ID_QLLH = userinfo.ID_QLLH;
                        model.quanly.TenDangNhap = model.quanly.Email;
                        model.quanly.MatKhau = "12345678";
                        int idtaikhoan = data.ThemQuanLyCon(model.quanly);
                        int idnhommacdinh = 28;
                        if (idtaikhoan > 0)
                        {
                            NhomDB.PhanNhom(idnhommacdinh, idtaikhoan);
                            LoaiKhachHangDB lkh_dl = new LoaiKhachHangDB();
                            LoaiKhachHangOBJ paramlkh =  new LoaiKhachHangOBJ();
                            paramlkh.ID_LoaiKhachHang = 0;
                            paramlkh.TenLoaiKhachHang = "Khách " + model.quanly.TenDayDu;
                            paramlkh.ID_QLLH = 1;
                            paramlkh.IconHienThi = "/assets/img/iconloaikhachhang/08.png";
                            int loaikhnew = lkh_dl.Them(paramlkh);
                            NhanVien_dl nvdl = new NhanVien_dl();
                            NhanVien obj = nvdl.GetNVTheoUserName(model.quanly.TenDangNhap, userinfo.ID_QLLH);
                            if (obj != null)
                            {
                                obj.IDQLLH = userinfo.ID_QLLH;
                                obj.ID_QuanLy = idtaikhoan;
                                obj.TenDangNhap = model.quanly.TenDangNhap;
                                obj.TenDayDu = model.quanly.TenDayDu;
                                obj.MatKhau = model.quanly.MatKhau;
                                obj.DienThoai = "";
                                obj.Email = "";
                                obj.ID_Nhom = idnhommacdinh;
                                obj.TruongNhom = 1;
                                obj.ID_NhomKhachHang_MacDinh = loaikhnew;
                                nvdl.CapNhatNhanVien(obj, userinfo.ID_QuanLy);
                            }
                            else
                            {
                                obj = new NhanVien();
                                obj.IDNV = 0;
                                obj.IDQLLH = userinfo.ID_QLLH;
                                obj.ID_QuanLy = idtaikhoan;
                                obj.ID_ChucVu = 0;
                                obj.TenDangNhap = model.quanly.TenDangNhap;
                                obj.TenDayDu = model.quanly.TenDayDu;
                                obj.MatKhau = model.quanly.MatKhau;
                                obj.DiaChi = "";
                                obj.QueQuan = "";
                                obj.DienThoai = "";
                                obj.Email = "";
                                obj.ChucVu = "";
                                obj.ID_Nhom = idnhommacdinh;
                                obj.GioiTinh = 1;
                                obj.TruongNhom = 1;
                                obj.NgaySinh = new DateTime(1900, 1, 1);
                                obj.ID_NhomKhachHang_MacDinh = loaikhnew;
                                nvdl.ThemNhanVien(obj);

                            }

                            EmailHelper helper = new EmailHelper();
                            string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/WelcomeOTA.html");
                            string bodyTemplate = System.IO.File.ReadAllText(path);
                            var html = Engine.Razor.RunCompile(bodyTemplate, "WelcomeOTA", model.quanly.GetType(), model.quanly);
                            helper.SendEmail(html.ToString(), model.quanly.Email, null, "[THÔNG BÁO] THƯ CHÀO MỪNG THAM GIA HỆ THỐNG TOURSHOPPING!");
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themtaikhoanthanhcong" });
                        }
                        if (idtaikhoan > 0)
                        {
                            NhomDB.PhanNhom(-1, idtaikhoan);
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themtaikhoanthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_taikhoandatontaihoacsoluongtaikhoandadatsoluongtoida" });
                        }
                    }
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
        [Route("themquanly")]
        public HttpResponseMessage themquanly([FromBody] NhanVienWebModels quanly)
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
                    UserinforData u_dl = new UserinforData();
                    if (u_dl.CheckTonTaiUserWeb(quanly.TenDangNhap, userinfo.ID_QLLH))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_quanlyvoitendangnhapnaydatontai" });
                    }
                    else
                    {
                        NhanVienWebData data = new NhanVienWebData();
                        quanly.ID_QLLH = userinfo.ID_QLLH;
                        int idtaikhoan = data.ThemQuanLyCon(quanly);
                        if (idtaikhoan > 0)
                        {
                            foreach (int id in quanly.DanhSachNhom)
                            {
                                NhomDB.PhanNhom(id, idtaikhoan);
                            }
                            NhanVien_dl nvdl = new NhanVien_dl();
                            NhanVien obj = nvdl.GetNVTheoUserName(quanly.TenDangNhap, userinfo.ID_QLLH);
                            if (obj != null)
                            {
                                obj.IDQLLH = userinfo.ID_QLLH;
                                obj.ID_QuanLy = idtaikhoan;
                                obj.TenDangNhap = quanly.TenDangNhap;
                                obj.TenDayDu = quanly.TenDayDu;
                                obj.MatKhau = quanly.MatKhau;
                                obj.DienThoai = quanly.Phone;
                                obj.Email = quanly.Email;
                                obj.ID_Nhom = quanly.DanhSachNhom.First();
                                obj.TruongNhom = 1;
                                nvdl.CapNhatNhanVien(obj, userinfo.ID_QuanLy);
                            }
                            else
                            {
                                obj = new NhanVien();
                                obj.IDNV = 0;
                                obj.IDQLLH = userinfo.ID_QLLH;
                                obj.ID_QuanLy = idtaikhoan;
                                obj.ID_ChucVu = 0;
                                obj.TenDangNhap = quanly.TenDangNhap;
                                obj.TenDayDu = quanly.TenDayDu;
                                obj.MatKhau = quanly.MatKhau;
                                obj.DiaChi = "";
                                obj.QueQuan = "";
                                obj.DienThoai = quanly.Phone;
                                obj.Email = quanly.Email;
                                obj.ChucVu = "";
                                obj.ID_Nhom = quanly.DanhSachNhom.First();
                                obj.GioiTinh = 1;
                                obj.TruongNhom = 1;
                                obj.NgaySinh = new DateTime(1900, 1, 1);
                                nvdl.ThemNhanVien(obj);

                            }

                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themtaikhoanthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_taikhoandatontaihoacsoluongtaikhoandadatsoluongtoida" });
                        }
                    }
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
        [Route("suaquanly")]
        public HttpResponseMessage suaquanly([FromBody] NhanVienWebModels quanly)
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
                    NhanVienWebData data = new NhanVienWebData();
                    quanly.ID_QLLH = userinfo.ID_QLLH;
                    int ID_Nhom = 0;
                    if (data.SuaQuanLyCon(quanly) > 0)
                    {
                        NhomDB.XoaPhanNhom(quanly.ID_QuanLy);
                        foreach (int id in quanly.DanhSachNhom)
                        {
                            NhomDB.PhanNhom(id, quanly.ID_QuanLy);
                            ID_Nhom = id;
                        }
                        NhanVien_dl nvdl = new NhanVien_dl();
                        NhanVien obj = nvdl.GetNVTheoUserName(quanly.TenDangNhap, userinfo.ID_QLLH);
                        if (obj != null)
                        {

                            obj.IDQLLH = userinfo.ID_QLLH;
                            obj.ID_QuanLy = quanly.ID_QuanLy;
                            obj.TenDangNhap = quanly.TenDangNhap;
                            obj.TenDayDu = quanly.TenDayDu;
                            obj.MatKhau = quanly.MatKhau;
                            obj.DienThoai = quanly.Phone;
                            obj.Email = quanly.Email;
                            obj.ID_Nhom = ID_Nhom;
                            obj.TruongNhom = 1;
                            nvdl.CapNhatNhanVien(obj, userinfo.ID_QuanLy);

                        }
                        else
                        {
                            obj = new NhanVien();
                            obj.IDNV = 0;
                            obj.IDQLLH = userinfo.ID_QLLH;
                            obj.ID_QuanLy = quanly.ID_QuanLy;
                            obj.ID_ChucVu = 0;
                            obj.TenDangNhap = quanly.TenDangNhap;
                            obj.TenDayDu = quanly.TenDayDu;
                            obj.MatKhau = quanly.MatKhau;
                            obj.DiaChi = "";
                            obj.QueQuan = "";
                            obj.DienThoai = quanly.Phone;
                            obj.Email = quanly.Email;
                            obj.ChucVu = "";
                            obj.ID_Nhom = ID_Nhom;
                            obj.GioiTinh = 1;
                            obj.TruongNhom = 1;
                            obj.NgaySinh = new DateTime(1900, 1, 1);
                            nvdl.ThemNhanVien(obj);

                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_suataikhoanthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_taikhoandatontaihoacsoluongtaikhoandadatsoluongtoida" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("treenhom")]
        public HttpResponseMessage gettree([FromUri] int idtaikhoan)
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
                    NhanVienWebData nhanVienWebData = new NhanVienWebData();
                    List<NhomModels> lstDanhMuc = new List<NhomModels>();
                    lstDanhMuc = nhanVienWebData.getlistnhomphanquyen(userinfo.ID_QLLH, userinfo.ID_QuanLy, idtaikhoan);

                    List<NhomModels> list = new List<NhomModels>();

                    foreach (NhomModels obj in lstDanhMuc)
                    {
                        IEnumerable<NhomModels> findCha = lstDanhMuc.Where(person => person.ID_Nhom == obj.ID_PARENT);

                        bool flag = true;
                        foreach (NhomModels i in findCha)
                        {
                            flag = false;
                            break;
                        }

                        if (flag)
                        {
                            NhomModels resultObj = new NhomModels();

                            TaoNhom(obj, lstDanhMuc, resultObj);

                            list.Add(resultObj);
                        }
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        public void TaoNhom(NhomModels obj, List<NhomModels> lstDanhMuc, NhomModels resultObj)
        {
            resultObj.ID_Nhom = obj.ID_Nhom;
            resultObj.TenNhom = obj.TenNhom;
            resultObj.ID_PARENT = obj.ID_PARENT;
            resultObj.TenHienThi_NhanVien = obj.TenHienThi_NhanVien;
            resultObj.MaNhom = obj.MaNhom;
            resultObj.isChecked = obj.isChecked;
            var query1 = lstDanhMuc.Where(person => person.ID_PARENT == obj.ID_Nhom);

            List<NhomModels> li = new List<NhomModels>();
            foreach (NhomModels obj1 in query1)
            {
                NhomModels objcon = new NhomModels();
                TaoNhom(obj1, lstDanhMuc, objcon);
                li.Add(objcon);
            }
            resultObj.childs = li;
        }
    }
}
