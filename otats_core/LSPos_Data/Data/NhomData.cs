using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class NhomData
    {
        private SqlDataHelper helper;
        public NhomData()
        {
            helper = new SqlDataHelper();
        }

        public DataTable GetDsNhomByIdCongTy(int id_QLLH)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", id_QLLH)
                    };

                DataSet ds = helper.ExecuteDataSet("getDSNhomTheoID_QLLH", pars);
                DataTable dt = ds.Tables[0];

                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }


        public DataTable getlistchietkhau()
        {
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_Nhom_HoaHong_GetAll");
                DataTable dt = ds.Tables[0];

                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable GetDsNhomByIdQuanLy(int idQuanLy)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idtaikhoan", idQuanLy)
                    };

                DataSet ds = helper.ExecuteDataSet("getDSNhomTheo_IDTaiKhoan", pars);
                DataTable dt = ds.Tables[0];

                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable CheckmaNhom(string manhom, int idql)
        {
            try
            {
                string sql = string.Format("select n.MaNhom from nhom n where upper(n.MaNhom)  = upper(\'{0}\') and n.ID_QLLH ={1}", manhom, idql);
                DataSet ds = helper.ExecuteDataSet(sql);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable ChecktenNhom(string tennhom, int idql)
        {
            try
            {
                string sql = string.Format("select n.TenNhom from nhom n where upper(n.TenNhom)  = upper(\'{0}\') and n.ID_QLLH ={1}", tennhom, idql);
                DataSet ds = helper.ExecuteDataSet(sql);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable CheckeditmaNhom(string manhom, int idql, int idNhom)
        {
            try
            {
                string sql = string.Format("select n.MaNhom from nhom n where upper(n.MaNhom)  = upper(\'{0}\') and n.ID_QLLH ={1} and n.ID_Nhom !={2} ", manhom, idql, idNhom);
                DataSet ds = helper.ExecuteDataSet(sql);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable CheckedittenNhom(string tennhom, int idql, int idNhom)
        {
            try
            {
                string sql = string.Format("select n.TenNhom from nhom n where upper(n.TenNhom)  = upper(\'{0}\') and n.ID_QLLH ={1} and n.ID_Nhom !={2}", tennhom, idql, idNhom);
                DataSet ds = helper.ExecuteDataSet(sql);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable CheckXoaNhomNV(int id_nhom, int idql)
        {
            try
            {
                string sql = string.Format("select nv.ID_NhanVien from NhanVien NV left join Nhom N on nv.ID_Nhom = N.ID_Nhom where nv.ID_QLLH ={0} and nv.ID_Nhom = {1} and nv.TrangthaiXoa <>1", idql, id_nhom);
                DataSet ds = helper.ExecuteDataSet(sql);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public static List<NhomOBJ> getDS_Nhom(int ID_QLLH, string lang)
        {
            string all = "All";
            string other = "Other";
            if (lang == "vi")
            {
                all = "Tất cả";
                other = "Khác";
            }

            try
            {
                List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
                SqlDataHelper sql = new SqlDataHelper();
                DataSet ds = sql.ExecuteDataSet("getDSNhomTheoID_QLLH",
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                  );

                if (ds == null)
                {
                    return null;
                }

                DataTable dtbl = null;
                dtbl = ds.Tables[0];
                NhomOBJ dmTatCa = new NhomOBJ();
                User_dl u = new User_dl();

                dmTatCa.ID_Nhom = -2;
                dmTatCa.ID_PARENT = 0;
                dmTatCa.TenNhom = all;
                int soLuongTatcaNV = 0;
                int soLuongTatcaQL = 0;
                int soLuongNVKhac = 0;
                int soLuongQLKhac = 0;

                try
                {
                    DataSet dsThongKe = sql.ExecuteDataSet("sp_Nhom_LaySoLuongNhanVien_QuanLy", new SqlParameter("@ID_QLLH", ID_QLLH));
                    DataTable tbThongKe = null;
                    tbThongKe = dsThongKe.Tables[0];
                    soLuongTatcaNV = int.Parse(tbThongKe.Rows[0]["SoLuongNhanVien"].ToString());
                    soLuongTatcaQL = int.Parse(tbThongKe.Rows[0]["SoLuongTaiKhoan"].ToString());
                    soLuongNVKhac = int.Parse(tbThongKe.Rows[0]["SoLuongNhanVienKhac"].ToString());
                    soLuongQLKhac = int.Parse(tbThongKe.Rows[0]["SoLuongQuanLyKhac"].ToString());

                    //CongTyOBJ cty = CongTyDB.ThongTinCongTyByID(ID_QLLH);

                    //dmTatCa.TenHienThi_NhanVien = all + " (" + soLuongTatcaNV + " / " + cty.soluongnhanvien_duocap + ")";
                    dmTatCa.TenHienThi_QuanLy = all + " (" + soLuongTatcaQL + ")";
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    dmTatCa.TenHienThi_NhanVien = all;
                    dmTatCa.TenHienThi_QuanLy = all;
                }
                lstDanhMuc.Add(dmTatCa);
                int cntNV = 0;
                int IDGocCongTy = 0;
                int SoLuongIDGoc = 0;
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    DataRow dr = dtbl.Rows[i];
                    NhomOBJ dm = new NhomOBJ();
                    dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                    dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                    if (dm.ID_PARENT == 0)
                    {
                        SoLuongIDGoc++;
                        IDGocCongTy = dm.ID_Nhom;
                    }
                    dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                    dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                    dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                    dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                    dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;
                    dm.TenNhom = dr["TenNhom"].ToString();

                    dm.MaNhom = dr["MaNhom"].ToString();
                    dm.SiteCode = dr["SiteCode"].ToString();

                    dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                    dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";
                    lstDanhMuc.Add(dm);
                    cntNV += dm.SoLuongNhanVien;
                }

                NhomOBJ dmKhac = new NhomOBJ();

                dmKhac.ID_Nhom = -1;
                //dmKhac.ID_PARENT = SoLuongIDGoc == 1 ? IDGocCongTy : 0;
                dmKhac.ID_PARENT = 0;
                dmKhac.TenHienThi_QuanLy = other + " (" + soLuongQLKhac + ")";
                dmKhac.TenHienThi_NhanVien = other + " (" + soLuongNVKhac + ")";
                dmKhac.TenNhom = other;

                if (lstDanhMuc.Count > 0)
                {
                    lstDanhMuc.Insert(lstDanhMuc.Count, dmKhac);
                }
                return lstDanhMuc;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public static List<NhomOBJ> getDS_NhomByTaiKhoan(int ID_TaiKhoan, int ID_QLLH, string lang)
        {

            List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomTheoIDTaiKhoan",
                new SqlParameter("@ID_TaiKhoan", ID_TaiKhoan)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            User_dl u = new User_dl();

            int soLuongTatcaNV = 0;
            int soLuongTatcaQL = 0;
            int soLuongNVKhac = 0;
            int soLuongQLKhac = 0;

            try
            {
                DataSet dsThongKe = sql.ExecuteDataSet("sp_Nhom_LaySoLuongNhanVien_QuanLy", new SqlParameter("@ID_QLLH", ID_QLLH));
                DataTable tbThongKe = null;
                tbThongKe = dsThongKe.Tables[0];
                soLuongTatcaNV = int.Parse(tbThongKe.Rows[0]["SoLuongNhanVien"].ToString());
                soLuongTatcaQL = int.Parse(tbThongKe.Rows[0]["SoLuongTaiKhoan"].ToString());
                soLuongNVKhac = int.Parse(tbThongKe.Rows[0]["SoLuongNhanVienKhac"].ToString());
                soLuongQLKhac = int.Parse(tbThongKe.Rows[0]["SoLuongQuanLyKhac"].ToString());

                //CongTyOBJ cty = CongTyDB.ThongTinCongTyByID(ID_QLLH);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            int cntNV = 0;
            int IDGocCongTy = 0;
            int SoLuongIDGoc = 0;
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                NhomOBJ dm = new NhomOBJ();
                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                if (dm.ID_PARENT == 0)
                {
                    SoLuongIDGoc++;
                    IDGocCongTy = dm.ID_Nhom;
                }
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString();

                dm.MaNhom = dr["MaNhom"].ToString();
                dm.SiteCode = dr["SiteCode"].ToString();

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";
                lstDanhMuc.Add(dm);
                cntNV += dm.SoLuongNhanVien;
            }


            return lstDanhMuc;
        }

        public static NhomOBJ getNhomByID(int ID_Nhom)
        {
            NhomOBJ dm = new NhomOBJ();

            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("sp_Nhom_GetById",
                new SqlParameter("@ID_Nhom", ID_Nhom)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];


            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString();
                dm.CongNoGioiHan = (dr["CongNoGioiHan"].ToString() != "") ? float.Parse(dr["CongNoGioiHan"].ToString()) : 0;

                dm.MaNhom = dr["MaNhom"].ToString();
                dm.SiteCode = dr["SiteCode"].ToString();

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";

            }

            return dm;
        }

        public bool setchietkhau(int idnhom, decimal hoahong)
        {
            int ID = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhom", idnhom),
                new SqlParameter("@hoahong", hoahong)
            };
                SqlDataHelper sql = new SqlDataHelper();
                int rowwaff = sql.ExecuteNonQuery("sp_Nhom_HoaHong_SetHoaHong", par);
                if (rowwaff > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return false;

        }
    }
}