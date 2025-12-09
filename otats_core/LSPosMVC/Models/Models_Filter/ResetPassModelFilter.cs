using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSPosMVC.Models
{
    public class ResetPassModelFilter
    {
        public int id { get; set; }
        public string newpass { get; set; }
        public string username { get; set; }
    }
}