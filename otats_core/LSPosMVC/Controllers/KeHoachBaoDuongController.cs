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
    [RoutePrefix("api/kehoachbaoduong")]
    public class KeHoachBaoDuongController : ApiController
    {

        public class RequestKeHoachParam
        {
            public int ID_NhanVien { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
        }

        [HttpGet]
        [Route("getkehoachbaoduongbynhanvien")]
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

                    List<KeHoachBaoDuongOBJ> listDH = KeHoachBaoDuongDB.KeHoachTuNgayDenNgay(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.ID_NhanVien, param.start.ToString("yyyy-MM-dd HH:mm:ss"), param.end.ToString("yyyy-MM-dd HH:mm:ss"), 0);
                    if (listDH == null)
                    {
                        listDH = new List<KeHoachBaoDuongOBJ>();
                    }
                    foreach (KeHoachBaoDuongOBJ item in listDH)
                    {
                        item.NgayBDTiepTheo = item.NgayBaoDuongDuKien.AddHours(1);
                        if (item.NgayBaoDuongDuKien.Year > 1900 && (item.NgayBaoDuongDuKien < DateTime.Now && item.TrangThai == 0))
                        {
                            item.text_color = "#DD4B39";
                            item.text_color_mota = "label_daquangaykehoachmachuadibaoduong";
                            //- Màu đỏ: kế hoạch đã quá giờ mà chưa vào điểm
                            // e.Row.Cells[8].CssClass = "label label-danger";

                        }

                        else if (item.TrangThai == 1)
                        {
                            item.text_color = "#3C8DBC";
                            item.text_color_mota = "label_dadibaoduongtheokehoach";
                            //-   //-Màu xanh blue: kế hoạch đã vào điểm trước/ đúng giờ theo dự kiến
                            //e.Row.Cells[8].CssClass = "label label-primary";

                        }

                        else if (item.NgayBaoDuongDuKien > DateTime.Now && item.TrangThai == 0)
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

        [HttpGet]
        [Route("getallxe")]
        public HttpResponseMessage GetAllXe()
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
                    XeDB xe_dl = new XeDB();
                    DataTable dsmh = new DataTable();

                    dsmh = xe_dl.GetDataDanhSach(userinfo.ID_QLLH);
                    List<XeOBJ> nv = new List<XeOBJ>();
                    foreach(DataRow dr in dsmh.Rows)
                    {
                        XeOBJ item = xe_dl.GetFromDataRow(dr);
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
                    KeHoachBaoDuongOBJ khdc_new = new KeHoachBaoDuongOBJ();
                    XeDB xe = new XeDB();
                    XeOBJ objXe = xe.GetById(param.ID_Xe);
                    khdc_new.ID_Xe = param.ID_Xe;
                    khdc_new.ID_NhanVien = objXe.ID_NhanVien;
                    khdc_new.NgayBaoDuongDuKien = param.Ngay;
                    if (khdc_new.NgayBaoDuongDuKien < DateTime.Now)
                    {

                        msg = "label_khongthelapkehoachchothoidiemoquakhumoibanvuilongchonlai";

                    }
                    if (KeHoachBaoDuongDB.ThemMoi(khdc_new) > 0)
                    {
                        msg = "label_lapkehoachthanhcong";
                        try
                        {
                            string mess_push = "Bạn có kế hoạch bảo dưỡng mới cho xe " + objXe.BienKiemSoat + " ngày " + param.Ngay.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch bảo dưỡng để kiểm tra";
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
                        //msg = "Lập kế hoạch thất bại, ngày " + param.Ngay.ToString("dd/MM/yyyy") + " đã có kế hoạch với xe " + objXe.BienKiemSoat;
                        msg = "label_lapkehoachthatbaivuilongthulai";
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
                    KeHoachBaoDuongOBJ khdc_new = new KeHoachBaoDuongOBJ();
                    XeDB xe = new XeDB();
                    XeOBJ objXe = xe.GetById(param.ID_Xe);
                    khdc_new.ID_Xe = param.ID_Xe;
                    khdc_new.ID_NhanVien = objXe.ID_NhanVien;
                    khdc_new.NgayBaoDuongDuKien = param.Ngay;
                    khdc_new.ID_Xe_KeHoachBaoDuong = param.ID;
                    if (khdc_new.NgayBaoDuongDuKien < DateTime.Now)
                    {

                        msg = "label_khongthelapkehoachchothoidiemoquakhumoibanvuilongchonlai";

                    }
                    if (KeHoachBaoDuongDB.CapNhat(khdc_new) > 0)
                    {
                        msg = "label_capnhatkehoachthanhcong";
                        try
                        {
                            string mess_push = "Bạn có kế hoạch bảo dưỡng mới cho xe " + objXe.BienKiemSoat + " ngày " + param.Ngay.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch bảo dưỡng để kiểm tra";
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
                    KeHoachBaoDuongDB.XoaKeHoach(param.ID);
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
