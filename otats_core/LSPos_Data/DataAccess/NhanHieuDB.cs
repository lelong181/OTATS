using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class NhanHieuDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(NhanHieuDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public NhanHieuDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public NhanHieuOBJ GetById(int ID)
    {
        NhanHieuOBJ rs = new NhanHieuOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanHieu", ID)
        };

        DataSet ds = db.ExecuteDataSet("sp_NhanHieu_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_NhanHieu = int.Parse(dr["ID_NhanHieu"].ToString());
            rs.TenNhanHieu = dr["TenNhanHieu"].ToString();
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;

            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.TenTrangThai = dr["TenTrangThai"].ToString();
        }
        catch
        {
            return null;
        }
        return rs;
    }
    public NhanHieuOBJ GetFromDataRow(DataRow dr)
    {
        NhanHieuOBJ rs = new NhanHieuOBJ();
 
      

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_NhanHieu = int.Parse(dr["ID_NhanHieu"].ToString());
            rs.TenNhanHieu = dr["TenNhanHieu"].ToString();
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;

            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.TenTrangThai = dr["TenTrangThai"].ToString();

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<NhanHieuOBJ> GetListDanhSach (int idct)
    {
        List<NhanHieuOBJ> lst = new List<NhanHieuOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
           DataTable  dt = db.ExecuteDataSet("sp_NhanHieu_GetAll", param).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                NhanHieuOBJ rs = GetFromDataRow(dr);
                lst.Add(rs);
            }
            
           
        }
        catch (Exception ex)
        {
            log.Error(ex);
            
        }
        return lst;
    }

    
    public DataTable GetDanhSach(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            dt = db.ExecuteDataSet("sp_NhanHieu_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public bool Them(NhanHieuOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                 
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenNhanHieu", obj.TenNhanHieu),
                    

                };
            int i = db.ExecuteNonQuery("sp_NhanHieu_ThemMoi", pars);
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
    public bool Sua(NhanHieuOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_NhanHieu", obj.ID_NhanHieu),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenNhanHieu", obj.TenNhanHieu),

                };
            int i = db.ExecuteNonQuery("sp_NhanHieu_Sua", pars);
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
    public bool Xoa(int ID_NhanHieu)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_NhanHieu", ID_NhanHieu) 
                    
                };
            int i = db.ExecuteNonQuery("sp_NhanHieu_Xoa", pars);
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