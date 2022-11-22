using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TencentSDK.Pay.Test
{
    [TestClass]
    public class TenPayV3UtilTests
    {
        [TestMethod]
        public void GetNoncestrTest()
        {
            string nonce_str = TenPayV3Util.GetNoncestr();
            Console.WriteLine(nonce_str);
            Assert.IsNotNull(nonce_str);
        }

        [TestMethod]
        public void GetTimestampTest()
        {
            string timeStamp = TenPayV3Util.GetTimestamp();
            Console.WriteLine(timeStamp);
            Assert.IsNotNull(timeStamp);
        }


    }
}