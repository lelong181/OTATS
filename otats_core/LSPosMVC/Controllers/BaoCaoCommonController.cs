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

namespace LSPosMVC.Controllers
{
    /// <summary>
    /// DONGNN -2019-07-15
    /// </summary>
    [Authorize]
    [RoutePrefix("api/ExportReport")]
    public class BaoCaoCommonController : ApiController
    {
        #region BÁO CÁO KHÁCH HÀNG

        //BÁO CÁO TỔNG HỢP THEO KHÁCH HÀNG
        [HttpGet]
        [Route("ExcelBaoCaoTongHopTheoKhachHang")]
        public HttpResponseMessage ExcelBaoCaoTongHopTheoKhachHang([FromUri] int ID_KhachHang, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoTongHopTheoKhachHang(userinfo.ID_QLLH, ID_KhachHang, fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM033_TongHopDonHangTheoKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopTheoKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM033_SummaryReportBasedOnCustomer_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopTheoKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO MẶT HÀNG - KHÁCH HÀNG - ĐƠN HÀNG
        [HttpGet]
        [Route("ExcelBaoCaoMH_KH_DH_NV")]
        public HttpResponseMessage ExcelBaoCaoMH_KH_DH_NV([FromUri] int id_MatHang, int id_KhachHang, DateTime fromdate, DateTime todate, int id_NhanVien)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoMH_KH_DH_NV(userinfo.ID_QLLH, id_MatHang, id_KhachHang, fromdate, todate, userinfo.ID_QuanLy, id_NhanVien);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM034_BaoCaoKhachHangMaThangDonHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_MH_KH_DH_NV.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM034_CustomerProductOrderReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_MH_KH_DH_NV_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO MẶT HÀNG - KHÁCH HÀNG
        [HttpGet]
        [Route("ExcelBaoCaoMatHang_KH")]
        public HttpResponseMessage ExcelBaoCaoMatHang_KH([FromUri] int id_MatHang, int id_KhachHang, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoMH_KH(userinfo.ID_QLLH, id_MatHang,0, id_KhachHang,"", fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM035_BaoCaoKhachHangMatHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_MatHang_KH.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM035_CustomerProductReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_MatHang_KH_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO ẢNH CHỤP
        [HttpGet]
        [Route("ExcelBaoCaoAnhChupTheoAlbum")]
        public HttpResponseMessage ExcelBaoCaoAnhChupTheoAlbum([FromUri] int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoAnhChupTheoAlbum(userinfo.ID_QLLH, id_NhanVien, id_KhachHang, fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM036_BaoCaoAnhChup_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_AnhChup.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM036_PhotosReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_AnhChup_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO TỔNG HỢP VÀO ĐIỂM THEO KHÁCH HÀNG
        [HttpGet]
        [Route("ExcelBaoCaoTongHopCheckInTheoKhachHang")]
        public HttpResponseMessage ExcelBaoCaoTongHopCheckInTheoKhachHang([FromUri] int id_Nhom, int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoTongHopCheckInTheoKhachHang(userinfo.ID_QLLH, id_NhanVien, fromdate, todate, userinfo.ID_QuanLy, id_KhachHang, id_Nhom);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM037_BaoCaoTongHopVaoDiemTheoKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopVaoDiemTheoKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM037_TotalCheckinBasedOnCustomerReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopVaoDiemTheoKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO VIẾNG THĂM KHÁCH HÀNG
        [HttpGet]
        [Route("ExcelBaoCaoViengThamTheoKhachHang")]
        public HttpResponseMessage ExcelBaoCaoViengThamTheoKhachHang([FromUri] int id_Nhom, int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoViengThamKhachHang(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_Nhom, id_KhachHang, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM038_BaoCaoViengThamKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoViengThamKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM038_CustomerVisitedReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoViengThamKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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
        //BÁO CÁO KHÁCH HÀNG MỞ MỚI
        [HttpGet]
        [Route("ExcelBaoCaoKhachHangMoMoi")]
        public HttpResponseMessage BaoCaoKhachHangMoMoi([FromUri]  int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate, int soDonHang, double giaTriDonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoKhachHangMoMoi(id_NhanVien, id_KhachHang, userinfo.ID_QuanLy, userinfo.ID_QLLH, fromdate, todate, soDonHang, giaTriDonHang);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM039_BaoCaoKhachHangMoMoi_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KhachHangMoMoi.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM039_NewlyOperatingCustomerReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KhachHangMoMoi_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO KHÁCH HÀNG THEO GIAO DỊCH
        [HttpGet]
        [Route("ExcelBaoCaoKhachHangTheoGiaoDich")]
        public HttpResponseMessage ExcelBaoCaoKhachHangTheoGiaoDich([FromUri]  int id_NhanVien, int id_KhachHang, int soNgay, int loai)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoKhachHangTheoGiaoDich(id_NhanVien, id_KhachHang, userinfo.ID_QuanLy, userinfo.ID_QLLH, soNgay, loai);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);


                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = "";
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM040_BaoCaoKhachHangTheoGiaoDich_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Reprot_KhachHangTheoGiaoDich.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM040_CustomerTransactionReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Reprot_KhachHangTheoGiaoDich_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO PHẢN HỒI
        [Route("ExcelBaoCaoPhanHoi")]
        [HttpGet]
        public HttpResponseMessage ExcelBaoCaoPhanHoi([FromUri]  int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoPhanHoi(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_NhanVien, id_KhachHang, fromdate, todate, 0, 0);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM041_BaoCaoPhanHoi_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_PhanHoi.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM041_FeedbackReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_PhanHoi_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO KHÁCH HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("ExcelBaoCaoKhachHangTheoKhuVuc")]
        public HttpResponseMessage ExcelBaoCaoKhachHangTheoKhuVuc([FromUri]  int id_Tinh, int id_Quan)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoKhachHangTheoKhuVuc(userinfo.ID_QLLH, id_Tinh, id_Quan);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);


                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = "";
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM042_BaoCaoKhachHangTheoKhuVuc_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KhachHangTheoKhuVuc.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM042_CustomerInAreaReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KhachHangTheoKhuVuc_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        #endregion

        #region BÁO CÁO NHÂN VIÊN
        //BÁO CÁO LỊCH SỬ GIAO HÀNG
        [HttpGet]
        [Route("ExcelBaoCaoLichSuGiaoHang")]
        public HttpResponseMessage ExcelBaoCaoLichSuGiaoHang([FromUri] int id_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoLichSuGiaoHang(userinfo.ID_QLLH, id_KhachHang, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM043_BaoCaoLichSuGiaoHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_LichSuGiaoHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM043_DeliveryHistoryReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_LichSuGiaoHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO LỊCH SỬ BẢO DƯỠNG SỬA CHỮA
        [HttpGet]
        [Route("ExcelBaoCaoLichSuBaoDuongSuaChua")]
        public HttpResponseMessage ExcelBaoCaoLichSuBaoDuongSuaChua([FromUri] int id_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoLichSuBaoDuongSuaChua(fromdate, todate, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM044_BaoCaoLichSuSuaChua_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_LichSuSuaChua.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM044_MaintenanceHistoryReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_LichSuSuaChua_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO KPI NHÂN VIÊN
        [HttpGet]
        [Route("ExcelBaoCaoKPINhanVien")]
        public HttpResponseMessage ExcelBaoCaoKPINhanVien([FromUri] int id_Nhom, int id_NhanVien, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoKPINhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_Nhom, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM045_BaoCaoKPINhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KPINhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM045_EmployeeKPIReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KPINhanVien_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO ĐƠN HÀNG THEO ĐIỂM
        [HttpGet]
        [Route("ExcelBaoCaoDonHangTheoDiem")]
        public HttpResponseMessage ExcelBaoCaoDonHangTheoDiem([FromUri] int id_Nhom, int id_NhanVien, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoDonHangTheoDiem(userinfo.ID_QLLH, id_NhanVien, fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM046_BaoCaoDonHangTheoDiem_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_DonHangTheoDiem.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM046_OrderBasedOnCheckinReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_DonHangTheoDiem_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO KPI HOÀN THÀNH ĐƯỢC TÍNH DỰA THEO ĐƠN HÀNG CƠ BẢN VÀ THỰC TẾ
        [HttpGet]
        [Route("ExcelbaoCaoHoanThanhDonHangCoBanThucTe")]
        public HttpResponseMessage ExcelbaoCaoHoanThanhDonHangCoBanThucTe([FromUri] int id_KhachHang, int id_NhanVien, int id_MatHang, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommon bc = new BaoCaoCommon();
                    DataSet ds = bc.BaoCaoHoanThanhTheoDonHangCoBanVaThucTe(userinfo.ID_QLLH, id_KhachHang, id_NhanVien, id_MatHang, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM047_BaoCaoKPINhanVienDonHangCoBan_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_HoanThanhDuaTrenDonHangThucTe.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM047_EmployeeOrderBasicKPIReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_HoanThanhDuaTrenDonHangThucTe_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO MẶT HÀNG - NHÂN VIÊN
        [HttpGet]
        [Route("ExcelBaoCaoMatHang_NV")]
        public HttpResponseMessage ExcelBaoCaoMatHang_NV([FromUri] int id_KhachHang, int id_NhanVien, int id_MatHang, int id_NganhHang, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BaoCaoMH_NV_New(userinfo.ID_QLLH, id_MatHang, id_NhanVien, fromdate, todate, userinfo.ID_QuanLy, id_NganhHang);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM048_BaoCaoTongHopMatHangTheoNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_MatHangTheoNhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM048_ProductByEmployeeSummaryReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_MatHangTheoNhanVien_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO QUÃNG ĐƯỜNG ĐI
        [HttpGet]
        [Route("ExcelBaoCaoQuangDuongDiChuyen")]
        public HttpResponseMessage ExcelBaoCaoQuangDuongDiChuyen([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BaoCaoQuangDuongDiChuyenExcel(userinfo.ID_QLLH, id_NhanVien, fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM049_BaoCaoKMDiChuyen_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_QuangDuongDiChuyen.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM049_DistanceTravelledKMReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_QuangDuongDiChuyen_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO DỪNG ĐỖ
        [HttpGet]
        [Route("ExcelBaoCaoDungDo")]
        public HttpResponseMessage ExcelBaoCaoDungDo([FromUri] int id_NhanVien, float dungDo, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BaoCaoDungDo(id_NhanVien, userinfo.ID_QuanLy, userinfo.ID_QLLH, fromdate, todate, dungDo);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM050_BaoCaoDungDo_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_DungDo.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM050_StopByReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_DungDo_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        // BÁO CÁO BẬT TẮT GPS
        [HttpGet]
        [Route("ExcelBaoCaoBatTatGPS")]
        public HttpResponseMessage ExcelBaoCaoBatTatGPS([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    // public DataTable BaoCaoTatBatGPS(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, int Loai);
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BaoCaoTatBatGPS(id_NhanVien, userinfo.ID_QuanLy, userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM051_BaoCaoTinhTrangGPS_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BatTatGPS.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM051_GPSStatusReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BatTatGPS_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO TÌNH TRẠNG FAKE GPS
        [HttpGet]
        [Route("ExcelBaoCaoBatTatFakeGPS")]
        public HttpResponseMessage ExcelBaoCaoBatTatFakeGPS([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    //Loai  --0 : tat fake, 1 : bat fake, -1 tat ca
                    // DataTable dsMatGPS = bc.BaoCaoTatBatFakeGPS(NhanVienDropDown.SelectedItem != null ? Convert.ToInt32(NhanVienDropDown.SelectedItem.Value) : 0, userinfo.ID_QuanLy, userinfo.ID_QLLH, dtFrom.Date, dtTo.Date, 1);
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BaoCaoTatBatFakeGPS(id_NhanVien, userinfo.ID_QuanLy, userinfo.ID_QLLH, fromdate, todate, 1);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM052_BaoCaoTinhTrangFakeGPS_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TinhTrangFakeGPS.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM052_FakeGPSStatusReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TinhTrangFakeGPS_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO VIẾNG THĂM TUYẾN
        [HttpGet]
        [Route("ExcelBaoCaoCheckInTuyen")]
        public HttpResponseMessage ExcelBaoCaoCheckInTuyen([FromUri]  int id_Tuyen, int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    //Loai  --0 : tat fake, 1 : bat fake, -1 tat ca
                    // DataTable dsMatGPS = bc.BaoCaoTatBatFakeGPS(NhanVienDropDown.SelectedItem != null ? Convert.ToInt32(NhanVienDropDown.SelectedItem.Value) : 0, userinfo.ID_QuanLy, userinfo.ID_QLLH, dtFrom.Date, dtTo.Date, 1);
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BaoCaoCheckInTuyen(userinfo.ID_QLLH, id_Tuyen, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM053_BaoCaoViengThamTheoTuyen_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_ViengThamTheoTuyen.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM053_VisitsByRouteReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_ViengThamTheoTuyen_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //LỊCH SỬ MẤT TÍN HIỆU
        [HttpGet]
        [Route("ExcelBaoCaoLichSuMatTinHieu")]
        public HttpResponseMessage ExcelBaoCaoLichSuMatTinHieu([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.LichSuMatTinHieu(userinfo.ID_QLLH, id_NhanVien, fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM054_BaoCaoLichSuMatTinHieu_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_LichSuaMatTinHieu.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM054_LostSignalHistoryReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_LichSuaMatTinHieu_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO KPI CÔNG VIỆC CHI TIẾT
        [HttpGet]
        [Route("ExcelBieuDoChiTieuKPICongViec")]
        public HttpResponseMessage ExcelBieuDoChiTieuKPICongViec([FromUri]  int id_Nhom, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BieuDoChiTieuKPICongViec(id_Nhom, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";
                        ds.Tables[1].TableName = "DATA1";
                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt = new DataTable();
                        dt = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt.TableName = "DATA2";
                        ds.Tables.Add(dt.Copy());
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        excel.ExportTemplateToStreamGird("Report_KPICongViecChiTiet.xls", ds, null, ref stream);
                        string filename = "BaoCaoKPICongViecChiTiet" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO TỔNG HỢP THỜI GIAN ĐĂNG NHẬP - ĐĂNG XUẤT
        [HttpGet]
        [Route("Excelbaocaotonghopravaodiem")]
        public HttpResponseMessage Excelbaocaotonghopravaodiem([FromUri]int id_Nhom, int id_NhanVien, DateTime to, DateTime from)
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
                    BaoCaoCommon bc = new BaoCaoCommon();
                    DataSet ds = bc.BaoCaoTongHopRaVaoDiem(userinfo.ID_QLLH, id_Nhom, id_NhanVien, to, from);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + from.ToString("dd/MM/yyyy") + " đến " + to.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + from.ToString("dd/MM/yyyy") + " to " + to.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM055_BaoCaoTongHopRaVaoDiem_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopVaoRaDiem.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM055_CheckinCheckoutSummaryReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopVaoRaDiem_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //KẾ HOẠCH LỊCH HẸN
        [HttpGet]
        [Route("ExcelBaoCaoLichHen")]
        public HttpResponseMessage ExcelBaoCaoLichHen([FromUri]int ID_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.GetDanhSachLichHen(id_NhanVien, ID_KhachHang, fromdate, todate, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM056_BaoCaoLichHen_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KeHoachLichHen.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM056_AppointmentReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_KeHoachLichHen_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        // Báo cáo viếng thăm khách hàng theo tuyến
        #region  Báo cáo viếng thăm khách hàng theo tuyến
        [HttpGet]
        [Route("ExcelbaoCaoViengThamKhachHangTheoTuyenChiTiet")]
        public HttpResponseMessage ExcelbaoCaoViengThamKhachHangTheoTuyenChiTiet([FromUri]int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.baoCaoViengThamKhachHangTheoTuyenChiTiet(userinfo.ID_QLLH, 0, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM057_BaoCaoViengThamKhachHangTheoTuyen_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_ViengThamKhachHangTheoTuyen.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM057_VisitedCustomerByRouteReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_ViengThamKhachHangTheoTuyen_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        [HttpGet]
        [Route("ExcelbaoCaoViengThamKhachHangTheoTuyenSoLuong")]
        public HttpResponseMessage ExcelbaoCaoViengThamKhachHangTheoTuyenSoLuong([FromUri]int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.baoCaoViengThamKhachHangTheoTuyenSoLuong(userinfo.ID_QLLH, 0, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM057_BaoCaoViengThamKhachHangTheoTuyen_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_ViengThamKhachHangTheoTuyenTheoSoLuong.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM057_VisitedCustomerByRouteReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_ViengThamKhachHangTheoTuyenTheoSoLuong_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        #endregion

        //BÁO CÁO CÔNG VIỆC NHÂN VIÊN
        [HttpGet]
        [Route("BaoCaoCongViecNhanVien")]
        public HttpResponseMessage BaoCaoCongViecNhanVien([FromUri]int id_Nhom, int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet ds = bc.BaoCaoCongViecNhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_Nhom, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM058_BaoCaoTongHopCongViecNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_CongViecNhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM058_EmployeeTaskSummaryReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_CongViecNhanVien_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        #endregion

        #region BIỂU ĐỒ SẢN LƯỢNG
        //BIỂU ĐỒ TOP 10 SẢN PHẨM BÁN CHẠY NHẤT

        [HttpGet]
        [Route("ExcelBieuDoTopTenSanPhamTheoKhachHang")]
        public HttpResponseMessage ExcelBieuDoTopTenSanPhamTheoKhachHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM059_Top10SanPhamBanChay_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenSanPhamTheoKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM059_Top10BestSellerProduct_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenSanPhamTheoKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        // BIỂU ĐỒ TOP 10 ĐƠN HÀNG THEO KHÁCH HÀNG
        [HttpGet]
        [Route("ExcelBieuDoTopTenDonHangKhachHang")]
        public HttpResponseMessage ExcelBieuDoTopTenDonHangKhachHang([FromUri] DateTime fromdate, DateTime todate, int orderby)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoBieuDoDonHangTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, orderby);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM060_Top10DonHangTheoKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDonHangKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM060_Top10OrderBasedOnCustomer_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDonHangKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        // BIỂU ĐỒ TOP 10 SẢN PHẨM BÁN THẤP NHẤT
        [HttpGet]
        [Route("ExcelBieuDoTopTenSanPhamBanThapTheoKhachHang")]
        public HttpResponseMessage ExcelBieuDoTopTenSanPhamBanThapTheoKhachHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 1);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM061_Top10SanPhamBanItNhat_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenSanPhamBanThapTheoKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM061_Top10LeastSoldProduct_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenSanPhamBanThapTheoKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        // BIỂU ĐỒ TOP 10 ĐƠN HÀNG THEO NHÂN VIÊN
        [HttpGet]
        [Route("ExcelBieuDoTopTenDonHangTheoNhanVien")]
        public HttpResponseMessage ExcelBieuDoTopTenDonHangTheoNhanVien([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoTopTenTheoNhanVien(userinfo.ID_QLLH, fromdate, todate, 1);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM062_Top10DonHangTheoNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDonHangTheoNhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM062_Top 10OrderBasedOnEmployee_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDonHangTheoNhanVien_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ ĐƠN HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("ExcelBieuDoDonHangTheoKhuVuc")]
        public HttpResponseMessage ExcelBieuDoDonHangTheoKhuVuc([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoDonHangTheoKhuVuc(0, userinfo.ID_QLLH, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM063_DonHangTheoKhuVuc_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_DonHangTheoKhuVuc.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM063_OrderBasedOnArea_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_DonHangTheoKhuVuc_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ PHÂN LOẠI KHÁCH HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("ExcelBieuDoPhanLoaiKhachHangTheoKhuVuc")]
        public HttpResponseMessage ExcelBieuDoPhanLoaiKhachHangTheoKhuVuc()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoPhanLoaiKhachHang(userinfo.ID_QLLH);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";
                        ds.Tables[1].TableName = "DATA1";
                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt = new DataTable();
                        dt = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt.TableName = "DATA2";
                        ds.Tables.Add(dt.Copy());
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        excel.ExportTemplateToStreamGird("Report_PhanLoaiKhachHangTheoKhuVuc.xls", ds, null, ref stream);
                        string filename = "BaoCao_PhanLoaiKhachHangTheoKhuVuc" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ PHÂN LOẠI KHÁCH HÀNG THEO NGÀNH HÀNG
        [HttpGet]
        [Route("ExcelBieuDoPhanLoaiKhachHangNganhHang")]
        public HttpResponseMessage ExcelBieuDoPhanLoaiKhachHangNganhHang()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoPhanLoaiKhachHangNganhHang(userinfo.ID_QLLH);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";
                        ds.Tables[1].TableName = "DATA1";
                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt = new DataTable();
                        dt = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt.TableName = "DATA2";
                        ds.Tables.Add(dt.Copy());
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        excel.ExportTemplateToStreamGird("Report_PhanLoaiKhachHangNganHang.xls", ds, null, ref stream);
                        string filename = "BaoCao_PhanLoaiKhachHangNganHang" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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
        #endregion

        #region BIỂU ĐỒ DOANH THU
        //BÁO CÁO TỔNG HỢP CHƯƠNG TRÌNH KHUYẾN MÃI

        [HttpGet]
        [Route("ExcelTonghopchuongtrinhkhuyenmai")]
        public HttpResponseMessage ExcelTonghopchuongtrinhkhuyenmai([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";
                        ds.Tables[1].TableName = "DATA1";
                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt = new DataTable();
                        dt = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt.TableName = "DATA2";
                        ds.Tables.Add(dt.Copy());
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        excel.ExportTemplateToStreamGird("Report_TopTenSanPhamTheoKhachHang.xls", ds, null, ref stream);
                        string filename = "BaoCaoTopTenSanPhamTheoKhachHang" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO CHI TIẾT CHƯƠNG TRÌNH KHUYẾN MÃI
        [HttpGet]
        [Route("ExcelChitietchuongtrinhkhuyenmai")]
        public HttpResponseMessage Chitietchuongtrinhkhuyenmai([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";
                        ds.Tables[1].TableName = "DATA1";
                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt = new DataTable();
                        dt = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt.TableName = "DATA2";
                        ds.Tables.Add(dt.Copy());
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        excel.ExportTemplateToStreamGird("Report_TopTenSanPhamTheoKhachHang.xls", ds, null, ref stream);
                        string filename = "BaoCaoTopTenSanPhamTheoKhachHang" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        // BIỂU ĐỒ TOP 10 DOANH THU THEO KHÁCH HÀNG
        [HttpGet]
        [Route("ExcelBieuDoTopTenDoanhThuKhachHang")]
        public HttpResponseMessage ExcelBieuDoTopTenDoanhThuKhachHang([FromUri] DateTime fromdate, DateTime todate, int orderby)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoBieuDoDoanhThuTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM064_Top10DoanhThuTheoKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDoanhThuTheoKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM064_Top10RevenueBasedOnCustomer_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDoanhThuTheoKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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
        //BIỂU ĐỒ TOP 10 DOANH THU THEO NHÂN VIÊN
        [HttpGet]
        [Route("ExcelBieuDoTopTenDoanhThuTheoNhanVien")]
        public HttpResponseMessage ExcelBieuDoTopTenDoanhThuTheoNhanVien([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoTopTenTheoNhanVien(userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM065_Top10DoanhThuTheoNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDoanhThuTheoNhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM065_Top10RevenueBasedOnEmployee_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TopTenDoanhThuTheoNhanVien_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ DOANH THU THÁNG
        [HttpGet]
        [Route("ExcelBieuDoDoanhThuThang")]
        public HttpResponseMessage ExcelBieuDoDoanhThuThang([FromUri]  DateTime fromdate, int id_Nhom, int id_NhanVien, int id_KhachHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoDoanhThuNgay_V2(userinfo.ID_QLLH, fromdate, userinfo.ID_QuanLy, id_Nhom, id_KhachHang, id_NhanVien);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = "";
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM066_DoanhThuThang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("ReportBieuDoDoanhThuThang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM066_MonthlyRevenue_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("ReportBieuDoDoanhThuThang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ DOANH THU THEO NHÓM NHÂN VIÊN
        [HttpGet]
        [Route("ExcelBieuDoDoanhThuTheoNhomNhanVien")]
        public HttpResponseMessage ExcelBieuDoDoanhThuTheoNhomNhanVien([FromUri]  DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoDanhThuDAL bc_dl = new BieuDoDanhThuDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoDoanhThuTheoNhomNhanVien(userinfo.ID_QLLH, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM067_DoanhThuTheoNhomNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoDoanhThuTheoNhomNhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM067_RevenueBasedOnEmployee Group_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoDoanhThuTheoNhomNhanVien_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ DOANH THU THEO KHU VỰC

        [HttpGet]
        [Route("ExcelBieuDoDoanhThuTheoKhuVuc")]
        public HttpResponseMessage ExcelBieuDoDoanhThuTheoKhuVuc([FromUri]  DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoDanhThuDAL bc_dl = new BieuDoDanhThuDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoDoanhThuTheoKhuVuc(userinfo.ID_QLLH, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {

                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM068_DoanhThuTheoKhuVuc_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoDoanhThuTheoKhuVuc.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM068_RevenueBasedOnArea_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoDoanhThuTheoKhuVuc_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ DOANH THU NHÂN VIÊN
        [HttpGet]
        [Route("ExcelBieuDoDoanhThuNhanVien")]
        public HttpResponseMessage ExcelBieuDoDoanhThuNhanVien([FromUri]  DateTime fromdate, DateTime todate, int id_Nhom, int id_NhanVien)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoDoanhThuTongHopNhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_Nhom, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM069_DoanhThuTheoNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoDoanhThuNhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM069_RevenueBasedOnEmployee_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoDoanhThuNhanVien_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BÁO CÁO BÁN HÀNG THEO KHÁCH HÀNG

        [HttpGet]
        [Route("ExcelBaoCaoBanHang")]
        public HttpResponseMessage ExcelBaoCaoBanHang([FromUri]  DateTime fromdate, DateTime todate, int id_KhachHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoDanhThuDAL bc_dl = new BieuDoDanhThuDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoBanHang_KhachHang(userinfo.ID_QLLH, id_KhachHang, fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM070_DoanhThuTheoKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoBanHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM070_RevenueBasedOnCustomer_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoBanHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //BIỂU ĐỒ TỶ TRỌNG DOANH THU KHÁCH HÀNG

        [HttpGet]
        [Route("APIBieuDoTyTrongKhachHang")]
        public HttpResponseMessage APIBieuDoTyTrongKhachHang([FromUri]  DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoDanhThuDAL bc_dl = new BieuDoDanhThuDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BieuDoTyTrongKhachHang(userinfo.ID_QLLH, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM071_TyTrongDoanhThuTheoKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoTyTrongKhachHang.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM071_RevenueRateBasedOnCustomer_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BieuDoTyTrongKhachHang_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        #endregion

        #region BÁO CÁO CÔNG NỢ
        //BÁO CÁO TỔNG HỢP THEO KHÁCH HÀNG
        [HttpGet]
        [Route("ExcelBaoCaoCongNo")]
        public HttpResponseMessage ExcelBaoCaoCongNo([FromUri] int ID_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCao_CongNoKhachhang(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_NhanVien, ID_KhachHang, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM072_BaoCaoTheoDoiCongNo_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoCongNo.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM072_LiabilityTrackingReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoCongNo_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        //THU HỒI CÔNG NỢ
        [HttpGet]
        [Route("ExcelBaoCaoThuHoiCongNo")]
        public HttpResponseMessage ExcelBaoCaoThuHoiCongNo([FromUri] int ID_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoThuHoiCongNo(userinfo.ID_QLLH, 0, ID_KhachHang, id_NhanVien, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM073_BaoCaoThuHoiCongNo_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoThuHoiCongNo.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM073_LiabilityCollectedReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoThuHoiCongNo_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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
        #endregion

        #region BÁO CÁO KHO HÀNG
        // BÁO CÁO XUẤT NHẬP TỒN

        [HttpGet]
        [Route("ExcelBaoCaoTongHopNhapXuatTon")]
        public HttpResponseMessage ExcelBaoCaoTongHopNhapXuatTon([FromUri] int id_KhoHang, int id_MatHang, DateTime fromdate, DateTime todate, int id_Loai)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoTongHopNhapXuatTon(userinfo.ID_QLLH, fromdate, todate, id_KhoHang, id_MatHang, id_Loai);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM074_BaoCaoXuatNhapTon_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopNhapXuatTon.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM074_InventoryTrackingReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopNhapXuatTon_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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

        [HttpGet]
        [Route("ExcelBaoCaoTongHopNhapXuatTonChiTiet")]
        public HttpResponseMessage ExcelBaoCaoTongHopNhapXuatTonChiTiet([FromUri] int id_KhoHang, int id_MatHang, DateTime fromdate, DateTime todate, int id_LoaiBienDong, int id_Loai)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoChiTietNhapXuatTon(userinfo.ID_QLLH, fromdate, todate, id_KhoHang, id_MatHang, id_LoaiBienDong, id_Loai);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM076_BaoCaoXuatNhapTonChiTiet_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopNhapXuatTonChiTiet.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM076_InventoryDetailTrackingReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopNhapXuatTonChiTiet_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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


        //BÁO CÁO TỔNG HỢP TỒN CÁC MẶT HÀNG CÁC KHO
        [HttpGet]
        [Route("ExcelBaoCaoTongHopNhapXuatTonCacKho")]
        public HttpResponseMessage ExcelBaoCaoTongHopNhapXuatTonCacKho([FromUri] int id_KhoHang, int id_MatHang, DateTime fromdate, DateTime todate, int id_Loai)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoTongHopNhapXuatTonCacKho(userinfo.ID_QLLH, fromdate, todate, id_KhoHang, id_MatHang, id_Loai);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

                        BaoCaoCommon baocao = new BaoCaoCommon();
                        DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        dt2.TableName = "DATA2";
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string title = "";
                        if (lang == "vi")
                        {
                            title = "Từ " + fromdate.ToString("dd/MM/yyyy") + " đến " + todate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            title = "From " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                        }

                        DataTable dt1 = new DataTable();
                        dt1.TableName = "DATA1";
                        dt1.Columns.Add("TITLE", typeof(String));
                        DataRow row = dt1.NewRow();
                        row["TITLE"] = title;
                        dt1.Rows.Add(row);

                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM075_BaoCaoXuatNhapTonCacKho_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopNhapXuatTonCacKho.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM075_InventoryReportByWareHouse_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TongHopNhapXuatTonCacKho_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
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


        #endregion

        #region BÁO CÁO KHUYẾN MÃI
        [HttpGet]
        [Route("ExcelBaoCaoTongHopChuongTrinhKhuyenMai")]
        public HttpResponseMessage ExcelBaoCaoTongHopChuongTrinhKhuyenMai([FromUri] int idctkm, int idkho, int idnhom)
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataTable dt = baocao.baocaotonghopchuongtrinhkhuyenmai(userinfo.ID_QLLH, idctkm, idkho, idnhom, 0);
                    dt.TableName = "DATA";

                    DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    dt2.TableName = "DATA2";
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    DataTable dt1 = new DataTable();
                    dt1.TableName = "DATA1";
                    dt1.Columns.Add("TITLE", typeof(String));
                    DataRow row = dt1.NewRow();
                    row["TITLE"] = "";
                    dt1.Rows.Add(row);

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(dt.Copy());
                    dataSet.Tables.Add(dt1.Copy());
                    dataSet.Tables.Add(dt2.Copy());

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();
                    if (lang == "vi")
                    {
                        filename = "BM083_BaoCaoTongHopChuongTrinhKhuyenMai_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("Report_BaoCaoTongHopChuongTrinhKhuyenMai.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM083_DiscountProgramSummaryReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("Report_BaoCaoTongHopChuongTrinhKhuyenMai_en.xls", dataSet, null, ref stream);
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
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("ExcelBaoCaoChiTietChuongTrinhKhuyenMai")]
        public HttpResponseMessage ExcelBaoCaoChiTietChuongTrinhKhuyenMai([FromUri] int idctkm, int idkho, int idnhanvien, int idhang)
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataTable dt = baocao.baocaochitietchuongtrinhkhuyenmai(userinfo.ID_QLLH, idctkm, idkho, idnhanvien, idhang);
                    dt.TableName = "DATA";

                    DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    dt2.TableName = "DATA2";
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    DataTable dt1 = new DataTable();
                    dt1.TableName = "DATA1";
                    dt1.Columns.Add("TITLE", typeof(String));
                    DataRow row = dt1.NewRow();
                    row["TITLE"] = "";
                    dt1.Rows.Add(row);

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(dt.Copy());
                    dataSet.Tables.Add(dt1.Copy());
                    dataSet.Tables.Add(dt2.Copy());

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();
                    if (lang == "vi")
                    {
                        filename = "BM084_BaoCaoChiTietChuongTrinhKhuyenMai_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("Report_BaoCaoChiTietChuongTrinhKhuyenMai.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM084_DetailDiscountProgramReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("Report_BaoCaoChiTietChuongTrinhKhuyenMai_en.xls", dataSet, null, ref stream);
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
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        #endregion
    }
}
