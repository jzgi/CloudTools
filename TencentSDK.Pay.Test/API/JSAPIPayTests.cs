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
            JSAPIPay jSAPIPay = new JSAPIPay(tenPaySetting);
            var certs = jSAPIPay.CertificatesAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(certs);
        }

        [TestMethod]
        public void GetPublicKeyAsyncTest()
        {
            JSAPIPay jSAPIPay = new JSAPIPay(tenPaySetting);
            var publicKeys = jSAPIPay.GetPublicKeysAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(publicKeys);
            Console.WriteLine(publicKeys);
        }

        TransactionsRequestData jsApiRequestData = null;
        [TestMethod]
        public void JsApiAsyncTest()
        {


            var price = 100;
            var name = "单元测试-" + DateTime.Now.ToString();
            var sp_billno = string.Format("{0}{1}{2}", tenPaySetting.TenPayV3_MchId/*10位*/, DateTimeOffset.Now.ToString("yyyyMMddHHmmss"),
                         TenPayV3Util.BuildRandomStr(6));
            var time_expire = DateTime.Now.AddHours(1).ToString($"yyyy-MM-ddTHH:mm:ss.fffzzz");

            //TODO: JsApiRequestData修改构造函数参数顺序
            jsApiRequestData = new(tenPaySetting.TenPayV3_AppId, tenPaySetting.TenPayV3_MchId, name,
             sp_billno, time_expire, null, tenPaySetting.TenPayV3_TenpayNotify, null,
             new() { currency = "CNY", total = price }, new(openId), null, null, null);

            JSAPIPay basePayApis = new JSAPIPay(tenPaySetting);
            var result = basePayApis.JsApiAsync(jsApiRequestData).GetAwaiter().GetResult();

            Console.WriteLine("微信支付 V3 预支付结果：" + result.ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultCode.Success);
            Assert.IsTrue(result.VerifySignSuccess == true);//通过验证
        }
    }
}