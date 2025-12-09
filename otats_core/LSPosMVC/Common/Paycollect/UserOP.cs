using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common.Paycollect
{
    public class UserOP
    {
        public string id {  get; set; }
        public string name {  get; set; }
        public string state {  get; set; }
        public string create_time {  get; set; }
        public AccountOP accounts {  get; set; }

    }
}
