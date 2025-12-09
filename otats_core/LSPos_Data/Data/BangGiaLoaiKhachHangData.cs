using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class BangGiaLoaiKhachHangData
    {
        private SqlDataHelper helper;
        public BangGiaLoaiKhachHangData()
        {
            helper = new SqlDataHelper();
        }

        public int add(BangGiaLoaiKhachHangModel bangGia)
        {
            int flag = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idMatHang", bangGia.IDMatHang),
                    new SqlParameter("@idLoaiKhachHang", bangGia.IDLoaiKhachHang),
                    new SqlParameter("@giaBanBuon", bangGia.GiaBanBuon),
                    new SqlParameter("@giaBanLe", bangGia.GiaBanLe),
                    new SqlParameter("@ghiChu", bangGia.GhiChu),
                    new SqlParameter("@createdBy", bangGia.CreatedBy),
                };

                flag = int.Parse(helper.ExecuteScalar("usp_vuongtm_BangGiaLoaiKhachHang_add", pars).ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return flag;
        }
        public bool update(BangGiaLoaiKhachHangModel bangGia)
        {
            bool flag = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@id", bangGia.ID),
                    new SqlParameter("@idMatHang", bangGia.IDMatHang),
                    new SqlParameter("@idLoaiKhachHang", bangGia.IDLoaiKhachHang),
                    new SqlParameter("@giaBanBuon", bangGia.GiaBanBuon),
                    new SqlParameter("@giaBanLe", bangGia.GiaBanLe),
                    new SqlParameter("@ghiChu", bangGia.GhiChu),
                    new SqlParameter("@updateBy", bangGia.UpdateBy),
                };

                if (helper.ExecuteNonQuery("usp_vuongtm_BangGiaLoaiKhachHang_update", pars) != 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public bool deletemark(int id, string updateBy, bool deleteMark)
        {
            bool flag = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@id", id),
                    new SqlParameter("@updateBy", updateBy),
                    new SqlParameter("@deleteMark", deleteMark)
                };

                if (helper.ExecuteNonQuery("usp_vuongtm_BangGiaLoaiKhachHang_SetDeleteMark", pars) != 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public bool deleteMarkofmathang(int idMatHang, string listID, string updateBy)
        {
            bool flag = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idMatHang", idMatHang),
                    new SqlParameter("@listID", listID),
                    new SqlParameter("@updateBy", updateBy)
                };

                if (helper.ExecuteNonQuery("usp_vuongtm_DeleteMarkOfMatHang", pars) != 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public List<BangGiaLoaiKhachHangModel> getbanggia(int idMatHang)
        {
            List<BangGiaLoaiKhachHangModel> list = new List<BangGiaLoaiKhachHangModel>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idMatHang", idMatHang)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_GetBangGiaLoaiKhachHangByIdMatHang", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    BangGiaLoaiKhachHangModel bangGia = new BangGiaLoaiKhachHangModel();
                    bangGia.ID = Convert.ToInt32(dr["ID"].ToString());
                    bangGia.IDMatHang = Convert.ToInt32(dr["IDMatHang"].ToString());
                    bangGia.IDLoaiKhachHang = Convert.ToInt32(dr["IDLoaiKhachHang"].ToString());
                    bangGia.GiaBanBuon = Convert.ToDouble(dr["GiaBanBuon"].ToString());
                    bangGia.GiaBanLe = Convert.ToDouble(dr["GiaBanLe"].ToString());
                    bangGia.TenLoaiKhachHang = dr["TenLoaiKhachHang"].ToString();
                    bangGia.GhiChu = dr["GhiChu"].ToString();

                    list.Add(bangGia);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }
        public List<BangGiaLoaiKhachHangModel> getloaikhachhangthieu(int idQLLH, int idMatHang)
        {
            List<BangGiaLoaiKhachHangModel> list = new List<BangGiaLoaiKhachHangModel>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idQLLH", idQLLH),
                    new SqlParameter("@idMatHang", idMatHang)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_LoadDanhSachLoaiKhachHangThieuChoBangGia", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    BangGiaLoaiKhachHangModel bangGia = new BangGiaLoaiKhachHangModel();
                    bangGia.ID = 0;
                    bangGia.IDMatHang = Convert.ToInt32(dr["IDMatHang"].ToString());
                    bangGia.IDLoaiKhachHang = Convert.ToInt32(dr["IDLoaiKhachHang"].ToString());
                    bangGia.GiaBanBuon = Convert.ToDouble(dr["GiaBanBuon"].ToString());
                    bangGia.GiaBanLe = Convert.ToDouble(dr["GiaBanLe"].ToString());
                    bangGia.TenLoaiKhachHang = dr["TenLoaiKhachHang"].ToString();
                    bangGia.GhiChu = dr["GhiChu"].ToString();

                    list.Add(bangGia);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }
        public BangGiaLoaiKhachHangModel getbanggiabyidmathangidloaikhachhang(int idMatHang, int idLoaiKhachHang)
        {
            BangGiaLoaiKhachHangModel bangGia = new BangGiaLoaiKhachHangModel();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idMatHang", idMatHang),
                    new SqlParameter("@idLoaiKhachHang", idLoaiKhachHang)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_GetBangGiaByIdMatHangIdLoaiKhachHang", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {

                    bangGia.ID = Convert.ToInt32(dr["ID"].ToString());
                    bangGia.IDMatHang = Convert.ToInt32(dr["IDMatHang"].ToString());
                    bangGia.IDLoaiKhachHang = Convert.ToInt32(dr["IDLoaiKhachHang"].ToString());
                    bangGia.GiaBanBuon = Convert.ToDouble(dr["GiaBanBuon"].ToString());
                    bangGia.GiaBanLe = Convert.ToDouble(dr["GiaBanLe"].ToString());
                    bangGia.TenLoaiKhachHang = dr["TenLoaiKhachHang"].ToString();
                    bangGia.GhiChu = dr["GhiChu"].ToString();

                    break;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return bangGia;
        }
        public List<LoaiKhachHangOBJ> getloaikhachhangbyidmathang(int idQLLH, int idMatHang)
        {
            List<LoaiKhachHangOBJ> list = new List<LoaiKhachHangOBJ>();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idQLLH", idQLLH),
                    new SqlParameter("@idMatHang", idMatHang)
                };

                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_GetListLoaiKhachHangByIdMatHang", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    LoaiKhachHangOBJ lkh = new LoaiKhachHangOBJ();
                    lkh.ID_LoaiKhachHang = Convert.ToInt32(dr["IDLoaiKhachHang"].ToString());
                    lkh.TenLoaiKhachHang = dr["TenLoaiKhachHang"].ToString();

                    list.Add(lkh);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;
        }
    }
}