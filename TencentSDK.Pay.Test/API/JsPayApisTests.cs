using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TencentSDK.Pay.Test
{
    [TestClass]
    public class JsPayApisTests : BaseTenPayTest
    {
        string openId = "oQ6Kf5qYzbz9UyttUjVYj6dhENyw";//换成测试人的 OpenId

        [TestMethod]
        public void CertificatesAsyncTest()
        {
            JsPayApis jsPayApis = new(_tenPaySetting);
            var certs = jsPayApis.CertificatesAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(certs);
        }

        [TestMethod]
        public void GetPublicKeyAsyncTest()
        {
            JsPayApis jsPayApis = new(_tenPaySetting);
            var publicKeys = jsPayApis.GetPublicKeysAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(publicKeys);
            Console.WriteLine(publicKeys);
        }

        TransactionsRequestData jsApiRequestData = null;
        [TestMethod]
        public void JsPayAsyncTest()
        {
            var price = 100;
            var name = "单元测试-" + DateTime.Now.ToString();
            var out_trade_no = string.Format("{0}{1}{2}", _tenPaySetting.TenPayV3_MchId/*10位*/, DateTimeOffset.Now.ToString("yyyyMMddHHmmss"),
                         TenPayV3Util.BuildRandomStr(6));
            var time_expire = DateTime.Now.AddHours(1).ToString($"yyyy-MM-ddTHH:mm:ss.fffzzz");

            //TODO: JsApiRequestData修改构造函数参数顺序
            jsApiRequestData = new(_tenPaySetting.TenPayV3_AppId, _tenPaySetting.TenPayV3_MchId, name,
             out_trade_no, time_expire, null, _tenPaySetting.TenPayV3_TenpayNotify, null,
             new() { currency = "CNY", total = price }, new(openId), null, null, null);

            JsPayApis jsPayApis = new(_tenPaySetting);
            var result = jsPayApis.JsPayAsync(jsApiRequestData).GetAwaiter().GetResult();
            Console.WriteLine("微信支付 V3 预支付结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }


        [TestMethod]
        public void OrderQueryByTransactionIdAsyncTest()
        {

            var transaction_id = "4200001673202211213161114305";//TODO: 这里应该填上已有订单的transaction_id

            JsPayApis jsPayApis = new(_tenPaySetting);
            var result = jsPayApis.OrderQueryByTransactionIdAsync(transaction_id, _tenPaySetting.TenPayV3_MchId, verifyTenPaySign: new VerifyTenPaySign()).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 订单查询结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }

        [TestMethod]
        public void OrderQueryByOutTradeNoAsyncTest()
        {

            if (jsApiRequestData == null)
            {
                JsPayAsyncTest();
            }

            var out_trade_no = jsApiRequestData.out_trade_no;// 这里应该填上已有订单的out_trade_no

            Console.WriteLine("out_trade_no：" + out_trade_no);

            JsPayApis jsPayApis = new(_tenPaySetting);
            var result = jsPayApis.OrderQueryByOutTradeNoAsync(out_trade_no, _tenPaySetting.TenPayV3_MchId).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 订单查询结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }

        [TestMethod]
        public void CloseOrderAsyncTest()
        {

            if (jsApiRequestData == null)
            {
                JsPayAsyncTest();
            }

            var out_trade_no = jsApiRequestData.out_trade_no;// 这里应该填上已有订单的out_trade_no
            // var out_trade_no = "162077360920221121203811119660";//TODO: 这里应该填上已有订单的out_trade_no

            JsPayApis jsPayApis = new(_tenPaySetting);
            var result = jsPayApis.CloseOrderAsync(out_trade_no, _tenPaySetting.TenPayV3_MchId).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 订单关闭结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            // Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }

        [TestMethod]
        public void RefundAsyncTest()
        {

            var transaction_id = "4200001673202211213161114305";//TODO: 应该填入订单的transaction_id
            var out_refund_no = "162077360920221121203811119660";//TODO: out_refund_no

            JsPayApis jsPayApis = new(_tenPaySetting);

            //查询订单获得订单金额
            var total = jsPayApis.OrderQueryByOutTradeNoAsync(out_refund_no, _tenPaySetting.TenPayV3_MchId).GetAwaiter().GetResult().amount.total;

            //请求退款
            RefundRequsetData requestData = new(transaction_id, null, out_refund_no, "退款单元测试", null, null, new(total, null, total, "CNY"), null);
            var result = jsPayApis.RefundAsync(requestData, verifyTenPaySign: new VerifyTenPaySign()).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 退款结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }
        [TestMethod]
        public void RefundQueryAsyncTest()
        {
            var out_refund_no = "162077360920221121203811119660";//TODO: 这里应该填上已有订单的out_refund_no

            JsPayApis jsPayApis = new(_tenPaySetting);
            var result = jsPayApis.RefundQueryAsync(out_refund_no, verifyTenPaySign: new VerifyTenPaySign()).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 退款查询结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }

    }
}