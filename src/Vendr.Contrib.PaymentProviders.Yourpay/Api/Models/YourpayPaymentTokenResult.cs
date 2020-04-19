using Newtonsoft.Json;

namespace Vendr.Contrib.PaymentProviders.Yourpay.Api.Models
{
    public class YourpayPaymentTokenResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("content")]
        public Content Content { get; set; }
    }

    public class Content
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("full_url")]
        public string FullUrl { get; set; }
    }
}
