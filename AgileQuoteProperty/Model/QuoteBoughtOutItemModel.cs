using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class QuoteBoughtOutItemModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal NetPrice { get; set; }
        public decimal quotedUnitPrice { get; set; }
        public decimal quotedNetPrice { get; set; }        
    }
}
