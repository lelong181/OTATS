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
using System.Web.Http.Cors;
using LSPosMVC.Models.Models_Filter;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/quanhuyen")]
    [EnableCors(origins: "*", "*", "*")]
    public class QuanController : ApiController
    {

        [HttpGet]
        [Route("getbyidtinh")]
        public HttpResponseMessage get([FromUri] TieuChiLoc tieuchi) 
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
                    Quan_dl quan_dl = new Quan_dl();
                    List<Quan> dsQuan = quan_dl.GetQuanTheoTinh(tieuchi.IdTinh).OrderBy(x => x.TenQuan).ToList();

                    response = Request.CreateResponse(HttpStatusCode.OK, dsQuan);
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
