using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AgileQuoteProperty.Model
{
    public class CreateQuoteProduct
    {
        [Required(ErrorMessage="Please enter value")]
        public string textboxone { get; set; }

        public StringBuilder editor { get; set; }
    }
}
