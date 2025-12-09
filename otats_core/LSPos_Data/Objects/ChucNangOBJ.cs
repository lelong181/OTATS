using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyOBJ
/// </summary>
public class ChucNangOBJ
{
    public ChucNangOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int STT { get; set; }
    public int ID_Nhom { get; set; }
    public int ID_ChucNang { get; set; }
    public string URL { get; set; }
    public string TenChucNang { get; set; }
    public DateTime InsertedTime { get; set; }
    public int TrangThai { get; set; }
    public string TenTrangThai { get; set; }
    public int LoaiThietBi { get; set; }
    public string TenLoaiThietBi { get; set; }
    public int ID_NhomChucNang { get; set; }
    public string ViTriChucNang { get; set; }
    public string TenNhomChucNang { get; set; }
    public string MaChucNang { get; set; }
    public int Them { get; set; }
    public int Sua { get; set; }
    public int Xoa { get; set; }
}