using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class BieuDoDiChuyenDTO
    {
        public BieuDoDiChuyenDTO()
        {
            title = "";
            categories = new List<string>();
            data = new List<double>();
        }
        public string title { get; set; }
        public List<string> categories { get; set; }
        public List<double> data { get; set; }
    }
}