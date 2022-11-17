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
        public async Task<JsApiReturnJson> JsAPIAsync(TransactionsRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = Config.TenPayV3Host + "/v3/pay/transactions/jsapi";
            TenPayAPIRequest tenPayApiRequest = new(_tenPaySetting);
            return await tenPayApiRequest.FetchJsonMessageAsync<JsApiReturnJson>(url, data, timeOut, verifyTenPaySign: new VerifyTenPaySign());
        }

        // TODO: 待测试
        /// <summary>
        /// 微信支付订单号查询
        /// </summary>
        /// <param name="transaction_id"> 微信支付系统生成的订单号 示例值：1217752501201407033233368018</param>
        /// <param name="mchid">直连商户的商户号，由微信支付生成并下发。 示例值：1230000109</param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<OrderReturnJson> OrderQueryByTransactionIdAsync(string transaction_id, string mchid, int timeOut = Config.TIME_OUT)
        {
            try
            {
                var url = Config.TenPayV3Host + $"/v3/pay/transactions/id/{transaction_id}?mchid={mchid}";
                TenPayAPIRequest tenPayApiRequest = new(_tenPaySetting);
                return await tenPayApiRequest.FetchJsonMessageAsync<OrderReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
            }
            catch (Exception ex)
            {
                return new OrderReturnJson() { ResultCode = new TenPayApiResultCode() { ErrorMessage = ex.Message } };
            }
        }
        /// <summary>
        /// 商户订单号查询
        /// <para>https://pay.weixin.qq.com/wiki/doc/apiv3_partner/apis/chapter4_1_2.shtml</para>
        /// </summary>
        /// <param name="out_trade_no"> 微信支付系统生成的订单号 示例值：1217752501201407033233368018</param>
        /// <param name="mchid">直连商户的商户号，由微信支付生成并下发。 示例值：1230000109</param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<OrderReturnJson> OrderQueryByOutTradeNoAsync(string out_trade_no, string mchid, int timeOut = Config.TIME_OUT)
        {
            try
            {
                var url = $"{Config.TenPayV3Host}/v3/pay/transactions/out-trade-no/{out_trade_no}?mchid={mchid}";
                TenPayAPIRequest tenPayApiRequest = new(_tenPaySetting);
                return await tenPayApiRequest.FetchJsonMessageAsync<OrderReturnJson>(url, null, timeOut, ApiRequestMethod.GET, verifyTenPaySign: new VerifyTenPaySign());
            }
            catch (Exception ex)
            {
                return new OrderReturnJson() { ResultCode = new TenPayApiResultCode() { ErrorMessage = ex.Message } };
            }
        }

        /// <summary>
        /// 关闭订单接口
        /// </summary>
        /// <param name="out_trade_no">商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一 示例值：1217752501201407033233368018</param>
        /// <param name="mchid">直连商户的商户号，由微信支付生成并下发。 示例值：1230000109</param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<JsonResult> CloseOrderAsync(string out_trade_no, string mchid, int timeOut = Config.TIME_OUT)
        {
            try
            {
                var url = $"{Config.TenPayV3Host}/v3/pay/transactions/out-trade-no/{out_trade_no}/close";
                TenPayAPIRequest tenPayApiRequest = new(_tenPaySetting);
                var data = new
                {
                    mchid = mchid
                };
                return await tenPayApiRequest.FetchJsonMessageAsync<JsonResult>(url, data, timeOut);
            }
            catch (Exception ex)
            {
                return new JsonResult() { ResultCode = new TenPayApiResultCode() { ErrorMessage = ex.Message } };
            }
        }
    }
}