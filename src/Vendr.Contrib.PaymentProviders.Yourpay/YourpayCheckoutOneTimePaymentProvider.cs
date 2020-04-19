using System;
using System.Web;
using System.Web.Mvc;
using Vendr.Core;
using Vendr.Core.Models;
using Vendr.Core.Web.Api;
using Vendr.Core.Web.PaymentProviders;

namespace Vendr.Contrib.PaymentProviders.Yourpay
{
    [PaymentProvider("yourpay-checkout-onetime", "Yourpay (One Time)", "Yourpay payment provider for one time payments")]
    public class YourpayCheckoutOneTimePaymentProvider : PaymentProviderBase<YourpayCheckoutOneTimeSettings>
    {
        public YourpayCheckoutOneTimePaymentProvider(VendrContext vendr)
            : base(vendr)
        { }

        public override bool FinalizeAtContinueUrl => true;

        public override PaymentFormResult GenerateForm(OrderReadOnly order, string continueUrl, string cancelUrl, string callbackUrl, YourpayCheckoutOneTimeSettings settings)
        {
            return new PaymentFormResult()
            {
                Form = new PaymentForm(continueUrl, FormMethod.Post)
            };
        }

        public override string GetCancelUrl(OrderReadOnly order, YourpayCheckoutOneTimeSettings settings)
        {
            return string.Empty;
        }

        public override string GetErrorUrl(OrderReadOnly order, YourpayCheckoutOneTimeSettings settings)
        {
            return string.Empty;
        }

        public override string GetContinueUrl(OrderReadOnly order, YourpayCheckoutOneTimeSettings settings)
        {
            settings.MustNotBeNull("settings");
            settings.ContinueUrl.MustNotBeNull("settings.ContinueUrl");

            return settings.ContinueUrl;
        }

        public override CallbackResult ProcessCallback(OrderReadOnly order, HttpRequestBase request, YourpayCheckoutOneTimeSettings settings)
        {
            return new CallbackResult
            {
                TransactionInfo = new TransactionInfo
                {
                    AmountAuthorized = order.TotalPrice.Value.WithTax,
                    TransactionFee = 0m,
                    TransactionId = Guid.NewGuid().ToString("N"),
                    PaymentStatus = PaymentStatus.Authorized
                }
            };
        }
    }
}
