using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common.Paycollect
{
    public class BatchOP
    {
        public string name {  get; set; }
        public string do_on {  get; set; }
        public string method {  get; set; }
        public string href {  get; set; }
        public object body { get; set; }
    }
}
