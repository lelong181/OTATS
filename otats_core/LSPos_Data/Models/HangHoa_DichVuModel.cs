using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class HangHoa_DichVuModel
    {
        public int ID { get; set; }
        public int ID_HangHoa { get; set; }
        public int ID_DichVu { get; set; }
        public int SoLuong { get; set; }
        public int HanSuDung { get; set; }
        public float GiaBan { get; set; }
        public int TrangThai { get; set; }
        public int Loai { get; set; } // 1: Optinal, 2: Fixed
        public string TenHienThi { get; set; }
        public int ID_NhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
    }
}
