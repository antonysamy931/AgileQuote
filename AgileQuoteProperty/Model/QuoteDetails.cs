using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class QuoteList
    {
        public List<QuoteDetails> QuoteLists { get; set; }
    }

    public class QuoteDetails
    {
        [Display(Name="Quote Number")]        
        public int qQuoteID { get; set; }     
           
        [Display(Name="Customer Name")]
        public string qQuoteName { get; set; }

        [Display(Name="Sales Organization Code")]
        public string qSalesOrgCode { get; set; }

        /*Sales organization*/
        [Display(Name="Sales Organization Name")]
        public string qSalesOrganization { get; set; }

        public List<SalesOrganizations> SalesOrg { get; set; }
        
        /*Currency*/
        [Display(Name="Currency")]
        public string currencyName { get; set; }

        public List<CurrencyValues> Currency { get; set; }

        /*Status*/
        [Display(Name="Status")]
        public string Status { get; set; }

        public List<string> StatusList { get; set; }

        [Display(Name="Budget Rate")]
        public decimal BudgetValue { get; set; }

        [Display(Name="Customer Name")]
        public string qCustomerName { get; set; }

        [Display(Name="Prepared By")]
        public string qPrepareBy { get; set; }

        [Display(Name="Created Date")]
        public DateTime? qCreatDate { get; set; }

        [Display(Name="Quote Requested Date")]
        public DateTime? qRequireDate { get; set; }

        [Display(Name="Sales Organization Code")]
        public string CustomerCode { get; set; }

        public string displayCreateDate { get; set; }
        public string displayRequireDate { get; set; }

        public string AccessType { get; set; }

        public int qLevel { get; set; }
    }
}
