using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PhanQuyenNganhHang_Nhom_dl
/// </summary>
public class PhanQuyenNganhHang_Nhom_dl
{
        private SqlDataHelper helper;

    
        
    public PhanQuyenNganhHang_Nhom_dl()
    {
        helper = new SqlDataHelper();
    }
     
    public bool ThemNganhHangChoNhom(int ID_Nhom, int ID_DANHMUC)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@ID_DANHMUC",ID_DANHMUC)
        };

            if (helper.ExecuteNonQuery("sp_PhanQuyenNganhHang_Nhom_ThemMoi", pars) != 0)
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
    public bool XoaNganhHang_Nhom(int ID_Nhom)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };

            if (helper.ExecuteNonQuery("sp_PhanQuyenNganhHang_Nhom_Xoa_ByID_Nhom", pars) != 0)
            {
                // xoa thanh cong
                return true;
            }
            else
            {
                // xoa that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    public bool XoaNganhHang_Nhom(int ID_Nhom,int ID_DANHMUC)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom),
              new SqlParameter("@ID_DANHMUC", ID_DANHMUC)
        };

            if (helper.ExecuteNonQuery("sp_PhanQuyenNganhHang_Nhom_Xoa_ByID_Nhom", pars) != 0)
            {
                // xoa thanh cong
                return true;
            }
            else
            {
                // xoa that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    public List<DanhMucOBJ> GetByIDNhom(int ID_Nhom)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };

        DataSet ds = helper.ExecuteDataSet("sp_PhanQuyenNganhHang_GetBy_ID_Nhom", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DanhMucOBJ> dsChucNang = new List<DanhMucOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                DanhMucOBJ cn = new DanhMucOBJ();
                cn.ID_Nhom = ID_Nhom;
                cn.ID_DANHMUC = int.Parse(dr["ID_DANHMUC"].ToString());
                cn.TenDanhMuc = dr["TenDanhMuc"].ToString();

               dsChucNang.Add(cn);
            }

            return dsChucNang;
        }
        catch
        {
            return null;
        }
    }
   
}