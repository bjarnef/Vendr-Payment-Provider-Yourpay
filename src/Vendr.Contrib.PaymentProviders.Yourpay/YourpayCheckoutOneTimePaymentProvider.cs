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
    public class YourpayCheckoutOneTimePaymentProvider : YourpayPaymentProviderBase<YourpayCheckoutOneTimeSettings>
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

        public override ApiResult FetchPaymentStatus(OrderReadOnly order, YourpayCheckoutOneTimeSettings settings)
        {
            // Get payment: https://yourpay.docs.apiary.io/#/reference/0/payment-data/payment-data

            try
            {

                //return new ApiResult()
                //{
                //    TransactionInfo = new TransactionInfoUpdate()
                //    {
                //        TransactionId = GetTransactionId(payment),
                //        PaymentStatus = GetPaymentStatus(payment)
                //    }
                //};
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutOneTimePaymentProvider>(ex, "Yourpay - FetchPaymentStatus");
            }

            return ApiResult.Empty;
        }

        public override ApiResult CancelPayment(OrderReadOnly order, YourpayCheckoutOneTimeSettings settings)
        {
            // Release payment: https://yourpay.docs.apiary.io/#/reference/0/payment-release/payment-release

            try
            {

                //return new ApiResult()
                //{
                //    TransactionInfo = new TransactionInfoUpdate()
                //    {
                //        TransactionId = GetTransactionId(payment),
                //        PaymentStatus = GetPaymentStatus(payment)
                //    }
                //};
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutOneTimePaymentProvider>(ex, "Yourpay - CancelPayment");
            }

            return ApiResult.Empty;
        }

        public override ApiResult CapturePayment(OrderReadOnly order, YourpayCheckoutOneTimeSettings settings)
        {
            // Capture payment: https://yourpay.docs.apiary.io/#/reference/0/payment-actions/capture-payment

            try
            {
                //return new ApiResult()
                //{
                //    TransactionInfo = new TransactionInfoUpdate()
                //    {
                //        TransactionId = GetTransactionId(payment),
                //        PaymentStatus = GetPaymentStatus(payment)
                //    }
                //};
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutOneTimeSettings>(ex, "Yourpay - CapturePayment");
            }

            return ApiResult.Empty;
        }

        public override ApiResult RefundPayment(OrderReadOnly order, YourpayCheckoutOneTimeSettings settings)
        {
            // Refund payment: https://yourpay.docs.apiary.io/#/reference/0/payment-actions/refund-payment

            try
            {

                //return new ApiResult()
                //{
                //    TransactionInfo = new TransactionInfoUpdate()
                //    {
                //        TransactionId = GetTransactionId(refund),
                //        PaymentStatus = GetPaymentStatus(refund)
                //    }
                //};
            }
            catch (Exception ex)
            {
                Vendr.Log.Error<YourpayCheckoutOneTimePaymentProvider>(ex, "Yourpay - RefundPayment");
            }

            return ApiResult.Empty;
        }
    }
}
