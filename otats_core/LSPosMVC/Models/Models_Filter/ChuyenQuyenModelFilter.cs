using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSPosMVC.Models
{
    public class ChuyenQuyenModelFilter
    {
        public int ID_NhanVien_Old { get; set; }
        public List<int> List_ID_NhanVien { get; set; }
        public List<int> List_ID_KhachHang { get; set; }
    }
}