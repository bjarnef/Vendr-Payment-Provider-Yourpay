using Newtonsoft.Json;

namespace Vendr.Contrib.PaymentProviders.Yourpay.Api.Models
{
    public class YourpayPayment
    {
        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("amount_text")]
        public string AmountText { get; set; }

        [JsonProperty("time_created")]
        public long TimeCreated { get; set; }
    }
}
