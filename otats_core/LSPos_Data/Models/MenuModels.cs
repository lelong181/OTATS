using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class MenuModels
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string URL_NEW { get; set; }
        public string icon { get; set; }
    }
}