using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KhuVuc_dl
/// </summary>
public class KhuVuc_dl
{
    static log4net.ILog log = log4net.LogManager.GetLogger(typeof(KhuVuc_dl));
    private SqlDataHelper helper;

    //public Tinh GetTinhFromDataRow(DataRow dr)
    //{
    //    try
    //    {
    //        Tinh tinh = new Tinh();
    //        tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
    //        tinh.TenTinh = dr["TenTinh"].ToString();
    //        return tinh;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public KhuVuc_dl()
    {
        helper = new SqlDataHelper();
    }

    public List<KhuVuc> GetAll()
    {

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllKhuVuc");
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KhuVuc> dsTinh = new List<KhuVuc>();
            foreach (DataRow dr in dt.Rows)
            {
                KhuVuc tinh = new KhuVuc();
                tinh.ID_KhuVuc = int.Parse(dr["ID_KhuVuc"].ToString());
                tinh.TenKhuVuc = dr["TenKhuVuc"].ToString();
                dsTinh.Add(tinh);
            }

            return dsTinh;
        }
        catch
        {
            return null;
        }
    }

    public List<Tinh> GetTinhTheoKhuVuc(int ID_KhuVuc)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhuVuc", ID_KhuVuc)
        };
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetTinhTheoKhuVuc", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<Tinh> dsQuan = new List<Tinh>();
            foreach (DataRow dr in dt.Rows)
            {
                Tinh quan = new Tinh();
                quan.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
                quan.TenTinh = dr["TenQuan"].ToString();
                quan.ID_KhuVuc = int.Parse(dr["ID_KhuVuc"].ToString());
                dsQuan.Add(quan);
            }

            return dsQuan;
        }
        catch
        {
            return null;
        }
    }
}


 