using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileQuoteProperty.Model;
using AgileQuoteData.DataLogic;

namespace AgileQuoteBusiness.BusinessLogic
{
    public class QuoteBL
    {
        QuoteDL quoteDL = new QuoteDL();        

        public List<QuoteDetails> GetQuoteListForCreateBL(int UserID)
        {
            try
            {
                return quoteDL.GetQuoteListForCreate(UserID);
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteDetails> GetQuoteListForApproveBL(int UserID)
        {
            try
            {
                return quoteDL.GetQuoteListForApprove(UserID);
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteDetails> GetQuoteListForCollaborateBL(int UserID)
        {
            try
            {
                return quoteDL.GetQuoteListForCollaborate(UserID);
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteDetails> GetAuthorizedRejectedQuoteListBL(int UserID)
        {
            try
            {
                return quoteDL.GetAuthorizedRejectedQuoteList(UserID);
            }
            catch
            {
                throw;
            }
        }       
    }
}
