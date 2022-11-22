using System.Text;
using System.Security.Cryptography;

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

        /// <summary>
        /// 随机生成Noncestr
        /// </summary>
        /// <returns></returns>
        public static string GetNoncestr()
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            // MD5 m5 = MD5.Create();
            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用指定编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding("utf-8").GetBytes(Guid.NewGuid().ToString());
            outputBye = m5.ComputeHash(inputBye);

            retStr = BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 获取微信时间格式
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            DateTime startTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime nowTime = DateTime.UtcNow;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            long timestamp = unixTime / 1000;
            return timestamp.ToString();
        }



    }
}
