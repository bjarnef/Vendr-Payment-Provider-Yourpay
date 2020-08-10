using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Vendr.Contrib.PaymentProviders.Yourpay.Api.Models;

namespace Vendr.Contrib.PaymentProviders.Yourpay.Api
{
    public class YourpayClient
    {
        private YourpayClientConfig _config;

        public YourpayClient(YourpayClientConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Generate token for payment window
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public YourpayPaymentTokenResult GenerateToken(object data)
        {
            return Request($"/v4.3/generate_token", (req) => req
                .SetQueryParams(data)
                .GetJsonAsync<YourpayPaymentTokenResult>());
        }

        /// <summary>
        /// Get payment data
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <returns></returns>
        public YourpayPaymentData GetPaymentData(string id)
        {
            return Request($"/v4.3/payment_data?id={id}", (req) => req
                .GetJsonAsync<YourpayPaymentData>());
        }

        /// <summary>
        /// Release payment
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <returns></returns>
        public YourpayPaymentResultBase ReleasePayment(string id)
        {
            return Request($"/v4.3/payment_release?id={id}", (req) => req
                .GetJsonAsync<YourpayPaymentResultBase>());
        }

        /// <summary>
        /// Capture payment
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <param name="amount">Amount to capture</param>
        /// <returns></returns>
        public YourpayPaymentData CapturePayment(string id, decimal amount)
        {
            return Request($"/v4.3/payment_action?id={id}&amount={amount}", (req) => req
                .GetJsonAsync<YourpayPaymentData>());
        }

        /// <summary>
        /// Refund payment
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <param name="amount">Negative amount for refund</param>
        /// <returns></returns>
        public YourpayPaymentData RefundPayment(string id, decimal amount)
        {
            return Request($"/v4.3/payment_action?id={id}&amount={amount}", (req) => req
                .GetJsonAsync<YourpayPaymentData>());
        }

        private TResult Request<TResult>(string url, Func<IFlurlRequest, Task<TResult>> func)
        {
            var result = default(TResult);

            try
            {
                var req = new FlurlRequest(_config.BaseUrl + url)
                        .ConfigureRequest(x =>
                        {
                            var jsonSettings = new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                DefaultValueHandling = DefaultValueHandling.Include,
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            };
                            x.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
                        }).
                        SetQueryParam("merchant_token", _config.Token);

                result = func.Invoke(req).Result;
            }
            catch (FlurlHttpException ex)
            {
                throw;
            }

            return result;
        }
    }
}