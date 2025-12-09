using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class KhachHangData
    {
        private SqlDataHelper helper;
        public KhachHangData()
        {
            helper = new SqlDataHelper();
        }

        public KhachHang GetKhachHangFromDataRow(DataRow dr)
        {
            try
            {
                KhachHang kh = new KhachHang();

                kh.IDKhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                kh.IDQLLH = (dr["ID_QLLH"] != null) ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                kh.Ten = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                kh.KinhDo = double.Parse(dr["KinhDo"].ToString());
                kh.ViDo = double.Parse(dr["ViDo"].ToString());
                kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
                kh.MaSoThue = dr["MaSoThue"].ToString();
                kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
                kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
                kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
                kh.SoDienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
                kh.SoDienThoai2 = (dr["DienThoai2"] != null) ? dr["DienThoai2"].ToString() : "";
                kh.SoDienThoai3 = (dr["DienThoai3"] != null) ? dr["DienThoai3"].ToString() : "";
                kh.SoDienThoaiMacDinh = (dr["DienThoaiMacDinh"] != null) ? dr["DienThoaiMacDinh"].ToString() : "";
                try
                {
                    kh.TenTinh = (dr["TenTinh"].ToString() != "") ? dr["TenTinh"].ToString() : "";
                    kh.TenQuan = (dr["TenQuan"].ToString() != "") ? dr["TenQuan"].ToString() : "";
                    kh.TenPhuong = (dr["TenPhuong"].ToString() != "") ? dr["TenPhuong"].ToString() : "";
                }
                catch { }

                kh.NguoiLienHe = (dr["NguoiLienHe"] != DBNull.Value) ? dr["NguoiLienHe"].ToString() : "";
                kh.ID_NhanVien = (dr["ID_NhanVien"] != DBNull.Value) ? Convert.ToInt16(dr["ID_NhanVien"]) : 0;
                //kh.TenNhanVien = (dr["TenNhanVien"] != DBNull.Value) ? dr["TenNhanVien"].ToString() : "";
                kh.SoTKNganHang = dr["SoTKNganHang"].ToString();
                kh.Email = (dr["Email"] != null) ? dr["Email"].ToString() : "";
                kh.Fax = dr["Fax"].ToString();
                kh.Website = dr["Website"].ToString();
                kh.GhiChu = dr["GhiChu"].ToString();
                kh.TrangThai = Convert.ToInt32(dr["TrangThai"]);
                kh.NgayTao = dr["insertedtime"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["insertedtime"]);
                kh.ID_QuanLy = (dr["ID_QuanLy"] != DBNull.Value) ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
                kh.ID_LoaiKhachHang = (dr["ID_LoaiKhachHang"] != DBNull.Value) ? int.Parse(dr["ID_LoaiKhachHang"].ToString()) : 0;
                kh.DuongPho = (dr["DuongPho"] != null) ? dr["DuongPho"].ToString() : "";
                //kh.danhsachanh = new List<ImageOBJ>();
                //kh.danhsachanh = KhachHang_dl.DanhSachAnh_ByIDKhachHang(int.Parse(dr["ID_KhachHang"].ToString()));
                kh.ID_NhomKH = (dr["ID_NhomKH"] != DBNull.Value) ? int.Parse(dr["ID_NhomKH"].ToString()) : 0;
                kh.ID_Cha = (dr["ID_Cha"] != DBNull.Value) ? int.Parse(dr["ID_Cha"].ToString()) : 0;
                kh.ID_KhuVuc = (dr["ID_KhuVuc"] != DBNull.Value) ? int.Parse(dr["ID_KhuVuc"].ToString()) : 0;
                kh.GhiChuKhiXoa = dr["GhiChuKhiXoa"].ToString();
                kh.DiaChiXuatHoaDon = dr["DiaChiXuatHoaDon"].ToString();
                try
                {
                    kh.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                    kh.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                    kh.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : kh.LastUpdate_ThoiGian_NhanVien;
                    kh.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : kh.LastUpdate_ThoiGian_QuanLy;
                    kh.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                    kh.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();

                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }

                return kh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public List<KhachHangDTO> GetDataKhachHang_Kendo(int ID_QLLH, int ID_QuanLy, int ID_Tinh, int ID_Quan, int ID_LoaiKhachHang, int ID_KenhBanHang, int startRecord, int maxRecords, FilterGrid filter, ref int TongSo)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@id_qllh",ID_QLLH),
                new SqlParameter("@ID_QuanLy",ID_QuanLy),
                //new SqlParameter("@ID_Tinh",ID_Tinh),
                //new SqlParameter("@ID_Quan",ID_Quan),
                //new SqlParameter("@ID_LoaiKhachHang",ID_LoaiKhachHang),
                //new SqlParameter("@ID_KenhBanHang",ID_KenhBanHang),
                new SqlParameter("@startRecord", startRecord),
                new SqlParameter("@maxRecords", maxRecords) ,
                new SqlParameter("@TenKhachHang", filter.TenKhachHang ),
                new SqlParameter("@MaKH ", filter.MaKH ),
                new SqlParameter("@DiaChi ", filter.DiaChi ),
                new SqlParameter("@TenTinh ", filter.TenTinh ),
                new SqlParameter("@TenQuan ", filter.TenQuan ),
                new SqlParameter("@TenPhuong ", filter.TenPhuong),
                new SqlParameter("@DienThoai", filter.DienThoai ),
                new SqlParameter("@Email", filter.Email ),
                new SqlParameter("@TenNhanVien", filter.TenNhanVien ),
                new SqlParameter("@TenLoaiKhachHang", filter.TenLoaiKhachHang),
                new SqlParameter("@TenNhomKH ", filter.TenNhomKH ),
                new SqlParameter("@MaSoThue ", filter.MaSoThue),
                new SqlParameter("@NguoiLienHe", filter.NguoiLienHe),
                new SqlParameter("@GhiChu ", filter.GhiChu)   };            //new SqlParameter("@NgayTao", filter.NgayTao);
                DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQLKENDO", param);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                if (dt2.Rows.Count > 0)
                {
                    TongSo = int.Parse(dt2.Rows[0]["soluong"].ToString());
                }
                else
                {
                    TongSo = 0;
                }
                List<KhachHangDTO> lkh = new List<KhachHangDTO>();
                foreach (DataRow dr in dt.Rows)
                {
                    KhachHangDTO kh = new KhachHangDTO();
                    kh.ID_KhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                    kh.ID_QLLH = (dr["ID_QLLH"] != null) ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                    kh.TenKhachHang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";


                    //kh.KinhDo = double.Parse(dr["KinhDo"].ToString());
                    double kinhdo = 0;
                    double.TryParse(dr["KinhDo"].ToString(), out kinhdo);
                    kh.KinhDo = kinhdo;
                    //kh.ViDo = double.Parse(dr["ViDo"].ToString());
                    double vido = 0;
                    double.TryParse(dr["ViDo"].ToString(), out vido);
                    kh.ViDo = vido;


                    kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
                    kh.MaSoThue = dr["MaSoThue"].ToString();
                    kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                    kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
                    kh.TenTinh = (dr["TenTinh"] != null) ? dr["TenTinh"].ToString() : "";
                    kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
                    kh.TenQuan = (dr["TenQuan"] != null) ? dr["TenQuan"].ToString() : "";
                    kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
                    kh.TenPhuong = (dr["TenPhuong"] != null) ? dr["TenPhuong"].ToString() : "";
                    kh.DienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
                    kh.DienThoai2 = (dr["DienThoai2"] != null) ? dr["DienThoai2"].ToString() : "";
                    kh.DienThoai3 = (dr["DienThoai3"] != null) ? dr["DienThoai3"].ToString() : "";
                    kh.Email = (dr["Email"] != null) ? dr["Email"].ToString() : "";
                    kh.TenNhanVien = (dr["TenNhanVien"] != null) ? dr["TenNhanVien"].ToString() : "";
                    kh.TenLoaiKhachHang = (dr["TenLoaiKhachHang"] != null) ? dr["TenLoaiKhachHang"].ToString() : "";
                    kh.ID_LoaiKhachHang = (dr["ID_LoaiKhachHang"].ToString() != "") ? int.Parse(dr["ID_LoaiKhachHang"].ToString()) : 0;
                    kh.ID_NhomKH = (dr["ID_NhomKH"].ToString() != "") ? int.Parse(dr["ID_NhomKH"].ToString()) : 0;
                    kh.TenNhomKH = (dr["TenNhomKH"] != null) ? dr["TenNhomKH"].ToString() : "";
                    kh.MaSoThue = (dr["MaSoThue"] != null) ? dr["MaSoThue"].ToString() : "";
                    kh.NguoiLienHe = (dr["NguoiLienHe"] != null) ? dr["NguoiLienHe"].ToString() : "";
                    kh.GhiChu = (dr["GhiChu"] != null) ? dr["GhiChu"].ToString() : "";
                    kh.NgayTao = dr["insertedtime"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["insertedtime"]);
                    kh.AnhDaiDien = KhachHang_dl.DanhSachAnh_ByIDKhachHang(int.Parse(dr["ID_KhachHang"].ToString()));
                    kh.TenKhuVuc = (dr["TenKhuVuc"] != null) ? dr["TenKhuVuc"].ToString() : "";
                    kh.ID_KhuVuc = (dr["ID_KhuVuc"].ToString() != "") ? int.Parse(dr["ID_KhuVuc"].ToString()) : 0;
                    kh.ID_KenhCapTren = (dr["ID_KBHCT"].ToString() != "") ? int.Parse(dr["ID_KBHCT"].ToString()) : 0;
                    kh.TenKenhCapTren = (dr["Ten_kenhBHCapTren"] != null) ? dr["Ten_kenhBHCapTren"].ToString() : "";
                    kh.ToaDoKhachHang = (dr["ToaDoKhachHang"] != null) ? dr["ToaDoKhachHang"].ToString() : "";
                    if (kh.AnhDaiDien.Count > 0)
                    {
                        kh.Imgurl = kh.AnhDaiDien[0].path_thumbnail_medium;
                    }

                    lkh.Add(kh);

                }
                return lkh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public int DeleteAllKhachHang(int ID_QLLH, int ID_QuanLy)
        {
            int re = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH",ID_QLLH),
                    new SqlParameter("@ID_QuanLy",ID_QuanLy)
                };
                DataSet ds = helper.ExecuteDataSet("VuongTM_KhachHang_DeleteAll", param);
                re = int.Parse(ds.Tables[0].Rows[0]["re"].ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return re;
        }
        public DataSet ExportDataKhachHang_Kendo(int ID_QLLH, int ID_QuanLy, ExportKhachHangDTO filter)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@id_qllh",ID_QLLH),
                new SqlParameter("@ID_QuanLy",ID_QuanLy),
                new SqlParameter("@TenKhachHang", filter.tenKhachHang ),
                new SqlParameter("@MaKH ", filter.maKH ),
                new SqlParameter("@DiaChi ", filter.diaChi ),
                new SqlParameter("@TenTinh ", filter.tenTinh ),
                new SqlParameter("@TenQuan ", filter.tenQuan ),
                new SqlParameter("@TenPhuong ", filter.tenPhuong),
                new SqlParameter("@DienThoai", filter.dienThoai ),
                new SqlParameter("@Email", filter.email ),
                new SqlParameter("@TenNhanVien", filter.tenNhanVien ),
                new SqlParameter("@TenLoaiKhachHang", filter.tenLoaiKhachHang),
                new SqlParameter("@TenNhomKH ", filter.tenNhomKH ),
                new SqlParameter("@MaSoThue ", filter.maSoThue),
                new SqlParameter("@NguoiLienHe", filter.nguoiLienHe),
                new SqlParameter("@GhiChu ", filter.ghiChu)   };            //new SqlParameter("@NgayTao", filter.NgayTao);
                DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHang_Excel", param);
                return ds;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public List<KhachHangDTO> GetDataKhachHangByNhanVien_Kendo(int ID_QLLH, int ID_QuanLy, string ID_NhanViens, int startRecord, int maxRecords, FilterGrid filter, ref int TongSo)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@id_qllh",ID_QLLH),
                new SqlParameter("@ID_QuanLy",ID_QuanLy),
                new SqlParameter("@ListIDNV",ID_NhanViens),
                new SqlParameter("@startRecord", startRecord),
                new SqlParameter("@maxRecords", maxRecords) ,
                new SqlParameter("@TenKhachHang", filter.TenKhachHang ),
                new SqlParameter("@MaKH", filter.MaKH ),
                new SqlParameter("@DiaChi", filter.DiaChi ),
                new SqlParameter("@TenTinh", filter.TenTinh ),
                new SqlParameter("@TenQuan", filter.TenQuan ),
                new SqlParameter("@TenPhuong", filter.TenPhuong),
                new SqlParameter("@DienThoai", filter.DienThoai ),
                new SqlParameter("@Email", filter.Email ),
                new SqlParameter("@TenNhanVien", filter.TenNhanVien ),
                new SqlParameter("@TenLoaiKhachHang", filter.TenLoaiKhachHang),
                new SqlParameter("@TenNhomKH", filter.TenNhomKH ),
                new SqlParameter("@MaSoThue", filter.MaSoThue),
                new SqlParameter("@NguoiLienHe", filter.NguoiLienHe),
                new SqlParameter("@GhiChu", filter.GhiChu)   };
                DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQLKENDO_NVQL", param);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                if (dt2.Rows.Count > 0)
                {
                    TongSo = int.Parse(dt2.Rows[0]["soluong"].ToString());
                }
                else
                {
                    TongSo = 0;
                }
                List<KhachHangDTO> lkh = new List<KhachHangDTO>();
                foreach (DataRow dr in dt.Rows)
                {
                    KhachHangDTO kh = new KhachHangDTO();
                    kh.ID_KhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                    kh.ID_QLLH = (dr["ID_QLLH"] != null) ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                    kh.TenKhachHang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";


                    //kh.KinhDo = double.Parse(dr["KinhDo"].ToString());
                    double kinhdo = 0;
                    double.TryParse(dr["KinhDo"].ToString(), out kinhdo);
                    kh.KinhDo = kinhdo;
                    //kh.ViDo = double.Parse(dr["ViDo"].ToString());
                    double vido = 0;
                    double.TryParse(dr["ViDo"].ToString(), out vido);
                    kh.ViDo = vido;


                    kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
                    kh.MaSoThue = dr["MaSoThue"].ToString();
                    kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                    kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
                    kh.TenTinh = (dr["TenTinh"] != null) ? dr["TenTinh"].ToString() : "";
                    kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
                    kh.TenQuan = (dr["TenQuan"] != null) ? dr["TenQuan"].ToString() : "";
                    kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
                    kh.TenPhuong = (dr["TenPhuong"] != null) ? dr["TenPhuong"].ToString() : "";
                    kh.DienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
                    kh.DienThoai2 = (dr["DienThoai2"] != null) ? dr["DienThoai2"].ToString() : "";
                    kh.DienThoai3 = (dr["DienThoai3"] != null) ? dr["DienThoai3"].ToString() : "";
                    kh.Email = (dr["Email"] != null) ? dr["Email"].ToString() : "";
                    kh.TenNhanVien = (dr["TenNhanVien"] != null) ? dr["TenNhanVien"].ToString() : "";
                    kh.TenLoaiKhachHang = (dr["TenLoaiKhachHang"] != null) ? dr["TenLoaiKhachHang"].ToString() : "";
                    kh.ID_LoaiKhachHang = (dr["ID_LoaiKhachHang"].ToString() != "") ? int.Parse(dr["ID_LoaiKhachHang"].ToString()) : 0;
                    kh.ID_NhomKH = (dr["ID_NhomKH"].ToString() != "") ? int.Parse(dr["ID_NhomKH"].ToString()) : 0;
                    kh.TenNhomKH = (dr["TenNhomKH"] != null) ? dr["TenNhomKH"].ToString() : "";
                    kh.MaSoThue = (dr["MaSoThue"] != null) ? dr["MaSoThue"].ToString() : "";
                    kh.NguoiLienHe = (dr["NguoiLienHe"] != null) ? dr["NguoiLienHe"].ToString() : "";
                    kh.GhiChu = (dr["GhiChu"] != null) ? dr["GhiChu"].ToString() : "";
                    kh.NgayTao = dr["insertedtime"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["insertedtime"]);
                    kh.AnhDaiDien = KhachHang_dl.DanhSachAnh_ByIDKhachHang(int.Parse(dr["ID_KhachHang"].ToString()));
                    kh.TenKhuVuc = (dr["TenKhuVuc"] != null) ? dr["TenKhuVuc"].ToString() : "";
                    kh.ID_KhuVuc = (dr["ID_KhuVuc"].ToString() != "") ? int.Parse(dr["ID_KhuVuc"].ToString()) : 0;
                    kh.ID_KenhCapTren = (dr["ID_KBHCT"].ToString() != "") ? int.Parse(dr["ID_KBHCT"].ToString()) : 0;
                    kh.TenKenhCapTren = (dr["Ten_kenhBHCapTren"] != null) ? dr["Ten_kenhBHCapTren"].ToString() : "";
                    kh.ToaDoKhachHang = (dr["ToaDoKhachHang"] != null) ? dr["ToaDoKhachHang"].ToString() : "";
                    if (kh.AnhDaiDien.Count > 0)
                    {
                        kh.Imgurl = kh.AnhDaiDien[0].path_thumbnail_medium;
                    }

                    lkh.Add(kh);

                }
                return lkh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable GetDataKhachHangByNhanVien_IDQL(int ID_QLLH, int ID_QuanLy)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@id_qllh",ID_QLLH),
                new SqlParameter("@ID_QuanLy",ID_QuanLy)   };
                DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL_New", param);
                DataTable dt = ds.Tables[0];

                //List<KhachHangDTO> lkh = new List<KhachHangDTO>();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    KhachHangDTO kh = new KhachHangDTO();
                //    kh.ID_KhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                //    kh.ID_QLLH = (dr["ID_QLLH"] != null) ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                //    kh.TenKhachHang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";

                //    //kh.KinhDo = double.Parse(dr["KinhDo"].ToString());
                //    double kinhdo = 0;
                //    double.TryParse(dr["KinhDo"].ToString(), out kinhdo);
                //    kh.KinhDo = kinhdo;
                //    //kh.ViDo = double.Parse(dr["ViDo"].ToString());
                //    double vido = 0;
                //    double.TryParse(dr["ViDo"].ToString(), out vido);
                //    kh.ViDo = vido;

                //    kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
                //    kh.MaSoThue = dr["MaSoThue"].ToString();
                //    kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                //    kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
                //    kh.TenTinh = (dr["TenTinh"] != null) ? dr["TenTinh"].ToString() : "";
                //    kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
                //    kh.TenQuan = (dr["TenQuan"] != null) ? dr["TenQuan"].ToString() : "";
                //    kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
                //    kh.TenPhuong = (dr["TenPhuong"] != null) ? dr["TenPhuong"].ToString() : "";
                //    kh.DienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
                //    kh.DienThoai2 = (dr["DienThoai2"] != null) ? dr["DienThoai2"].ToString() : "";
                //    kh.DienThoai3 = (dr["DienThoai3"] != null) ? dr["DienThoai3"].ToString() : "";
                //    kh.Email = (dr["Email"] != null) ? dr["Email"].ToString() : "";
                //    kh.TenNhanVien = (dr["TenNhanVien"] != null) ? dr["TenNhanVien"].ToString() : "";
                //    kh.TenLoaiKhachHang = (dr["TenLoaiKhachHang"] != null) ? dr["TenLoaiKhachHang"].ToString() : "";
                //    kh.ID_LoaiKhachHang = (dr["ID_LoaiKhachHang"].ToString() != "") ? int.Parse(dr["ID_LoaiKhachHang"].ToString()) : 0;
                //    kh.ID_NhomKH = (dr["ID_NhomKH"].ToString() != "") ? int.Parse(dr["ID_NhomKH"].ToString()) : 0;
                //    kh.TenNhomKH = (dr["TenNhomKH"] != null) ? dr["TenNhomKH"].ToString() : "";
                //    kh.MaSoThue = (dr["MaSoThue"] != null) ? dr["MaSoThue"].ToString() : "";
                //    kh.NguoiLienHe = (dr["NguoiLienHe"] != null) ? dr["NguoiLienHe"].ToString() : "";
                //    kh.GhiChu = (dr["GhiChu"] != null) ? dr["GhiChu"].ToString() : "";
                //    kh.NgayTao = dr["insertedtime"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["insertedtime"]);
                //    kh.AnhDaiDien = KhachHang_dl.DanhSachAnh_ByIDKhachHang(int.Parse(dr["ID_KhachHang"].ToString()));
                //    kh.TenKhuVuc = (dr["TenKhuVuc"] != null) ? dr["TenKhuVuc"].ToString() : "";
                //    kh.ID_KhuVuc = (dr["ID_KhuVuc"].ToString() != "") ? int.Parse(dr["ID_KhuVuc"].ToString()) : 0;
                //    kh.ID_KenhCapTren = (dr["ID_KBHCT"].ToString() != "") ? int.Parse(dr["ID_KBHCT"].ToString()) : 0;
                //    kh.TenKenhCapTren = (dr["Ten_kenhBHCapTren"] != null) ? dr["Ten_kenhBHCapTren"].ToString() : "";
                //    kh.ToaDoKhachHang = (dr["ToaDoKhachHang"] != null) ? dr["ToaDoKhachHang"].ToString() : "";
                //    if (kh.AnhDaiDien.Count > 0)
                //    {
                //        kh.Imgurl = kh.AnhDaiDien[0].path_thumbnail_medium;
                //    }

                //    lkh.Add(kh);

                //}
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable GetKhachHangQuanLy(int ID_NhanVien)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@ID_NhanVien",ID_NhanVien)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangQuanLy_v1", param);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public DataTable GetDataKhachHangByNhanVien_KendoDT(int ID_QLLH, int ID_QuanLy, string ID_NhanViens, int startRecord, int maxRecords, FilterGrid filter, ref int TongSo)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@id_qllh",ID_QLLH),
                new SqlParameter("@ID_QuanLy",ID_QuanLy),
                new SqlParameter("@ListIDNV",ID_NhanViens),
                new SqlParameter("@startRecord", startRecord),
                new SqlParameter("@maxRecords", maxRecords) ,
                new SqlParameter("@TenKhachHang", filter.TenKhachHang ),
                new SqlParameter("@MaKH", filter.MaKH ),
                new SqlParameter("@DiaChi", filter.DiaChi ),
                new SqlParameter("@TenTinh", filter.TenTinh ),
                new SqlParameter("@TenQuan", filter.TenQuan ),
                new SqlParameter("@TenPhuong", filter.TenPhuong),
                new SqlParameter("@DienThoai", filter.DienThoai ),
                new SqlParameter("@Email", filter.Email ),
                new SqlParameter("@TenNhanVien", filter.TenNhanVien ),
                new SqlParameter("@TenLoaiKhachHang", filter.TenLoaiKhachHang),
                new SqlParameter("@TenNhomKH", filter.TenNhomKH ),
                new SqlParameter("@MaSoThue", filter.MaSoThue),
                new SqlParameter("@NguoiLienHe", filter.NguoiLienHe),
                new SqlParameter("@GhiChu", filter.GhiChu)   };
                DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQLKENDO_NVQL", param);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                if (dt2.Rows.Count > 0)
                {
                    TongSo = int.Parse(dt2.Rows[0]["soluong"].ToString());
                }
                else
                {
                    TongSo = 0;
                }

                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public DataTable GetDataKhachHangAll_Combo(int IDQLLH, int ID_QuanLy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),

        };

                DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL_ComBo_Kendo", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);

            }


            return dt;
        }
        public DataTable GetDataKhachHangAll_ServerPaging(int ID_QLLH, int ID_QuanLy, int Take, int Skip)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] parameter = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@Take", Take),
                    new SqlParameter("@Skip", Skip),
                    };

                DataSet ds = helper.ExecuteDataSet("vuongtm_combo_serverpaging_getlist", parameter);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return dt;
        }

        public DataTable GetDataKhachHangBy_IDQLLH(int IDQL)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
           new SqlParameter("@ID_QLLH", IDQL),
            new SqlParameter("@laykhachhangnhomcha", 1)
        };

                DataSet ds = helper.ExecuteDataSet("sp_App_DanhSachCuaHangTheoIDCha_Export", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);

            }


            return dt;
        }

        public DataTable GetDataKhachHangDuyetXoa(int id_qllh, int ID_QuanLy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@id_qllh", id_qllh),
                new SqlParameter("@ID_QuanLy", ID_QuanLy)
                };

                DataSet ds = helper.ExecuteDataSet("VuongTM_DuyetXoaKhachHang_GetList", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return dt;
        }

        public int SaoChepPhanQuyenNhanVien(int ID_NhanVienNguon, int ID_NhanVienDich, string ID_Quyen, int ID_QuanLy)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVienNguon", ID_NhanVienNguon),
            new SqlParameter("ID_NhanVienDich", ID_NhanVienDich) ,
            new SqlParameter("ID_Quyen", ID_Quyen),
               new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                LSPos_Data.Utilities.Log.Info("SaoChepPhanQuyenNhanVien : ID_NhanVienNguon = " + ID_NhanVienNguon + " - ID_NhanVienDich : " + ID_NhanVienDich + " - ID_Quyen : " + ID_Quyen + " - ID_QuanLy : " + ID_QuanLy);
                return helper.ExecuteNonQuery("sp_QL_PhanQuyenNhanVien_Copy", pars);

            }
            catch
            {
                return 0;
            }
        }

        public int XoaPhanQuyenNhanVien(int ID_NhanVien)
        {
            SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_NhanVien", ID_NhanVien),

        };
            try
            {
                return helper.ExecuteNonQuery("sp_PhanQuyen_XoaByNhanVien", pars);
            }
            catch
            {
                return 0;
            }
        }

        public class FilterGrid
        {
            public string DiaChi { get; set; }
            public string TenKhachHang { get; set; }
            public string MaKH { get; set; }
            public string TenTinh { get; set; }
            public string TenQuan { get; set; }
            public string TenPhuong { get; set; }
            public string DienThoai { get; set; }
            public string Email { get; set; }
            public string TenNhanVien { get; set; }
            public string TenLoaiKhachHang { get; set; }
            public string TenNhomKH { get; set; }
            public string MaSoThue { get; set; }
            public string NguoiLienHe { get; set; }
            public string GhiChu { get; set; }
            public DateTime NgayTao { get; set; }
            public int CountTongSo { get; set; }


        }
    }
}