using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class DichVuModel
    {
        public int ID { get; set; } 
        public string MaDichVu { get; set; }
        public string TenDichVu { get;set; }
        public string MoTa { get;set; }
        public float GiaBan { get;set; }
        public string NgayTrongTuan { get;set; }
        public string SiteCode { get; set; }
        public int ID_NhaCungCap { get; set; }
        public DateTime BatDauBan { get;set; }
        public DateTime KetThucBan { get;set; }
    }
}
