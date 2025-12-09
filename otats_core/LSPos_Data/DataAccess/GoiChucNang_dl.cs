using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GoiUngDung_dl
/// </summary>
public class GoiChucNang_dl
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(GoiChucNang_dl));
    public static SqlDataHelper db = new SqlDataHelper();
    public GoiChucNang_dl()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bool ThemMoi(GoiChucNangOBJ UD)
    {
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@TenGoiChucNang", UD.TenGoi),
                new SqlParameter("@MoTa", UD.MoTa),
            };

            return db.ExecuteNonQuery("sp_GoiChucNang_ThemMoi", param) > 0;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }

    public DataTable GetAllGoiChucNang()
    {
        try
        {
            return db.ExecuteDataSet_INCLUDE_AutoIncrement("sp_GoiChucNang_GetAll");
        }
        catch (Exception Ex)
        {
            log.Error(Ex);
            return null;
        }
    }
    public List<GoiChucNangOBJ> GetChucNangBy_ID(int ID) 
    {
        try
        {
            List<GoiChucNangOBJ> list = new List<GoiChucNangOBJ>();
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_GoiChucNang", ID)
            };
            DataTable dt = db.ExecuteDataSet("sp_GoiChucNang_GetById", par).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                //GoiUngDungOBJ ud = new GoiUngDungOBJ();
                //ud.IDGoiUngDung = int.Parse(dr["IDGoiUngDung"].ToString());
                //ud.TenGoi = dr["TenGoi"].ToString();
                //ud.MoTa = dr["MoTa"].ToString();
                //list.Add(ud);
                list.Add(new GoiChucNangOBJ
                { 
                    IDGoiChucNang = int.Parse(dr["ID_GoiChucNang"].ToString()),
                    TenGoi = dr["TenGoiChucNang"].ToString(),
                    MoTa = dr["MoTa"].ToString(),
                });
            }
            return list;
        }
        catch (Exception ex) 
        {
            return null;
        }
    }
    public bool CapNhat(GoiChucNangOBJ ud)
    {
        try 
        {
            SqlParameter[] parm = new SqlParameter[]
        {
            new SqlParameter("@ID_GoiChucNang", ud.IDGoiChucNang),
            new SqlParameter("@TenGoiChucNang", ud.TenGoi),
            new SqlParameter("@MoTa", ud.MoTa)

        };
            return db.ExecuteNonQuery("sp_GoiChucNang_CapNhat", parm) > 0;
        }
        catch (Exception ex) 
        {
            log.Error(ex);
            return false;
        }
    }
    public bool Xoa(int ID_GoiChucNang)
    {
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_GoiChucNang", ID_GoiChucNang),
                 
            };

            return db.ExecuteNonQuery("sp_GoiChucNang_Delete", param) > 0;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }
}