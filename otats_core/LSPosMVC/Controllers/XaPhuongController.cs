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

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/xaphuong")]
    [EnableCors(origins: "*", "*", "*")]
    public class XaPhuongController : ApiController
    {
        [HttpGet]
        [Route("getbyidquan")]
        public HttpResponseMessage get([FromUri] int IdQuan)
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
                    Phuong_dl phuong = new Phuong_dl();
                    List<Phuong> dsphuong = phuong.GetPhuongTheoQuan(IdQuan).OrderBy(x => x.TenPhuong).ToList();

                    response = Request.CreateResponse(HttpStatusCode.OK, dsphuong);
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
