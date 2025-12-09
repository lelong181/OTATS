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

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/loaikhachhang")]
    public class LoaiKhachHangController : ApiController
    {
        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage get()
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
                    LoaiKhachHangDB lkh = new LoaiKhachHangDB();
                    List<LoaiKhachHangOBJ> lst = lkh.GetListDanhSachLoaiKhachHang(userinfo.ID_QLLH);

                    response = Request.CreateResponse(HttpStatusCode.OK, lst);
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
        public HttpResponseMessage getbyid([FromUri] int id)
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
                    LoaiKhachHangDB lkh = new LoaiKhachHangDB();
                    LoaiKhachHangOBJ lst = lkh.GetLoaiKhachHangById(id);

                    response = Request.CreateResponse(HttpStatusCode.OK, lst);
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
        public HttpResponseMessage Delete([FromUri] int ID)
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
                    LoaiKhachHangDB lkh_dl = new LoaiKhachHangDB();
                    LoaiKhachHangOBJ item = lkh_dl.GetLoaiKhachHangById(ID);
                    if (lkh_dl.Xoa(item))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("themmoi")]
        public HttpResponseMessage Add([FromBody] LoaiKhachHangOBJ paramlkh)
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
                    LoaiKhachHangDB lkh_dl = new LoaiKhachHangDB();
                    try
                    {
                        paramlkh.ID_QLLH = userinfo.ID_QLLH;
                        if (paramlkh.ID_LoaiKhachHang > 0)
                        {
                            if (lkh_dl.Sua(paramlkh))
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                            else
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                        else
                        {
                            if (lkh_dl.Them(paramlkh) > 0)
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        response = Request.CreateResponse(HttpStatusCode.NotModified);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
    }
}
