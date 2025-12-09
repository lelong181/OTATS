using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class ChucVuDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(ChucVuDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public ChucVuDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public ChucVuOBJ GetChucVuById(int ID_ChucVu)
    {
        ChucVuOBJ rs = new ChucVuOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_ChucVu", ID_ChucVu)
        };

        DataSet ds = db.ExecuteDataSet("sp_ChucVu_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_ChucVu = int.Parse(dr["ID_ChucVu"].ToString());
            rs.TenChucVu = dr["TenChucVu"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
             
        }
        catch
        {
            return null;
        }
        return rs;
    }
    public ChucVuOBJ GetFromDataRow(DataRow dr)
    {
        ChucVuOBJ rs = new ChucVuOBJ();
 
      

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_ChucVu = int.Parse(dr["ID_ChucVu"].ToString());
            rs.TenChucVu = dr["TenChucVu"].ToString();

            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<ChucVuOBJ> GetListDanhSachChucVu(int idct)
    {
        List<ChucVuOBJ> lst = new List<ChucVuOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
           DataTable  dt = db.ExecuteDataSet("sp_LoaiKhachHang_GetAll", param).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                ChucVuOBJ rs = GetFromDataRow(dr);
                lst.Add(rs);
            }
            
           
        }
        catch (Exception ex)
        {
            log.Error(ex);
            
        }
        return lst;
    }

    public List<ChucVuOBJ> GetByNam(int idct, string TenLoaiKhachHang)
    {
        List<ChucVuOBJ> lst = new List<ChucVuOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct),
                 new SqlParameter("@TenLoaiKhachHang", TenLoaiKhachHang)
            };
            DataTable dt = db.ExecuteDataSet("sp_LoaiKhachHang_GetAll", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ChucVuOBJ rs = GetFromDataRow(dr);
                lst.Add(rs);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }

    public DataTable LayDanhSach(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            dt = db.ExecuteDataSet("sp_ChucVu_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public bool Them(ChucVuOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                 
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenChucVu", obj.TenChucVu) 

                };
            int i = db.ExecuteNonQuery("sp_ChucVu_ThemMoi", pars);
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
    public bool Sua(ChucVuOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_ChucVu", obj.ID_ChucVu),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenChucVu", obj.TenChucVu) 

                };
            int i = db.ExecuteNonQuery("sp_ChucVu_Sua", pars);
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
    public bool Xoa(ChucVuOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_ChucVu", obj.ID_ChucVu) 
                    
                };
            int i = db.ExecuteNonQuery("sp_ChucVu_Xoa", pars);
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