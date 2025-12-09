using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSPosMVC.Models
{
    public class GroupLinkModel
    {
        public List<int> listid { get; set; } = new List<int>();
        public string grouptext { get; set; }
    }
}