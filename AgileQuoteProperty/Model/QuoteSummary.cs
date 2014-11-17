using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class QuoteSummary
    {
        public QuoteBundleMaterialList summaryQuoteBundle { get; set; }        
        public QuoteBundleMaterialList summaryQuoteConfigureBundle { get; set; }
        public Quote summaryQuote { get; set; }
        public InstallCharges summaryQuoteInstallationCharges { get; set; }
        public List<QuoteBoughtOutItemModel> summaryQuoteBoughtoutItem { get; set; }
        public TotalUnitDiscount summaryQuoteBoughtoutItemTotal { get; set; }
        public QuoteBundleMaterialList summaryQuoteBundleInstall { get; set; }
        public List<QuoteBundleMaterial> summaryQuoteBundleWarrenty { get; set; }
        public List<QuoteShipping> summaryQuoteShipping { get; set; }
        public decimal summaryQuoteShippingTotalCost { get; set; }
        public decimal summaryQuotePrice { get; set; }
        public QuoteQualitativeInformation summaryQuoteQualitativeInformation { get; set; }
    }
}
