using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string MaCongty { get; set; }
        public string PassWord { get; set; }
        public int IsNhanVien { get; set; }
        public string IDPUSH { get; set; }
    }
}
