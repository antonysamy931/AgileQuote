using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class QuoteQualitativeInformation
    {

        public string txtQuoteValue { get; set; }
        public decimal txtGrossMarginAmount { get; set; }
        public decimal txtPercentage { get; set; }
        public string txtMagrinAmountPercentage { get; set; }
        public string txtLeadTime { get; set; }
        public string txtWinProbability { get; set; }
        public string txtareaScopeofWork { get; set; }
        public string txtareaExecutiveSummary { get; set; }
        public string txtareaPrimaryCompetitor { get; set; }
        public string txtareaSellingPrice { get; set; }
        public string txtareaPaymentTerms { get; set; }
        public string txtareaRiskMitigation { get; set; }        
        public string txtareaAnyOtherComments { get; set; }

        public string Business {get;set;}
        public string defaultBusiness {get;set;}
        public List<string> BusinessList { get; set; }

    }

    public class QuoteQualitativeInformationValues
    {
        public string txtQuoteTotalValue { get; set; }
        public string txtMarginAmountPercentageValue { get; set; }
    }
}
