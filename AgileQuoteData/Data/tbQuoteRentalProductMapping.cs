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
    
    public partial class tbQuoteRentalProductMapping
    {
        public int Sno { get; set; }
        public Nullable<int> QuoteID { get; set; }
        public Nullable<int> RentalProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Warrenty { get; set; }
        public Nullable<decimal> OverrideWarrenty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<decimal> NetPrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    
        public virtual tbQuote tbQuote { get; set; }
        public virtual tbRentalProduct tbRentalProduct { get; set; }
    }
}