using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class AlbumModel
    {
        public long ID_Album {get; set; }
        public DateTime NgayTao { get; set; }
        public float KinhDo { get; set; }
        public float ViDo { get; set; }
        public string HinhDaiDien { get; set; }
        public long ID_KhachHang { get; set; }
        public long ID_NhanVien { get; set; }
        public long ID_QLLH { get; set; }
        public string GhiChu { get; set; }
        public string DiaChi { get; set; }
        public int ID_LoaiAlbum { get; set; }
        public int Loai { get; set; } //0 : Anh dai ly, 1 : Bien bang, 2: San pham , 3: anh bao duong, 4 :don hang, 5: cong viec
        public int ID_TanSuatViengTham { get; set; }
        public DateTime NgayKeHoach { get; set; }
        public DateTime NgayViengTham { get; set; }
        public int ID_Xe_LichSuBD { get; set; }
        public int ID_DonHang { get; set; }
        public int ID_CongViec { get; set; }
        public int ID_CheckIn { get; set; }

        public string TenKhachHang { get; set; }
        public string TenNhanVien { get; set; }
        public string DiaChiKhachHang { get; set; }
    }
}
