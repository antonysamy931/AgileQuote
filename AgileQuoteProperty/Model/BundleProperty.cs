using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class BundleProperty
    {
        public int sno { get; set; }
        public int BundleId { get; set; }
        public string BundleName { get; set; }
        public string BundleDescription { get; set; }
        public decimal BundleUnitPrice { get; set; }
        public int BundleDiscount { get; set; }
        public decimal BundleNetPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class BundleListProperty
    {
        public List<BundleProperty> BundleList { get; set; }
    }
}
