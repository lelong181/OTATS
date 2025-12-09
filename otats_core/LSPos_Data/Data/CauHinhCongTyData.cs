using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class CauHinhCongTyData
    {
        private SqlDataHelper helper;
        public CauHinhCongTyData()
        {
            helper = new SqlDataHelper();
        }

        public CauHinhCongTyModel get(int id_QLLH)
        {
            CauHinhCongTyModel ch = new CauHinhCongTyModel();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("id_QLLH", id_QLLH)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_getcauhinhcongty", pars);
                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    ch.tenCongTy = dr["tenCongTy"].ToString();
                    ch.diaChi = dr["diaChi"].ToString();
                    ch.dienThoai = dr["dienThoai"].ToString();
                    ch.email = dr["email"].ToString();
                    ch.soPhutVaoDiemToiThieu = Convert.ToInt32(dr["soPhutVaoDiemToiThieu"].ToString());
                    ch.thoiGianThongBaoGiaMoi = Convert.ToInt32(dr["thoiGianThongBaoGiaMoi"].ToString());
                    ch.dinhDangSo = Convert.ToInt32(dr["dinhDangSo"].ToString());
                    ch.iconPath = dr["iconPath"].ToString();
                    ch.suDungBangGiaLoaiKhachHang = Convert.ToBoolean(dr["suDungBangGiaLoaiKhachHang"].ToString());
                    ch.SoChuSoThapPhan = Convert.ToInt32(dr["SoChuSoThapPhan"].ToString());
                    break;
                }

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ch;
        }

        public bool update(int id_QLLH, CauHinhCongTyModel ch)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@id_QLLH", id_QLLH),
                    new SqlParameter("@tenCongTy", ch.tenCongTy),
                    new SqlParameter("@diaChi", ch.diaChi),
                    new SqlParameter("@dienThoai", ch.dienThoai),
                    new SqlParameter("@email", ch.email),
                    new SqlParameter("@soPhutVaoDiemToiThieu", ch.soPhutVaoDiemToiThieu),
                    new SqlParameter("@thoiGianThongBaoGiaMoi", ch.thoiGianThongBaoGiaMoi),
                    new SqlParameter("@dinhDangSo", ch.dinhDangSo),
                    new SqlParameter("@suDungBangGiaLoaiKhachHang", ch.suDungBangGiaLoaiKhachHang)
                    };

                if (helper.ExecuteNonQuery("usp_vuongtm_capnhatcauhinhcongty", pars) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
    }
}