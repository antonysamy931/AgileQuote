using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AgileQuoteProperty.Model;

namespace AgileQuoteServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        /// <summary>
        /// Get Company list interface
        /// </summary>
        /// <returns>Collections</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        Dictionary<int, string> CompanyList();

        /// <summary>
        /// Check authentication interface
        /// </summary>
        /// <param name="oLogin">login class property</param>
        /// <returns>bool</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        bool CheckAuthentication(Login oLogin);

        /// <summary>
        /// Checkusername interface
        /// </summary>
        /// <param name="oLogin">login class property</param>
        /// <returns>bool</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        bool CheckUsername(Login oLogin);

        /// <summary>
        /// Get user authentication details
        /// </summary>
        /// <param name="oLogin">login class property</param>
        /// <returns>dictionary</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        Dictionary<int, string> GetUserID(Login oLogin);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string GetUserRole(Login oLogin);               

        /// <summary>
        /// Load currency value list interface
        /// </summary>
        /// <returns>List collection class object</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        List<CurrencyValues> LoadCurrency();

        /// <summary>
        /// Load sales organization list interface
        /// </summary>
        /// <returns>List collection class object</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        List<SalesOrganizations> LoadSalesOrg();

        /// <summary>
        /// Load quote status list interface
        /// </summary>
        /// <returns>String Collection</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        List<string> LoadStatus();

        /// <summary>
        /// Qualitative information dropdownlist data
        /// </summary>
        /// <returns>Collection of string</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        List<string> LoadBusinessStatus();

        /// <summary>
        /// Insert Quote Details
        /// </summary>
        /// <returns>Returns QuoteId</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        int InsertQuoteDetails(Quote qQuote);

        [OperationContract]
        [FaultContract(typeof(Error))]
        int QuoteSaveAsNewQuote(int QuoteID,int UserID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        void UpdateQuoteDetails(AgileQuoteProperty.Model.Quote qQuote, int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string InsertQuotePriceDetails(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string QuoteShippingInsert(int QuoteID, decimal TruckCost, decimal DieselCost, decimal OtherCost);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteShipping> GetQuoteShippingList(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        decimal GetQuoteShippingTotal(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string UpdateQuoteShipping(int QuoteID, int ShippingID, decimal TruckCost, decimal DieselCost, bool Delete, decimal OtherCost);

        [OperationContract]
        [FaultContract(typeof(Error))]
        QuoteShipping GetQuoteShippingChargesDetails(int QuoteID, int ShippingID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        QuoteBundleMaterialList GetQuoteBaseMaterialBundleList(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteDetails> GetQuoteListForCreate(int UserID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteDetails> GetQuoteListForApprove(int UserID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteDetails> GetQuoteListForCollaborate(int UserID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteDetails> GetAuthorizedRejectedQuoteList(int UserID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        QuoteBundleMaterial GetQuoteBaseMaterialBundle(int QuoteID, int? MaterialID, int? BundleID, string Type);        

        [OperationContract]
        [FaultContract(typeof(Error))]
        AgileQuoteProperty.Model.Quote GetQuoteDetails(int QuoteID, int UserID);

        /// <summary>
        /// Qualtative information get method
        /// </summary>
        /// <param name="QuoteID">integer quoteid</param>
        /// <returns>Object</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        AgileQuoteProperty.Model.QuoteQualitativeInformation GetQualitativeInfomation(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        QuoteQualitativeInformationValues GetTotalQuoteQualitativeTotalService(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<decimal> GetMinimumMaximumAmount();

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<MaterialProperty> GetMaterialListBasedOnCategory(string CategoryName, decimal BudgetTargetRate);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<MaterialProperty> GetSugectionMaterial(int MaterialId);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<MaterialProperty> GetOfferedMaterial(int MaterialId);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<MaterialProperty> GetBundleMappingMaterial(int BundleID, decimal BudgetTargetRate);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<BundleProperty> GetBundleListBasedOnCategory(string CategoryName, decimal BudgetTargetRate);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<string> GetCategoryNameList();

        [OperationContract]
        [FaultContract(typeof(Error))]
        MaterialProperty GetMaterialSingleRecord(int MaterialID, decimal BudgetTargetRate);

        [OperationContract]
        [FaultContract(typeof(Error))]
        BundleProperty GetBundleSingleRecord(int BundleID, decimal BudgetTargetRate);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string UpdateBundleMaterialRecord(int QuoteID, int? MaterialID, int? BundleID, string Type, int Quantity, decimal NetPrice);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string UpdateBundleMaterialRecordDiscount(int QuoteID, string Type, int? MaterialID, int? BundleID, int Discount);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string DeleteBundleMaterialRecord(int QuoteID, int? MaterialID, int? BundleID, string Type);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string AddBundleMaterialToQuote(int QuoteID, List<MaterialBundleValue> MaterialIDlist, List<MaterialBundleValue> BundleIDlist);

        [OperationContract]
        [FaultContract(typeof(Error))]
        TotalUnitDiscount GetQuoteMaterialBundleTotal(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string GetSymbol(int? QuoteID, string CurrencyName);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string AssignedApproverToQuote(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string ApprovedQuote(int QuoteID, int UserID, string Description);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string RejectQuote(int QuoteID, int UserID, string Description);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string RejectCreatorQuote(int QuoteID, int UserID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string InsertShippingToQuote(InstallCharges oShippingCharges, int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string InsertQualitativeInformation(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string InsertQualitativeInformationUpdate(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments);

        [OperationContract]
        [FaultContract(typeof(Error))]
        InstallCharges GetQuoteShippingDetails(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteBundleMaterial> GetQuoteMaterialBundleListwithwarranty(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string UpdateMaterialBundleWarrentyRecord(int QuoteID, string Type, int? MaterialId, int? BundleId, int OverrideWarrenty);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string InsertQuoteBoughtOutItem(int QuoteID, string ItemName, decimal UnitPrice, int Quantity);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteBoughtOutItemModel> GetQuoteBasedBoughtoutItemList(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        QuoteBoughtOutItemModel GetQuoteBasedBoughtoutItemRecord(int QuoteID, int ItemID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string UpdateQuoteBasedBoughtoutItemRecord(int QuoteID, decimal UnitPrice, int Quantity, int ItemId);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string DeleteQuoteBasedBoughtoutItemRecord(int QuoteID, int ItemId);

        [OperationContract]
        [FaultContract(typeof(Error))]
        TotalUnitDiscount GetTotalCostBoughtoutItem(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<string> GetRoleNameList();

        [OperationContract]
        [FaultContract(typeof(Error))]
        bool QuoteAuthorizeStatus(int QuoteID, int UserID, string Type);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteCollabrationModel> GetcollabratorListOnQuote(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<QuoteCollabrationModel> GetCollaboratorList(int QuoteID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string GetCollaboratorRemarks(int QuoteID, int UserID);

        [OperationContract]
        [FaultContract(typeof(Error))]
        string InsertCollabratorQuote(int QuoteID, Dictionary<int, string> UserList);


        [OperationContract]
        [FaultContract(typeof(Error))]
        string UpdateCollabratorStatusForQuote(int QuoteID, int UserID, string Status, string StatusDescription, bool Delete);

        [OperationContract]
        [FaultContract(typeof(Error))]
        List<ApproverQuoteStatus> GetApproverQuoteStatusList(int QuoteID);

        /// <summary>
        /// System service interface
        /// </summary>
        /// <param name="value">integer value</param>
        /// <returns>String</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        string GetData(int value);

        /// <summary>
        /// System service interface
        /// </summary>
        /// <param name="composite">Class</param>
        /// <returns>Class object</returns>
        [OperationContract]
        [FaultContract(typeof(Error))]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
