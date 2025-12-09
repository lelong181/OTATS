using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Aspose.Cells;
using System.Reflection;
using System.Configuration;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using LSPos_Data.Models;
using LSPos_Data.Data;
using LSPosMVC.Models.Models_Filter;


namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/baocaonhanvienviengthamtuyen")]
    public class BaoCaoNhanVienViengThamController : ApiController
    {
        /// <summary>
        /// Tiêu chí lọc
        /// </summary>

        /// <summary>
        /// Get data báo cáo viếng thăm tuyến chi tiết
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getdatabaocaochitiet")]
        public HttpResponseMessage GetDataBaoCao([FromBody] TieuChiLoc param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

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
                    BaoCaoNhanVienViengThamData data = new BaoCaoNhanVienViengThamData();
                    List<BaoCaoNhanVienViengThamModel> databaocao = data.GetDataBaoCao(userinfo.ID_QLLH, 0, param.ID_NhanVien, param.TuNgay, param.DenNgay);
                    response = Request.CreateResponse(HttpStatusCode.OK, databaocao);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        /// <summary>
        /// Get data báo cáo số lượng chi tiết theo tuyến 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getdatabaocaosoluong")]
        public HttpResponseMessage GetDataBaoCaoSoLuong([FromBody] TieuChiLoc param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

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
                    BaoCaoNhanVienViengThamData data = new BaoCaoNhanVienViengThamData();
                    List<BaoCaoSoLuongNhanVienViengThamModel> databaocao = data.GetDataBaoCaoSoLuong(userinfo.ID_QLLH, 0, param.ID_NhanVien, param.TuNgay, param.DenNgay);
                    response = Request.CreateResponse(HttpStatusCode.OK, databaocao);
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
