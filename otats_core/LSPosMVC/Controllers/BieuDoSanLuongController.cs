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
using log4net.Core;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/BieuDoSanLuong")]
    public class BieuDoSanLuongController : ApiController
    {
        //BIỂU ĐỒ TOP 10 NHÂN VIÊN DI CHUYỂN
        [HttpGet]
        [Route("BieuDoDiChuyen")]
        public HttpResponseMessage BieuDoDiChuyen([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                BieuDoDiChuyenDTO bdo = new BieuDoDiChuyenDTO();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    bdo = bc_dl.GetTop10DiChuyen(fromdate, todate, userinfo.ID_QLLH);
                    if (bdo != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, bdo);
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




        //BIỂU ĐỒ TOP 10 NHÂN VIÊN VIẾNG THĂM
        [HttpGet]
        [Route("BaoCaoViengThamNhanVien")]
        public HttpResponseMessage BaoCaoViengThamNhanVien([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                BieuDoViengThamDTO bdo = new BieuDoViengThamDTO();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    bdo = bc_dl.GetBieuDoTop10ViengTham(fromdate, todate, userinfo.ID_QLLH);
                    if (bdo != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, bdo);
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

        //BIỂU ĐỒ TOP 10 KHÁCH HÀNG VIẾNG THĂM
        [HttpGet]
        [Route("BieuDoViengThamKhachHang")]
        public HttpResponseMessage BieuDoViengThamKhachHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                BieuDoViengThamDTO bdo = new BieuDoViengThamDTO();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    bdo = bc_dl.GetBieuDoTop10KHViengTham(fromdate, todate, userinfo.ID_QLLH);
                    if (bdo != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, bdo);
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





        //BIỂU ĐỒ TOP 10 SẢN PHẨM BÁN CHẠY NHẤT
        [HttpGet]
        [Route("BaoCaoTopTenSanPhamTheoKhachHang")]
        public HttpResponseMessage BaoCaoTopTenSanPhamTheoKhachHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                //BieuDoDiChuyenDTO bdo = new BieuDoDiChuyenDTO();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = new DataSet();
                    int ID_QuanLy = userinfo.ID_QuanLy;
                    if (userinfo.Level == 1)
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 0);
                    }
                    else
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang_Level2(userinfo.ID_QLLH, ID_QuanLy, fromdate, todate, 0);
                    }
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

        //BÁO CÁO BIỂU ĐỒ TOP 10 SẢN PHẨM BÁN CHẠY NHẤT
        [HttpGet]
        [Route("BieuDoTopTenSanPhamTheoKhachHang")]
        public HttpResponseMessage BieuDoTopTenSanPhamTheoKhachHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                List<object> iData = new List<object>();
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = new DataSet();
                    int ID_QuanLy = userinfo.ID_QuanLy;
                    if (userinfo.Level == 1)
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 0);
                    }
                    else
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang_Level2(userinfo.ID_QLLH, ID_QuanLy, fromdate, todate, 0);
                    }
                    if (ds.Tables.Count > 0)
                    {
                        List<string> labels = new List<string>();
                        List<double> lst_dataItem1 = new List<double>();
                        List<double> lst_dataItem2 = new List<double>();
                        int i = 0;
                        Double max = 1;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //Them top 10
                            if (i <= 9)
                            {
                                labels.Add(row["TenHang"].ToString());

                                lst_dataItem1.Add(Convert.ToDouble(row["DoanhThu"].ToString()));

                                Double a = Convert.ToDouble(row["SoLuong"].ToString());
                                lst_dataItem2.Add(a);

                                if (a > max)
                                {
                                    max = a;
                                }
                            }
                            i++;
                        }
                        //tính stepsize
                        int result = 1;
                        Double stepSize = (max / 5) != 0 ? (max / 5) : 1;
                        if (stepSize > 5)
                        {
                            result = Convert.ToInt32((stepSize / 5) * 5);
                        }
                        else
                        {
                            result = Convert.ToInt32(stepSize);
                        }
                        iData.Add(labels);
                        iData.Add(lst_dataItem1);
                        iData.Add(lst_dataItem2);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        iData.Add(result);
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





        //BIỂU ĐỒ TOP 10 ĐƠN HÀNG THEO KHÁCH HÀNG
        [HttpGet]
        [Route("BaoCaoTopTenDonHangKhachHang")]
        public HttpResponseMessage BaoCaoTopTenDonHangKhachHang([FromUri] DateTime fromdate, DateTime todate, int phanloai)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoBieuDoDonHangTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, phanloai);
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

        //BÁO CÁO BIỂU ĐỒ TOP 10 ĐƠN HÀNG THEO KHÁCH HÀNG
        [HttpGet]
        [Route("BieuDoTopTenDonHangKhachHang")]
        public HttpResponseMessage BieuDoTopTenDonHangKhachHang([FromUri] DateTime fromdate, DateTime todate, int phanloai)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                List<object> iData = new List<object>();
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.BaoCaoBieuDoDonHangTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, phanloai);
                    if (ds.Tables.Count > 0)
                    {
                        List<string> labels = new List<string>();

                        List<float> lst_dataItem = new List<float>();

                        float max = 0;

                        int i = 0;
                        //ds.Tables[0].Columns.Add("STT", typeof(int));
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //row["STT"] = i + 1;

                            //Them top 10
                            if (i <= 9)
                            {
                                labels.Add(row["TenKhachHang"].ToString());

                                float a = Convert.ToSingle(row["SoDonHang"].ToString());

                                lst_dataItem.Add(a);

                                if (a > max)
                                {
                                    max = a;
                                }
                            }
                            i++;
                        }

                        //tính stepsize
                        int result = 1;
                        int stepSize = Convert.ToInt32(max / 5) != 0 ? Convert.ToInt32(max / 5) : 1;

                        if (stepSize > 5)
                        {
                            result = (stepSize / 5) * 5;
                        }
                        else
                        {
                            result = stepSize;
                        }
                        iData.Add(labels);

                        iData.Add(lst_dataItem);
                        iData.Add(result);
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

        //BIỂU ĐỒ TOP 10 SẢN PHẨM BÁN THẤP NHẤT
        [HttpGet]
        [Route("BaoCaoTopTenSanPhamBanThapTheoKhachHang")]
        public HttpResponseMessage BaoCaoTopTenSanPhamBanThapTheoKhachHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = new DataSet();
                    int ID_QuanLy = userinfo.ID_QuanLy;
                    if (userinfo.Level == 1)
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 1);
                    }
                    else
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang_Level2(userinfo.ID_QLLH, ID_QuanLy, fromdate, todate, 1);
                    }
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

        //BÁO CÁO BIỂU ĐỒ TOP 10 SẢN PHẨM BÁN THẤP NHẤT
        [HttpGet]
        [Route("BieuDoTopTenSanPhamBanThapTheoKhachHang")]
        public HttpResponseMessage BieuDoTopTenSanPhamBanThapTheoKhachHang([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                List<object> iData = new List<object>();
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = new DataSet();
                    int ID_QuanLy = userinfo.ID_QuanLy;
                    if (userinfo.Level == 1)
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang(userinfo.ID_QLLH, fromdate, todate, 1);
                    }
                    else
                    {
                        ds = bc_dl.BieuDoTopTenSanPhamTheoKhachHang_Level2(userinfo.ID_QLLH, ID_QuanLy, fromdate, todate, 1);
                    }
                    if (ds.Tables.Count > 0)
                    {

                        List<string> labels = new List<string>();

                        List<double> lst_dataItem1 = new List<double>();
                        List<double> lst_dataItem2 = new List<double>();


                        double max = 1;
                        int i = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {

                            //Them top 10
                            if (i <= 9)
                            {
                                labels.Add(row["TenHang"].ToString());

                                lst_dataItem1.Add(Convert.ToDouble(row["DoanhThu"].ToString()));

                                double a = Convert.ToDouble(row["SoLuong"].ToString());
                                lst_dataItem2.Add(a);

                                if (a > max)
                                {
                                    max = a;
                                }
                            }
                            i++;
                        }


                        //tính stepsize
                        int result = 1;
                        double stepSize = (max / 5) != 0 ? (max / 5) : 1;

                        if (stepSize > 5)
                        {
                            result = Convert.ToInt32((stepSize / 5) * 5);
                        }
                        else
                        {
                            result = Convert.ToInt32(stepSize);
                        }

                        iData.Add(labels);

                        iData.Add(lst_dataItem1);
                        iData.Add(lst_dataItem2);
                        //iData.Add(stepSize);
                        iData.Add(userinfo.DinhDangTienSoThapPhan);
                        iData.Add(result);
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




        //BIỂU ĐỒ TOP 10 ĐƠN HÀNG THEO NHÂN VIÊN
        [HttpGet]
        [Route("BaoCaoTopTenDonHangTheoNhanVien")]
        public HttpResponseMessage BaoCaoTopTenDonHangTheoNhanVien([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
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

        //BÁO CÁO BIỂU ĐỒ TOP 10 ĐƠN HÀNG THEO NHÂN VIÊN
        [HttpGet]
        [Route("BieuDoTopTenDonHangTheoNhanVien")]
        public HttpResponseMessage BieuDoTopTenDonHangTheoNhanVien([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                List<object> iData = new List<object>();
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
                        List<string> labels = new List<string>();
                        List<double> lst_dataItem = new List<double>();
                        double max = 1;
                        int i = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //Them top 10
                            if (i <= 9)
                            {
                                labels.Add(row["TenNhanVien"].ToString());

                                double a = Convert.ToDouble(row["SoDonHang"].ToString());

                                lst_dataItem.Add(Convert.ToDouble(row["SoDonHang"].ToString()));

                                if (a > max)
                                {
                                    max = a;
                                }
                            }
                            i++;
                        }

                        //tính stepsize
                        int result = 1;
                        double stepSize = (max / 5) != 0 ? (max / 5) : 1;

                        if (stepSize > 5)
                        {
                            result = Convert.ToInt32((stepSize / 5) * 5);
                        }
                        else
                        {
                            result = Convert.ToInt32(stepSize);
                        }

                        iData.Add(labels);
                        iData.Add(lst_dataItem);
                        iData.Add(result);
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




        //BIỂU ĐỒ ĐƠN HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("BaoCaoDonHangTheoKhuVuc")]
        public HttpResponseMessage BaoCaoDonHangTheoKhuVuc([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
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

        //BÁO CÁO BIỂU ĐỒ ĐƠN HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("BieuDoDonHangTheoKhuVuc")]
        public HttpResponseMessage BieuDoDonHangTheoKhuVuc([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                List<object> iData = new List<object>();
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
                        List<string> labels = new List<string>();
                        List<float> lst_dataItem = new List<float>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            labels.Add(row["Ten"].ToString());
                            lst_dataItem.Add(Convert.ToSingle(row["SoDonHang"].ToString()));
                        }

                        iData.Add(labels);

                        iData.Add(lst_dataItem);
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



        //BIỂU ĐỒ ĐƠN HÀNG THEO NHÓM NHÂN VIÊN
        [HttpGet]
        [Route("BieuDoDonHangTheoNhanVien")]
        public HttpResponseMessage BieuDoDonHangTheoNhanVien([FromUri] DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                BieuDo_DonHang_NhomNhanVienDTO iData = new BieuDo_DonHang_NhomNhanVienDTO();
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    iData = bc_dl.GetBieuDo_DonHang_NhomNhanVien(fromdate, todate, userinfo.ID_QLLH);
                    if (iData != null)
                    {
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




        //BIỂU ĐỒ PHÂN LOẠI KHÁCH HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("BaoCaoPhanLoaiKhachHangTheoKhuVuc")]
        public HttpResponseMessage BaoCaoPhanLoaiKhachHangTheoKhuVuc()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
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

        //BÁO CÁO BIỂU ĐỒ PHÂN LOẠI KHÁCH HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("BieuDoPhanLoaiKhachHangTheoKhuVuc")]
        public HttpResponseMessage BieuDoPhanLoaiKhachHangTheoKhuVuc()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                List<object> iData = new List<object>();
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

                        List<string> labels = new List<string>();
                        List<double> lst_dataItem = new List<double>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            labels.Add(row["TenKhuVuc"].ToString());
                            lst_dataItem.Add(Convert.ToDouble(row["SoKhachHang"].ToString()));
                        }
                        iData.Add(labels);
                        iData.Add(lst_dataItem);
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



        //BIỂU ĐỒ PHÂN LOẠI KHÁCH HÀNG THEO NGÀNH HÀNG
        [HttpGet]
        [Route("BaoCaoPhanLoaiKhachHangNganhHang")]
        public HttpResponseMessage BaoCaoPhanLoaiKhachHangNganhHang()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
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

        //BIỂU ĐỒ PHÂN LOẠI KHÁCH HÀNG THEO NGÀNH HÀNG
        [HttpGet]
        [Route("BieuDoPhanLoaiKhachHangNganhHang")]
        public HttpResponseMessage BieuDoPhanLoaiKhachHangNganhHang()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                BieuDoSanLuongDAL bc_dl = new BieuDoSanLuongDAL();
                List<object> iData = new List<object>();
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

                        List<string> labels = new List<string>();
                        List<double> lst_dataItem = new List<double>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            labels.Add(row["TenDanhMuc"].ToString());

                            lst_dataItem.Add(Convert.ToDouble(row["SoKhachHang"].ToString()));
                        }
                        iData.Add(labels);

                        iData.Add(lst_dataItem);
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


    }
}
