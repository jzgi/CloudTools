using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TencentSDK.Pay
{
    public class TenPayAPIException : Exception
    {
        public TenPayAPIException(string message) : base(message)
        {
        }

        public TenPayAPIException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}