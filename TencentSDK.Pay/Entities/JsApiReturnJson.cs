using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentSDK.Pay
{
    public class JsApiReturnJson : JsonResult
    {
        /// <summary>
        /// 预支付交易会话标识。用于后续接口调用中使用，该值有效期为2小时
        /// 示例值：wx201410272009395522657a690389285100
        /// </summary>
        public string prepay_id { get; set; }
    }
}
