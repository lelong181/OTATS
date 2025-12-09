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
using LSPos_Data.Data;
using LSPos_Data.Models;

namespace LSPosMVC.Controllers
{
    /// <summary>
    /// Dongnn edit 2019-07-11
    /// </summary>
    [Authorize]
    [RoutePrefix("api/baocaodoanhthu")]
    public class BaoCaoDoanhThuController : ApiController
    {

        /// <summary>
        ///  Báo cáo doanh thu theo nhân viên
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("baocaodoanhthunhanvien")]
        public HttpResponseMessage BaoCaoDoanhThuTheoNhanVien([FromBody] RequestBaoCaoDoanhThu request)
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
                    BaoCaoDoanhThuData bcdt = new BaoCaoDoanhThuData();
                    List<BaoCaoDoanhThuModel> rs = bcdt.GetBaoCaoDoanhThu_NhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy, request.FromMonth, request.FromYear, request.ToMonth, request.ToYear, request.ID_NhanVien, 0, request.ID_KhachHang);
                    List<double> chartvalue = new List<double>();
                    List<string> chartaxis = new List<string>();
                    foreach (BaoCaoDoanhThuModel bc in rs)
                    {
                        chartvalue.Add(bc.TongTien);
                        chartaxis.Add(bc.TimeText);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { griddata = rs, chartValue = chartvalue, chartAxis = chartaxis });
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
        /// Filter
        /// </summary>

        public class RequestBaoCaoDoanhThu
        {
            public int ID_NhanVien { get; set; }
            public int ID_KhachHang { get; set; }
            public string FromMonth { get; set; }
            public string ToMonth { get; set; }
            public string FromYear { get; set; }
            public string ToYear { get; set; }
        }
    }
}
