using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class BaoCaoCommonDAL
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();
        public BaoCaoCommonDAL()
        {
            //
            // TODO: Add constructor logic here
            //
            helper = new SqlDataHelper();
        }

        public DataSet BaoCaoDoanhThu(int IDQLLH, int ID_NhanVien, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoDoanhThu", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoDoanhThuTheoNgay(int IDQLLH, int ID_NhanVien, int ID_KhachHang, DateTime date, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("date", date),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDoanhThuTheoNgay", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoDoanhThuNgay(int IDQLLH, DateTime Ngay, int ID_QuanLy, int ID_Nhom, int ID_NhanVien)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
             new SqlParameter("ID_Nhom", ID_Nhom),
            new SqlParameter("Ngay", Ngay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
             new SqlParameter("ID_NhanVien", ID_NhanVien)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDoanhThuTrongNgay", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoDoanhThuNgay_V2(int IDQLLH, DateTime Ngay, int ID_QuanLy, int ID_Nhom, int ID_KhachHang, int ID_NhanVien)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("Ngay", Ngay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
             new SqlParameter("ID_Nhom", ID_Nhom),
             new SqlParameter("ID_KhachHang", ID_KhachHang),
              new SqlParameter("ID_NhanVien", ID_NhanVien)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDoanhThuTrongNgay_v2", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoTrangChu(int IDQLLH, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_GetBaoCaoTrangChu", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoTrangChu_v3(int IDQLLH, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_GetBaoCaoTrangChu_v3", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        /// <summary>
        /// Hàm mới cập nhật v1.9.9.3 : nhân viên trực tuyến lấy theo trangthaiketnoi thay vì trước đó lấy theo trạng thái bản tin gps gửi lên
        /// 
        /// </summary>
        /// <param name="IDQLLH"></param>
        /// <param name="ID_QuanLy"></param>
        /// <returns></returns>
        public DataSet BaoCaoTrangChu_v4(int IDQLLH, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_GetBaoCaoTrangChu_v4", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoDonHangTongQuan(int IDQLLH, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("from", dtFrom),
            new SqlParameter("to", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTongQuan", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoMH_KH(int IDQLLH, int ID_Hang, int ID_LoaiKhachHang, int ID_KhachHang, string idnhanvien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("ID_NhanVien", idnhanvien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoMHKH_v2", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoMH_KH_DH(int IDQLLH, int ID_Hang, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoMH_KH_DH", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoMH_KH_DH_NV(int IDQLLH, int ID_Hang, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy, int ID_NhanVien)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoMH_KH_DH_NV", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoMH_NV(int IDQLLH, int ID_Hang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoMHNV", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }


        public DataSet BaoCaoMH_NV_ChiTiet(int IDQLLH, int ID_Hang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoMHNV_ChiTiet", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoDonHang(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHang", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoLoTrinh(int IDQLLH, int ID_NhanVien, DateTime dtReceivedTime)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtReceivedTime", dtReceivedTime)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoLoTrinh", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataTable BaoCaoAnhChup(int idcty, int idnhanvien, int idkhachhang, DateTime tungay, DateTime denNgay, int ID_QuanLy)
        {
            DataTable dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", idcty),
            new SqlParameter("ID_NhanVien", idnhanvien),
            new SqlParameter("ID_KhachHang", idkhachhang),
            new SqlParameter("dtFrom", tungay),
            new SqlParameter("dtTo", denNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                dt = helper.ExecuteDataSet("sp_web_BaoCaoAnhChup", pars).Tables[0];
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataSet BaoCaoQuangDuongDiChuyen(int ID_QLLH, int ID_NhanVien, DateTime tungay, DateTime denNgay, int ID_QuanLy)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("tungay", tungay),
            new SqlParameter("denNgay", denNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                dt = helper.ExecuteDataSet("sp_QL_BaoCaoKmGPSDiChuyenMoi_v2", pars);
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataSet BaoCaoQuangDuongDiChuyenExcel(int ID_QLLH, int ID_NhanVien, DateTime tungay, DateTime denNgay, int ID_QuanLy)
        {

            DataTable rs = new DataTable();
            try
            {
                BaoCao_dl bcd = new BaoCao_dl();

                if (tungay.Date == DateTime.Now.Date)
                {
                    rs = bcd.BaoCaoKmDiChuyen_TrongNgay(ID_NhanVien, ID_QuanLy, ID_QLLH, tungay, denNgay);
                }
                else
                {
                    rs = bcd.BaoCaoKmDiChuyen(ID_NhanVien, ID_QuanLy, ID_QLLH, tungay, denNgay);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(rs.Copy());

            DataTable dt = new DataTable();
            dt.Columns.Add("TITLE", typeof(String));

            DataRow dr = dt.NewRow();
            dr["TITLE"] = "Từ " + tungay.ToString("dd/MM/yyyy") + " đến " + denNgay.ToString("dd/MM/yyyy");

            ds.Tables.Add(dt.Copy());

            return ds;
        }

        public DataSet BaoCaoDonHangTheoDiem(int ID_QLLH, int ID_NhanVien, DateTime TuNgay, DateTime DenNgay, int ID_QuanLy)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                dt = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTheoDiem", pars);
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataTable BaoCaoDonHangTheoDiem_ChiTiet(int ID_NhanVien, DateTime TuNgay, DateTime DenNgay)
        {
            DataTable dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
        };
            try
            {
                dt = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTheoDiem_ChiTiet", pars).Tables[0];
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public int DeleteAnhChup(int ImageID)
        {
            try
            {
                return helper.ExecuteNonQuery("sp_QL_DeleteImage", new SqlParameter("ImageID", ImageID));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static ThongKeTrucTuyenOBJ ThongKeTrucTuyenIDCT(int idct, int ID_QuanLy, int ID_Nhom)
        {
            ThongKeTrucTuyenOBJ rs = new ThongKeTrucTuyenOBJ();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@idct", idct),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };
            try
            {
                DataRow dr = db.ExecuteDataSet("sp_QL_ThongKeNhanVienTrucTuyen", pars).Tables[0].Rows[0];
                rs.tructuyen = int.Parse(dr["ol"].ToString());
                rs.chuadangnhapduoi60 = int.Parse(dr["60off"].ToString());
                rs.chuadangnhaptren60 = int.Parse(dr["off60"].ToString());
                rs.mattinhieuduoi60 = int.Parse(dr["60dc"].ToString());
                rs.mattinhieutren60 = int.Parse(dr["dc60"].ToString());
                rs.tong = int.Parse(dr["total"].ToString());

            }
            catch
            {
                rs.tructuyen = 0;
                rs.chuadangnhapduoi60 = 0;
                rs.chuadangnhaptren60 = 0;
                rs.mattinhieutren60 = 0;
                rs.mattinhieuduoi60 = 0;
                rs.tong = 0;
            }
            return rs;
        }
        /// <summary>
        /// v1.9.9.3 : Lay trang thai truc tuyen theo trang thai ket noi - TRUONGNM
        /// </summary>
        /// <param name="idct"></param>
        /// <param name="ID_QuanLy"></param>
        /// <param name="ID_Nhom"></param>
        /// <returns></returns>
        public static ThongKeTrucTuyenOBJ ThongKeTrucTuyenIDCT_v2(int idct, int ID_QuanLy, int ID_Nhom)
        {
            ThongKeTrucTuyenOBJ rs = new ThongKeTrucTuyenOBJ();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@idct", idct),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };
            try
            {
                DataRow dr = db.ExecuteDataSet("sp_QL_ThongKeNhanVienTrucTuyen_v2", pars).Tables[0].Rows[0];
                rs.tructuyen = int.Parse(dr["ol"].ToString());
                rs.chuadangnhapduoi60 = int.Parse(dr["60off"].ToString());
                rs.chuadangnhaptren60 = int.Parse(dr["off60"].ToString());
                rs.mattinhieuduoi60 = int.Parse(dr["60dc"].ToString());
                rs.mattinhieutren60 = int.Parse(dr["dc60"].ToString());
                rs.tong = int.Parse(dr["total"].ToString());

            }
            catch
            {
                rs.tructuyen = 0;
                rs.chuadangnhapduoi60 = 0;
                rs.chuadangnhaptren60 = 0;
                rs.mattinhieutren60 = 0;
                rs.mattinhieuduoi60 = 0;
                rs.tong = 0;
            }
            return rs;
        }
        public DataSet BaoCaoTongHopTheoKhachHang(int IDQLLH, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("from", dtFrom),
            new SqlParameter("to", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoTongHopTheoKhachHang", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoChuyenDo(DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("from", dtFrom),
            new SqlParameter("to", dtTo),
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoChuyenDo", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoThuHoiCongNo(int ID_QLLH, int ID_QuanLy, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@ID_KhachHang", ID_KhachHang),
                new SqlParameter("@ID_NhanVien", ID_NhanVien),
                new SqlParameter("@dtFrom", dtFrom),
                new SqlParameter("@dtTo", dtTo)
            };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoThuHoiCongNo", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoThuHoiCongNo_ChiTiet(int ID_QLLH, int ID_QuanLy, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@ID_KhachHang", ID_KhachHang),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("dtFrom", dtFrom),
                new SqlParameter("dtTo", dtTo)
                };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_BaoCaoThuHoiCongNo_ChiTiet", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoTatBatGPS(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, int Loai)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("IDNV", ID_NhanVien),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
                new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("Loai", Loai),
                };
            try
            {
                dt = helper.ExecuteDataSet("sp_BaoCaoMatTinHieuGPS", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }
        public DataSet BaoCaoTatBatFakeGPS(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, int Loai)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("IDNV", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd HH:MM:ss")),
             new SqlParameter("Loai", Loai),
        };
            try
            {
                dt = helper.ExecuteDataSet("sp_BaoCaoBatTatFake", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }

        public DataSet BaoCaoDungDo(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, float sec = 0)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("@sec", sec),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd HH:mm:ss")),
             };
            try
            {
                dt = helper.ExecuteDataSet("sp_BaoCaoDungDo_vuongtm_v1", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }

        public DataTable BaoCaoKmDiChuyen(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataTable dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd 23:59:59")),

        };
            try
            {
                dt = helper.ExecuteDataSet("sp_NhanVien_BaoCaoKMDiChuyen", pars).Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }
        public DataTable BaoCaoKmDiChuyen_TrongNgay(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataTable dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
           new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd 23:59:59")),

        };
            try
            {
                dt = helper.ExecuteDataSet("sp_NhanVien_BaoCaoKMDiChuyen_TrongNgay", pars).Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }

        public DataSet BaoCaoLichSuThaoTac(int ID_QLLH, int ID_QuanLy, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int Loai)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(BaoCao_dl));
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", dtTo.ToString("yyyy-MM-dd 23:59:59")),
             new SqlParameter("@Loai", Loai),
        };
            try
            {
                log.Info("sp_BaoCao_LichSuThaoTac : ID_QLLH - " + ID_QLLH + " ID_QuanLy - " + ID_QuanLy + " ID_KhachHang - " + ID_KhachHang + " ID_NhanVien - " + ID_NhanVien + " dtFrom - " + dtFrom.ToString("yyyy-MM-dd") + " dtTo - " + dtTo.ToString("yyyy-MM-dd"));
                ds = helper.ExecuteDataSet("sp_BaoCao_LichSuThaoTac", pars);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
            return ds;
        }


        public DataSet BaoCaoViengThamKhachHang(int ID_QLLH, int ID_QuanLy, int ID_Nhom, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
             new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@TuNgay", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("@DenNgay", dtTo.ToString("yyyy-MM-dd 23:59:59"))
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_BaoCao_ViengThamTheoKhachHang", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoDoanhThuTongHopNhanVien(int ID_QLLH, int ID_QuanLy, int ID_Nhom, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
             new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@TuNgay", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("@DenNgay", dtTo.ToString("yyyy-MM-dd 23:59:59"))
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoDoanhThu_TongHop", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }


        public DataSet BaoCaoKhachHangMoMoi(int ID_NhanVien, int ID_KhachHang, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, int SoDonHang, double GiaTriDonHang)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
                new SqlParameter("TuNgay", TuNgay.ToString("yyyy-MM-dd")),
                new SqlParameter("DenNgay", DenNgay.ToString("yyyy-MM-dd 23:59:59")),
                new SqlParameter("SoDonHang", SoDonHang),
                new SqlParameter("GiaTriDonHang", GiaTriDonHang),

                };
            try
            {
                dt = helper.ExecuteDataSet("sp_BaoCaoKhachHangMoMoi", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }

        public DataSet BaoCaoKhachHangTheoGiaoDich(int ID_NhanVien, int ID_KhachHang, int ID_QuanLy, int ID_QLLH, int SoNgay, int Loai)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
                new SqlParameter("Loai", Loai),
                new SqlParameter("SoNgay", SoNgay),
                };
            try
            {
                dt = helper.ExecuteDataSet("sp_BaoCaoKhachHangTheoGiaoDich", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }


        public DataTable BaoCaoTongHopPhanHoi(int ID_NhanVien, int ID_KhachHang, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataTable dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("TuNgay", TuNgay.ToString("yyyy-MM-dd")),
           new SqlParameter("DenNgay", DenNgay.ToString("yyyy-MM-dd 23:59:59")),

        };
            try
            {
                dt = helper.ExecuteDataSet("sp_PhanHoi_BaoCaoTongHop", pars).Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }
        public DataTable BaoCaoChiTietPhanHoi(int ID_NhanVien, int ID_KhachHang, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, int ID_PhanHoi)
        {
            DataTable dt = null;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_PhanHoi", ID_PhanHoi),
              new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("TuNgay", TuNgay.ToString("yyyy-MM-dd")),
           new SqlParameter("DenNgay", DenNgay.ToString("yyyy-MM-dd 23:59:59")),

        };
            try
            {
                dt = helper.ExecuteDataSet("sp_PhanHoi_BaoCaoChiTiet", pars).Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                dt = null;
            }

            return dt;
        }

        public DataSet BaoCaoAnhChupTheoAlbum(int idcty, int idnhanvien, int idkhachhang, DateTime tungay, DateTime denNgay, int ID_QuanLy)
        {
            DataSet dt = null;
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", idcty),
                new SqlParameter("ID_NhanVien", idnhanvien),
                new SqlParameter("ID_KhachHang", idkhachhang),
                new SqlParameter("dtFrom", tungay.ToString("yyyy-MM-dd")),
                new SqlParameter("dtTo", denNgay.ToString("yyyy-MM-dd 23:59:59")),
                new SqlParameter("ID_QuanLy", ID_QuanLy)
                };
            try
            {
                dt = db.ExecuteDataSet("sp_web_BaoCaoAnhChup_album_v1", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return dt;
        }

        public DataSet BaoCaoTongHopCheckInTheoKhachHang(int IDQLLH, int IDNV, DateTime TuNgay, DateTime DenNgay, int ID_QuanLy, int ID_KhachHang, int idNhom)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", IDNV),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_Nhom", idNhom)
        };

            DataSet ds = helper.ExecuteDataSet("sp_vuongtm_QL_BaoCaoTongHopVaoDiemTheoKhachHang", pars);
            return ds;
        }
        public DataSet BaoCaoPhanHoi(int id_QLLH, int id_QuanLy, int id_NhanVien, int id_KhachHang, DateTime TuNgay, DateTime DenNgay, int AllFrom, int AllTo)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("ID_QLLH", id_QLLH),
            new SqlParameter("ID_QuanLy", id_QuanLy),
            new SqlParameter("ID_NhanVien", id_NhanVien),
            new SqlParameter("ID_KhachHang", id_KhachHang),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("AllFrom", AllFrom),
            new SqlParameter("AllTo", AllTo)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoPhanHoi", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoKhachHangTheoKhuVuc(int id_QLLH, int ID_Tinh, int ID_Quan)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("ID_Tinh", ID_Tinh),
            new SqlParameter("ID_Quan", ID_Quan)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoKhachHangTheoKhuVuc", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoLichSuGiaoHang(int id_QLLH, int idKhachHang, int idNhanVien, DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("idKhachHang", idKhachHang),
            new SqlParameter("idNhanVien", idNhanVien),
            new SqlParameter("from", from),
            new SqlParameter("to", to)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_LichSuGiaoHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoLichSuBaoDuongSuaChua(DateTime from, DateTime to, int ID_QLLH, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("from", from),
            new SqlParameter("to", to),
             new SqlParameter("ID_QuanLy", ID_QuanLy),
             new SqlParameter("ID_QLLH", ID_QLLH)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoLichSuBaoDuongSuaChua", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoKPINhanVien(int id_QLLH, int id_QuanLy, int id_Nhom, int id_NhanVien, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", id_QLLH),
            new SqlParameter("@ID_QuanLy", id_QuanLy),
            new SqlParameter("@ID_NhanVien", id_NhanVien),
            new SqlParameter("@ID_Nhom", id_Nhom),
            new SqlParameter("@dtFrom", TuNgay),
            new SqlParameter("@dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_BaoCao_KPI", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoMH_NV_New(int IDQLLH, int ID_Hang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy, int idNganhHang = 0)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("@idNganhHang", idNganhHang)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_QL_BaoCaoMHNV_v1", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoCheckInTuyen(int id_QLLH, int id_tuyen, int idnhanvien, DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@id_QLLH", id_QLLH),
            new SqlParameter("@id_tuyen", id_tuyen),
            new SqlParameter("@from", from),
            new SqlParameter("@to", to),
            new SqlParameter("@idnhanvien", idnhanvien)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_sontq_BaoCaoCheckInTheoTuyen_v2", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet LichSuMatTinHieu(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
            try
            {
                ds = db.ExecuteDataSet("sp_LichSuMatTinHieu_GetAll", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoChiTieuKPIViengTham(int id_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@id_qllh", id_QLLH),
            new SqlParameter("@from", TuNgay),
            new SqlParameter("@to", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BieuDoKPI_ViengTham", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoChiTieuKPICongViec(int nhom, DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@nhom", nhom),
            new SqlParameter("@from", from),
            new SqlParameter("@to", to)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoKPICongViec", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet GetDanhSachLichHen(int ID_NhanVien, int ID_KhachHang, DateTime TuNgay, DateTime DenNgay, int ID_QLLH, int ID_QuanLy)
        {
            DataSet dt = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@ID_NhanVien", ID_NhanVien),
                new SqlParameter("@ID_KhachHang", ID_KhachHang),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay),
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_QuanLy", ID_QuanLy)
                };
                dt = db.ExecuteDataSet("sp_LichHen_GetDanhSachLichHen", param);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;

            }
            return dt;
        }
        public DataSet baoCaoViengThamKhachHangTheoTuyenChiTiet(int id_QLLH, int ID_Tuyen, int ID_NhanVien, DateTime tungay, DateTime denngay)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_QLLH", id_QLLH),
                new SqlParameter("@id_tuyen", ID_Tuyen),
                new SqlParameter("@from", tungay),
                new SqlParameter("@to", denngay),
                new SqlParameter("@idnhanvien", ID_NhanVien),
            };

            DataSet ds = new DataSet();
            try
            {
                ds = helper.ExecuteDataSet("sp_BaoCaoCheckInTheoTuyen_Kendo", Param);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoChiTietCheckInTuyen(int id_tuyen, int idnhanvien, DateTime ngay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@id_tuyen", id_tuyen),
                new SqlParameter("@ngay", ngay),
                new SqlParameter("@idnhanvien", idnhanvien)
                };
            try
            {
                ds = helper.ExecuteDataSet("sp_sontq_BaoCaoChiTietCheckInTheoTuyen", Parammeter);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ds;
        }

        public DataSet baoCaoViengThamKhachHangTheoTuyenSoLuong(int id_QLLH, int ID_Tuyen, int ID_NhanVien, DateTime tungay, DateTime denngay)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_QLLH", id_QLLH),
                new SqlParameter("@id_tuyen", ID_Tuyen),
                new SqlParameter("@from", tungay),
                new SqlParameter("@to", denngay),
                new SqlParameter("@idnhanvien", ID_NhanVien),
            };
            DataSet ds = new DataSet();
            try
            {
                ds = helper.ExecuteDataSet("sp_BaoCaoViengThamKHTheoTuyen_Kendo", Param);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoCongViecNhanVien(int id_QLLH, int id_QuanLy, int id_Nhom, int id_NhanVien, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("ID_QLLH", id_QLLH),
            new SqlParameter("ID_QuanLy", id_QuanLy),
            new SqlParameter("ID_NhanVien", id_NhanVien),
            new SqlParameter("ID_Nhom", id_Nhom),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BaoCaoCongViecNhanVien", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCao_CongNoKhachhang(int idqllh, int idquanly, int idnv, int idkh, DateTime begindate, DateTime enddate)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", idqllh),
                new SqlParameter("@ID_QuanLy", idquanly),
                new SqlParameter("@ID_KhachHang", idkh),
                new SqlParameter("@ID_NhanVien", idnv),
                new SqlParameter("@DenNgay", enddate.ToString("yyyy-MM-dd 23:59:59")),
                new SqlParameter("@tuNgay", begindate)
            };
            try
            {
                ds = helper.ExecuteDataSet("sp_vuongtm_BaoCao_CongNoKhachHang_v1", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoTongHopNhapXuatTon(int id_QLLH, DateTime from, DateTime to, int idKho, int idMatHang, int loai)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@id_QLLH", id_QLLH),
            new SqlParameter("from", from),
            new SqlParameter("to", to),
            new SqlParameter("@idKho", idKho),
            new SqlParameter("@idHang", idMatHang),
            new SqlParameter("@loai", loai)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoTongHopNhapXuatTon", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoChiTietNhapXuatTon(int id_QLLH, DateTime from, DateTime to, int idKho, int idMatHang, int loaibiendong, int loai)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@id_QLLH", id_QLLH),
            new SqlParameter("from", from),
            new SqlParameter("to", to),
            new SqlParameter("@idKho", idKho),
            new SqlParameter("@idHang", idMatHang),
            new SqlParameter("@loaibiendong", loaibiendong),
            new SqlParameter("@loai", loai)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoChiTietNhapXuatTon", Parammeter);
            }
            catch (Exception ex)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoTongHopNhapXuatTonCacKho(int id_QLLH, DateTime from, DateTime to, int idKho, int idMatHang, int loai)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@id_QLLH", id_QLLH),
            new SqlParameter("from", from),
            new SqlParameter("to", to),
            new SqlParameter("@idKho", idKho),
            new SqlParameter("@idHang", idMatHang),
            new SqlParameter("@loai", loai)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_BaoCaoTongHopNhapXuatTonCacKho", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoTopTenSanPhamTheoKhachHang(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OrderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OrderBy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenSanPhamTheoKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet BaoCaoBieuDoDonHangTheoKhachHang(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OderBy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenDonHangTheoKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoTopTenTheoNhanVien(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OrderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OrderBy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenTheoNhanVien", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoDonHangTheoKhuVuc(int LoaiKhuVuc, int id_QLLH, DateTime TuNgay, DateTime DenNgay)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("LoaiKhuVuc", LoaiKhuVuc),
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoDonHangTheoKhuVuc", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoPhanLoaiKhachHang(int id_QLLH)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoPhanLoaiKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BieuDoPhanLoaiKhachHangNganhHang(int id_QLLH)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoPhanLoaiKhachHangNganhHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet BaoCaoBieuDoDoanhThuTheoKhachHang(int id_QLLH, DateTime TuNgay, DateTime DenNgay, int OrderBy)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("id_QLLH", id_QLLH),
            new SqlParameter("dtFrom", TuNgay),
            new SqlParameter("dtTo", DenNgay),
            new SqlParameter("OrderBy", OrderBy)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_web_BieuDoTopTenDoanhThuTheoKhachHang", Parammeter);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataTable BaoCaoSoatVeKhachHang(DateTime tungay, DateTime dengay, string sitecode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", dengay),
                new SqlParameter("@sitecode", sitecode)
                };

                ds = helper.ExecuteDataSet("sp_App_GetDSBaoCaoSoatVe", pars);

            }
            catch (Exception ex)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public DataTable BaoCaoCongSoatVe(DateTime tungay, DateTime dengay,string sitecode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", dengay),
                new SqlParameter("@sitecode", sitecode),
                };

                ds = helper.ExecuteDataSet("sp_App_GetDSCongSoatVe", pars);

            }
            catch (Exception ex)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public DataTable DanhSachACMBD(DateTime tungay, DateTime dengay,string sitecode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", dengay),
                new SqlParameter("@sitecode", sitecode),
                };

                ds = helper.ExecuteDataSet("sp_App_GetDSACM", pars);

            }
            catch (Exception ex)
            {
                return null;
            }
            return ds.Tables[0];
        }
    }
}