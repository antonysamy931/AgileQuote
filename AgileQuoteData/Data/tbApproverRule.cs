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
    
    public partial class tbApproverRule
    {
        public int RuleId { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
        public int MinimumDiscount { get; set; }
        public int MaximumDiscount { get; set; }
        public string RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
