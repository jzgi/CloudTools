namespace TencentSDK.Pay
{
    public class JSAPIPay
    {
        private ITenPaySetting _tenPaySetting;


        public JSAPIPay(ITenPaySetting tenPaySetting)
        {
            _tenPaySetting = tenPaySetting;
        }
        public async Task<CertificatesJsonResult> CertificatesAsync(int timeOut = Config.TIME_OUT)
        {
            var url = Config.TenPayV3Host + "/v3/certificates";
            TenPayAPIRequest tenPayApiRequest = new(_tenPaySetting);
            return await tenPayApiRequest.FetchJsonMessageAsync<CertificatesJsonResult>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 获取微信支付证书的公钥
        /// </summary>
        /// <returns>证书集合类</returns>
        public async Task<PublicKeyCollection> GetPublicKeysAsync()
        {
            var certificates = await CertificatesAsync();
            if (!certificates.ResultCode.Success)
            {
                throw new TenPayAPIException("获取证书公钥失败：" + certificates.ResultCode.ErrorMessage);
            }

            if (certificates.data?.Length == 0)
            {
                throw new TenPayAPIException("Certificates 获取结果为空");
            }

            PublicKeyCollection keys = new();

            foreach (var cert in certificates.data)
            {
                var publicKey = SecurityHelper.AesGcmDecryptCiphertext(_tenPaySetting.TenPayV3_APIv3Key, cert.encrypt_certificate.nonce,
                                    cert.encrypt_certificate.associated_data, cert.encrypt_certificate.ciphertext);
                keys[cert.serial_no] = SecurityHelper.GetUnwrapCertKey(publicKey);
            }
            return keys;
        }
        /// <summary>
        /// JSAPI下单接口
        /// <para>在微信支付服务后台生成JSAPI预支付交易单，返回预支付交易会话标识</para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<JsApiReturnJson> JsApiAsync(TransactionsRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = Config.TenPayV3Host + "/v3/pay/transactions/jsapi";
            TenPayAPIRequest tenPayApiRequest = new(_tenPaySetting);
            return await tenPayApiRequest.FetchJsonMessageAsync<JsApiReturnJson>(url, data, timeOut,verifyTenPaySign:new VerifyTenPaySign());
        }

    }
}