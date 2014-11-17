using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileQuoteData.Data;
using AgileQuoteProperty.Model;
using LogWriter;

namespace AgileQuoteData.DataLogic
{
    public class LoginDL
    {
        AgileQuoteAdminEntities aEntities = new AgileQuoteAdminEntities();

        /// <summary>
        /// Get company list
        /// </summary>
        /// <returns>Dictionary collection</returns>
        public Dictionary<int, string> CompanyList()
        {
            Dictionary<int, string> cList = new Dictionary<int, string>();
            try
            {
                var oCompanyList = aEntities.tbCompanies.Where(x => x.IsActive == true && x.IsDelete == false).ToList();
                if (oCompanyList != null)
                {
                    foreach (var item in oCompanyList)
                    {
                        cList.Add(item.CompanyCode, item.CompanyName);
                    }
                }
                return cList;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Check user name and password for authentication
        /// </summary>
        /// <param name="oLogin">Login class object</param>
        /// <returns>Boolean object</returns>
        public bool checkUsernamePassword(Login oLogin)
        {
            try
            {
                return aEntities.tbMembershipLogins.Where(x => x.CompanyName == oLogin.CompanyName
                    && x.EmailID == oLogin.userName
                    && x.Password == oLogin.password
                    && x.IsActive == true
                    && x.IsDelete == false).Any();               
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Check username
        /// </summary>
        /// <param name="oLogin">Login class objec</param>
        /// <returns>Boolean</returns>
        public bool checkUsername(Login oLogin)
        {
            try
            {
                return aEntities.tbMembershipLogins.Where(x => x.EmailID == oLogin.userName
                    && x.IsActive == true && x.IsDelete == false).Any();                
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="oLogin">Login object</param>
        /// <returns>Collection</returns>
        public Dictionary<int, string> Get_UserID(Login oLogin)
        {
            Dictionary<int, string> userDetails = new Dictionary<int, string>();
            try
            {
                var aCompanyCode = aEntities.tbCompanies.Where(x => x.CompanyName == oLogin.CompanyName && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                if (aCompanyCode != null)
                {
                    var aValues = aEntities.tbCustomerDetails.Where(x => x.EmailID == oLogin.userName
                        && x.CompanyCode == aCompanyCode.CompanyCode
                        && x.IsActive == true && x.IsDelete == false).Select(x => new { x.UserID, x.FirstName, x.LastName }).FirstOrDefault();
                    if (aValues != null)
                    {
                        userDetails.Add(aValues.UserID, aValues.FirstName + " " + aValues.LastName);
                    }
                }
                return userDetails;
            }
            catch
            {
                throw;
            }
        }

        public string GetUserRole(Login oLogin)
        {
            try
            {
                string RoleName = string.Empty;
                var aCompanyCode = aEntities.tbCompanies.Where(x => x.CompanyName == oLogin.CompanyName && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                if (aCompanyCode != null)
                {
                    var aValues = aEntities.tbCustomerDetails.Where(x => x.EmailID == oLogin.userName
                        && x.CompanyCode == aCompanyCode.CompanyCode
                        && x.IsActive == true && x.IsDelete == false).Select(x => new { x.UserID, x.FirstName, x.LastName, x.RoleName }).FirstOrDefault();
                    if (aValues != null)
                    {
                        RoleName = aValues.RoleName;
                    }
                }
                return RoleName;
            }
            catch
            {
                throw;
            }
        }
    }
}
