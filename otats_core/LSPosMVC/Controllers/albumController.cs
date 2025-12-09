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
    [RoutePrefix("api/album")]
    public class albumController : ApiController
    {
        [HttpGet]
        [Route("getalbum")]
        public HttpResponseMessage getalbum([FromUri] int id_album, int id_image, int id_checkin, int id_baoduong)
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
                    if(id_image > 0)
                    {
                        ImageOBJ data = ImageDB.GetImageById(id_image);
                        response = Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else if (id_checkin > 0)
                    {
                        List<ImageOBJ> data = ImageDB.DanhSachAnh_ByIDCheckIn(id_checkin);
                        response = Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else if (id_baoduong > 0)
                    {
                        List<ImageOBJ> data = ImageDB.DanhSachAnh_ByIDLichSu(id_baoduong);
                        response = Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else
                    {
                        AlbumOBJ data = ImageDB.GetAlbumById(id_album);
                        response = Request.CreateResponse(HttpStatusCode.OK, data);
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
