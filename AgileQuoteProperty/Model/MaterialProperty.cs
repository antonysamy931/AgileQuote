using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class MaterialProperty
    {
        public int sno { get; set; }
        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialDescription { get; set; }
        public decimal MaterialAmount { get; set; }
        public int MaterialDiscount { get; set; }
        public int Quantity { get; set; }

        public List<string> CategoryName { get; set; }
    }

    public class MaterialListProperty
    {
        public List<MaterialProperty> MaterialList { get; set; }
    }

    public class MaterialBundleValue
    {
        public int Code { get; set; }
        public int Quantity { get; set; }
    }
}
