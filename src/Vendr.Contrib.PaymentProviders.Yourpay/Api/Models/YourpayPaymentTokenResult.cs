using Newtonsoft.Json;

namespace Vendr.Contrib.PaymentProviders.Yourpay.Api.Models
{
    public class YourpayPaymentTokenResult : YourpayPaymentResultBase
    {
        [JsonProperty("content")]
        public TokenContent Content { get; set; }
    }

    public class TokenContent
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("full_url")]
        public string FullUrl { get; set; }
    }
}
