using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PhienLamViecOBJ
/// </summary>
public class ChiTietPhienLamViecOBJ
{
    public ChiTietPhienLamViecOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int idnhanvien { get; set; } 
    public string thoigianbatdau { get; set; }
    public string thoigianketthuc { get; set; }
    public int loai { get; set; }
    public string tenloai { get; set; }
    public double kinhdo { get; set; }
    public double vido { get; set; }
    public double accuracy { get; set; }
}