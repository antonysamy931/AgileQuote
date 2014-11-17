using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class ReturnJson
    {
        public string RedirectUrl { get; set; }
        public bool IsRedirect { get; set; }
        public object Result { get; set; }
    }
}
