using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace AgileQuoteProperty.Model
{
    public class Login
    {
        [Required(ErrorMessage="Please enter username")]
        [Display(Name="User Name")]        
        [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage="Please enter valid email address")]
        public string userName { get; set; }

        [Required(ErrorMessage="Please enter password")]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage="Please enter text")]
        [Display(Name="Secret Number")]
        public int RandomNumber { get; set; }

        [Required(ErrorMessage="Please select company name")]
        [Display(Name="Company Name")]
        public string CompanyName { get; set; }        

        public Dictionary<int, string> CompanyList { get; set; }        
    }
}
