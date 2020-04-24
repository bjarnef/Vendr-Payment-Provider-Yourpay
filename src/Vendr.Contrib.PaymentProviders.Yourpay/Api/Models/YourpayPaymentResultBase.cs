using Newtonsoft.Json;

namespace Vendr.Contrib.PaymentProviders.Yourpay.Api.Models
{
    public class YourpayPaymentResultBase
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
