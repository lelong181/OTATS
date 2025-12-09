using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for User_dl
/// </summary>
public class User_dl
{
    private SqlDataHelper helper;
    public User_dl()
    {
        helper = new SqlDataHelper();
    }

    public string checkSoLuongAccount(int ID_QLLH)
    {
        SqlParameter rs = new SqlParameter();
        rs.Direction = ParameterDirection.Output;
        rs.DbType = DbType.Int32;
        rs.ParameterName = "rs";

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            rs
        };

        if (helper.ExecuteNonQuery("sp_QL_CheckSoLuongAccount", pars) != 0)
        {
            return rs.Value.ToString();
        }
        else
        {
            return "-1";
        }
    }

    public bool UserLogIn(string username, string password)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("username", username),
            new SqlParameter("password", password),
            new SqlParameter("passwordmd5", md5(password))
        };

            DataSet ds = helper.ExecuteDataSet("CheckUserQLLogIn", pars);
            DataTable dt = ds.Tables[0];

            return (dt.Rows.Count > 0);
        }
        catch (Exception ex)
        {

            return false;
        }
    }
    public bool UserLogIn(string username, string password, string macongty)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("macongty", macongty),
            new SqlParameter("username", username),
            new SqlParameter("password", password),
            new SqlParameter("passwordmd5", md5(password))
        };

            DataSet ds = helper.ExecuteDataSet("CheckUserQLLogIn", pars);
            DataTable dt = ds.Tables[0];

            return (dt.Rows.Count > 0);
        }
        catch (Exception ex)
        {

            return false;
        }
    }

    public bool AdminLogIn(string username, string password)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("username", username),
            new SqlParameter("password", password),
            new SqlParameter("passwordmd5", md5(password))
        };
            DataSet ds = helper.ExecuteDataSet("CheckUserAdminLogIn_2", pars);
            DataTable dt = ds.Tables[0];

            return (dt.Rows.Count > 0);
        }
        catch (Exception ex)
        {

            return false;
        }
    }
    public List<UserInfo> GetListUserInfo(int ID_QuanLy)
    {
        List<UserInfo> dsql = new List<UserInfo>();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("GetAllUserQLInfo", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        foreach (DataRow dr in dt.Rows)
        {

            UserInfo user = new UserInfo(int.Parse(dr["idtaikhoan"].ToString()), dr["taikhoan"].ToString(), int.Parse(dr["idcongty"].ToString()), int.Parse(dr["level"].ToString()), dr["TenAdmin"].ToString(), false, (dr["ID_Cha"].ToString() != "" ? int.Parse(dr["ID_Cha"].ToString()) : 0), dr["DanhSachNhom"].ToString(), 0, "", 0, 0, "", 0, "", 0, 0);
            user.Email = dr["Email"].ToString();
            user.Phone = dr["Phone"].ToString();
            dsql.Add(user);
        }

        return dsql;

    }
    public List<UserInfo> GetListUserByNhom(int ID_Nhom, int ID_QLLH)
    {
        List<UserInfo> dsql = new List<UserInfo>();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Nhom", ID_Nhom),
            new SqlParameter("ID_QLLH", ID_QLLH)
        };

        DataSet ds = helper.ExecuteDataSet("GetAllUserQLInfo_TheoIDNhom", pars);
        DataTable dt = ds.Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            UserInfo user = new UserInfo(int.Parse(dr["idtaikhoan"].ToString()), dr["taikhoan"].ToString(), int.Parse(dr["idcongty"].ToString()), int.Parse(dr["level"].ToString()), dr["TenAdmin"].ToString(), false, (dr["ID_Cha"].ToString() != "" ? int.Parse(dr["ID_Cha"].ToString()) : 0), dr["DanhSachNhom"].ToString(), 0, "", 0, 0, "", 0, "", 0, 0);
            user.Email = dr["Email"].ToString();
            user.Phone = dr["Phone"].ToString();
            dsql.Add(user);
        }
        //List<NhomOBJ> lstNhomCon = NhomDB.getDS_NhomCon_ById(ID_Nhom);
        //if (lstNhomCon != null)
        //{
        //    foreach (NhomOBJ nhom in lstNhomCon)
        //    {
        //        List<UserInfo> lstNV = GetListUserByNhom(nhom.ID_Nhom, ID_QLLH);
        //        if (lstNV != null)
        //        {
        //            foreach (UserInfo n in lstNV)
        //            {
        //                dsql.Add(n);
        //            }
        //        }
        //    }
        //}
        return dsql;

    }
    public List<UserInfo> GetListUserInfoTheoTaiKhoan(int ID_QuanLy, int ID_TaiKhoan)
    {
        List<UserInfo> dsql = new List<UserInfo>();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QuanLy),
            new SqlParameter("ID_TaiKhoan", ID_TaiKhoan)
        };

        DataSet ds = helper.ExecuteDataSet("GetAllUserQLInfoTheoIDTaiKhoan", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        foreach (DataRow dr in dt.Rows)
        {
            UserInfo user = new UserInfo(int.Parse(dr["ID"].ToString()), "", 0, 0, dr["NAME"].ToString(), false, int.Parse(dr["ParentID"].ToString()), "", 0, "", 0, 0, "", 0, "", 0, 0);
            dsql.Add(user);
        }

        return dsql;

    }

    public UserInfo GetUserInfo(string username, string macongty)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("username", username),
             new SqlParameter("macongty", macongty)
        };

        DataSet ds = helper.ExecuteDataSet("GetUserQLInfo", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        DataRow dr = dt.Rows[0];
        UserInfo u = new UserInfo(int.Parse(dr["idtaikhoan"].ToString()),
            dr["taikhoan"].ToString(),
            int.Parse(dr["idcongty"].ToString()),
            int.Parse(dr["level"].ToString()),
            dr["TenAdmin"].ToString(),
            false, (dr["ID_Cha"].ToString() != "" ? int.Parse(dr["ID_Cha"].ToString()) : 0), dr["DanhSachNhom"].ToString(), 0, dr["icon_path"].ToString(), (dr["XuatLoTrinhDiaChi"].ToString() != "" ? int.Parse(dr["XuatLoTrinhDiaChi"].ToString()) : 0), (dr["QuyenNhapChiTietMatHang"].ToString() != "" ? int.Parse(dr["QuyenNhapChiTietMatHang"].ToString()) : 0), dr["MauInDonHang"].ToString(), (dr["XuatDanhSachDonHangTheoMau"].ToString() != "" ? int.Parse(dr["XuatDanhSachDonHangTheoMau"].ToString()) : 0), dr["DinhDang_NgayHienThi"].ToString(), (dr["DinhDangTienSoThapPhan"].ToString() != "" ? int.Parse(dr["DinhDangTienSoThapPhan"].ToString()) : 0), 0);
        return u;
    }
    public UserInfo GetUserInfo(int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QuanLy", ID_QuanLy),

        };

        DataSet ds = helper.ExecuteDataSet("GetUserQLInfo_ByID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        DataRow dr = dt.Rows[0];
        UserInfo user = new UserInfo(int.Parse(dr["idtaikhoan"].ToString()), dr["taikhoan"].ToString(), int.Parse(dr["idcongty"].ToString()), int.Parse(dr["level"].ToString()), dr["TenAdmin"].ToString(), false, (dr["ID_Cha"].ToString() != "" ? int.Parse(dr["ID_Cha"].ToString()) : 0), dr["DanhSachNhom"].ToString(), 0, dr["icon_path"].ToString(), 0, 0, "", 0, "", 0, 0);
        user.Email = dr["Email"].ToString();
        user.Phone = dr["Phone"].ToString();
        return user;
    }
    public UserInfo GetAdminInfo(string username)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("username", username)
        };

        DataSet ds = helper.ExecuteDataSet("GetAdminInfo", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        DataRow dr = dt.Rows[0];
        // return new UserInfo(0, dr["TaiKhoan"].ToString(), 0, 1, dr["TenAdmin"].ToString(), true, 0, "", int.Parse(dr["CapDo"].ToString()), "");
        return new UserInfo(int.Parse(dr["IDAdmin"].ToString()), dr["TaiKhoan"].ToString(), 1, 1, dr["TenAdmin"].ToString(), true, 0, "", int.Parse(dr["CapDo"].ToString()), "", 0, 0, "", 0, "", 0, 0);
    }
    public List<NhomOBJ> GetDanhSachNhomQuanLy(int ID_QuanLy)
    {
        List<NhomOBJ> dsNhom = new List<NhomOBJ>();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_TaiKhoan_GetDanhSachNhomQuanLy", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;
        foreach (DataRow dr in dt.Rows)
        {
            NhomOBJ nhom = new NhomOBJ();
            NhomOBJ dm = new NhomOBJ();
            dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
            dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
            dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
            dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
            dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
            dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
            dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;
            dm.TenNhom = dr["TenNhom"].ToString();
            dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
            dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";
            dsNhom.Add(dm);

        }
        return dsNhom;

    }
    public bool XoaUser(int ID_QLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        return helper.ExecuteNonQuery("sp_QL_XoaUser", pars) > 0;
    }

    public bool ChangeUserPassword(string username, string newpassword)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("username", username),
                new SqlParameter("password", md5(newpassword))
            };

            if (helper.ExecuteNonQuery("DoiMKUserQL", pars) != 0)
            {
                // update thanh cong
                return true;
            }
            else
            {
                // update that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    public bool Check_RequestMapService(int ID_QLLH, DateTime ngay)
    {
        bool checkOK = false;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("Ngay", ngay.ToString("yyyy-MM-dd"))
        };

        object obj = helper.ExecuteScalar("sp_ThongKeRequestMapService_ThemMoi", pars);
        int id = int.Parse(obj.ToString());
        if (id > 0)
        {
            checkOK = true;
        }
        return checkOK;

    }
    public static byte[] encryptData(string data)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashedBytes;
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
        return hashedBytes;
    }
    public static string md5(string data)
    {
        return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
    }

    public List<TaiKhoanOBJ> GetDSTaiKhoan(int ID_QLLH)
    {
        try
        {
            List<TaiKhoanOBJ> lstk = new List<TaiKhoanOBJ>();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH)
            };
            DataSet ds = helper.ExecuteDataSet("getDSTaiKhoan_TheoID_QLLH", pars);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                TaiKhoanOBJ tk = new TaiKhoanOBJ();
                tk.TenTaiKhoan = row["TenAdmin"].ToString();
                tk.IDTaiKhoan = int.Parse(row["idtaikhoan"].ToString());
                lstk.Add(tk);
            }

            return lstk;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public DataSet BaoCaoLichSuThayDoi(int IDQLLH, int ID_TaiKhoan, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", IDQLLH),
            new SqlParameter("@ID_TaiKhoan", ID_TaiKhoan),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@From", dtFrom),
            new SqlParameter("@To", dtTo.ToString("yyyy-MM-dd 23:59:59"))

        };
        try
        {
            ds = helper.ExecuteDataSet("sp_web_BaoCaoLichSuThaoTac", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public bool ChangeAdminUserPassword(string username, string newpassword)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("username", username),
                new SqlParameter("password", md5(newpassword))
            };

            if (helper.ExecuteNonQuery("DoiMKAdmin", pars) != 0)
            {
                // update thanh cong
                return true;
            }
            else
            {
                // update that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }


    public DataTable GetDanhSachTaiKhoanQuanTri()
    {


        DataSet ds = helper.ExecuteDataSet("sp_TaiKhoanAdmin_GetAll");
        DataTable dt = ds.Tables[0];
        return dt;

    }
}