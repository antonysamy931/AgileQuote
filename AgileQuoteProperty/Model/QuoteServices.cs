using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{

    public class QuoteServiceslist
    {
        public List<QuoteServices> listQuoteService { get; set; }
        public int Sno { get; set; }
        public int QuoteID { get; set; }
        public int NoEmp { get; set; }
        public decimal PerdayCost { get; set; }
        public decimal TotalCost { get; set; }
        public int NoDays { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }

    public class QuoteServices
    {
        public int Sno { get; set; }
        public int QuoteID { get; set; }
        public int NoEmp { get; set; }
        public decimal PerdayCost { get; set; }
        public decimal TotalCost { get; set; }
        public int NoDays { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public List<QuoteServices> listQuoteService { get; set; }

        public string TotalCost1 { get; set; }
    }
}
