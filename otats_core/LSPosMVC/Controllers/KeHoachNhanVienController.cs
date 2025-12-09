using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using LSPos_Data.Data;
using LSPosMVC.Models;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/kehoachnhanvien")]
    public class KeHoachNhanVienController : ApiController
    {

        public class RequestKeHoachParam
        {
            public int ID_NhanVien { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
        }

        [HttpGet]
        [Route("getkehoachbynhanvien")]
        public HttpResponseMessage GetAllKeHoachByNhanVien(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            RequestKeHoachParam param = JsonConvert.DeserializeObject<RequestKeHoachParam>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    KeHoachDiChuyen_dl kh = new KeHoachDiChuyen_dl();

                    BaoCaoCommon bc = new BaoCaoCommon();
                    DataTable dskhdc = bc.GetKeHoachTheoNhanVien_Moi(param.ID_NhanVien, param.start, param.end, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    response = Request.CreateResponse(HttpStatusCode.OK, dskhdc);
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
        [Route("getallnhanvien")]
        public HttpResponseMessage GetAllNhanVien()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    DataTable dt = nv_dl.GetDataNhanVien_LoaiBoDanhDauXoa(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    List<NhanVien> nv = new List<NhanVien>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        nv.Add(nv_dl.GetNhanVienFromDataRow(dr));
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, nv);
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
        [Route("getallnhanviencombo")]
        public HttpResponseMessage GetAllNhanVienCombo()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    DataTable dt = nv_dl.GetDataNhanVien_LoaiBoDanhDauXoa(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    List<NhanVien> nv = new List<NhanVien>();
                    nv.Add(new NhanVien() { TenDayDu = "Tất cả", IDNV = 0 });
                    foreach (DataRow dr in dt.Rows)
                    {
                        nv.Add(nv_dl.GetNhanVienFromDataRow(dr));
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, nv);
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
        [Route("getkhachhangbynhanvien")]
        public HttpResponseMessage GetKhachHangByNhanVien([FromUri] int ID_NhanVien)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    DataTable dsKH = kh_dl.GetKhachHangDaCapQuyen(ID_NhanVien, -1, 0, 0, 0, 0);
                    List<KhachHang> kh = new List<KhachHang>();
                    foreach (DataRow dr in dsKH.Rows)
                    {
                        KhachHang k = new KhachHang();
                        k.TenDayDu = dr["TenKhachHang"].ToString();
                        k.IDKhachHang = int.Parse(dr["ID_KhachHang"].ToString());
                        kh.Add(k);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, kh);
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
        [Route("create")]
        public HttpResponseMessage CreateLich(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            CreateModelKeHoach param = JsonConvert.DeserializeObject<CreateModelKeHoach>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                string msg = "";

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    if (param.ID_KhachHang > 0)
                    {
                        KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                        KeHoachDiChuyenObj khdc_new = new KeHoachDiChuyenObj();
                        khdc_new.IDKeHoach = 0;
                        khdc_new.IDKhachHang = param.ID_KhachHang;
                        khdc_new.IDNhanVien = param.ID_NhanVien;
                        khdc_new.ThoiGianCheckInDuKien = param.BatDau;
                        khdc_new.ThoiGianCheckOutDuKien = param.KetThuc;
                        khdc_new.NgayTao = DateTime.Now;
                        if (khdc_new.ThoiGianCheckInDuKien > khdc_new.ThoiGianCheckOutDuKien)
                        {

                            msg = "label_thoigianvaodiemdukienkhongthelonhonthoigianradiemdukienmoibanvuilongnhaplai";
                        }
                        if (khdc_new.ThoiGianCheckInDuKien == khdc_new.ThoiGianCheckOutDuKien)
                        {

                            msg = "label_thoigianvaodiemdukienkhongthebangthoigianradiemdukienmoibanvuilongnhaplai";
                        }
                        if (khdc_new.ThoiGianCheckInDuKien < DateTime.Now)
                        {

                            msg = "label_khongthelapkehoachchothoidiemoquakhumoibanvuilongchonlai";

                        }
                        khdc_new.GhiChu = param.GhiChu;
                        khdc_new.ViecCanLam = param.ViecCanLam;
                        if (DateTime.Compare(khdc_new.ThoiGianCheckInDuKien, DateTime.Now) > 0)
                        {
                            if (khdc_dl.ThemKeHoach(khdc_new) > 0)
                            {
                                msg = "label_lapkehoachthanhcong";
                                try
                                {
                                    string mess_push = "Bạn có kế hoạch mới ngày " + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                    String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.IDNhanVien + "&ngay=" + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = msg });

                            }
                            else
                            {
                                msg = "label_lapkehoachthatbaivuilongthulai";
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = msg });
                            }
                        }
                        else
                        {
                            msg = "label_lapkehoachthatbaivuilongkiemtrangay";
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = msg });
                        }
                    }
                    else
                    {
                        KhachHang_dl kh_dl = new KhachHang_dl();
                        DataTable dsKH = kh_dl.GetKhachHangDaCapQuyen(param.ID_NhanVien, -1, 0, 0, 0, 0);
                        foreach (DataRow dr in dsKH.Rows)
                        {
                            KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                            KeHoachDiChuyenObj khdc_new = new KeHoachDiChuyenObj();
                            khdc_new.IDKeHoach = 0;
                            khdc_new.IDKhachHang = int.Parse(dr["ID_KhachHang"].ToString());
                            khdc_new.IDNhanVien = param.ID_NhanVien;
                            khdc_new.ThoiGianCheckInDuKien = param.BatDau;
                            khdc_new.ThoiGianCheckOutDuKien = param.KetThuc;
                            khdc_new.NgayTao = DateTime.Now;
                            khdc_new.GhiChu = param.GhiChu;
                            khdc_new.ViecCanLam = param.ViecCanLam;
                            if (khdc_dl.ThemKeHoach(khdc_new) > 0)
                            {
                                msg = "label_lapkehoachthanhcong";
                                try
                                {
                                    string mess_push = "Bạn có kế hoạch mới ngày " + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                    String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.IDNhanVien + "&ngay=" + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                            }
                            else
                            {
                                msg = "label_lapkehoachthatbaivuilongthulai";
                            }
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = msg });
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
        [Route("createMulti")]
        public HttpResponseMessage CreateLich([FromBody]List<CreateModelKeHoach> paramss)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                string msg = "";

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    int nguoitao = userinfo.ID_QuanLy;
                    DateTime ngaytao = DateTime.Now;
                    foreach (CreateModelKeHoach param in paramss)
                    {
                        if (param.ID_KhachHang > 0)
                        {
                            KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                            KeHoachDiChuyenObj khdc_new = new KeHoachDiChuyenObj();
                            khdc_new.IDKeHoach = param.ID;
                            khdc_new.IDKhachHang = param.ID_KhachHang;
                            khdc_new.IDNhanVien = param.ID_NhanVien;
                            khdc_new.ThoiGianCheckInDuKien = param.BatDau;
                            khdc_new.ThoiGianCheckOutDuKien = param.KetThuc;
                            khdc_new.NguoiTao = nguoitao;
                            khdc_new.NgayTao = ngaytao;
                            if (khdc_new.ThoiGianCheckInDuKien > khdc_new.ThoiGianCheckOutDuKien)
                            {

                                msg = "label_thoigianvaodiemdukienkhongthelonhonthoigianradiemdukienmoibanvuilongnhaplai";
                            }
                            if (khdc_new.ThoiGianCheckInDuKien == khdc_new.ThoiGianCheckOutDuKien)
                            {

                                msg = "label_thoigianvaodiemdukienkhongthebangthoigianradiemdukienmoibanvuilongnhaplai";
                            }
                            if (khdc_new.ThoiGianCheckInDuKien < DateTime.Now)
                            {

                                msg = "label_khongthelapkehoachchothoidiemoquakhumoibanvuilongchonlai";

                            }
                            khdc_new.GhiChu = param.GhiChu;
                            khdc_new.ViecCanLam = param.ViecCanLam;
                            if (DateTime.Compare(khdc_new.ThoiGianCheckInDuKien, DateTime.Now) > 0)
                            {
                                bool success = false;
                                if(khdc_new.IDKeHoach == 0)
                                {
                                    success = (khdc_dl.ThemKeHoach(khdc_new) > 0);
                                }
                                else
                                {
                                    success = (khdc_dl.SuaKeHoach(khdc_new) > 0);
                                }
                                if (success)
                                {
                                    msg = "label_lapkehoachthanhcong";
                                    try
                                    {
                                        string mess_push = "Bạn có kế hoạch mới ngày " + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                        String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.IDNhanVien + "&ngay=" + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                    }
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = msg });

                                }
                                else
                                {
                                    msg = "label_lapkehoachthatbaivuilongthulai";
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = msg });
                                }
                            }
                            else
                            {
                                msg = "label_lapkehoachthatbaivuilongkiemtrangay";
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = msg });
                            }
                        }
                        else
                        {
                            KhachHang_dl kh_dl = new KhachHang_dl();
                            DataTable dsKH = kh_dl.GetKhachHangDaCapQuyen(param.ID_NhanVien, -1, 0, 0, 0, 0);
                            foreach (DataRow dr in dsKH.Rows)
                            {
                                KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                                KeHoachDiChuyenObj khdc_new = new KeHoachDiChuyenObj();
                                khdc_new.IDKeHoach = 0;
                                khdc_new.IDKhachHang = int.Parse(dr["ID_KhachHang"].ToString());
                                khdc_new.IDNhanVien = param.ID_NhanVien;
                                khdc_new.ThoiGianCheckInDuKien = param.BatDau;
                                khdc_new.ThoiGianCheckOutDuKien = param.KetThuc;
                                khdc_new.GhiChu = param.GhiChu;
                                khdc_new.ViecCanLam = param.ViecCanLam;
                                if (khdc_dl.ThemKeHoach(khdc_new) > 0)
                                {
                                    msg = "Lập kế hoạch thành công";
                                    try
                                    {
                                        string mess_push = "Bạn có kế hoạch mới ngày " + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                        String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.IDNhanVien + "&ngay=" + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                    }
                                }
                                else
                                {
                                    msg = "Lập kế hoạch thất bại, vui lòng thử lại";
                                }
                            }
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = msg });
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

        [HttpGet]
        [Route("update")]
        public HttpResponseMessage UpdateLich(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            CreateModelKeHoach param = JsonConvert.DeserializeObject<CreateModelKeHoach>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                string err = "";
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                    KeHoachDiChuyenObj khdc_new = khdc_dl.GetKeHoachById(param.ID).First();
                    khdc_new.IDKhachHang = param.ID_KhachHang;
                    khdc_new.IDNhanVien = param.ID_NhanVien;
                    khdc_new.ThoiGianCheckInDuKien = param.BatDau;
                    khdc_new.ThoiGianCheckOutDuKien = param.KetThuc;

                    if (khdc_new.ThoiGianCheckInDuKien > khdc_new.ThoiGianCheckOutDuKien)
                    {
                        err = "label_thoigianvaodiemdukienkhongthelonhonthoigianradiemdukienmoibanvuilongnhaplai";
                    }
                    if (khdc_new.ThoiGianCheckInDuKien == khdc_new.ThoiGianCheckOutDuKien)
                    {

                        err = "label_thoigianvaodiemdukienkhongthebangthoigianradiemdukienmoibanvuilongnhaplai";
                    }
                    if (khdc_new.ThoiGianCheckInDuKien < DateTime.Now)
                    {

                        err = "label_khongtheluukehoachchothoidiemoquakhumoibanvuilongchonlai";

                    }
                    if (khdc_new.ThoiGianCheckInThucTe.Year != 1900 && khdc_new.ThoiGianCheckInThucTe < DateTime.Now)
                    {

                        err = "label_kehoachdacodulieukhongchophepsuahoacxoathongtin";

                    }
                    khdc_new.GhiChu = param.GhiChu;
                    khdc_new.ViecCanLam = param.ViecCanLam;
                    if (err == "")
                    {
                        if (khdc_dl.SuaKeHoach(khdc_new) > 0)
                        {
                            err = "label_luuthongtinkehoachthanhcong";
                            try
                            {
                                string mess_push = "Bạn có kế hoạch mới ngày " + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.IDNhanVien + "&ngay=" + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }

                        }
                    }
                }
                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = err });

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("delete")]
        public HttpResponseMessage DeleteLich(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            CreateModelKeHoach param = JsonConvert.DeserializeObject<CreateModelKeHoach>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                    List<KeHoachDiChuyenObj> khl = khdc_dl.GetKeHoachById(param.ID);
                    foreach (KeHoachDiChuyenObj kh in khl)
                    {
                        if (DateTime.Compare(kh.ThoiGianCheckInDuKien, DateTime.Now) < 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, false);
                        }
                        else
                        {
                            khdc_dl.DeleteKeHoach(kh);
                        }

                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
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
