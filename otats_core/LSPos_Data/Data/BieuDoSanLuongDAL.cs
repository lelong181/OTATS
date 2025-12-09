using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LSPos_Data.Models;
using System.Data;
using System.Data.SqlClient;

namespace LSPos_Data.Data
{


    public class BieuDoSanLuongDAL
    {
        private SqlDataHelper helper;

        public BieuDoSanLuongDAL()
        {
            helper = new SqlDataHelper();
        }


        public BieuDoDiChuyenDTO GetTop10DiChuyen(DateTime startdate, DateTime enddate, int id_qllh)
        {
            BieuDoDiChuyenDTO item = new BieuDoDiChuyenDTO();
            string sql = @" declare @table as Table(sokmdichuyen float,TenNhanVien nvarchar(150))
            Insert into @table Select KmDiCHuyen, nv.TenNhanVien from PhienLamViec join NhanVien nv on PhienLamViec.ID_NhanVien = nv.ID_NhanVien ";
            item.title = "từ ngày " + startdate.ToString("dd/MM/yyyy") + " đến ngày " + enddate.AddDays(-1).ToString("dd/MM/yyyy");
            sql += " where ThoiGianBatDau between '" + startdate.ToString("yyyy-MM-dd") + "' and '" + enddate.ToString("yyyy-MM-dd") + "' ";
            sql += @" Select top 10(sum(sokmdichuyen)) as sokmdichuyen, TenNhanVien from @table
            group by TenNhanVien order by sokmdichuyen desc";
            DataSet ds = helper.ExecuteDataSet(sql);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["sokmdichuyen"].ToString() != "0")
                {
                    item.categories.Add(dr["TenNhanVien"].ToString());
                    item.data.Add(double.Parse(dr["sokmdichuyen"].ToString()));
                }
            }
            return item;
        }

        public BieuDoViengThamDTO GetBieuDoTop10ViengTham(DateTime startdate, DateTime enddate, int id_qllh)
        {
            BieuDoViengThamDTO item = new BieuDoViengThamDTO();
            string sql = @"Declare @table as Table(IDCheckIn int,MaNhanVien nvarchar(150))
                                                Insert into @table
                                                Select IDCheckIn,MaNhanVien  from CheckIn"
                 + " Where ID_QLLH = '" + id_qllh + "' AND InsertedTime BETWEEN '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            sql += @" Select top 10 count(*) as countcheckin, nv.TenNhanVien from @table t 
                                                join Nhanvien nv on t.MaNhanVien = nv.ID_NhanVien
                                                group by nv.TenNhanVien
                                                order by count(*) desc";
            item.title = "từ ngày " + startdate.ToString("dd/MM/yyyy") + " đến ngày " + enddate.ToString("dd/MM/yyyy");

            DataSet ds = helper.ExecuteDataSet(sql);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                item.categories.Add(dr["TenNhanVien"].ToString());
                item.data.Add(int.Parse(dr["countcheckin"].ToString()));
            }
            return item;
        }

        public BieuDoViengThamDTO GetBieuDoTop10KHViengTham(DateTime startdate, DateTime enddate, int id_qllh)
        {
            BieuDoViengThamDTO item = new BieuDoViengThamDTO();
            string sql = @"Declare @table as Table(IDCheckIn int,MaKhachHang nvarchar(150))
                                                Insert into @table
                                                Select IDCheckIn,MaKhachHang  from CheckIn"
                + " Where ID_QLLH = '" + id_qllh + "' and  InsertedTime BETWEEN '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            item.title = "từ ngày " + startdate.ToString("dd/MM/yyyy") + " đến ngày " + enddate.ToString("dd/MM/yyyy");
            sql += @" Select top 10 count(*) as countcheckin, kh.TenKhachHang from @table t 
                                                join KhachHang kh on t.MaKhachHang = kh.ID_KhachHang
                                                group by kh.TenKhachHang
                                                order by count(*) desc";
            DataSet ds = helper.ExecuteDataSet(sql);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                item.categories.Add(dr["TenKhachHang"].ToString());
                item.data.Add(int.Parse(dr["countcheckin"].ToString()));
            }
            return item;
        }
       
        public DataSet BaoCaoBieuDoDonHangTheoKhachHang(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OderBy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenDonHangTheoKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoTopTenSanPhamTheoKhachHang(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OrderBy)
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
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenSanPhamTheoKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BieuDoTopTenSanPhamTheoKhachHang_Level2(int id_QLLH, int ID_QuanLy, DateTime TuNgay, DateTime DenNgay, int OrderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OrderBy),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenSanPhamTheoKhachHang_PCHL", Parammeter);
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
        public DataSet BieuDoDonHangTheoKhuVuc(int LoaiKhuVuc, int id_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("LoaiKhuVuc", LoaiKhuVuc),
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoDonHangTheoKhuVuc", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public BieuDo_DonHang_NhomNhanVienDTO GetBieuDo_DonHang_NhomNhanVien(DateTime startdate, DateTime enddate, int id_qllh)
        {
            BieuDo_DonHang_NhomNhanVienDTO item = new BieuDo_DonHang_NhomNhanVienDTO();
            DataSet dsnhom = helper.ExecuteDataSet("Select * from Nhom where ID_QLLH = " + id_qllh);
            DataTable dtnhom = dsnhom.Tables[0];
            foreach (DataRow drnhom in dtnhom.Rows)
            {
                int idnhom = int.Parse(drnhom["ID_Nhom"].ToString());
                string tennhom = drnhom["TenNhom"].ToString();
                string sql = @"Select count(*) from DonHang dh 
                                                            join Nhanvien nv on dh.ID_NhanVien = nv.ID_NhanVien
                                                            where nv.ID_QLLH = " + id_qllh + " and nv.ID_Nhom = " + idnhom
                            + " AND dh.CreateDate BETWEEN '" + startdate.ToString("yyyy-MM-dd") + "' AND '" + enddate.ToString("yyyy-MM-dd 23:59:59") + "'";
                item.title = "từ ngày " + startdate.ToString("dd/MM/yyyy") + " đến ngày " + enddate.ToString("dd/MM/yyyy");
                object count = helper.ExecuteScalar(sql);
                int countdonhang = int.Parse(count.ToString());
                item.data.Add(countdonhang);
                item.categories.Add(tennhom);
            }
            return item;
        }
        public DataSet BieuDoPhanLoaiKhachHang(int id_QLLH)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoPhanLoaiKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoPhanLoaiKhachHangNganhHang(int id_QLLH)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoPhanLoaiKhachHangNganhHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
    }
}
