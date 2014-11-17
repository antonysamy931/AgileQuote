using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class QuoteBundleMaterialList
    {
        public List<QuoteBundleMaterial> ListQuoteBundleMaterial { get; set; }
        public QuoteBundleMaterial qQuoteBundleMaterial { get; set; }        
    }

    public class QuoteBundleMaterial
    {
        public int Sno { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string MaterialCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public int Discount { get; set; }
        public string Type { get; set; }
        public decimal Installation { get; set; }
        public decimal Warrenty { get; set; }
        public decimal OverRideWarrenty { get; set; }

        public List<MaterialProperty> ListOfMaterialMappedBundle { get; set; }

        [Display(Name = "TOTAL UNIT PRICE")]
        public string qTotalUnitPrice { get; set; }

        [Display(Name = "TOTAL DISCOUNT")]
        public int qTotalDiscount { get; set; }

        [Display(Name = "TOTAL NET PRICE")]
        public string qTotalNetPrice { get; set; }

        public string qTotalGrossPrice { get; set; }

        public decimal qTotalInstallationCost { get; set; }
    }

    public class TotalUnitDiscount
    {
        public string TotalUnitPrice { get; set; }
        public int TotalDiscount { get; set; }
        public string TotalNetPrice { get; set; }
        public string TotalGrossPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string TotalQuotedUnitPrice { get; set; }
        public string TotalQuotedNetPrice { get; set; }
    }
}
