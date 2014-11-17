using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileQuoteProperty.Model;
using AgileQuoteData.DataLogic;


namespace AgileQuoteBusiness.BusinessLogic
{
    public class LoginBL
    {
        LoginDL oLoginDL = new LoginDL();

        /// <summary>
        /// Company list
        /// </summary>
        /// <returns>Collections</returns>
        public Dictionary<int, string> GetCompanyList()
        {
            try
            {
                return oLoginDL.CompanyList();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// User Authentication 
        /// </summary>
        /// <param name="oLogin">Class object</param>
        /// <returns>Boolean</returns>
        public bool AuthenticationCheck(Login oLogin)
        {
            try
            {
                return oLoginDL.checkUsernamePassword(oLogin);
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Check username valid or not
        /// </summary>
        /// <param name="oLogin">Class object</param>
        /// <returns>Boolean</returns>
        public bool UsernameCheck(Login oLogin)
        {
            try
            {
                return oLoginDL.checkUsername(oLogin);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="oLogin">Class object</param>
        /// <returns>Collection</returns>
        public Dictionary<int,string> GetUserID(Login oLogin)
        {
            try
            {
                return oLoginDL.Get_UserID(oLogin);
            }
            catch
            {
                throw;
            }
        }

        public string GetUserRoleBL(Login oLogin)
        {
            try
            {
                return oLoginDL.GetUserRole(oLogin);
            }
            catch
            {
                throw;
            }
        }
    }
}
