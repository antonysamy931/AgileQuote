using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AgileQuoteBusiness.BusinessLogic;
using AgileQuoteProperty.Model;

namespace AgileQuoteServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Service1 : IService1
    {
        #region BusinessClass
        QuoteBL quoteBL = new QuoteBL();
        LoginBL loginBL = new LoginBL();
        CreateQuoteBL CreateQuoteBL = new CreateQuoteBL();
        Error eDetails = new Error();
        #endregion

        #region AuthenticationService
        /// <summary>
        /// Check authentication service check user authentication
        /// </summary>
        /// <param name="oLogin">User property</param>
        /// <returns>Bool value</returns>
        public bool CheckAuthentication(Login oLogin)
        {
            try
            {                
                return loginBL.AuthenticationCheck(oLogin);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, "Invalid operation.", FaultCode.CreateSenderFaultCode(null));
            }
        }

        /// <summary>
        /// Check user username exist or not
        /// </summary>
        /// <param name="oLogin">User property</param>
        /// <returns>Bool</returns>
        public bool CheckUsername(Login oLogin)
        {
            try
            {
                return loginBL.UsernameCheck(oLogin);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        /// <summary>
        /// Get userid and user name(first name + last name)
        /// </summary>
        /// <param name="oLogin">User property</param>
        /// <returns>Collection</returns>
        public Dictionary<int, string> GetUserID(Login oLogin)
        {
            try
            {
                return loginBL.GetUserID(oLogin);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string GetUserRole(Login oLogin)
        {
            try
            {
                return loginBL.GetUserRoleBL(oLogin);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        #region Load Dropdownlist
        /// <summary>
        /// Load Company list
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> CompanyList()
        {
            try
            {
                return loginBL.GetCompanyList();
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.Message);
            }
        }

        /// <summary>
        /// Get currency list
        /// </summary>
        /// <returns>Collection currencyclass object</returns>
        public List<CurrencyValues> LoadCurrency()
        {
            try
            {
                return CreateQuoteBL.LoadCurrency();
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        /// <summary>
        /// Get sales organization list
        /// </summary>
        /// <returns>Collection salesorg class object</returns>
        public List<SalesOrganizations> LoadSalesOrg()
        {
            try
            {
                return CreateQuoteBL.LoadSales();
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        /// <summary>
        /// Load status
        /// </summary>
        /// <returns></returns>
        public List<string> LoadStatus()
        {
            try
            {
                return CreateQuoteBL.GetQuoteStatus();
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.Message);
            }
        }

        /// <summary>
        /// Qualitative information dropdownlist data
        /// </summary>
        /// <returns>Collection of string</returns>
        public List<string> LoadBusinessStatus()
        {
            try
            {
                return CreateQuoteBL.GetQuoteBusinessStatus();
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.Message);
            }
        }

        #endregion

        #region Quote Shipping

        public string QuoteShippingInsert(int QuoteID, decimal TruckCost, decimal DieselCost, decimal OtherCost)
        {
            try
            {
                return CreateQuoteBL.QuoteShippingInsertBL(QuoteID, TruckCost, DieselCost, OtherCost);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteShipping> GetQuoteShippingList(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetQuoteShippingListBL(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public decimal GetQuoteShippingTotal(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetQuoteShippingTotalBL(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string UpdateQuoteShipping(int QuoteID, int ShippingID, decimal TruckCost, decimal DieselCost, bool Delete, decimal OtherCost)
        {
            try
            {
                return CreateQuoteBL.UpdateQuoteShippingBL(QuoteID, ShippingID, TruckCost, DieselCost, Delete, OtherCost);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public QuoteShipping GetQuoteShippingChargesDetails(int QuoteID, int ShippingID)
        {
            try
            {
                return CreateQuoteBL.GetQuoteShippingChargesDetailsBL(QuoteID, ShippingID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        #region Create Quote

        public int InsertQuoteDetails(Quote qQuote)
        {
            try
            {
                return CreateQuoteBL.InsertQuoteDetailsBL(qQuote);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public int QuoteSaveAsNewQuote(int QuoteID, int UserID)
        {
            try
            {
                return CreateQuoteBL.QuoteSaveAsNewQuoteBL(QuoteID, UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public void UpdateQuoteDetails(AgileQuoteProperty.Model.Quote qQuote, int QuoteID)
        {
            try
            {
                CreateQuoteBL.UpdateQuoteDetailsBL(qQuote, QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string InsertQuotePriceDetails(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.QuotePriceDetailsInsert(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        #region Quote

        public QuoteBundleMaterialList GetQuoteBaseMaterialBundleList(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.QuoteBasedMaterialBundleList(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteDetails> GetQuoteListForCreate(int UserID)
        {
            try
            {
                return quoteBL.GetQuoteListForCreateBL(UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteDetails> GetQuoteListForApprove(int UserID)
        {
            try
            {
                return quoteBL.GetQuoteListForApproveBL(UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteDetails> GetQuoteListForCollaborate(int UserID)
        {
            try
            {
                return quoteBL.GetQuoteListForCollaborateBL(UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteDetails> GetAuthorizedRejectedQuoteList(int UserID)
        {
            try
            {
                return quoteBL.GetAuthorizedRejectedQuoteListBL(UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public QuoteBundleMaterial GetQuoteBaseMaterialBundle(int QuoteID, int? MaterialID, int? BundleID, string Type)
        {
            try
            {
                return CreateQuoteBL.QuoteBasedMaterialBundle(QuoteID, Type, MaterialID, BundleID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        //public QuoteBundleMaterialList GetConfigureBundleSingleRecord(int BundleId, int QuoteID)
        //{
        //    try
        //    {
        //        return CreateQuoteBL.GetConfigureBundleSingleRecordBL(BundleId, QuoteID);
        //    }
        //    catch (Exception ex)
        //    {
        //        eDetails.ErrorDetails = ex.StackTrace;
        //        eDetails.ErrorMessage = ex.Message;
        //        throw new FaultException<Error>(eDetails, ex.ToString());
        //    }
        //}

        public AgileQuoteProperty.Model.Quote GetQuoteDetails(int QuoteID, int UserID)
        {
            try
            {
                return CreateQuoteBL.RetriveQuoteDetails(QuoteID, UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }




        #endregion

        /// <summary>
        /// Get Quatitative Information
        /// </summary>
        /// <param name="QuoteID">QuoteID</param>
        /// <returns>Object</returns>
        public AgileQuoteProperty.Model.QuoteQualitativeInformation GetQualitativeInfomation(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.RetriveQuoteQualitativeInformationDetails(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public QuoteQualitativeInformationValues GetTotalQuoteQualitativeTotalService(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetTotalQuoteQualitativeTotal(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #region Material Bundle

        public List<decimal> GetMinimumMaximumAmount()
        {
            try
            {
                return CreateQuoteBL.GetMinimumMaximumAmountBL();
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<MaterialProperty> GetMaterialListBasedOnCategory(string CategoryName, decimal BudgetTargetRate)
        {
            try
            {
                return CreateQuoteBL.MaterialListBasedOnCategoryName(CategoryName, BudgetTargetRate);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<MaterialProperty> GetSugectionMaterial(int MaterialId)
        {
            try
            {
                return CreateQuoteBL.GetSugectionMaterialBL(MaterialId);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<MaterialProperty> GetOfferedMaterial(int MaterialId)
        {
            try
            {
                return CreateQuoteBL.GetOfferedMaterialBL(MaterialId);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<MaterialProperty> GetBundleMappingMaterial(int BundleID, decimal BudgetTargetRate)
        {
            try
            {
                return CreateQuoteBL.GetBundleMappingMaterialBL(BundleID, BudgetTargetRate);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<BundleProperty> GetBundleListBasedOnCategory(string CategoryName, decimal BudgetTargetRate)
        {
            try
            {
                return CreateQuoteBL.BundleListBasedOnCategoryName(CategoryName, BudgetTargetRate);
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<string> GetCategoryNameList()
        {
            try
            {
                return CreateQuoteBL.GetCategoryList();
            }
            catch (Exception ex)
            {
                eDetails.ErrorDetails = ex.StackTrace;
                eDetails.ErrorMessage = ex.Message;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public MaterialProperty GetMaterialSingleRecord(int MaterialID, decimal BudgetTargetRate)
        {
            try
            {
                return CreateQuoteBL.MaterialSingleRecordGet(MaterialID, BudgetTargetRate);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public BundleProperty GetBundleSingleRecord(int BundleID, decimal BudgetTargetRate)
        {
            try
            {
                return CreateQuoteBL.BundleSingleRecordGet(BundleID, BudgetTargetRate);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        #region Update Quote based Material Bundle RentalProduct RentalSpar

        public string UpdateBundleMaterialRecord(int QuoteID, int? MaterialID, int? BundleID, string Type, int Quantity, decimal NetPrice)
        {
            try
            {
                return CreateQuoteBL.BundleMaterialUpdate(QuoteID, Type, MaterialID, BundleID, Quantity, NetPrice);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string UpdateBundleMaterialRecordDiscount(int QuoteID, string Type, int? MaterialID, int? BundleID, int Discount)
        {
            try
            {
                return CreateQuoteBL.UpdateBundleMaterialRecordDiscountBL(QuoteID, Type, MaterialID, BundleID, Discount);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        #region Delete Quote based Material Bundle RentalProduct RentalSpar

        public string DeleteBundleMaterialRecord(int QuoteID, int? MaterialID, int? BundleID, string Type)
        {
            try
            {
                return CreateQuoteBL.BundlematerialDelete(QuoteID, Type, MaterialID, BundleID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        #region Add Material Bundle RentalProduct RentalSpar to Quote

        public string AddBundleMaterialToQuote(int QuoteID, List<MaterialBundleValue> MaterialIDlist, List<MaterialBundleValue> BundleIDlist)
        {
            try
            {
                return CreateQuoteBL.BundleMaterialAddToQuote(QuoteID, MaterialIDlist, BundleIDlist);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        #region Calculate total UnitPrice NetPrice Discount for Quote based Material bundle rentalproduct rentalspar

        public TotalUnitDiscount GetQuoteMaterialBundleTotal(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.QuoteMaterialBundleTotal(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        #endregion

        public string GetSymbol(int? QuoteID, string CurrencyName)
        {
            try
            {
                return CreateQuoteBL.GetCurrencySymbol(QuoteID, CurrencyName);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string AssignedApproverToQuote(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.InsertAssignedApproverToQuote(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string ApprovedQuote(int QuoteID, int UserID, string Description)
        {
            try
            {
                return CreateQuoteBL.UpdateApprovedQuote(QuoteID, UserID, Description);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string RejectQuote(int QuoteID, int UserID, string Description)
        {
            try
            {
                return CreateQuoteBL.UpdateRejectQuote(QuoteID, UserID, Description);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string RejectCreatorQuote(int QuoteID, int UserID)
        {
            try
            {
                return CreateQuoteBL.RejectCreatorQuoteBL(QuoteID, UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string InsertShippingToQuote(InstallCharges oShippingCharges, int QuoteID)
        {
            try
            {
                return CreateQuoteBL.InsertShippingToQuoteBL(oShippingCharges, QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string InsertQualitativeInformation(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {
            return CreateQuoteBL.InsertQualitativeInformationBL(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);

        }

        public string InsertQualitativeInformationUpdate(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {
            return CreateQuoteBL.InsertQualitativeInformationUpdateBL(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);

        }


        public InstallCharges GetQuoteShippingDetails(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetQuoteShippingDetailsBL(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteBundleMaterial> GetQuoteMaterialBundleListwithwarranty(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetQuoteMaterialBundleListwithwarrantyBL(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string UpdateMaterialBundleWarrentyRecord(int QuoteID, string Type, int? MaterialId, int? BundleId, int OverrideWarrenty)
        {
            try
            {
                return CreateQuoteBL.UpdateMaterialBundleWarrentyRecordBL(QuoteID, Type, MaterialId, BundleId, OverrideWarrenty);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string InsertQuoteBoughtOutItem(int QuoteID, string ItemName, decimal UnitPrice, int Quantity)
        {
            try
            {
                return CreateQuoteBL.InsertQuoteBoughtOutItemBL(QuoteID, ItemName, UnitPrice, Quantity);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteBoughtOutItemModel> GetQuoteBasedBoughtoutItemList(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetQuoteBasedBoughtoutItemListBL(QuoteID);

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public QuoteBoughtOutItemModel GetQuoteBasedBoughtoutItemRecord(int QuoteID, int ItemID)
        {
            try
            {
                return CreateQuoteBL.GetQuoteBasedBoughtoutItemRecordBL(QuoteID, ItemID);

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string UpdateQuoteBasedBoughtoutItemRecord(int QuoteID, decimal UnitPrice, int Quantity, int ItemId)
        {
            try
            {
                return CreateQuoteBL.UpdateQuoteBasedBoughtoutItemRecordBL(QuoteID, UnitPrice, Quantity, ItemId);

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string DeleteQuoteBasedBoughtoutItemRecord(int QuoteID, int ItemId)
        {
            try
            {
                return CreateQuoteBL.DeleteQuoteBasedBoughtoutItemRecordBL(QuoteID, ItemId);

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public TotalUnitDiscount GetTotalCostBoughtoutItem(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetTotalCostBoughtoutItemBL(QuoteID);

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<string> GetRoleNameList()
        {
            try
            {
                return CreateQuoteBL.GetRoleNameListBL();

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public bool QuoteAuthorizeStatus(int QuoteID, int UserID, string Type)
        {
            try
            {
                return CreateQuoteBL.QuoteAuthorizeStatusBL(QuoteID, UserID, Type);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteCollabrationModel> GetcollabratorListOnQuote(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetcollabratorListOnQuoteBL(QuoteID);

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<QuoteCollabrationModel> GetCollaboratorList(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetCollaboratorListBL(QuoteID);

            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string GetCollaboratorRemarks(int QuoteID, int UserID)
        {
            try
            {
                return CreateQuoteBL.GetCollaboratorRemarksBL(QuoteID, UserID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string InsertCollabratorQuote(int QuoteID, Dictionary<int, string> UserList)
        {
            try
            {
                return CreateQuoteBL.InsertCollabratorQuoteBL(QuoteID, UserList);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public string UpdateCollabratorStatusForQuote(int QuoteID, int UserID, string Status, string StatusDescription, bool Delete)
        {
            try
            {
                return CreateQuoteBL.UpdateCollabratorStatusForQuoteBL(QuoteID, UserID, Status, StatusDescription, Delete);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        public List<ApproverQuoteStatus> GetApproverQuoteStatusList(int QuoteID)
        {
            try
            {
                return CreateQuoteBL.GetApproverQuoteStatusListBL(QuoteID);
            }
            catch (Exception ex)
            {
                eDetails.ErrorMessage = ex.Message;
                eDetails.ErrorDetails = ex.StackTrace;
                throw new FaultException<Error>(eDetails, ex.ToString());
            }
        }

        /*System default services start*/
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        /*System default services end*/
    }
}
