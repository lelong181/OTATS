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
using LSPos_Data.Models;
using LSPos_Data.DataAccess;
using static LSPos_Data.Data.NhanVienAppController;
using NPOI.SS.Formula.Functions;
using RazorEngine;
using RazorEngine.Templating;
using BusinessLayer.Repository;
using LSPosMVC.App_Start;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/huongdanvien")]
    public class HDVController : ApiController
    {

        private SellRepository _sellRepository;

        public HDVController()
        {
            _sellRepository = new SellRepository();
        }


        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage getall()
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
                    HuongDanVienDAO nvapp = new HuongDanVienDAO();
                    List<HuongDanVienModel> dsnv = nvapp.GetDSHDV();

                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        [Route("getbymatthe")]
        public HttpResponseMessage getbymatthe([FromUri] string mathe)
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
                    HuongDanVienDAO nvapp = new HuongDanVienDAO();
                    HuongDanVienModel dsnv = nvapp.GetHDV_ByMaThe(mathe);

                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        [AllowAnonymous]
        [Route("themmoi")]
        public HttpResponseMessage themmoi([FromBody] HuongDanVienModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                //UserInfo userinfo = utilsCommon.checkAuthorization();
                //if (userinfo == null)
                //{
                //    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                //}
                //else
                //{
                HuongDanVienDAO nvapp = new HuongDanVienDAO();
                HuongDanVienModel dsnv = nvapp.GetHDV_ByMaThe(model.MaTheHDV);
                if (dsnv == null)
                {
                    model.MatKhau = "12345678";
                    int id = nvapp.ThemHuongDanVien(model);
                    if (id == 0)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Có lỗi trong quá trình xử lý thông tin, vui lòng liên hệ quản trị viên!" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Lưu thông tin thành công!" });

                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Thông tin số thẻ hướng dẫn viên đã tồn tại" });
                }
                //}
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("kichhoat_hdv")]
        public HttpResponseMessage kichhoat_hdv([FromUri] int ID)
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
                    HuongDanVienDAO nvapp = new HuongDanVienDAO();
                    HuongDanVienModel dsnv = nvapp.GetHDV_ByID(ID);
                    bool success = nvapp.ActiveHuongDanVien(ID);
                    if (success)
                    {
                        EmailHelper helper = new EmailHelper();
                        string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/WelcomeHDV.html");
                        string bodyTemplate = System.IO.File.ReadAllText(path);
                        var html = Engine.Razor.RunCompile(bodyTemplate, "WelcomeHDV", dsnv.GetType(), dsnv);
                        helper.SendEmail(html.ToString(), dsnv.Email, null, "[THÔNG BÁO] THƯ CHÀO MỪNG HDV THAM GIA HỆ THỐNG OTA TRÀNG AN!");
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = success, msg = "Kích hoạt tài khoản thành công" });

                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("khoa_hdv")]
        public HttpResponseMessage khoa_hdv([FromUri] int ID)
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
                    HuongDanVienDAO nvapp = new HuongDanVienDAO();
                    bool success = nvapp.InactiveHuongDanVien(ID);

                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = success, msg = "Khoá tài khoản thành công" });

                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("yeucaufoc")]
        public HttpResponseMessage yeucaufoc([FromUri] string AccountCode)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                //UserInfo userinfo = utilsCommon.checkAuthorization();

                //if (userinfo == null)
                //{
                //    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                //}
                //else
                //{
                DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                List<DonHang_DichVuRequestAPIModel> models = dvdao.GetCauHinhFOC();
                var data = BookingUtilities.ConfirmFOCBookingRequestToLocalAPI(models, _sellRepository);
                response = Request.CreateResponse(HttpStatusCode.OK, data);
                //}
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
