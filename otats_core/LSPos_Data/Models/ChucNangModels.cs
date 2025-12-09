using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ChucNangModels
    {
        //public ChucNangModels()
        //{
        //    this.ListChild = new List<ChucNangModels>();
        //}
        public int ID_ChucNang { get; set; }
        public int ID_NhomChucNang { get; set; }
        public string TenChucNang { get; set; }
        public string TenNhomChucNang { get; set; }
        public string URL { get; set; }
        public string icon { get; set; }
        public string iconparent { get; set; }
        //public List<ChucNangModels> ListChild { set; get; }
    }
}