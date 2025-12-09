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
    [RoutePrefix("api/kehoachgiaohang")]
    public class KeHoachGiaoHangController : ApiController
    {

        public class RequestKeHoachParam
        {
            public int ID_NhanVien { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
        }

        [HttpGet]
        [Route("getkehoachgiaohangbynhanvien")]
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
                    KeHoachDonHang_dl kh = new KeHoachDonHang_dl();

                    List<KeHoachDonHangOBJ> listDH = kh.GetKeHoachTheoNhanVien_Moi(param.ID_NhanVien, param.start, param.end, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    if (listDH == null)
                    {
                        listDH = new List<KeHoachDonHangOBJ>();
                    }
                    foreach (KeHoachDonHangOBJ item in listDH)
                    {
                        item.NgayTaoDonHang = item.ThoiGianDuKien.AddHours(1);
                        if (item.ThoiGianDuKien.Year > 1900 && (item.ThoiGianDuKien < DateTime.Now && item.ThoiGianThucTe.Year < 2000))
                        {
                            item.text_color = "#DD4B39";
                            item.text_color_mota = "label_kehoachdaquagiomachuagiao";
                            //- Màu đỏ: kế hoạch đã quá giờ mà chưa vào điểm
                            // e.Row.Cells[8].CssClass = "label label-danger";

                        }
                        else if (item.ThoiGianDuKien < item.ThoiGianThucTe && item.ThoiGianThucTe.Year > 2000)
                        {
                            item.text_color = "#F39C12";
                            item.text_color_mota = "label_kehoachdagiaohangnhungmuonhonsovoithoigiandukien";
                            //- Màu vàng: kế hoạch đã vào điểm nhưng muộn hơn so với thời gian dự kiến
                            //e.Row.Cells[8].CssClass = "label label-warning";

                        }
                        else if (item.ThoiGianDuKien >= item.ThoiGianThucTe && item.ThoiGianThucTe.Year > 2000)
                        {
                            item.text_color = "#3C8DBC";
                            item.text_color_mota = "label_kehoachdagiaohangtruocdunggiotheodukien";
                            //-   //-Màu xanh blue: kế hoạch đã vào điểm trước/ đúng giờ theo dự kiến
                            //e.Row.Cells[8].CssClass = "label label-primary";

                        }

                        else if (item.ThoiGianDuKien > DateTime.Now)
                        {
                            item.text_color = "#00A65A";
                            item.text_color_mota = "label_kehoachchuaden";
                            //-Màu xanh: kế hoạch chưa đến giờ
                            // e.Row.Cells[8].CssClass = "label label-success";

                        }
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, listDH);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public class DonHangComboModel
        {
            public string Ma { get; set; }
            public string Ten { get; set; }
        }

        [HttpGet]
        [Route("getalldonhang")]
        public HttpResponseMessage GetAllDonHang()
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
                    DonHang_dl dh_dl = new DonHang_dl();
                    DateTime dtFrom = new DateTime(1900, 01, 01);
                    DateTime dtTo = DateTime.Now;
                    DataTable dt = dh_dl.GetDataDonHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, dtFrom, dtTo, 1, 0, 0, 0, 0, 0, "");
                    List<DonHangComboModel> nv = new List<DonHangComboModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        DonHangComboModel item = new DonHangComboModel();
                        item.Ma = dr["ID_DonHang"].ToString();
                        item.Ten = "Mã: " + dr["MaThamChieu"] + " (KH: " + dr["TenKhachHang"] + " - Ngày tạo: " + Convert.ToDateTime(dr["CreateDate"]).ToString("dd/MM/yyyy HH:mm") + ")";
                        nv.Add(item);
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
                    bool flag = true;
                    if(flag && param.ID_NhanVien <= 0)
                    {
                        flag = false;
                        msg = "label_lapkehoachthatbaivuilongthulai";
                    }
                    if (flag && param.ID_DonHang <= 0)
                    {
                        flag = false;
                        msg = "label_lapkehoachthatbaivuilongthulai";
                    }
                    if (flag && param.Ngay < DateTime.Now)
                    {
                        flag = false;
                        msg = "label_khongthelapkehoachchothoidiemoquakhumoibanvuilongchonlai";
                    }

                    if (flag)
                    {
                        KeHoachDonHang_dl khdc_dl = new KeHoachDonHang_dl();
                        KeHoachDonHangOBJ khdc_new = new KeHoachDonHangOBJ();
                        khdc_new.IDKeHoach = 0;
                        khdc_new.ID_KhachHang = param.ID_KhachHang;
                        khdc_new.ID_DonHang = param.ID_DonHang;
                        khdc_new.ID_NhanVien = param.ID_NhanVien;
                        khdc_new.ThoiGianDuKien = param.Ngay;
                        khdc_new.GhiChu = param.GhiChu;
                        if (khdc_dl.ThemKeHoach(khdc_new) > 0)
                        {
                            msg = "label_lapkehoachthanhcong";
                            try
                            {
                                string mess_push = "Bạn có kế hoạch giao hàng mới ngày " + param.Ngay.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.ID_NhanVien + "&ngay=" + param.Ngay.ToString("dd/MM/yyyy") + "&message=" + mess_push);
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
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = msg });
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
        [Route("update")]
        public HttpResponseMessage UpdateLich([FromBody] CreateModelKeHoach param)
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
                    bool flag = true;
                    if (flag && param.ID_NhanVien <= 0)
                    {
                        flag = false;
                        msg = "label_lapkehoachthatbaivuilongthulai";
                    }
                    if (flag && param.ID_DonHang <= 0)
                    {
                        flag = false;
                        msg = "label_lapkehoachthatbaivuilongthulai";
                    }
                    if (flag && param.Ngay < DateTime.Now)
                    {
                        flag = false;
                        msg = "label_khongthelapkehoachchothoidiemoquakhumoibanvuilongchonlai";
                    }

                    if (flag)
                    {
                        KeHoachDonHang_dl khdc_dl = new KeHoachDonHang_dl();
                        KeHoachDonHangOBJ khdc_new = khdc_dl.GetKeHoachById(param.ID).First();
                        khdc_new.ID_DonHang = param.ID_DonHang;
                        khdc_new.ID_NhanVien = param.ID_NhanVien;
                        khdc_new.ThoiGianDuKien = param.Ngay;
                        khdc_new.GhiChu = param.GhiChu;
                        khdc_new.GhiChu = param.GhiChu;
                        if (khdc_dl.SuaKeHoach(khdc_new) > 0)
                        {
                            msg = "label_lapkehoachthanhcong";
                            try
                            {
                                string mess_push = "Bạn có kế hoạch giao hàng mới ngày " + param.Ngay.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.ID_NhanVien + "&ngay=" + param.Ngay.ToString("dd/MM/yyyy") + "&message=" + mess_push);
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
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = msg });
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
                    KeHoachDonHang_dl khdc_dl = new KeHoachDonHang_dl();
                    KeHoachDonHangOBJ kh = khdc_dl.GetKeHoachById(param.ID).First();
                    if (DateTime.Compare(kh.ThoiGianDuKien, DateTime.Now) > 0)
                    {
                        khdc_dl.XoaKeHoach(param.ID);
                        response = Request.CreateResponse(HttpStatusCode.OK, true);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, false);
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
    }
}
