using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class PhieuTonDauData
    {
        private SqlDataHelper helper;
        public PhieuTonDauData()
        {
            helper = new SqlDataHelper();
        }

        public int addPhieuTonDau(PhieuTonDauModel phieu)
        {
            int flag = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_PhieuTonDau", phieu.ID_PhieuTonDau),
                    new SqlParameter("@ID_QLLH", phieu.ID_QLLH),
                    new SqlParameter("@ID_NhanVien", phieu.ID_NhanVien),
                    new SqlParameter("@ID_KhoHang", phieu.ID_KhoHang),
                    new SqlParameter("@NgayChotTon", phieu.NgayChotTon),
                    new SqlParameter("@UpdateBy", phieu.UpdateBy),
                };

                flag = int.Parse(helper.ExecuteScalar("usp_vuongtm_PhieuTonDau_add", pars).ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public int addChiTietTonDau(PhieuTonDauChiTietModel chitiet)
        {
            int flag = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_ChiTietPhieuTonDau", chitiet.ID_ChiTietPhieuTonDau),
                    new SqlParameter("@ID_PhieuTonDau", chitiet.ID_PhieuTonDau),
                    new SqlParameter("@ID_HangHoa", chitiet.ID_HangHoa),
                    new SqlParameter("@SoLuong", chitiet.SoLuong),
                    new SqlParameter("@UpdateBy", chitiet.UpdateBy)
                };

                flag = int.Parse(helper.ExecuteScalar("usp_vuongtm_PhieuTonDauChiTiet_add", pars).ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public bool deletemarkmulti(int ID_PhieuTonDau, string listID, string updateBy)
        {
            bool flag = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_PhieuTonDau", ID_PhieuTonDau),
                    new SqlParameter("@listID", listID),
                    new SqlParameter("@updateBy", updateBy)
                };

                if (helper.ExecuteNonQuery("usp_vuongtm_PhieuTonDauChiTiet_DeleteMarkmulti", pars) != 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public DataSet getphieutondau(int id_QLLH, int idKho)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@id_QLLH", id_QLLH),
                    new SqlParameter("@idKho", idKho)
                };

                ds = helper.ExecuteDataSet("usp_vuongtm_PhieuTonDau_getbykho", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return ds;
        }
        public int getidphieutondau(int id_QLLH, int idKho)
        {
            int id = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@id_QLLH", id_QLLH),
                    new SqlParameter("@idKho", idKho)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_PhieuTonDau_getbykho", pars);
                DataTable dtTonDau = ds.Tables[0];
                foreach (DataRow dr in dtTonDau.Rows)
                {

                    id = Convert.ToInt32(dr["ID_PhieuTonDau"].ToString());
                    break;
                }

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return id;
        }
        public int getidphieuChiTiettondau(int id_QLLH, int ID_HangHoa)
        {
            int id = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@id_QLLH", id_QLLH),
                    new SqlParameter("@ID_HangHoa", ID_HangHoa)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_getChiTietPhieuTonDau", pars);
                DataTable dtTonDau = ds.Tables[0];
                foreach (DataRow dr in dtTonDau.Rows)
                {

                    id = Convert.ToInt32(dr["ID_ChiTietPhieuTonDau"].ToString());
                    break;
                }

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return id;
        }
        public DataSet _GettemplateTonDau( int ID_QLLH)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                    };
                DataSet ds = helper.ExecuteDataSet("sp_QL_Getdata_TemplateTonDau_Kendo", pars);

                return ds;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
    }
}