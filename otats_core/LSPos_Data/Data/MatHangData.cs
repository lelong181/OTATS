using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LSPos_Data.Data
{
    public class MatHangData
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();

        public MatHangData()
        {
            helper = new SqlDataHelper();
        }
        public bool CheckTrungMaMatHang(int ID_QLLH, string MaHang)
        {
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@MaHang", MaHang)
                };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_MatHang_CheckTrungTheoMaHang", Parammeter);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
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

        public bool CheckTrungServiceRateID(int ID_QLLH, string ServiceRateID)
        {
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ServiceRateID", ServiceRateID)
                };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_MatHang_CheckTrungServiceRateID", Parammeter);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
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
        public List<ComboboxDTO> ComboxboxMatHang(int ID_QLLH)
        {
            List<ComboboxDTO> combo = new List<ComboboxDTO>();

            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH)};
                DataSet ds = helper.ExecuteDataSet("sp_getComboboxMathang", Parammeter);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ComboboxDTO cb = new ComboboxDTO();
                        cb.id = int.Parse(dr["ID_Hang"].ToString());
                        cb.Name = dr["TenHang"].ToString();
                        combo.Add(cb);
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return combo;
        }
        public DataSet GetDSMatHang(int ID_DANHMUC, int ID_QLLH)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_DANHMUC", ID_DANHMUC),
                new SqlParameter("@ID_QLLH", ID_QLLH)
                };

                ds = helper.ExecuteDataSet("getDS_HangHoa_ByIdDanhMuc", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }
        public static List<DanhMucOBJ> getDS_DanhMuc(int Id_QLLH, int ID_QuanLy, string lang)
        {
            try
            {
                List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();

                string all = "All";
                if (lang == "vi")
                {
                    all = "Tất cả mặt hàng";
                }

                SqlDataHelper sql = new SqlDataHelper();
                DataSet ds = sql.ExecuteDataSet("getDSDanhMucTheoID_QLLH_v1",
                    new SqlParameter("@ID_QLLH", Id_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy)
                  );

                if (ds == null)
                {
                    return null;
                }

                DataTable dtbl = null;
                dtbl = ds.Tables[0];
                DanhMucOBJ dmTatCa = new DanhMucOBJ();

                dmTatCa.ID_DANHMUC = -1;
                dmTatCa.ID_PARENT = 0;

                DataRow[] dr1 = dtbl.Select("ID_PARENT IS NULL OR ID_PARENT = 0 ");
                int Tong = 0;
                foreach (DataRow d in dr1)
                {
                    Tong += (d["SoLuongMatHang"].ToString() != "") ? int.Parse(d["SoLuongMatHang"].ToString()) : 0;
                }
                dmTatCa.SoLuongMatHang = Tong;
                dmTatCa.TenHienThi = all;
                dmTatCa.AnhDaiDien = "FileUpload/Images/all.png";
                lstDanhMuc.Insert(0, dmTatCa);

                DanhMucOBJ dmKhac = new DanhMucOBJ();
                dmKhac.ID_DANHMUC = 0;
                dmKhac.ID_PARENT = 0;

                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    DataRow dr = dtbl.Rows[i];
                    DanhMucOBJ dm = new DanhMucOBJ();
                    dm.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                    dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                    dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                    dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                    dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                    dm.TenDanhMuc = dr["TenDanhMuc"].ToString();
                    dm.AnhDaiDien = dr["AnhDaiDien"].ToString();

                    //dm.SoLuongDanhMucCon = (dr["SoLuongDanhMucCon"].ToString() != "") ? int.Parse(dr["SoLuongDanhMucCon"].ToString()) : 0;
                    dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;
                    dm.TenHienThi = dr["TenDanhMuc"].ToString() /*+ " (" + dm.SoLuongMatHang + ")"*/;
                    
                    lstDanhMuc.Add(dm);
                }

                return lstDanhMuc;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
    }
}