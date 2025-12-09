using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LSPos_Data.Models;
using System.Data;
using System.Data.SqlClient;

namespace LSPos_Data.Data
{


    public class BieuDoDanhThuDAL
    {
        private SqlDataHelper helper;

        public BieuDoDanhThuDAL()
        {
            helper = new SqlDataHelper();
        }

        public DataSet BaoCaoBieuDoDoanhThuTheoKhachHang(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OrderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OrderBy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenDoanhThuTheoKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BieuDoTopTenTheoNhanVien(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OrderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OrderBy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenTheoNhanVien", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BieuDoDoanhThuTheoNhomNhanVien(int id_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoDoanhThuTheoNhomNhanVien", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        
        
        public DataSet ChietKhauDoanhThuTheoNhomNhanVien(int id_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_ChietKhauDoanhThuTheoNhomNhanVien", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BieuDoDoanhThuTheoKhuVuc(int id_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoDoanhThuTheoKhuVuc", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoBanHang_KhachHang(int IDQLLH, int IDKH, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_KhachHang", IDKH),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHang_KhachHang", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BieuDoTyTrongKhachHang(int id_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTyTrongKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
    }
}
