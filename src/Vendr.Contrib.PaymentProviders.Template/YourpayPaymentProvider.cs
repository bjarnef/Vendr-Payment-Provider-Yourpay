using System;
using System.Web;
using System.Web.Mvc;
using Vendr.Core;
using Vendr.Core.Models;
using Vendr.Core.Web.Api;
using Vendr.Core.Web.PaymentProviders;

namespace Vendr.Contrib.PaymentProviders
{
    [PaymentProvider("yourpay", "Yourpay", "Yourpay payment provider")]
    public class YourpayPaymentProvider : PaymentProviderBase<YourpaySettings>
    {
        public YourpayPaymentProvider(VendrContext vendr)
            : base(vendr)
        { }

        public override bool FinalizeAtContinueUrl => true;

        public override PaymentFormResult GenerateForm(OrderReadOnly order, string continueUrl, string cancelUrl, string callbackUrl, YourpaySettings settings)
        {
            return new PaymentFormResult()
            {
                Form = new PaymentForm(continueUrl, FormMethod.Post)
            };
        }

        public override string GetCancelUrl(OrderReadOnly order, YourpaySettings settings)
        {
            return string.Empty;
        }

        public override string GetErrorUrl(OrderReadOnly order, YourpaySettings settings)
        {
            return string.Empty;
        }

        public override string GetContinueUrl(OrderReadOnly order, YourpaySettings settings)
        {
            settings.MustNotBeNull("settings");
            settings.ContinueUrl.MustNotBeNull("settings.ContinueUrl");

            return settings.ContinueUrl;
        }

        public override CallbackResult ProcessCallback(OrderReadOnly order, HttpRequestBase request, YourpaySettings settings)
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
