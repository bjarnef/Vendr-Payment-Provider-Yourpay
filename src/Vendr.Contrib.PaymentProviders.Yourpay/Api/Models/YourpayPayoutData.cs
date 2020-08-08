using Newtonsoft.Json;

namespace Vendr.Contrib.PaymentProviders.Yourpay.Api.Models
{
    public class YourpayPayoutData
    {
        [JsonProperty("action_id")]
        public int ActionId { get; set; }

        [JsonProperty("action_type")]
        public string ActionType { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("splitted_days_one")]
        public int SplittedDaysOne { get; set; }

        [JsonProperty("splitted_days_two")]
        public int SplittedDaysTwo { get; set; }

        [JsonProperty("splitted_days_id")]
        public int SplittedDaysId { get; set; }
    }
}
