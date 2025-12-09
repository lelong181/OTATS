using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class PhieuDieuChinhNhapData
    {
        private SqlDataHelper helper;
        public PhieuDieuChinhNhapData()
        {
            helper = new SqlDataHelper();
        }

        public int addPhieuDieuChinh(PhieuDieuChinhNhapKhoModel phieu)
        {
            int flag = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_PhieuDieuChinhNhapKho", phieu.ID_PhieuDieuChinhNhapKho),
                    new SqlParameter("@ID_QLLH", phieu.ID_QLLH),
                    new SqlParameter("@ID_PhieuNhap", phieu.ID_PhieuNhap),
                    new SqlParameter("@ID_NhanVien", phieu.ID_NhanVien),
                    new SqlParameter("@DienGiai", phieu.DienGiai),
                    new SqlParameter("@UpdateBy", phieu.UpdateBy),
                };

                flag = int.Parse(helper.ExecuteScalar("usp_vuongtm_PhieuDieuChinhNhapKho_add", pars).ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public int addChiTietDieuChinh(PhieuDieuChinhNhapKhoChiTietModel chitiet)
        {
            int flag = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_PhieuDieuChinhNhapChiTiet", chitiet.ID_PhieuDieuChinhNhapChiTiet),
                    new SqlParameter("@ID_PhieuDieuChinhNhap", chitiet.ID_PhieuDieuChinhNhap),
                    new SqlParameter("@ID_ChiTietPhieuNhap", chitiet.ID_ChiTietPhieuNhap),
                    new SqlParameter("@ID_HangHoa", chitiet.ID_HangHoa),
                    new SqlParameter("@SoLuong", chitiet.SoLuong),
                    new SqlParameter("@SoLuongDieuChinh", chitiet.SoLuongDieuChinh),
                    new SqlParameter("@LoaiDieuChinh", chitiet.LoaiDieuChinh),
                    new SqlParameter("@UpdateBy", chitiet.UpdateBy)
                };

                flag = int.Parse(helper.ExecuteScalar("usp_vuongtm_PhieuDieuChinhNhapKhoChiTiet_add", pars).ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public bool deletemarkmulti(int ID_PhieuDieuChinhNhap, string listID, string updateBy)
        {
            bool flag = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_PhieuDieuChinhNhap", ID_PhieuDieuChinhNhap),
                    new SqlParameter("@listID", listID),
                    new SqlParameter("@updateBy", updateBy)
                };

                if (helper.ExecuteNonQuery("usp_vuongtm_PhieuDieuChinhNhapKhoChiTiet_DeleteMarkmulti", pars) != 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public List<PhieuNhapModel> getlistphieunhap(int ID_QLLH)
        {
            List<PhieuNhapModel> list = new List<PhieuNhapModel>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_PhieuNhap_DanhSach", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    PhieuNhapModel item = new PhieuNhapModel();
                    item.ID_PhieuNhap = Convert.ToInt32(dr["ID_PhieuNhap"].ToString());
                    item.TenPhieuNhap = dr["TenPhieuNhap"].ToString();

                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }
        public List<PhieuNhapChiTietModel> getchitietphieunhap(int ID_PhieuNhap)
        {
            List<PhieuNhapChiTietModel> list = new List<PhieuNhapChiTietModel>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_PhieuNhap", ID_PhieuNhap)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_PhieuNhapChiTiet_getlist", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    PhieuNhapChiTietModel item = new PhieuNhapChiTietModel();
                    
                    item.ID_ChiTietPhieuNhap = Convert.ToInt32(dr["ID_ChiTietPhieuNhap"].ToString());
                    item.ID_PhieuNhap = Convert.ToInt32(dr["ID_PhieuNhap"].ToString());
                    item.ID_HangHoa = Convert.ToInt32(dr["ID_HangHoa"].ToString());
                    item.SoLuong = Convert.ToSingle(dr["SoLuong"].ToString());
                    
                    item.MaHang = dr["MaHang"].ToString();
                    item.TenHang = dr["TenHang"].ToString();

                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }
        public List<PhieuDieuChinhNhapKhoModel> getlistphieudieuchinh(int ID_QLLH)
        {
            List<PhieuDieuChinhNhapKhoModel> list = new List<PhieuDieuChinhNhapKhoModel>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_PhieuDieuChinhNhapKho_getlist", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    PhieuDieuChinhNhapKhoModel item = new PhieuDieuChinhNhapKhoModel();
                    item.ID_PhieuDieuChinhNhapKho = Convert.ToInt32(dr["ID_PhieuDieuChinhNhapKho"].ToString());
                    item.ID_NhanVien = Convert.ToInt32(dr["ID_NhanVien"].ToString());
                    item.ID_PhieuNhap = Convert.ToInt32(dr["ID_PhieuNhap"].ToString());
                    item.DienGiai = dr["DienGiai"].ToString();
                    item.TenPhieuNhap = dr["TenPhieuNhap"].ToString();
                    item.TenNhanVien = dr["TenNhanVien"].ToString();

                    try
                    {
                        item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }


                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }
        public List<PhieuDieuChinhNhapKhoChiTietModel> getchitietdieuchinh(int ID_PhieuDieuChinhNhapKho)
        {
            List<PhieuDieuChinhNhapKhoChiTietModel> list = new List<PhieuDieuChinhNhapKhoChiTietModel>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_PhieuDieuChinhNhapKho", ID_PhieuDieuChinhNhapKho)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_PhieuDieuChinhNhapKhoChiTiet_getlist", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    PhieuDieuChinhNhapKhoChiTietModel item = new PhieuDieuChinhNhapKhoChiTietModel();
                    item.ID_PhieuDieuChinhNhapChiTiet = Convert.ToInt32(dr["ID_PhieuDieuChinhNhapChiTiet"].ToString());
                    item.ID_PhieuDieuChinhNhap = Convert.ToInt32(dr["ID_PhieuDieuChinhNhap"].ToString());
                    item.ID_ChiTietPhieuNhap = Convert.ToInt32(dr["ID_ChiTietPhieuNhap"].ToString());
                    item.ID_HangHoa = Convert.ToInt32(dr["ID_HangHoa"].ToString());
                    item.SoLuongPhieuNhap = Convert.ToSingle(dr["SoLuongPhieuNhap"].ToString());
                    item.SoLuong = Convert.ToSingle(dr["SoLuong"].ToString());
                    item.SoLuongDieuChinh = Convert.ToSingle(dr["SoLuongDieuChinh"].ToString());
                    item.LoaiDieuChinh = Convert.ToInt32(dr["LoaiDieuChinh"].ToString());
                    item.TenHang = dr["TenHang"].ToString();
                    item.MaHang = dr["MaHang"].ToString();
                    item.TenLoaiDieuChinh = dr["TenLoaiDieuChinh"].ToString();

                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }
        public DataTable baocaodieuchinh(int ID_QLLH, int ID_QuanLy, int ID_Kho, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@ID_Kho", ID_Kho),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoDieuChinh_v1", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return dt;
        }
        public DataSet excelbaocaodieuchinh(int ID_QLLH, int ID_QuanLy, int ID_Kho, DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@ID_Kho", ID_Kho),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to)
                };

                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoDieuChinh_exel_v1", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return ds;
        }
    }
}