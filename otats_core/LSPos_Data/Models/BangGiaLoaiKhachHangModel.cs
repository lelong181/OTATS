using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class BangGiaLoaiKhachHangModel
    {
        public int ID { get; set; }
        public int IDMatHang { get; set; }
        public int IDLoaiKhachHang { get; set; }
        public double GiaBanBuon { get; set; }
        public double GiaBanLe { get; set; }
        public string GhiChu { get; set; }
        public bool DeleteMark { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public string TenLoaiKhachHang { get; set; }
    }
}