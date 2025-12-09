using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LSPos_Data.Models;
using System.Data;
using System.Data.SqlClient;
using Kendo.DynamicLinq;
using BusinessLayer.Model.Reports;
using BusinessLayer.Model.API;
using Model.StoredProcedure;

namespace LSPos_Data.Data
{
    public class BaoCaoCommon
    {
        private SqlDataHelper helper;

        public BaoCaoCommon()
        {
            helper = new SqlDataHelper();
        }

        public List<RptRevenueSummary> TongHopDoanhThuCacDichVu(int ID_QLLH, int ID_QuanLy, DateTime date, string SiteCode)
        {
            List<RptRevenueSummary> rs = new List<RptRevenueSummary>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@Date", date),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_TongHopDoanhThuCacDichVu_GetBySite", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    RptRevenueSummary item = GetObjectFromDataRowUtil<RptRevenueSummary>.ToOject(dr);
                    rs.Add(item);
                }
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return rs;
        }

        public List<RevenueTotalByCashierRes> TongHopDoanhThuTheoCa(int ID_QLLH, int ID_QuanLy, DateTime date, string SiteCode)
        {
            List<RevenueTotalByCashierRes> rs = new List<RevenueTotalByCashierRes>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@Date", date),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_TongHopDoanhThuTheoCa_GetBySite", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    RevenueTotalByCashierRes item = GetObjectFromDataRowUtil<RevenueTotalByCashierRes>.ToOject(dr);
                    rs.Add(item);
                }
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return rs;
        }

        public List<RevenueTotalByCashierRes> TongHopDoanhThuTheoCaForTA(int ID_QLLH, int ID_QuanLy, DateTime date, string SiteCode)
        {
            List<RevenueTotalByCashierRes> rs = new List<RevenueTotalByCashierRes>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@Date", date),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_TongHopDoanhThuTheoCaForTA_GetBySite", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    RevenueTotalByCashierRes item = GetObjectFromDataRowUtil<RevenueTotalByCashierRes>.ToOject(dr);
                    rs.Add(item);
                }
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return rs;
        }

        public List<RptRevenueSummaryByShift> TongHopThanhToanThuNgan(int ID_QLLH, int ID_QuanLy, DateTime date, string SiteCode)
        {
            List<RptRevenueSummaryByShift> rs = new List<RptRevenueSummaryByShift>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@Date", date),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_TongHopThanhToanThuNgan_GetBySite", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    RptRevenueSummaryByShift item = GetObjectFromDataRowUtil<RptRevenueSummaryByShift>.ToOject(dr);
                    rs.Add(item);
                }
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return rs;
        }

        public List<RptRevenueSummaryByPaymentType> TongHopHTTT(int ID_QLLH, int ID_QuanLy, DateTime date, string SiteCode)
        {
            List<RptRevenueSummaryByPaymentType> rs = new List<RptRevenueSummaryByPaymentType>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@Date", date),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_TongHopHTTT_GetBySite", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    RptRevenueSummaryByPaymentType item = GetObjectFromDataRowUtil<RptRevenueSummaryByPaymentType>.ToOject(dr);
                    rs.Add(item);
                }
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return rs;
        }

        public DataTable TongHopDVTheoBooking(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, string SiteCode)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_TongHopDonHangDichVu", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public DataTable TongHopDVTheoBooking2(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, string SiteCode)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_TongHopDonHangDichVu2", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public DataTable checkusingticket(string SiteCode, string BookingCode, string ServiceID)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@BookingCode", BookingCode),
                    new SqlParameter("@ServiceID", ServiceID),
                    new SqlParameter("@SiteCode", SiteCode)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_CheckStatusTicket", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public DataTable DoiSoatDVTheoBooking(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, string SiteCode)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),
                    new SqlParameter("@SiteCode", SiteCode),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_DoiSoatDichVu", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public DataTable LichSuDungViLspay(int ID_NhomTaiKhoan, DateTime from, DateTime to)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_NhomTaiKhoan", ID_NhomTaiKhoan),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_LichSuDungViLspay", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }
        
        public DataTable DoiSoatOnepay(DateTime from, DateTime to)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_DoiSoatOnepay", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public DataTable SoLuongVeCacKhuBan(int ID_DanhMuc, DateTime from, DateTime to)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_DanhMuc", ID_DanhMuc),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCaoTongHop_SoLuongVeCacKhuBan", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public DataTable TyLePhanBoGiaVe(int ID_DanhMuc)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_DanhMuc", ID_DanhMuc)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCaoTongHop_TyLePhanBoGiaVe", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public List<BaocaoDTO> BaocaoDoanhThu(int IDQLLH, int idnv, int idkh, int id_quanly, int startRecord, int maxRecords, FilterBaocao filter
            , ref int TongSo
            , ref double soDonHang, ref double daHoanTat, ref double chuaHoanTat, ref double slHuy, ref double tongTienChuaChietKhau, ref double tongTienChietKhau
            , ref double tongTien, ref double tienDaThanhToan)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH),
                new SqlParameter("ID_NhanVien", idnv),
                new SqlParameter("ID_KhachHang", idkh),
                new SqlParameter("dtFrom", filter.Fromdate),
                new SqlParameter("dtTo", filter.Todate),
                new SqlParameter("ID_QuanLy", id_quanly),
                new SqlParameter("startRecord", startRecord),
                new SqlParameter("maxRecords", maxRecords),
                //  new SqlParameter("CreateDate", filter.CreateDate),
                new SqlParameter("SoDonHang", filter.SoDonHang),
                new SqlParameter("DaHoanTat", filter.DaHoanTat),
                new SqlParameter("ChuaHoanTat", filter.ChuaHoanTat),
                new SqlParameter("SLHuy", filter.SLHuy),
                new SqlParameter("TongTienChuaChietKhau", filter.TongTienChuaChietKhau),
                new SqlParameter("TongTienChietKhau", filter.TongTienChietKhau),
                new SqlParameter("TongTien", filter.TongTien),
                new SqlParameter("TienDaThanhToan", filter.TienDaThanhToan),

                };
                DataSet ds = helper.ExecuteDataSet("sp_web_BaoCaoDoanhThu_KenDo", pars);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                DataTable dt3 = ds.Tables[2];
                if (dt2.Rows.Count > 0)
                {
                    TongSo = int.Parse(dt2.Rows[0]["soluong"].ToString());
                }
                else
                {
                    TongSo = 0;
                }

                if (dt3.Rows.Count > 0)
                {
                    soDonHang = double.Parse(dt3.Rows[0]["soDonHang"].ToString());
                    daHoanTat = double.Parse(dt3.Rows[0]["daHoanTat"].ToString());
                    chuaHoanTat = double.Parse(dt3.Rows[0]["chuaHoanTat"].ToString());
                    slHuy = double.Parse(dt3.Rows[0]["slHuy"].ToString());
                    tongTienChuaChietKhau = double.Parse(dt3.Rows[0]["tongTienChuaChietKhau"].ToString());
                    tongTienChietKhau = double.Parse(dt3.Rows[0]["tongTienChietKhau"].ToString());
                    tongTien = double.Parse(dt3.Rows[0]["tongTien"].ToString());
                    tienDaThanhToan = double.Parse(dt3.Rows[0]["tienDaThanhToan"].ToString());
                }
                else
                {
                    soDonHang = 0;
                    daHoanTat = 0;
                    chuaHoanTat = 0;
                    slHuy = 0;
                    tongTienChuaChietKhau = 0;
                    tongTienChietKhau = 0;
                    tongTien = 0;
                    tienDaThanhToan = 0;
                }
                List<BaocaoDTO> listBaocao = new List<BaocaoDTO>();

                foreach (DataRow row in dt.Rows)
                {
                    BaocaoDTO baocao = new BaocaoDTO();
                    if (row["CreateDate"].ToString() != "")
                    {
                        baocao.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                    }
                    baocao.SoDonHang = int.Parse(row["SoDonHang"].ToString());
                    baocao.DaHoanTat = int.Parse(row["DaHoanTat"].ToString());
                    baocao.ChuaHoanTat = int.Parse(row["ChuaHoanTat"].ToString());
                    baocao.SLHuy = int.Parse(row["SLHuy"].ToString());
                    baocao.TongTienChuaChietKhau = row["TongTienChuaChietKhau"].ToString() != "" ? double.Parse(row["TongTienChuaChietKhau"].ToString()) : 0;
                    baocao.TongTienChietKhau = row["TongTienChietKhau"].ToString() != "" ? double.Parse(row["TongTienChietKhau"].ToString()) : 0;
                    baocao.TongTien = row["TongTien"].ToString() != "" ? double.Parse(row["TongTien"].ToString()) : 0;
                    baocao.TienDaThanhToan = row["TienDaThanhToan"].ToString() != "" ? double.Parse(row["TienDaThanhToan"].ToString()) : 0;
                    listBaocao.Add(baocao);

                }
                return listBaocao;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public DataSet BaoCaoDoanhThu_Report(int IDQLLH, int ID_NhanVien, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoDoanhThu_KenDo_Report", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ds;
        }

        public List<BaocaoDTO> BaoCaoDoanhThu_New(int IDQLLH, int ID_NhanVien, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            List<BaocaoDTO> lstBaoCao = new List<BaocaoDTO>();
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoDoanhThu_KenDo_Report", pars);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        BaocaoDTO baocao = new BaocaoDTO();
                        if (row["CreateDate"].ToString() != "")
                        {
                            baocao.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        baocao.SoDonHang = int.Parse(row["SoDonHang"].ToString());
                        baocao.DaHoanTat = int.Parse(row["DaHoanTat"].ToString());
                        baocao.ChuaHoanTat = int.Parse(row["ChuaHoanTat"].ToString());
                        baocao.SLHuy = int.Parse(row["SLHuy"].ToString());
                        baocao.TongTienChuaChietKhau = row["TongTienChuaChietKhau"].ToString() != "" ? double.Parse(row["TongTienChuaChietKhau"].ToString()) : 0;
                        baocao.TongTienChietKhau = row["TongTienChietKhau"].ToString() != "" ? double.Parse(row["TongTienChietKhau"].ToString()) : 0;
                        baocao.TongTien = row["TongTien"].ToString() != "" ? double.Parse(row["TongTien"].ToString()) : 0;
                        baocao.TienDaThanhToan = row["TienDaThanhToan"].ToString() != "" ? double.Parse(row["TienDaThanhToan"].ToString()) : 0;
                        lstBaoCao.Add(baocao);

                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;

            }
            return lstBaoCao;
        }


        public List<BaocaoDonHangDTO> BaoCaoDonHangTongQuan(int IDQLLH, int id_quanly, DateTime from, DateTime to, int startRecord, int maxRecords, FilterTongQuanBaocao filter, ref int TongSo
            , ref double TongSoDon, ref double DaHoanTat, ref double ChuaHoanTat, ref double Huy, ref double ChuaThanhToan, ref double ThanhToan1Phan
            , ref double DaThanhToan, ref double ChuaGiaoHang, ref double GiaoHang1Phan, ref double DaGiaoHang, ref double TongTien, ref double TienDaThanhToan, ref double ConLai)

        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", id_quanly),
            new SqlParameter("from", from),
            new SqlParameter("to", to),
            new SqlParameter("startRecord", startRecord),
            new SqlParameter("maxRecords", maxRecords),
          //  new SqlParameter("CreateDate", filter.CreateDate),
            new SqlParameter("TongSoDon", filter.TongSoDon),
            new SqlParameter("DaHoanTat", filter.DaHoanTat),
            new SqlParameter("ChuaHoanTat", filter.ChuaHoanTat),
            new SqlParameter("Huy", filter.Huy),
            new SqlParameter("ChuaThanhToan", filter.ChuaThanhToan),
            new SqlParameter("ThanhToan1Phan", filter.ThanhToan1Phan),
            new SqlParameter("DaThanhToan", filter.DaThanhToan),
            new SqlParameter("ChuaGiaoHang", filter.ChuaGiaoHang),
            new SqlParameter("GiaoHang1Phan", filter.GiaoHang1Phan),
            new SqlParameter("DaGiaoHang", filter.DaGiaoHang),
            new SqlParameter("TongTien", filter.TongTien),
            new SqlParameter("TienDaThanhToan", filter.TienDaThanhToan),
            new SqlParameter("ConLai", filter.ConLai)

    };
                DataSet ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTongQuan_Kendo", pars);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                DataTable dt3 = ds.Tables[2];
                if (dt2.Rows.Count > 0)
                {
                    TongSo = int.Parse(dt2.Rows[0]["soluong"].ToString());
                }
                else
                {
                    TongSo = 0;
                }
                if (dt3.Rows.Count > 0)
                {
                    TongSoDon = double.Parse(dt3.Rows[0]["TongSoDon"].ToString());
                    DaHoanTat = double.Parse(dt3.Rows[0]["DaHoanTat"].ToString());
                    ChuaHoanTat = double.Parse(dt3.Rows[0]["ChuaHoanTat"].ToString());
                    Huy = double.Parse(dt3.Rows[0]["Huy"].ToString());
                    ChuaThanhToan = double.Parse(dt3.Rows[0]["ChuaThanhToan"].ToString());
                    ThanhToan1Phan = double.Parse(dt3.Rows[0]["ThanhToan1Phan"].ToString());
                    DaThanhToan = double.Parse(dt3.Rows[0]["DaThanhToan"].ToString());
                    ChuaGiaoHang = double.Parse(dt3.Rows[0]["ChuaGiaoHang"].ToString());
                    GiaoHang1Phan = double.Parse(dt3.Rows[0]["GiaoHang1Phan"].ToString());
                    DaGiaoHang = double.Parse(dt3.Rows[0]["DaGiaoHang"].ToString());
                    TongTien = double.Parse(dt3.Rows[0]["TongTien"].ToString());
                    TienDaThanhToan = double.Parse(dt3.Rows[0]["TienDaThanhToan"].ToString());
                    ConLai = double.Parse(dt3.Rows[0]["ConLai"].ToString());
                }
                else
                {
                    TongSoDon = 0;
                    DaHoanTat = 0;
                    ChuaHoanTat = 0;
                    Huy = 0;
                    ChuaThanhToan = 0;
                    ThanhToan1Phan = 0;
                    DaThanhToan = 0;
                    ChuaGiaoHang = 0;
                    GiaoHang1Phan = 0;
                    DaGiaoHang = 0;
                    TongTien = 0;
                    TienDaThanhToan = 0;
                    ConLai = 0;
                }

                List<BaocaoDonHangDTO> listBaocao = new List<BaocaoDonHangDTO>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BaocaoDonHangDTO baocao = new BaocaoDonHangDTO();
                    baocao.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                    baocao.TongSoDon = int.Parse(row["TongSoDon"].ToString());
                    baocao.DaHoanTat = int.Parse(row["DaHoanTat"].ToString());
                    baocao.ChuaHoanTat = int.Parse(row["ChuaHoanTat"].ToString());
                    baocao.Huy = int.Parse(row["Huy"].ToString());
                    baocao.ChuaThanhToan = int.Parse(row["ChuaThanhToan"].ToString());
                    baocao.ThanhToan1Phan = int.Parse(row["ThanhToan1Phan"].ToString());
                    baocao.DaThanhToan = int.Parse(row["DaThanhToan"].ToString());
                    baocao.ChuaGiaoHang = int.Parse(row["ChuaGiaoHang"].ToString());
                    baocao.GiaoHang1Phan = int.Parse(row["GiaoHang1Phan"].ToString());
                    baocao.DaGiaoHang = int.Parse(row["DaGiaoHang"].ToString());
                    baocao.TongTien = row["TongTien"].ToString() != "" ? double.Parse(row["TongTien"].ToString()) : 0;
                    baocao.TienDaThanhToan = row["TienDaThanhToan"].ToString() != "" ? double.Parse(row["TienDaThanhToan"].ToString()) : 0;
                    baocao.ConLai = row["ConLai"].ToString() != "" ? double.Parse(row["ConLai"].ToString()) : 0;
                    listBaocao.Add(baocao);
                }
                return listBaocao;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataSet BaoCaoDonHangTongQuan_Report(int IDQLLH, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("from", dtFrom),
            new SqlParameter("to", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTongQuan_Kendo_Report", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }


        public DataSet GetCheckInById_New(int IdCheckIn, int ID_QuanLy)
        {
            DataSet ds = null;
            
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("id", IdCheckIn),
                    new SqlParameter("ID_QuanLy", ID_QuanLy)
                    };
                ds = helper.ExecuteDataSet("sp_QL_ExportBaoCaoVaoRaDiem_ById_new_v1", pars);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }


            return ds;
        }

        public DataSet GetCheckInTheoIDNV_New(int IDQLLH, int ID_Nhom, int IDNV, DateTime TuNgay, DateTime DenNgay, int ID_QuanLy, int ID_KhachHang, int type, int ID_LoaiKhachHang)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Nhom", ID_Nhom),
                new SqlParameter("ID_QLLH", IDQLLH),
                new SqlParameter("ID_NhanVien", IDNV),
                new SqlParameter("TuNgay", TuNgay),
                new SqlParameter("DenNgay", DenNgay),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
                new SqlParameter("Loai", type),// : check in , 1 : check out,
                new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang)
        };
                ds = helper.ExecuteDataSet("sp_QL_ExportBaoCaoVaoRaDiem_v1", pars);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }
        public DataTable getchecklistbykhachhang(int ID_QLLH, int idkhachhang, int idcheckin)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@idkhachhang", idkhachhang),
                    new SqlParameter("@idcheckin", idcheckin)
                    };

                DataSet ds = helper.ExecuteDataSet("VuongTM_CheckList_CheckIn_Getlist", pars);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public DataTable GetInfoCompany(int ID_QLLH, int ID_QuanLy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy)
                    };

                DataSet ds = helper.ExecuteDataSet("Business_Dongnn_GetInfoCompany_v1", pars);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public string GetCurrentLanguages(int ID_QLLH, int ID_QuanLy)
        {
            string lang = "vi";
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy)
                    };

                DataSet ds = helper.ExecuteDataSet("Business_VuongTM_GetCurrentLanguages", pars);
                if (ds.Tables.Count > 0)
                {
                    lang = ds.Tables[0].Rows[0]["lang"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return lang;
        }
        public List<BaocaoDonHangDTO> BaoCaoTongHopDonHang(int IDQLLH, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            List<BaocaoDonHangDTO> listBaocao = new List<BaocaoDonHangDTO>();
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("from", dtFrom),
            new SqlParameter("to", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTongQuan_Kendo_Report", pars);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        BaocaoDonHangDTO baocao = new BaocaoDonHangDTO();
                        baocao.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        baocao.TongSoDon = int.Parse(row["TongSoDon"].ToString());
                        baocao.DaHoanTat = int.Parse(row["DaHoanTat"].ToString());
                        baocao.ChuaHoanTat = int.Parse(row["ChuaHoanTat"].ToString());
                        baocao.Huy = int.Parse(row["Huy"].ToString());
                        baocao.ChuaThanhToan = int.Parse(row["ChuaThanhToan"].ToString());
                        baocao.ThanhToan1Phan = int.Parse(row["ThanhToan1Phan"].ToString());
                        baocao.DaThanhToan = int.Parse(row["DaThanhToan"].ToString());
                        baocao.ChuaGiaoHang = int.Parse(row["ChuaGiaoHang"].ToString());
                        baocao.GiaoHang1Phan = int.Parse(row["GiaoHang1Phan"].ToString());
                        baocao.DaGiaoHang = int.Parse(row["DaGiaoHang"].ToString());
                        baocao.TongTien = row["TongTien"].ToString() != "" ? double.Parse(row["TongTien"].ToString()) : 0;
                        baocao.TienDaThanhToan = row["TienDaThanhToan"].ToString() != "" ? double.Parse(row["TienDaThanhToan"].ToString()) : 0;
                        baocao.ConLai = row["ConLai"].ToString() != "" ? double.Parse(row["ConLai"].ToString()) : 0;
                        listBaocao.Add(baocao);
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return listBaocao;
        }

        public List<BaoCaoDonHangNhanVienDTO> BaoCaoDonHangTheoNhanVien(int IDQLLH, int idnv, DateTime from, DateTime to, int id_quanly, int startRecord, int maxRecords, FilterDonHangNV filter
            , ref int TongSo
            , ref double TongDonHang, ref double DonThanhCong, ref double TongTienChuaChietKhau, ref double TongTienChietKhau, ref double TongTien, ref double DaThanhToan)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", idnv),
            new SqlParameter("dtFrom", from),
            new SqlParameter("dtTo", to),
            new SqlParameter("ID_QuanLy", id_quanly),
            new SqlParameter("startRecord", startRecord),
            new SqlParameter("maxRecords", maxRecords),
            new SqlParameter("TenNhanVien", filter.TenNhanVien),
            new SqlParameter("TongDonHang", filter.TongDonHang),
            new SqlParameter("DonThanhCong", filter.DonThanhCong),
            new SqlParameter("TongTienChuaChietKhau", filter.TongTienChuaChietKhau),
            new SqlParameter("TongTienChietKhau", filter.TongTienChietKhau),
            new SqlParameter("TongTien", filter.TongTien),
            new SqlParameter("DaThanhToan", filter.DaThanhToan),

    };
                DataSet ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHang_Kendo", pars);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                DataTable dt3 = ds.Tables[2];
                if (dt2.Rows.Count > 0)
                {
                    TongSo = int.Parse(dt2.Rows[0]["soluong"].ToString());
                }
                else
                {
                    TongSo = 0;
                }

                if (dt3.Rows.Count > 0)
                {
                    TongDonHang = double.Parse(dt3.Rows[0]["TongDonHang"].ToString());
                    DonThanhCong = double.Parse(dt3.Rows[0]["DonThanhCong"].ToString());
                    TongTienChuaChietKhau = double.Parse(dt3.Rows[0]["TongTienChuaChietKhau"].ToString());
                    TongTienChietKhau = double.Parse(dt3.Rows[0]["TongTienChietKhau"].ToString());
                    TongTien = double.Parse(dt3.Rows[0]["TongTien"].ToString());
                    DaThanhToan = double.Parse(dt3.Rows[0]["DaThanhToan"].ToString());
                }
                else
                {
                    TongDonHang = 0;
                    DonThanhCong = 0;
                    TongTienChuaChietKhau = 0;
                    TongTienChietKhau = 0;
                    TongTien = 0;
                    DaThanhToan = 0;
                }

                List<BaoCaoDonHangNhanVienDTO> listBaocao = new List<BaoCaoDonHangNhanVienDTO>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BaoCaoDonHangNhanVienDTO baocao = new BaoCaoDonHangNhanVienDTO();
                    baocao.TenNhanVien = (row["TenNhanVien"].ToString());
                    baocao.TongDonHang = int.Parse(row["TongDonHang"].ToString());
                    baocao.DonThanhCong = int.Parse(row["DonThanhCong"].ToString());
                    baocao.TongTienChuaChietKhau = row["TongTienChuaChietKhau"].ToString() != "" ? double.Parse(row["TongTienChuaChietKhau"].ToString()) : 0;
                    baocao.TongTienChietKhau = row["TongTienChietKhau"].ToString() != "" ? double.Parse(row["TongTienChietKhau"].ToString()) : 0;
                    baocao.TongTien = row["TongTien"].ToString() != "" ? double.Parse(row["TongTien"].ToString()) : 0;
                    baocao.DaThanhToan = row["DaThanhToan"].ToString() != "" ? double.Parse(row["DaThanhToan"].ToString()) : 0;
                    listBaocao.Add(baocao);
                }

                return listBaocao;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataSet BaoCaoDonHang(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("dtFrom", dtFrom),
                new SqlParameter("dtTo", dtTo),
                new SqlParameter("ID_QuanLy", ID_QuanLy)
            };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHang_Kendo_Report", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ds;
        }

        public List<BaoCaoDonHangNhanVienDTO> BaoCaoDonHangTheoNhanVienNew(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            List<BaoCaoDonHangNhanVienDTO> listBaocao = new List<BaoCaoDonHangNhanVienDTO>();
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHang_Kendo_Report", pars);
                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        BaoCaoDonHangNhanVienDTO baocao = new BaoCaoDonHangNhanVienDTO();
                        baocao.TenNhanVien = (row["TenNhanVien"].ToString());
                        baocao.TongDonHang = int.Parse(row["TongDonHang"].ToString());
                        baocao.DonThanhCong = int.Parse(row["DonThanhCong"].ToString());
                        baocao.TongTienChuaChietKhau = row["TongTienChuaChietKhau"].ToString() != "" ? double.Parse(row["TongTienChuaChietKhau"].ToString()) : 0;
                        baocao.TongTienChietKhau = row["TongTienChietKhau"].ToString() != "" ? double.Parse(row["TongTienChietKhau"].ToString()) : 0;
                        baocao.TongTien = row["TongTien"].ToString() != "" ? double.Parse(row["TongTien"].ToString()) : 0;
                        baocao.DaThanhToan = row["DaThanhToan"].ToString() != "" ? double.Parse(row["DaThanhToan"].ToString()) : 0;
                        listBaocao.Add(baocao);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return listBaocao;
        }

        public DataSet PhienLamViec_Offline(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy, int idNhom)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("dtFrom", dtFrom),
                new SqlParameter("dtTo", dtTo),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("@ID_Nhom", idNhom)
            };

            try
            {
                ds = helper.ExecuteDataSet("sp_vuongtm_NhanVien_GetPhienLamViec_v2", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoMatKetNoi(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy, int idNhom)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("dtFrom", dtFrom),
                new SqlParameter("dtTo", dtTo),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("@ID_Nhom", idNhom)
            };

            try
            {
                ds = helper.ExecuteDataSet("sp_vuongtm_NhanVien_Getthoigianmatketnoi", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoHoanThanhTheoDonHangCoBanVaThucTe(int id_qllh, int id_KhachHang, int id_NhanVien, int id_MatHang, DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("id_qllh", id_qllh),
                new SqlParameter("id_KhachHang", id_KhachHang),
                new SqlParameter("id_NhanVien", id_NhanVien),
                new SqlParameter("id_MatHang", id_MatHang),
                new SqlParameter("from", from),
                new SqlParameter("to", to)
            };

            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoKPIHoanThanhTheoDonHangCoBanVaThucTe", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoTongHopRaVaoDiem(int id_qllh, int id_Nhom, int id_NhanVien, DateTime to, DateTime from)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("id_qllh", id_qllh),
                new SqlParameter("id_NhanVien", id_NhanVien),
                new SqlParameter("id_Nhom", id_Nhom),
                new SqlParameter("to", to),
                new SqlParameter("from", from)
            };

            try
            {
                ds = helper.ExecuteDataSet("usp_sontq_BaoCaoTongHopRaVaoDiem", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoFakeVaoDiemKhachHang(int ID_QLLH, int idNhanVien, int idKhachHang, DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@idNhanVien", idNhanVien),
                new SqlParameter("@idKhachHang", idKhachHang)
            };

            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoFakeVaoDiemKhachHang", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataSet GetListTinNhan(int ID_QLLH, int ID_QuanLy, int ID_NHANVIEN, int TypeSend, DateTime from, DateTime to, int chuadoc = 0)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_NHANVIEN", ID_NHANVIEN),
                new SqlParameter("@dtFrom", from),
                new SqlParameter("@dtTo", to),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@TypeSend", TypeSend),
                new SqlParameter("@chuadoc", chuadoc)
            };

            try
            {
                ds = helper.ExecuteDataSet("vuongtm_tinnhan_getlistbyidnhanvien", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataTable GetKeHoachTheoNhanVien_Moi(int ID_NhanVien, DateTime TuNgay, DateTime DenNgay, int ID_QLLH, int ID_QuanLy)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@TuNgay", TuNgay),
            new SqlParameter("@DenNgay", DenNgay),
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien_New_vuongtm_v3", pars);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                    return dt;
                else
                    return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);

                return null;
            }
        }

        public DataTable GetViTriTatCaNV(int idcty, int ID_QuanLy, int ID_Nhom, int loctrangthai)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idcty", idcty),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@ID_Nhom", ID_Nhom),
                    new SqlParameter("@loctrangthai", loctrangthai)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_QL_getViTriTatCaNV", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public DataTable GetViTriTatCaNVTheoToaDo(int idcty, int ID_QuanLy, float KinhDo, float ViDo, int sombankinh, int trangthai)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idcty", idcty),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@KinhDo", KinhDo),
                    new SqlParameter("@ViDo", ViDo),
                    new SqlParameter("@mbankinh", sombankinh),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_QL_getViTriTatCaNV_TheoToaDo", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public DataTable baocaotonghopchuongtrinhkhuyenmai(int ID_QLLH, int ID_CTKM, int ID_Kho, int ID_Nhom, int ID_Hang)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_QLLH", ID_QLLH),
                    new SqlParameter("ID_CTKM", ID_CTKM),
                    new SqlParameter("ID_Kho", ID_Kho),
                    new SqlParameter("ID_Nhom", ID_Nhom),
                    new SqlParameter("ID_Hang", ID_Hang)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_TRUONGNM_BaoCaoKhuyenMai_TongHop", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public DataTable baocaochitietchuongtrinhkhuyenmai(int ID_QLLH, int ID_CTKM, int ID_Kho, int ID_NhanVien, int ID_Hang)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_QLLH", ID_QLLH),
                    new SqlParameter("ID_CTKM", ID_CTKM),
                    new SqlParameter("ID_Kho", ID_Kho),
                    new SqlParameter("ID_NhanVien", ID_NhanVien),
                    new SqlParameter("ID_Hang", ID_Hang)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_TRUONGNM_BaoCaoKhuyenMai_ChiTiet", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public DataTable BaoCaoLichSuThaoTac(int ID_QLLH, int ID_QuanLy, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int Loai)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@ID_KhachHang", ID_KhachHang),
                    new SqlParameter("@ID_NhanVien", ID_NhanVien),
                    new SqlParameter("dtFrom", dtFrom),
                    new SqlParameter("dtTo", dtTo),
                    new SqlParameter("@Loai", Loai),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_BaoCao_LichSuThaoTac_vuongtm", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        #region Class dùng cho báo cáo
        public class khachhangparam
        {
            public int ID_KhachHang { get; set; }
            public string TenKhachHang { get; set; }
            public int ID_LoaiKhachHang { get; set; }
        }
        public class RequestGridParam
        {
            public DataSourceRequest request { get; set; }
            public TieuChiLoc tieuchiloc { get; set; }
        }
        public class TieuChiLoc
        {
            public int idnv { get; set; }
            public int idkh { get; set; }
            public string fromdate { get; set; }
            public string todate { get; set; }
        }

        public class BaocaoDTO
        {
            public DateTime CreateDate { get; set; }

            public int SoDonHang { get; set; }
            public int DaHoanTat { get; set; }
            public int ChuaHoanTat { get; set; }
            public int SLHuy { get; set; }
            public double TongTienChuaChietKhau { get; set; }
            public double TongTienChietKhau { get; set; }
            public double TongTien { get; set; }
            public double TienDaThanhToan { get; set; }
        }
        public class FilterBaocao
        {
            public DateTime CreateDate { get; set; }
            public DateTime Fromdate { get; set; }
            public DateTime Todate { get; set; }
            public int SoDonHang { get; set; }
            public int DaHoanTat { get; set; }
            public int ChuaHoanTat { get; set; }
            public int SLHuy { get; set; }
            public double TongTienChuaChietKhau { get; set; }
            public double TongTienChietKhau { get; set; }
            public double TongTien { get; set; }
            public double TienDaThanhToan { get; set; }
        }


        public class BaocaoDonHangDTO
        {
            public DateTime CreateDate { get; set; }
            public int TongSoDon { get; set; }
            public int ChuaHoanTat { get; set; }
            public int DaHoanTat { get; set; }
            public int Huy { get; set; }
            public int ChuaThanhToan { get; set; }
            public int ThanhToan1Phan { get; set; }
            public int DaThanhToan { get; set; }
            public int ChuaGiaoHang { get; set; }
            public int GiaoHang1Phan { get; set; }
            public int DaGiaoHang { get; set; }
            public double TongTien { get; set; }
            public double TienDaThanhToan { get; set; }
            public double ConLai { get; set; }
        }
        public class FilterTongQuanBaocao
        {
            public DateTime CreateDate { get; set; }
            public int TongSoDon { get; set; }
            public int ChuaHoanTat { get; set; }
            public int DaHoanTat { get; set; }
            public int Huy { get; set; }
            public int ChuaThanhToan { get; set; }
            public int ThanhToan1Phan { get; set; }
            public int DaThanhToan { get; set; }
            public int ChuaGiaoHang { get; set; }
            public int GiaoHang1Phan { get; set; }
            public int DaGiaoHang { get; set; }
            public double TongTien { get; set; }
            public double TienDaThanhToan { get; set; }
            public double ConLai { get; set; }
        }
        public class BaoCaoDonHangNhanVienDTO
        {
            public string TenNhanVien { get; set; }
            public int TongDonHang { get; set; }
            public int DonThanhCong { get; set; }
            public double TongTienChuaChietKhau { get; set; }
            public double TongTienChietKhau { get; set; }
            public double TongTien { get; set; }
            public double DaThanhToan { get; set; }
        }
        public class FilterDonHangNV
        {
            public string TenNhanVien { get; set; }
            public int TongDonHang { get; set; }
            public int DonThanhCong { get; set; }
            public double TongTienChuaChietKhau { get; set; }
            public double TongTienChietKhau { get; set; }
            public double TongTien { get; set; }
            public double DaThanhToan { get; set; }
        }
        public class Aggregates
        {
            public Aggregates()
            {
                this.soDonHang = new AggregatesValue(0);
                this.daHoanTat = new AggregatesValue(0);
                this.chuaHoanTat = new AggregatesValue(0);
                this.slHuy = new AggregatesValue(0);
                this.tongTienChuaChietKhau = new AggregatesValue(0);
                this.tongTienChietKhau = new AggregatesValue(0);
                this.tongTien = new AggregatesValue(0);
                this.tienDaThanhToan = new AggregatesValue(0);
            }
            public AggregatesValue soDonHang { set; get; }
            public AggregatesValue daHoanTat { set; get; }
            public AggregatesValue chuaHoanTat { set; get; }
            public AggregatesValue slHuy { set; get; }
            public AggregatesValue tongTienChuaChietKhau { set; get; }
            public AggregatesValue tongTienChietKhau { set; get; }
            public AggregatesValue tongTien { set; get; }
            public AggregatesValue tienDaThanhToan { set; get; }
        }

        public class AggregatesValue
        {
            public AggregatesValue(double _sum)
            {
                this.sum = _sum;
            }
            public double sum { set; get; }
        }
        public class AggregatesBaoCaoDonHangTheoNhanVien
        {
            public AggregatesBaoCaoDonHangTheoNhanVien()
            {
                this.tongDonHang = new AggregatesValue(0);
                this.donThanhCong = new AggregatesValue(0);
                this.tongTienChuaChietKhau = new AggregatesValue(0);
                this.tongTienChietKhau = new AggregatesValue(0);
                this.tongTien = new AggregatesValue(0);
                this.daThanhToan = new AggregatesValue(0);
            }
            public AggregatesValue tongDonHang { set; get; }
            public AggregatesValue donThanhCong { set; get; }
            public AggregatesValue tongTienChuaChietKhau { set; get; }
            public AggregatesValue tongTienChietKhau { set; get; }
            public AggregatesValue tongTien { set; get; }
            public AggregatesValue daThanhToan { set; get; }
        }
        public class AggregatesBaoCaoDonHangTongQuan
        {
            public AggregatesBaoCaoDonHangTongQuan()
            {
                this.tongSoDon = new AggregatesValue(0);
                this.daHoanTat = new AggregatesValue(0);
                this.chuaHoanTat = new AggregatesValue(0);
                this.huy = new AggregatesValue(0);
                this.chuaThanhToan = new AggregatesValue(0);
                this.thanhToan1Phan = new AggregatesValue(0);
                this.daThanhToan = new AggregatesValue(0);
                this.chuaGiaoHang = new AggregatesValue(0);
                this.giaoHang1Phan = new AggregatesValue(0);
                this.daGiaoHang = new AggregatesValue(0);
                this.tongTien = new AggregatesValue(0);
                this.tienDaThanhToan = new AggregatesValue(0);
                this.conLai = new AggregatesValue(0);
            }
            public AggregatesValue tongSoDon { set; get; }
            public AggregatesValue daHoanTat { set; get; }
            public AggregatesValue chuaHoanTat { set; get; }
            public AggregatesValue huy { set; get; }
            public AggregatesValue chuaThanhToan { set; get; }
            public AggregatesValue thanhToan1Phan { set; get; }
            public AggregatesValue daThanhToan { set; get; }
            public AggregatesValue chuaGiaoHang { set; get; }
            public AggregatesValue giaoHang1Phan { set; get; }
            public AggregatesValue daGiaoHang { set; get; }
            public AggregatesValue tongTien { set; get; }
            public AggregatesValue tienDaThanhToan { set; get; }
            public AggregatesValue conLai { set; get; }
        }
        #endregion
    }
}