using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.DataAccess
{
    public class HuongDanVienDAO
    {
        private SqlDataHelper helper;
        public HuongDanVienDAO()
        {
            helper = new SqlDataHelper();
        }
        public List<HuongDanVienModel> GetDSHDV()
        {
            List<HuongDanVienModel> dsnv = new List<HuongDanVienModel>();
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_NhanVien_GetDSHDV");
                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    HuongDanVienModel nv = GetObjectFromDataRowUtil<HuongDanVienModel>.ToOject(dr);
                    dsnv.Add(nv);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dsnv;
        }

        public HuongDanVienModel GetHDV_ByMaThe(string MaTheHDV)
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@MaTheHDV", MaTheHDV)
            };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_NhanVien_GetByMaThe", pars);
                DataTable dt = ds.Tables[0];
                HuongDanVienModel dsnv = new HuongDanVienModel();

                foreach (DataRow dr in dt.Rows)
                {
                    dsnv = GetObjectFromDataRowUtil<HuongDanVienModel>.ToOject(dr);
                    return dsnv;

                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public HuongDanVienModel GetHDV_ByID(int ID)
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID", ID)
            };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_HuongDanVien_GetByID", pars);
                DataTable dt = ds.Tables[0];
                HuongDanVienModel dsnv = new HuongDanVienModel();

                foreach (DataRow dr in dt.Rows)
                {
                    dsnv = GetObjectFromDataRowUtil<HuongDanVienModel>.ToOject(dr);
                    return dsnv;

                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }

        public int ThemHuongDanVien(HuongDanVienModel nhanVienModels)
        {
            int re = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_NhanVien", nhanVienModels.ID),
                    new SqlParameter("@TenNhanVien", nhanVienModels.TenDayDu),
                    new SqlParameter("@TenDangNhap", nhanVienModels.TenDangNhap),
                    new SqlParameter("@MatKhau", Utils.md5(nhanVienModels.MatKhau)),
                     new SqlParameter("@Email", nhanVienModels.Email),
                    new SqlParameter("@DienThoai", nhanVienModels.DienThoai),
                    new SqlParameter("@MaTheHDV", nhanVienModels.MaTheHDV),
                    new SqlParameter("@CCCD", nhanVienModels.CCCD),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_NhanVien_ThemHuongDanVien", pars);
                DataTable dt = ds.Tables[0];
                re = int.Parse(dt.Rows[0]["re"].ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return re;
        }

        public bool ActiveHuongDanVien(int ID_NhanVien)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_NhanVien", ID_NhanVien)
                    };

                int ra = helper.ExecuteNonQuery("sp_NhanVien_ActiveHuongDanVien", pars);
                if (ra > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public bool InactiveHuongDanVien(int ID_NhanVien)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_NhanVien", ID_NhanVien)
                    };

                int ra = helper.ExecuteNonQuery("sp_NhanVien_InactiveHuongDanVien", pars);
                if (ra > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }
    }
}
