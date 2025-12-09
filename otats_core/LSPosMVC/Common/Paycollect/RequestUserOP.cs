using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common.Paycollect
{
    public class  RequestUserOP
    {
        public string name {  get; set; }
        public string gender {  get; set; }
        public string address {  get; set; }
        public string mobile_number {  get; set; }
        public string email {  get; set; }
        public string id_card {  get; set; }
        public string issue_date {  get; set; }
        public string issue_by {  get; set; }
        public string bank_id {  get; set; }
        public string description {  get; set; }
        public string reference { get; set; }
    }
}
