using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ApGia_dl
/// </summary>
public class ApGia_dl
{
    private SqlDataHelper helper;
    public ApGia_dl()
    {

        helper = new SqlDataHelper();
    }
    public ApGia GetApGiaFromDataRow(DataRow dr)
    {
        try
        {
            ApGia gia = new ApGia();
            gia.ID_ApGia = int.Parse(dr["ID_ApGia"].ToString());
            gia.ID_QuanLy = int.Parse(dr["ID_QuanLy"].ToString());
            gia.ID_Hang = int.Parse(dr["ID_Hang"].ToString());
            gia.TrangThai = int.Parse(dr["TrangThai"].ToString());
            gia.TuNgay = DateTime.Parse(dr["TuNgay"].ToString());
            gia.GiaBanBuon = Convert.ToUInt32(double.Parse(dr["GiaBanBuon"].ToString()));
            gia.GiaBanLe = Convert.ToUInt32(double.Parse(dr["GiaBanLe"].ToString()));

            return gia;
        }
        catch
        {
            return null;
        }
    }

    public ApGia GetGiaMoiTheoID(int ID_MatHang)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Hang", ID_MatHang)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetApGiaTheoMatHang", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            ApGia gia = GetApGiaFromDataRow(dr);
            return gia;
        }
        catch
        {
            return null;
        }
    }

    public int ApGiaMoi(ApGia gia)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QuanLy", gia.ID_QuanLy),
            new SqlParameter("ID_Hang", gia.ID_Hang),
            new SqlParameter("GiaBanBuon", Convert.ToDouble(gia.GiaBanBuon)),
            new SqlParameter("GiaBanLe", Convert.ToDouble(gia.GiaBanLe)),
            new SqlParameter("TuNgay", gia.TuNgay)
        };

        try
        {
            return helper.ExecuteNonQuery("sp_QL_ApGia", pars);
        }
        catch
        {
            return 0;
        }
    }

    public int HuyApGia(int ID_Hang)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Hang", ID_Hang)
        };

        try
        {
            return helper.ExecuteNonQuery("sp_QL_HuyApGia", pars);
        }
        catch
        {
            return 0;
        }
    }
}