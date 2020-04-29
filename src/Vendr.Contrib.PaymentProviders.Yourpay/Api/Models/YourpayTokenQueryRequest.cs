using Newtonsoft.Json;

namespace Vendr.Contrib.PaymentProviders.Yourpay.Api.Models
{
    public class YourpayTokenQueryRequest
    {
        [JsonProperty("MerchantNumber")]
        public string MerchantNumber { get; set; }

        [JsonProperty("ShopPlatform")]
        public string ShopPlatform { get; set; }
        
        //[JsonProperty("ccrg")]
        //public int CCRG { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("cartid")]
        public string CartId { get; set; }

        [JsonProperty("accepturl")]
        public string AcceptUrl { get; set; }

        [JsonProperty("callbackurl")]
        public string CallbackUrl { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("customername")]
        public string CustomerName { get; set; }

        //[JsonProperty("time")]
        //public string Time { get; set; }

        //[JsonProperty("use3ds")]
        //public int Use3ds { get; set; }

        [JsonProperty("autocapture")]
        public string AutoCapture { get; set; }
    }
}
