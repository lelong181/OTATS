using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class NhanVienHinhThucTTModels
    {
        public int ID { get; set; }
        public int ID_NhanVien { get; set; }
        public int ID_HTTT { get; set; }
        public DateTime NgayTao { get; set; } 
    }
}
