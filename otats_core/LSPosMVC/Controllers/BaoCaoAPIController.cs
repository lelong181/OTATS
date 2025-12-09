using LSPosMVC.Common;
using LSPos_Data.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LSPosMVC.Controllers
{
    /// <summary>
    /// DONGNN -- 2019-07-15
    /// </summary>
    [Authorize]
    [RoutePrefix("api/BaoCaoAPI")]
    public class BaoCaoAPIController : ApiController
    {
        #region BÁO CÁO KHÁCH HÀNG

        //BÁO CÁO TỔNG HỢP THEO KHÁCH HÀNG
        [HttpGet]
        [Route("BaoCaoTongHopTheoKhachHang")]
        public HttpResponseMessage BaoCaoTongHopTheoKhachHang([FromUri] int ID_KhachHang, DateTime fromdate, DateTime todate)
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
        //BÁO CÁO CHUYẾN ĐÒ
        [HttpGet]
        [Route("BaoCaoChuyenDo")]
        public HttpResponseMessage BaoCaoChuyenDo([FromUri] DateTime fromdate, DateTime todate)
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
                    DataSet ds = bc_dl.BaoCaoChuyenDo(fromdate, todate);
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
        //BÁO CÁO MẶT HÀNG - KHÁCH HÀNG - ĐƠN HÀNG
        [Route("BaoCaoMH_KH_DH_NV")]
        public HttpResponseMessage BaoCaoMH_KH_DH_NV([FromUri] int id_MatHang, int id_KhachHang, DateTime fromdate, DateTime todate, int id_NhanVien)
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

        //BÁO CÁO MẶT HÀNG - KHÁCH HÀNG
        [HttpGet]
        [Route("BaoCaoMatHang_KH")]
        public HttpResponseMessage BaoCaoMatHang_KH([FromUri] int id_MatHang,int id_LoaiKhachHang, int id_KhachHang,string idnhanvien, DateTime fromdate, DateTime todate)
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
                    DataSet ds = bc_dl.BaoCaoMH_KH(userinfo.ID_QLLH, id_MatHang, id_LoaiKhachHang, id_KhachHang, idnhanvien, fromdate, todate, userinfo.ID_QuanLy);
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

        //BÁO CÁO ẢNH CHỤP
        [HttpGet]
        [Route("BaoCaoAnhChupTheoAlbum")]
        public HttpResponseMessage BaoCaoAnhChupTheoAlbum([FromUri] int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO TỔNG HỢP VÀO ĐIỂM THEO KHÁCH HÀNG
        [HttpGet]
        [Route("BaoCaoTongHopCheckInTheoKhachHang")]
        public HttpResponseMessage BaoCaoTongHopCheckInTheoKhachHang([FromUri] int id_Nhom, int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO VIẾNG THĂM KHÁCH HÀNG
        [HttpGet]
        [Route("BaoCaoViengThamTheoKhachHang")]
        public HttpResponseMessage BaoCaoViengThamTheoKhachHang([FromUri] int id_Nhom, int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
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
                    DataSet dt = bc_dl.BaoCaoViengThamKhachHang(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_Nhom, id_KhachHang, id_NhanVien, fromdate, todate);
                    if (dt.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, dt.Tables[0]);
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
        [Route("BieuDoViengThamTheoKhachHang")]
        public HttpResponseMessage BieuDoViengThamTheoKhachHang([FromUri] int id_Nhom, int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
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
                    DataSet dt = bc_dl.BaoCaoViengThamKhachHang(userinfo.ID_QLLH, userinfo.ID_QuanLy, id_Nhom, id_KhachHang, id_NhanVien, fromdate, todate);

                    if (dt.Tables.Count > 0)
                    {
                        List<object> iData = new List<object>();

                        List<string> labels = new List<string>();

                        List<double> lst_dataItem = new List<double>();
                        int i = 0;
                        //ds.Tables[0].Columns.Add("STT", typeof(int));
                        foreach (DataRow row in dt.Tables[0].Rows)
                        {

                            labels.Add(row["TenNhanVien"].ToString());

                            lst_dataItem.Add(Convert.ToDouble(row["SoKhachKhongViengTham"].ToString()));
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
        //BÁO CÁO KHÁCH HÀNG MỞ MỚI
        [HttpGet]
        [Route("BaoCaoKhachHangMoMoi")]
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

        //BÁO CÁO KHÁCH HÀNG THEO GIAO DỊCH
        [HttpGet]
        [Route("BaoCaoKhachHangTheoGiaoDich")]
        public HttpResponseMessage BaoCaoKhachHangTheoGiaoDich([FromUri]  int id_NhanVien, int id_KhachHang, int soNgay, int loai)
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

        //BÁO CÁO PHẢN HỒI
        [HttpGet]
        [Route("BaoCaoPhanHoi")]
        public HttpResponseMessage BaoCaoPhanHoi([FromUri]  int id_NhanVien, int id_KhachHang, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO KHÁCH HÀNG THEO KHU VỰC
        [HttpGet]
        [Route("BaoCaoKhachHangTheoKhuVuc")]
        public HttpResponseMessage BaoCaoKhachHangTheoKhuVuc([FromUri]  int id_Tinh, int id_Quan)
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

        #endregion

        #region BÁO CÁO NHÂN VIÊN
        //BÁO CÁO LỊCH SỬ GIAO HÀNG
        [HttpGet]
        [Route("BaoCaoLichSuGiaoHang")]
        public HttpResponseMessage BaoCaoLichSuGiaoHang([FromUri] int id_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO LỊCH SỬ BẢO DƯỠNG SỬA CHỮA
        [HttpGet]
        [Route("BaoCaoLichSuBaoDuongSuaChua")]
        public HttpResponseMessage BaoCaoLichSuBaoDuongSuaChua([FromUri] int id_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO KPI NHÂN VIÊN
        [HttpGet]
        [Route("BaoCaoKPINhanVien")]
        public HttpResponseMessage BaoCaoKPINhanVien([FromUri] int id_Nhom, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO ĐƠN HÀNG THEO ĐIỂM
        [HttpGet]
        [Route("BaoCaoDonHangTheoDiem")]
        public HttpResponseMessage BaoCaoDonHangTheoDiem([FromUri] int id_Nhom, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO ĐƠN HÀNG THEO ĐIỂM
        [HttpGet]
        [Route("BaoCaoDonHangTheoDiem_ChiTiet")]
        public HttpResponseMessage BaoCaoDonHangTheoDiem_ChiTiet([FromUri]int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    DataTable dt = bc_dl.BaoCaoDonHangTheoDiem_ChiTiet(id_NhanVien, fromdate, todate);
                    if (dt.Rows.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, dt);
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
        [Route("baoCaoHoanThanhDonHangCoBanThucTe")]
        public HttpResponseMessage baoCaoHoanThanhDonHangCoBanThucTe([FromUri] int id_KhachHang, int id_NhanVien, int id_MatHang, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO MẶT HÀNG - NHÂN VIÊN
        [HttpGet]
        [Route("BaoCaoMatHang_NV")]
        public HttpResponseMessage BaoCaoMatHang_NV([FromUri] int id_KhachHang, int id_NhanVien, int id_MatHang, int id_NganhHang, DateTime fromdate, DateTime todate)
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
        [Route("BaoCaoMatHang_NV_ChiTiet")]
        public HttpResponseMessage BaoCaoMatHang_NV_ChiTiet([FromUri] int id_KhachHang, int id_NhanVien, int id_MatHang, DateTime fromdate, DateTime todate)
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
                    BaoCao_dl dl = new BaoCao_dl();
                    DataSet ds = dl.BaoCaoMH_NV_ChiTiet(userinfo.ID_QLLH, id_MatHang, id_NhanVien, fromdate, todate, userinfo.ID_QuanLy);
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

        //BÁO CÁO QUÃNG ĐƯỜNG ĐI
        [HttpGet]
        [Route("BaoCaoQuangDuongDiChuyen")]
        public HttpResponseMessage BaoCaoQuangDuongDiChuyen([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    DataTable rs = new DataTable();
                    try
                    {
                        BaoCao_dl bcd = new BaoCao_dl();

                        if (fromdate.Date == DateTime.Now.Date)
                        {
                            rs = bcd.BaoCaoKmDiChuyen_TrongNgay(id_NhanVien, userinfo.ID_QuanLy, userinfo.ID_QLLH, fromdate, todate);
                        }
                        else
                        {
                            rs = bcd.BaoCaoKmDiChuyen(id_NhanVien, userinfo.ID_QuanLy, userinfo.ID_QLLH, fromdate, todate);
                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, rs);

                    //BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    //DataSet ds = bc.BaoCaoQuangDuongDiChuyen(userinfo.ID_QLLH, id_NhanVien, fromdate, todate, userinfo.ID_QuanLy);
                    //if (ds.Tables.Count > 0)
                    //{
                    //    response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
                    //}
                    //else
                    //{
                    //    response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
                    //}
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
        [Route("BaoCaoDungDo")]
        public HttpResponseMessage BaoCaoDungDo([FromUri]  int id_NhanVien, float dungDo, DateTime fromdate, DateTime todate)
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

        // BÁO CÁO BẬT TẮT GPS
        [HttpGet]
        [Route("BaoCaoBatTatGPS")]
        public HttpResponseMessage BaoCaoBatTatGPS([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO TÌNH TRẠNG FAKE GPS
        [HttpGet]
        [Route("BaoCaoBatTatFakeGPS")]
        public HttpResponseMessage BaoCaoBatTatFakeGPS([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //BÁO CÁO VIẾNG THĂM TUYẾN
        [HttpGet]
        [Route("BaoCaoCheckInTuyen")]
        public HttpResponseMessage BaoCaoCheckInTuyen([FromUri]  int id_Tuyen, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //LỊCH SỬ MẤT TÍN HIỆU
        [HttpGet]
        [Route("BaoCaoLichSuMatTinHieu")]
        public HttpResponseMessage BaoCaoLichSuMatTinHieu([FromUri]  int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //BIỂU ĐỒ KPI CÁC CHỈ TIÊU
        [HttpGet]
        [Route("BieuDoKPI")]
        public HttpResponseMessage BieuDoKPI([FromUri] string _date)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                List<object> iData = new List<object>();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime to = from.AddMonths(3).AddSeconds(-1);

                    string[] a = _date.Split('/');
                    int quy = Convert.ToInt32(a[0].ToString());
                    int nam = Convert.ToInt32(a[1].ToString());

                    if (quy == 0)
                    {
                        from = new DateTime(nam, 1, 1);
                        to = from.AddYears(1).AddSeconds(-1);
                    }
                    else
                    {
                        switch (quy)
                        {
                            case 1:
                                from = new DateTime(nam, 1, 1);
                                to = from.AddMonths(3).AddSeconds(-1);
                                break;
                            case 2:
                                from = new DateTime(nam, 4, 1);
                                to = from.AddMonths(3).AddSeconds(-1);
                                break;
                            case 3:
                                from = new DateTime(nam, 7, 1);
                                to = from.AddMonths(3).AddSeconds(-1);
                                break;
                            default:
                                from = new DateTime(nam, 10, 1);
                                to = from.AddMonths(3).AddSeconds(-1);
                                break;
                        }
                    }
                    BaoCaoCommonDAL bc = new BaoCaoCommonDAL();
                    DataSet dsBieuDo = bc.BieuDoChiTieuKPIViengTham(userinfo.ID_QLLH, from, to);

                    if (dsBieuDo.Tables.Count > 0)
                    {
                        //Chỉ tiêu viếng thăm
                        DataTable dtViengTham = dsBieuDo.Tables[0];

                        List<string> label_ViengTham = new List<string>();
                        List<float> data_KeHoachViengTham = new List<float>();
                        List<float> data_ThucTeViengTham = new List<float>();


                        foreach (DataRow dr in dtViengTham.Rows)
                        {
                            float keHoach = Convert.ToSingle(dr["KeHoachViengTham"].ToString());
                            float thucTe = Convert.ToSingle(dr["ThucTeViengTham"].ToString());

                            label_ViengTham.Add(dr["Thang"].ToString());

                            data_KeHoachViengTham.Add(keHoach);
                            data_ThucTeViengTham.Add(thucTe);
                        }

                        iData.Add(label_ViengTham);
                        iData.Add(data_KeHoachViengTham);
                        iData.Add(data_ThucTeViengTham);

                        //Chỉ tiêu doanh số
                        DataTable dtDoanhSo = dsBieuDo.Tables[1];

                        List<string> label_DoanhSo = new List<string>();
                        List<double> data_KeHoachDoanhSo = new List<double>();
                        List<double> data_ThucTeDoanhSo = new List<double>();

                        foreach (DataRow dr in dtDoanhSo.Rows)
                        {
                            double keHoach = Convert.ToDouble(dr["KeHoachDoanhSo"].ToString());
                            double thucTe = Convert.ToDouble(dr["ThucTeDoanhSo"].ToString());

                            label_DoanhSo.Add(dr["Thang"].ToString());

                            data_KeHoachDoanhSo.Add(keHoach);
                            data_ThucTeDoanhSo.Add(thucTe);
                        }

                        iData.Add(label_DoanhSo);
                        iData.Add(data_KeHoachDoanhSo);
                        iData.Add(data_ThucTeDoanhSo);

                        //Chỉ tiêu đơn hàng
                        DataTable dtDonHang = dsBieuDo.Tables[2];

                        List<string> label_DonHang = new List<string>();
                        List<float> data_KeHoachDonHang = new List<float>();
                        List<float> data_ThucTeDonHang = new List<float>();

                        foreach (DataRow dr in dtDonHang.Rows)
                        {
                            float keHoach = Convert.ToSingle(dr["KeHoachDonHang"].ToString());
                            float thucTe = Convert.ToSingle(dr["ThucTeDonHang"].ToString());

                            label_DonHang.Add(dr["Thang"].ToString());

                            data_KeHoachDonHang.Add(keHoach);
                            data_ThucTeDonHang.Add(thucTe);
                        }

                        iData.Add(label_DonHang);
                        iData.Add(data_KeHoachDonHang);
                        iData.Add(data_ThucTeDonHang);

                        //Chỉ tiêu ngày công
                        DataTable dtNgayCong = dsBieuDo.Tables[3];

                        List<string> label_NgayCong = new List<string>();
                        List<float> data_KeHoachNgayCong = new List<float>();
                        List<float> data_ThucTeNgayCong = new List<float>();

                        foreach (DataRow dr in dtNgayCong.Rows)
                        {
                            float keHoach = Convert.ToSingle(dr["KeHoachNgayCong"].ToString());
                            float thucTe = Convert.ToSingle(dr["ThucTeNgayCong"].ToString());

                            label_NgayCong.Add(dr["Thang"].ToString());

                            data_KeHoachNgayCong.Add(keHoach);
                            data_ThucTeNgayCong.Add(thucTe);
                        }

                        iData.Add(label_NgayCong);
                        iData.Add(data_KeHoachNgayCong);
                        iData.Add(data_ThucTeNgayCong);
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

        //BIỂU ĐỒ KPI CÔNG VIỆC
        #region BIỂU ĐỒ KPI CÔNG VIỆC

        //BÁO CÁO KPI CÔNG VIỆC CHI TIẾT
        [HttpGet]
        [Route("BieuDoChiTieuKPICongViec")]
        public HttpResponseMessage BieuDoChiTieuKPICongViec([FromUri]  int id_Nhom, DateTime fromdate, DateTime todate)
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

        //BIỂU ĐỒ KPI CÔNG VIỆC
        [HttpGet]
        [Route("BieuDoKPICongViec")]
        public HttpResponseMessage BieuDoKPICongViec([FromUri] int id_Nhom, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                List<object> iData = new List<object>();
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
                        DataTable dtBieuDo = ds.Tables[0];
                        List<string> labels = new List<string>();

                        List<float> lst_dataItem1 = new List<float>();
                        List<float> lst_dataItem2 = new List<float>();

                        foreach (DataRow dr in dtBieuDo.Rows)
                        {
                            float a = Convert.ToSingle(dr["Tong"].ToString());
                            float b = Convert.ToSingle(dr["HoanThanh"].ToString());

                            labels.Add(dr["TenNhanVien"].ToString());

                            lst_dataItem1.Add(a);
                            lst_dataItem2.Add(b);
                        }

                        iData.Add(labels);

                        iData.Add(lst_dataItem1);
                        iData.Add(lst_dataItem2);

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
        #endregion

        //BÁO CÁO TỔNG HỢP THỜI GIAN ĐĂNG NHẬP - ĐĂNG XUẤT
        [HttpGet]
        [Route("baocaotonghopravaodiem")]
        public HttpResponseMessage baocaotonghopravaodiem([FromUri]int id_Nhom, int id_NhanVien, DateTime to, DateTime from)
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

        //KẾ HOẠCH LỊCH HẸN
        [HttpGet]
        [Route("BaoCaoLichHen")]
        public HttpResponseMessage BaoCaoLichHen([FromUri]int ID_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        // Báo cáo viếng thăm khách hàng theo tuyến
        #region  Báo cáo viếng thăm khách hàng theo tuyến
        [HttpGet]
        [Route("baoCaoViengThamKhachHangTheoTuyenChiTiet")]
        public HttpResponseMessage baoCaoViengThamKhachHangTheoThuyenChiTiet([FromUri]int id_NhanVien, DateTime fromdate, DateTime todate)
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
        [Route("baoCaoViengThamKhachHangTheoTuyenChiTiet_detail")]
        public HttpResponseMessage baoCaoViengThamKhachHangTheoThuyenChiTiet_detail([FromUri]int idtuyen, int idnhanvien, DateTime day)
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
                    DataSet ds = bc.BaoCaoChiTietCheckInTuyen(idtuyen, idnhanvien, day);
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
        [Route("baoCaoViengThamKhachHangTheoTuyenSoLuong")]
        public HttpResponseMessage baoCaoViengThamKhachHangTheoTuyenSoLuong([FromUri]int id_NhanVien, DateTime fromdate, DateTime todate)
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

        #endregion

        #region BÁO CÁO CÔNG NỢ
        //BÁO CÁO TỔNG HỢP THEO KHÁCH HÀNG
        [HttpGet]
        [Route("BaoCaoCongNo")]
        public HttpResponseMessage BaoCaoCongNo([FromUri] int ID_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //THU HỒI CÔNG NỢ
        [HttpGet]
        [Route("BaoCaoThuHoiCongNo")]
        public HttpResponseMessage BaoCaoThuHoiCongNo([FromUri] int ID_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
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

        //THU HỒI CÔNG NỢ CHI TIẾT
        [HttpGet]
        [Route("BaoCaoThuHoiCongNo_ChiTiet")]
        public HttpResponseMessage BaoCaoThuHoiCongNo_ChiTiet([FromUri] int ID_KhachHang, int id_NhanVien, DateTime fromdate, DateTime todate)
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
                    DataSet ds = bc_dl.BaoCaoThuHoiCongNo_ChiTiet(userinfo.ID_QLLH, 0, ID_KhachHang, id_NhanVien, fromdate, todate);
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


        #endregion

        #region BÁO CÁO KHO HÀNG
        // BÁO CÁO XUẤT NHẬP TỒN

        [HttpGet]
        [Route("BaoCaoTongHopNhapXuatTon")]
        public HttpResponseMessage BaoCaoTongHopNhapXuatTon([FromUri] int id_KhoHang, int id_MatHang, DateTime fromdate, DateTime todate, int id_Loai)
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


        // BÁO CÁO XUẤT NHẬP TỒN CHI TIẾT
        [HttpGet]
        [Route("BaoCaoTongHopNhapXuatTon_ChiTiet")]
        public HttpResponseMessage BaoCaoTongHopNhapXuatTon_ChiTiet([FromUri] int id_KhoHang, int id_MatHang, DateTime fromdate, DateTime todate, int id_LoaiBienDong, int id_Loai)
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


        //BÁO CÁO TỔNG HỢP TỒN CÁC MẶT HÀNG CÁC KHO
        [HttpGet]
        [Route("BaoCaoTongHopNhapXuatTonCacKho")]
        public HttpResponseMessage BaoCaoTongHopNhapXuatTonCacKho([FromUri] int id_KhoHang, int id_MatHang, DateTime fromdate, DateTime todate, int id_Loai)
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


        #endregion

    }
}
