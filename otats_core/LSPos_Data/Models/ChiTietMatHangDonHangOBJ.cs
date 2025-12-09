using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChiTietMatHangDonHangOBJ
{
    public long ID_ChiTiet_MatHang_DonHang { get; set; }
    public string MoTa { get; set; }
    public long ID_DonHang { get; set; }
    public long ID_MatHang { get; set; }
    public double ChieuDai { get; set; }
    public double ChieuRong { get; set; }
    public double ChieuCao { get; set; }
    public double SoLuong { get; set; }
    public double Giaban { get; set; }
    public double TongTien { get; set; }
    public string NgayTao { get; set; }
}