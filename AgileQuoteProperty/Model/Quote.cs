using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using AgileQuoteProperty.Validation;

namespace AgileQuoteProperty.Model
{
    public class Quote
    {
        /*Common*/
        public int CreatedBy { get; set; }
        public int QuoteID { get; set; }

        /*Quote Creation*/
        //[Required(ErrorMessage="*")]
        [Required(ErrorMessage = "Please Enter Quote Name")]
        [Display(Name = "Quote Name")]
        public string QuoteName { get; set; }

        [Required(ErrorMessage = "Please Enter Sales Organization Code")]
        //[Required(ErrorMessage="*")]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Please Select Sales Organization Name")]
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Sales Organization Name")]
        public string salesOrgName { get; set; }

        [Required(ErrorMessage = "Please Enter Customer Name")]
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        public string defaultSalesOrg { get; set; }

        public List<SalesOrganizations> SalesOrg { get; set; }

        [Required(ErrorMessage = "Please Select Currency")]
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Currency Name")]
        public string currencyName { get; set; }

        public string defaultCurrency { get; set; }

        public List<CurrencyValues> Currency { get; set; }

        [Required(ErrorMessage = "Please Select Quote Requested Date ")]
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Requested Date")]
        public string RequrestedDate { get; set; }

        [Required(ErrorMessage = "Please Enter Prepared")]
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        //[Required(ErrorMessage = "Please Select Status")]
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Quote Status")]
        public string Status { get; set; }

        public string defaultStatus { get; set; }

        public List<string> StatusList { get; set; }

        [Required(ErrorMessage = "Please Enter  Budget Rate Target ")]
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Budget Value")]
        public decimal BudgetValue { get; set; }

        public string CreateDate { get; set; }

        /*Billing Address*/
        [Display(Name = "Address Line One")]
        [Required(ErrorMessage = "Please Enter Address Line One")]
        //[Required(ErrorMessage = "*")]
        public string BillingAddressOne { get; set; }

        [Display(Name = "Address Line Two")]
        public string BillingAddressTwo { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please Enter City")]
        //[Required(ErrorMessage = "*")]
        public string BillingCity { get; set; }

        [Display(Name = "State")]
        [RequiredIf("BillingCountry", "USA", ErrorMessage = "Please Enter State")]
        //[Required(ErrorMessage="Please Enter State")]
        //[Required(ErrorMessage = "*")]
        public string BillingState { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please Enter Country")]
        //[Required(ErrorMessage = "*")]
        public string BillingCountry { get; set; }

        [Display(Name = "Zip Code")]
        //[RegularExpression(@"^(\d{6})$", ErrorMessage = "Please Enter valid postal number.")]
        [Required(ErrorMessage = "Please Enter Zip Code")]
        //[Required(ErrorMessage = "*")]
        public string BillingPostalCode { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Please Enter Phone Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter Valid Phone Number")]
        //[Required(ErrorMessage = "*")]
        public string BillingPhoneNumber { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^(\d{10})$", ErrorMessage = "Please Enter Valid Mobile Number")]
        //[Required(ErrorMessage="Please Enter Mobile Number")]
        //[Required(ErrorMessage = "*")]
        public string BillingMobileNumber { get; set; }

        /*Shiping Address*/
        [Display(Name = "Address Line One")]
        [Required(ErrorMessage = "Please Enter Address Line One")]
        //[Required(ErrorMessage = "*")]
        public string ShippingAddressOne { get; set; }

        [Display(Name = "Address Line Two")]
        public string ShippingAddressTwo { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please Enter City")]
        //[Required(ErrorMessage = "*")]
        public string ShippingCity { get; set; }

        [Display(Name = "State")]
        [RequiredIf("ShippingCountry", "USA", ErrorMessage = "Please Enter State")]
        //[Required(ErrorMessage = "Please Enter State")]
        //[Required(ErrorMessage = "*")]
        public string ShippingState { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please Enter Country")]
        //[Required(ErrorMessage = "*")]
        public string ShippingCountry { get; set; }

        [Display(Name = "Zip Code")]
        //[RegularExpression(@"^(\d{6})$", ErrorMessage = "Please Enter Valid Postal Number.")]
        [Required(ErrorMessage = "Please Enter Zip Code")]
        //[Required(ErrorMessage = "*")]
        public string ShippingPostalCode { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Please Enter Phone Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter Valid Phone Number")]
        //[Required(ErrorMessage = "*")]
        public string ShippingPhoneNumber { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^(\d{10})$", ErrorMessage = "Please Enter Valid Mobile Number")]
        //[Required(ErrorMessage = "Please Enter Mobile Number")]
        //[Required(ErrorMessage = "*")]
        public string ShippingMobileNumber { get; set; }

        public string qQuoteCurrencyName { get; set; }

        public string qQuoteSalesOrgName { get; set; }

        public bool qCreateBy { get; set; }
    }

    public class CurrencyValues
    {
        public string CurrencyName { get; set; }
        public decimal ValueCurrency { get; set; }
    }

    public class SalesOrganizations
    {
        public string salesOrgName { get; set; }
        public string customerCode { get; set; }
    }
}
