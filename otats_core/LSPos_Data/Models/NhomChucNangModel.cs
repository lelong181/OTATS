using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class NhomChucNangModel
    {
        public int ID_NhomChucNang { get; set; }
        public int STT { get; set; }
        public string TenNhomChucNang { get; set; }
        public int IDParent { get; set; }
        public string icon { get; set; }
    }
}