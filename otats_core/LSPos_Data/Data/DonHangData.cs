using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static System.Net.WebRequestMethods;

namespace LSPos_Data.Data
{
    public class DonHangData
    {
        private SqlDataHelper helper;
        public DonHangData()
        {
            helper = new SqlDataHelper();
        }
        public class ResultPaging
        {
            public List<DonHang> li { set; get; }
            public double TongTien { set; get; }
            public double TienDaThanhToan { set; get; }
            public double ConLai { set; get; }

        }

        public class FilterGridDonHang
        {
            public FilterGridDonHang()
            {
                this.MaDonHang = "";
                this.TenKhachHang = "";
                this.NgayLap = new DateTime(1900, 1, 1);
                this.TenNhanVien = "";
                this.MaKhachHang = "";
                this.DienThoai = "";
                this.DiaChi = "";
                this.GhiChu = "";
                this.ViTriTao = "";
                this.TrangThaiDonHang = -1;
                this.TrangThaiGiaoHang = 0;
                this.TongTien = -1;
                this.DaThanhToan = -1;
                this.ConLai = -1;
            }
            public string MaDonHang { get; set; }
            public string TenKhachHang { get; set; }
            public DateTime NgayLap { get; set; }
            public string TenNhanVien { get; set; }
            public string MaKhachHang { get; set; }
            public string DienThoai { get; set; }
            public string DiaChi { get; set; }
            public string GhiChu { get; set; }
            public int TrangThaiDonHang { get; set; }
            public int TrangThaiGiaoHang { get; set; }
            public float TongTien { get; set; }
            public float DaThanhToan { get; set; }
            public float ConLai { get; set; }
            public string ViTriTao { get; set; }
        }

        public ResultPaging GetDSDonHangAll(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa, string ListIDNhom,
            int startRecord, int maxRecords, FilterGridDonHang filters, ref int TongSo)
        {
            DonHang_dl donHang_dl = new DonHang_dl();
            if (ListIDNhom.EndsWith(","))
            {
                ListIDNhom = ListIDNhom.Substring(0, ListIDNhom.Length - 1);
            }
            ResultPaging result = new ResultPaging();
            List<DonHang> dsdh = new List<DonHang>();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("from", from),
                new SqlParameter("to", to),
                new SqlParameter("trangthaigiao", trangthaigiao),
                new SqlParameter("trangthaithanhtoan", trangthaithanhtoan),
                new SqlParameter("trangthaihoanthanh",trangthaihoanthanh)  ,
                new SqlParameter("ID_KhachHang", ID_KhachHang),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("ID_HangHoa", ID_HangHoa),
                new SqlParameter("ListIDNhom", ListIDNhom),

                new SqlParameter("startRecord", startRecord),
                new SqlParameter("maxRecords", maxRecords),

                new SqlParameter("@maDonHang", filters.MaDonHang),
                new SqlParameter("@tenKhachHang", filters.TenKhachHang),
                new SqlParameter("@ngayLap", filters.NgayLap),
                new SqlParameter("@tenNhanVienLap", filters.TenNhanVien),
                new SqlParameter("@maKhachHang", filters.MaKhachHang),
                new SqlParameter("@dienthoai", filters.DienThoai),
                new SqlParameter("@diaChi", filters.DiaChi)  ,
                new SqlParameter("@ghiChu", filters.GhiChu),
                new SqlParameter("@trangThaiDonHang", filters.TrangThaiDonHang),
                new SqlParameter("@trangThaiGiaoHang", filters.TrangThaiGiaoHang),
                new SqlParameter("@tongTien", filters.TongTien),
                new SqlParameter("@daThanhToan", filters.DaThanhToan),
                new SqlParameter("@conLai", filters.ConLai),
                new SqlParameter("@viTriTao", filters.ViTriTao)
            };

            DataSet ds = helper.ExecuteDataSet("sp_vuongtm_QL_GetAllDonHang_paging", pars);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            DataTable dt2 = ds.Tables[2];

            if (dt1.Rows.Count > 0)
            {
                TongSo = int.Parse(dt1.Rows[0]["soluong"].ToString());
            }
            else
            {
                TongSo = 0;
            }

            double TongTien = 0;
            double TienDaThanhToan = 0;
            double ConLai = 0;
            if (dt2.Rows.Count > 0)
            {
                TongTien = double.Parse(dt2.Rows[0]["TongTien"].ToString());
                TienDaThanhToan = double.Parse(dt2.Rows[0]["TienDaThanhToan"].ToString());
                ConLai = double.Parse(dt2.Rows[0]["ConLai"].ToString());
            }

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DonHang dh = donHang_dl.GetDonHangFromDataRow(dr);
                    dsdh.Add(dh);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            result.li = dsdh;
            result.TongTien = TongTien;
            result.TienDaThanhToan = TienDaThanhToan;
            result.ConLai = ConLai;
            return result;
        }

        public string getlistid(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa, string ListIDNhom, FilterGridDonHang filters)
        {
            DonHang_dl donHang_dl = new DonHang_dl();
            if (ListIDNhom.EndsWith(","))
            {
                ListIDNhom = ListIDNhom.Substring(0, ListIDNhom.Length - 1);
            }
            ResultPaging result = new ResultPaging();
            List<DonHang> dsdh = new List<DonHang>();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("from", from),
                new SqlParameter("to", to),
                new SqlParameter("trangthaigiao", trangthaigiao),
                new SqlParameter("trangthaithanhtoan", trangthaithanhtoan),
                new SqlParameter("trangthaihoanthanh",trangthaihoanthanh)  ,
                new SqlParameter("ID_KhachHang", ID_KhachHang),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("ID_HangHoa", ID_HangHoa),
                new SqlParameter("ListIDNhom", ListIDNhom),

                new SqlParameter("@maDonHang", filters.MaDonHang),
                new SqlParameter("@tenKhachHang", filters.TenKhachHang),
                new SqlParameter("@ngayLap", filters.NgayLap),
                new SqlParameter("@tenNhanVienLap", filters.TenNhanVien),
                new SqlParameter("@maKhachHang", filters.MaKhachHang),
                new SqlParameter("@dienthoai", filters.DienThoai),
                new SqlParameter("@diaChi", filters.DiaChi)  ,
                new SqlParameter("@ghiChu", filters.GhiChu),
                new SqlParameter("@trangThaiDonHang", filters.TrangThaiDonHang),
                new SqlParameter("@trangThaiGiaoHang", filters.TrangThaiGiaoHang),
                new SqlParameter("@tongTien", filters.TongTien),
                new SqlParameter("@daThanhToan", filters.DaThanhToan),
                new SqlParameter("@conLai", filters.ConLai),
                new SqlParameter("@viTriTao", filters.ViTriTao)
            };

            DataSet ds = helper.ExecuteDataSet("sp_vuongtm_QL_Getlistiddonhang", pars);
            DataTable dt = ds.Tables[0];

            string listid = "";

            try
            {
                int i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    if (i == 1)
                    {
                        listid += dr["ID_DonHang"].ToString();
                        i = 2;
                    }
                    else
                    {
                        listid += "," + dr["ID_DonHang"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return listid;
        }
        public bool HuyDonHang(string LyDo, int ID_QuanLy, string ListID)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@LyDo", LyDo),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@ListID", ListID)
            };

                if (helper.ExecuteNonQuery("usp_vuongtm_QL_HuyDonHang_multi", pars) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        
        public bool GiaHanVe(long ID_DonHang, DateTime Ngay)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_DonHang", ID_DonHang),
                new SqlParameter("@Ngay", Ngay)
            };

                if (helper.ExecuteNonQuery("sp_DonHang_GiaHanVe", pars) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public DataTable GetDSTrangThaiGiaoHang(int ID_QuanLy, string lang)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@lang", lang)
            };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllTrangThaiGiaoHang_v1", pars);
            DataTable dt = ds.Tables[0];

            return dt;
        }
        public int GetTrangThaiGiaoHang_ByName(string name)
        {
            int result = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@name", name)
                };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetTrangThaiGiaoHang_ByName", pars);
                DataTable dt = ds.Tables[0];

                result = int.Parse(dt.Rows[0]["ID_TrangThaiGiaoHang"].ToString());
            }
            catch { }
            return result;
        }
        public DataTable GetDSTrangThaiThanhToan(int ID_QuanLy, string lang)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@lang", lang)
            };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllTrangThaiThanhToan_v1", pars);
            DataTable dt = ds.Tables[0];

            return dt;
        }
        public string GetMaThamChieu(int ID_QLLH, string MaThamChieu)
        {
            string tontai = "";
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@MaThamChieu", MaThamChieu)
            };
                object obj = helper.ExecuteScalar("sp_DonHang_CheckTrungMaThamChieu", par);
                if (obj != null)
                {
                    tontai = obj.ToString();
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return tontai;
        }

        public int CheckEchotoken(string EchoToken)
        {
            int tontai = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@EchoToken", EchoToken)
            };
                object obj = helper.ExecuteScalar("sp_DonHang_CheckTrungEchoToken", par);
                if (obj != null)
                {
                    tontai = int.Parse(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return tontai;
        }


        public DonHangModels TaoDonHang(DonHangModels dh)
        {
            if (dh.idctkm == 0)
            {
                dh.tongtienchietkhau = 0;
            }

            DonHangModels rs = new DonHangModels();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idct", dh.idct),
                new SqlParameter("@idnhanvien", dh.idnhanvien),
                new SqlParameter("@hdv", dh.hdv),
                new SqlParameter("@idcuahang", dh.idcuahang),
                new SqlParameter("@tongtien", dh.tongtien),
                new SqlParameter("@ghichu", dh.ghichu),
                new SqlParameter("@mathamchieu", dh.mathamchieu),
                new SqlParameter("@thoigiantao", dh.thoigiantao),
                new SqlParameter("@idcheckin", dh.idcheckin),
                new SqlParameter("@idctkm", dh.idctkm),
                new SqlParameter("@tongtienchietkhau", dh.tongtienchietkhau),
                new SqlParameter("@chietkhautien", dh.chietkhautien),
                new SqlParameter("@chietkhauphantram", dh.chietkhauphantram),
                new SqlParameter("@chietkhautien_khac", dh.chietKhauTienKhac),
                new SqlParameter("@chietkhauphantram_khac", dh.chietKhauPhanTramKhac),
                new SqlParameter("@chietkhautien_theoctkm", dh.chietkhautien_theoctkm),
                new SqlParameter("@chietkhauphantram_theoctkm", dh.chietkhauphantram_theoctkm),
                new SqlParameter("@phuthuphantram", dh.phuthuphantram),
                new SqlParameter("@phuthutien", dh.phuthutien),
                new SqlParameter("@lydophuthu", dh.lydophuthu),
                new SqlParameter("@idnhanvientao", dh.idnhanvientao),
                new SqlParameter("@kinhdo", dh.kinhdo),
                new SqlParameter("@vido", dh.vido),
                new SqlParameter("@diachitao", dh.diachitao),
                new SqlParameter("@diachixuathoadon", dh.diachixuathoadon),
                new SqlParameter("@accountcode", dh.LS_AccountCode),
                new SqlParameter("@bookingcode", dh.LS_BookingCode),
                new SqlParameter("@xuathoadon", dh.xuathoadon),
                new SqlParameter("@hinhthucban", dh.hinhthucban),
                new SqlParameter("@invetaiquay", dh.invetaiquay)

            };
                rs.iddonhang = int.Parse(helper.ExecuteScalar("sp_App_TaoDonHang", par).ToString());
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }
        public int TaoChiTietDonHang(ChiTietDonHangModels cdh)
        {
            int ID = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                    new SqlParameter("@iddonhang", cdh.iddonhang),
                    new SqlParameter("@idhanghoa", cdh.idhanghoa),
                    new SqlParameter("@soluong", cdh.soluong),
                    new SqlParameter("@ghichu", cdh.ghichu),
                    new SqlParameter("@hinhthucban", cdh.hinhthucban),
                    new SqlParameter("@giakhac", cdh.giakhac),
                    new SqlParameter("@idctkm", cdh.idctkm),
                    new SqlParameter("@tongtienchietkhau", cdh.tongtienchietkhau),
                    new SqlParameter("@chietkhautien", cdh.chietkhautien),
                    new SqlParameter("@chietkhauphantram", cdh.chietkhauphantram),
                    new SqlParameter("@phantramhaohut", cdh.phantramhaohut),
                    new SqlParameter("@soluonghaohut", cdh.soluonghaohut),
                    new SqlParameter("@idhaohut", cdh.idhaohut),
                    new SqlParameter("@HangKhuyenMai", cdh.HangKhuyenMai),
                    new SqlParameter("@Ngay", cdh.Ngay),
                    new SqlParameter("@ID_ChiTietDonHang_DatKhuyenMai", cdh.ID_ChiTietDonHang_DatKhuyenMai),


                    };
                object obj = helper.ExecuteScalar("sp_App_TaoChiTietDonHang_v2", par);
                if (obj != null)
                {
                    ID = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return ID;
        }

        public int TaoChiTietDonHangLS(ChiTietDonHangModels cdh)
        {
            int ID = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                    new SqlParameter("@iddonhang", cdh.iddonhang),
                    new SqlParameter("@idhanghoa", cdh.idhanghoa),
                    new SqlParameter("@soluong", cdh.soluong),
                    new SqlParameter("@Ngay", cdh.Ngay),


                    };
                object obj = helper.ExecuteScalar("sp_App_TaoChiTietDonHang_LSTour", par);
                if (obj != null)
                {
                    ID = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return ID;
        }
        public DonHangModels LayDonHang(string MaThamChieu)
        {
            DonHangModels rs = new DonHangModels();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@MaThamChieu", MaThamChieu)

            };


                DataTable dt = helper.ExecuteDataSet("selectDonHangTheoMaThamCieu", par).Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs = new DonHangModels();
                        rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                        rs.iddonhang = int.Parse(dr["ID_DonHang"].ToString());
                        rs.idnhanvien = int.Parse(dr["ID_NhanVien"].ToString());
                        rs.isProcess = int.Parse(dr["IsProcess"].ToString());
                        rs.idcuahang = int.Parse(dr["ID_KhachHang"].ToString());
                        double TongChietKhauMatHang = 0;
                        try
                        {
                            TongChietKhauMatHang = dr["TongChietKhauMatHang"].ToString() != "" ? double.Parse(dr["TongChietKhauMatHang"].ToString()) : 0;
                        }
                        catch (Exception)
                        {
                        }

                        double ChietKhauTienTrucTiep = 0;
                        try
                        {
                            ChietKhauTienTrucTiep = dr["ChietKhauTienTrucTiep"].ToString() != "" ? double.Parse(dr["ChietKhauTienTrucTiep"].ToString()) : 0;
                        }
                        catch (Exception)
                        {
                        }
                        rs.tongtien = double.Parse(dr["TongTien"].ToString());

                        //rs.tongtien = double.Parse(dr["TongTien"].ToString()) + TongChietKhauMatHang;
                        rs.thoigiantao = Convert.ToDateTime(dr["CreateDate"].ToString());
                        rs.ghichu = dr["GhiChu"].ToString();
                        rs.mathamchieu = dr["MaThamChieu"].ToString();
                        rs.trangthaigiaohang = dr["ID_TrangThaiGiaoHang"].ToString() != "" ? int.Parse(dr["ID_TrangThaiGiaoHang"].ToString()) : 0;
                        rs.trangthaithanhtoan = dr["ID_TrangThaiThanhToan"].ToString() != "" ? int.Parse(dr["ID_TrangThaiThanhToan"].ToString()) : 0;
                        rs.trangthaidonhang = dr["ID_TrangThaiDonHang"].ToString() != "" ? int.Parse(dr["ID_TrangThaiDonHang"].ToString()) : 0;
                        rs.idctkm = dr["ID_CTKM"].ToString() != "" ? int.Parse(dr["ID_CTKM"].ToString()) : 0;
                        rs.tenctkm = dr["TenCTKM"].ToString();
                        rs.chietkhauphantram = dr["ChietKhauPhanTram"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                        rs.chietkhautien = dr["ChietKhauTien"].ToString() != "" ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;
                        //rs.tongtienchietkhau = dr["TongTienChietKhau"].ToString() != "" ? double.Parse(dr["TongTienChietKhau"].ToString()) : 0;

                        rs.tongtienchietkhau = ChietKhauTienTrucTiep;
                        rs.tenkhachhang = dr["TenKhachHang"].ToString();
                        rs.chitietdonhang = LayChiTietDonHang(rs.iddonhang);
                        rs.macuahang = dr["MaKH"].ToString();
                        rs.tiendathanhtoan = dr["TienDaThanhToan"].ToString() != "" ? double.Parse(dr["TienDaThanhToan"].ToString()) : 0;
                        rs.tenvilspay = dr["TenNhom"].ToString();

                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }


                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }
        public DonHangModels LayDonHang(int iddh)
        {
            DonHangModels rs = new DonHangModels();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_DonHang", iddh)

            };


                DataTable dt = helper.ExecuteDataSet("selectDonHangTheoID", par).Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs = new DonHangModels();
                        rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                        rs.iddonhang = int.Parse(dr["ID_DonHang"].ToString());
                        rs.idnhanvien = int.Parse(dr["ID_NhanVien"].ToString());
                        rs.isProcess = int.Parse(dr["IsProcess"].ToString());
                        rs.idcuahang = int.Parse(dr["ID_KhachHang"].ToString());
                        double TongChietKhauMatHang = 0;
                        try
                        {
                            TongChietKhauMatHang = dr["TongChietKhauMatHang"].ToString() != "" ? double.Parse(dr["TongChietKhauMatHang"].ToString()) : 0;
                        }
                        catch (Exception)
                        {
                        }

                        double ChietKhauTienTrucTiep = 0;
                        try
                        {
                            ChietKhauTienTrucTiep = dr["ChietKhauTienTrucTiep"].ToString() != "" ? double.Parse(dr["ChietKhauTienTrucTiep"].ToString()) : 0;
                        }
                        catch (Exception)
                        {
                        }
                        rs.tongtien = double.Parse(dr["TongTien"].ToString());

                        //rs.tongtien = double.Parse(dr["TongTien"].ToString()) + TongChietKhauMatHang;
                        rs.thoigiantao = Convert.ToDateTime(dr["CreateDate"].ToString());
                        rs.ghichu = dr["GhiChu"].ToString();
                        rs.mathamchieu = dr["MaThamChieu"].ToString();
                        rs.trangthaigiaohang = dr["ID_TrangThaiGiaoHang"].ToString() != "" ? int.Parse(dr["ID_TrangThaiGiaoHang"].ToString()) : 0;
                        rs.trangthaithanhtoan = dr["ID_TrangThaiThanhToan"].ToString() != "" ? int.Parse(dr["ID_TrangThaiThanhToan"].ToString()) : 0;
                        rs.trangthaidonhang = dr["ID_TrangThaiDonHang"].ToString() != "" ? int.Parse(dr["ID_TrangThaiDonHang"].ToString()) : 0;
                        rs.idctkm = dr["ID_CTKM"].ToString() != "" ? int.Parse(dr["ID_CTKM"].ToString()) : 0;
                        rs.tenctkm = dr["TenCTKM"].ToString();
                        rs.chietkhauphantram = dr["ChietKhauPhanTram"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                        rs.chietkhautien = dr["ChietKhauTien"].ToString() != "" ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;
                        //rs.tongtienchietkhau = dr["TongTienChietKhau"].ToString() != "" ? double.Parse(dr["TongTienChietKhau"].ToString()) : 0;

                        rs.tongtienchietkhau = ChietKhauTienTrucTiep;
                        rs.tenkhachhang = dr["TenKhachHang"].ToString();
                        rs.chitietdonhang = LayChiTietDonHang(rs.iddonhang);
                        rs.macuahang = dr["MaKH"].ToString();
                        rs.tiendathanhtoan = dr["TienDaThanhToan"].ToString() != "" ? double.Parse(dr["TienDaThanhToan"].ToString()) : 0;
                        rs.tenvilspay = dr["TenNhom"].ToString();

                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }


                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }
        public List<ChiTietDonHangModels> LayChiTietDonHang(int iddonhang)
        {
            List<ChiTietDonHangModels> rs = new List<ChiTietDonHangModels>();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_DonHang", iddonhang)
            };

                DataTable dt = helper.ExecuteDataSet("selectChiTietDonHang", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        ChiTietDonHangModels ctdh = GetChiTietDonHangFromDataRow(dr);
                        rs.Add(ctdh);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }


            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return rs;
        }

        public List<ChiTietDonHangModels> LayChiTietDonHang(int iddonhang, string language)
        {
            List<ChiTietDonHangModels> rs = new List<ChiTietDonHangModels>();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_DonHang", iddonhang)
            };

                DataTable dt = helper.ExecuteDataSet("selectChiTietDonHang", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        ChiTietDonHangModels ctdh = GetChiTietDonHangFromDataRow(dr, language);
                        ctdh.dschitietmathang = GetDSChiTietMatHangDonHang(ctdh.idchitietdonhang, ctdh.iddonhang, ctdh.idhanghoa);
                        ctdh.lstDichVu = GetDSHangHoaDichVuDonHang(ctdh.idchitietdonhang);
                        if (ctdh.IsDichVu > 0)
                        {
                            ctdh.tongTien = ctdh.lstDichVu.Sum(x => x.SoLuong * x.GiaBan);
                        }
                        rs.Add(ctdh);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }


            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return rs;
        }

        public List<ChiTietDonHangModels> LayChiTietDonHangForPrint(int iddonhang, string language)
        {
            List<ChiTietDonHangModels> rs = new List<ChiTietDonHangModels>();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_DonHang", iddonhang)
            };

                DataTable dt = helper.ExecuteDataSet("selectChiTietDonHang", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        ChiTietDonHangModels ctdh = GetChiTietDonHangFromDataRow(dr, language);
                        //ctdh.dschitietmathang = GetDSChiTietMatHangDonHang(ctdh.idchitietdonhang, ctdh.iddonhang, ctdh.idhanghoa);
                        ctdh.lstDichVu = GetDSHangHoaDichVuDonHang(ctdh.idchitietdonhang);
                        if (ctdh.IsDichVu > 0)
                        {
                            ctdh.tongTien = ctdh.lstDichVu.Sum(x => x.SoLuong * x.GiaBan);
                        }
                        rs.Add(ctdh);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }


            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return rs;
        }

        public ChiTietDonHangModels GetChiTietDonHangFromDataRow(DataRow dr, string language)
        {
            try
            {
                ChiTietDonHangModels ctdh = new ChiTietDonHangModels();

                ctdh.idchitietdonhang = int.Parse(dr["ID_ChiTietDonHang"].ToString());
                ctdh.iddonhang = int.Parse(dr["ID_DonHang"].ToString());
                ctdh.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                ctdh.tenhang = dr["TenHang"].ToString();
                ctdh.mahang = dr["MaHang"].ToString();
                ctdh.ghichu = dr["GhiChu"].ToString();
                ctdh.soluong = double.Parse(dr["SoLuong"].ToString());
                ctdh.dagiao = int.Parse(dr["DaGiao"].ToString());
                ctdh.HangKhuyenMai = string.IsNullOrWhiteSpace(dr["HangKhuyenMai"].ToString()) ? 0 : int.Parse(dr["HangKhuyenMai"].ToString());
                ctdh.tendonvi = dr["TenDonVi"].ToString();
                ctdh.hinhthucban = int.Parse(dr["HinhThucBan"].ToString());
                ctdh.tenhinhthucban = (ctdh.HangKhuyenMai == 1) ? (language == "en" ? "Promotion Items" : "Hàng khuyến mại") : ((ctdh.hinhthucban == 1) ? (language == "en" ? "Wholesale Price" : "Giá bán buôn") : (ctdh.hinhthucban == 2) ? (language == "en" ? "Other Price" : "Giá bán khác") : (language == "en" ? "Retail Price" : "Giá bán lẻ"));
                ctdh.giaban = double.Parse(dr["GiaBan"].ToString());
                ctdh.giabuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()) : 0;
                ctdh.giale = dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()) : 0;
                ctdh.giakhac = (double.Parse(dr["GiaKhac"] != DBNull.Value ? dr["GiaKhac"].ToString() : "0"));
                ctdh.chietkhauphantram_banbuon = double.Parse(dr["ChietKhauPhanTram_BanBuon"] != DBNull.Value ? dr["ChietKhauPhanTram_BanBuon"].ToString() : "0");
                ctdh.chietkhauphantram_banle = double.Parse(dr["ChietKhauPhanTram_BanLe"] != DBNull.Value ? dr["ChietKhauPhanTram_BanLe"].ToString() : "0");
                ctdh.chietkhautien_banbuon = double.Parse(dr["ChietKhauTien_BanBuon"] != DBNull.Value ? dr["ChietKhauTien_BanBuon"].ToString() : "0");
                ctdh.chietkhautien_banle = double.Parse(dr["ChietKhauTien_BanLe"] != DBNull.Value ? dr["ChietKhauTien_BanLe"].ToString() : "0");
                ctdh.IsDichVu = (dr["IsDichVu"].ToString() != "") ? int.Parse(dr["IsDichVu"].ToString()) : 0;
                ctdh.tongtienchietkhau = double.Parse(dr["TongTienChietKhau"] != DBNull.Value ? dr["TongTienChietKhau"].ToString() : "0");
                ctdh.chietkhauphantram = double.Parse(dr["ChietKhauPhanTram"] != DBNull.Value ? dr["ChietKhauPhanTram"].ToString() : "0");
                ctdh.chietkhautien = double.Parse(dr["ChietKhauTien"] != DBNull.Value ? dr["ChietKhauTien"].ToString() : "0");
                ctdh.idctkm = dr["ID_CTKM"].ToString() != "" ? int.Parse(dr["ID_CTKM"].ToString()) : 0;
                ctdh.tenctkm = dr["TenCTKM"].ToString();
                ctdh.mahang = dr["MaHang"].ToString();
                ctdh.iddanhmuc = dr["ID_DANHMUC"].ToString() != "" ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                ctdh.tendanhmuc = dr["TenDanhMuc"].ToString();
                ctdh.tendonvi = dr["TenDonVi"].ToString();
                ctdh.ghichugia = dr["GhiChuGia"].ToString();
                ctdh.tonthucte = double.Parse(dr["TonThucTe"] != DBNull.Value ? dr["TonThucTe"].ToString() : "0");
                ctdh.tonorder = double.Parse(dr["TonOrder"] != DBNull.Value ? dr["TonOrder"].ToString() : "0");
                ctdh.dagiao = double.Parse(dr["DaGiao"] != DBNull.Value ? dr["DaGiao"].ToString() : "0");
                //ctdh.TongTien = (ctdh.HinhThucBan == 1) ? (UInt32)(ctdh.SoLuong * giaBanBuon) : (ctdh.HinhThucBan == 2) ? (UInt32)(ctdh.SoLuong * giaKhac) : (UInt32)(ctdh.SoLuong * giaBanLe);
                ctdh.tongTien = (double)ctdh.soluong * ctdh.giaban;
                ctdh.Ngay = dr["Ngay"].ToString() != "" ? Convert.ToDateTime(dr["Ngay"].ToString()) : new DateTime(1900, 1, 1);
                //ctdh.DaThanhToan = double.Parse(dr["DaThanhToan"].ToString());
                ctdh.tenhienthi = dr["TenHang"].ToString();
                ctdh.linkgioithieu = dr["LinkGioiThieu"].ToString();
                ctdh.mota = dr["MoTa"].ToString();
                ctdh.tenKho = dr["TenKho"].ToString();

                try
                {
                    ctdh.tennhacungcap = dr["TenNhaCungCap"].ToString();
                    ctdh.tennhanhieu = dr["TenNhanHieu"].ToString();
                }
                catch (Exception)
                {
                }

                //try
                //{
                //    ctdh.phantramhaohut = double.Parse(dr["PhanTramHaoHut"] != DBNull.Value ? dr["PhanTramHaoHut"].ToString() : "0");
                //    ctdh.soluonghaohut = double.Parse(dr["SoLuongHaoHut"] != DBNull.Value ? dr["SoLuongHaoHut"].ToString() : "0");
                //    ctdh.idhaohut = int.Parse(dr["ID_HaoHut"] != DBNull.Value ? dr["ID_HaoHut"].ToString() : "0");
                //}
                //catch (Exception)
                //{
                //}
                ctdh.idkhoxuat = 0;
                ctdh.tenkhoxuat = "";
                //try
                //{
                //    DataTable dt = GetChuongTrinhKhuyenMai_ChiTietDonHang(ctdh.idchitietdonhang);
                //    if (dt != null)
                //    {
                //        ctdh.tenctkm = "";
                //        KhuyenMai_dl km = new KhuyenMai_dl();
                //        ctdh.chietkhauphantram_banbuon = 0;
                //        ctdh.chietkhautien_banle = 0;
                //        ctdh.chietkhautien_banle = 0;
                //        ctdh.chietkhauphantram_banle = 0;
                //        ctdh.tongtienchietkhau = 0;
                //        double giaban = 0;
                //        foreach (DataRow drr in dt.Rows)
                //        {
                //            KhuyenMai kmai = km.GetKhuyenMaiByID(int.Parse(drr["ID_CTKM"].ToString()));
                //            ctdh.tenctkm += " - " + kmai.TenCTKM + "</br>";
                //            foreach (ChiTietKhuyenMai ctkm in kmai.ChiTietCTKM)
                //            {
                //                if (ctkm.ID_Hang == ctdh.idhanghoa)
                //                {
                //                    switch (ctdh.hinhthucban)
                //                    {
                //                        case 0:
                //                            // Bán lẻ
                //                            ctdh.chietkhauphantram_banle += ctkm.ChietKhauPhanTram_BanLe;
                //                            ctdh.chietkhautien_banle += ctkm.ChietKhauTien_BanLe;
                //                            ctdh.chietkhautien += ctkm.ChietKhauTien_BanLe;
                //                            ctdh.chietkhauphantram += ctkm.ChietKhauPhanTram_BanLe;
                //                            giaban = ctdh.giale;
                //                            break;
                //                        case 1:
                //                            ctdh.chietkhauphantram_banle += ctkm.ChietKhauPhanTram_BanBuon;
                //                            ctdh.chietkhautien_banle += ctkm.ChietKhauTien_BanBuon;
                //                            ctdh.chietkhautien += ctkm.ChietKhauTien_BanBuon;
                //                            ctdh.chietkhauphantram += ctkm.ChietKhauPhanTram_BanBuon;
                //                            giaban = ctdh.giabuon;
                //                            break;
                //                        // Bán buôn
                //                        case 2:
                //                            // Khác
                //                            ctdh.chietkhauphantram_banle += ctkm.ChietKhauPhanTram_BanLe;
                //                            ctdh.chietkhautien_banle += ctkm.ChietKhauTien_BanLe;
                //                            ctdh.chietkhautien += ctkm.ChietKhauTien_BanLe;
                //                            ctdh.chietkhauphantram += ctkm.ChietKhauPhanTram_BanLe;
                //                            giaban = ctdh.giakhac;
                //                            break;
                //                        default:
                //                            break;
                //                    }
                //                }

                //            }

                //        }
                //        ctdh.tongtienchietkhau = ((ctdh.soluong * giaban) * ctdh.chietkhauphantram) / 100 + ctdh.chietkhautien;
                //        ctdh.tongTien = ctdh.soluong * giaban - ctdh.tongtienchietkhau;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    LSPos_Data.Utilities.Log.Error(ex);
                //}
                return ctdh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public ChiTietDonHangModels GetChiTietDonHangFromDataRow(DataRow dr)
        {
            try
            {
                ChiTietDonHangModels ctdh = new ChiTietDonHangModels();

                ctdh.idchitietdonhang = int.Parse(dr["ID_ChiTietDonHang"].ToString());
                ctdh.iddonhang = int.Parse(dr["ID_DonHang"].ToString());
                ctdh.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                ctdh.tenhang = dr["TenHang"].ToString();
                ctdh.mahang = dr["MaHang"].ToString();
                ctdh.ghichu = dr["GhiChu"].ToString();
                ctdh.soluong = double.Parse(dr["SoLuong"].ToString());
                ctdh.dagiao = int.Parse(dr["DaGiao"].ToString());
                ctdh.HangKhuyenMai = string.IsNullOrWhiteSpace(dr["HangKhuyenMai"].ToString()) ? 0 : int.Parse(dr["HangKhuyenMai"].ToString());
                ctdh.tendonvi = dr["TenDonVi"].ToString();
                ctdh.hinhthucban = int.Parse(dr["HinhThucBan"].ToString());
                ctdh.tenhinhthucban = (ctdh.HangKhuyenMai == 1) ? "Hàng khuyến mại" : ((ctdh.hinhthucban == 1) ? "Giá bán buôn" : (ctdh.hinhthucban == 2) ? "Giá bán khác" : "Giá bán lẻ");
                ctdh.giaban = double.Parse(dr["GiaBan"].ToString());
                ctdh.giabuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()) : 0;
                ctdh.giale = dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()) : 0;
                ctdh.giakhac = (double.Parse(dr["GiaKhac"] != DBNull.Value ? dr["GiaKhac"].ToString() : "0"));
                ctdh.chietkhauphantram_banbuon = double.Parse(dr["ChietKhauPhanTram_BanBuon"] != DBNull.Value ? dr["ChietKhauPhanTram_BanBuon"].ToString() : "0");
                ctdh.chietkhauphantram_banle = double.Parse(dr["ChietKhauPhanTram_BanLe"] != DBNull.Value ? dr["ChietKhauPhanTram_BanLe"].ToString() : "0");
                ctdh.chietkhautien_banbuon = double.Parse(dr["ChietKhauTien_BanBuon"] != DBNull.Value ? dr["ChietKhauTien_BanBuon"].ToString() : "0");
                ctdh.chietkhautien_banle = double.Parse(dr["ChietKhauTien_BanLe"] != DBNull.Value ? dr["ChietKhauTien_BanLe"].ToString() : "0");

                ctdh.tongtienchietkhau = double.Parse(dr["TongTienChietKhau"] != DBNull.Value ? dr["TongTienChietKhau"].ToString() : "0");
                ctdh.chietkhauphantram = double.Parse(dr["ChietKhauPhanTram"] != DBNull.Value ? dr["ChietKhauPhanTram"].ToString() : "0");
                ctdh.chietkhautien = double.Parse(dr["ChietKhauTien"] != DBNull.Value ? dr["ChietKhauTien"].ToString() : "0");
                ctdh.idctkm = dr["ID_CTKM"].ToString() != "" ? int.Parse(dr["ID_CTKM"].ToString()) : 0;
                ctdh.tenctkm = dr["TenCTKM"].ToString();
                ctdh.mahang = dr["MaHang"].ToString();
                ctdh.iddanhmuc = dr["ID_DANHMUC"].ToString() != "" ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                ctdh.tendanhmuc = dr["TenDanhMuc"].ToString();
                ctdh.tendonvi = dr["TenDonVi"].ToString();
                ctdh.ghichugia = dr["GhiChuGia"].ToString();
                ctdh.tonthucte = double.Parse(dr["TonThucTe"] != DBNull.Value ? dr["TonThucTe"].ToString() : "0");
                ctdh.tonorder = double.Parse(dr["TonOrder"] != DBNull.Value ? dr["TonOrder"].ToString() : "0");
                ctdh.dagiao = double.Parse(dr["DaGiao"] != DBNull.Value ? dr["DaGiao"].ToString() : "0");
                //ctdh.TongTien = (ctdh.HinhThucBan == 1) ? (UInt32)(ctdh.SoLuong * giaBanBuon) : (ctdh.HinhThucBan == 2) ? (UInt32)(ctdh.SoLuong * giaKhac) : (UInt32)(ctdh.SoLuong * giaBanLe);
                ctdh.tongTien = (double)ctdh.soluong * ctdh.giaban;

                //ctdh.DaThanhToan = double.Parse(dr["DaThanhToan"].ToString());
                ctdh.tenhienthi = dr["TenHang"].ToString();
                ctdh.linkgioithieu = dr["LinkGioiThieu"].ToString();
                ctdh.mota = dr["MoTa"].ToString();
                ctdh.tenKho = dr["TenKho"].ToString();

                try
                {
                    ctdh.tennhacungcap = dr["TenNhaCungCap"].ToString();
                    ctdh.tennhanhieu = dr["TenNhanHieu"].ToString();
                }
                catch (Exception)
                {
                }

                try
                {
                    ctdh.phantramhaohut = double.Parse(dr["PhanTramHaoHut"] != DBNull.Value ? dr["PhanTramHaoHut"].ToString() : "0");
                    ctdh.soluonghaohut = double.Parse(dr["SoLuongHaoHut"] != DBNull.Value ? dr["SoLuongHaoHut"].ToString() : "0");
                    ctdh.idhaohut = int.Parse(dr["ID_HaoHut"] != DBNull.Value ? dr["ID_HaoHut"].ToString() : "0");
                }
                catch (Exception)
                {
                }
                ctdh.idkhoxuat = 0;
                ctdh.tenkhoxuat = "";
                try
                {
                    DataTable dt = GetChuongTrinhKhuyenMai_ChiTietDonHang(ctdh.idchitietdonhang);
                    if (dt != null)
                    {
                        ctdh.tenctkm = "";
                        KhuyenMai_dl km = new KhuyenMai_dl();
                        ctdh.chietkhauphantram_banbuon = 0;
                        ctdh.chietkhautien_banle = 0;
                        ctdh.chietkhautien_banle = 0;
                        ctdh.chietkhauphantram_banle = 0;
                        ctdh.tongtienchietkhau = 0;
                        double giaban = 0;
                        foreach (DataRow drr in dt.Rows)
                        {
                            KhuyenMai kmai = km.GetKhuyenMaiByID(int.Parse(drr["ID_CTKM"].ToString()));
                            ctdh.tenctkm += " - " + kmai.TenCTKM + "</br>";
                            foreach (ChiTietKhuyenMai ctkm in kmai.ChiTietCTKM)
                            {
                                if (ctkm.ID_Hang == ctdh.idhanghoa)
                                {
                                    switch (ctdh.hinhthucban)
                                    {
                                        case 0:
                                            // Bán lẻ
                                            ctdh.chietkhauphantram_banle += ctkm.ChietKhauPhanTram_BanLe;
                                            ctdh.chietkhautien_banle += ctkm.ChietKhauTien_BanLe;
                                            ctdh.chietkhautien += ctkm.ChietKhauTien_BanLe;
                                            ctdh.chietkhauphantram += ctkm.ChietKhauPhanTram_BanLe;
                                            giaban = ctdh.giale;
                                            break;
                                        case 1:
                                            ctdh.chietkhauphantram_banle += ctkm.ChietKhauPhanTram_BanBuon;
                                            ctdh.chietkhautien_banle += ctkm.ChietKhauTien_BanBuon;
                                            ctdh.chietkhautien += ctkm.ChietKhauTien_BanBuon;
                                            ctdh.chietkhauphantram += ctkm.ChietKhauPhanTram_BanBuon;
                                            giaban = ctdh.giabuon;
                                            break;
                                        // Bán buôn
                                        case 2:
                                            // Khác
                                            ctdh.chietkhauphantram_banle += ctkm.ChietKhauPhanTram_BanLe;
                                            ctdh.chietkhautien_banle += ctkm.ChietKhauTien_BanLe;
                                            ctdh.chietkhautien += ctkm.ChietKhauTien_BanLe;
                                            ctdh.chietkhauphantram += ctkm.ChietKhauPhanTram_BanLe;
                                            giaban = ctdh.giakhac;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                            }

                        }
                        ctdh.tongtienchietkhau = ((ctdh.soluong * giaban) * ctdh.chietkhauphantram) / 100 + ctdh.chietkhautien;
                        ctdh.tongTien = ctdh.soluong * giaban - ctdh.tongtienchietkhau;
                    }
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }
                return ctdh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public int SuaDonHang(DonHangv2 dh, int ID_NhanVien, int ID_QuanLy)
        {
            int kq = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@iddonhang", dh.ID_DonHang),
                new SqlParameter("@idct", dh.ID_QLLH),
                new SqlParameter("@idnhanvien", dh.ID_NhanVien),
                new SqlParameter("@idcuahang", dh.ID_KhachHang),
                new SqlParameter("@tongtien", dh.TongTien),
                new SqlParameter("@ghichu", dh.GhiChu),
                new SqlParameter("@mathamchieu", dh.MaThamChieu),

                new SqlParameter("@idcheckin", 0),
                new SqlParameter("@idctkm", dh.ID_CTKM),
                new SqlParameter("@tongtienchietkhau", dh.ID_CTKM > 0 ? dh.TongTienChietKhau  : 0 ),
                new SqlParameter("@chietkhautien", dh.ChietKhauTien),
                new SqlParameter("@chietkhauphantram", dh.ChietKhauPhanTram),
                new SqlParameter("@chietkhautien_khac", dh.ChietKhauTienKhac),
                new SqlParameter("@chietkhauphantram_khac", dh.ChietKhauPhanTramKhac),
                new SqlParameter("@chietkhautien_theoctkm", dh.ChietKhauTienTheoCTKM),
                new SqlParameter("@chietkhauphantram_theoctkm", dh.ChietKhauPhanTramTheoCTKM),
                  new SqlParameter("@LastUpdate_ID_NhanVien", ID_NhanVien),
                    new SqlParameter("@LastUpdate_ID_QuanLy", ID_QuanLy),
                     new SqlParameter("@ID_TrangThaiDongHang", dh.ID_TrangThaiDongHang),
            };

                kq = helper.ExecuteNonQuery("sp_App_SuaDonHang", par);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return kq;
        }
        public int TaoDSChiTietMatHangDonHang(long idChiTietDH, int iddonhang, List<ChiTietMatHangDonHangModels> ds)
        {
            try
            {
                foreach (ChiTietMatHangDonHangModels ct in ds)
                {
                    if (ct.HanSuDung.Year < 1900)
                    {
                        ct.HanSuDung = DateTime.Now.Date;
                        ct.MaVeBoSung = ct.MoTa;
                    }
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@mota", ct.MoTa),
                        new SqlParameter("@idchitietdh", idChiTietDH),
                        new SqlParameter("@iddonhang", iddonhang),
                        new SqlParameter("@idmathang", ct.ID_MatHang),
                        new SqlParameter("@chieudai", ct.ChieuDai),
                        new SqlParameter("@chieurong", ct.ChieuRong),
                        new SqlParameter("@chieucao", ct.ChieuCao),
                        new SqlParameter("@soluong", ct.SoLuong),
                        new SqlParameter("@giaban", ct.Giaban),
                        new SqlParameter("@tongtien", ct.TongTien),
                        new SqlParameter("@mabookingdichvu", ct.MaBookingDichVu),
                        new SqlParameter("@madonhangdichvu", ct.MaDonHangDichVu),
                        new SqlParameter("@mavedichvu", ct.MaVeDichVu),
                        new SqlParameter("@hansudung", ct.HanSuDung),
                        new SqlParameter("@mavekhac", ct.MaVeBoSung),
                        new SqlParameter("@iddichvu", ct.ID_DichVu)
                    };

                    //Chuyển v2
                    helper.ExecuteNonQuery("sp_App_TaoChiTietMatHangDonHang_v2", param).ToString();
                }

                return 1;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return 0;
            }
        }
        public int SuaDSChiTietMatHangDonHang(int idchitietdh, int iddonhang, int idhanghoa, List<ChiTietMatHangDonHangModels> ds)
        {
            try
            {
                SqlParameter[] param1 = new SqlParameter[]
                {
                    new SqlParameter("@idchitietdh", idchitietdh)
                };

                //Xóa hết chi tiết mặt hàng đơn hàng
                helper.ExecuteNonQuery("sp_App_XoaChiTietMatHangDonHang_v2", param1);

                foreach (ChiTietMatHangDonHangModels ct in ds)
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                    new SqlParameter("@mota", ct.MoTa),
                    new SqlParameter("@idchitietdh", idchitietdh),
                    new SqlParameter("@iddonhang", iddonhang),
                    new SqlParameter("@idmathang", ct.ID_MatHang),
                    new SqlParameter("@chieudai", ct.ChieuDai),
                    new SqlParameter("@chieurong", ct.ChieuRong),
                    new SqlParameter("@chieucao", ct.ChieuCao),
                    new SqlParameter("@soluong", ct.SoLuong),
                    new SqlParameter("@giaban", ct.Giaban),
                    new SqlParameter("@tongtien", ct.TongTien)
                    };

                    helper.ExecuteNonQuery("sp_App_TaoChiTietMatHangDonHang_v2", param).ToString();
                }

                return 1;

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return 0;
            }
        }
        public bool CapNhatSTT_DonHang(string ID_Nhom, int STT_DonHang)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();
                SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("ID_Nhom",  ID_Nhom),
               new SqlParameter("STT_DonHang", STT_DonHang)

            };

                if (helper.ExecuteNonQuery("sp_Nhom_Update_STT_DonHang", pars) != 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateMaVeKhac(long ID_ChiTiet_MatHang_DonHang, string MaVeKhac)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();
                SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("@ID_ChiTiet_MatHang_DonHang",  ID_ChiTiet_MatHang_DonHang),
               new SqlParameter("@MaVeKhac", MaVeKhac)

            };

                if (helper.ExecuteNonQuery("sp_ChiTietMatHang_DonHang_UpdateMaVeKhac", pars) != 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdateGroupLink(string ID_ChiTiet_MatHang_DonHang, string GroupLink)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();
                SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("@ID_ChiTiet_MatHang_DonHang",  ID_ChiTiet_MatHang_DonHang),
               new SqlParameter("@GroupLink", GroupLink)

            };

                if (helper.ExecuteNonQuery("sp_ChiTietMatHang_DonHang_UpdateGroupLink", pars) != 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTrangThaiDonHang(long ID_DonHang, int TrangThai)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();
                SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("@ID_DonHang",  ID_DonHang),
               new SqlParameter("@TrangThai", TrangThai)

            };

                if (helper.ExecuteNonQuery("sp_DonHang_UpdateTrangThai", pars) != 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTrangThai(long ID_ChiTiet_MatHang_DonHang, int TrangThai, int ID_NhanVien)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();
                SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("@ID_ChiTietMatHang_DonHang",  ID_ChiTiet_MatHang_DonHang),
               new SqlParameter("@TrangThai", TrangThai),
               new SqlParameter("@ID_NhanVien", ID_NhanVien)

            };

                if (helper.ExecuteNonQuery("sp_updateTrangThai_TheDo", pars) != 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public DataTable GetNhanVienCapQuyen(int ID_DonHang)
        {
            DataTable dt = new DataTable();

            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_DonHang", ID_DonHang)
            };

            DataSet ds = helper.ExecuteDataSet("sp_vuongtm_DonHang_DanhSachNhanVienPhanCong", pars);
            dt = ds.Tables[0];
            return dt;

        }
        public bool XoaChiTietDonHang(ChiTietDonHangModels cdh)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idchitietdonhang", cdh.idchitietdonhang),
                new SqlParameter("@iddonhang", cdh.iddonhang),
                new SqlParameter("@idhanghoa", cdh.idhanghoa),
                new SqlParameter("@soluong", cdh.soluong),
                new SqlParameter("@ghichu", cdh.ghichu),
                new SqlParameter("@hinhthucban", cdh.hinhthucban),
                new SqlParameter("@giakhac", cdh.giakhac),
                new SqlParameter("@giaban", cdh.giaban),
                new SqlParameter("@idnhanvien", cdh.idnhanvien),
                new SqlParameter("@idquanly", cdh.idquanly),
                new SqlParameter("@ngaysuadoi", cdh.ngaysuadoi)
            };

                return helper.ExecuteNonQuery("sp_App_XoaChiTietDonHang", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
        public bool XoaChiTietDonHangTang(int ID_DonHang)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_DonHang", ID_DonHang),
            };

                return helper.ExecuteNonQuery("sp_App_XoaChiTietDonHangTang", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
        public int SuaChiTietDonHang(ChiTietDonHangModels cdh)
        {
            int ID = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                    new SqlParameter("@idchitietdonhang", cdh.idchitietdonhang),
                    new SqlParameter("@iddonhang", cdh.iddonhang),
                    new SqlParameter("@idhanghoa", cdh.idhanghoa),
                    new SqlParameter("@soluong", cdh.soluong),
                    new SqlParameter("@ghichu", cdh.ghichu),
                    new SqlParameter("@hinhthucban", cdh.hinhthucban),
                    new SqlParameter("@giakhac", cdh.giakhac),
                    new SqlParameter("@giaban", cdh.giaban),
                    new SqlParameter("@idnhanvien", cdh.idnhanvien),
                    new SqlParameter("@idquanly", cdh.idquanly),
                    new SqlParameter("@ngaysuadoi", cdh.ngaysuadoi),
                    new SqlParameter("@idctkm", cdh.idctkm),
                    new SqlParameter("@tongtienchietkhau", cdh.tongtienchietkhau),
                    new SqlParameter("@chietkhautien", cdh.chietkhautien),
                    new SqlParameter("@chietkhauphantram", cdh.chietkhauphantram),
                    new SqlParameter("@phantramhaohut", cdh.phantramhaohut),
                    new SqlParameter("@soluonghaohut", cdh.soluonghaohut),
                    new SqlParameter("@idhaohut", cdh.idhaohut),
                };
                object obj = helper.ExecuteScalar("sp_App_SuaChiTietDonHang", par);
                if (obj != null)
                {
                    ID = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);

            }
            return ID;
        }

        public DataTable GetListDonHang(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa, string ListIDNhom, int taidiem, int trangthaixem)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),
                    new SqlParameter("@trangthaigiao", trangthaigiao),
                    new SqlParameter("@trangthaithanhtoan", trangthaithanhtoan),
                    new SqlParameter("@trangthaihoanthanh", trangthaihoanthanh),
                    new SqlParameter("@ID_KhachHang", ID_KhachHang),
                    new SqlParameter("@ID_NhanVien", ID_NhanVien),
                    new SqlParameter("@ID_HangHoa", ID_HangHoa),
                    new SqlParameter("@ListIDNhom", ListIDNhom),
                    new SqlParameter("@taidiem", taidiem),
                    new SqlParameter("@trangthaixem", trangthaixem)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllDonHang_v3", Parammeter);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        
        public DataTable GetListDonHangByHDV(int ID_QuanLy, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),                  
                    };

                DataSet ds = helper.ExecuteDataSet("sp_DonHang_GetListDonHangByHDV", Parammeter);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public DataSet DanhSachDonHangExport(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa, string ListIDNhom, int taidiem, int trangthaixem, string listid)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to),
                    new SqlParameter("@trangthaigiao", trangthaigiao),
                    new SqlParameter("@trangthaithanhtoan", trangthaithanhtoan),
                    new SqlParameter("@trangthaihoanthanh", trangthaihoanthanh),
                    new SqlParameter("@ID_KhachHang", ID_KhachHang),
                    new SqlParameter("@ID_NhanVien", ID_NhanVien),
                    new SqlParameter("@ID_HangHoa", ID_HangHoa),
                    new SqlParameter("@ListIDNhom", ListIDNhom),
                    new SqlParameter("@taidiem", taidiem),
                    new SqlParameter("@trangthaixem", trangthaixem),
                    new SqlParameter("@listid", listid)
                    };

                ds = helper.ExecuteDataSet("sp_QL_GetAllDonHang_v2_exportexcel", Parammeter);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }
        public DataSet DanhSachChiTietDonHang(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa, string ListIDNhom, int taidiem, int trangthaixem, string listid)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Parammeter = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@from", from),
            new SqlParameter("@to", to),
            new SqlParameter("@trangthaigiao", trangthaigiao),
            new SqlParameter("@trangthaithanhtoan", trangthaithanhtoan),
            new SqlParameter("@trangthaihoanthanh", trangthaihoanthanh),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@ID_HangHoa", ID_HangHoa),
            new SqlParameter("@ListIDNhom", ListIDNhom),
            new SqlParameter("@taidiem", taidiem),
            new SqlParameter("@trangthaixem", trangthaixem),
            new SqlParameter("@listid", listid)
        };
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_danhsachchitietdonhang_Kendo_New", Parammeter);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow drr in ds.Tables[0].Rows)
                    {
                        KhuyenMai_dl km = new KhuyenMai_dl();
                        double ckpt = 0;
                        double ckt = 0;
                        DataTable dt = GetChuongTrinhKhuyenMai_ChiTietDonHang(int.Parse(drr["ID_ChiTietDonHang"].ToString()));
                        if (dt != null)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                KhuyenMai kmai = km.GetKhuyenMaiByID(int.Parse(dr["ID_CTKM"].ToString()));
                                foreach (ChiTietKhuyenMai ctkm in kmai.ChiTietCTKM)
                                {
                                    if (ctkm.ID_Hang == int.Parse(drr["ID_HangHoa"].ToString()))
                                    {
                                        switch (int.Parse(drr["HinhThucBan"].ToString()))
                                        {
                                            case 0:
                                                // Bán lẻ
                                                ckpt += kmai.ChietKhauPhanTram;
                                                ckt += kmai.ChietKhauTien;
                                                break;
                                            case 1:
                                                ckt += kmai.ChietKhauTien;
                                                ckpt += kmai.ChietKhauPhanTram;
                                                break;
                                            // Bán buôn
                                            case 2:
                                                // Khác
                                                ckt += kmai.ChietKhauTien;
                                                ckpt += kmai.ChietKhauPhanTram;
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                }
                            }
                        }
                        drr["ChietKhauPhanTram"] = ckpt;
                        drr["ChietKhauTien"] = ckt;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet _GettemplatematHang(int id_QLLH)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", id_QLLH)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_QL_Getdata_Template_Kendo", pars);

                return ds;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public DataSet _GettemplateGiamatHang(int IdDanhMuc, int ID_QLLH)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                     new SqlParameter("@ID_DANHMUC", IdDanhMuc),
                     new SqlParameter("@ID_QLLH", ID_QLLH)
                    };

                DataSet ds = helper.ExecuteDataSet("getDS_HangHoa_ByIdDanhMuc", pars);

                return ds;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public List<MatHang> getDS_HangHoa_ByIdKhachHang(int idKhachHang, int ID_QLLH)
        {
            List<MatHang> rs = new List<MatHang>();

            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@idKhachHang", idKhachHang),
                new SqlParameter("@ID_QLLH", ID_QLLH)
                };

                DataTable dt = helper.ExecuteDataSet("usp_vuongtm_GetListHangHoaTheoBangGiaLoaiKhachHang", pars).Tables[0];
                int a = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs.Add(GetMatHangFromDataRow(dr));
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }

        public List<ChiTietMatHangDonHangModels> GetDSChiTietMatHangDonHang(int idchitietdh, int iddonhang, int idmathang)
        {
            List<ChiTietMatHangDonHangModels> rs = new List<ChiTietMatHangDonHangModels>();

            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@idchitietdh", idchitietdh),
                new SqlParameter("@iddonhang", iddonhang),
                new SqlParameter("@idmathang", idmathang)
                };

                DataTable dt = helper.ExecuteDataSet("sp_App_GetDSChiTietMatHangDonHang_v2", pars).Tables[0];
                int a = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs.Add(GetObjectFromDataRowUtil<ChiTietMatHangDonHangModels>.ToOject(dr));
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }

        public List<ChiTietMatHangDonHangModels> GetDSChiTietMatHangDonHang_DichVu(int idchitietdh, int iddonhang, int idmathang, int iddichvu, string bookingcode, string sitecode, string grouplink = "")
        {
            List<ChiTietMatHangDonHangModels> rs = new List<ChiTietMatHangDonHangModels>();

            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@idchitietdh", idchitietdh),
                new SqlParameter("@iddonhang", iddonhang),
                new SqlParameter("@idmathang", idmathang),
                new SqlParameter("@iddichvu", iddichvu),
                new SqlParameter("@bookingcode", bookingcode),
                new SqlParameter("@sitecode", sitecode),
                new SqlParameter("@grouplink", grouplink)
                };

                DataTable dt = helper.ExecuteDataSet("sp_App_GetDSChiTietMatHangDonHang_v3", pars).Tables[0];
                int a = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs.Add(GetObjectFromDataRowUtil<ChiTietMatHangDonHangModels>.ToOject(dr));
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }

        public List<ChiTietMatHangDonHangModels> GetDSChiTietMatHangDonHang_DichVuV2(int? idmathang, string bookingcode, string sitecode, string grouplink = "")
        {
            List<ChiTietMatHangDonHangModels> rs = new List<ChiTietMatHangDonHangModels>();

            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@idmathang", idmathang),
                new SqlParameter("@bookingcode", bookingcode),
                new SqlParameter("@sitecode", sitecode),
                new SqlParameter("@grouplink", grouplink)
                };

                DataTable dt = helper.ExecuteDataSet("sp_App_GetDSChiTietMatHangDonHang_v4", pars).Tables[0];
                int a = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs.Add(GetObjectFromDataRowUtil<ChiTietMatHangDonHangModels>.ToOject(dr));
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }

        public List<HangHoa_DichVuModel> GetDSHangHoaDichVuDonHang(int idchitietdh)
        {
            List<HangHoa_DichVuModel> rs = new List<HangHoa_DichVuModel>();

            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_ChiTietDonHang", idchitietdh)
                };

                DataTable dt = helper.ExecuteDataSet("sp_ChiTietMatHang_DichVu_GetByChiTietDonHang", pars).Tables[0];
                int a = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs.Add(GetObjectFromDataRowUtil<HangHoa_DichVuModel>.ToOject(dr));
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }

        public List<MatHang> getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoNhanVien(int idKhachHang, int ID_QLLH, int ID_NhanVien, int ID_DANHMUC)
        {
            List<MatHang> rs = new List<MatHang>();
            HangHoa_DichVuDAO hhdvdao = new HangHoa_DichVuDAO();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idKhachHang", idKhachHang),
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_NhanVien", ID_NhanVien),
                    new SqlParameter("@ID_DANHMUC", ID_DANHMUC)
                    };

                DataTable dt = helper.ExecuteDataSet("usp_vuongtm_GetListHangHoaTheoBangGiaLoaiKhachHang_v2", pars).Tables[0];
                int a = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        MatHang item = GetMatHangFromDataRow(dr);
                        item.lstDichVu = hhdvdao.GetAllByHangHoa(item.IDMatHang);
                        rs.Add(item);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }
        public List<MatHang> getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoQuanLy(int idKhachHang, int ID_QLLH, int ID_NhanVien, int ID_DANHMUC)
        {
            List<MatHang> rs = new List<MatHang>();
            HangHoa_DichVuDAO hhdvdao = new HangHoa_DichVuDAO();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@idKhachHang", idKhachHang),
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_NhanVien", ID_NhanVien),
                    new SqlParameter("@ID_DANHMUC", ID_DANHMUC)
                    };

                DataTable dt = helper.ExecuteDataSet("usp_vuongtm_GetListHangHoaTheoBangGiaLoaiKhachHang_v3", pars).Tables[0];
                int a = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        MatHang item = GetMatHangFromDataRow(dr);
                        item.lstDichVu = hhdvdao.GetAllByHangHoa(item.IDMatHang);
                        rs.Add(item);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return rs;
            }
        }
        public MatHang GetMatHangFromDataRow(DataRow dr)
        {
            try
            {
                MatHang mh = new MatHang();
                mh.IDMatHang = int.Parse(dr["ID_Hang"].ToString());
                mh.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
                mh.MaHang = dr["MaHang"].ToString();
                mh.TenHang = dr["TenHang"].ToString();
                mh.IDDonVi = int.Parse(dr["ID_DonVi"].ToString());
                mh.TenDonVi = dr["TenDonVi"].ToString();
                mh.KhuyenMai = dr["KhuyenMai"].ToString();
                mh.SoLuong = double.Parse(dr["SoLuong"].ToString());
                //mh.GiaBuon = Convert.ToUInt32(double.Parse(dr["GiaBanBuon"].ToString()));
                //mh.GiaLe = Convert.ToUInt32(double.Parse(dr["GiaBanLe"].ToString()));
                mh.GiaBuon = (dr["GiaBanBuon"].ToString() == "") ? mh.GiaBuon = -1 : double.Parse(dr["GiaBanBuon"].ToString());
                mh.GiaLe = (dr["GiaBanLe"].ToString() == "") ? mh.GiaLe = -1 : double.Parse(dr["GiaBanLe"].ToString());
                mh.LoiNhuan = mh.GiaLe - mh.GiaBuon;
                mh.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
                mh.IsDichVu = (dr["IsDichVu"].ToString() != "") ? int.Parse(dr["IsDichVu"].ToString()) : 0;
                mh.TenDanhMuc = dr["TenDanhMuc"].ToString();
                mh.GhiChuGia = dr["GhiChuGia"].ToString();

                try
                {
                    mh.AnhDaiDien = dr["AnhDaiDien"].ToString();
                    mh.ID_NhanHieu = (dr["ID_NhanHieu"].ToString() != "") ? int.Parse(dr["ID_NhanHieu"].ToString()) : 0;
                    mh.ID_NhaCungCap = (dr["ID_NhaCungCap"].ToString() != "") ? int.Parse(dr["ID_NhaCungCap"].ToString()) : 0;
                    mh.LinkGioiThieu = dr["LinkGioiThieu"].ToString();
                    mh.MoTa = dr["MoTa"].ToString();
                    //mh.MoTaNgan = dr["MoTaNgan"].ToString();
                    mh.SoLuongTon = dr.Table.Columns.Contains("SoLuongTon") ? ((dr["SoLuongTon"].ToString() != "") ? double.Parse(dr["SoLuongTon"].ToString()) : 0) : ((dr["SoLuong"].ToString() != "") ? double.Parse(dr["SoLuong"].ToString()) : 0);

                    mh.DanhSachAnh = HangHoaAlbumDB.LayDanhSachAnh_TheoMatHang(mh.IDQLLH, mh.IDMatHang);
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }
                return mh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public int ApGiaMoi(ApGia gia)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QuanLy", gia.ID_QuanLy),
                new SqlParameter("ID_Hang", gia.ID_Hang),
                new SqlParameter("GiaBanBuon", Convert.ToDouble(gia.GiaBanBuon)),
                new SqlParameter("GiaBanLe", Convert.ToDouble(gia.GiaBanLe)),
                new SqlParameter("TuNgay", gia.TuNgay)
                };
                return helper.ExecuteNonQuery("sp_QL_ApGia", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return 0;
            }
        }
        public List<DonHang> GetDSDonHangChuaDoc(int ID_QLLH, int ID_QuanLy)
        {
            List<DonHang> dsdh = new List<DonHang>();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSDonHangChuaDoc", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;

            try
            {

                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    DonHang dh = new DonHang_dl().GetDonHangFromDataRow(dr);
                    dh.ThoiGian = dh.NgayTao.ToString("dd/MM/yyyy HH:mm:ss");
                    dh.SoTien = String.Format("{0:c0}", dh.TongTien).Replace('$', ' ');
                    dsdh.Add(dh);
                }


            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return dsdh;
        }
        public DataTable GetChuongTrinhKhuyenMai_ChiTietDonHang(int ID_ChiTietDonHang)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_ChiTietDonHang", ID_ChiTietDonHang)
        };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_GetChuongTrinhKhuyenMai_ChiTietDonHang", pars);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                    return null;
                else
                    return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return null;
        }
        public DonHangv2 GetDonHangTheoID_v2(int ID_DonHang, int ID_QuanLy)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_DonHang", ID_DonHang),
                new SqlParameter("ID_QuanLy", ID_QuanLy)
                };
            try
            {
                DataSet ds = helper.ExecuteDataSet("selectDonHangTheoID_v1", pars);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                    return null;


                DataRow dr = dt.Rows[0];
                DonHangv2 dh = GetDonHangFromDataRow_v2(dr);
                return dh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public DonHangv2 GetDonHangFromDataRow_v2(DataRow dr)
        {
            try
            {
                DonHangv2 dh = new DonHangv2();

                dh.ID_DonHang = int.Parse(dr["ID_DonHang"].ToString());
                dh.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
                dh.ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
                dh.ID_KhachHang = int.Parse(dr["ID_KhachHang"].ToString());
                dh.TenNhanVien = dr["TenNhanVien"].ToString();
                dh.TenKhachHang = dr["TenKhachHang"].ToString();
                dh.DienThoai = dr["DienThoai"].ToString();
                dh.DiaChi = dr["DiaChi"].ToString();
                dh.Email = dr["Email"].ToString();
                dh.EmailHDV = dr["EmailHDV"].ToString();
                dh.TongTien = dr["TongTien"].ToString() != "" ? double.Parse(dr["TongTien"].ToString()) : 0;
                dh.XuatHoaDon = dr["XuatHoaDon"].ToString() != "" ? bool.Parse(dr["XuatHoaDon"].ToString()) : false;
                dh.InVeTaiQuay = dr["InVeTaiQuay"].ToString() != "" ? bool.Parse(dr["InVeTaiQuay"].ToString()) : false;
                dh.NgayTao = Convert.ToDateTime(dr["CreateDate"].ToString());
                dh.isProcess = int.Parse(dr["IsProcess"].ToString());
                if (dh.isProcess == 0)
                {
                    dh.isProcess_Name = "Chưa hoàn tất";
                }
                else if (dh.isProcess == 1)
                {
                    dh.isProcess_Name = "Đã hoàn tất";
                }
                else if (dh.isProcess == 2)
                {
                    dh.isProcess_Name = "Hủy";
                }
                dh.HinhThucBan = int.Parse(dr["HinhThucBan"].ToString());
                dh.GhiChu = dr["GhiChu"].ToString();
                dh.DaXem = Convert.ToInt32(dr["DaXem"]);
                dh.ID_TrangThaiGiaoHang = dr["ID_TrangThaiGiaoHang"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiGiaoHang"]) : 0;

                dh.ID_TrangThaiThanhToan = dr["ID_TrangThaiThanhToan"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiThanhToan"]) : 0;
                dh.MaThamChieu = dr["MaThamChieu"].ToString();
                try
                {
                    dh.TienDaThanhToan = dr["TienDaThanhToan"].ToString() != "" ? double.Parse(dr["TienDaThanhToan"].ToString()) : 0;
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    dh.TienDaThanhToan = 0;

                }
                dh.TenTrangThaiGiaoHang = dr["TenTrangThaiGiaoHang"].ToString();
                try
                {
                    dh.ID_CTKM = dr["ID_CTKM"].ToString() != "" ? Convert.ToInt32(dr["ID_CTKM"]) : 0;
                    dh.TenCTKM = dr["TenCTKM"].ToString();
                    dh.ChietKhauPhanTram = dr["ChietKhauPhanTram"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                    dh.ChietKhauTien = dr["ChietKhauTien"].ToString() != "" ? Convert.ToDouble(dr["ChietKhauTien"]) : 0;
                    dh.TongTienChietKhau = dr["TongTienChietKhau"].ToString() != "" ? Convert.ToDouble(dr["TongTienChietKhau"]) : 0;

                    dh.ChietKhauPhanTramTheoCTKM = dr["ChietKhauPhanTram_TheoCTKM"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram_TheoCTKM"].ToString()) : 0;
                    dh.ChietKhauTienTheoCTKM = dr["ChietKhauTien_TheoCTKM"].ToString() != "" ? double.Parse(dr["ChietKhauTien_TheoCTKM"].ToString()) : 0;
                    dh.ChietKhauPhanTramKhac = dr["ChietKhauPhanTram_Khac"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram_Khac"].ToString()) : 0;
                    dh.ChietKhauTienKhac = dr["ChietKhauTien_Khac"].ToString() != "" ? double.Parse(dr["ChietKhauTien_Khac"].ToString()) : 0;
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }
                dh.ProcessDate = dr["ProcessDate"].ToString() != "" ? Convert.ToDateTime(dr["ProcessDate"].ToString()) : new DateTime(1900, 1, 1);
                dh.ID_TrangThaiDongHang = dr["ID_TrangThaiDonHang"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiDonHang"]) : 0;
                dh.TenTrangThaiDongHang = dr["TenTrangThai"].ToString();
                try
                {
                    dh.LyDo = dr["LyDo"].ToString();
                }
                catch (Exception)
                {
                    dh.LyDo = "";
                }
                try
                {
                    dh.MaKH = dr["MaKH"].ToString();
                    dh.NguoiThaoTac = dr["NguoiThaoTac"].ToString();

                }
                catch (Exception)
                {
                    dh.NguoiThaoTac = "";
                }


                try
                {
                    dh.ToaDoKhachHang = dr["ToaDoKhachHang"].ToString();

                }
                catch (Exception)
                {

                }

                try
                {
                    dh.DiaChiTao = dr["DiaChiTao"].ToString();
                    dh.KinhDo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0;
                    dh.ViDo = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0;
                    dh.DiaChiXuatHoaDon = dr["DiaChiXuatHoaDon"].ToString();
                    dh.SoLanInHoaDon = dr["SoLanInHoaDon"].ToString() != "" ? int.Parse(dr["SoLanInHoaDon"].ToString()) : 0;
                }
                catch (Exception)
                {

                }

                try
                {
                    dh.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                    dh.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                    dh.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : dh.LastUpdate_ThoiGian_NhanVien;
                    dh.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : dh.LastUpdate_ThoiGian_QuanLy;
                    dh.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                    dh.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();

                }
                catch (Exception)
                {

                }

                //dh.danhsachanh = ImageDB.DanhSachAlbum_TheoDonHang(dh.ID_DonHang);
                dh.dskhuyenmai = GetDanhSachCTKM_DonHang(dh.ID_DonHang);
                //dh.DaXem = Convert.ToInt32(dr["ID_TrangThaiGiaoHang"]);

                //dh.DaXem = Convert.ToInt32(dr["DaXem"]);
                return dh;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public static List<CTKMOBJ> GetDanhSachCTKM_DonHang(int ID_DonHang)
        {
            List<CTKMOBJ> lstrs = new List<CTKMOBJ>();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                    new SqlParameter("ID_DonHang", ID_DonHang)
            };
                SqlDataHelper helper = new SqlDataHelper();
                DataTable dt = helper.ExecuteDataSet("sp_CTKM_DonHang_GetByID_DonHang", par).Tables[0];
                DateTime d;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        CTKMOBJ rs = new CTKMOBJ();
                        rs.idctkm = int.Parse(dr["ID_CTKM"].ToString());
                        rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                        rs.idnhanvien = (dr["ID_NhanVien"].ToString() != "") ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
                        rs.idquanly = (dr["ID_QuanLy"].ToString() != "") ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
                        rs.loai = (dr["Loai"].ToString() != "") ? int.Parse(dr["Loai"].ToString()) : 0;
                        rs.chietkhauphantram = (dr["ChietKhauPhanTram"].ToString() != "") ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                        rs.chietkhautien = (dr["ChietKhauTien"].ToString() != "") ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;
                        try
                        {
                            if (rs.ngayketthuc.Year != 1900 && (new DateTime(rs.ngayketthuc.Year, rs.ngayketthuc.Month, rs.ngayketthuc.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)))
                            {
                                rs.hethan = 1;
                            }
                            else
                            {
                                rs.hethan = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            rs.hethan = 0;
                        }
                        rs.ngayapdung = (dr["NgayApDung"].ToString() != "") ? Convert.ToDateTime(dr["NgayApDung"].ToString()) : rs.ngayapdung;
                        rs.ngayketthuc = (dr["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(dr["NgayKetThuc"].ToString()) : rs.ngayketthuc;
                        rs.ngaytao = (dr["NgayTao"].ToString() != "") ? Convert.ToDateTime(dr["NgayTao"].ToString()) : rs.ngaytao; Convert.ToDateTime(dr["NgayTao"].ToString());
                        rs.ghichu = dr["GhiChu"].ToString();
                        rs.tenctkm = dr["TenCTKM"].ToString();
                        rs.trangthai = (dr["trangthai"].ToString() != "") ? int.Parse(dr["trangthai"].ToString()) : 0;



                        lstrs.Add(rs);


                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstrs;
        }

        public int ThemMoiGiaoHang(GiaoHangOBJ ghobj)
        {
            int ID_GiaoHang = 0;

            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_DonHang", ghobj.iddonhang),
                new SqlParameter("@ID_NhanVien", ghobj.idnhanvien),
                new SqlParameter("@ID_QuanLy", ghobj.idquanly),
                new SqlParameter("@GhiChu", ghobj.ghichu)
            };
                object obj = helper.ExecuteScalar("sp_GiaoHang_Insert", par);
                ID_GiaoHang = obj != null ? int.Parse(obj.ToString()) : 0;

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ID_GiaoHang;
        }

        public bool GiaoHangDonHang(int loai, int iddonhang, int idnhanvien, int idmathang, string ghichu, double soluong, int hinhthucban, int idquanly, int ID_GiaoHang)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@loai", loai),
                new SqlParameter("@iddonhang", iddonhang),
                new SqlParameter("@soluonggiao", soluong),
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@idmathang", idmathang),
                new SqlParameter("@ghichu", ghichu),
                new SqlParameter("@idquanly", idquanly),
                new SqlParameter("@idgiaohang", ID_GiaoHang),
                new SqlParameter("@hinhthucban", hinhthucban)
            };

                return helper.ExecuteNonQuery("sp_App_GiaoHangDonHang", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public bool ThanhToanDonHang(int iddonhang, double tien, int idnhanvien, long idquanly, string ghichu, string url, int httt = 0)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@loai", 1),
                new SqlParameter("@iddonhang", iddonhang),
                new SqlParameter("@sotien", tien),
                new SqlParameter("@ghichu", ghichu),
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@idquanly", idquanly),
                new SqlParameter("@imageurl", url),
                new SqlParameter("@httt", httt)
            };

                return helper.ExecuteNonQuery("sp_App_ThanhToanDonHang", par) > 0;

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public bool ThanhToanDonHang1phan(int iddonhang, double tien, int idnhanvien, string ghichu, long idquanly, string url, int httt = 0)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@loai", 2),
                new SqlParameter("@iddonhang", iddonhang),
                new SqlParameter("@sotien", tien),
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@ghichu", ghichu),
                new SqlParameter("@idquanly", idquanly),
                    new SqlParameter("@imageurl", url),
                new SqlParameter("@httt", httt)

            };

                return helper.ExecuteNonQuery("sp_App_ThanhToanDonHang", par) > 0;

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public bool CapNhatThanhToan1Ngay(int iddonhang, int idnhanvien, double tien, int langiao)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_DonHang", iddonhang),
                new SqlParameter("@SoTien", tien),
                new SqlParameter("@ID_NhanVien", idnhanvien),
                new SqlParameter("@LanGiao", langiao)
            };

                return helper.ExecuteNonQuery("SP_App_CapNhatThanhToan1Ngay", par) > 0;

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public bool BienDongSoDuNhomTaiKhoan(int ID_Nhom, DateTime NgayTao, int LoaiBienDong, int ID_DonHang, float SoTien, int TrangThai, string GhiChu)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@ID_Nhom", ID_Nhom),
                    new SqlParameter("@NgayTao", NgayTao),
                    new SqlParameter("@LoaiBienDong", LoaiBienDong),
                    new SqlParameter("@ID_DonHang", ID_DonHang),
                    new SqlParameter("@SoTien", SoTien),
                    new SqlParameter("@TrangThai", TrangThai),
                    new SqlParameter("@GhiChu", GhiChu),
                };
                return helper.ExecuteNonQuery("sp_BienDongSoDu", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public float BienDongSoDuNhom_TongTienHienTai(int ID_Nhom)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]
                {
                    new SqlParameter("@ID_Nhom", ID_Nhom),
                };
                return float.Parse(helper.ExecuteScalar("sp_BienDongSoDu_TongSoDuNhom", par).ToString());

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return 0;
        }

        public bool BienDongSoDuNhom_UpdateTrangThaiThanhToan(int ID_TaiKhoan, int ID_Donhang)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]
                {
                    new SqlParameter("@ID_TaiKhoan", ID_TaiKhoan),
                    new SqlParameter("@ID_DonHang", ID_Donhang),
                };
                return helper.ExecuteNonQuery("sp_BienDongSoDu_UpdateTrangThaiThanhToan", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public bool BienDongSoDuNhom_HuyDonHang(int ID_TaiKhoan, int ID_DonHang)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]
                {
                    new SqlParameter("@ID_TaiKhoan", ID_TaiKhoan),
                    new SqlParameter("@ID_DonHang", ID_DonHang),
                };
                return helper.ExecuteNonQuery("sp_BienDongSoDu_HuyDonHang", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return false;
        }

        public float BienDongSoDuNhom_SoDuDauKy(int ID_Nhom, DateTime to)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]
                {
                     new SqlParameter("@ID_Nhom", ID_Nhom),
                    new SqlParameter("@Date_To", to)                    
                };
                return float.Parse(helper.ExecuteScalar("sp_BienDongSoDu_SoDuDauKy", par).ToString());

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return 0;
        }

        public DataTable BienDongSoDuNhom_GetAll(int ID_Nhom, DateTime from, DateTime to)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[]
                {
                    new SqlParameter("@ID_Nhom", ID_Nhom),
                    new SqlParameter("@Date_From", from),
                    new SqlParameter("@Date_To", to),
                };
                DataSet data = helper.ExecuteDataSet("sp_BienDongSoDu_ListAll", par);

                if (data.Tables[0].Rows.Count == 0)
                    return null;
                else
                    return data.Tables[0]; ;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return null;
        }
    }
}