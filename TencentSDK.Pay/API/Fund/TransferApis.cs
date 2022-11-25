using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TencentSDK.Pay
{
    public class TransferApis
    {
        private ITenPaySetting _tenPaySetting;

        public TransferApis(ITenPaySetting tenPaySetting)
        {
            this._tenPaySetting = tenPaySetting;
        }

        /// <summary>
        /// 发起商家转账API
        /// <para>https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter4_3_1.shtml</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public async Task<BatchesReturnJson> BatchesAsync(BatchesRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = Config.TenPayV3Host + "/v3/transfer/batches";
            TenPayAPIRequest tenPayApiRequest = new(_tenPaySetting);
            return await tenPayApiRequest.FetchJsonMessageAsync<BatchesReturnJson>(url, data, timeOut);
        }
    }
}