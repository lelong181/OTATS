using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHang_dl
/// </summary>
public class Quyen_dl
{
    private SqlDataHelper helper;

    public Quyen_dl()
	{
		//
		// TODO: Add constructor logic here
		//
        helper = new SqlDataHelper();
	}

    public Quyen GetQuyenFromDataRow(DataRow dr)
    {
        try
        {
            Quyen quyen = new Quyen();
            quyen.ID_Quyen = int.Parse(dr["ID_Quyen"].ToString());
            quyen.TenQuyen = dr["TenQuyen"].ToString();

            return quyen;
        }
        catch
        {
            return null;
        }
    }

    public List<Quyen> GetAllQuyen()
    {
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetQuyen", null);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<Quyen> dsQuyen = new List<Quyen>();
            foreach (DataRow dr in dt.Rows)
            {
                Quyen quyen = GetQuyenFromDataRow(dr);
                dsQuyen.Add(quyen);
            }

            return dsQuyen;
        }
        catch
        {
            return null;
        }
    }
    public bool CheckQuyen(int ID_NhanVien, int ID_KhachHang, int ID_Quyen)
    {
        bool ketqua = false;
        try
        {

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_NhanVien", ID_NhanVien),
                new SqlParameter("@ID_KhachHang", ID_KhachHang)
                
            };

            DataTable dt = helper.ExecuteDataSet("sp_QL_GetPhanQuyen", param).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                string Idquyen = dr["ID_Quyen"].ToString();
                if (Idquyen.Contains(";" + ID_Quyen + ";"))
                {
                    ketqua = true;
                }
            }

            return ketqua;
        }
        catch (Exception ex)
        {
            
            return ketqua;
        }
    }



}