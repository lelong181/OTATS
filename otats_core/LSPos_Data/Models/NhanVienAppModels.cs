using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class NhanVienAppModels
    {
        public NhanVienAppModels()
        {
            //
            // TODO: Add constructor logic here
            //
            sokytusaudauphaydonvitien = 2;
        }

        public int idct { get; set; }

        public int idnhanvien { get; set; }
        public int ID_QuanLy { get; set; }

        public string tendangnhap { get; set; }

        public string matkhau { get; set; }

        public string matkhaucu { get; set; }

        public string tennhanvien { get; set; }

        public int thoigiancapnhatbantin { get; set; }

        public DateTime? thoigiandangnhapcuoicung { get; set; }

        public int dangtructuyen { get; set; }

        public int goiungdung { get; set; }

        public string device { get; set; }

        public string urlserver { get; set; }

        public int os { get; set; } // 1: ios, 2: android
        public string ver { get; set; }

        public string idpush { get; set; }
        public int hinhthucdangxuat { get; set; } // 0: Dang xuat chu dong, 1: Dang xuat cuong che
        public int TGCanhBaoMatKetNoi { get; set; }//phut
        public int TGCanhBaoLaiMatKetNoi { get; set; }//phut
        public int sotinnhanchuadoc { get; set; }
        public int TypeApp { get; set; } // 1 : Release, 0, NULL : DEV
        public double kinhdo { get; set; }

        public double vido { get; set; }
        public double accuracy { get; set; }
        public int idnhom { get; set; }
        public int chophepvaodiemkhongkehoach { get; set; }
        public int bankinhchophep { get; set; }
        public int TGLayViTri { get; set; }
        public int ThongBaoPhienBanMoi { get; set; }
        public string newversion { get; set; }
        public string msg_newversion { get; set; }
        public int trangthaigps { get; set; }
        public int sokytusaudauphaydonvitien { get; set; }
        public string Min_Version { get; set; }
        public string Max_Version { get; set; }

        public string icon_path { get; set; }

        public int MaDonHang_TuSinh { get; set; }
        public int MaDonHang_TuSinh_TheoKy { get; set; }
        public string MaDonHang_CauTruc { get; set; }
        public string devicename { get; set; }
        public string osversion { get; set; }
        public string dongmay { get; set; }
        public string doimay { get; set; }
        public string imei { get; set; }
        public int ChiCaiDatPhanMem1Lan { get; set; }
        public int isFakeGPS { get; set; }
        public int isCheDoTietKiemPin { get; set; }
        public DateTime ngaycaidat { get; set; }
        public int TGThongBaoDenKeHoach { get; set; } //phut
        public int ChanDangNhapFakeGPS { get; set; }
        public int ChiTietMatHangKhiTaoDonHang { get; set; }
        public int ChiLapDonHangKhiVaoDiem { get; set; }
        public string AnhDaiDien { get; set; }
        public string AnhDaiDien_thumbnail_medium { get; set; }
        public string AnhDaiDien_thumbnail_small { get; set; }

        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public int GioiTinh { get; set; } // 1: Nam, 2: Nu
        public string QueQuan { get; set; }
        public DateTime NgaySinh { get; set; }
        public string SessionID { get; set; }
        public string CurrentIP { get; set; }
        public int Loai { get; set; }
        public int STT_DonHang { get; set; }
        public string MaNhom { get; set; }
        public string DinhDangNgayHienThi { get; set; }
        public int DinhDangTienSoThapPhan { get; set; }
        public int ID_NhomKhachHang_MacDinh { get; set; }
    }
}