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
public class GoiUngDung_dl
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(GoiUngDung_dl));
    public static SqlDataHelper db = new SqlDataHelper();
    public GoiUngDung_dl()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bool ThemGoiUngDung(GoiUngDungOBJ UD)
    {
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@TenGoi", UD.TenGoi),
                new SqlParameter("@MoTa", UD.MoTa),
            };

            return db.ExecuteNonQuery("sp_GoiUngDung_ThemMoi", param) > 0;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }

    public DataTable GetAllGoiUngDung()
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
    public List<GoiUngDungOBJ> GetUngDung_IDUngDung(int ID) 
    {
        try
        {
            List<GoiUngDungOBJ> list = new List<GoiUngDungOBJ>();
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@IDUngDung", ID)
            };
            DataTable dt = db.ExecuteDataSet("getUngDung_Theo_IDUngDung", par).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                //GoiUngDungOBJ ud = new GoiUngDungOBJ();
                //ud.IDGoiUngDung = int.Parse(dr["IDGoiUngDung"].ToString());
                //ud.TenGoi = dr["TenGoi"].ToString();
                //ud.MoTa = dr["MoTa"].ToString();
                //list.Add(ud);
                list.Add(new GoiUngDungOBJ 
                { 
                    IDGoiUngDung = int.Parse(dr["IDGoiUngDung"].ToString()),
                    TenGoi = dr["TenGoi"].ToString(),
                     
                });
            }
            return list;
        }
        catch (Exception ex) 
        {
            return null;
        }
    }
    public bool SuaGoiUngDung(GoiUngDungOBJ ud)
    {
        try 
        {
            SqlParameter[] parm = new SqlParameter[]
        {
            new SqlParameter("@IDGoiUngDung", ud.IDGoiUngDung),
            new SqlParameter("@TenGoi", ud.TenGoi),
            new SqlParameter("@MoTa", ud.MoTa)

        };
            return db.ExecuteNonQuery("Update_GoiUngDung", parm) > 0;
        }
        catch (Exception ex) 
        {
            log.Error(ex);
            return false;
        }
    }
    public bool XoaUngDung(int IDGoiUngDung)
    {
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@IDGoiUngDung", IDGoiUngDung),
                 
            };

            return db.ExecuteNonQuery("Delete_GoiUngDung_ID", param) > 0;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }
}