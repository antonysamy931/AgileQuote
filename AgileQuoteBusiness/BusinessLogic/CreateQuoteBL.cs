using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using AgileQuoteData.DataLogic;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;
using System.Runtime.InteropServices.ComTypes;

namespace AgileQuoteBusiness.BusinessLogic
{
    public class CreateQuoteBL
    {
        CreateQuoteDL oCrateQuoteDL = new CreateQuoteDL();

        /// <summary>
        /// Get sales organization list from database
        /// </summary>
        /// <returns>List class object</returns>
        public List<SalesOrganizations> LoadSales()
        {
            try
            {
                return oCrateQuoteDL.LoadSalesOrg();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get currency list from database
        /// </summary>
        /// <returns>List class object</returns>
        public List<CurrencyValues> LoadCurrency()
        {
            try
            {
                return oCrateQuoteDL.LoadCurrency();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get status list
        /// </summary>
        /// <returns>string collection</returns>
        public List<string> GetQuoteStatus()
        {
            try
            {
                return oCrateQuoteDL.LoadQuoteStatus();
            }
            catch
            {
                throw;
            }
        }

        public List<string> GetQuoteBusinessStatus()
        {
            try
            {
                return oCrateQuoteDL.LoadQuoteBusinessStatus();
            }
            catch
            {
                throw;
            }
        }




        public QuoteBundleMaterial QuoteBasedMaterialBundleSingleRecordSelect(int QuoteID, string Type, int? MaterialId, int? BundleId)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteBasedMaterialBundle(QuoteID, MaterialId, BundleId, Type);
            }
            catch
            {
                throw;
            }
        }

       

        public QuoteBundleMaterialList QuoteBasedMaterialBundleList(int QuoteID)
        {
            try
            {
                decimal TotalUnitPrice = ConstantValues.Zero;
                decimal TotalDiscount = ConstantValues.Zero;
                decimal TotalNetPrice = ConstantValues.Zero;
                decimal TempUnitPrice = ConstantValues.Zero;
                decimal TotalGrossPrice = ConstantValues.Zero;

                QuoteBundleMaterial oQuoteBundleMaterial = new QuoteBundleMaterial();
                QuoteBundleMaterialList ooQuoteBundleMaterialList = new QuoteBundleMaterialList();
                System.Globalization.NumberFormatInfo nfi = oCrateQuoteDL.GetFormatInfo(QuoteID);

                var oQuoteBundleMaterialList = oCrateQuoteDL.GetQuoteBasedMaterialBundleList(QuoteID);
                if (oQuoteBundleMaterialList.Count > ConstantValues.Zero)
                {
                    foreach (var item in oQuoteBundleMaterialList)
                    {
                        TotalUnitPrice = TotalUnitPrice + item.UnitPrice;
                        TempUnitPrice = TempUnitPrice + (item.UnitPrice * item.Quantity);                        
                        TotalNetPrice = TotalNetPrice + item.NetPrice;
                        TotalGrossPrice = TotalGrossPrice + item.GrossPrice;
                    }
                    
                    TotalDiscount = (1 - Convert.ToDecimal((double)TotalNetPrice / (double)TempUnitPrice)) * 100;
                    oQuoteBundleMaterial.qTotalDiscount = Convert.ToInt32(Math.Ceiling(TotalDiscount));
                    oQuoteBundleMaterial.qTotalUnitPrice = Extension.FormatCurrency(nfi, Extension.MathRound(TotalUnitPrice));
                    oQuoteBundleMaterial.qTotalNetPrice = Extension.FormatCurrency(nfi, Extension.MathRound(TotalNetPrice));
                    oQuoteBundleMaterial.qTotalGrossPrice = Extension.FormatCurrency(nfi, Extension.MathRound(TotalGrossPrice));

                }
                ooQuoteBundleMaterialList.ListQuoteBundleMaterial = oQuoteBundleMaterialList;
                ooQuoteBundleMaterialList.qQuoteBundleMaterial = oQuoteBundleMaterial;
                return ooQuoteBundleMaterialList;
            }
            catch
            {
                throw;
            }
        }

        public QuoteBundleMaterial QuoteBasedMaterialBundle(int QuoteID, string Type, int? MaterialID, int? BundleID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteBasedMaterialBundle(QuoteID, MaterialID, BundleID, Type);
            }
            catch
            {
                throw;
            }
        }

        public AgileQuoteProperty.Model.Quote RetriveQuoteDetails(int QuoteID, int UserID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteDetails(QuoteID, UserID);
            }
            catch
            {
                throw;
            }
        }

        public AgileQuoteProperty.Model.QuoteQualitativeInformation RetriveQuoteQualitativeInformationDetails(int QuoteID)
        {
            try
            {
                var QuoteQualitativeInformation = oCrateQuoteDL.GetQuoteQualitativeInformationDetails(QuoteID);
                var TotalValue = QuoteMaterialBundleTotal(QuoteID);
                if (TotalValue != null)
                {
                    System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                    string Symbol = GetCurrencySymbol(QuoteID, null);
                    nfi.CurrencySymbol = Symbol;
                    QuoteQualitativeInformation.txtQuoteValue = Extension.FormatCurrency(nfi, TotalValue.TotalNetPrice);
                    QuoteQualitativeInformation.txtPercentage = Extension.IntToDecimal(TotalValue.TotalDiscount);
                    QuoteQualitativeInformation.txtMagrinAmountPercentage = Extension.FormatCurrency(nfi, TotalValue.TotalGrossPrice) + " / " + TotalValue.TotalDiscount + "%";
                }
                return QuoteQualitativeInformation;
            }
            catch
            {
                throw;
            }
        }

        public QuoteQualitativeInformationValues GetTotalQuoteQualitativeTotal(int QuoteID)
        {
            try
            {
                var Information = new QuoteQualitativeInformationValues();
                var TotalValue = QuoteMaterialBundleTotal(QuoteID);
                if (TotalValue != null)
                {
                    string Symbol = GetCurrencySymbol(QuoteID, null);
                    System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                    nfi.CurrencySymbol = Symbol;
                    Information.txtQuoteTotalValue = Extension.FormatCurrency(nfi, TotalValue.TotalNetPrice);
                    Information.txtMarginAmountPercentageValue = Extension.FormatCurrency(nfi, TotalValue.TotalGrossPrice) + " / " + TotalValue.TotalDiscount + "%";
                }
                return Information;
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> MaterialListBasedOnCategoryName(string CategoryName, decimal BudgetTargetRate)
        {
            try
            {
                return oCrateQuoteDL.GetMaterialListBasedOnCategory(CategoryName, BudgetTargetRate);
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> GetSugectionMaterialBL(int MaterialId)
        {
            try
            {
                return oCrateQuoteDL.GetSugectionMaterial(MaterialId);
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> GetOfferedMaterialBL(int MaterialId)
        {
            try
            {
                return oCrateQuoteDL.GetOfferedMaterial(MaterialId);
            }
            catch
            {
                throw;
            }
        }

        public List<BundleProperty> BundleListBasedOnCategoryName(string CategoryName, decimal BudgetTargetRate)
        {
            try
            {
                return oCrateQuoteDL.GetBundleListBasedOnCategory(CategoryName, BudgetTargetRate);
            }
            catch
            {
                throw;
            }
        }

        public List<string> GetCategoryList()
        {
            try
            {
                return oCrateQuoteDL.GetCategoryNameList();
            }
            catch
            {
                throw;
            }
        }

        public MaterialProperty MaterialSingleRecordGet(int MaterialID, decimal BudgetTargetRate)
        {
            try
            {
                return oCrateQuoteDL.GetMaterialSingleRecord(MaterialID, BudgetTargetRate);
            }
            catch
            {
                throw;
            }
        }

        public BundleProperty BundleSingleRecordGet(int BundleID, decimal BudgetTargetRate)
        {
            try
            {
                return oCrateQuoteDL.GetBundleSingleRecord(BundleID, BudgetTargetRate);
            }
            catch
            {
                throw;
            }
        }

        public string BundleMaterialUpdate(int QuoteID, string Type, int? MaterialID, int? BundleID, int Quantity, decimal NetPrice)
        {
            try
            {
                return oCrateQuoteDL.UpdateMaterialBundleRecord(QuoteID, Type, MaterialID, BundleID, Quantity, NetPrice, false);
            }
            catch
            {
                throw;
            }
        }

        public string UpdateBundleMaterialRecordDiscountBL(int QuoteID, string Type, int? MaterialID, int? BundleID, int Discount)
        {
            try
            {
                return oCrateQuoteDL.UpdateBundleMaterialRecordDiscount(QuoteID, Type, MaterialID, BundleID, Discount);
            }
            catch
            {
                throw;
            }
        }

        public string BundlematerialDelete(int QuoteID, string Type, int? MaterialID, int? BundleID)
        {
            try
            {
                return oCrateQuoteDL.UpdateMaterialBundleRecord(QuoteID, Type, MaterialID, BundleID, null, null, true);
            }
            catch
            {
                throw;
            }
        }

        

        public string BundleMaterialAddToQuote(int QuoteID, List<MaterialBundleValue> MaterialIDlist, List<MaterialBundleValue> BundleIDlist)
        {
            try
            {
                return oCrateQuoteDL.AddBundleMaterialRecordToQuote(QuoteID, MaterialIDlist, BundleIDlist);
            }
            catch
            {
                throw;
            }
        }

       
        
        public string ReadWordDocument()
        {

            return string.Empty;
        }

        public TotalUnitDiscount QuoteMaterialBundleTotal(int QuoteID)
        {
            try
            {
                TotalUnitDiscount oTotalUnitDiscount = new TotalUnitDiscount();
                System.Globalization.NumberFormatInfo nfi = oCrateQuoteDL.GetFormatInfo(QuoteID);

                var oList = QuoteBasedMaterialBundleList(QuoteID);
                if (oList != null)
                {
                    oTotalUnitDiscount.TotalUnitPrice = Extension.FormatCurrency(nfi, oList.qQuoteBundleMaterial.qTotalUnitPrice);
                    oTotalUnitDiscount.TotalNetPrice = Extension.FormatCurrency(nfi, oList.qQuoteBundleMaterial.qTotalNetPrice);
                    oTotalUnitDiscount.TotalDiscount = oList.qQuoteBundleMaterial.qTotalDiscount;
                    oTotalUnitDiscount.TotalGrossPrice = Extension.FormatCurrency(nfi, oList.qQuoteBundleMaterial.qTotalGrossPrice);
                }

                return oTotalUnitDiscount;
            }
            catch
            {
                throw;
            }
        }       

        public int InsertQuoteDetailsBL(Quote qQuote)
        {
            try
            {
                return oCrateQuoteDL.InsertQuoteDetailsDL(qQuote);
            }
            catch
            {
                throw;
            }
        }

        public int QuoteSaveAsNewQuoteBL(int QuoteID, int UserID)
        {
            try
            {
                return oCrateQuoteDL.QuoteSaveAsNewQuote(QuoteID, UserID);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateQuoteDetailsBL(AgileQuoteProperty.Model.Quote qQuote, int QuoteID)
        {
            try
            {
                oCrateQuoteDL.UpdateQuoteDetails(qQuote, QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public string QuotePriceDetailsInsert(int QuoteID)
        {
            try
            {
                decimal UnitPrice = ConstantValues.Zero;
                int Discount = Convert.ToInt32(ConstantValues.Zero);
                decimal NetPrice = ConstantValues.Zero;
                decimal GrossPrice = ConstantValues.Zero;                

                decimal InstallationNetPrice = ConstantValues.Zero;
                decimal BoughtOutItemNetPrice = ConstantValues.Zero;
                decimal ShippingCost = ConstantValues.Zero;

                decimal budgetTargetPrice = oCrateQuoteDL.GetBudgetTargetRate(QuoteID);

                var materialBundle = QuoteMaterialBundleTotal(QuoteID);
                if (materialBundle != null)
                {        
                    UnitPrice = UnitPrice + Extension.StringToDecimal(materialBundle.TotalUnitPrice);
                    GrossPrice = GrossPrice + Extension.StringToDecimal(materialBundle.TotalGrossPrice);
                    Discount = Discount + (int)materialBundle.TotalDiscount;
                    NetPrice = NetPrice + Extension.StringToDecimal(materialBundle.TotalNetPrice);

                }

                var BoughtOutItemCharges = GetTotalCostBoughtoutItemBL(QuoteID);
                if (BoughtOutItemCharges != null)
                {                
                    BoughtOutItemNetPrice = Extension.StringToDecimal(BoughtOutItemCharges.TotalQuotedNetPrice);
                }

                var InstallCharges = GetQuoteShippingDetailsBL(QuoteID);
                if (InstallCharges != null)
                {                 
                    InstallationNetPrice = Extension.StringToDecimal(InstallCharges.TotalCost);
                }

                var shippingCharges = GetQuoteShippingListBL(QuoteID);
                if (shippingCharges.Count > 0)
                {
                    decimal tempShippingCost = 0;
                    foreach (var item in shippingCharges)
                    {
                        tempShippingCost = tempShippingCost + item.dieselCost;
                        tempShippingCost = tempShippingCost + item.truckCost;
                    }
                 
                    ShippingCost = ShippingCost + tempShippingCost;
                }

                return oCrateQuoteDL.InsertQuotePriceDetails(UnitPrice, Discount, NetPrice, QuoteID, InstallationNetPrice, BoughtOutItemNetPrice, ShippingCost);
            }
            catch
            {
                throw;
            }
        }

        public string GetCurrencySymbol(int? QuoteID, string CurrencyName)
        {
            try
            {
                return oCrateQuoteDL.GetSymbol(QuoteID, CurrencyName);
            }
            catch
            {
                throw;
            }
        }
       
        public string InsertAssignedApproverToQuote(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.AssignedApproverToQuote(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public string UpdateApprovedQuote(int QuoteID, int UserID, string Description)
        {
            try
            {
                return oCrateQuoteDL.ApprovedQuote(QuoteID, UserID, Description);
            }
            catch
            {
                throw;
            }
        }

        public string UpdateRejectQuote(int QuoteID, int UserID, string Description)
        {
            try
            {
                return oCrateQuoteDL.RejectQuote(QuoteID, UserID, Description);
            }
            catch
            {
                throw;
            }
        }

        public string RejectCreatorQuoteBL(int QuoteID, int UserID)
        {
            try
            {
                return oCrateQuoteDL.RejectCreatorQuote(QuoteID, UserID);
            }
            catch
            {
                throw;
            }
        }

        public string InsertShippingToQuoteBL(InstallCharges oShippingCharges, int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.InsertShippingToQuote(oShippingCharges, QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public string InsertQualitativeInformationBL(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {

            return oCrateQuoteDL.InsertQualitativeInformationDL(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);

        }

        public string InsertQualitativeInformationUpdateBL(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {

            return oCrateQuoteDL.InsertQualitativeInformationUpdateDL(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);

        }

        public InstallCharges GetQuoteShippingDetailsBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteShippingDetails(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteBundleMaterial> GetQuoteMaterialBundleListwithwarrantyBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteMaterialBundleListwithwarranty(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public string UpdateMaterialBundleWarrentyRecordBL(int QuoteID, string Type, int? MaterialId, int? BundleId, int OverrideWarrenty)
        {
            try
            {
                return oCrateQuoteDL.UpdateMaterialBundleWarrentyRecord(QuoteID, Type, MaterialId, BundleId, OverrideWarrenty);
            }
            catch
            {
                throw;
            }
        }

        public string InsertQuoteBoughtOutItemBL(int QuoteID, string ItemName, decimal UnitPrice, int Quantity)
        {
            try
            {
                return oCrateQuoteDL.InsertQuoteBoughtOutItem(QuoteID, ItemName, UnitPrice, Quantity);
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteBoughtOutItemModel> GetQuoteBasedBoughtoutItemListBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteBasedBoughtoutItemList(QuoteID);
            }
            catch
            {
                throw;
            }
        }


        public QuoteBoughtOutItemModel GetQuoteBasedBoughtoutItemRecordBL(int QuoteID, int ItemID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteBasedBoughtoutItemRecord(QuoteID, ItemID);
            }
            catch
            {
                throw;
            }
        }

        public string UpdateQuoteBasedBoughtoutItemRecordBL(int QuoteID, decimal UnitPrice, int Quantity, int ItemId)
        {
            try
            {
                return oCrateQuoteDL.UpdateQuoteBasedBoughtoutItemRecord(QuoteID, UnitPrice, Quantity, ItemId);
            }
            catch
            {
                throw;
            }
        }

        public string DeleteQuoteBasedBoughtoutItemRecordBL(int QuoteID, int ItemId)
        {
            try
            {
                return oCrateQuoteDL.DeleteQuoteBasedBoughtoutItemRecord(QuoteID, ItemId);
            }
            catch
            {
                throw;
            }
        }

        public TotalUnitDiscount GetTotalCostBoughtoutItemBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetTotalCostBoughtoutItem(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public List<string> GetRoleNameListBL()
        {
            try
            {
                return oCrateQuoteDL.GetRoleNameList();
            }
            catch
            {
                throw;
            }
        }

       
        public List<QuoteCollabrationModel> GetcollabratorListOnQuoteBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetcollabratorListOnQuote(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public string InsertCollabratorQuoteBL(int QuoteID, Dictionary<int, string> UserList)
        {
            try
            {
                return oCrateQuoteDL.InsertCollabratorQuote(QuoteID, UserList);
            }
            catch
            {
                throw;
            }
        }

        public List<ApproverQuoteStatus> GetApproverQuoteStatusListBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetApproverQuoteStatusList(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public string UpdateCollabratorStatusForQuoteBL(int QuoteID, int UserID, string Status, string StatusDescription, bool Delete)
        {
            try
            {
                return oCrateQuoteDL.UpdateCollabratorStatusForQuote(QuoteID, UserID, Status, StatusDescription, Delete);
            }
            catch
            {
                throw;
            }
        }

        public bool QuoteAuthorizeStatusBL(int QuoteID, int UserID, string Type)
        {
            try
            {
                return oCrateQuoteDL.QuoteAuthorizeStatus(QuoteID, UserID, Type);
            }
            catch
            {
                throw;
            }
        }

        public string QuoteShippingInsertBL(int QuoteID, decimal TruckCost, decimal DieselCost, decimal OtherCost)
        {
            try
            {
                return oCrateQuoteDL.QuoteShippingInsert(QuoteID, TruckCost, DieselCost, OtherCost);
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteShipping> GetQuoteShippingListBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteShippingList(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public decimal GetQuoteShippingTotalBL(int QuoteID)
        {
            try
            {
                decimal totalShipping = ConstantValues.Zero;
                List<QuoteShipping> oShippingList = GetQuoteShippingListBL(QuoteID);
                if (oShippingList.Count > ConstantValues.Zero)
                {

                    foreach (var item in oShippingList)
                    {
                        totalShipping = totalShipping + item.truckCost;
                        totalShipping = totalShipping + item.dieselCost;
                    }
                }
                return Math.Round(totalShipping);
            }
            catch
            {
                throw;
            }
        }

        public string UpdateQuoteShippingBL(int QuoteID, int ShippingID, decimal TruckCost, decimal DieselCost, bool Delete, decimal OtherCost)
        {
            try
            {
                return oCrateQuoteDL.UpdateQuoteShipping(QuoteID, ShippingID, TruckCost, DieselCost, Delete, OtherCost);
            }
            catch
            {
                throw;
            }
        }

        public QuoteShipping GetQuoteShippingChargesDetailsBL(int QuoteID, int ShippingID)
        {
            try
            {
                return oCrateQuoteDL.GetQuoteShippingChargesDetails(QuoteID, ShippingID);
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> GetBundleMappingMaterialBL(int BundleID, decimal BudgetTargetRate)
        {
            try
            {
                return oCrateQuoteDL.GetBundleMappingMaterial(BundleID, BudgetTargetRate);
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteCollabrationModel> GetCollaboratorListBL(int QuoteID)
        {
            try
            {
                return oCrateQuoteDL.GetCollaboratorList(QuoteID);
            }
            catch
            {
                throw;
            }
        }

        public string GetCollaboratorRemarksBL(int QuoteID, int UserID)
        {
            try
            {
                return oCrateQuoteDL.GetCollaboratorRemarks(QuoteID, UserID);
            }
            catch
            {
                throw;
            }
        }

        public List<decimal> GetMinimumMaximumAmountBL()
        {
            try
            {
                return oCrateQuoteDL.GetMinimumMaximumAmount();
            }
            catch
            {
                throw;
            }
        }

    }
}
