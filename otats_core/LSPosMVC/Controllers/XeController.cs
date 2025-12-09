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
    [RoutePrefix("api/xe")]
    [EnableCors(origins: "*", "*", "*")]
    public class XeController : ApiController
    {
        [HttpGet]
        [Route("getall")]
        public DataSourceResult getall(HttpRequestMessage requestMessage)
        {
            DataSourceRequest param = JsonConvert.DeserializeObject<DataSourceRequest>(
             // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
             requestMessage.RequestUri.ParseQueryString().GetKey(0)
         );
            UserInfo userinfo = utilsCommon.checkAuthorization();

            XeDB km_dl = new XeDB();

            FilterGrid filter = new FilterGrid();
            filter.BienSo = "";
            filter.ChuKyBD = 0;
            filter.NamSX = 0;
            filter.NgayBDGanNhat = new DateTime(1900, 1, 1);
            filter.NgayBDTiepTheo = new DateTime(1900, 1, 1);
            filter.SoCho = 0;
            filter.TenNhanVien = "";
            int tongso = 0;
            if (param.Filter != null)
            {
                foreach (Filter f in param.Filter.Filters)
                {
                    switch (f.Field)
                    {
                        case "bienKiemSoat":
                            filter.BienSo = f.Value.ToString();
                            break;
                        case "tenNhanVien":
                            filter.TenNhanVien = f.Value.ToString();
                            break;
                        case "namSX":
                            filter.NamSX = int.Parse(f.Value.ToString());
                            break;
                        case "ngayBDGanNhat":
                            filter.NgayBDGanNhat = Convert.ToDateTime(f.Value.ToString());
                            break;
                        case "ngayBDTiepTheo":
                            filter.NgayBDTiepTheo = Convert.ToDateTime(f.Value.ToString());
                            break;
                        case "chuKy":
                            filter.ChuKyBD = int.Parse(f.Value.ToString());
                            break;
                        case "soCho":
                            filter.SoCho = int.Parse(f.Value.ToString());
                            break;
                    }
                }
            }

            List<XeDTO> data = new XeData().GetDSXe(userinfo.ID_QLLH, filter.BienSo, filter.NamSX, filter.TenNhanVien, filter.SoCho, filter.NgayBDGanNhat, filter.NgayBDTiepTheo, ref tongso);
            DataSourceResult s = new DataSourceResult();
            s.Data = data;
            s.Total = tongso;
            s.Aggregates = null;
            return s;
        }

        public class FilterGrid
        {
            public string BienSo { get; set; }
            public string TenNhanVien { get; set; }
            public int NamSX { get; set; }
            public DateTime NgayBDGanNhat { get; set; }
            public DateTime NgayBDTiepTheo { get; set; }
            public int ChuKyBD { get; set; }
            public int SoCho { get; set; }
        }

        [HttpGet]
        [Route("getalldata")]
        public HttpResponseMessage getalldata()
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
                    XeDB xedb = new XeDB();
                    DataTable dt = xedb.GetDataDanhSach(userinfo.ID_QLLH);
                    XeData xdt = new XeData();
                    List<XeDTO> list = xdt.GetDataFromRow(dt);
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
        [Route("ExportExcelXe")]
        public HttpResponseMessage ExportExcelXe()
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
                    XeDB xedb = new XeDB();
                    DataTable xe = xedb.GetDataDanhSach(userinfo.ID_QLLH);
                    xe.TableName = "DATA";

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    dt2.TableName = "DATA2";
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    //DataTable dt1 = new DataTable();
                    //dt1.TableName = "DATA1";
                    //dt1.Columns.Add("TITLE", typeof(String));
                    //DataRow row = dt1.NewRow();
                    //row["TITLE"] = "";
                    //dt1.Rows.Add(row);

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(xe.Copy());
                    //dataSet.Tables.Add(dt1.Copy());
                    dataSet.Tables.Add(dt2.Copy());

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM010_QuanLyXeNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("ExcelXe.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM010_ManageEmployeeVehicles_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("ExcelXe_en.xls", dataSet, null, ref stream);
                    }

                    response.Content = new ByteArrayContent(stream.ToArray());
                    response.Content.Headers.Add("x-filename", filename);
                    response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = filename;
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;

        }

        [HttpGet]
        [Route("getbyid")]
        public HttpResponseMessage getbyid([FromUri] int ID)
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
                    XeDB xedb = new XeDB();
                    XeOBJ item = xedb.GetById(ID);
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public class LichSuSuDung
        {
            public int ID_NhanVien { get; set; }
            public string TenNhanVien { get; set; }
            public string BienKiemSoat { get; set; }
            public DateTime ApDungTuNgay { get; set; }
            public DateTime ApDungDenNgay { get; set; }
        }

        public class LichSuBaoDuong
        {
            public int ID_Xe_LichSuBaoDuong { get; set; }
            public string BienKiemSoat { get; set; }
            public DateTime NgayBaoDuong { get; set; }
            public int SoLuongAnh { get; set; }
            public string DiaDiemBaoDuong { get; set; }
            public string DiaChiBaoDuong { get; set; }
            public string NoiDung { get; set; }
            public double ChiPhi { get; set; }
            public DateTime NgayBDTiepTheo { get; set; }
        }

        [HttpGet]
        [Route("getlichsudung")]
        public HttpResponseMessage GetLichSuDung([FromUri] int ID)
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
                    XeDB xedb = new XeDB();
                    DataTable dt = xedb.GetLichSuSuDung(ID);
                    List<LichSuSuDung> result = new List<LichSuSuDung>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        LichSuSuDung h = new LichSuSuDung();
                        h.ID_NhanVien = (dr["ID_NhanVien"] != null) ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
                        h.ApDungDenNgay = dr["ApDungDenNgay"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ApDungDenNgay"]);
                        h.ApDungTuNgay = dr["ApDungTuNgay"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ApDungTuNgay"]);
                        h.BienKiemSoat = (dr["BienKiemSoat"] != null) ? dr["BienKiemSoat"].ToString() : "";
                        h.TenNhanVien = (dr["TenNhanVien"] != null) ? dr["TenNhanVien"].ToString() : "";
                        result.Add(h);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        [Route("getlichbaoduong")]
        public HttpResponseMessage GetLichBaoDuong([FromUri] int ID)
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
                    XeDB xedb = new XeDB();
                    DataTable dt = xedb.GetLichSuBaoDuong(ID);
                    List<LichSuBaoDuong> result = new List<LichSuBaoDuong>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        LichSuBaoDuong h = new LichSuBaoDuong();
                        h.ID_Xe_LichSuBaoDuong = (dr["ID_Xe_LichSuBaoDuong"] != null) ? int.Parse(dr["ID_Xe_LichSuBaoDuong"].ToString()) : 0;
                        h.NgayBaoDuong = dr["NgayBaoDuong"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayBaoDuong"]);
                        h.NgayBDTiepTheo = dr["NgayBDTiepTheo"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayBDTiepTheo"]);
                        h.BienKiemSoat = (dr["BienKiemSoat"] != null) ? dr["BienKiemSoat"].ToString() : "";
                        h.ChiPhi = (dr["ChiPhi"] != null) ? double.Parse(dr["ChiPhi"].ToString()) : 0;
                        h.DiaChiBaoDuong = (dr["DiaChiBaoDuong"] != null) ? dr["DiaChiBaoDuong"].ToString() : "";
                        h.DiaDiemBaoDuong = (dr["DiaDiemBaoDuong"] != null) ? dr["DiaDiemBaoDuong"].ToString() : "";
                        h.NoiDung = (dr["NoiDung"] != null) ? dr["NoiDung"].ToString() : "";
                        h.SoLuongAnh = (dr["SoLuongAnh"] != null) ? int.Parse(dr["SoLuongAnh"].ToString()) : 0;
                        result.Add(h);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        public HttpResponseMessage Create([FromBody] XeOBJ item)
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
                    XeDB xedb = new XeDB();
                    item.NgayTao = new DateTime();
                    item.ID_QLLH = userinfo.ID_QLLH;
                    if (xedb.Them(item))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_taoxethanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_trungbienkiemsoatvuilongthulai" });
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
        [Route("update")]
        public HttpResponseMessage Update([FromBody] XeOBJ item)
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
                    XeDB xedb = new XeDB();
                    item.ID_QLLH = userinfo.ID_QLLH;
                    {

                        if (xedb.Sua(item, userinfo.ID_QuanLy))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_suathongtinxethanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_trungbienkiemsoatvuilongthulai" });

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
                    bool flag = true;
                    int numsucsess = 0;
                    int numfail = 0;
                    XeDB xedb = new XeDB();
                    foreach (int ID in Ids)
                    {
                        if (xedb.Xoa(ID, userinfo.ID_QuanLy))
                            numsucsess += 1;
                        else
                        {
                            flag = false;
                            numfail += 1;
                        }
                    }

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    string msg = "";
                    string msg_beg = "";
                    string msg_end = "";
                    if (lang == "vi")
                    {
                        msg_beg = "Xóa xe thành công ";
                        msg_end = " xe!";
                    }
                    else
                    {
                        msg_beg = "Deleted successfully to ";
                        msg_end = " vehicle(s)!";
                    }
                    
                    if (flag)
                    {
                        msg = msg_beg + numsucsess.ToString() + msg_end;
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = msg });
                    }
                    else
                    {
                        msg = msg_beg + numfail.ToString() + msg_end;
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = msg });
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
