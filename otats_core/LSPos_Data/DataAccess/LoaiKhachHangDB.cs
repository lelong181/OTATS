using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class LoaiKhachHangDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoaiKhachHangDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public LoaiKhachHangDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public LoaiKhachHangOBJ GetLoaiKhachHangById(int ID_LoaiKhachHang)
    {
        LoaiKhachHangOBJ rs = new LoaiKhachHangOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang)
        };

        DataSet ds = db.ExecuteDataSet("sp_LoaiKhachHang_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_LoaiKhachHang = int.Parse(dr["ID_LoaiKhachHang"].ToString());
            rs.TenLoaiKhachHang = dr["TenLoaiKhachHang"].ToString();
            rs.IconHienThi = dr["IconHienThi"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public LoaiKhachHangOBJ GetLoaiKhachHangFromDataRow(DataRow dr)
    {
        LoaiKhachHangOBJ rs = new LoaiKhachHangOBJ();



        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_LoaiKhachHang = int.Parse(dr["ID_LoaiKhachHang"].ToString());
            rs.TenLoaiKhachHang = dr["TenLoaiKhachHang"].ToString();
            rs.IconHienThi = dr["IconHienThi"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<LoaiKhachHangOBJ> GetListDanhSachLoaiKhachHang(int idct)
    {
        List<LoaiKhachHangOBJ> lst = new List<LoaiKhachHangOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            DataTable dt = db.ExecuteDataSet("sp_LoaiKhachHang_GetAll", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                LoaiKhachHangOBJ rs = GetLoaiKhachHangFromDataRow(dr);
                lst.Add(rs);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }

    public List<LoaiKhachHangOBJ> GetLoaiKhachHang(int idct, string TenLoaiKhachHang)
    {
        List<LoaiKhachHangOBJ> lst = new List<LoaiKhachHangOBJ>();
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
                LoaiKhachHangOBJ rs = GetLoaiKhachHangFromDataRow(dr);
                lst.Add(rs);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }

    public DataTable LayDanhSachLoaiKhachHang(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            dt = db.ExecuteDataSet("sp_LoaiKhachHang_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public int Them(LoaiKhachHangOBJ obj)
    {
        int ret = 0;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenLoaiKhachHang", obj.TenLoaiKhachHang),
                    new SqlParameter("IconHienThi", obj.IconHienThi),

                };
            object rs = db.ExecuteScalar("sp_LoaiKhachHang_ThemMoi", pars);
            ret = int.Parse(rs.ToString());

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public bool Sua(LoaiKhachHangOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_LoaiKhachHang", obj.ID_LoaiKhachHang),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenLoaiKhachHang", obj.TenLoaiKhachHang),
                     new SqlParameter("IconHienThi", obj.IconHienThi)

                };
            int i = db.ExecuteNonQuery("sp_LoaiKhachHang_Sua", pars);
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
    public bool Xoa(LoaiKhachHangOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_LoaiKhachHang", obj.ID_LoaiKhachHang)

                };
            int i = db.ExecuteNonQuery("sp_LoaiKhachHang_Xoa", pars);
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