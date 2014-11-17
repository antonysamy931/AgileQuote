using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AgileQuoteProperty.Validation
{
    public class RequiredIfValidator : DataAnnotationsModelValidator<RequiredIfAttribute>
    {
        public RequiredIfValidator(ModelMetadata metadata, ControllerContext context, RequiredIfAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var fields = Metadata.ContainerType.GetProperty(Attribute.DependentProperty);
            if (fields != null)
            {
                var value = fields.GetValue(container, null);
                if (value != null)
                {
                    if ((value == null && Attribute.TargetValue == null) || (value.Equals(Attribute.TargetValue)))
                    {
                        if (!Attribute.IsValid(Metadata.Model))
                        {
                            yield return new ModelValidationResult { Message = ErrorMessage };
                        }
                    }
                }
            }
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;

//namespace CustomAttribute.Models
//{
//    public class CustomeAtt : ValidationAttribute
//    {
//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            if (value.ToString().Contains(">") || value.ToString().Contains("<"))
//            {
//                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
//            }

            
//            return ValidationResult.Success;
//        }
//    }
//}