using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PhanQuyenChucNang_Nhom_dl
/// </summary>
public class PhanQuyenChucNang_Nhom_dl
{
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
        public ChucNangOBJ GetKhachHangFromDataRow(DataRow dr)
        {
            ChucNangOBJ cn = new ChucNangOBJ();
            try
            {
                try
                {
                    cn.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
                    cn.TenChucNang = dr["TenChucNang"].ToString();
                    cn.URL = dr["URL"].ToString();
                    cn.InsertedTime = DateTime.Parse(dr["InsertedTime"].ToString());
                }
                catch (Exception)
                {
                }

            }
            catch
            {
                return null;
            }
            return cn;
        }
    public PhanQuyenChucNang_Nhom_dl()
    {
        helper = new SqlDataHelper();
    }
    public bool ThemChucNangChoNhom_Quyen(int ID_Nhom, int ID_ChucNang, int Them,int Sua, int Xoa)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@ID_ChucNang",ID_ChucNang),
            new SqlParameter("@Them",Them),
            new SqlParameter("@Sua",Sua),
            new SqlParameter("@Xoa",Xoa)
        };

            if (helper.ExecuteNonQuery("sp_Insert_PQChucNang_Nhom_v2", pars) != 0)
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
    public bool ThemChucNangChoNhom(int ID_Nhom, int ID_ChucNang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@ID_ChucNang",ID_ChucNang)
        };

            if (helper.ExecuteNonQuery("sp_Insert_PQChucNang_Nhom", pars) != 0)
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
    public bool DeleteChucNang_Nhom(int ID_Nhom)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };

            if (helper.ExecuteNonQuery("sp_Delete_PQChucNang_Nhom", pars) != 0)
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
    public List<ChucNangOBJ> GetChucNangIDNhom(int ID_Nhom)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetChucNangTheoIDNhom", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChucNangOBJ> dsChucNang = new List<ChucNangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                ChucNangOBJ cn = new ChucNangOBJ();

                cn.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
                cn.Them = dr["Them"].ToString() != "" ? int.Parse(dr["Them"].ToString()) : 0;
                cn.Sua = dr["Sua"].ToString() != "" ? int.Parse(dr["Sua"].ToString()) : 0;
                cn.Xoa = dr["Xoa"].ToString() != "" ? int.Parse(dr["Xoa"].ToString()) : 0;

                dsChucNang.Add(cn);
            }

            return dsChucNang;
        }
        catch
        {
            return null;
        }
    }
    public List<ChucNangOBJ> GetChucNangIDNhom_LoaiThietBi(int ID_Nhom, int LoaiThietBi)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@LoaiThietBi", LoaiThietBi)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetChucNangTheoIDNhom", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChucNangOBJ> dsChucNang = new List<ChucNangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                ChucNangOBJ cn = new ChucNangOBJ();
                cn.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
                dsChucNang.Add(cn);
            }

            return dsChucNang;
        }
        catch
        {
            return null;
        }
    }

    public List<ChucNangOBJ> GetChucNangID_QuanLy(int ID_Quanly)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Quanly", ID_Quanly)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetChucNangTheoIDQuanLy", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChucNangOBJ> dsChucNang = new List<ChucNangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                ChucNangOBJ dv = new ChucNangOBJ();
                dv.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
                dv.TenChucNang = dr["TenChucNang"].ToString();
                dv.TenNhomChucNang = dr["TenNhomChucNang"].ToString();
                dv.MaChucNang = dr["MaChucNang"].ToString();
                dv.URL = dr["URL"].ToString();
                dv.InsertedTime = DateTime.Parse(dr["InsertedTime"].ToString());
                try
                {
                    dv.Sua = dr["Sua"].ToString() != "" ? int.Parse(dr["Sua"].ToString()) : 0;
                    dv.Them = dr["Them"].ToString() != "" ? int.Parse(dr["Them"].ToString()) : 0;
                    dv.Xoa = dr["Xoa"].ToString() != "" ? int.Parse(dr["Xoa"].ToString()) : 0;
                }
                catch (Exception ex)
                {


                }

                dsChucNang.Add(dv);
            }

            return dsChucNang;
        }
        catch
        {
            return null;
        }
    }
    public List<ChucNangOBJ> GetChucNangID_QuanLy_v2(int ID_Quanly)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Quanly", ID_Quanly)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetChucNangTheoIDQuanLy_v2", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChucNangOBJ> dsChucNang = new List<ChucNangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                ChucNangOBJ dv = new ChucNangOBJ();
                dv.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
                dv.TenChucNang = dr["TenChucNang"].ToString();
                dv.TenNhomChucNang = dr["TenNhomChucNang"].ToString();
                dv.MaChucNang = dr["MaChucNang"].ToString();
                dv.URL = dr["URL"].ToString();
                dv.InsertedTime = DateTime.Parse(dr["InsertedTime"].ToString());
                try
                {
                    dv.Sua = dr["Sua"].ToString() != "" ? int.Parse(dr["Sua"].ToString()) : 0;
                    dv.Them = dr["Them"].ToString() != "" ? int.Parse(dr["Them"].ToString()) : 0;
                    dv.Xoa = dr["Xoa"].ToString() != "" ? int.Parse(dr["Xoa"].ToString()) : 0;
                }
                catch (Exception ex)
                {

                    
                }
                dsChucNang.Add(dv);
            }

            return dsChucNang;
        }
        catch
        {
            return null;
        }
    }
}