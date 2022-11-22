using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TencentSDK.Pay
{
    public interface IVerifyTenPaySign
    {
        public Task<string> GetAPIv3PublicKeyAsync(ITenPaySetting tenPaySetting, string wechatpaySerial);
    }

    public class VerifyTenPaySign : IVerifyTenPaySign
    {
        public async Task<string> GetAPIv3PublicKeyAsync(ITenPaySetting tenPaySetting, string wechatpaySerial)
        {
            JSAPIPay jSAPIPay = new JSAPIPay(tenPaySetting);
            var keys = await jSAPIPay.GetPublicKeysAsync();
            if (keys.TryGetValue(wechatpaySerial, out string publicKey))
            {
                return publicKey;
            }

            throw new TenPayAPIException("公钥序列号不存在！请查看日志！");
        }
    }

    public class TenPaySignHelper
    {
        /// <summary>
        /// 获取调起支付所需的签名
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="nonceStr">随机串</param>
        /// <param name="package">格式：prepay_id={0}</param>
        /// <param name="privateKey">商户证书私钥</param>
        /// <returns></returns>
        public static string CreatePaySign(string timeStamp, string nonceStr, string package, string appId, string privateKey)
        {
            string contentForSign = $"{appId}\n{timeStamp}\n{nonceStr}\n{package}\n";
            return CreateSign(contentForSign, privateKey);
        }
        /// <summary>
        /// 微信支付签名方法
        /// </summary>
        /// <param name="message"></param>
        /// <param name="privateKey">私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----亦不包括结尾的-----END PRIVATE KEY-----</param>
        /// <returns></returns>
        public static string CreateSign(string message, string privateKey)
        {
            _ = privateKey ?? throw new ArgumentException($"{nameof(privateKey)} 不能为 null！");

            byte[] keyData = Convert.FromBase64String(privateKey);

            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(keyData, out _);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                return Convert.ToBase64String(rsa.SignData(data, 0, data.Length, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
        }

        /// <summary>
        /// 检验签名，以确保回调是由微信支付发送。
        /// 签名规则见微信官方文档 https://pay.weixin.qq.com/wiki/doc/apiv3/wechatpay/wechatpay4_1.shtml。
        /// return bool
        /// </summary>
        /// <param name="wechatpayTimestamp">HTTP头中的应答时间戳</param>
        /// <param name="wechatpayNonce">HTTP头中的应答随机串</param>
        /// <param name="wechatpaySignatureBase64">HTTP头中的应答签名（Base64）</param>
        /// <param name="content">应答报文主体</param>
        /// <param name="pubKey">平台公钥（必须是Unwrap的公钥）</param>
        /// <returns></returns>
        public static bool VerifyTenpaySign(string wechatpayTimestamp, string wechatpayNonce, string wechatpaySignatureBase64, string content, string pubKey)
        {
            //验签名串
            string contentForSign = $"{wechatpayTimestamp}\n{wechatpayNonce}\n{content}\n";

            //Base64 解码 pubKey（必须已经使用 ApiSecurityHelper.GetUnwrapCertKey() 方法进行 Unwrap）
            var bs = Convert.FromBase64String(pubKey);
            //使用 X509Certificate2 证书
            var x509 = new X509Certificate2(bs);
            //AsymmetricAlgorithm对象
            var key = x509.PublicKey.Key;

            //RSAPKCS1SignatureDeformatter 对象
            RSAPKCS1SignatureDeformatter df = new RSAPKCS1SignatureDeformatter(key);
            //指定 SHA256
            df.SetHashAlgorithm("SHA256");
            //SHA256Managed 方法已弃用，使用 SHA256.Create() 生成 SHA256 对象
            var sha256 = SHA256.Create();
            //应答签名
            byte[] signature = Convert.FromBase64String(wechatpaySignatureBase64);
            //对比签名
            byte[] compareByte = sha256.ComputeHash(Encoding.UTF8.GetBytes(contentForSign));
            //验证签名
            var result = df.VerifySignature(compareByte, signature);

            return result;
        }

    }
}