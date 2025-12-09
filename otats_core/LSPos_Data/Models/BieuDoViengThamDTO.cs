using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class BieuDoViengThamDTO
    {
        public BieuDoViengThamDTO()
        {
            title = "";
            categories = new List<string>();
            data = new List<int>();
        }
        public string title { get; set; }
        public List<string> categories { get; set; }
        public List<int> data { get; set; }
    }
}