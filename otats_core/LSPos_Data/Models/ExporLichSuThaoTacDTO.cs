using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ExporLichSuThaoTacDTO
    {

        public ExporLichSuThaoTacDTO()
        {
            this.idKhachHang = 0;
            this.idNhanVien = 0;
            this.thaoTacUser = "";
            this.from = DateTime.Now;
            this.to = DateTime.Now;
            this.thaoTac = 0;
            this.noiDung = "";
            this.anhDaiDien = "";
            this.anhDaiDien_thumbnail_small = "";
            this.thaoTacHienThi = "";
            this.noiDungHienThi = "";
            this.nhanVien = "";
            this.thoiGian = "";
            this.diaChi = "";
        }
        public int idKhachHang { set; get; }
        public int idNhanVien { set; get; }
        public string thaoTacUser { set; get; }
        public DateTime from { set; get; }
        public DateTime to { set; get; }
        public int thaoTac { set; get; }
        public string noiDung { set; get; }
        public string anhDaiDien { set; get; }
        public string anhDaiDien_thumbnail_small { set; get; }
        public string thaoTacHienThi { set; get; }
        public string noiDungHienThi { set; get; }
        public string nhanVien { set; get; }
        public string thoiGian { set; get; }
        public string diaChi { set; get; }
    }
}
