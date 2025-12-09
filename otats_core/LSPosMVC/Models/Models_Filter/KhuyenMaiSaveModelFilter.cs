using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSPosMVC.Models
{
    public class KhuyenMaiSaveModelFilter
    {
        public KhuyenMai KhuyenMaiObj { get; set; }
        public DataTable DanhSachHangTang { get; set; }
        public string url_img { get; set; }
    }
}