using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common.Paycollect
{
    public class BatchOPResponse
    {
        public string status { get;set; }
        public string headers { get;set; }
        public object body { get;set; }
    }
}
