using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace TencentSDK.Pay
{
    public enum ApiRequestMethod
    {
        GET,
        POST,
        DELETE,
        PUT,
        PATCH,
    }
    /// <summary>
    /// 微信支付API请求
    /// </summary>    
    public class TenPayAPIRequest
    {
        public ITenPaySetting _tenpayV3Setting { get; }

        public TenPayAPIRequest(ITenPaySetting paySetting)
        {
            _tenpayV3Setting = paySetting;
        }

        public void SetHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Senparc.Weixin.TenPayV3-C#", typeof(TenPayAPIRequest).Assembly.GetName().Version.ToString()));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(".NET", Environment.Version.ToString()));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue($"({Environment.OSVersion.ToString()})"));
        }

        /// <summary>
        /// 微信支付接口请求方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="timeOut"></param>
        /// <param name="requestMethod"></param>
        /// <param name="checkDataNotNull"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> FetchResponseMessageAsync(string url, object data, int timeOut = 10000, ApiRequestMethod requestMethod = ApiRequestMethod.POST, bool checkDataNotNull = true)
        {
            try
            {
                TenPayHttpHandler httpHandler = new(_tenpayV3Setting);

                //创建 HttpClient
                HttpClient client = new HttpClient(httpHandler);//TODO: 有资源消耗和效率问题
                //设置超时时间
                client.Timeout = TimeSpan.FromMilliseconds(timeOut);

                //设置 HTTP 请求头
                SetHeader(client);

                HttpResponseMessage responseMessage = null;
                switch (requestMethod)
                {
                    case ApiRequestMethod.GET:
                        responseMessage = await client.GetAsync(url);

                        break;
                    //TODO: 此处新增DELETE方法 待测试是否有问题
                    case ApiRequestMethod.DELETE:
                        responseMessage = await client.DeleteAsync(url);
                        break;
                    case ApiRequestMethod.POST:
                    case ApiRequestMethod.PUT:
                    case ApiRequestMethod.PATCH:
                        //检查是否为空
                        if (checkDataNotNull)
                        {
                            _ = data ?? throw new ArgumentNullException($"{nameof(data)} 不能为 null！");
                        }

                        //设置请求 Json 字符串
                        var options = new JsonSerializerOptions { IgnoreNullValues = true };
                        string jsonString = data != null
                            ? JsonSerializer.Serialize(data, options)
                            : "";

                        //设置 HttpContent
                        var hc = new StringContent(jsonString, Encoding.UTF8, mediaType: "application/json");
                        //获取响应结果
                        responseMessage = requestMethod switch
                        {
                            ApiRequestMethod.POST => await client.PostAsync(url, hc),
                            ApiRequestMethod.PUT => await client.PutAsync(url, hc),
                            ApiRequestMethod.PATCH => await client.PatchAsync(url, hc),
                            _ => throw new ArgumentOutOfRangeException(nameof(requestMethod))
                        };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(requestMethod));
                }

                return responseMessage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 微信支付接口请求方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="timeOut"></param>
        /// <param name="requestMethod"></param>
        /// <param name="verifyTenPaySign"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> FetchJsonMessageAsync<T>(string url, object data, int timeOut = Config.TIME_OUT, ApiRequestMethod requestMethod = ApiRequestMethod.POST, IVerifyTenPaySign verifyTenPaySign = null)
        where T : JsonResult
        {
            T result = null;
            try
            {
                HttpResponseMessage responseMessage = await FetchResponseMessageAsync(url, data, timeOut, requestMethod);
                string content = await responseMessage.Content.ReadAsStringAsync();//TODO:如果不正确也要返回详情

#if DEBUG
                Console.WriteLine("Content:" + content + ",,Headers:" + responseMessage.Headers.ToString());
#endif

                //检查响应代码
                TenPayApiResultCode resutlCode = TenPayApiResultCode.TryGetCode(responseMessage.StatusCode, content);

                if (resutlCode.Success)
                {
                    //验证微信签名
                    var wechatpayTimestamp = responseMessage.Headers.GetValues("Wechatpay-Timestamp").First();
                    var wechatpayNonce = responseMessage.Headers.GetValues("Wechatpay-Nonce").First();
                    var wechatpaySignatureBase64 = responseMessage.Headers.GetValues("Wechatpay-Signature").First();//后续需要base64解码
                    var wechatpaySerial = responseMessage.Headers.GetValues("Wechatpay-Serial").First();

                    if (string.IsNullOrEmpty(content))
                    {
                        result = GetInstance<T>(true);
                    }
                    else
                    {
                        result = JsonSerializer.Deserialize<T>(content);
                    }

                    if (verifyTenPaySign != null)
                    {
                        try
                        {
                            var pubKey = await verifyTenPaySign.GetAPIv3PublicKeyAsync(this._tenpayV3Setting, wechatpaySerial);
                            result.VerifySignSuccess = TenPaySignHelper.VerifyTenpaySign(wechatpayTimestamp, wechatpayNonce, wechatpaySignatureBase64, content, pubKey);
                        }
                        catch (Exception ex)
                        {
                            throw new TenPayAPIException("RequestAsync 签名验证失败：" + ex.Message, ex);
                        }
                    }
                }
                else
                {
                    result = GetInstance<T>(true);
                    resutlCode.Additional = content;
                }
                //T result = resutlCode.Success ? (await responseMessage.Content.ReadAsStringAsync()).GetObject<T>() : new T();
                result.ResultCode = resutlCode;

                return result;
            }
            catch (Exception ex)
            {
                result = GetInstance<T>(false);
                if (result != null)
                {
                    result.ResultCode = new() { ErrorMessage = ex.Message };
                }

                return result;
            }
        }

        private T GetInstance<T>(bool throwIfFaild)
           where T : JsonResult
        {
            if (typeof(T).IsClass)
            {
                string path = typeof(T).FullName + "," + typeof(T).Assembly.GetName().Name;//命名空间.类型名,程序集
                Type o = Type.GetType(path);//加载类型
                object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                return (T)obj;//类型转换并返回
            }
            else if (throwIfFaild)
            {
                throw new TenPayAPIException("GetInstance 失败，此类型无法自动生成：" + typeof(T).FullName);
            }
            return null;
        }
    }
}