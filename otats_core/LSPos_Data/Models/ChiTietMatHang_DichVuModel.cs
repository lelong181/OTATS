using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class ChiTietMatHang_DichVuModel
    {
        public long ID { get; set; }
        public long ID_ChiTietDonHang  { get; set; }
        public long ID_DonHang { get; set; }
        public long ID_MatHang { get; set; }
        public long ID_DichVu { get; set; }
        public int SoLuong { get; set; }
        public float GiaBan { get; set; }
        public int Loai { get; set; }
        public string TenHienThi { get; set; }
        public string BookingCode { get; set; }
        public string InvoiceCode { get; set; }
    }
}
