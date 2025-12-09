using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DiaGioiHanhChinhDB
/// </summary>
public class DiaGioiHanhChinhDB
{

    public static SqlDataHelper db = new SqlDataHelper();

    public DiaGioiHanhChinhDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<DiaGioiHanhChinhOBJ> DanhSachDiaGioiHanhChinh(int idcha)
    {
        List<DiaGioiHanhChinhOBJ> rs = new List<DiaGioiHanhChinhOBJ>();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("idcha", idcha)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_DanhSachDiaGioiHanhChinh", pars).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(new DiaGioiHanhChinhOBJ
                {
                    id = int.Parse(dr["id"].ToString()),
                    idcha = int.Parse(dr["idcha"].ToString()),
                    ten = dr["ten"].ToString(),
                    kinhdo = double.Parse(dr["kinhdo"].ToString()),
                    vido = double.Parse(dr["vido"].ToString())
                });
            }
        }
        catch { }
        return rs;
    }

    public static List<DiaGioiHanhChinhOBJ> TimKiemTheoTen(string input)
    {
        List<DiaGioiHanhChinhOBJ> rs = new List<DiaGioiHanhChinhOBJ>();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@input", input)
            };
            DataTable dt = db.ExecuteDataSet("[sp_QL_TimKiemTinh]", pars).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    rs.Add(new DiaGioiHanhChinhOBJ
                    {
                        id = int.Parse(dr["id"].ToString()),
                        idcha = int.Parse(dr["idcha"].ToString()),
                        ten = dr["ten"].ToString(),
                        kinhdo = (dr["kinhdo"] != null) ? double.Parse(dr["kinhdo"].ToString()) : 0,
                        vido = (dr["vido"] != null) ? double.Parse(dr["vido"].ToString()) : 0
                    });
                }
                catch
                {

                }
            }
        }
        catch { }
        return rs;
    }
}