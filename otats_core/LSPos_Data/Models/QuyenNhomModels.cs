using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class QuyenNhomModels
    {
        public int ID_Nhom { get; set; }
        public List<QuyenModels> ListChucNangWEB { get; set; }
        public List<QuyenModels> ListChucNangAPP { get; set; }
    }
}