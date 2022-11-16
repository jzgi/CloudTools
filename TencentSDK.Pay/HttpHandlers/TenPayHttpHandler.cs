namespace TencentSDK.Pay
{
    public class TenPayHttpHandler : DelegatingHandler
    {
        private ITenPaySetting _tenPaySetting;

        public TenPayHttpHandler(ITenPaySetting tenPaySetting)
        {
             InnerHandler = new HttpClientHandler();
            _tenPaySetting = tenPaySetting;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = await BuildAuthAsync(request);
            string value = $"WECHATPAY2-SHA256-RSA2048 {auth}";
            request.Headers.Add("Authorization", value);

            return await base.SendAsync(request, cancellationToken);
        }


        /// <summary>
        /// 生成 Authorization 头
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<string> BuildAuthAsync(HttpRequestMessage request)
        {
            string method = request.Method.ToString();
            string body = "";
            if (method == "POST" || method == "PUT" || method == "PATCH")
            {
                var content = request.Content;
                body = await content.ReadAsStringAsync();
            }

            string uri = request.RequestUri.PathAndQuery;
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string nonce = Path.GetRandomFileName();

            string message = $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
            string signature = TenPaySignHelper.CreateSign(message, _tenPaySetting.TenPayV3_PrivateKey);

            return $"mchid=\"{_tenPaySetting.TenPayV3_MchId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{_tenPaySetting.TenPayV3_SerialNumber}\",signature=\"{signature}\"";
        }

    }
}