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
    
    public partial class tbQuoteService
    {
        public int Sno { get; set; }
        public int QuoteID { get; set; }
        public int NoEmp { get; set; }
        public decimal PerdayCost { get; set; }
        public decimal TotalCost { get; set; }
        public int NoDays { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    
        public virtual tbQuote tbQuote { get; set; }
        public virtual tbQuote tbQuote1 { get; set; }
    }
}
