using Newtonsoft.Json;
namespace Vendr.Contrib.PaymentProviders.Yourpay.Api.Models
{
    public class PaymentData : PaymentResultBase
    {
        [JsonProperty("content")]
        public PaymentContent Content { get; set; }
    }

    public class PaymentContent
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("transaction_id")]
        public int TransactionId { get; set; }

        [JsonProperty("merchant_id")]
        public int MerchantId { get; set; }

        [JsonProperty("merchant")]
        public string Merchant { get; set; }

        [JsonProperty("test")]
        public bool Test { get; set; }

        [JsonProperty("fee")]
        public int Fee { get; set; }

        [JsonProperty("fee_text")]
        public string FeeText { get; set; }

        [JsonProperty("card_type")]
        public int CardType { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("card_holder")]
        public string CardHolder { get; set; }

        [JsonProperty("card_country")]
        public string CardCountry { get; set; }

        [JsonProperty("secure")]
        public bool Secure { get; set; }

        [JsonProperty("ccrg")]
        public string CCRG{ get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("payments")]
        public object[] Payments { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("amount_text")]
        public string AmountText { get; set; }

        [JsonProperty("amount_captured")]
        public int AmountCaptured { get; set; }

        [JsonProperty("amount_captured_text")]
        public string AmountCapturedText { get; set; }

        [JsonProperty("amount_refunded")]
        public int AmountRefunded { get; set; }

        [JsonProperty("amount_refunded_text")]
        public string AmountRefundedText { get; set; }

        [JsonProperty("flag_mass_capture")]
        public bool FlagMassCapture { get; set; }

        [JsonProperty("flag_mass_release")]
        public bool FlagMassRelease { get; set; }

        [JsonProperty("flag_mass_refund")]
        public bool FlagMassRefund { get; set; }

        [JsonProperty("time_created")]
        public long TimeCreated { get; set; }

        [JsonProperty("time_captured")]
        public long TimeCaptured { get; set; }

        [JsonProperty("time_refunded")]
        public long TimeRefunded { get; set; }

        [JsonProperty("time_released")]
        public long TimeReleased { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("consumer_data")]
        public object[] ConsumerData { get; set; }
    }
}