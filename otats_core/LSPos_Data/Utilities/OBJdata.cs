using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for lotrinhmattinhieuOBJ2
/// </summary>
public class OBJdata
{
    public OBJdata() { }

    public bool status { get; set; }
   
    public string msg { get; set; }

    public lotrinhOBJ LoTrinhDauTien { get; set; }
    public lotrinhOBJ LoTrinhCuoiCung { get; set; }

    public List<lotrinhOBJ> datalotrinh_suydien { get; set; }
    public List<lotrinhOBJ> datalotrinh_suydien_offline { get; set; }
    public List<lotrinhOBJ> datalotrinh { get; set; }
    public List<banglotrinhOBJ> databanglotrinh { get; set; }
    public List<lotrinhtrinhvaodiemOBJ> datavaodiem { get; set; }
    public List<lotrinhtrinhvaodiemOBJ> dataradiem { get; set; }
    public List<lotrinhmattinhieuOBJ> datamattinhieu { get; set; }

    public List<KeHoachDiChuyenObj> dataKeHoachDiChuyen { get; set; }

}