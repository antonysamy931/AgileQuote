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
    
    public partial class tbQuoteCollabrator
    {
        public int Sno { get; set; }
        public int QuoteID { get; set; }
        public int ColabratorID { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
    }
}