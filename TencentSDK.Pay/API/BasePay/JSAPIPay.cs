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

    }
}