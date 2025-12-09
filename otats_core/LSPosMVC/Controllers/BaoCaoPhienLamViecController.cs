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

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/phienlamviec")]
    public class BaoCaoPhienLamViecController : ApiController
    {

        [HttpGet]
        [Route("baocaomatketnoi")]
        public HttpResponseMessage baocaomatketnoi([FromUri] int idNhom, int idnhanvien, DateTime from, DateTime to)
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
                    BaoCaoCommon baoCao_DB = new BaoCaoCommon();
                    DataSet ds = baoCao_DB.BaoCaoMatKetNoi(userinfo.ID_QLLH, idnhanvien, from, to, userinfo.ID_QuanLy, idNhom);
                    DataTable dtDulieu = ds.Tables[0];
                    response = Request.CreateResponse(HttpStatusCode.OK, dtDulieu);
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
        [Route("Excelbaocaomatketnoi")]
        public HttpResponseMessage Excelbaocaomatketnoi([FromUri] int idNhom, int idnhanvien, DateTime from, DateTime to)
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
                    BaoCaoCommon baoCao_DB = new BaoCaoCommon();
                    DataSet ds = baoCao_DB.BaoCaoMatKetNoi(userinfo.ID_QLLH, idnhanvien, from, to, userinfo.ID_QuanLy, idNhom);
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
                        filename = "BM011_BaoCaoKhongKetNoi_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelKhongKetNoi.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM011_DisconnectedReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelKhongKetNoi_en.xls", dataSet, null, ref stream);
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



        /// <summary>
        /// get danh sách nhân viên theo nhóm
        /// </summary>
        /// <param name="idNhom"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getlistnhanvien")]
        public HttpResponseMessage getlistnhanvien([FromUri] int idNhom)
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
                    NhanVienApp nv_dl = new NhanVienApp();
                    if (idNhom <= 0)
                    {
                        List<NhanVien> dsnv = nv_dl.GetDSNhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                        response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
                    }
                    else
                    {
                        List<NhanVien> dsnv = nv_dl.GetDataNhanVien_TheoNhomQuanLy(userinfo.ID_QLLH, idNhom);

                        response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        /// <summary>
        /// Báo cáo phiên làm việc theo thông tin nhân viên
        /// </summary>
        /// <param name="idNhom"></param>
        /// <param name="idnhanvien"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("loaddata")]
        public HttpResponseMessage loaddata([FromUri] int idNhom, int idnhanvien, DateTime from, DateTime to)
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
                    BaoCaoCommon baoCao_DB = new BaoCaoCommon();
                    DataSet ds = baoCao_DB.PhienLamViec_Offline(userinfo.ID_QLLH, idnhanvien, from, to, userinfo.ID_QuanLy, idNhom);
                    DataTable dtDulieu = ds.Tables[0];

                    response = Request.CreateResponse(HttpStatusCode.OK, dtDulieu);
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
        [Route("ExportExcelPhienLamViec")]
        public HttpResponseMessage ExportExcelPhienLamViec([FromUri] int idNhom, int idnhanvien, DateTime from, DateTime to)
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
                    BaoCaoCommon baoCao_DB = new BaoCaoCommon();
                    DataSet ds = baoCao_DB.PhienLamViec_Offline(userinfo.ID_QLLH, idnhanvien, from, to, userinfo.ID_QuanLy, idNhom);
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
                        filename = "BM009_PhienLamViec_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelPhienLamViec.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM009_WorkSession_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelPhienLamViec_en.xls", dataSet, null, ref stream);
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

        /// <summary>
        /// Get thông tin KM di chuyển
        /// </summary>
        /// <param name="idnhanvien"></param>
        /// <param name="thoigiandangnhap"></param>
        /// <param name="thoigiandangxuatphien"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getkmdichuyen")]
        public HttpResponseMessage KMDiChuyen([FromUri] int idnhanvien, DateTime thoigiandangnhap, DateTime thoigiandangxuatphien)
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
                    DataSet ds = NhanVien_dl.PhienLamViec(userinfo.ID_QLLH, idnhanvien, thoigiandangnhap, thoigiandangxuatphien, userinfo.ID_QuanLy);
                    double KM = 0;
                    foreach (DataRow drPhien in ds.Tables[0].Rows)
                    {
                        DateTime dtThoiGianDangNhap = DateTime.Parse(drPhien["thoigiandangnhap"].ToString());
                        DateTime dtThoiGianDangXuat = drPhien["thoigiandangxuatphien"].ToString() != "" ? DateTime.Parse(drPhien["thoigiandangxuatphien"].ToString()) : (drPhien["thoigiandangnhaptieptheo"].ToString() != "" ? DateTime.Parse(drPhien["thoigiandangnhaptieptheo"].ToString()) : thoigiandangxuatphien);
                        double k = GetKMDiChuyen(idnhanvien, dtThoiGianDangNhap, dtThoiGianDangXuat, userinfo);
                        KM += k;
                        if (dtThoiGianDangXuat == thoigiandangxuatphien)
                            break;

                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, KM);
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
        /// Get số km di chuyển
        /// </summary>
        /// <param name="IDNV"></param>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public double GetKMDiChuyen(int IDNV, DateTime dtFrom, DateTime dtTo, UserInfo userinfo)
        {
            double kc = 0;
            int MinKhoangCach = 100;
            int thoigianlaybantin = 3000;
            double tocDoHopLeToiDa = 300;
            DataTable dt = LoTrinhDB.LichSuDiChuyenTheoNhanVien_Online_Offline(IDNV, dtFrom, dtTo, userinfo.ID_QLLH, userinfo.ID_QuanLy);
            List<Point> lpoint = new List<Point>();
            List<Point> lpoint_Offline = new List<Point>();

            int j = 0;


            List<Point> lpointDiChuyen = new List<Point>();

            foreach (DataRow i in dt.Rows)
            {
                Point.PointType t = Point.PointType.Move;

                if (i["ghichu"].ToString() == "Ngoại tuyến")
                {
                    t = Point.PointType.Offline;
                }

                Point p = null;
                try
                {
                    p = new Point { Lat = double.Parse(i["vido"].ToString()), Lng = double.Parse(i["kinhdo"].ToString()), thoigianbantin = DateTime.Parse(i["thoigian"].ToString()), Time = DateTime.Parse(i["thoigian"].ToString()), Type = t, OrigIndex = j, accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = i["tenkhachhang"].ToString(), diachikhachhang = i["diachikhachhang"].ToString(), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()) : new DateTime(1900, 01, 01) };

                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }
                lpoint.Add(p);
                j++;

                if (p.Type == Point.PointType.Move || p.Type == Point.PointType.Offline)
                {
                    lpointDiChuyen.Add(p);
                }

            }

            List<Point> filteredPoints = null;
            LoTrinhGPSFilter filter = new LoTrinhGPSFilter(lpointDiChuyen);
            filteredPoints = filter.FilterAnhTrungNangCap(lpointDiChuyen, MinKhoangCach, thoigianlaybantin, tocDoHopLeToiDa);
            kc = filter.tongKM;
            return kc;
        }
    }
}
