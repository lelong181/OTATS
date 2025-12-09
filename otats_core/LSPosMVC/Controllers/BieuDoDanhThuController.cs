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
using System.Globalization;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/BieuDoDoanhThu")]
    public class BieuDoDanhThuController : ApiController
    {

        //BÁO CÁO TỔNG HỢP TỒN CÁC MẶT HÀNG CÁC KHO
        [HttpGet]
        [Route("BaoCaoTongHopChuongTrinhKhuyenMai")]
        public HttpResponseMessage BaoCaoTongHopChuongTrinhKhuyenMai([FromUri]  DateTime fromdate, DateTime todate, int orderby)
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
                    DataSet ds = bc_dl.BaoCaoBieuDoDoanhThuTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, orderby);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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



        //BIỂU ĐỒ TOP 10 NHÂN VIÊN DI CHUYỂN
        [HttpGet]
        [Route("APIBieuDoTopTenDoanhThuKhachHang")]
        public HttpResponseMessage APIBieuDoTopTenDoanhThuKhachHang([FromUri]  DateTime fromdate, DateTime todate, int orderby)
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
                 DataSet   ds = bc_dl.BaoCaoBieuDoDoanhThuTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, orderby);
                    if (ds.Tables.Count >0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BieuDoTopTenDoanhThuKhachHang")]
        public HttpResponseMessage BieuDoTopTenDoanhThuKhachHang([FromUri]  DateTime fromdate, DateTime todate, int orderby)
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
                    DataSet ds = bc_dl.BaoCaoBieuDoDoanhThuTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, orderby);
                    if (ds.Tables.Count >0)
                    {
                        List<object> iData = new List<object>();

                        List<string> labels = new List<string>();

                        List<double> lst_dataItem = new List<double>();

                        int i = 0;
                        //ds.Tables[0].Columns.Add("STT", typeof(int));
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //row["STT"] = i + 1;

                            //Them top 10
                            if (i <= 9)
                            {
                                labels.Add(row["TenKhachHang"].ToString());

                                lst_dataItem.Add(Convert.ToDouble(row["DoanhThu"].ToString()));
                            }
                            i++;
                        }
                        iData.Add(labels);
                        iData.Add(lst_dataItem);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        response = Request.CreateResponse(HttpStatusCode.OK, iData);
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
        [Route("APIBieuDoTopTenDoanhThuTheoNhanVien")]
        public HttpResponseMessage APIBieuDoTopTenDoanhThuTheoNhanVien([FromUri]  DateTime fromdate, DateTime todate)
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
                    DataSet ds = bc_dl.BieuDoTopTenTheoNhanVien(userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BieuDoTopTenDoanhThuTheoNhanVien")]
        public HttpResponseMessage BieuDoTopTenDoanhThuTheoNhanVien([FromUri]  DateTime fromdate, DateTime todate)
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
                    DataSet ds = bc_dl.BieuDoTopTenTheoNhanVien(userinfo.ID_QLLH, fromdate, todate, 0);
                    if (ds.Tables.Count > 0)
                    {
                        List<object> iData = new List<object>();

                        List<string> labels = new List<string>();

                        List<double> lst_dataItem = new List<double>();

                        int i = 0;
                        //ds.Tables[0].Columns.Add("STT", typeof(int));
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //row["STT"] = i + 1;

                            //Them top 10
                            if (i <= 9)
                            {
                                labels.Add(row["TenNhanVien"].ToString());

                                lst_dataItem.Add(Convert.ToDouble(row["DoanhThu"].ToString()));
                            }
                            i++;
                        }

                        iData.Add(labels);

                        iData.Add(lst_dataItem);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        response = Request.CreateResponse(HttpStatusCode.OK, iData);
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



        // DOANH THU THÁNG 

        [HttpGet]
        [Route("APIBieuDoDoanhThuThang")]
        public HttpResponseMessage APIBieuDoDoanhThuThang([FromUri]  DateTime fromdate, int id_Nhom, int id_NhanVien, int id_KhachHang)
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
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BieuDoDoanhThuThang")]
        public HttpResponseMessage BieuDoDoanhThuThang([FromUri]  DateTime fromdate, int id_Nhom, int id_NhanVien, int id_KhachHang)
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
                    DataSet ds = bc_dl.BaoCaoDoanhThuNgay_V2(userinfo.ID_QLLH,fromdate,userinfo.ID_QuanLy, id_Nhom,id_KhachHang, id_NhanVien);
                    if (ds.Tables.Count > 0)
                    {
                        List<object> iData = new List<object>();

                        DataTable dtBieuDo = ds.Tables[0];
                        DateTime Thang = new DateTime();

                        try
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            try
                            {
                                Thang = DateTime.ParseExact("01/" + fromdate, "dd/MM/yyyy", provider);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    Thang = DateTime.ParseExact("1/" + fromdate, "d/M/yyyy", provider);
                                }
                                catch (Exception)
                                {
                                }
                            }

                        }
                        catch (Exception)
                        {
                        }

                        DateTime _fromdate = new DateTime(Thang.Year, Thang.Month, 1);
                        DateTime todate = _fromdate.AddMonths(1).AddDays(-1);

                        for (DateTime i = fromdate; i < todate; i = i.AddDays(1))
                        {
                            DataRow dr = dtBieuDo.NewRow();
                            dr["InsertDate"] = i;
                            dr["TongTien"] = 0;
                            dr["DonHang"] = 0;
                            dr["Label"] = "Doanh thu";
                            dr["NgayHienThi"] = i.ToString("dd");
                            if (dtBieuDo.Select("InsertDate = '" + i.ToString("yyyy-MM-dd") + "'").Length == 0)
                                dtBieuDo.Rows.Add(dr);
                        }

                        List<string> labels = new List<string>();

                        List<double> lst_dataItem1 = new List<double>();
                        List<float> lst_dataItem2 = new List<float>();

                        dtBieuDo.DefaultView.Sort = "InsertDate ASC";
                        dtBieuDo = dtBieuDo.DefaultView.ToTable();

                        float max = 1;

                        foreach (DataRow dr in dtBieuDo.Rows)
                        {
                            //float a = Convert.ToSingle(dr["TongTien"].ToString());
                            float b = Convert.ToSingle(dr["DonHang"].ToString());


                            if (b > max)
                                max = b;

                            DateTime day = Convert.ToDateTime(dr["InsertDate"].ToString());
                            labels.Add(day.ToString("dd"));

                            lst_dataItem1.Add(Convert.ToDouble(dr["TongTien"].ToString()));
                            lst_dataItem2.Add(Convert.ToSingle(dr["DonHang"].ToString()));
                        }

                        //tính stepsize
                        int result = 1;
                        float stepSize = (max / 5) != 0 ? (max / 5) : 1;

                        if (stepSize > 5)
                        {
                            result = Convert.ToInt32((stepSize / 5)) * 5;
                        }
                        else
                        {
                            result = stepSize < 1 ? 1 : Convert.ToInt32(stepSize);
                        }//end stepsize

                        iData.Add(labels);

                        iData.Add(lst_dataItem1);
                        iData.Add(lst_dataItem2);
                        iData.Add(stepSize);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        response = Request.CreateResponse(HttpStatusCode.OK, iData);
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
        [Route("APIBieuDoDoanhThuTheoNhomNhanVien")]
        public HttpResponseMessage APIBieuDoDoanhThuTheoNhomNhanVien([FromUri]  DateTime fromdate, DateTime todate)
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
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BieuDoDoanhThuTheoNhomNhanVien")]
        public HttpResponseMessage BieuDoDoanhThuTheoNhomNhanVien([FromUri]  DateTime fromdate, DateTime todate)
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
                        List<object> iData = new List<object>();

                        List<string> labels = new List<string>();

                        List<double> lst_dataItem = new List<double>();

                        int i = 0;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {

                            //Them top 10
                            if (i <= 9)
                            {
                                labels.Add(row["TenNhom"].ToString());

                                lst_dataItem.Add(Convert.ToDouble(row["DoanhThu"].ToString()));
                            }
                            i++;
                        }

                        iData.Add(labels);

                        iData.Add(lst_dataItem);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        response = Request.CreateResponse(HttpStatusCode.OK, iData);
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
        [Route("APIBieuDoDoanhThuTheoKhuVuc")]
        public HttpResponseMessage APIBieuDoDoanhThuTheoKhuVuc([FromUri]  DateTime fromdate, DateTime todate)
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
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BieuDoDoanhThuTheoKhuVuc")]
        public HttpResponseMessage BieuDoDoanhThuTheoKhuVuc([FromUri]  DateTime fromdate, DateTime todate)
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
                        List<object> iData = new List<object>();
                        List<string> labels = new List<string>();
                        List<double> lst_dataItem = new List<double>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            labels.Add(row["TenKhuVuc"].ToString());
                            lst_dataItem.Add(Convert.ToDouble(row["TongTien"].ToString()));
                        }
                        iData.Add(labels);
                        iData.Add(lst_dataItem);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        response = Request.CreateResponse(HttpStatusCode.OK, iData);
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
        [Route("APIBieuDoDoanhThuNhanVien")]
        public HttpResponseMessage APIBieuDoDoanhThuNhanVien([FromUri]  DateTime fromdate, DateTime todate, int id_Nhom, int id_NhanVien)
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
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BieuDoDoanhThuNhanVien")]
        public HttpResponseMessage BieuDoDoanhThuNhanVien([FromUri]  DateTime fromdate, DateTime todate, int id_Nhom, int id_NhanVien)
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
                        List<object> iData = new List<object>();
                        List<string> labels = new List<string>();
                        List<double> lst_dataItem = new List<double>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            labels.Add(row["TenNhanVien"].ToString());
                            lst_dataItem.Add(Convert.ToDouble(row["TongTien"].ToString()));
                        }
                        iData.Add(labels);
                        iData.Add(lst_dataItem);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        response = Request.CreateResponse(HttpStatusCode.OK, iData);

                        //response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BaoCaoBanHang")]
        public HttpResponseMessage BaoCaoBanHang([FromUri]  DateTime fromdate, DateTime todate, int id_KhachHang)
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
                    DataSet ds = bc_dl.BaoCaoBanHang_KhachHang(userinfo.ID_QLLH, id_KhachHang,  fromdate, todate, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("BieuDoTyTrongKhachHang")]
        public HttpResponseMessage BieuDoTyTrongKhachHang([FromUri]  DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                List<object> iData = new List<object>();
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
                        List<string> labels = new List<string>();
                        List<Double> lst_dataItem = new List<Double>();
                        int i = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (i <= 9)
                            {
                                labels.Add(row["TenLoaiKhachHang"].ToString());
                                lst_dataItem.Add(Convert.ToDouble(row["TongTien"].ToString()));
                            }
                            i++;
                        }
                        iData.Add(labels);
                        iData.Add(lst_dataItem);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        response = Request.CreateResponse(HttpStatusCode.OK, iData);
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
        [Route("APIChietKhauDoanhThuTheoNhomNhanVien")]
        public HttpResponseMessage APIChietKhauDoanhThuTheoNhomNhanVien([FromUri] DateTime fromdate, DateTime todate)
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
                    DataSet ds = bc_dl.ChietKhauDoanhThuTheoNhomNhanVien(userinfo.ID_QLLH, fromdate, todate);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
    }
}
