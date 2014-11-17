using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{    
    public class InstallCharges
    {
        public int NoEmployees { get; set; }
        public string CostPerDay { get; set; }
        public int NoDays { get; set; }
        public string TotalCost { get; set; }
    }

    public class QuoteShipping
    {
        public int Sno { get; set; }
        public int ShippingID { get; set; }
        public decimal truckCost { get; set; }
        public decimal dieselCost { get; set; }
        public decimal otherCost { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
