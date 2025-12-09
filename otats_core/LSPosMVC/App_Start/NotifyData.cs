using LSPosMVC;
using LSPosMVC.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class NotifyData
    {
        readonly string _connString = new SqlDataHelper().GetConnectionString();

        public void GetAllHoatDong(int ID_QLLH)
        {

            DependencyHoatDong.instance.CreateCommand(new SqlCommand(@"SELECT Checkin.[IDCheckIn], Checkin.[DiaChiRaDiem], Checkin.[DiaChiVaoDiem] FROM [dbo].[Checkin] JOIN [dbo].[NhanVien] ON [Checkin].MaNhanVien = NhanVien.ID_NhanVien where NhanVien.ID_QLLH = " + ID_QLLH, DependencyHoatDong.instance.conn));

        }
        public void GetAllDangNhap(int ID_QLLH)
        {

            DependencyDangNhap.instance.CreateCommand(new SqlCommand(@"SELECT LichSuDangNhap.[idnhanvien], LichSuDangNhap.[hinhthucdangxuat],LichSuDangNhap.[id] FROM [dbo].[LichSuDangNhap] JOIN [dbo].[NhanVien] ON LichSuDangNhap.idnhanvien = NhanVien.ID_NhanVien where NhanVien.ID_QLLH = " + ID_QLLH, DependencyDangNhap.instance.conn));

        }
        public void GetAllChupAnh(int ID_QLLH)
        {

            DependencyChupAnh.instance.CreateCommand(new SqlCommand(@"SELECT AlbumImages.[ID_Album] FROM [dbo].[AlbumImages] where AlbumImages.[ID_QLLH] = " + ID_QLLH, DependencyHoatDong.instance.conn));
        }
        public List<DonHang> GetAllDonHang(int ID_QLLH, int ID_QuanLy)
        {
            DependencyDonhang.instance.CreateCommand(new SqlCommand(@"SELECT DonHang.[ID_DonHang],DonHang.[TongTien], DonHang.[CreateDate], DonHang.[ID_NhanVien], DonHang.[DaXem], KhachHang.[TenKhachHang] from [dbo].[DonHang] join [dbo].[KhachHang] on [DonHang].ID_KhachHang = [KhachHang].ID_KhachHang where DonHang.ID_QLLH = " + ID_QLLH, DependencyDonhang.instance.conn));
            DonHangData dh = new DonHangData();
            return dh.GetDSDonHangChuaDoc(ID_QLLH, ID_QuanLy);
        }
        public void GetAllTinNhan(int ID_QLLH)
        {
            DependencyTinNhan.instance.CreateCommand(new SqlCommand(@"SELECT TINNHAN.[ID_TINNHAN] from [dbo].[TINNHAN] where TINNHAN.ID_QLLH = " + ID_QLLH, DependencyTinNhan.instance.conn));
        }


    }
}