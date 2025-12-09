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
    [RoutePrefix("api/congty")]
    public class CongTyController : ApiController
    {
        [HttpGet]
        [Route("getthongtin")]
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
                    CongTyOBJ cty = CongTyDB.ThongTinCongTyByID(userinfo.ID_QLLH);

                    ThongTinCongTy thongTinCongTy = new ThongTinCongTy();
                    thongTinCongTy.IsAdmin = userinfo.IsAdmin;
                    thongTinCongTy.Username = userinfo.Username;
                    thongTinCongTy.TenAdmin = userinfo.TenAdmin;
                    thongTinCongTy.ThoiHanHopDong = cty.thoihanhopdong;
                    thongTinCongTy.TenCongTy = cty.tencongty;

                    response = Request.CreateResponse(HttpStatusCode.OK, thongTinCongTy);
                }
            }
            catch (Exception ex) {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex); }

            return response;
        }

        public class ThongTinCongTy
        {
            public bool IsAdmin { get; set; }
            public string Username { get; set; }
            public string TenAdmin { get; set; }
            public DateTime ThoiHanHopDong { get; set; }
            public string TenCongTy { get; set; }
        }
    }
}
