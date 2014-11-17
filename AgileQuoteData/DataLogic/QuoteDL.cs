using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;
using AgileQuoteData.Data;

namespace AgileQuoteData.DataLogic
{
    public class QuoteDL
    {        
        public AgileQuoteEntities aEntities = new AgileQuoteEntities();

        public List<QuoteDetails> GetQuoteListForCreate(int UserID)
        {
            try
            {
                List<QuoteDetails> qDetailsList = new List<QuoteDetails>();
                QuoteDetails quoteDetails = null;
                var qList = aEntities.spSelectCreateQuote(UserID).ToList();
                if (qList.Count > 0)
                {
                    foreach (var item in qList)
                    {
                        quoteDetails = new QuoteDetails();
                        quoteDetails.qQuoteID = item.QuoteID;
                        quoteDetails.qQuoteName = item.QuoteName;
                        quoteDetails.qCustomerName = item.CustomerName;
                        quoteDetails.qPrepareBy = item.PreparedBy;
                        quoteDetails.qSalesOrganization = item.SalesOrganizationName;
                        quoteDetails.qCreatDate = item.CreatedDate;
                        quoteDetails.displayCreateDate = Extension.DateFormat(item.CreatedDate);
                        quoteDetails.qRequireDate = item.RequiredDate.Value;
                        quoteDetails.displayRequireDate = Extension.DateFormat(item.RequiredDate.Value);
                        quoteDetails.AccessType = item.AccessLevel;
                        quoteDetails.Status = item.QuoteStatus;
                        qDetailsList.Add(quoteDetails);
                    }
                }
                return qDetailsList;

            }
            catch
            {
                throw;
            }
        }

        public List<QuoteDetails> GetQuoteListForApprove(int UserID)
        {
            try
            {
                List<QuoteDetails> qDetailsList = new List<QuoteDetails>();
                QuoteDetails quoteDetails = null;
                var qList = aEntities.spApproveQuote(UserID).ToList();
                if (qList.Count > 0)
                {
                    foreach (var item in qList)
                    {
                        quoteDetails = new QuoteDetails();
                        quoteDetails.qQuoteID = item.QuoteID;
                        quoteDetails.qQuoteName = item.QuoteName;
                        quoteDetails.qCustomerName = item.CustomerName;
                        quoteDetails.qPrepareBy = item.PreparedBy;
                        quoteDetails.qSalesOrganization = item.SalesOrganizationName;
                        quoteDetails.qCreatDate = item.CreatedDate;
                        quoteDetails.displayCreateDate = Extension.DateFormat(item.CreatedDate);
                        quoteDetails.qRequireDate = item.RequiredDate.Value;
                        quoteDetails.displayRequireDate = Extension.DateFormat(item.RequiredDate.Value);
                        quoteDetails.AccessType = item.AccessLevel;
                        quoteDetails.Status = item.Status;
                        quoteDetails.qLevel = item.Flag;
                        qDetailsList.Add(quoteDetails);
                    }
                }
                return qDetailsList;

            }
            catch
            {
                throw;
            }
        }

        public List<QuoteDetails> GetQuoteListForCollaborate(int UserID)
        {
            try
            {
                List<QuoteDetails> qDetailsList = new List<QuoteDetails>();
                QuoteDetails quoteDetails = null;
                var qList = aEntities.spCollaboratorQuote(UserID).ToList();
                if (qList.Count > 0)
                {
                    foreach (var item in qList)
                    {
                        quoteDetails = new QuoteDetails();
                        quoteDetails.qQuoteID = item.QuoteID;
                        quoteDetails.qQuoteName = item.QuoteName;
                        quoteDetails.qCustomerName = item.CustomerName;
                        quoteDetails.qPrepareBy = item.PreparedBy;
                        quoteDetails.qSalesOrganization = item.SalesOrganizationName;
                        quoteDetails.qCreatDate = item.CreatedDate;
                        quoteDetails.displayCreateDate = Extension.DateFormat(item.CreatedDate);
                        quoteDetails.qRequireDate = item.RequiredDate.Value;
                        quoteDetails.displayRequireDate = Extension.DateFormat(item.RequiredDate.Value);
                        quoteDetails.AccessType = item.AccessLevel;
                        quoteDetails.Status = item.Status;
                        quoteDetails.qLevel = item.Flag;
                        qDetailsList.Add(quoteDetails);
                    }
                }
                return qDetailsList;

            }
            catch
            {
                throw;
            }
        }

        public List<QuoteDetails> GetAuthorizedRejectedQuoteList(int UserID)
        {
            try
            {
                List<QuoteDetails> qDetailsList = new List<QuoteDetails>();
                QuoteDetails quoteDetails = null;
                var qList = aEntities.spAllAuthorizedRejectedQuote(UserID).ToList();
                if (qList.Count > 0)
                {
                    foreach (var item in qList)
                    {
                        quoteDetails = new QuoteDetails();
                        quoteDetails.qQuoteID = item.QuoteID;
                        quoteDetails.qQuoteName = item.QuoteName;
                        quoteDetails.qCustomerName = item.CustomerName;
                        quoteDetails.qPrepareBy = item.PreparedBy;
                        quoteDetails.qSalesOrganization = item.SalesOrganizationName;
                        quoteDetails.qCreatDate = item.CreatedDate;
                        quoteDetails.displayCreateDate = Extension.DateFormat(item.CreatedDate);
                        quoteDetails.qRequireDate = item.RequiredDate.Value;
                        quoteDetails.displayRequireDate = Extension.DateFormat(item.RequiredDate.Value);
                        quoteDetails.AccessType = item.AccessLevel;
                        quoteDetails.Status = item.QuoteStatus;
                        qDetailsList.Add(quoteDetails);
                    }
                }
                return qDetailsList;

            }
            catch
            {
                throw;
            }
        }
    }
}

