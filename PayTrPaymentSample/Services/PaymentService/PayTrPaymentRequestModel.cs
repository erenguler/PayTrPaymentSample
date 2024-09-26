using Newtonsoft.Json;

namespace PayTrPaymentSample
{
    public class PayTrPaymentRequestModel
    {
        /// <summary>
        /// Mağaza no
        /// </summary>
        [JsonProperty("merchant_id")] public string MerchantId { get; set; }

        /// <summary>
        /// Müşteri ip
        /// </summary>
        [JsonProperty("user_ip ")] public string UserIp { get; set; }

        /// <summary>
        /// Sipariş numarası: Her işlemde benzersiz olmalıdır!! Bu bilgi bildirim sayfanıza yapılacak bildirimde geri gönderilir.
        /// </summary>
        [JsonProperty("merchant_oid")] public string MerchantOid { get; set; }

        /// <summary>
        /// Müşteri eposta adresi
        /// </summary>
        [JsonProperty("email ")] public string Email { get; set; }

        /// <summary>
        /// Ödeme tutarı - Tahsil edilecek tutar. 9.99 için 9.99 * 100 = 999 gönderilmelidir.
        /// </summary>
        [JsonProperty("payment_amount")] public string PaymentAmount { get; set; }

        /// <summary>
        /// Para birimi
        /// </summary>
        [JsonProperty("currency")] public string Currency { get; set; } = "TL";

        /// <summary>
        /// Sepet içeriği. örn:
        /// object[][] user_basket = {
        ///        new object[] {"Örnek ürün 1", "18.00", 1}, // 1. ürün (Ürün Ad - Birim Fiyat - Adet)
        ///            new object[] {"Örnek ürün 2", "33.25", 2}, // 2. ürün (Ürün Ad - Birim Fiyat - Adet)
        ///            new object[] { "Örnek ürün 3", "45.42", 1 }, // 3. ürün (Ürün Ad - Birim Fiyat - Adet)
        ///        };
        /// </summary>
        [JsonProperty("user_basket")] public Dictionary<string, int> UserBasket { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Taksit görüntülenmesin
        /// </summary>
        [JsonProperty("no_installment")] public int NoInstallment { get; set; }

        /// <summary>
        /// En fazla taksit sayısı
        /// </summary>
        [JsonProperty("max_installment")] public int MaxInstallment { get; set; }

        /// <summary>
        /// paytr_token
        /// </summary>
        [JsonProperty("paytr_token")] public string PaytrToken { get; set; }

        /// <summary>
        /// Müşteri adı ve soyadı
        /// </summary>
        [JsonProperty("user_name")] public string UserName { get; set; }

        /// <summary>
        /// Müşteri adresi
        /// </summary>
        [JsonProperty("user_address")] public string UserAddress { get; set; }

        /// <summary>
        /// Müşteri telefon numarası
        /// </summary>
        [JsonProperty("user_phone")] public string UserPhone { get; set; }

        /// <summary>
        /// Başarılı ödeme sonrası yönlendirilecek sayfa
        /// </summary>
        [JsonProperty("merchant_ok_url")] public string MerchantOkUrl { get; set; }

        /// <summary>
        /// Hata durumunda yönlendirilecek sayfa
        /// </summary>
        [JsonProperty("merchant_fail_url")] public string MerchantFailUrl { get; set; }

        /// <summary>
        /// Test mod
        /// </summary>
        [JsonProperty("test_mode")] public int? TestMode { get; set; }

        /// <summary>
        /// Hata döndür
        /// </summary>
        [JsonProperty("debug_on ")] public int? DebugOn { get; set; }

        /// <summary>
        /// Zaman aşımı limiti
        /// </summary>
        [JsonProperty("timeout_limit")] public int? TimeoutLimit { get; set; }

        /// <summary>
        /// Dil
        /// </summary>
        [JsonProperty("lang")] public string? Lang { get; set; } = "tr";
    }

}
