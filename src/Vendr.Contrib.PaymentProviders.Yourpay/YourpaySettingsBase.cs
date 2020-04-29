using Vendr.Core.Web.PaymentProviders;

namespace Vendr.Contrib.PaymentProviders.Yourpay
{
    public class YourpaySettingsBase
    {
        [PaymentProviderSetting(Name = "Continue URL",
            Description = "The URL to continue to after this provider has done processing. eg: /continue/",
            SortOrder = 100)]
        public string ContinueUrl { get; set; }

        [PaymentProviderSetting(Name = "Cancel URL",
            Description = "The URL to return to if the payment attempt is canceled. eg: /cancel/",
            SortOrder = 200)]
        public string CancelUrl { get; set; }

        [PaymentProviderSetting(Name = "Error URL",
            Description = "The URL to return to if the payment attempt errors. eg: /error/",
            SortOrder = 300)]
        public string ErrorUrl { get; set; }

        [PaymentProviderSetting(Name = "Merchant ID",
            Description = "Merchant ID used for test payments.",
            SortOrder = 600)]
        public string MerchantId { get; set; }

        [PaymentProviderSetting(Name = "Production Merchant ID",
            Description = "Merchant ID used for real payments.",
            SortOrder = 700)]
        public string ProductionMerchantId { get; set; }

        [PaymentProviderSetting(Name = "Merchant Token",
            Description = "Merchant Token used in API.",
            SortOrder = 800)]
        public string MerchantToken { get; set; }

        [PaymentProviderSetting(Name = "Integration Key",
            Description = "Integration Key specified in Yourpay (optional).",
            SortOrder = 900)]
        public string IntegrationKey { get; set; }

        [PaymentProviderSetting(Name = "Language",
            Description = "Language in payment window.",
            SortOrder = 1000)]
        public string Language { get; set; }

        [PaymentProviderSetting(Name = "Test Mode",
            Description = "Set whether to process payments in test mode.",
            SortOrder = 10000)]
        public bool TestMode { get; set; }
    }
}
