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
    
    public partial class tbQuoteBoughtOutItem
    {
        public int ItemID { get; set; }
        public int QuoteID { get; set; }
        public string boughtItem { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public decimal UnitPriceQuoted { get; set; }
        public decimal TotalPriceQuoted { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    
        public virtual tbQuote tbQuote { get; set; }
    }
}
