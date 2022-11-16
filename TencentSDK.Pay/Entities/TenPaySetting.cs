using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TencentSDK.Pay
{
    public class TenPaySetting : ITenPaySetting
    {
        /// <summary>
        /// MchId（商户ID）
        /// </summary>
        public virtual string TenPayV3_MchId { get; set; }

        /// <summary>
        /// MchKey
        /// </summary>
        public virtual string TenPayV3_Key { get; set; }
        /// <summary>
        /// 微信支付证书位置（物理路径），在 .NET Core 下执行 TenPayV3InfoCollection.Register() 方法会为 HttpClient 自动添加证书
        /// </summary>
        public virtual string TenPayV3_CertPath { get; set; }
        /// <summary>
        /// 微信支付证书密码，在 .NET Core 下执行 TenPayV3InfoCollection.Register() 方法会为 HttpClient 自动添加证书
        /// </summary>
        public virtual string TenPayV3_CertSecret { get; set; }
        /// <summary>
        /// 微信支付AppId
        /// </summary>
        public virtual string TenPayV3_AppId { get; set; }
        /// <summary>
        /// 微信支付AppSecert
        /// </summary>
        public virtual string TenPayV3_AppSecret { get; set; }

        /// <summary>
        /// 微信支付TenpayNotify
        /// </summary>
        public virtual string TenPayV3_TenpayNotify { get; set; }

        /// <summary>
        /// 微信支付（V3）证书私钥
        /// <para>获取途径：apiclient_key.pem</para>
        /// </summary>
        public virtual string TenPayV3_PrivateKey { get; set; }
        /// <summary>
        /// 微信支付（V3）证书序列号
        /// <para>查看地址：https://pay.weixin.qq.com/index.php/core/cert/api_cert#/api-cert-manage</para>
        /// </summary>
        public virtual string TenPayV3_SerialNumber { get; set; }
        /// <summary>
        /// APIv3 密钥。在微信支付后台设置：https://pay.weixin.qq.com/index.php/core/cert/api_cert#/
        /// </summary>
        public virtual string TenPayV3_APIv3Key { get; set; }
    }
    public interface ITenPaySetting
    {
        /// <summary>
        /// MchId（商户ID）
        /// </summary>
        string TenPayV3_MchId { get; set; }

        /// <summary>
        /// 微信支付AppId
        /// </summary>
        string TenPayV3_AppId { get; set; }
        /// <summary>
        /// 微信支付AppSecert
        /// </summary>
        string TenPayV3_AppSecret { get; set; }
        /// <summary>
        /// APIv3 密钥。在微信支付后台设置：https://pay.weixin.qq.com/index.php/core/cert/api_cert#/
        /// </summary>
        string TenPayV3_APIv3Key { get; set; }

        /// <summary>
        /// 微信支付TenpayNotify
        /// </summary>
        string TenPayV3_TenpayNotify { get; set; }

        /// <summary>
        /// 微信支付（V3）证书私钥
        /// <para>获取途径：apiclient_key.pem</para>
        /// </summary>
        string TenPayV3_PrivateKey { get; set; }
        /// <summary>
        /// 微信支付（V3）证书序列号
        /// <para>查看地址：https://pay.weixin.qq.com/index.php/core/cert/api_cert#/api-cert-manage</para>
        /// </summary>
        string TenPayV3_SerialNumber { get; set; }
    }
}