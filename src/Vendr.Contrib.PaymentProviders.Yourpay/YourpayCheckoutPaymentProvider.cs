using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Vendr.Contrib.PaymentProviders.Yourpay.Api;
using Vendr.Contrib.PaymentProviders.Yourpay.Api.Models;
using Vendr.Core;
using Vendr.Core.Models;
using Vendr.Core.Web.Api;
using Vendr.Core.Web.PaymentProviders;

namespace Vendr.Contrib.PaymentProviders.Yourpay
{
    [PaymentProvider("yourpay-checkout", "Yourpay Checkout", "Yourpay payment provider for one time payments")]
    public class YourpayCheckoutPaymentProvider : YourpayPaymentProviderBase<YourpayCheckoutSettings>
    {
        public YourpayCheckoutPaymentProvider(VendrContext vendr)
            : base(vendr)
        { }

        public override bool CanCancelPayments => true;
        public override bool CanCapturePayments => true;
        public override bool CanRefundPayments => true;
        public override bool CanFetchPaymentStatus => true;

        public override bool FinalizeAtContinueUrl => true;

        public override IEnumerable<TransactionMetaDataDefinition> TransactionMetaDataDefinitions => new[]{
            new TransactionMetaDataDefinition("yourpayPaymentId", "Yourpay Payment ID"),
            new TransactionMetaDataDefinition("yourpayPaymentToken", "Yourpay Payment Token")
        };

        public override PaymentFormResult GenerateForm(OrderReadOnly order, string continueUrl, string cancelUrl, string callbackUrl, YourpayCheckoutSettings settings)
        {
            var currency = Vendr.Services.CurrencyService.GetCurrency(order.CurrencyId);
            var currencyCode = currency.Code.ToUpperInvariant();

            // Ensure currency has valid ISO 4217 code
            if (!Iso4217.CurrencyCodes.ContainsKey(currencyCode))
            {
                throw new Exception("Currency must be a valid ISO 4217 currency code: " + currency.Name);
            }

            var orderAmount = AmountToMinorUnits(order.TotalPrice.Value.WithTax);

            var paymentToken = order.Properties["yourpayPaymentToken"]?.Value ?? null;
            string paymentFormLink = string.Empty;

            bool autoCapture = settings.AutoCapture;

            try
            {
                var clientConfig = GetYourpayClientConfig(settings);
                var client = new YourpayClient(clientConfig);

                var merchantId = settings.TestMode ? settings.MerchantId : settings.ProductionMerchantId;

                var obj = new YourpayTokenQueryRequest
                {
                    MerchantNumber = merchantId,
                    //ShopPlatform = "Vendr",
                    Amount = Convert.ToInt32(orderAmount),
                    Currency = currencyCode,
                    CartId = order.OrderNumber,
                    AcceptUrl = continueUrl,
                    CallbackUrl = callbackUrl,
                    CustomerName = $"{order.CustomerInfo.FirstName} {order.CustomerInfo.LastName}",
                    AutoCapture = autoCapture ? "1" : "0"
                };

                if (!string.IsNullOrWhiteSpace(settings.Language))
                {
                    obj.Language = settings.Language;
                }

                var json = JsonConvert.SerializeObject(obj);
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                // Generate token
                var payment = client.GenerateToken(data);

                if (payment.Content != null)
                {
                    paymentToken = payment.Content.Token;
                    paymentFormLink = payment.Content.FullUrl;
                }
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutPaymentProvider>(ex, "Yourpay - error creating payment.");
            }

            return new PaymentFormResult()
            {
                MetaData = new Dictionary<string, string>
                {
                    { "yourpayPaymentToken", paymentToken },
                    { "yourpayAutoCapture", autoCapture.ToString() }
                },
                Form = new PaymentForm(paymentFormLink, FormMethod.Get)
            };
        }

        public override CallbackResult ProcessCallback(OrderReadOnly order, HttpRequestBase request, YourpayCheckoutSettings settings)
        {
            try
            {
                // Callback response: https://www.yourpay.eu/support/hosted-payment-window/

                var uxtime = request["uxtime"];
                var merchantNumber = request["MerchantNumber"];
                var transactionId = request["tid"];
                var tchecksum = request["tchecksum"];
                var checksum = request["checksum"];
                var orderId = request["orderid"];
                var strFee = request["transfee"] ?? "0";
                var strAmount = request["amount"];
                var cvc = request["cvc"];
                var expmonth = request["expmonth"];
                var expyear = request["expyear"];
                var tcardno = request["tcardno"];
                var time = request["time"];
                var cardId = request["cardid"];
                var currencyCode = request["currency"];
                var autoCapture = request["autocapture"];

                bool captured = autoCapture == "1" && bool.TryParse(order.Properties["yourpayAutoCapture"]?.Value, out bool c) ? c : false;

                var totalAmount = decimal.Parse(strAmount, CultureInfo.InvariantCulture) + decimal.Parse(strFee, CultureInfo.InvariantCulture);

                var clientConfig = GetYourpayClientConfig(settings);
                var client = new YourpayClient(clientConfig);

                var result = client.GetPaymentData(transactionId);
                
                if (result.Success)
                {
                    var paymentId = result.Content.Id.ToString();

                    // Verify: Checksum = SHA1 encode of PaymentID + Integrationkey.
                    if (checksum == SHA1Hash(paymentId + settings.IntegrationKey))
                    {
                        return new CallbackResult
                        {
                            MetaData = new Dictionary<string, string>
                            {
                                { "yourpayPaymentId", paymentId }
                            },
                            TransactionInfo = new TransactionInfo
                            {
                                AmountAuthorized = AmountFromMinorUnits((long)totalAmount),
                                TransactionId = transactionId,
                                PaymentStatus = !captured ? PaymentStatus.Authorized : PaymentStatus.Captured
                            }
                        };
                    }
                    else
                    {
                        Vendr.Log.Warn<YourpayCheckoutPaymentProvider>($"Yourpay [{order.OrderNumber}] - checksum security check failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutPaymentProvider>(ex, "Yourpay - ProcessCallback");
            }

            return CallbackResult.Empty;
        }

        public override ApiResult FetchPaymentStatus(OrderReadOnly order, YourpayCheckoutSettings settings)
        {
            // Get payment: https://yourpay.docs.apiary.io/#/reference/0/payment-data/payment-data

            try
            {
                var clientConfig = GetYourpayClientConfig(settings);
                var client = new YourpayClient(clientConfig);

                var paymentStatus = order.TransactionInfo.PaymentStatus;
                var transactionId = order.TransactionInfo.TransactionId;
                var result = client.GetPaymentData(transactionId);

                if (result != null && result.Success)
                {
                    return new ApiResult()
                    {
                        TransactionInfo = new TransactionInfoUpdate()
                        {
                            TransactionId = transactionId,
                            PaymentStatus = paymentStatus ?? PaymentStatus.Authorized
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutPaymentProvider>(ex, "Yourpay - FetchPaymentStatus");
            }

            return ApiResult.Empty;
        }

        public override ApiResult CancelPayment(OrderReadOnly order, YourpayCheckoutSettings settings)
        {
            // Release payment: https://yourpay.docs.apiary.io/#/reference/0/payment-release/payment-release

            try
            {
                var clientConfig = GetYourpayClientConfig(settings);
                var client = new YourpayClient(clientConfig);

                var transactionId = order.TransactionInfo.TransactionId;
                var result = client.ReleasePayment(transactionId);

                if (result != null && result.Success)
                {
                    return new ApiResult()
                    {
                        TransactionInfo = new TransactionInfoUpdate()
                        {
                            TransactionId = transactionId,
                            PaymentStatus = PaymentStatus.Cancelled
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutPaymentProvider>(ex, "Yourpay - CancelPayment");
            }

            return ApiResult.Empty;
        }

        public override ApiResult CapturePayment(OrderReadOnly order, YourpayCheckoutSettings settings)
        {
            // Capture payment: https://yourpay.docs.apiary.io/#/reference/0/payment-actions/capture-payment

            try
            {
                var clientConfig = GetYourpayClientConfig(settings);
                var client = new YourpayClient(clientConfig);

                var transactionId = order.TransactionInfo.TransactionId;
                var amount = AmountToMinorUnits(order.TransactionInfo.AmountAuthorized.Value);

                var result = client.CapturePayment(transactionId, amount);

                if (result != null && result.Success)
                {
                    return new ApiResult()
                    {
                        TransactionInfo = new TransactionInfoUpdate()
                        {
                            TransactionId = transactionId,
                            PaymentStatus = PaymentStatus.Captured
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutPaymentProvider>(ex, "Yourpay - CapturePayment");
            }

            return ApiResult.Empty;
        }

        public override ApiResult RefundPayment(OrderReadOnly order, YourpayCheckoutSettings settings)
        {
            // Refund payment: https://yourpay.docs.apiary.io/#/reference/0/payment-actions/refund-payment

            try
            {
                var clientConfig = GetYourpayClientConfig(settings);
                var client = new YourpayClient(clientConfig);

                var transactionId = order.TransactionInfo.TransactionId;
                var amount = -Math.Abs(AmountToMinorUnits(order.TransactionInfo.AmountAuthorized.Value));

                var result = client.RefundPayment(transactionId, amount);

                if (result != null && result.Success)
                {
                    return new ApiResult()
                    {
                        TransactionInfo = new TransactionInfoUpdate()
                        {
                            TransactionId = transactionId,
                            PaymentStatus = PaymentStatus.Refunded
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutPaymentProvider>(ex, "Yourpay - RefundPayment");
            }

            return ApiResult.Empty;
        }
    }
}
