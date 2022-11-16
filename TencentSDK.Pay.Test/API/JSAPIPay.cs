using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TencentSDK.Pay.Test.API
{
    [TestClass]
    public class JSAPIPayTest : BaseTenPayTest
    {
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
    }
}