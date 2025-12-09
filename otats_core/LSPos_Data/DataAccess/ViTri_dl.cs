using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ViTri_dl
/// </summary>
/// 


public class ViTri_dl
{

    public static SqlDataHelper db = new SqlDataHelper();

	public ViTri_dl()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static List<ToaDo> GetViTriDinhKyNV(int IDQL, int IDNV, DateTime TuNgay, DateTime DenNgay)
    {
        try
        {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", IDQL),
            new SqlParameter("@IDNV", IDNV),
            new SqlParameter("@TuNgay", TuNgay),
            new SqlParameter("@DenNgay", DenNgay)
        };
        DataTable dt = new DataTable();

        dt = db.ExecuteDataSet("getViTriDinhKy", pars).Tables[0];

       
            List<ToaDo> dstd = new List<ToaDo>();
            foreach (DataRow dr in dt.Rows)
            {
                ToaDo td = new ToaDo();
                td.KinhDo = double.Parse(dr["KinhDo"].ToString());
                td.ViDo = double.Parse(dr["ViDo"].ToString());
                td.UpdatedTime = DateTime.Parse(dr["ReceivedTime"].ToString());
                dstd.Add(td);
            }

            return dstd;
        }
        catch
        {
            return null;
        }
    }

    public static gpsnhanvien GetViTriHienTaiNV(int idnhanvien)
    {
        gpsnhanvien td = new gpsnhanvien();
        DataTable dt = new DataTable();
        

        try
        {
            SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@IDNhanVien", idnhanvien),
            };

            dt = db.ExecuteDataSet("getViTriOnlineTheoIDNV", param).Tables[0];
            DataRow dr = dt.Rows[0];

            td.KinhDo = (dr["KinhDo"] == null || dr["KinhDo"].ToString().Trim() == "") ? 0 : double.Parse(dr["KinhDo"].ToString());
            td.ViDo = (dr["ViDo"] == null || dr["ViDo"].ToString().Trim() == "") ? 0 : double.Parse(dr["ViDo"].ToString());
            td.thoigiancapnhat = (dr["ReceivedTime"] == null || dr["ReceivedTime"].ToString().Trim() == "") ? "" : DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
            td.tennhanvien = NhanVien_dl.TenNhanVien(idnhanvien);
            return td;
        }
        catch(Exception ex)
        {
            return null;
        }
    }
}

public class ToaDo
{
    public double KinhDo { get; set; }
    public double ViDo { get; set; }
    public DateTime UpdatedTime { get; set; }
}