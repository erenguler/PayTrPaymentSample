using Newtonsoft.Json;

namespace PayTrPaymentSample
{
    public class PayTrPaymentResponseModel
    {
        /// <summary>
        /// Mağaza sipariş no: Satış işlemi için belirlediğiniz ve 1. ADIM’da gönderdiğiniz sipariş numarası
        /// </summary>
        [JsonProperty("merchant_oid")] public string MerchantOid { get; set; }

        /// <summary>
        /// Ödeme işleminin sonucu (success veya failed)
        /// </summary>
        [JsonProperty("status")] public string? Status { get; set; }

        /// <summary>
        /// Müşteriden tahsil edilen toplam tutar (100 ile çarpılmış hali gönderilir. 34.56 => 3456)
        /// Not: Müşteri vade farklı taksit seçtiği vb. durumlarda, 1. ADIM’da gönderdiğiniz "payment_amount" değerinden daha yüksek olabilir
        /// </summary>
        [JsonProperty("total_amount")] public string? TotalAmount { get; set; }

        /// <summary>
        /// PayTR sisteminden gönderilen değerlerin doğruluğunu kontrol etmeniz için güvenlik amaçlı oluşturulan hash değeri
        /// </summary>
        [JsonProperty("hash")] public string Hash { get; set; }

        /// <summary>
        /// Ödemenin onaylanmaması durumunda gönderilir (Bkz: 2. Adım İçin Hata Kodları ve Açıklamaları Tablosu)
        /// </summary>
        [JsonProperty("failed_reason_code")] public string? FailedReasonCode { get; set; }

        /// <summary>
        /// Ödemenin neden onaylanmadığı mesajını içerir (Bkz: 2. Adım İçin Hata Kodları ve Açıklamaları Tablosu)
        /// </summary>
        [JsonProperty("failed_reason_msg")] public string? FailedReasonMsg { get; set; }

        /// <summary>
        /// Mağazanız test modunda iken veya canlı modda yapılan test işlemlerde 1 olarak gönderilir
        /// </summary>
        [JsonProperty("test_mode")] public string? TestMode { get; set; }

        /// <summary>
        /// Ödeme şekli: Müşterinin hangi ödeme şekli ile ödemesini tamamladığını belirtir. 'card' veya 'eft' değerlerini alır.
        /// </summary>
        [JsonProperty("payment_type")] public string? PaymentType { get; set; }

        /// <summary>
        /// Para birimi: Ödemenin hangi para birimi üzerinden yapıldığını belirtir. ‘TL’, ‘USD’,‘EUR’, ‘GBP’, ‘RUB’ değerlerinden birini alır
        /// </summary>
        [JsonProperty("currency")] public string? Currency { get; set; }

        /// <summary>
        /// Sipariş tutarı: 1. ADIM’da gönderdiğiniz “payment_amount” değeridir. (100 ile çarpılmış hali gönderilir. 34.56 => 3456)
        /// </summary>
        [JsonProperty("payment_amount")] public string? PaymentAmount { get; set; }
    }

}
