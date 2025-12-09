using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyOBJ
/// </summary>
public class CongTyOBJ
{
    public CongTyOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int idcongty { get; set; }
    public string tencongty { get; set; }
    public string sodienthoai { get; set; }
    public int thoigiancapnhatbantin { get; set; }
    public int goiungdungid { get; set; }
    public string urlserver { get; set; }
    public double bankinhchophep { get; set; }
    public DateTime thoihanhopdong { get; set; }
    public int soluongnhanvien_duocap { get; set; }
}