using System;
using System.Text;
using System.Net;

namespace TencentSDK.Pay
{
    /// <summary>
    /// 微信支付工具类
    /// </summary>
    public class TenPayV3Util
    {
        public static Random random = new Random();

        /// <summary>
        /// 取随机数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BuildRandomStr(int length)
        {
            int num;

            lock (random)
            {
                num = random.Next();
            }

            string str = num.ToString();

            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str = str.Insert(0, "0");
                    n--;
                }
            }

            return str;
        }


    }
}
