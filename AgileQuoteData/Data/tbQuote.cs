//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgileQuoteData.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbQuote
    {
        public tbQuote()
        {
            this.tbQuoteBundleMappings = new HashSet<tbQuoteBundleMapping>();
            this.tbQuoteMaterialMappings = new HashSet<tbQuoteMaterialMapping>();
            this.tbQuoteRentalProductMappings = new HashSet<tbQuoteRentalProductMapping>();
            this.tbQuoteRentalSparsMappings = new HashSet<tbQuoteRentalSparsMapping>();
            this.tbQuoteAuthorizes = new HashSet<tbQuoteAuthorize>();
            this.tbQuoteBundleInstalations = new HashSet<tbQuoteBundleInstalation>();
            this.tbQuoteMaterialInstalations = new HashSet<tbQuoteMaterialInstalation>();
            this.tbQuoteServices = new HashSet<tbQuoteService>();
            this.tbQuoteServices1 = new HashSet<tbQuoteService>();
            this.tbQuoteBoughtOutItems = new HashSet<tbQuoteBoughtOutItem>();
            this.tbQuoteShippings = new HashSet<tbQuoteShipping>();
        }
    
        public int QuoteID { get; set; }
        public string QuoteName { get; set; }
        public string QuoteDescription { get; set; }
        public string QuoteStatus { get; set; }
        public string CustomerName { get; set; }
        public string SalesOrganizationName { get; set; }
        public string ReferenceCustomerCode { get; set; }
        public string PreparedBy { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<decimal> BudgetRateTarget { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<decimal> NetPrice { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<decimal> InstallationCost { get; set; }
        public Nullable<decimal> BoughtOutItemCost { get; set; }
        public Nullable<decimal> ShippingCost { get; set; }
        public Nullable<int> CompanyCode { get; set; }
    
        public virtual ICollection<tbQuoteBundleMapping> tbQuoteBundleMappings { get; set; }
        public virtual ICollection<tbQuoteMaterialMapping> tbQuoteMaterialMappings { get; set; }
        public virtual ICollection<tbQuoteRentalProductMapping> tbQuoteRentalProductMappings { get; set; }
        public virtual ICollection<tbQuoteRentalSparsMapping> tbQuoteRentalSparsMappings { get; set; }
        public virtual ICollection<tbQuoteAuthorize> tbQuoteAuthorizes { get; set; }
        public virtual ICollection<tbQuoteBundleInstalation> tbQuoteBundleInstalations { get; set; }
        public virtual ICollection<tbQuoteMaterialInstalation> tbQuoteMaterialInstalations { get; set; }
        public virtual ICollection<tbQuoteService> tbQuoteServices { get; set; }
        public virtual ICollection<tbQuoteService> tbQuoteServices1 { get; set; }
        public virtual ICollection<tbQuoteBoughtOutItem> tbQuoteBoughtOutItems { get; set; }
        public virtual ICollection<tbQuoteShipping> tbQuoteShippings { get; set; }
    }
}
