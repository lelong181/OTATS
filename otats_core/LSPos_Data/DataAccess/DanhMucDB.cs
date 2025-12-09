using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HangHoa
/// </summary>
public class DanhMucDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(DanhMucDB));
  
   public static SqlDataHelper db = new SqlDataHelper();

   public DanhMucDB()
	{
		//
		// TODO: Add constructor logic here
		//
	}
   public static List<DanhMucOBJ> getDS_DanhMuc(int Id_QLLH)
    {
        try
        {
            List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSDanhMucTheoID_QLLH",
                new SqlParameter("@ID_QLLH", Id_QLLH)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            DanhMucOBJ dmTatCa = new DanhMucOBJ();
           // dmTatCa.SoLuongMatHang = DemSoLuongMatHang(-1,Id_QLLH);
            dmTatCa.ID_DANHMUC = -1;
            dmTatCa.ID_PARENT = 0;
           // dmTatCa.TenHienThi = "Tất cả mặt hàng" + " (" + dmTatCa.SoLuongMatHang + ")";
           // dmTatCa.TenHienThi = "Tất cả mặt hàng";
            DataRow[] dr1 = dtbl.Select("ID_PARENT IS NULL OR ID_PARENT = 0 ");
            int Tong = 0;
            foreach (DataRow d in dr1)
            {
                Tong += (d["SoLuongMatHang"].ToString() != "") ? int.Parse(d["SoLuongMatHang"].ToString()) : 0;
            }
            dmTatCa.SoLuongMatHang = Tong;
            dmTatCa.TenHienThi = "Tất cả mặt hàng" + " (" + dmTatCa.SoLuongMatHang + ")";
            lstDanhMuc.Insert(0, dmTatCa);
          
            //
            DanhMucOBJ dmKhac = new DanhMucOBJ();
            dmKhac.ID_DANHMUC = 0;
            dmKhac.ID_PARENT = 0;
           
            //get all danh sach mat hang khong có
            //MatHang_dl mh = new MatHang_dl();
            //dmKhac.SoLuongMatHang = mh.DemTongMatHangKhongCoDanhMuc(Id_QLLH);
            //dmKhac.TenHienThi = "Khác" + " (" + dmKhac.SoLuongMatHang + ")";
            //lstDanhMuc.Insert(1, dmKhac);
            //lstDanhMuc.Insert(lstDanhMuc.Count, dmKhac);


            //lstDanhMuc.Add(dmTatCa);
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                DanhMucOBJ dm = new DanhMucOBJ();
                dm.ID_DANHMUC =   (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao ;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenDanhMuc = dr["TenDanhMuc"].ToString();
                
                //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
                dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
                //dm.SoLuongMatHang = DemSoLuongMatHang(dm.ID_DANHMUC, dm.ID_QLLH);
                dm.TenHienThi = dr["TenDanhMuc"].ToString() + " (" + dm.SoLuongMatHang + ")";


                //dm.TenHienThi = dr["TenDanhMuc"].ToString();
                lstDanhMuc.Add(dm);
            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    
   public static List<DanhMucOBJ> getDS_DanhMucCapCha(int Id_QLLH)
   {
       try
       {
           List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getDSDanhMucTheoID_QLLH",
               new SqlParameter("@ID_QLLH", Id_QLLH)
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
               if (dr["ID_PARENT"].ToString() == "" || dr["ID_PARENT"].ToString() == "0")
               {
                   DanhMucOBJ dm = new DanhMucOBJ();
                   dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                   dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                   dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                   dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                   dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                   dm.TenDanhMuc = dr["TenDanhMuc"].ToString();
                    dm.TenHienThi = dr["TenDanhMuc"].ToString() + " (" + dm.SoLuongMatHang + ")";
                    //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
                   dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
                  // dm.SoLuongMatHang = DemSoLuongMatHang(dm.ID_DANHMUC, dm.ID_QLLH);

                   lstDanhMuc.Add(dm);
               }
           }

           return lstDanhMuc;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }

   //public static List<DanhMucOBJ> getDS_DanhMucToNhat(int ID_QLLH)
   //{
   //    try
   //    {
   //        List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
   //        SqlDataHelper sql = new SqlDataHelper();
   //        DataSet ds = sql.ExecuteDataSet("sp_QL_GetDanhMucCon",
   //            new SqlParameter("@ID_QLLH", ID_QLLH)
   //          );

   //        if (ds == null)
   //        {
   //            return null;
   //        }

   //        DataTable dtbl = null;
   //        dtbl = ds.Tables[0];
   //        for (int i = 0; i < dtbl.Rows.Count; i++)
   //        {
   //            DataRow dr = dtbl.Rows[i];
   //            DanhMucOBJ dm = new DanhMucOBJ();
   //            dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
   //            dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
   //            dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
   //            dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
   //            dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
   //            dm.TenDanhMuc = dr["TenDanhMuc"].ToString();

   //            lstDanhMuc.Add(dm);
   //        }

   //        return lstDanhMuc;
   //    }
   //    catch (Exception ex)
   //    {
   //        log.Error(ex);
   //        return null;
   //    }
   //}
   public static List<DanhMucOBJ> getDS_DanhMucCon_ByIdDanhMuc(int ID_DANHMUC)
   {
       try
       {
           List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getDS_DanhMucCon_ByIdDanhMuc",
               new SqlParameter("@ID_DANHMUC", ID_DANHMUC)
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
               DanhMucOBJ dm = new DanhMucOBJ();
               dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
               dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
               dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
               dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
               dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
               dm.TenDanhMuc = dr["TenDanhMuc"].ToString();
               dm.AnhDaiDien = dr["AnhDaiDien"].ToString();
               
               
                dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
                //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
                //dm.SoLuongMatHang = DemSoLuongMatHang(dm.ID_DANHMUC,dm.ID_QLLH);
                //dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
                dm.TenHienThi = dr["TenDanhMuc"].ToString();

                lstDanhMuc.Add(dm);
           }

           return lstDanhMuc;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }

   //public static int DemSoLuongMatHang(int ID_DANHMUC,int ID_QLLH)
   //{

   //    int sl = 0;
   //    //if (ID_DANHMUC == -1)
   //    //{
   //    //    List<HangHoaOBJ> lst = getDS_HangHoa_ByIdDanhMuc(ID_DANHMUC, ID_QLLH);
   //    //    sl += lst.Count;
   //    //}
   //    //else
   //    //{
   //        List<HangHoaOBJ> lst = getDS_HangHoa_ByIdDanhMuc(ID_DANHMUC, ID_QLLH);
   //        sl += lst.Count;
           
   //    //}
   //    return sl;
   //}

   public static List<HangHoaOBJ> getDS_HangHoa_ByIdDanhMuc(int ID_DANHMUC, int ID_QLLH)
   {
       List<HangHoaOBJ> rs = new List<HangHoaOBJ>();

       try
       {

           SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_DANHMUC", ID_DANHMUC),
                new SqlParameter("@ID_QLLH", ID_QLLH)
            };

           DataTable dt = db.ExecuteDataSet("getDS_HangHoa_ByIdDanhMuc", pars).Tables[0];
           int a = dt.Rows.Count;
           foreach (DataRow dr in dt.Rows)
           {
               try
               {
                   rs.Add(new HangHoaOBJ
                   {
                       idhang = int.Parse(dr["ID_Hang"].ToString()),
                       mahang = dr["MaHang"].ToString(),
                       tenhang = dr["TenHang"].ToString(),
                       giabuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()).ToString("n0") : (-1).ToString("n0"),
                       giale = dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()).ToString("n0") : (-1).ToString("n0"),
                       tonkho = double.Parse(dr["SoLuong"].ToString()),
                       donvi = dr["TenDonVi"].ToString(),
                       soluong = double.Parse(dr["SoLuong"].ToString()),
                       khuyenmai = dr["KhuyenMai"].ToString(),
                       iddanhmuc = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0,
                       tendanhmuc = dr["TenDanhMuc"].ToString(),

                   });
               }
               catch (Exception ex)
               {
                   log.Error(ex);
               }
           }

           //lay cac mat hang cua danh muc con
           List<DanhMucOBJ> lstDMCon = getDS_DanhMucCon_ByIdDanhMuc(ID_DANHMUC);
           foreach (DanhMucOBJ dm in lstDMCon)
           {
               List<HangHoaOBJ> lstHangHoaCon = getDS_HangHoa_ByIdDanhMuc(dm.ID_DANHMUC, dm.ID_QLLH);
               foreach (HangHoaOBJ h in lstHangHoaCon)
               {
                   rs.Add(h);
               }
           }

           return rs;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return rs;
       }
   }

   public static bool ThemDanhMuc(DanhMucOBJ dm)
   {
       try
       {
           SqlDataHelper helper = new SqlDataHelper();
           SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", dm.ID_QLLH),
                new SqlParameter("ID_PARENT", dm.ID_PARENT),
                new SqlParameter("TenDanhMuc", dm.TenDanhMuc),
                new SqlParameter("AnhDaiDien", dm.AnhDaiDien),
                
            };
           int i = helper.ExecuteNonQuery("sp_DANHMUC_Insert", pars);
           if (i > 0)
           {
               // them thanh cong
               return true;
           }
           else
           {
               // them that bai
               return false;
           }
       }
       catch
       {
           return false;
       }
   }

   public static bool SuaDanhMuc(DanhMucOBJ dm)
   {
       try
       {
           SqlDataHelper helper = new SqlDataHelper();
           SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("ID_DANHMUC", dm.ID_DANHMUC), 
               new SqlParameter("ID_QLLH", dm.ID_QLLH),
                new SqlParameter("ID_PARENT", dm.ID_PARENT),
                new SqlParameter("TenDanhMuc", dm.TenDanhMuc),
                new SqlParameter("AnhDaiDien", dm.AnhDaiDien),
                new SqlParameter("TrangThai", dm.TrangThai),
                
            };

           if (helper.ExecuteNonQuery("sp_DANHMUC_Update", pars) != 0)
           {
              
               return true;
           }
           else
           {
               return false;
           }
       }
       catch
       {
           return false;
       }
   }

   public static bool XoaDanhMuc(DanhMucOBJ dm)
   {
       try
       {
           SqlDataHelper helper = new SqlDataHelper();
           SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("ID_DANHMUC", dm.ID_DANHMUC), 
              
                
            };

           if (helper.ExecuteNonQuery("sp_DANHMUC_Delete", pars) != 0)
           {

               return true;
           }
           else
           {
               return false;
           }
       }
       catch
       {
           return false;
       }
   }

   public static DanhMucOBJ get_DanhMucById(int ID_DANHMUC)
   {
       try
       {
           DanhMucOBJ dm = new DanhMucOBJ();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getDSDanhMucTheoID_DANHMUC",
               new SqlParameter("@ID_DANHMUC", ID_DANHMUC)
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
               
               dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
               dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
               dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
               dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
               dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
               dm.TenDanhMuc = dr["TenDanhMuc"].ToString();
               //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
               dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
              // dm.SoLuongMatHang = DemSoLuongMatHang(dm.ID_DANHMUC, dm.ID_QLLH);
               dm.TenHienThi = dr["TenDanhMuc"].ToString() + " (" + dm.SoLuongMatHang + ")";
                
           }

           return dm;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }
   public static DanhMucOBJ get_DanhMucByName(int ID_QLLH, string TenDanhMuc)
   {
       try
       {
           DanhMucOBJ dm = new DanhMucOBJ();
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("getDSDanhMucTheoTen_DanhMuc",
               new SqlParameter("@ID_QLLH", ID_QLLH),
                 new SqlParameter("@TenDanhMuc", TenDanhMuc)
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

               dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
               dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
               dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
               dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
               dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
               dm.TenDanhMuc = dr["TenDanhMuc"].ToString();
               //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
               dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
               // dm.SoLuongMatHang = DemSoLuongMatHang(dm.ID_DANHMUC, dm.ID_QLLH);
               dm.TenHienThi = dr["TenDanhMuc"].ToString() + " (" + dm.SoLuongMatHang + ")";

           }

           return dm;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }

   public static DataTable getDataDS_DanhMuc(int Id_QLLH)
   {
       try
       {
           SqlDataHelper sql = new SqlDataHelper();
           DataSet ds = sql.ExecuteDataSet("sp_DANHMUC_getDSDanhMucTheoID_QLLH",
               new SqlParameter("@ID_QLLH", Id_QLLH)
             );

           if (ds == null)
           {
               return null;
           }

           DataTable dtbl = null;
           dtbl = ds.Tables[0];


           return dtbl;
       }
       catch (Exception ex)
       {
           log.Error(ex);
           return null;
       }
   }

    public static List<DanhMucOBJ> getDS_DanhMuc_TheoPhanQuyen(int ID_DANHMUC, int ID_QLLH, int ID_NhanVien)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
               
                new SqlParameter("@ID_QLLH", ID_QLLH),
                  new SqlParameter("@idnhanvien", ID_NhanVien),
            };
            List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSDanhMucTheoID_QLLH_TheoPhanQuyen",
                pars
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            DanhMucOBJ dmTatCa = new DanhMucOBJ();
            // dmTatCa.SoLuongMatHang = DemSoLuongMatHang(-1,Id_QLLH);
            dmTatCa.ID_DANHMUC = -1;
            dmTatCa.ID_PARENT = 0;
            // dmTatCa.TenHienThi = "Tất cả mặt hàng" + " (" + dmTatCa.SoLuongMatHang + ")";
            // dmTatCa.TenHienThi = "Tất cả mặt hàng";
            DataRow[] dr1 = dtbl.Select("ID_PARENT IS NULL OR ID_PARENT = 0 ");
            int Tong = 0;
            foreach (DataRow d in dr1)
            {
                Tong += (d["SoLuongMatHang"].ToString() != "") ? int.Parse(d["SoLuongMatHang"].ToString()) : 0;
            }
            dmTatCa.SoLuongMatHang = Tong;
            dmTatCa.TenHienThi = "Tất cả mặt hàng" + " (" + dmTatCa.SoLuongMatHang + ")";
            lstDanhMuc.Insert(0, dmTatCa);

            //
            DanhMucOBJ dmKhac = new DanhMucOBJ();
            dmKhac.ID_DANHMUC = 0;
            dmKhac.ID_PARENT = 0;
             


            //lstDanhMuc.Add(dmTatCa);
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                DanhMucOBJ dm = new DanhMucOBJ();
                dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenDanhMuc = dr["TenDanhMuc"].ToString();

                //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
                dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
                //dm.SoLuongMatHang = DemSoLuongMatHang(dm.ID_DANHMUC, dm.ID_QLLH);
                //dm.TenHienThi = dr["TenDanhMuc"].ToString() + " (" + dm.SoLuongMatHang + ")";
                dm.TenHienThi = dr["TenDanhMuc"].ToString();


                //dm.TenHienThi = dr["TenDanhMuc"].ToString();
                lstDanhMuc.Add(dm);
            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public static List<DanhMucOBJ> getDS_DanhMuc_TheoPhanQuyen_Admin(int ID_DANHMUC, int ID_QLLH, int ID_NhanVien)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                new SqlParameter("@ID_QLLH", ID_QLLH),
                  new SqlParameter("@idnhanvien", ID_NhanVien),
            };
            List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSDanhMucTheoID_QLLH_TheoPhanQuyen_Admin",
                pars
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            DanhMucOBJ dmTatCa = new DanhMucOBJ();
            // dmTatCa.SoLuongMatHang = DemSoLuongMatHang(-1,Id_QLLH);
            dmTatCa.ID_DANHMUC = -1;
            dmTatCa.ID_PARENT = 0;
            // dmTatCa.TenHienThi = "Tất cả mặt hàng" + " (" + dmTatCa.SoLuongMatHang + ")";
            // dmTatCa.TenHienThi = "Tất cả mặt hàng";
            DataRow[] dr1 = dtbl.Select("ID_PARENT IS NULL OR ID_PARENT = 0 ");
            int Tong = 0;
            foreach (DataRow d in dr1)
            {
                Tong += (d["SoLuongMatHang"].ToString() != "") ? int.Parse(d["SoLuongMatHang"].ToString()) : 0;
            }
            dmTatCa.SoLuongMatHang = Tong;
            dmTatCa.TenHienThi = "Tất cả mặt hàng" + " (" + dmTatCa.SoLuongMatHang + ")";
            lstDanhMuc.Insert(0, dmTatCa);

            //
            DanhMucOBJ dmKhac = new DanhMucOBJ();
            dmKhac.ID_DANHMUC = 0;
            dmKhac.ID_PARENT = 0;



            //lstDanhMuc.Add(dmTatCa);
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                DanhMucOBJ dm = new DanhMucOBJ();
                dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenDanhMuc = dr["TenDanhMuc"].ToString();

                //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
                dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
                //dm.SoLuongMatHang = DemSoLuongMatHang(dm.ID_DANHMUC, dm.ID_QLLH);
                //dm.TenHienThi = dr["TenDanhMuc"].ToString() + " (" + dm.SoLuongMatHang + ")";
                dm.TenHienThi = dr["TenDanhMuc"].ToString();


                //dm.TenHienThi = dr["TenDanhMuc"].ToString();
                lstDanhMuc.Add(dm);
            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }


}