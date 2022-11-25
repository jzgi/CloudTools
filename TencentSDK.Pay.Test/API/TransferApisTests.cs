using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TencentSDK.Pay.Test
{
    [TestClass]
    public class TransferApisTests : BaseTenPayTest
    {
        string openId = "oQ6Kf5qYzbz9UyttUjVYj6dhENyw";//换成测试人的 OpenId
        [TestMethod]
        public void BatchesAsyncTest()
        {
            var price = 1;
            var name = "单元测试-" + DateTime.Now.ToString();
            var out_batch_no = string.Format("{0}{1}{2}", _tenPaySetting.TenPayV3_MchId/*10位*/, DateTimeOffset.Now.ToString("yyyyMMddHHmmss"),
                         TenPayV3Util.BuildRandomStr(6));
            var time_expire = DateTime.Now.AddHours(1).ToString($"yyyy-MM-ddTHH:mm:ss.fffzzz");
            var out_detail_no = out_batch_no + "1";
            BatchesRequestData batchesRequestData = new(_tenPaySetting.TenPayV3_AppId, out_batch_no,
             name, name, price, 1, new Transfer_Detail_List[] {
                 new() { out_detail_no=out_detail_no, transfer_amount=price, transfer_remark=name, openid=openId}
            });

            TransferApis transferApis = new(_tenPaySetting);
            var result = transferApis.BatchesAsync(batchesRequestData).GetAwaiter().GetResult();
            Console.WriteLine("微信支付商户转账到零钱结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }
    }
}