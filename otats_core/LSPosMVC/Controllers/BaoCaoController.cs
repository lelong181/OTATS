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
using static LSPos_Data.Data.BaoCaoCommon;
using BusinessLayer.Repository;
using BusinessLayer.Model.API;
using BusinessLayer.Model.Reports;
using LSPos_Data.Models;
using LSPosMVC.App_Start;
using Model.StoredProcedure;

/// <summary>
/// DongnnUpdate 2019-08-07
/// </summary>
/// 
namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/baocao")]
    public class BaoCaoController : ApiController
    {
        [HttpGet]
        [Route("getallnhanvien")]
        public HttpResponseMessage getallnhanvien()
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
                    List<NhanVien> dsnv = nv_dl.GetDSNhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy);



                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        [Route("getallkhachhang")]
        public HttpResponseMessage getallkhachhang()
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
                    KhachHangData kh_dl = new KhachHangData();
                    DataTable khachhang = kh_dl.GetDataKhachHangAll_Combo(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    List<khachhangparam> lstKhachHang = new List<khachhangparam>();

                    foreach (DataRow dr in khachhang.Rows)
                    {
                        khachhangparam khd = new khachhangparam();
                        khd.ID_KhachHang = int.Parse(dr["ID_KhachHang"].ToString());
                        khd.TenKhachHang = dr["TenKhachHang"].ToString();
                        khd.ID_LoaiKhachHang = int.Parse(dr["ID_LoaiKhachHang"].ToString());
                        lstKhachHang.Add(khd);

                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, lstKhachHang);
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
        [Route("getallkhachhang_serverpaging")]
        public HttpResponseMessage getallkhachhang_serverpaging([FromUri] int take, int skip)
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
                    KhachHangData kh_dl = new KhachHangData();
                    DataTable data = kh_dl.GetDataKhachHangAll_ServerPaging(userinfo.ID_QLLH, userinfo.ID_QuanLy, take, skip);

                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        //Báo cáo soát vé theo khách hàng
        [HttpGet]
        [Route("BaoCaoSoatVe")]
        public HttpResponseMessage Baocaosoatve([FromUri] DateTime fromdate, DateTime todate, string sitecode)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
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
                    DataTable ds = bc_dl.BaoCaoSoatVeKhachHang(fromdate, todate, sitecode);
                    response = Request.CreateResponse(HttpStatusCode.OK, ds);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        //Báo cáo cổng soát vé
        [HttpGet]
        [Route("BaoCaoCongSoatVe")]
        public HttpResponseMessage Baocaocongsoatve([FromUri] DateTime fromdate, DateTime todate, string sitecode)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
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
                    DataTable ds = bc_dl.BaoCaoCongSoatVe(fromdate, todate, sitecode);
                    response = Request.CreateResponse(HttpStatusCode.OK, ds);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        //Danh sách cổng ACM
        [HttpGet]
        [Route("DanhSachCongACM")]
        public HttpResponseMessage Danhsachcongacm([FromUri] DateTime fromdate, DateTime todate,string sitecode)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            BaoCaoCommonDAL bc_dl = new BaoCaoCommonDAL();
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
                    DataTable ds = bc_dl.DanhSachACMBD(fromdate, todate, sitecode);
                    response = Request.CreateResponse(HttpStatusCode.OK, ds);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        #region Báo cáo doanh thu

        // Get báo cáo doanh thu
        [HttpGet]
        [Route("BaocaoDoanhThuNew")]
        public HttpResponseMessage BaocaoDoanhThuNew([FromUri] int idnv, int idkh, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    List<BaocaoDTO> lstBaoCao = new List<BaocaoDTO>();
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    lstBaoCao = baocao.BaoCaoDoanhThu_New(userinfo.ID_QLLH, idnv, idkh, fromdate, todate, userinfo.ID_QuanLy);

                    response = Request.CreateResponse(HttpStatusCode.OK, lstBaoCao);
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
        [Route("BaocaoDoanhThuReport")]
        public HttpResponseMessage BaocaoDoanhThu([FromUri] int idnv, int idkh, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    BaoCao_dl baoCao_dl = new BaoCao_dl();
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataSet ds = baocao.BaoCaoDoanhThu_Report(userinfo.ID_QLLH, idnv, idkh, fromdate, todate, userinfo.ID_QuanLy);

                    ds.Tables[0].TableName = "DATA";

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
                        filename = "BM024_BaoCaoDoanhThu_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCaoDoanhThu.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM024_RevenueReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCaoDoanhThu_en.xls", dataSet, null, ref stream);
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

        // Báo cáo Doanh thu phân trang và fillter server (OLD)
        [HttpGet]
        //[AllowAnonymous]
        [Route("BaocaoDoanhThu")]
        public DataSourceResult GetOrders(HttpRequestMessage requestMessage)
        {
            BaoCaoCommon.RequestGridParam param = JsonConvert.DeserializeObject<BaoCaoCommon.RequestGridParam>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            System.Collections.Specialized.NameValueCollection cls = requestMessage.RequestUri.ParseQueryString();

            UserInfo userinfo = utilsCommon.checkAuthorization();

            BaoCaoCommon baocao = new BaoCaoCommon();

            List<BaocaoDTO> lstBaoCao = new List<BaocaoDTO>();
            FilterBaocao filter = new FilterBaocao();

            int tongso = 0;

            double soDonHang = 0;
            double daHoanTat = 0;
            double chuaHoanTat = 0;
            double slHuy = 0;
            double tongTienChuaChietKhau = 0;
            double tongTienChietKhau = 0;
            double tongTien = 0;
            double tienDaThanhToan = 0;

            if (param.request.Filter != null)
            {
                foreach (Filter f in param.request.Filter.Filters)
                {
                    switch (f.Field)
                    {
                        case "createDate":
                            filter.CreateDate = DateTime.Parse(f.Value.ToString());
                            break;
                        case "soDonHang":
                            filter.SoDonHang = int.Parse(f.Value.ToString());
                            break;
                        case "daHoanTat":
                            filter.DaHoanTat = int.Parse(f.Value.ToString());
                            break;
                        case "chuaHoanTat":
                            filter.ChuaHoanTat = int.Parse(f.Value.ToString());
                            break;
                        case "sLHuy":
                            filter.SLHuy = int.Parse(f.Value.ToString());
                            break;
                        case "tongTienChuaChietKhau":
                            filter.TongTienChuaChietKhau = double.Parse(f.Value.ToString());
                            break;
                        case "tongTienChietKhau":
                            filter.TongTienChietKhau = double.Parse(f.Value.ToString());
                            break;
                        case "tongTien":
                            filter.TongTien = double.Parse(f.Value.ToString());
                            break;
                        case "tienDaThanhToan":
                            filter.TienDaThanhToan = double.Parse(f.Value.ToString());
                            break;

                    }
                }
            }
            string formatString = "dd/MM/yyyy";
            if (param.tieuchiloc.fromdate != "")
            {
                filter.Fromdate = DateTime.ParseExact(param.tieuchiloc.fromdate, formatString, null);
            }
            if (param.tieuchiloc.todate != "")
            {
                filter.Todate = DateTime.ParseExact(param.tieuchiloc.todate, formatString, null);
            }
            //BaoCao_dl baoCao_dl = new BaoCao_dl();
            //DataSet ds = baoCao_dl.BaoCaoDoanhThu(userinfo.ID_QLLH, param.tieuchiloc.idnv, param.tieuchiloc.idkhc, param.tieuchiloc.fromdate, param.tieuchiloc.todate, userinfo.ID_QuanLy);
            lstBaoCao = baocao.BaocaoDoanhThu(userinfo.ID_QLLH, param.tieuchiloc.idnv, param.tieuchiloc.idkh, userinfo.ID_QuanLy, param.request.Skip, param.request.Take, filter
                , ref tongso
                , ref soDonHang, ref daHoanTat, ref chuaHoanTat, ref slHuy, ref tongTienChuaChietKhau, ref tongTienChietKhau, ref tongTien, ref tienDaThanhToan);

            lstBaoCao = lstBaoCao.OrderByDescending(x => x.CreateDate).ToList();

            Aggregates aggregates = new Aggregates();
            aggregates.soDonHang = new AggregatesValue(soDonHang);
            aggregates.daHoanTat = new AggregatesValue(daHoanTat);
            aggregates.chuaHoanTat = new AggregatesValue(chuaHoanTat);
            aggregates.slHuy = new AggregatesValue(slHuy);
            aggregates.tongTienChuaChietKhau = new AggregatesValue(tongTienChuaChietKhau);
            aggregates.tongTienChietKhau = new AggregatesValue(tongTienChietKhau);
            aggregates.tongTien = new AggregatesValue(tongTien);
            aggregates.tienDaThanhToan = new AggregatesValue(tienDaThanhToan);

            DataSourceResult s = new DataSourceResult();
            s.Data = lstBaoCao;
            s.Total = tongso;
            s.Aggregates = aggregates;
            //return lstKhachHang.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);
            return s;
            //return dsnv.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);

            //BaoCao_dl baoCao_dl = new BaoCao_dl();
            //DataSet ds = baoCao_dl.BaoCaoDoanhThu(userinfo.ID_QLLH, param.tieuchiloc.idnv, param.tieuchiloc.idkhc, param.tieuchiloc.fromdate, param.tieuchiloc.todate, userinfo.ID_QuanLy);


            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    BaocaoDTO baocao = new BaocaoDTO();
            //    baocao.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
            //    baocao.SoDonHang = int.Parse(row["SoDonHang"].ToString());
            //    baocao.DaHoanTat = int.Parse(row["DaHoanTat"].ToString());
            //    baocao.ChuaHoanTat = int.Parse(row["ChuaHoanTat"].ToString());
            //    baocao.SLHuy = int.Parse(row["SLHuy"].ToString());
            //    baocao.TongTienChuaChietKhau = row["TongTienChuaChietKhau"].ToString() != "" ? double.Parse(row["TongTienChuaChietKhau"].ToString()) : 0;
            //    baocao.TongTienChietKhau = row["TongTienChietKhau"].ToString() != "" ? double.Parse(row["TongTienChietKhau"].ToString()) : 0;
            //    baocao.TongTien = row["TongTien"].ToString() != "" ? double.Parse(row["TongTien"].ToString()) : 0;
            //    baocao.TienDaThanhToan = row["TienDaThanhToan"].ToString() != "" ? double.Parse(row["TienDaThanhToan"].ToString()) : 0;
            //    lstBaoCao.Add(baocao);
            //}
            ////NhanVien_dl nv_dl = new NhanVien_dl();
            ////List<NhanVien> dsnv = nv_dl.GetDSNhanVien_TheoNhomQuanLy(userinfo.ID_QLLH, param.tieuchiloc.IdNhom);

            //return lstBaoCao.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);

        }

        #endregion

        #region Báo cáo đơn hàng tổng quan  + Tổng hợp đơn hàng
        [HttpGet]
        [Route("BaoCaoTongHopDonHang")]
        public HttpResponseMessage BaoCaoTongHopDonHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    //string formatString = "dd/MM/yyyy";
                    //DateTime from_date = DateTime.ParseExact(fromdate, formatString, null);
                    //DateTime to_date = DateTime.ParseExact(todate, formatString, null);
                    List<BaocaoDonHangDTO> listBaocao = new List<BaocaoDonHangDTO>();
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    listBaocao = baocao.BaoCaoTongHopDonHang(userinfo.ID_QLLH, fromdate, todate, userinfo.ID_QuanLy);
                    response = Request.CreateResponse(HttpStatusCode.OK, listBaocao);
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
        [Route("BaoCaoDonHangTongQuan")]
        public DataSourceResult BaoCaoDonHangTongQuan(HttpRequestMessage requestMessage)
        {
            DataSourceResult s = new DataSourceResult();
            try
            {
                BaoCaoCommon.RequestGridParam param = JsonConvert.DeserializeObject<BaoCaoCommon.RequestGridParam>(
                    // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                    requestMessage.RequestUri.ParseQueryString().GetKey(0)
                );
                System.Collections.Specialized.NameValueCollection cls = requestMessage.RequestUri.ParseQueryString();

                UserInfo userinfo = utilsCommon.checkAuthorization();

                List<BaocaoDonHangDTO> lstBaoCao = new List<BaocaoDonHangDTO>();

                BaoCaoCommon baocao = new BaoCaoCommon();

                FilterTongQuanBaocao filter = new FilterTongQuanBaocao();
                int tongso = 0;

                double tongSoDon = 0;
                double daHoanTat = 0;
                double chuaHoanTat = 0;
                double huy = 0;
                double chuaThanhToan = 0;
                double thanhToan1Phan = 0;
                double daThanhToan = 0;
                double chuaGiaoHang = 0;
                double giaoHang1Phan = 0;
                double daGiaoHang = 0;
                double tongTien = 0;
                double tienDaThanhToan = 0;
                double conLai = 0;

                if (param.request.Filter != null)
                {
                    foreach (Filter f in param.request.Filter.Filters)
                    {
                        switch (f.Field)
                        {
                            case "createDate":
                                filter.CreateDate = DateTime.Parse(f.Value.ToString());
                                break;
                            case "tongSoDon":
                                filter.TongSoDon = int.Parse(f.Value.ToString());
                                break;
                            case "daHoanTat":
                                filter.DaHoanTat = int.Parse(f.Value.ToString());
                                break;
                            case "chuaHoanTat":
                                filter.ChuaHoanTat = int.Parse(f.Value.ToString());
                                break;
                            case "huy":
                                filter.Huy = int.Parse(f.Value.ToString());
                                break;
                            case "chuaThanhToan":
                                filter.ChuaThanhToan = int.Parse(f.Value.ToString());
                                break;
                            case "thanhToan1Phan":
                                filter.ThanhToan1Phan = int.Parse(f.Value.ToString());
                                break;
                            case "daThanhToan":
                                filter.DaThanhToan = int.Parse(f.Value.ToString());
                                break;
                            case "chuaGiaoHang":
                                filter.ChuaGiaoHang = int.Parse(f.Value.ToString());
                                break;
                            case "giaoHang1Phan":
                                filter.GiaoHang1Phan = int.Parse(f.Value.ToString());
                                break;
                            case "daGiaoHang":
                                filter.DaGiaoHang = int.Parse(f.Value.ToString());
                                break;
                            case "tongTien":
                                filter.TongTien = double.Parse(f.Value.ToString());
                                break;
                            case "tienDaThanhToan":
                                filter.TienDaThanhToan = double.Parse(f.Value.ToString());
                                break;
                            case "conLai":
                                filter.ConLai = double.Parse(f.Value.ToString());
                                break;

                        }
                    }
                }
                string formatString = "dd/MM/yyyy";
                DateTime from_date = DateTime.ParseExact(param.tieuchiloc.fromdate, formatString, null);
                DateTime to_date = DateTime.ParseExact(param.tieuchiloc.todate, formatString, null);

                lstBaoCao = baocao.BaoCaoDonHangTongQuan(userinfo.ID_QLLH, userinfo.ID_QuanLy, from_date, to_date, param.request.Skip, param.request.Take, filter, ref tongso
                    , ref tongSoDon, ref daHoanTat, ref chuaHoanTat, ref huy, ref chuaThanhToan, ref thanhToan1Phan
                , ref daThanhToan, ref chuaGiaoHang, ref giaoHang1Phan, ref daGiaoHang, ref tongTien, ref tienDaThanhToan, ref conLai);
                lstBaoCao = lstBaoCao.OrderByDescending(x => x.CreateDate).ToList();

                AggregatesBaoCaoDonHangTongQuan aggregates = new AggregatesBaoCaoDonHangTongQuan();
                aggregates.tongSoDon = new AggregatesValue(tongSoDon);
                aggregates.daHoanTat = new AggregatesValue(daHoanTat);
                aggregates.chuaHoanTat = new AggregatesValue(chuaHoanTat);
                aggregates.huy = new AggregatesValue(huy);
                aggregates.chuaThanhToan = new AggregatesValue(chuaThanhToan);
                aggregates.thanhToan1Phan = new AggregatesValue(thanhToan1Phan);
                aggregates.daThanhToan = new AggregatesValue(daThanhToan);
                aggregates.chuaGiaoHang = new AggregatesValue(chuaGiaoHang);
                aggregates.giaoHang1Phan = new AggregatesValue(giaoHang1Phan);
                aggregates.daGiaoHang = new AggregatesValue(daGiaoHang);
                aggregates.tongTien = new AggregatesValue(tongTien);
                aggregates.tienDaThanhToan = new AggregatesValue(tienDaThanhToan);
                aggregates.conLai = new AggregatesValue(conLai);

                s.Data = lstBaoCao;
                s.Total = tongso;
                s.Aggregates = aggregates;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return s;

        }

        [HttpGet]
        [Route("BaoCaoTongHopDonHangReport")]
        public HttpResponseMessage BaoCaoDonHangTongQuanReport([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    //string formatString = "dd/MM/yyyy";
                    //DateTime from_date = DateTime.ParseExact(tieuchi.fromdate, formatString, null);
                    //DateTime to_date = DateTime.ParseExact(tieuchi.todate, formatString, null);
                    //BaoCao_dl baoCao_dl = new BaoCao_dl();
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataSet ds = baocao.BaoCaoDonHangTongQuan_Report(userinfo.ID_QLLH, fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count == 0)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.Created, new { success = false, msg = "Không tồn tại dữ liệu" });
                    }
                    DataTable dt = new DataTable();
                    ExportExcel ExportExcel = new ExportExcel();
                    ds.Tables[0].TableName = "DATA";

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
                        filename = "BM022_TongHopDonHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCaoTongHopDonHang.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM022_TotalOrder_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCaoTongHopDonHang_en.xls", dataSet, null, ref stream);
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

        #region Báo cáo đơn hàng theo nhân viên
        [HttpGet]
        [Route("BaoCaoDonHangTheoNhanVien")]
        public DataSourceResult BaoCaoDonHangTheoNhanVien(HttpRequestMessage requestMessage)
        {
            DataSourceResult s = new DataSourceResult();
            try

            {
                BaoCaoCommon.RequestGridParam param = JsonConvert.DeserializeObject<BaoCaoCommon.RequestGridParam>(
                    // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                    requestMessage.RequestUri.ParseQueryString().GetKey(0)
                );
                System.Collections.Specialized.NameValueCollection cls = requestMessage.RequestUri.ParseQueryString();

                UserInfo userinfo = utilsCommon.checkAuthorization();

                List<BaoCaoDonHangNhanVienDTO> lstBaoCao = new List<BaoCaoDonHangNhanVienDTO>();

                BaoCaoCommon baocao = new BaoCaoCommon();

                FilterDonHangNV filter = new FilterDonHangNV();
                int tongso = 0;

                double tongDonHang = 0;
                double donThanhCong = 0;
                double tongTienChuaChietKhau = 0;
                double tongTienChietKhau = 0;
                double tongTien = 0;
                double daThanhToan = 0;

                if (param.request.Filter != null)
                {
                    foreach (Filter f in param.request.Filter.Filters)
                    {
                        switch (f.Field)
                        {
                            case "tenNhanVien":
                                filter.TenNhanVien = f.Value.ToString();
                                break;
                            case "tongDonHang":
                                filter.TongDonHang = int.Parse(f.Value.ToString());
                                break;
                            case "donThanhCong":
                                filter.DonThanhCong = int.Parse(f.Value.ToString());
                                break;
                            case "tongTienChuaChietKhau":
                                filter.TongTienChuaChietKhau = int.Parse(f.Value.ToString());
                                break;
                            case "tongTienChietKhau":
                                filter.TongTienChietKhau = int.Parse(f.Value.ToString());
                                break;
                            case "tongTien":
                                filter.TongTien = int.Parse(f.Value.ToString());
                                break;
                            case "daThanhToan":
                                filter.DaThanhToan = int.Parse(f.Value.ToString());
                                break;
                        }
                    }
                }
                string formatString = "dd/MM/yyyy";
                DateTime from_date = DateTime.ParseExact(param.tieuchiloc.fromdate, formatString, null);
                DateTime to_date = DateTime.ParseExact(param.tieuchiloc.todate, formatString, null);
                //BaoCao_dl baoCao_dl = new BaoCao_dl();
                //DataSet ds = baoCao_dl.BaoCaoDoanhThu(userinfo.ID_QLLH, param.tieuchiloc.idnv, param.tieuchiloc.idkhc, param.tieuchiloc.fromdate, param.tieuchiloc.todate, userinfo.ID_QuanLy);
                lstBaoCao = baocao.BaoCaoDonHangTheoNhanVien(userinfo.ID_QLLH, param.tieuchiloc.idnv, from_date, to_date, userinfo.ID_QuanLy, param.request.Skip, param.request.Take, filter, ref tongso
                    , ref tongDonHang, ref donThanhCong, ref tongTienChuaChietKhau, ref tongTienChietKhau, ref tongTien, ref daThanhToan);
                // lstBaoCao = lstBaoCao.OrderByDescending(x => x.CreateDate).ToList();

                AggregatesBaoCaoDonHangTheoNhanVien aggregates = new AggregatesBaoCaoDonHangTheoNhanVien();
                aggregates.tongDonHang = new AggregatesValue(tongDonHang);
                aggregates.donThanhCong = new AggregatesValue(donThanhCong);
                aggregates.tongTienChuaChietKhau = new AggregatesValue(tongTienChuaChietKhau);
                aggregates.tongTienChietKhau = new AggregatesValue(tongTienChietKhau);
                aggregates.tongTien = new AggregatesValue(tongTien);
                aggregates.daThanhToan = new AggregatesValue(daThanhToan);


                s.Data = lstBaoCao;
                s.Total = tongso;
                s.Aggregates = aggregates;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            //return lstKhachHang.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);
            return s;

        }

        [HttpGet]
        [Route("BaoCaoDonHangTheoNhanVienNew")]
        public HttpResponseMessage BaoCaoDonHangTheoNhanVienNew([FromUri] int idnv, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    List<BaoCaoDonHangNhanVienDTO> listBaocao = new List<BaoCaoDonHangNhanVienDTO>();
                    //string formatString = "dd/MM/yyyy";
                    //DateTime from_date = DateTime.ParseExact(fromdate, formatString, null);
                    //DateTime to_date = DateTime.ParseExact(todate, formatString, null);
                    BaoCao_dl baoCao_dl = new BaoCao_dl();
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    listBaocao = baocao.BaoCaoDonHangTheoNhanVienNew(userinfo.ID_QLLH, idnv, fromdate, todate, userinfo.ID_QuanLy);
                    response = Request.CreateResponse(HttpStatusCode.OK, listBaocao);
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
        [Route("BaoCaoDonHangTheoNhanVienReport")]
        public HttpResponseMessage BaoCaoDonHangTheoNhanVienReport([FromUri] int idnv, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    //string formatString = "dd/MM/yyyy";
                    //DateTime from_date = DateTime.ParseExact(fromdate, formatString, null);
                    //DateTime to_date = DateTime.ParseExact(todate, formatString, null);
                    BaoCao_dl baoCao_dl = new BaoCao_dl();
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataSet ds = baocao.BaoCaoDonHang(userinfo.ID_QLLH, idnv, fromdate, todate, userinfo.ID_QuanLy);

                    ExportExcel ExportExcel = new ExportExcel();
                    ds.Tables[0].TableName = "DATA";

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
                        filename = "BM023_BaoCaoDonHangTheoNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCaoDonHangNhanVien.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM023_OrderBasedOnEmployeeReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCaoDonHangNhanVien_en.xls", dataSet, null, ref stream);
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

        #region Báo cáo tin nhắn
        [HttpGet]
        [Route("baocaochitiettinnhan")]
        public HttpResponseMessage baocaochitiettinnhan([FromUri] int idtinnhan)
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
                    TinNhanOBJ ds = TinNhanDB.getTinNhanTheoID_TINNHAN(idtinnhan);
                    List<TinNhanOBJ> lst = new List<TinNhanOBJ>();
                    lst.Add(ds);
                    if (ds.TypeSend == 1 && ds.TrangThai == 0) //nhan vien gui cho quan ly ma chua doc thi cap nhat trang thai thanh da doc
                    {
                        ds.ID_QUANLY = userinfo.ID_QuanLy;
                        TinNhanDB.CapNhatTinNhanQuanLyDaXem(ds);
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

        [HttpGet]
        [Route("baocaotinnhan")]
        public HttpResponseMessage baocaotinnhan([FromUri] int idnhanvien, int chuadoc, DateTime from, DateTime to)
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
                    DataSet ds = bc.GetListTinNhan(userinfo.ID_QLLH, userinfo.ID_QuanLy, idnhanvien, -1, from, to, chuadoc);

                    response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("ExcelBaoCaoTinNhan")]
        public HttpResponseMessage ExcelBaoCaoTinNhan([FromUri] int idnhanvien, int chuadoc, DateTime from, DateTime to)
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
                    BaoCaoCommon bc = new BaoCaoCommon();
                    DataSet ds = bc.GetListTinNhan(userinfo.ID_QLLH, userinfo.ID_QuanLy, idnhanvien, -1, from, to, chuadoc);

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
                            filename = "BM013_BaoCaoTinNhan_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TinNhan.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM013_MessageReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TinNhan_en.xls", dataSet, null, ref stream);
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
        [Route("baocaotinnhannhanvien")]
        public HttpResponseMessage baocaotinnhannhanvien([FromUri] int idnhanvien, DateTime from, DateTime to)
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
                    DataSet ds = bc.GetListTinNhan(userinfo.ID_QLLH, userinfo.ID_QuanLy, idnhanvien, 1, from, to);

                    response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("ExcelBaoCaoTinNhanNhanVien")]
        public HttpResponseMessage ExcelBaoCaoTinNhanNhanVien([FromUri] int idnhanvien, DateTime from, DateTime to)
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
                    BaoCaoCommon bc = new BaoCaoCommon();
                    DataSet ds = bc.GetListTinNhan(userinfo.ID_QLLH, userinfo.ID_QuanLy, idnhanvien, 1, from, to);

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
                            filename = "BM012_TinNhanGuiTuNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TinNhanNhanVien.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM012_EmployeeMessage_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_TinNhanNhanVien_en.xls", dataSet, null, ref stream);
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

        #region Báo cáo lịch sử vào ra điểm
        public class ResultLichSuVaoRaDiem
        {
            public DataTable data { get; set; }
            public int VaoDiem { get; set; }
            public int RaDiem { get; set; }
        }

        [HttpGet]
        [Route("lichsuvaoradiem")]

        public HttpResponseMessage lichsuvaoradiem([FromUri] int idnhom, int idnhanvien, int loaikhachhang, DateTime from, DateTime to, int id, int type, int idkhachhang, int vaodiemtheokhachhang)
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
                    ResultLichSuVaoRaDiem result = new ResultLichSuVaoRaDiem();
                    CheckInOut_dl cio_dl = new CheckInOut_dl();

                    DataTable dsci = null;

                    if (id > 0)
                        dsci = cio_dl.GetCheckInById_New(id);
                    else
                        dsci = cio_dl.GetCheckInTheoIDNV_New(userinfo.ID_QLLH, idnhom, idnhanvien, from, to, userinfo.ID_QuanLy, idkhachhang, type, loaikhachhang);

                    DataRow[] drRa = dsci.Select("CheckOutDay IS NOT NULL");
                    DataRow[] drVao = dsci.Select("CheckInDay IS NOT NULL");
                    result.data = dsci;
                    result.VaoDiem = drVao != null ? drVao.Length : 0;
                    result.RaDiem = drRa != null ? drRa.Length : 0;

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
        [Route("ExportExcelLichSuRavaoDiem")]

        public HttpResponseMessage ExportExcelLichSuRavaoDiem([FromUri] int idnhom, int idnhanvien, int loaikhachhang, DateTime from, DateTime to, int id, int type, int idkhachhang, int vaodiemtheokhachhang)
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
                    ResultLichSuVaoRaDiem result = new ResultLichSuVaoRaDiem();
                    BaoCaoCommon cio_dl = new BaoCaoCommon();
                    DataSet ds = null;
                    if (id > 0)
                        ds = cio_dl.GetCheckInById_New(id, userinfo.ID_QuanLy);
                    else
                        ds = cio_dl.GetCheckInTheoIDNV_New(userinfo.ID_QLLH, idnhom, idnhanvien, from, to, userinfo.ID_QuanLy, idkhachhang, type, loaikhachhang);

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
                        filename = "BM008_LichSuVaoRaDiem_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelLichSuRaVaoDiem.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM008_CheckinCheckoutHistory_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelLichSuRaVaoDiem_en.xls", dataSet, null, ref stream);
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
        [Route("getchecklistbykhachhang")]
        public HttpResponseMessage getchecklistbykhachhang([FromUri] int idkhachhang, int idcheckin)
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
                    BaoCaoCommon baoCaoCommon = new BaoCaoCommon();
                    DataTable data = baoCaoCommon.getchecklistbykhachhang(userinfo.ID_QLLH, idkhachhang, idcheckin);

                    response = Request.CreateResponse(HttpStatusCode.OK, data);
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

        #region Báo cáo fake vào điểm khách hàng
        [HttpGet]
        [Route("baocaofake")]
        public HttpResponseMessage BaoCaoFakeVaoDiemKhachHang([FromUri] int id_KhachHang, int id_NhanVien, DateTime from, DateTime to)
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
                    DataTable dt = bc.BaoCaoFakeVaoDiemKhachHang(userinfo.ID_QLLH, id_NhanVien, id_KhachHang, from, to).Tables[0];

                    response = Request.CreateResponse(HttpStatusCode.OK, dt);
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

        #region Báo cáo chương trình khuyến mãi
        [HttpGet]
        [Route("listkhuyenmai")]
        public HttpResponseMessage listkhuyenmai()
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
                    ChuongTrinhKMDAL km_dl = new ChuongTrinhKMDAL();

                    DataTable dskm = km_dl.GetDSKhuyenmaiCombo(userinfo.ID_QLLH);

                    response = Request.CreateResponse(HttpStatusCode.OK, dskm);
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
        [Route("tonghopchuongtrinhkhuyenmai")]
        public HttpResponseMessage tonghopchuongtrinhkhuyenmai([FromUri] int idctkm, int idkho, int idnhom)
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

                    response = Request.CreateResponse(HttpStatusCode.OK, dt);
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
        [Route("chitietchuongtrinhkhuyenmai")]
        public HttpResponseMessage chitietchuongtrinhkhuyenmai([FromUri] int idctkm, int idkho, int idnhanvien, int idhang)
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

                    response = Request.CreateResponse(HttpStatusCode.OK, dt);
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
        [Route("ticket_tonghopdoanhthu")]
        public HttpResponseMessage ticket_tonghopdoanhthu([FromUri] string SiteCode, DateTime ReportDate)
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
                    ReportsRepository reportsRepository = new ReportsRepository();
                    RevenueDetailRequest model = new RevenueDetailRequest();
                    SiteModel site = new SiteDAO().GetSite(SiteCode);
                    new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, "", "");
                    model.StartDate = ReportDate.AddHours(10);
                    model.EndDate = ReportDate.AddHours(10);
                    model.ServiceRateId = "";
                    model.ServiceGroupId = "";
                    model.ServiceSubGroupId = "";
                    model.Sale = "";
                    model.TypeView = "0";
                    model.Channels = "";
                    model.Profile = "";
                    model.RevenueView = "0";
                    List<RptRevenueSummary> data = reportsRepository.RevenueSummary(model);
                    List<RptRevenueSummary> data2 = new BaoCaoCommon().TongHopDoanhThuCacDichVu(userinfo.ID_QLLH, userinfo.ID_QuanLy, ReportDate, SiteCode);
                    List<RptRevenueSummary> result = new List<RptRevenueSummary>();
                    result.AddRange(data);
                    result.AddRange(data2);
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
        [Route("ticket_tonghopdoanhthutheoca")]
        public HttpResponseMessage ticket_tonghopdoanhthutheoca([FromUri] string SiteCode, DateTime ReportDate)
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
                    ReportsRepository reportsRepository = new ReportsRepository();
                    RevenueTotalByCashierReq model = new RevenueTotalByCashierReq();
                    SiteModel site = new SiteDAO().GetSite(SiteCode);
                    model.FromDate = ReportDate.AddHours(10);
                    model.ToDate = ReportDate.AddHours(10);
                    model.ServiceRateIDStr = "";
                    model.CashierStr = "";
                    List<RevenueTotalByCashierRes> data2 = new List<RevenueTotalByCashierRes>();
                    List<RevenueTotalByCashierRes> result = new List<RevenueTotalByCashierRes>();

                    if (SiteCode == "TRANGAN")
                    {
                        new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, "", "");
                        List<RevenueTotalByCashierRes> data = reportsRepository.RevenueTotalByCashier(model);
                        result.AddRange(data.Where(x => !x.ID.Contains("b2b_online")).ToList());

                        data2 = new BaoCaoCommon().TongHopDoanhThuTheoCaForTA(userinfo.ID_QLLH, userinfo.ID_QuanLy, ReportDate, SiteCode);
                    }
                    else
                    {
                        data2 = new BaoCaoCommon().TongHopDoanhThuTheoCa(userinfo.ID_QLLH, userinfo.ID_QuanLy, ReportDate, SiteCode);

                    }
                    result.AddRange(data2);
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
        [Route("ticket_tonghopthanhtoanthungan")]
        public HttpResponseMessage ticket_tonghopthanhtoanthungan([FromUri] string SiteCode, DateTime ReportDate)
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
                    ReportsRepository reportsRepository = new ReportsRepository();
                    RevenueSummaryByShiftRequest model = new RevenueSummaryByShiftRequest();
                    SiteModel site = new SiteDAO().GetSite(SiteCode);
                    model.StartDate = ReportDate.AddHours(10);
                    model.EndDate = ReportDate.AddHours(10);
                    model.Cashier = "";
                    model.Channels = "";
                    List<RptRevenueSummaryByShift> data2 = new BaoCaoCommon().TongHopThanhToanThuNgan(userinfo.ID_QLLH, userinfo.ID_QuanLy, ReportDate, SiteCode);

                    List<RptRevenueSummaryByShift> result = new List<RptRevenueSummaryByShift>();
                    if (SiteCode == "TRANGAN")
                    {
                        new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, "", "");
                        List<RptRevenueSummaryByShift> data = reportsRepository.RevenueSummaryByShift(model);
                        result.AddRange(data.Where(x => !x.ID.Contains("b2b_online")).ToList());

                    }
                    result.AddRange(data2);
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
        [Route("ticket_tonghophttt")]
        public HttpResponseMessage ticket_tonghophttt([FromUri] string SiteCode, DateTime ReportDate)
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
                    ReportsRepository reportsRepository = new ReportsRepository();
                    RevenueSummaryByShiftRequest model = new RevenueSummaryByShiftRequest();
                    SiteModel site = new SiteDAO().GetSite(SiteCode);
                    model.StartDate = ReportDate.AddHours(10);
                    model.EndDate = ReportDate.AddHours(10);
                    model.Cashier = "";
                    model.Channels = "";
                    
                    List<RptRevenueSummaryByPaymentType> data2 = new BaoCaoCommon().TongHopHTTT(userinfo.ID_QLLH, userinfo.ID_QuanLy, ReportDate, SiteCode);
                    List<RptRevenueSummaryByPaymentType> result = new List<RptRevenueSummaryByPaymentType>();
                    if (SiteCode == "TRANGAN")
                    {
                        new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, "", "");
                        List<RptRevenueSummaryByPaymentType> data = reportsRepository.RevenueSummaryByPaymenttype(model);
                        result.AddRange(data.Where(x => !x.UserName.Contains("b2b_online")).ToList());
                    }
                    result.AddRange(data2);
                    foreach (var item in result)
                    {
                        item.Total = item.S_9950 + item.S_9951 + item.S_9953 + item.S_9954 + item.S_9965 + item.S_9975 + item.S_9980 + item.lspay;


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
        [Route("gettonghopdichvutheobooking")]
        public HttpResponseMessage gettonghopdichvutheobooking([FromUri] string SiteCode, DateTime From, DateTime To)
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
                    DataTable data2 = new BaoCaoCommon().TongHopDVTheoBooking(userinfo.ID_QLLH, userinfo.ID_QuanLy, From, To, SiteCode);
                    response = Request.CreateResponse(HttpStatusCode.OK, data2);
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
        [Route("gettonghopdichvutheobooking2")]
        public HttpResponseMessage gettonghopdichvutheobooking2([FromUri] string SiteCode, DateTime From, DateTime To)
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
                    DataTable data2 = new BaoCaoCommon().TongHopDVTheoBooking2(userinfo.ID_QLLH, userinfo.ID_QuanLy, From, To, SiteCode);
                    response = Request.CreateResponse(HttpStatusCode.OK, data2);
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
        [Route("checkusingticket")]
        public HttpResponseMessage checkusingticket([FromUri] string SiteCode, string BookingCode, string ServiceID)
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
                    DataTable data2 = new BaoCaoCommon().checkusingticket(SiteCode, BookingCode, ServiceID);
                    response = Request.CreateResponse(HttpStatusCode.OK, data2);
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
        [Route("getdoisoatdichvu")]
        public HttpResponseMessage getdoisoatdichvu([FromUri] string SiteCode, DateTime From, DateTime To)
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
                    DataTable data2 = new BaoCaoCommon().DoiSoatDVTheoBooking(userinfo.ID_QLLH, userinfo.ID_QuanLy, From, To, SiteCode);
                    response = Request.CreateResponse(HttpStatusCode.OK, data2);
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
        [Route("getBaoCaoSuDungVi")]
        public HttpResponseMessage GetBaoCaoSuDungVi([FromUri] int id_nhom, DateTime from, DateTime to)
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
                    DataTable data2 = new BaoCaoCommon().LichSuDungViLspay(id_nhom, from, to);
                    response = Request.CreateResponse(HttpStatusCode.OK, data2);
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
        [Route("getBaoCaoDoiSoatOnepay")]
        public HttpResponseMessage GetBaoCaoDoiSoatOnepay([FromUri] DateTime from, DateTime to)
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
                    DataTable data2 = new BaoCaoCommon().DoiSoatOnepay(from, to);
                    response = Request.CreateResponse(HttpStatusCode.OK, data2);
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
        [AllowAnonymous]
        [Route("TongHopSoLuongVeBanCacKhu")]
        public HttpResponseMessage TongHopSoLuongVeBanCacKhu([FromUri] int ID_DanhMuc, DateTime from, DateTime to, string SiteCode)
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
                    DataTable data = new DataTable();
                    data.Columns.Add("MaHang", typeof(string));
                    data.Columns.Add("TenHang", typeof(string));
                    data.Columns.Add("GiaBanLe", typeof(float));

                    data.Columns.Add("TRANGAN_GiaBan", typeof(float));
                    data.Columns.Add("TAMCHUC_GiaBan", typeof(float));
                    data.Columns.Add("BAIDINH_GiaBan", typeof(float));
                    data.Columns.Add("TAMCOC_GiaBan", typeof(float));
                    data.Columns.Add("PCHL_GiaBan", typeof(float));
                    data.Columns.Add("TAG_GiaBan", typeof(float));

                    data.Columns.Add("TRANGAN_SoLuong", typeof(int));
                    data.Columns.Add("TAMCHUC_SoLuong", typeof(int));
                    data.Columns.Add("BAIDINH_SoLuong", typeof(int));
                    data.Columns.Add("TAMCOC_SoLuong", typeof(int));
                    data.Columns.Add("PCHL_SoLuong", typeof(int));
                    data.Columns.Add("TAG_SoLuong", typeof(int));
                    data.Columns.Add("Tong_SoLuong", typeof(int));

                    data.Columns.Add("TRANGAN_TraLai", typeof(float));
                    data.Columns.Add("TAMCHUC_TraLai", typeof(float));
                    data.Columns.Add("BAIDINH_TraLai", typeof(float));
                    data.Columns.Add("TAMCOC_TraLai", typeof(float));
                    data.Columns.Add("PCHL_TraLai", typeof(float));
                    data.Columns.Add("TAG_TraLai", typeof(float));
                    data.Columns.Add("TongTien_TraLai", typeof(decimal));

                    data.Columns.Add("TRANGAN_PhaiThu", typeof(float));
                    data.Columns.Add("TAMCHUC_PhaiThu", typeof(float));
                    data.Columns.Add("BAIDINH_PhaiThu", typeof(float));
                    data.Columns.Add("TAMCOC_PhaiThu", typeof(float));
                    data.Columns.Add("PCHL_PhaiThu", typeof(float));
                    data.Columns.Add("TAG_PhaiThu", typeof(float));

                    data.Columns.Add("TongTien_PhaiThu", typeof(decimal));
                    data.Columns.Add("Tong_DoanhThuTaiKhu", typeof(decimal));
                    data.Columns.Add("Tong_DoanhThuCacKhu", typeof(decimal));

                    DataTable data1 = new BaoCaoCommon().SoLuongVeCacKhuBan(ID_DanhMuc, from, to);
                    DataTable data2 = new BaoCaoCommon().TyLePhanBoGiaVe(ID_DanhMuc);
                    for (int i = 0; i < data1.Rows.Count; i++)
                    {
                        float GiaBanLe = float.Parse(data2.Rows[i]["GiaBanLe"].ToString());
                        DataRow dataRow = data.NewRow();
                        dataRow["MaHang"] = data1.Rows[i]["MaHang"].ToString();
                        dataRow["TenHang"] = data1.Rows[i]["TenHang"].ToString();
                        dataRow["GiaBanLe"] = GiaBanLe;

                        float TRANGAN_GiaBan = data2.Rows[i]["TRANGAN"].ToString() != "" ? float.Parse(data2.Rows[i]["TRANGAN"].ToString()) : 0;
                        float TAMCHUC_GiaBan = data2.Rows[i]["TAMCHUC"].ToString() != "" ? float.Parse(data2.Rows[i]["TAMCHUC"].ToString()) : 0;
                        float BAIDINH_GiaBan = data2.Rows[i]["BAIDINH"].ToString() != "" ? float.Parse(data2.Rows[i]["BAIDINH"].ToString()) : 0;
                        float TAMCOC_GiaBan = data2.Rows[i]["TAMCOC"].ToString() != "" ? float.Parse(data2.Rows[i]["TAMCOC"].ToString()) : 0;
                        float PCHL_GiaBan = data2.Rows[i]["PCHL"].ToString() != "" ? float.Parse(data2.Rows[i]["PCHL"].ToString()) : 0;
                        float TAG_GiaBan = data2.Rows[i]["TAG"].ToString() != "" ? float.Parse(data2.Rows[i]["TAG"].ToString()) : 0;

                        dataRow["TRANGAN_GiaBan"] = TRANGAN_GiaBan;
                        dataRow["TAMCHUC_GiaBan"] = TAMCHUC_GiaBan;
                        dataRow["BAIDINH_GiaBan"] = BAIDINH_GiaBan;
                        dataRow["TAMCOC_GiaBan"] = TAMCOC_GiaBan;
                        dataRow["PCHL_GiaBan"] = PCHL_GiaBan;
                        dataRow["TAG_GiaBan"] = TAG_GiaBan;

                        int TRANGAN_SoLuong = data1.Rows[i]["TRANGAN"].ToString() != "" ? int.Parse(data1.Rows[i]["TRANGAN"].ToString()) : 0;
                        int TAMCHUC_SoLuong = data1.Rows[i]["TAMCHUC"].ToString() != "" ? int.Parse(data1.Rows[i]["TAMCHUC"].ToString()) : 0;
                        int BAIDINH_SoLuong = data1.Rows[i]["BAIDINH"].ToString() != "" ? int.Parse(data1.Rows[i]["BAIDINH"].ToString()) : 0;
                        int TAMCOC_SoLuong = data1.Rows[i]["TAMCOC"].ToString() != "" ? int.Parse(data1.Rows[i]["TAMCOC"].ToString()) : 0;
                        int PCHL_SoLuong = data1.Rows[i]["PCHL"].ToString() != "" ? int.Parse(data1.Rows[i]["PCHL"].ToString()) : 0;
                        int TAG_SoLuong = data1.Rows[i]["TAG"].ToString() != "" ? int.Parse(data1.Rows[i]["TAG"].ToString()) : 0;
                        int Tong_SoLuong = TRANGAN_SoLuong +TAMCHUC_SoLuong + BAIDINH_SoLuong + TAMCOC_SoLuong + PCHL_SoLuong + TAG_SoLuong;

                        dataRow["TRANGAN_SoLuong"] = TRANGAN_SoLuong;
                        dataRow["TAMCHUC_SoLuong"] = TAMCHUC_SoLuong;
                        dataRow["BAIDINH_SoLuong"] = BAIDINH_SoLuong;
                        dataRow["TAMCOC_SoLuong"] = TAMCOC_SoLuong;
                        dataRow["PCHL_SoLuong"] = PCHL_SoLuong;
                        dataRow["TAG_SoLuong"] = TAG_SoLuong;
                        dataRow["Tong_SoLuong"] = Tong_SoLuong;
                        dataRow["Tong_DoanhThuCacKhu"] = Tong_SoLuong * GiaBanLe;

                        switch (SiteCode)
                        {
                            case "TRANGAN":
                                // công thức tính tiền trả lại các đơn vị : số lượng combo bán ở TRANGAN * giá vé thực nhận từng khu

                                dataRow["TRANGAN_TraLai"] = TRANGAN_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCHUC_TraLai"] = TRANGAN_SoLuong * TAMCHUC_GiaBan;
                                dataRow["BAIDINH_TraLai"] = TRANGAN_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCOC_TraLai"] = TRANGAN_SoLuong * TAMCOC_GiaBan;
                                dataRow["PCHL_TraLai"] = TRANGAN_SoLuong * PCHL_GiaBan;
                                dataRow["TAG_TraLai"] = TRANGAN_SoLuong * TAG_GiaBan;
                                dataRow["TongTien_TraLai"] = TRANGAN_SoLuong * (TRANGAN_GiaBan + BAIDINH_GiaBan + TAMCOC_GiaBan + PCHL_GiaBan + TAG_GiaBan);

                                // công thức tính tiền thu từ các đơn vị : số lượng combo bán ở các đơn vị * giá vé thực nhận ở TRANGAN
                                dataRow["TRANGAN_PhaiThu"] = TRANGAN_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCHUC_PhaiThu"] = TAMCHUC_SoLuong * TRANGAN_GiaBan;
                                dataRow["BAIDINH_PhaiThu"] = BAIDINH_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCOC_PhaiThu"] = TAMCOC_SoLuong * TRANGAN_GiaBan;
                                dataRow["PCHL_PhaiThu"] = PCHL_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAG_PhaiThu"] = TAG_SoLuong * TRANGAN_GiaBan;
                                dataRow["TongTien_PhaiThu"] = TRANGAN_GiaBan * (BAIDINH_SoLuong + TAMCOC_SoLuong + PCHL_SoLuong + TAG_SoLuong);
                                dataRow["Tong_DoanhThuTaiKhu"] = TRANGAN_SoLuong * GiaBanLe;

                                break;
                            case "BAIDINH":
                                // công thức tính tiền trả lại các đơn vị  : số lượng combo bán ở BAIDINH * giá vé thực nhận từng khu
                                dataRow["TRANGAN_TraLai"] = BAIDINH_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCHUC_TraLai"] = BAIDINH_SoLuong * TAMCHUC_GiaBan;
                                dataRow["BAIDINH_TraLai"] = BAIDINH_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCOC_TraLai"] = BAIDINH_SoLuong * TAMCOC_GiaBan;
                                dataRow["PCHL_TraLai"] = BAIDINH_SoLuong * PCHL_GiaBan;
                                dataRow["TAG_TraLai"] = BAIDINH_SoLuong * TAG_GiaBan;
                                dataRow["TongTien_TraLai"] = BAIDINH_SoLuong * (TRANGAN_GiaBan + BAIDINH_GiaBan + TAMCOC_GiaBan + PCHL_GiaBan + TAG_GiaBan);

                                // công thức tính tiền thu từ các đơn vị : số lượng combo bán ở các đơn vị * giá vé thực nhận ở BAIDINH
                                dataRow["TRANGAN_PhaiThu"] = TRANGAN_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCHUC_PhaiThu"] = TAMCHUC_SoLuong * BAIDINH_GiaBan;
                                dataRow["BAIDINH_PhaiThu"] = BAIDINH_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCOC_PhaiThu"] = TAMCOC_SoLuong * BAIDINH_GiaBan;
                                dataRow["PCHL_PhaiThu"] = PCHL_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAG_PhaiThu"] = TAG_SoLuong * BAIDINH_GiaBan;
                                dataRow["TongTien_PhaiThu"] = BAIDINH_GiaBan * (TRANGAN_SoLuong + TAMCOC_SoLuong + PCHL_SoLuong + TAG_SoLuong);
                                dataRow["Tong_DoanhThuTaiKhu"] = BAIDINH_SoLuong * GiaBanLe;

                                break;
                            case "TAMCOC":
                                // công thức tính tiền trả lại các đơn vị  : số lượng combo bán ở TAMCOC * giá vé thực nhận từng khu
                                dataRow["TRANGAN_TraLai"] = TAMCOC_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCHUC_TraLai"] = TAMCOC_SoLuong * TAMCHUC_GiaBan;
                                dataRow["BAIDINH_TraLai"] = TAMCOC_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCOC_TraLai"] = TAMCOC_SoLuong * TAMCOC_GiaBan;
                                dataRow["PCHL_TraLai"] = TAMCOC_SoLuong * PCHL_GiaBan;
                                dataRow["TAG_TraLai"] = TAMCOC_SoLuong * TAG_GiaBan;
                                dataRow["TongTien_TraLai"] = TAMCOC_SoLuong * (TRANGAN_GiaBan + BAIDINH_GiaBan + TAMCOC_GiaBan + PCHL_GiaBan + TAG_GiaBan);

                                // công thức tính tiền thu từ các đơn vị : số lượng combo bán ở các đơn vị * giá vé thực nhận ở TAMCOC
                                dataRow["TRANGAN_PhaiThu"] = TRANGAN_SoLuong * TAMCOC_GiaBan;
                                dataRow["TAMCHUC_PhaiThu"] = TAMCHUC_SoLuong * TAMCOC_GiaBan;
                                dataRow["BAIDINH_PhaiThu"] = BAIDINH_SoLuong * TAMCOC_GiaBan;
                                dataRow["TAMCOC_PhaiThu"] = TAMCOC_SoLuong * TAMCOC_GiaBan;
                                dataRow["PCHL_PhaiThu"] = PCHL_SoLuong * TAMCOC_GiaBan;
                                dataRow["TAG_PhaiThu"] = TAG_SoLuong * TAMCOC_GiaBan;
                                dataRow["TongTien_PhaiThu"] = TAMCOC_GiaBan * (TRANGAN_SoLuong + BAIDINH_SoLuong + PCHL_SoLuong + TAG_SoLuong);
                                dataRow["Tong_DoanhThuTaiKhu"] = TAMCOC_SoLuong * GiaBanLe;

                                break;
                            case "PCHL":
                                // công thức tính tiền trả lại các đơn vị  : số lượng combo bán ở PCHL * giá vé thực nhận từng khu
                                dataRow["TRANGAN_TraLai"] = PCHL_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCHUC_TraLai"] = PCHL_SoLuong * TAMCHUC_GiaBan;
                                dataRow["BAIDINH_TraLai"] = PCHL_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCOC_TraLai"] = PCHL_SoLuong * TAMCOC_GiaBan;
                                dataRow["PCHL_TraLai"] = PCHL_SoLuong * PCHL_GiaBan;
                                dataRow["TAG_TraLai"] = PCHL_SoLuong * TAG_GiaBan;
                                dataRow["TongTien_TraLai"] = PCHL_SoLuong * (TRANGAN_GiaBan + BAIDINH_GiaBan + TAMCOC_GiaBan + PCHL_GiaBan + TAG_GiaBan);

                                // công thức tính tiền thu từ các đơn vị : số lượng combo bán ở các đơn vị * giá vé thực nhận ở PCHL
                                dataRow["TRANGAN_PhaiThu"] = TRANGAN_SoLuong * PCHL_GiaBan;
                                dataRow["TAMCHUC_PhaiThu"] = TAMCHUC_SoLuong * PCHL_GiaBan;
                                dataRow["BAIDINH_PhaiThu"] = BAIDINH_SoLuong * PCHL_GiaBan;
                                dataRow["TAMCOC_PhaiThu"] = TAMCOC_SoLuong * PCHL_GiaBan;
                                dataRow["PCHL_PhaiThu"] = PCHL_SoLuong * PCHL_GiaBan;
                                dataRow["TAG_PhaiThu"] = TAG_SoLuong * PCHL_GiaBan;
                                dataRow["TongTien_PhaiThu"] = PCHL_GiaBan * (TRANGAN_SoLuong + BAIDINH_SoLuong + TAMCOC_SoLuong + TAG_SoLuong);
                                dataRow["Tong_DoanhThuTaiKhu"] = PCHL_SoLuong * GiaBanLe;

                                break;
                            case "TAG":
                                // công thức tính tiền trả lại các đơn vị  : số lượng combo bán ở TAG * giá vé thực nhận từng khu
                                dataRow["TRANGAN_TraLai"] = TAG_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCHUC_TraLai"] = TAG_SoLuong * TAMCHUC_GiaBan;
                                dataRow["BAIDINH_TraLai"] = TAG_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCOC_TraLai"] = TAG_SoLuong * TAMCOC_GiaBan;
                                dataRow["PCHL_TraLai"] = TAG_SoLuong * PCHL_GiaBan;
                                dataRow["TAG_TraLai"] = TAG_SoLuong * TAG_GiaBan;
                                dataRow["TongTien_TraLai"] = TAG_SoLuong * (TRANGAN_GiaBan + TRANGAN_GiaBan + TAMCOC_GiaBan + PCHL_GiaBan + TAG_GiaBan);

                                // công thức tính tiền thu từ các đơn vị : số lượng combo bán ở các đơn vị * giá vé thực nhận ở TAG
                                dataRow["TRANGAN_PhaiThu"] = TRANGAN_SoLuong * TAG_GiaBan;
                                dataRow["TAMCHUC_PhaiThu"] = TAMCHUC_SoLuong * TAG_GiaBan;
                                dataRow["BAIDINH_PhaiThu"] = BAIDINH_SoLuong * TAG_GiaBan;
                                dataRow["TAMCOC_PhaiThu"] = TAMCOC_SoLuong * TAG_GiaBan;
                                dataRow["PCHL_PhaiThu"] = PCHL_SoLuong * TAG_GiaBan;
                                dataRow["TAG_PhaiThu"] = TAG_SoLuong * TAG_GiaBan;
                                dataRow["TongTien_PhaiThu"] = TAG_GiaBan * (TRANGAN_SoLuong + BAIDINH_SoLuong + TAMCOC_SoLuong + PCHL_SoLuong);
                                dataRow["Tong_DoanhThuTaiKhu"] = TAG_SoLuong * GiaBanLe;

                                break;
                            case "TAMCHUC":
                                // công thức tính tiền trả lại các đơn vị  : số lượng combo bán ở TAG * giá vé thực nhận từng khu
                                dataRow["TRANGAN_TraLai"] = TAMCHUC_SoLuong * TRANGAN_GiaBan;
                                dataRow["TAMCHUC_TraLai"] = TAMCHUC_SoLuong * TAMCHUC_GiaBan;
                                dataRow["BAIDINH_TraLai"] = TAMCHUC_SoLuong * BAIDINH_GiaBan;
                                dataRow["TAMCOC_TraLai"] = TAMCHUC_SoLuong * TAMCOC_GiaBan;
                                dataRow["PCHL_TraLai"] = TAMCHUC_SoLuong * PCHL_GiaBan;
                                dataRow["TAG_TraLai"] = TAMCHUC_SoLuong * TAG_GiaBan;
                                dataRow["TongTien_TraLai"] = TAMCHUC_SoLuong * (TRANGAN_GiaBan + TRANGAN_GiaBan + TAMCOC_GiaBan + PCHL_GiaBan + TAG_GiaBan);

                                // công thức tính tiền thu từ các đơn vị : số lượng combo bán ở các đơn vị * giá vé thực nhận ở TAG
                                dataRow["TRANGAN_PhaiThu"] = TRANGAN_SoLuong * TAMCHUC_GiaBan;
                                dataRow["TAMCHUC_PhaiThu"] = TAMCHUC_SoLuong * TAMCHUC_GiaBan;
                                dataRow["BAIDINH_PhaiThu"] = BAIDINH_SoLuong * TAMCHUC_GiaBan;
                                dataRow["TAMCOC_PhaiThu"] = TAMCOC_SoLuong * TAMCHUC_GiaBan;
                                dataRow["PCHL_PhaiThu"] = PCHL_SoLuong * TAMCHUC_GiaBan;
                                dataRow["TAG_PhaiThu"] = TAG_SoLuong * TAMCHUC_GiaBan;
                                dataRow["TongTien_PhaiThu"] = TAMCHUC_GiaBan * (TRANGAN_SoLuong + BAIDINH_SoLuong + TAMCOC_SoLuong + PCHL_SoLuong);
                                dataRow["Tong_DoanhThuTaiKhu"] = TAMCHUC_SoLuong * GiaBanLe;

                                break;
                        }
                        data.Rows.Add(dataRow);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
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