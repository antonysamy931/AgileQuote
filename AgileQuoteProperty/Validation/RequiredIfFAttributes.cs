using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AgileQuoteProperty.Validation
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private RequiredAttribute innerAttribute = new RequiredAttribute();
        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            this.DependentProperty = dependentProperty;
            this.TargetValue = targetValue;
        }

        public override bool IsValid(object value)
        {
            return innerAttribute.IsValid(value);
        }
    }
}
