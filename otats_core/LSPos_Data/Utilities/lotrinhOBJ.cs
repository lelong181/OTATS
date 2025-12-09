using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for lotrinhmattinhieuOBJ2
/// </summary>
public class lotrinhOBJ
{
    public lotrinhOBJ() { }
    public string thoigian { get; set; }
    public string ghichu { get; set; }
    public double kinhdo { get; set; }
    public double vido { get; set; }
    public double accuracy { get; set; }
    public double speed { get; set; }
    public string thoigianketthuc { get; set; }
    public int idnhanvien { get; set; }
    public string tennhanvien { get; set; }
    public string tinhtrangpin { get; set; }

    public double ThoiGianDungDo_Giay
    {
        get
        {
            double tg = 0;

            if (speed == 0 && thoigianketthuc != null && DateTime.ParseExact(thoigianketthuc, new string[] { "dd/MM/yyyy HH:mm:ss" }, null, System.Globalization.DateTimeStyles.None).Year > 2000)
            {
                TimeSpan tsDung = DateTime.ParseExact(thoigianketthuc, new string[] { "dd/MM/yyyy HH:mm:ss" }, null, System.Globalization.DateTimeStyles.None) - DateTime.ParseExact(thoigian, new string[] { "dd/MM/yyyy HH:mm:ss" }, null, System.Globalization.DateTimeStyles.None);
                tg = tsDung.TotalSeconds > 20 ? tsDung.TotalSeconds : 0;
            }

            return tg;
        }


    }
}