using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TencentSDK.Pay.Test.API
{
    [TestClass]
    public class JSAPIPayTests : BaseTenPayTest
    {
        string openId = "oQ6Kf5qYzbz9UyttUjVYj6dhENyw";//换成测试人的 OpenId

        [TestMethod]
        public void CertificatesAsyncTest()
        {
            JSAPIPay jSAPIPay = new JSAPIPay(_tenPaySetting);
            var certs = jSAPIPay.CertificatesAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(certs);
        }

        [TestMethod]
        public void GetPublicKeyAsyncTest()
        {
            JSAPIPay jSAPIPay = new JSAPIPay(_tenPaySetting);
            var publicKeys = jSAPIPay.GetPublicKeysAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(publicKeys);
            Console.WriteLine(publicKeys);
        }

        TransactionsRequestData jsApiRequestData = null;
        [TestMethod]
        public void JsAPIAsyncTest()
        {
            var price = 100;
            var name = "单元测试-" + DateTime.Now.ToString();
            var sp_billno = string.Format("{0}{1}{2}", _tenPaySetting.TenPayV3_MchId/*10位*/, DateTimeOffset.Now.ToString("yyyyMMddHHmmss"),
                         TenPayV3Util.BuildRandomStr(6));
            var time_expire = DateTime.Now.AddHours(1).ToString($"yyyy-MM-ddTHH:mm:ss.fffzzz");

            //TODO: JsApiRequestData修改构造函数参数顺序
            jsApiRequestData = new(_tenPaySetting.TenPayV3_AppId, _tenPaySetting.TenPayV3_MchId, name,
             sp_billno, time_expire, null, _tenPaySetting.TenPayV3_TenpayNotify, null,
             new() { currency = "CNY", total = price }, new(openId), null, null, null);

            JSAPIPay basePayApis = new JSAPIPay(_tenPaySetting);
            var result = basePayApis.JsAPIAsync(jsApiRequestData).GetAwaiter().GetResult();
            Console.WriteLine("微信支付 V3 预支付结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }


        [TestMethod()]
        public void OrderQueryByTransactionIdAsyncTest()
        {

            var transaction_id = "1217752501201407033233368018";//TODO: 这里应该填上已有订单的transaction_id

            JSAPIPay basePayApis = new JSAPIPay(_tenPaySetting);
            var result = basePayApis.OrderQueryByTransactionIdAsync(transaction_id, _tenPaySetting.TenPayV3_MchId).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 订单查询结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }

        [TestMethod()]
        public void OrderQueryByOutTradeNoAsyncTest()
        {

            if (jsApiRequestData == null)
            {
                JsAPIAsyncTest();
            }

            var out_trade_no = jsApiRequestData.out_trade_no;// 这里应该填上已有订单的out_trade_no

            Console.WriteLine("out_trade_no：" + out_trade_no);

            JSAPIPay basePayApis = new JSAPIPay(_tenPaySetting);
            var result = basePayApis.OrderQueryByOutTradeNoAsync(out_trade_no, _tenPaySetting.TenPayV3_MchId).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 订单查询结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }

        [TestMethod()]
        public void CloseOrderAsyncTest()
        {

            var out_trade_no = "out_trade_no";//TODO: 这里应该填上已有订单的out_trade_no

            JSAPIPay basePayApis = new JSAPIPay(_tenPaySetting);
            var result = basePayApis.CloseOrderAsync(out_trade_no, _tenPaySetting.TenPayV3_MchId).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 订单关闭结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }

    }
}