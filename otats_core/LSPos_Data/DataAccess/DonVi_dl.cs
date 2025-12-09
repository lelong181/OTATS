using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonVi_dl
/// </summary>
public class DonVi_dl
{
    private SqlDataHelper helper;

    public DonVi GetMatHangFromDataRow(DataRow dr)
    {
        try
        {
            DonVi dv = new DonVi();
            dv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
            dv.ID_DonVi = int.Parse(dr["ID_DonVi"].ToString());
            dv.TenDonVi = dr["TenDonVi"].ToString();
            return dv;
        }
        catch
        {
            return null;
        }
    }

    public DonVi_dl()
    {
        //
        // TODO: Add constructor logic here
        //
        helper = new SqlDataHelper();
    }

    public List<DonVi> GetDonViAll(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getDSDonVi", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DonVi> dsdv = new List<DonVi>();
            foreach (DataRow dr in dt.Rows)
            {
                DonVi dv = new DonVi();
                dv.ID_DonVi = int.Parse(dr["ID_DonVi"].ToString());
                dv.TenDonVi = dr["TenDonVi"].ToString();
                dsdv.Add(dv);
            }

            return dsdv;
        }
        catch
        {
            return null;
        }
    }

    public DonVi GetDonViByID(int ID_DonVi)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonVi", ID_DonVi)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getDonViTheoID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            DonVi dv = new DonVi();
            dv.ID_DonVi = int.Parse(dr["ID_DonVi"].ToString());
            dv.TenDonVi = dr["TenDonVi"].ToString();
            dv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());

            return dv;
        }
        catch
        {
            return null;
        }
    }

    public DonVi GetDonViByName(string TenDonVi,int ID_QLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("TenDonVi", TenDonVi),
            new SqlParameter("ID_QLLH", ID_QLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetDonViByName", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            DonVi dv = new DonVi();
            dv.ID_DonVi = int.Parse(dr["ID_DonVi"].ToString());
            dv.TenDonVi = dr["TenDonVi"].ToString();
            dv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());

            return dv;
        }
        catch
        {
            return null;
        }
    }

    public List<DonVi> GetDonViByIDQLLH(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getDSDonViByIDQL", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DonVi> dsdv = new List<DonVi>();
            foreach (DataRow dr in dt.Rows)
            {
                DonVi dv = new DonVi();
                dv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
                dv.ID_DonVi = int.Parse(dr["ID_DonVi"].ToString());
                dv.TenDonVi = dr["TenDonVi"].ToString();
                dsdv.Add(dv);
            }

            return dsdv;
        }
        catch
        {
            return null;
        }
    }


    public bool ThemDonVi(DonVi dv)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_DonVi", int.Parse("0")),
                new SqlParameter("ID_QLLH", dv.IDQLLH),
                new SqlParameter("TenDonVi", dv.TenDonVi)
            };

            if (helper.ExecuteNonQuery("sp_web_insertdonvi", pars) != 0)
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

    public bool CheckTrungDonVi(int ID_QLLH,string TenDonVi,int ID_DonVi)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("TenDonVi", TenDonVi),
                new SqlParameter("ID_DonVi", ID_DonVi)
            };

            if (helper.ExecuteDataSet("sp_QL_CheckTrungDonVi", pars).Tables[0].Rows.Count>0)
            {
                //đã tồn tại
                return false;
            }
            else
            {
                //chưa tồn tại
                return true;
            }
        }
        catch
        {
            return true;
        }
    }

    public bool CapNhatDonVi(DonVi dv)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_DonVi", dv.ID_DonVi),
                new SqlParameter("ID_QLLH", dv.IDQLLH),
                new SqlParameter("TenDonVi", dv.TenDonVi)
            };

            if (helper.ExecuteNonQuery("sp_web_insertdonvi", pars) != 0)
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

    public bool DeleteDonVi(DonVi dv)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_DonVi", dv.ID_DonVi),
                new SqlParameter("ID_QLLH", dv.IDQLLH)
            };

            if (helper.ExecuteNonQuery("sp_web_deletedonvi", pars) != 0)
            {
                // delete thanh cong
                return true;
            }
            else
            {
                // delete that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}