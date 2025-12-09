using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class UserinforData
    {
        private SqlDataHelper helper;
        public UserinforData()
        {
            helper = new SqlDataHelper();
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

        public bool ResetPassword(int IDQLLH, int id_NhanVien, string matkhau)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", IDQLLH),
            new SqlParameter("@id_NhanVien", id_NhanVien),
            new SqlParameter("@matkhau", md5(matkhau))
        };
            try
            {
                if (helper.ExecuteNonQuery("Business_Dongnn_ResetPass", pars) != 0)
                {
                    // cap nhat thanh cong
                    return true;
                }
                else
                {
                    // cap nhat that bai
                    return false;

                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
        public UserInfo GetUserInfo(string username, string macongty)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("username", username),
                new SqlParameter("macongty", macongty)
            };

            DataSet ds = helper.ExecuteDataSet("GetUserQLInfo_NhanVien", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;

            DataRow dr = dt.Rows[0];
            UserInfo u = new UserInfo(int.Parse(dr["ID_NhanVien"].ToString()),
                dr["TenDangNhap"].ToString(),
                int.Parse(dr["ID_QLLH"].ToString()),
                2, dr["TenNhanVien"].ToString(),
                false, 0, dr["DanhSachNhom"].ToString(), -1, dr["icon_path"].ToString(), (dr["XuatLoTrinhDiaChi"].ToString() != "" ? int.Parse(dr["XuatLoTrinhDiaChi"].ToString()) : 0),
                (dr["QuyenNhapChiTietMatHang"].ToString() != "" ? int.Parse(dr["QuyenNhapChiTietMatHang"].ToString()) : 0), dr["MauInDonHang"].ToString(),
                (dr["XuatDanhSachDonHangTheoMau"].ToString() != "" ? int.Parse(dr["XuatDanhSachDonHangTheoMau"].ToString()) : 0),
                dr["DinhDang_NgayHienThi"].ToString(), (dr["DinhDangTienSoThapPhan"].ToString() != "" ? int.Parse(dr["DinhDangTienSoThapPhan"].ToString()) : 0), 0);
            u.IsHDV = int.Parse(dr["IsHDV"].ToString()) == 1 ? true : false;

            return u;
        }

        public bool UserLogInNhanVien(string username, string password, string macongty)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("macongty", macongty),
                    new SqlParameter("username", username),
                    new SqlParameter("password", password),
                    new SqlParameter("passwordmd5", md5(password))
                    };

                DataSet ds = helper.ExecuteDataSet("CheckUserQLLogIn_NhanVien", pars);
                DataTable dt = ds.Tables[0];

                return (dt.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool UserLogIn(string username, string password, int ID_QLLH)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("username", username),
                    new SqlParameter("password", password),
                    new SqlParameter("passwordmd5", md5(password))
                    };

                DataSet ds = helper.ExecuteDataSet("CheckUserQLLogIn_v1", pars);
                DataTable dt = ds.Tables[0];

                return (dt.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool CheckTonTaiUserWeb(string username, int ID_QLLH)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@username", username)
                    };

                DataSet ds = helper.ExecuteDataSet("CheckTonTaiUserWeb", pars);
                DataTable dt = ds.Tables[0];

                return (dt.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
        public bool ChangeUserPassword(string username, string newpassword, int idcongty)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@idcongty", idcongty),
                new SqlParameter("username", username),
                new SqlParameter("password", md5(newpassword))
                };

                if (helper.ExecuteNonQuery("DoiMKUserQL_v1", pars) != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool ChangeLang(int id, string lang)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@id", id),
                new SqlParameter("lang", lang)
                };

                if (helper.ExecuteNonQuery("sp_User_Changelang", pars) != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }


        public ChucNangOBJ CheckQuyen(string url, int ID_QuanLy)
        {
            ChucNangOBJ dv = new ChucNangOBJ();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@url", url),
                    new SqlParameter("ID_QuanLy", ID_QuanLy)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_ChucNang_CheckQuyenByUrlNew", pars);
                DataRow dr = ds.Tables[0].Rows[0];

                try
                {
                    dv.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
                    dv.TenChucNang = dr["TenChucNang"].ToString();
                    dv.Xoa = dr["Xoa"].ToString() != "" ? int.Parse(dr["Xoa"].ToString()) : 0;
                    dv.Them = dr["Them"].ToString() != "" ? int.Parse(dr["Them"].ToString()) : 0;
                    dv.Sua = dr["Sua"].ToString() != "" ? int.Parse(dr["Sua"].ToString()) : 0;
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }

                try
                {
                    //dv.MaChucNang = dr["MaChucNang"].ToString();
                    //dv.URL = dr["URL"].ToString();
                    //dv.InsertedTime = DateTime.Parse(dr["InsertedTime"].ToString());
                    //dv.TenNhomChucNang = dr["TenNhomChucNang"].ToString();
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dv;
        }
    }
}