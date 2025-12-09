using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class HangHoa_LoiNhuanModel
    {
        public int ID { set; get; }
        public int ID_HangHoa { set; get; }
        public int SoLuongToiThieu { set; get; }
        public int SoLuongToiDa { set; get; }
        public int ID_NhomTaiKhoan { set; get; }
        public string TenNhom { get; set; }
        public float TyLeHoaHong { set; get; }
        public float TienHoaHong { set; get; }
        public DateTime NgayTao { set; get; }
        public int TrangThai { set; get; }
    }
}