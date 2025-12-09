using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ExportKhachHangDTO
    {
            public ExportKhachHangDTO()
            {
                this.diaChi = "";
                this.tenKhachHang = "";
                this.maKH = "";
                this.tenTinh = "";
                this.tenQuan = "";
                this.tenPhuong = "";
                this.dienThoai = "";
                this.email = "";
                this.tenNhanVien = "";
                this.tenLoaiKhachHang = "";
                this.tenNhomKH = "";
                this.maSoThue = "";
                this.nguoiLienHe = "";
                this.ghiChu = "";
                this.ngayTao = null;
            }
            public string diaChi { get; set; }
            public string tenKhachHang { get; set; }
            public string maKH { get; set; }
            public string tenTinh { get; set; }
            public string tenQuan { get; set; }
            public string tenPhuong { get; set; }
            public string dienThoai { get; set; }
            public string email { get; set; }
            public string tenNhanVien { get; set; }
            public string tenLoaiKhachHang { get; set; }
            public string tenNhomKH { get; set; }
            public string maSoThue { get; set; }
            public string nguoiLienHe { get; set; }
            public string ghiChu { get; set; }
            public DateTime? ngayTao { get; set; }

        }
    }