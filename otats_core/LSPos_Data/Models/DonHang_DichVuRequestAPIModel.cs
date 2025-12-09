using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class DonHang_DichVuRequestAPIModel
    {
        public int ID { get; set; }        
        public string BookingCode { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime Ngay { get; set; }
        public int HanSuDung { get; set; }
        public int ID_DonHang { get; set; }
        public int ID_ChiTietDonHang { get; set; }
        public int ID_DichVu { get; set; }
        public int ID_MatHang { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public float ThanhTien { get; set; }
        public string GhiChu { get; set; }
        public string ProfileCode { get; set; }
        public string ProfileID { get; set; }
        public string MemberID { get; set; }
        public string PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public DichVuModel DichVu { get; set; }
        public SiteModel Site { get; set; }
        public List<ChiTietMatHangDonHangModels> lstTicket { get; set; }
        public string BankAccNumber { get; set; }
        public string BankCode { get; set; }
        
    }
}
