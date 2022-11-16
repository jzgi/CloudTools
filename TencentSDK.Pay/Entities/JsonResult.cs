using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TencentSDK.Pay
{
    public class JsonResult
    {
        /// <summary>
        /// 回复状态码
        /// </summary>
        public TenPayApiResultCode ResultCode { get; set; } = new TenPayApiResultCode();

        /// <summary>
        /// 回复签名是否正确 在有错误的情况下，或不要求验证签名时 为null
        /// <para>通常情况下，必须为 true 才表明签名通过</para>
        /// </summary>
        public bool? VerifySignSuccess { get; set; } = null;
    }
}