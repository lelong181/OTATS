using LSPosMVC.Common;
using LSPos_Data.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/kenhbanhang")]
    public class KenhBanHangController : ApiController
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
                    KenhBanHangDB kenhBanHangDB = new KenhBanHangDB();
                    List<KenhBanHangOBJ> lst = KenhBanHangDB.GetListKenhBanHang(userinfo.ID_QLLH);

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

        public class KenhCapTren
        {
            public int ID_KhachHang { get; set; }
            public string TenKhachHang { get; set; }
        }

        [HttpGet]
        [Route("getkenhcaptren")]
        public HttpResponseMessage getkenhcaptren([FromUri] int idkenh)
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
                    List<KenhCapTren> lst = new List<KenhCapTren>();

                    KhachHang_dl kh_dl = new KhachHang_dl();
                    DataTable DSNV = kh_dl.GetDataKhachHangBy_IDNhomKH(idkenh);

                    foreach(DataRow dr in DSNV.Rows)
                    {
                        KenhCapTren kenhCapTren = new KenhCapTren();
                        kenhCapTren.ID_KhachHang = Convert.ToInt32(dr["ID_KhachHang"].ToString());
                        kenhCapTren.TenKhachHang = dr["TenKhachHang"].ToString();
                        lst.Add(kenhCapTren);
                    }

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
    }
}
