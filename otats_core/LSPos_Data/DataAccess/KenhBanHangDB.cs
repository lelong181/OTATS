using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class KenhBanHangDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(KenhBanHangDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public KenhBanHangDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static  KenhBanHangOBJ GetKenhBanHangById(int ID_KenhBanHang)
    {
        KenhBanHangOBJ rs = new KenhBanHangOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhomKH", ID_KenhBanHang)
        };

        DataSet ds = db.ExecuteDataSet("sp_NhomKhachHang_GetById", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs = GetKenhBanHangFromDataRow(dr);


        }
        catch
        {
            return null;
        }
        return rs;
    }
    public static KenhBanHangOBJ GetKenhBanHangFromDataRow(DataRow dr)
    {
        KenhBanHangOBJ rs = new KenhBanHangOBJ();
        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_KenhBanHang = int.Parse(dr["ID_NhomKH"].ToString());
            rs.ID_KenhCapTren = dr["ID_Cha"].ToString() != "" ? int.Parse(dr["ID_Cha"].ToString()) : 0;
            rs.TenKenhBanHang = dr["TenNhomKH"].ToString();
            rs.MaKenhBanHang = dr["MaNhomKH"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
        }
        catch
        {
            return null;
        }
        return rs;
    }
    public static  List<KenhBanHangOBJ> GetListKenhBanHang(int idct)
    {
        List<KenhBanHangOBJ> lst = new List<KenhBanHangOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
           DataTable  dt = db.ExecuteDataSet("sp_NhomKhachHang_GetAll", param).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                KenhBanHangOBJ rs = GetKenhBanHangFromDataRow(dr);
                lst.Add(rs);
            }
            
           
        }
        catch (Exception ex)
        {
            log.Error(ex);
            
        }
        return lst;
    }

    public static List<KenhBanHangOBJ> getKenhBanHang_CapCha(int Id_QLLH)
    {
        try
        {
            List<KenhBanHangOBJ> lstDanhMuc = new List<KenhBanHangOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("sp_NhomKhachHang_GetAll",
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
                if (dr["ID_Cha"].ToString() == "" || dr["ID_Cha"].ToString() == "0")
                {
                    KenhBanHangOBJ dm = GetKenhBanHangFromDataRow(dr);

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

    public static List<KenhBanHangOBJ> getKenhBanHang_ByIdCapCha(int ID_KenhBanHang, int ID_QLLH)
    {
        try
        {
            List<KenhBanHangOBJ> lstDanhMuc = new List<KenhBanHangOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("sp_NhomKhachHang_GetByIdCapCha_v2",
                new SqlParameter("@ID_NhomKH", ID_KenhBanHang),
                 new SqlParameter("@ID_QLLH", ID_QLLH)
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
                KenhBanHangOBJ dm = GetKenhBanHangFromDataRow(dr);
                  
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

    public List<KenhBanHangOBJ> GetKenhBanHang(int idct, string TenKenhBanHang)
    {
        List<KenhBanHangOBJ> lst = new List<KenhBanHangOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct),
                 new SqlParameter("@TenNhomKhachHang", TenKenhBanHang)
            };
            DataTable dt = db.ExecuteDataSet("sp_NhomKhachHang_GetAll", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                KenhBanHangOBJ rs = GetKenhBanHangFromDataRow(dr);
                lst.Add(rs);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }
    public static DataTable LayDanhSachKenhBanHang(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            dt = db.ExecuteDataSet("sp_NhomKhachHang_GetAll", param).Tables[0];
            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public  static bool Them(KenhBanHangOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                 new SqlParameter("ID_Cha", obj.ID_KenhCapTren),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenNhomKH", obj.TenKenhBanHang),
                    new SqlParameter("MaNhomKH", obj.MaKenhBanHang),


                };
            int i = db.ExecuteNonQuery("sp_NhomKhachHang_ThemMoi", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public static bool Sua(KenhBanHangOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                 new SqlParameter("ID_NhomKH", obj.ID_KenhBanHang),
                    new SqlParameter("ID_Cha", obj.ID_KenhCapTren),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenNhomKH", obj.TenKenhBanHang),
                     new SqlParameter("MaNhomKH", obj.MaKenhBanHang),
                      new SqlParameter("TrangThai", obj.TrangThai)

                };
            int i = db.ExecuteNonQuery("sp_NhomKhachHang_Sua", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public static bool Xoa(int ID_KenhBanHang)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_NhomKH", ID_KenhBanHang)

                };
            int i = db.ExecuteNonQuery("sp_NhomKH_Xoa", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }

}