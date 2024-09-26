using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PayTrPaymentSample
{
    public class PayTrPaymentService
    {
        private readonly ILogger<PayTrPaymentService> _logger;
        private readonly PayTrSettings _settings;

        public PayTrPaymentService(ILogger<PayTrPaymentService> logger, IOptions<PayTrSettings> options)
        {
            _settings = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// PayTr'den IFrame url'i ister 
        /// </summary>
        /// <param name="model">İstek için gerekli bilgiler modeli</param>
        /// <returns> IFrame url'i </returns>
        public async Task<string> GetIFrameUrl(PayTrPaymentRequestModel model)
        {
            NameValueCollection data = new NameValueCollection();

            // fiyat bilgilerini ayarla
            var amount = Convert.ToDouble(model.PaymentAmount).ToPrice();
            var amountString = (amount * 100).ToString();

            data["merchant_id"] = _settings.MerchantId;
            data["user_ip"] = model.UserIp;
            data["merchant_oid"] = model.MerchantOid;
            data["email"] = model.Email;
            data["payment_amount"] = amountString;
            data["currency"] = model.Currency;
            data["no_installment"] = model.NoInstallment.ToString();
            data["max_installment"] = model.MaxInstallment.ToString();
            data["user_name"] = model.UserName;
            data["user_address"] = model.UserAddress;
            data["user_phone"] = model.UserAddress;
            data["merchant_ok_url"] = model.MerchantOkUrl;
            data["merchant_fail_url"] = model.MerchantFailUrl;
            data["test_mode"] = model.TestMode.ToString();
            data["debug_on"] = model.DebugOn.ToString();
            data["timeout_limit"] = model.TimeoutLimit.ToString();
            data["lang"] = model.Lang;

            // Sepet içerği oluşturma fonksiyonu, değiştirilmeden kullanılabilir.
            object[][] user_basket = new object[model.UserBasket.Count][];
            int index = 0;
            foreach (var item in model.UserBasket)
            {
                user_basket[index] = new object[] { item.Key, "amount", item.Value };
                index++;
            }

            string user_basket_json = JsonConvert.SerializeObject(user_basket);
            string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));
            data["user_basket"] = user_basketstr;

            // Token oluşturma fonksiyonu, değiştirilmeden kullanılmalıdır.
            string concat = string.Concat(
                _settings.MerchantId
                , model.UserIp
                , model.MerchantOid
                , model.Email
                , amountString
                , user_basketstr
                , model.NoInstallment.ToString()
                , model.MaxInstallment.ToString()
                , model.Currency
                , model.TestMode.ToString()
                , _settings.MerchantSalt
            );
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.MerchantKey));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(concat));
            data["paytr_token"] = Convert.ToBase64String(b);

            var iframeUrl = string.Empty;

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] result = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                string ResultAuthTicket = Encoding.UTF8.GetString(result);
                dynamic json = JToken.Parse(ResultAuthTicket);

                if (json.status == "success")
                {
                    iframeUrl = "https://www.paytr.com/odeme/guvenli/" + json.token;
                }
                else
                {
                    _logger.LogError("PAYTR iframe failed", (object)json.reason);
                    iframeUrl = null;
                }
            }

            return iframeUrl;
        }

        public bool ValidatePayTrRequest(PayTrPaymentResponseModel response)
        {
            // POST değerleri ile hash oluştur.
            string concat = string.Concat(response.MerchantOid, _settings.MerchantSalt, response.Status, response.TotalAmount);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.MerchantKey));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(concat));
            string token = Convert.ToBase64String(b);

            if (response.Hash.ToString() != token)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }

}
