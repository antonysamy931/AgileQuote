using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Model
{
    public class ApproverQuoteStatus
    {
        public string RoleName { get; set; }
        public string EmailAddress { get; set; }
        public int AuthorizerID { get; set; }
        public string QuoteStatus { get; set; }
        public string StatusDescription { get; set; }
    }
}
