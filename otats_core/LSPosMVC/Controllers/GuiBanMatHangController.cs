
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


namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/guibanmathang")]
    public class GuiBanMatHangController : ApiController
    {

        [HttpGet]
        [Route("getdsmtahangbykhach")]
        public HttpResponseMessage GetMatHangByKhachHang([FromUri]int ID_KhachHang)
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
                    GuiBanMatHangData data = new GuiBanMatHangData();
                    List<KhachHang_HangGuiModel> list = data.GetAllByKhachHang(ID_KhachHang, userinfo.ID_QLLH);
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

        [HttpGet]
        [Route("getallkhachhanggui")]
        public HttpResponseMessage GetAllKhachHangGui()
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
                    GuiBanMatHangData data = new GuiBanMatHangData();
                    List<KhachHang_HangGuiModel> list = data.GetAllKhachHangGui(userinfo.ID_QLLH);
                    XeData xdt = new XeData();
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

        [HttpPost]
        [Route("hoantat")]
        public HttpResponseMessage hoantat([FromBody] List<KhachHang_HangGuiModel> listobj)
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
                    GuiBanMatHangData data = new GuiBanMatHangData();
                    int idKhachHang = 0;

                    string listid = "0";
                    if (listobj != null)
                    {
                        foreach (KhachHang_HangGuiModel obj in listobj)
                        {
                            if (obj.ID_MatHang > 0)
                            {
                                obj.ID_QLLH = userinfo.ID_QLLH;
                                obj.UpdatedBy = userinfo.Username;
                                int idgui = data.add(obj);
                            }
                            listid = listid + obj.ID_MatHang.ToString() + ",";
                            idKhachHang = obj.ID_KhachHang;

                        }
                    }

                    //Đánh dấu xóa các dòng bị xóa (không thuộc listid)
                    bool result = data.deletemarkmulti(idKhachHang, listid, userinfo.Username);
                    if(result)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Gửi mặt hàng cho khách hàng thành công!" });

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
        [Route("create")]
        public HttpResponseMessage Create([FromBody] KhachHang_HangGuiModel item)
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
                    GuiBanMatHangData data = new GuiBanMatHangData();
                    item.ID_QLLH = userinfo.ID_QLLH;
                    if (data.Create(item))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Gửi mặt hàng cho khách hàng thành công!" });

                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Gửi mặt hàng cho khách hàng thất bại, vui lòng thử lại!" });

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
        [Route("delete")]
        public HttpResponseMessage Delete([FromBody] List<int> Ids)
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
                    GuiBanMatHangData data = new GuiBanMatHangData();
                    foreach (int ID in Ids)
                    {
                        if (data.Delete(ID))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Xóa mặt hàng gửi thành công!" });

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
    }
}
