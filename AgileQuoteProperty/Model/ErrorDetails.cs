using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace AgileQuoteProperty.Model
{    
    public class Error
    {        
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
}
