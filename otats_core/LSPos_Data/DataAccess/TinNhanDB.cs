using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HangHoa
/// </summary>
public class TinNhanDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TinNhanDB));

    public static SqlDataHelper db = new SqlDataHelper();

   public TinNhanDB()
	{
		//
		// TODO: Add constructor logic here
		//
	}
   public static List<TinNhanOBJ> getDSTinNhanTheoID_NHANVIEN(int TypeSend,int ID_QLLH,int ID_QuanLy, int ID_NhanVien, DateTime? dtFrom, DateTime? dtTo)
   {
       try
       {
           List<TinNhanOBJ> lstTinNhan = new List<TinNhanOBJ>();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getDSTinNhanTheoID_NHANVIEN",
                new SqlParameter("@ID_QLLH", ID_QLLH),
               new SqlParameter("@ID_NHANVIEN", ID_NhanVien),
               new SqlParameter("@ID_QuanLy", ID_QuanLy),
               new SqlParameter("@TypeSend", TypeSend),
               (dtFrom != null ? new SqlParameter("@dtFrom", dtFrom.Value.ToString("yyyy-MM-dd")) : new SqlParameter("@dtFrom", DBNull.Value)),
               (dtTo != null ? new SqlParameter("@dtTo", dtTo.Value.ToString("yyyy-MM-dd 23:59:59")) : new SqlParameter("@dtTo", DBNull.Value))
             );

           if (ds == null)
           {
               return null;
           }

           DataTable dtbl = null;
           dtbl = ds.Tables[0];
           for (int i = 0; i < dtbl.Rows.Count; i++)
           {
               DataRow dr = dtbl.Rows[i];

               TinNhanOBJ tinnhan = new TinNhanOBJ();
               tinnhan.ID_NHANVIEN = int.Parse(dr["ID_NHANVIEN"].ToString());
               tinnhan.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
               tinnhan.ID_TINNHAN = int.Parse(dr["ID_TINNHAN"].ToString());
               tinnhan.ID_QUANLY = dr["ID_QUANLY"].ToString() != "" ? int.Parse(dr["ID_QUANLY"].ToString()) : 0;
               tinnhan.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
               tinnhan.TrangThaiHienThi = dr["TrangThai"].ToString() == "1" ? "Đã xem" : "Chưa xem";
               tinnhan.NoiDung = dr["NoiDung"].ToString();
               tinnhan.NgayGui = dr["NgayGui"].ToString() != "" ? DateTime.Parse(dr["NgayGui"].ToString()) : tinnhan.NgayGui;
               tinnhan.NgayXem = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()) : tinnhan.NgayXem;
               tinnhan.NgayXemHienThi = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
               tinnhan.TenQuanLy = dr["TenQuanLy"].ToString();
               tinnhan.TenNhanVien = dr["TenNhanVien"].ToString();
               tinnhan.LoaiGui = dr["TypeSend"].ToString() == "1" ? "Nhân viên gửi" : "Quản lý gửi";
               tinnhan.TypeSend = dr["TypeSend"].ToString() != "" ? int.Parse(dr["TypeSend"].ToString()) : 0;
               lstTinNhan.Add(tinnhan);
           }

           return lstTinNhan;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }
   public static List<TinNhanOBJ> getDSTinNhanTheoID_NHANVIEN(int ID_QLLH, int ID_NhanVien, DateTime? dtFrom, DateTime? dtTo, int ID_QuanLy)
   {
       try
       {
           List<TinNhanOBJ> lstTinNhan = new List<TinNhanOBJ>();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getDSTinNhanTheoID_NHANVIEN",
                new SqlParameter("@ID_QLLH", ID_QLLH),
               new SqlParameter("@ID_NHANVIEN", ID_NhanVien),
               new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@TypeSend", "0"),
               (dtFrom != null ? new SqlParameter("@dtFrom", dtFrom.Value.ToString("yyyy-MM-dd")) : new SqlParameter("@dtFrom", DBNull.Value)),
               (dtTo != null ? new SqlParameter("@dtTo", dtTo.Value.ToString("yyyy-MM-dd 23:59:59")) : new SqlParameter("@dtTo", DBNull.Value))
             );

           if (ds == null)
           {
               return null;
           }

           DataTable dtbl = null;
           dtbl = ds.Tables[0];
           for (int i = 0; i < dtbl.Rows.Count; i++)
           {
               DataRow dr = dtbl.Rows[i];

               TinNhanOBJ tinnhan = new TinNhanOBJ();
               //tinnhan.ID_NHANVIEN = int.Parse(dr["ID_NHANVIEN"].ToString());
               //tinnhan.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
               //tinnhan.ID_TINNHAN = int.Parse(dr["ID_TINNHAN"].ToString());
               //tinnhan.ID_QUANLY = dr["ID_QUANLY"].ToString() != "" ? int.Parse(dr["ID_QUANLY"].ToString()) : 0;
               //tinnhan.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
               //tinnhan.TrangThaiHienThi = dr["TrangThai"].ToString() == "1" ? "Đã xem" : "Chưa xem";
               //tinnhan.NoiDung = dr["NoiDung"].ToString();
               //tinnhan.NgayGui = dr["NgayGui"].ToString() != "" ? DateTime.Parse(dr["NgayGui"].ToString()) : tinnhan.NgayGui;
               //tinnhan.NgayXem = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()) : tinnhan.NgayXem;
               //tinnhan.NgayXemHienThi = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
               //tinnhan.TenQuanLy = dr["TenQuanLy"].ToString();
               //tinnhan.TenNhanVien = dr["TenNhanVien"].ToString();

               tinnhan.ID_NHANVIEN = int.Parse(dr["ID_NHANVIEN"].ToString());
               tinnhan.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
               tinnhan.ID_TINNHAN = int.Parse(dr["ID_TINNHAN"].ToString());
               tinnhan.ID_QUANLY = dr["ID_QUANLY"].ToString() != "" ? int.Parse(dr["ID_QUANLY"].ToString()) : 0;
               tinnhan.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
               tinnhan.TrangThaiHienThi = dr["TrangThai"].ToString() == "1" ? "Đã xem" : "Chưa xem";
               tinnhan.NoiDung = dr["NoiDung"].ToString();
               tinnhan.NgayGui = dr["NgayGui"].ToString() != "" ? DateTime.Parse(dr["NgayGui"].ToString()) : tinnhan.NgayGui;
               tinnhan.NgayXem = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()) : tinnhan.NgayXem;
               tinnhan.NgayXemHienThi = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
               tinnhan.TenQuanLy = dr["TenQuanLy"].ToString();
               tinnhan.TenNhanVien = dr["TenNhanVien"].ToString();
               tinnhan.LoaiGui = dr["TypeSend"].ToString() == "1" ? "Nhân viên gửi" : "Quản lý gửi";
               tinnhan.TypeSend = dr["TypeSend"].ToString() != "" ? int.Parse(dr["TypeSend"].ToString()) : 0;

               lstTinNhan.Add(tinnhan);
           }

           return lstTinNhan;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }

   public static TinNhanOBJ getTinNhanTheoID_TINNHAN(int ID_TINNHAN)
   {
       try
       {
          TinNhanOBJ tinnhan = new TinNhanOBJ();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getTinNhanTheoID_TINNHAN",
               new SqlParameter("@ID_TINNHAN", ID_TINNHAN)
               
             );

           if (ds == null)
           {
               return null;
           }

           DataTable dtbl = null;
           dtbl = ds.Tables[0];
           for (int i = 0; i < dtbl.Rows.Count; i++)
           {
               DataRow dr = dtbl.Rows[i];
               
               
               tinnhan.ID_NHANVIEN = int.Parse(dr["ID_NHANVIEN"].ToString());
               tinnhan.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
               tinnhan.ID_TINNHAN = int.Parse(dr["ID_TINNHAN"].ToString());
               tinnhan.ID_QUANLY = dr["ID_QUANLY"].ToString() != "" ? int.Parse(dr["ID_QUANLY"].ToString()) : 0;
               tinnhan.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
               tinnhan.TrangThaiHienThi = dr["TrangThai"].ToString() == "1" ? "Đã xem" : "Chưa xem";
               tinnhan.NoiDung = dr["NoiDung"].ToString();
               tinnhan.NgayGui = dr["NgayGui"].ToString() != "" ? DateTime.Parse(dr["NgayGui"].ToString()) : tinnhan.NgayGui;
               tinnhan.NgayXem = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()) : tinnhan.NgayXem;
               tinnhan.NgayXemHienThi = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
               tinnhan.TenQuanLy = dr["TenQuanLy"].ToString();
               tinnhan.TenNhanVien = dr["TenNhanVien"].ToString();
               tinnhan.LoaiGui = dr["TypeSend"].ToString() == "1" ? "Nhân viên gửi" : "Quản lý gửi";
               tinnhan.TypeSend = dr["TypeSend"].ToString() != "" ? int.Parse(dr["TypeSend"].ToString()) : 0;
               
           }
           return tinnhan;
            
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }
   public static bool CapNhatTinNhanDaXem(TinNhanOBJ tn)
   {
       try
       {
           SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_TINNHAN", tn.ID_TINNHAN),
                new SqlParameter("@TrangThai", 1),
                new SqlParameter("@NgayXem", DateTime.Now)
                
            };

           return db.ExecuteNonQuery("sp_TinNhan_CapNhatTrangThaiDaXem", param) > 0;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return false;
       }
   }
   public static bool ThemTinNhan(TinNhanOBJ tn)
   {
       bool gok = false;
       try
       {
           SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_NHANVIEN", tn.ID_NHANVIEN),
                new SqlParameter("@ID_QLLH", tn.ID_QLLH),
                new SqlParameter("@ID_QUANLY", tn.ID_QUANLY),
                new SqlParameter("@NgayGui", tn.NgayGui),
                new SqlParameter("@NoiDung", tn.NoiDung),
                new SqlParameter("@TrangThai", tn.TrangThai),
                
                
            };

           if (db.ExecuteNonQuery("sp_TinNhan_ThemTinNhan", param) > 0)
               gok = true;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           
       }
       return false;
   }

   public static List<TinNhanOBJ> getDSTinNhanTheoID_ChuaDoc(int ID_QLLH, int ID_QuanLy)
   {
       try
       {
           List<TinNhanOBJ> lstTinNhan = new List<TinNhanOBJ>();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getDSTinNhanTheoIDQuanLy_ChuaDoc",
                new SqlParameter("@ID_QLLH", ID_QLLH),
               new SqlParameter("@ID_QuanLy", ID_QuanLy)
                
             );

           if (ds == null)
           {
               return null;
           }

           DataTable dtbl = null;
           dtbl = ds.Tables[0];
           for (int i = 0; i < dtbl.Rows.Count; i++)
           {
               DataRow dr = dtbl.Rows[i];

               TinNhanOBJ tinnhan = new TinNhanOBJ();
               tinnhan.ID_NHANVIEN = int.Parse(dr["ID_NHANVIEN"].ToString());
               tinnhan.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
               tinnhan.ID_TINNHAN = int.Parse(dr["ID_TINNHAN"].ToString());
               tinnhan.ID_QUANLY = dr["ID_QUANLY"].ToString() != "" ? int.Parse(dr["ID_QUANLY"].ToString()) : 0;
               tinnhan.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
               tinnhan.TrangThaiHienThi = dr["TrangThai"].ToString() == "1" ? "Đã xem" : "Chưa xem";
               tinnhan.NoiDung = dr["NoiDung"].ToString();
               tinnhan.NgayGui = dr["NgayGui"].ToString() != "" ? DateTime.Parse(dr["NgayGui"].ToString()) : tinnhan.NgayGui;
               tinnhan.NgayXem = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()) : tinnhan.NgayXem;
               tinnhan.NgayGuiHienThi = dr["NgayGui"].ToString() != "" ? DateTime.Parse(dr["NgayGui"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
               tinnhan.NgayXemHienThi = dr["NgayXem"].ToString() != "" ? DateTime.Parse(dr["NgayXem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
               tinnhan.TenQuanLy = dr["TenQuanLy"].ToString();
               tinnhan.TenNhanVien = dr["TenNhanVien"].ToString();
               lstTinNhan.Add(tinnhan);
           }

           return lstTinNhan;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }

   public static bool CapNhatTinNhanQuanLyDaXem(TinNhanOBJ tn)
   {
       try
       {
           SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_TINNHAN", tn.ID_TINNHAN),
                new SqlParameter("@TrangThai", 1),
                new SqlParameter("@NgayXem", DateTime.Now),
                new SqlParameter("@ID_QuanLy", tn.ID_QUANLY),
                
            };

           return db.ExecuteNonQuery("sp_TinNhan_CapNhatTrangThai_QuanLy_DaXem", param) > 0;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return false;
       }
   }

}