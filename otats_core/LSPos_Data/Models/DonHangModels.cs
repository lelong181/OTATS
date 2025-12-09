using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class DonHangModels
    {
        public DonHangModels()
        {
        }

        public string mathamchieu { get; set; }

        public int iddonhang { get; set; }

        public int idct { get; set; }

        public int idnhanvien { get; set; }

        public int hdv { get; set; }

        public int idcuahang { get; set; }
        public int isProcess { get; set; }
        public double tongtien { get; set; }
        //public double DaGiao { get; set; }
        public string ghichu { get; set; }

        public DateTime thoigiantao { get; set; }
        public int trangthaigiaohang { get; set; }
        public int trangthaithanhtoan { get; set; }
        public int trangthaidonhang { get; set; }
        public List<ChiTietDonHangModels> chitietdonhang { get; set; }

        public double chietkhauphantram { get; set; }
        public double chietkhautien { get; set; }
        public double chietkhauphantram_theoctkm { get; set; }
        public double chietkhautien_theoctkm { get; set; }
        public double chietKhauPhanTramKhac { get; set; }
        public double chietKhauTienKhac { get; set; }
        public double tongtienchietkhau { get; set; }
        public int idctkm { get; set; }
        public string tenctkm { get; set; }
        public int idcheckin { get; set; }
        public string macuahang { get; set; }
        public string tenkhachhang { get; set; }

        public double tiendathanhtoan { get; set; }

        public double phantramhaohut { get; set; }
        public double soluonghaohut { get; set; }
        public int idnhanvientao { get; set; }
        public double kinhdo { get; set; }
        public double vido { get; set; }
        public string diachitao { get; set; }
        public string diachixuathoadon { get; set; }
        public string sodienthoai { get; set; }
        public string diachikhachhang { get; set; }
        public List<AlbumOBJ> albumanh { get; set; }        
        public NhanVien nhanvien { get; set; }

        public string tenvilspay { get; set; }
        public string LS_AccountCode { get; set; }
        public string LS_BookingCode { get; set; }
        public bool xuathoadon { get; set; }
        public bool invetaiquay { get; set; }
        public int hinhthucthanhtoan { get; set; }
        public int hinhthucban { get; set; }
        public float phuthuphantram { get; set; }
        public float phuthutien { get; set; }
        public string lydophuthu { get; set; }
        public string vpc_SecureHash { get; set; }
        public string echotoken { get; set; }

    }

    public class GiaoHangOBJ
    {
        public GiaoHangOBJ() { }

        public int iddonhang { get; set; }

        public int idnhanvien { get; set; }
        public string ghichu { get; set; }
        public int idquanly { get; set; }
        public int langiao { get; set; }
        public DateTime ngaygiao { get; set; }

        public List<ChiTietGiaoHangOBJ> chitietgiaohang { get; set; }

    }

    public class ChiTietGiaoHangOBJ
    {
        public ChiTietGiaoHangOBJ() { }

        public int idhang { get; set; }

        public double soluonggiao { get; set; }

        public string ghichu { get; set; }

        public int hinhthucban { get; set; }
        public string tenhang { get; set; }
        public string tendonvi { get; set; }
        public string tendanhmuc { get; set; }
        public string mahang { get; set; }
        public double giabanbuon { get; set; }
        public double giabanle { get; set; }
        public int idkho { get; set; }        

    }
}

