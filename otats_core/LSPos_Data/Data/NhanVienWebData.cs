using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class NhanVienWebData
    {
        private SqlDataHelper helper;
        public NhanVienWebData()
        {
            helper = new SqlDataHelper();
        }

        public List<NhomModels> getlistnhomphanquyen(int ID_QLLH, int ID_QuanLy, int ID_TaiKhoan)
        {
            List<NhomModels> list = new List<NhomModels>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@ID_TaiKhoan", ID_TaiKhoan),
                    };

                DataSet ds = helper.ExecuteDataSet("vuongtm_nhanvienweb_getlistnhom", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    NhomModels dm = new NhomModels();
                    dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                    dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                    dm.TenNhom = dr["TenNhom"].ToString();
                    dm.MaNhom = dr["MaNhom"].ToString();
                    dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                    dm.isChecked = ((dr["Checked"].ToString() != "") ? int.Parse(dr["Checked"].ToString()) : 0) == 1;
                    list.Add(dm);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }

        public int ThemQuanLyCon(NhanVienWebModels obj)
        {
            var id = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("idcongty", obj.ID_QLLH),
                    new SqlParameter("taikhoan", obj.TenDangNhap),
                    new SqlParameter("matkhau", Utils.md5(obj.MatKhau)),
                    new SqlParameter("ID_Cha", 0),
                    new SqlParameter("TenAdmin", obj.TenDayDu),
                    new SqlParameter("Email", obj.Email),
                    new SqlParameter("Phone", obj.Phone),
                    };

                if(helper.ExecuteNonQuery("sp_QL_ThemQuanLyCon", pars) > 0)
                {
                    SqlParameter[] parGet = new SqlParameter[]{
                        new SqlParameter("idcongty", obj.ID_QLLH),
                        new SqlParameter("taikhoan", obj.TenDangNhap)
                        };
                    object objId = helper.ExecuteScalar("sp_QL_GetQlyByUsername", parGet);
                    id = int.Parse(objId.ToString());
                }
            }
            catch(Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return id;
        }
        public int SuaQuanLyCon(NhanVienWebModels obj)
        {
            var id = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("idcongty", obj.ID_QLLH),
                    new SqlParameter("taikhoan", obj.TenDangNhap),
                    new SqlParameter("TenAdmin", obj.TenDayDu),
                    };

                id = helper.ExecuteNonQuery("sp_QL_SuaQuanLyCon", pars);
                
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return id;
        }
    }
}