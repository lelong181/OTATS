using LSPos_API.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_API.Model.RequestModel
{
    public class Req<T>
    {
        public T Value { get; set; }

        public string Hashed { get; set; }

        public string Json()
        {
            return JsonConvert.SerializeObject(Value);
        }

        public bool IsValid(string secretKey)
        {
            string jsonReq = Json() + secretKey;
            string jsonHashed = HashedUtil.GenerateSHA256String(jsonReq);
            return Hashed.Equals(jsonHashed);
        }
    }
}
