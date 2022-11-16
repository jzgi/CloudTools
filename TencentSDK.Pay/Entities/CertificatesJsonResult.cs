using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TencentSDK.Pay
{
    public class CertificatesJsonResult : JsonResult
    {
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string serial_no { get; set; }
        public DateTime effective_time { get; set; }
        public DateTime expire_time { get; set; }
        public Encrypt_Certificate encrypt_certificate { get; set; }
    }

    public class Encrypt_Certificate
    {
        public string algorithm { get; set; }
        public string nonce { get; set; }
        public string associated_data { get; set; }
        public string ciphertext { get; set; }
    }

}