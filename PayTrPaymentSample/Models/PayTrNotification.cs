namespace PayTrPaymentSample.Models
{
    public class PayTrNotification
    {
        public string MerchantOid { get; set; }

        public string? Status { get; set; }

        public string? TotalAmount { get; set; }

        public string? Hash { get; set; }

        public string? FailedReasonCode { get; set; }

        public string? FailedReasonMsg { get; set; }

        public string? TestMode { get; set; }

        public string? PaymentType { get; set; }

        public string? Currency { get; set; }

        public string? PaymentAmount { get; set; }

        public string? Result { get; set; }

        public bool IsValidated { get; set; }
    }
}
