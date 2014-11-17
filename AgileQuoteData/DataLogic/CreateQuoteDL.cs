using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;
using AgileQuoteData.Data;
using System.Globalization;

namespace AgileQuoteData.DataLogic
{
    public class CreateQuoteDL
    {
        AgileQuoteEntities aQuoteEntities = new AgileQuoteEntities();
        AgileQuoteAdminEntities aQuoteAdminEntities = new AgileQuoteAdminEntities();

        /// <summary>
        /// Get sales organization list from database
        /// </summary>
        /// <returns>List of class object</returns>
        public List<SalesOrganizations> LoadSalesOrg()
        {
            List<SalesOrganizations> lSalesOrg = new List<SalesOrganizations>();
            SalesOrganizations oSalesOrganization = null;
            try
            {
                var ooSalesOrg = aQuoteEntities.SalesOrgSelect(null).ToList();
                if (ooSalesOrg != null)
                {
                    foreach (var item in ooSalesOrg)
                    {
                        oSalesOrganization = new SalesOrganizations();
                        oSalesOrganization.customerCode = item.ReferenceCustomerCode;
                        oSalesOrganization.salesOrgName = item.SalesOrganizationName;
                        lSalesOrg.Add(oSalesOrganization);
                    }
                }
                return lSalesOrg;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get currency list from database
        /// </summary>
        /// <returns>List of class object</returns>
        public List<CurrencyValues> LoadCurrency()
        {
            List<CurrencyValues> lCurrency = new List<CurrencyValues>();
            CurrencyValues oValues = null;
            try
            {
                var ooCurrency = aQuoteEntities.CurrencySelect(null).ToList();
                if (ooCurrency != null)
                {
                    foreach (var item in ooCurrency)
                    {
                        oValues = new CurrencyValues();
                        oValues.CurrencyName = item.CurrencyName;
                        oValues.ValueCurrency = item.Amount.Value;
                        lCurrency.Add(oValues);
                    }
                }
                return lCurrency;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Load quote status list
        /// </summary>
        /// <returns>string collection</returns>
        public List<string> LoadQuoteStatus()
        {
            List<string> oStatus = new List<string>();
            try
            {
                var sObject = aQuoteEntities.tbQuoteStatus.Where(x => x.IsActive == true && x.IsDelete == false).ToList();
                if (sObject != null)
                {
                    foreach (var item in sObject)
                    {
                        oStatus.Add(item.QuoteStatusName);
                    }
                }
                return oStatus;
            }
            catch
            {
                throw;
            }
        }

        public List<string> LoadQuoteBusinessStatus()
        {
            List<string> dBusinessStatus = new List<string>();
            var dObject = aQuoteEntities.tbQuoteBusinessLists.ToList();
            foreach (var item in dObject)
            {
                dBusinessStatus.Add(item.QuoteBusinessName);
            }
            return dBusinessStatus;
        }

        /// <summary>
        /// InsertQuoteDetails mothod insert Quote details and Quote billing and shipping details
        /// </summary>
        /// <param name="qQuote">Quote Class object</param>
        /// <returns>Integer QuoteID</returns>
        public int InsertQuoteDetailsDL(AgileQuoteProperty.Model.Quote qQuote)
        {
            try
            {
                var oCustomerDetails = aQuoteAdminEntities.tbCustomerDetails.Where(x => x.UserID == qQuote.CreatedBy && x.IsActive == true && x.IsDelete == false).FirstOrDefault();

                tbQuote oQuote = new tbQuote();
                oQuote.CreatedBy = qQuote.CreatedBy;
                oQuote.PreparedBy = qQuote.PreparedBy;
                oQuote.QuoteStatus = ConstantValues.QuoteProcessingStatus;
                oQuote.QuoteName = qQuote.QuoteName;
                oQuote.ReferenceCustomerCode = qQuote.CustomerCode;
                oQuote.RequiredDate = DateTime.Parse(qQuote.RequrestedDate);
                oQuote.SalesOrganizationName = qQuote.salesOrgName;
                oQuote.CustomerName = qQuote.CustomerName;
                oQuote.CurrencyName = qQuote.currencyName;
                oQuote.BudgetRateTarget = qQuote.BudgetValue;
                oQuote.IsActive = true;
                oQuote.IsDelete = false;

                if (oCustomerDetails != null)
                {
                    oQuote.CompanyCode = oCustomerDetails.CompanyCode;
                }
                aQuoteEntities.tbQuotes.Add(oQuote);
                aQuoteEntities.SaveChanges();

                tbAddress billingAddress = new tbAddress();
                billingAddress.QuoteID = oQuote.QuoteID;
                billingAddress.AddressOne = qQuote.BillingAddressOne;
                billingAddress.AddressTwo = qQuote.BillingAddressTwo;
                billingAddress.City = qQuote.BillingCity;
                billingAddress.State = qQuote.BillingState;
                billingAddress.Country = qQuote.BillingCountry;
                billingAddress.PostalCode = qQuote.BillingPostalCode;
                billingAddress.PhoneNumber = qQuote.BillingPhoneNumber;
                billingAddress.MobileNumber = qQuote.BillingMobileNumber;
                billingAddress.AddressType = ConstantValues.Billing;
                billingAddress.IsActive = true;
                billingAddress.IsDelete = false;
                aQuoteAdminEntities.tbAddresses.Add(billingAddress);

                tbAddress shippingAddress = new tbAddress();
                shippingAddress.QuoteID = oQuote.QuoteID;
                shippingAddress.AddressOne = qQuote.ShippingAddressOne;
                shippingAddress.AddressTwo = qQuote.ShippingAddressTwo;
                shippingAddress.City = qQuote.ShippingCity;
                shippingAddress.State = qQuote.ShippingState;
                shippingAddress.Country = qQuote.ShippingCountry;
                shippingAddress.PostalCode = qQuote.ShippingPostalCode;
                shippingAddress.PhoneNumber = qQuote.ShippingPhoneNumber;
                shippingAddress.MobileNumber = qQuote.ShippingMobileNumber;
                shippingAddress.AddressType = ConstantValues.Shipping;
                shippingAddress.IsDelete = false;
                shippingAddress.IsActive = true;
                aQuoteAdminEntities.tbAddresses.Add(shippingAddress);

                aQuoteAdminEntities.SaveChanges();

                return oQuote.QuoteID;
            }
            catch
            {
                throw;
            }
        }

        public int QuoteSaveAsNewQuote(int QuoteID, int UserID)
        {
            try
            {
                string preparedBy = string.Empty;
                int CreatedBy = Convert.ToInt32(ConstantValues.Zero);
                int CompanyCode = Convert.ToInt32(ConstantValues.Zero);
                var oUserDetails = aQuoteAdminEntities.tbCustomerDetails.Where(x => x.UserID == UserID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oUserDetails != null)
                {
                    preparedBy = oUserDetails.FirstName + " " + oUserDetails.LastName;
                    CreatedBy = UserID;
                    CompanyCode = oUserDetails.CompanyCode;
                }
                int QuoteId = aQuoteEntities.spSaveAsNewQuote(QuoteID, CreatedBy, CompanyCode, preparedBy, ConstantValues.QuoteRequestDateConstant).Select(x => x.QuoteID).FirstOrDefault();
                aQuoteAdminEntities.spSaveAsNewQuoteAddress(QuoteID, QuoteId);
                return QuoteId;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateQuoteDetails(AgileQuoteProperty.Model.Quote qQuote, int QuoteID)
        {
            try
            {
                var oQuoteDetails = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oQuoteDetails != null)
                {
                    oQuoteDetails.SalesOrganizationName = qQuote.salesOrgName;
                    oQuoteDetails.ReferenceCustomerCode = qQuote.CustomerCode;
                    oQuoteDetails.CustomerName = qQuote.CustomerName;
                    oQuoteDetails.RequiredDate = DateTime.Parse(qQuote.RequrestedDate);
                    oQuoteDetails.CurrencyName = qQuote.currencyName;
                    oQuoteDetails.BudgetRateTarget = qQuote.BudgetValue;
                    oQuoteDetails.QuoteName = qQuote.QuoteName;
                    oQuoteDetails.PreparedBy = qQuote.PreparedBy;
                    oQuoteDetails.QuoteStatus = ConstantValues.QuoteProcessingStatus;
                }

                aQuoteEntities.SaveChanges();

                var oQuoteBillingAddress = aQuoteAdminEntities.tbAddresses.Where(x => x.QuoteID == QuoteID && x.AddressType == ConstantValues.Billing && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oQuoteBillingAddress != null)
                {
                    oQuoteBillingAddress.AddressOne = qQuote.BillingAddressOne;
                    oQuoteBillingAddress.AddressTwo = qQuote.BillingAddressTwo;
                    oQuoteBillingAddress.Country = qQuote.BillingCountry;
                    oQuoteBillingAddress.State = qQuote.BillingState;
                    oQuoteBillingAddress.City = qQuote.BillingCity;
                    oQuoteBillingAddress.PostalCode = qQuote.BillingPostalCode;
                    oQuoteBillingAddress.PhoneNumber = qQuote.BillingPhoneNumber;
                    oQuoteBillingAddress.MobileNumber = qQuote.BillingMobileNumber;
                }

                var oQuoteShippingAddress = aQuoteAdminEntities.tbAddresses.Where(x => x.QuoteID == QuoteID && x.AddressType == ConstantValues.Shipping && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oQuoteShippingAddress != null)
                {
                    oQuoteShippingAddress.AddressOne = qQuote.ShippingAddressOne;
                    oQuoteShippingAddress.AddressTwo = qQuote.ShippingAddressTwo;
                    oQuoteShippingAddress.Country = qQuote.ShippingCountry;
                    oQuoteShippingAddress.State = qQuote.ShippingState;
                    oQuoteShippingAddress.City = qQuote.ShippingCity;
                    oQuoteShippingAddress.PostalCode = qQuote.ShippingPostalCode;
                    oQuoteShippingAddress.PhoneNumber = qQuote.ShippingPhoneNumber;
                    oQuoteShippingAddress.MobileNumber = qQuote.ShippingMobileNumber;
                }

                aQuoteAdminEntities.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public string InsertQuotePriceDetails(decimal QuoteUnitPrice, int QuoteDiscount, decimal QuoteNetPrice, int QuoteID, decimal InstallationNetPrice, decimal BoughtOutItemNetPrice, decimal ShippingCost)
        {
            try
            {
                var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID).FirstOrDefault();
                if (oQuote != null)
                {
                    oQuote.UnitPrice = Convert.ToDecimal(QuoteUnitPrice);
                    oQuote.Discount = QuoteDiscount;
                    oQuote.NetPrice = QuoteNetPrice;
                    oQuote.InstallationCost = InstallationNetPrice;
                    oQuote.BoughtOutItemCost = BoughtOutItemNetPrice;
                    oQuote.ShippingCost = ShippingCost;
                    aQuoteEntities.SaveChanges();
                }
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Budget Target Rate method get currency values
        /// </summary>
        /// <param name="QuoteID">Integer quote id</param>
        /// <returns>decimal values</returns>
        public decimal GetBudgetTargetRate(int QuoteID)
        {
            try
            {
                return aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID).Select(x => x.BudgetRateTarget).FirstOrDefault().Value;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetQuoteBasedMaterialBundleList method retrived Quote base mapped Material and Bundle with cost and discount
        /// </summary>
        /// <param name="QuoteID"></param>
        /// <returns>List of object</returns>
        public List<QuoteBundleMaterial> GetQuoteBasedMaterialBundleList(int QuoteID)
        {
            try
            {
                decimal oBundgetTargetRate = GetBudgetTargetRate(QuoteID);
                List<QuoteBundleMaterial> oQuoteBundleMaterialList = new List<QuoteBundleMaterial>();
                QuoteBundleMaterial oQuoteBundleMaterial = null;
                int Sno = Convert.ToInt32(ConstantValues.One);
                int UserID = Convert.ToInt32(ConstantValues.Zero);
                var qQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID).FirstOrDefault();
                if (qQuote != null)
                {
                    UserID = qQuote.CreatedBy;
                }
                var oBundleMaterial = aQuoteEntities.spQuoteBasedMaterialBundle(QuoteID, UserID).ToList();


                if (oBundleMaterial != null)
                {
                    foreach (var item in oBundleMaterial)
                    {
                        oQuoteBundleMaterial = new QuoteBundleMaterial();
                        oQuoteBundleMaterial.Quantity = item.Quantity.Value;
                        oQuoteBundleMaterial.Code = item.Code;
                        oQuoteBundleMaterial.Description = item.Discription;
                        oQuoteBundleMaterial.Discount = item.Discount.Value;
                        oQuoteBundleMaterial.Name = item.Name;
                        oQuoteBundleMaterial.NetPrice = Extension.MathRound(item.NetPrice.Value);
                        oQuoteBundleMaterial.GrossPrice = Extension.MathRound(item.UnitPrice.Value * item.Quantity.Value);
                        oQuoteBundleMaterial.Sno = Sno;
                        oQuoteBundleMaterial.UnitPrice = Extension.MathRound(item.UnitPrice.Value);
                        oQuoteBundleMaterial.Type = item.Type;
                        if (item.Type == ConstantValues.TypeBundle)
                        {
                            oQuoteBundleMaterial.ListOfMaterialMappedBundle = GetBundleMappingMaterial(item.Code, oBundgetTargetRate);
                        }
                        oQuoteBundleMaterialList.Add(oQuoteBundleMaterial);
                        Sno++;
                    }
                }
                return oQuoteBundleMaterialList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Single record based on Quote and select Material or bundle
        /// </summary>
        /// <param name="MaterialID">Nullable MaterialID Select material record</param>
        /// <param name="BundleID">Nullable BundleID select Bundle record</param>
        /// <param name="Type">Sting used to select if the record 'Bundle' or 'Material'</param>
        /// <returns>Class object</returns>
        public QuoteBundleMaterial GetQuoteBasedMaterialBundle(int QuoteID, int? MaterialID, int? BundleID, string Type)
        {
            try
            {
                QuoteBundleMaterial oQuoteBundleMaterial = new QuoteBundleMaterial();
                decimal bTargetRate = GetBudgetTargetRate(QuoteID);
                var spQuoteBasedBundleMaterial = aQuoteEntities.spQuoteBasedMaterialBundleRecordSelect(Type, QuoteID, MaterialID == Convert.ToInt32(ConstantValues.Zero) ? null : MaterialID, BundleID == 0 ? null : BundleID).FirstOrDefault();
                if (spQuoteBasedBundleMaterial != null)
                {
                    oQuoteBundleMaterial.Quantity = spQuoteBasedBundleMaterial.Quantity.Value;
                    oQuoteBundleMaterial.Code = spQuoteBasedBundleMaterial.Code;
                    oQuoteBundleMaterial.Description = spQuoteBasedBundleMaterial.Discription;
                    oQuoteBundleMaterial.Discount = spQuoteBasedBundleMaterial.Discount.Value;
                    oQuoteBundleMaterial.Name = spQuoteBasedBundleMaterial.Name;
                    oQuoteBundleMaterial.NetPrice = Extension.MathRound(spQuoteBasedBundleMaterial.NetPrice.Value);
                    oQuoteBundleMaterial.Type = spQuoteBasedBundleMaterial.Type;
                    oQuoteBundleMaterial.UnitPrice = Extension.MathRound(spQuoteBasedBundleMaterial.UnitPrice.Value);
                    oQuoteBundleMaterial.Warrenty = spQuoteBasedBundleMaterial.Warrenty == null ? ConstantValues.Zero : Extension.MonthConverter(spQuoteBasedBundleMaterial.Warrenty.Value);
                    oQuoteBundleMaterial.OverRideWarrenty = spQuoteBasedBundleMaterial.OverrideWarrenty == null ? ConstantValues.Zero : Extension.MonthConverter(spQuoteBasedBundleMaterial.OverrideWarrenty.Value);
                }
                return oQuoteBundleMaterial;
            }
            catch
            {
                throw;
            }
        }

        public MaterialProperty GetMaterialSingleRecord(int MaterialID, decimal? BudgetTargetRate)
        {
            try
            {
                MaterialProperty mMaterialProperty = new MaterialProperty();
                var sqMaterialSelect = aQuoteEntities.MaterialSelect(MaterialID.ToString()).FirstOrDefault();
                if (sqMaterialSelect != null)
                {
                    mMaterialProperty.MaterialAmount = Extension.MathRound(sqMaterialSelect.Amount);
                    mMaterialProperty.MaterialCode = sqMaterialSelect.MaterialCode;
                    mMaterialProperty.MaterialDescription = sqMaterialSelect.MaterialDescription;
                    mMaterialProperty.MaterialDiscount = sqMaterialSelect.Discount.Value;
                    mMaterialProperty.MaterialId = sqMaterialSelect.MaterialID;
                    mMaterialProperty.MaterialName = sqMaterialSelect.MaterialName;
                }
                return mMaterialProperty;
            }
            catch
            {
                throw;
            }
        }

        public BundleProperty GetBundleSingleRecord(int BundleID, decimal? BudgetTargetRate)
        {
            try
            {
                BundleProperty bBundleProperty = new BundleProperty();
                var sqBundleSelect = aQuoteEntities.spBundleSelect(BundleID).FirstOrDefault();
                if (sqBundleSelect != null)
                {
                    bBundleProperty.BundleDescription = sqBundleSelect.BundleDescription;
                    bBundleProperty.BundleDiscount = sqBundleSelect.Discount.Value;
                    bBundleProperty.BundleId = sqBundleSelect.BundleID;
                    bBundleProperty.BundleName = sqBundleSelect.BundleName;
                    bBundleProperty.BundleNetPrice = Extension.MathRound(sqBundleSelect.NetPrice.Value);
                    bBundleProperty.BundleUnitPrice = Extension.MathRound(sqBundleSelect.UnitPrice.Value);
                }
                return bBundleProperty;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Quote Details
        /// </summary>
        /// <param name="QuoteID">Quote id integer</param>
        /// <returns></returns>
        public AgileQuoteProperty.Model.Quote GetQuoteDetails(int QuoteID, int UserID)
        {
            try
            {
                AgileQuoteProperty.Model.Quote qQuote = new AgileQuoteProperty.Model.Quote();
                qQuote.SalesOrg = LoadSalesOrg();
                qQuote.Currency = LoadCurrency();
                qQuote.StatusList = LoadQuoteStatus();
                var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oQuote != null)
                {
                    qQuote.CreatedBy = oQuote.CreatedBy;
                    qQuote.QuoteID = QuoteID;
                    qQuote.CustomerCode = oQuote.ReferenceCustomerCode;
                    qQuote.defaultSalesOrg = oQuote.ReferenceCustomerCode;
                    qQuote.CustomerName = oQuote.CustomerName;
                    qQuote.QuoteName = oQuote.QuoteName;
                    qQuote.qQuoteCurrencyName = oQuote.CurrencyName;
                    qQuote.qQuoteSalesOrgName = oQuote.SalesOrganizationName;
                    var oCurrencyValue = aQuoteEntities.tbCurrencies.Where(x => x.CurrencyName == oQuote.CurrencyName).FirstOrDefault();
                    if (oCurrencyValue != null)
                    {
                        qQuote.defaultCurrency = oCurrencyValue.Amount.ToString();
                    }
                    qQuote.RequrestedDate = Extension.DateFormat(oQuote.RequiredDate.Value);
                    qQuote.PreparedBy = oQuote.PreparedBy;
                    qQuote.defaultStatus = oQuote.QuoteStatus;
                    qQuote.BudgetValue = oQuote.BudgetRateTarget.Value;
                    qQuote.CreateDate = oQuote.CreatedDate.ToString();
                }

                var oQuoteAddress = aQuoteAdminEntities.tbAddresses.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).ToList();

                if (oQuoteAddress != null)
                {
                    foreach (var item in oQuoteAddress)
                    {
                        if (item.AddressType == ConstantValues.Billing)
                        {
                            qQuote.BillingAddressOne = item.AddressOne;
                            qQuote.BillingAddressTwo = item.AddressTwo;
                            qQuote.BillingCity = item.City;
                            qQuote.BillingState = item.State;
                            qQuote.BillingCountry = item.Country;
                            qQuote.BillingPostalCode = item.PostalCode;
                            qQuote.BillingPhoneNumber = item.PhoneNumber;
                            qQuote.BillingMobileNumber = item.MobileNumber;
                        }
                        else
                        {
                            qQuote.ShippingAddressOne = item.AddressOne;
                            qQuote.ShippingAddressTwo = item.AddressTwo;
                            qQuote.ShippingCity = item.City;
                            qQuote.ShippingState = item.State;
                            qQuote.ShippingCountry = item.Country;
                            qQuote.ShippingPostalCode = item.PostalCode;
                            qQuote.ShippingPhoneNumber = item.PhoneNumber;
                            qQuote.ShippingMobileNumber = item.MobileNumber;
                        }
                    }
                }

                var oQuoteCollabrator = aQuoteEntities.tbQuoteCollabrators.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).ToList();
                List<int> oCollabratorList = new List<int>();
                if (oQuoteCollabrator.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    foreach (var item in oQuoteCollabrator)
                    {
                        oCollabratorList.Add(item.ColabratorID);
                    }
                }

                if (qQuote.CreatedBy == UserID)
                {
                    qQuote.qCreateBy = true;
                }
                else
                {
                    qQuote.qCreateBy = false;
                }
                return qQuote;
            }
            catch
            {
                throw;
            }
        }
        public AgileQuoteProperty.Model.QuoteQualitativeInformation GetQuoteQualitativeInformationDetails(int QuoteID)
        {
            try
            {
                AgileQuoteProperty.Model.QuoteQualitativeInformation uQuoteQualitativeInformation = new AgileQuoteProperty.Model.QuoteQualitativeInformation();

                var QualitativeInformation = aQuoteEntities.tbQuoteQualitativeInformations.Where(x => x.QuoteID == QuoteID).FirstOrDefault();

                if (QualitativeInformation != null)
                {
                    uQuoteQualitativeInformation.Business = QualitativeInformation.NewRepeatBusiness;
                    uQuoteQualitativeInformation.txtareaAnyOtherComments = QualitativeInformation.AnyOtherComments;
                    uQuoteQualitativeInformation.txtareaExecutiveSummary = QualitativeInformation.ExecutiveSummary;
                    uQuoteQualitativeInformation.txtareaPaymentTerms = QualitativeInformation.PaymentTerms;
                    uQuoteQualitativeInformation.txtareaPrimaryCompetitor = QualitativeInformation.PrimaryCompetitor;
                    uQuoteQualitativeInformation.txtareaRiskMitigation = QualitativeInformation.RiskMitigation;
                    uQuoteQualitativeInformation.txtareaScopeofWork = QualitativeInformation.ScopeofWork;
                    uQuoteQualitativeInformation.txtareaSellingPrice = QualitativeInformation.SellingPrice;
                    uQuoteQualitativeInformation.txtLeadTime = QualitativeInformation.Leadtime;
                    uQuoteQualitativeInformation.txtWinProbability = QualitativeInformation.WinProbability;
                }

                return uQuoteQualitativeInformation;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Get material list based on category method returns collection of material based on category name
        /// </summary>
        /// <param name="CategoryName">Category name string</param>
        /// <returns>List of material property</returns>
        public List<MaterialProperty> GetMaterialListBasedOnCategory(string CategoryName, decimal? BudgetTargetRate)
        {
            List<MaterialProperty> oMaterialList = new List<MaterialProperty>();
            MaterialProperty oMaterial = null;
            int Sno = Convert.ToInt32(ConstantValues.One);
            try
            {
                var oMaterialCategoryBase = aQuoteEntities.spMaterialCategoryBaseSelect(CategoryName).ToList();
                if (oMaterialCategoryBase != null)
                {
                    foreach (var item in oMaterialCategoryBase)
                    {
                        oMaterial = new MaterialProperty();
                        oMaterial.MaterialAmount = Extension.MathRound(item.Amount);
                        oMaterial.MaterialCode = item.MaterialCode;
                        oMaterial.MaterialDescription = item.MaterialDescription;
                        oMaterial.MaterialDiscount = item.Discount.Value;
                        oMaterial.MaterialId = item.MaterialID;
                        oMaterial.MaterialName = item.MaterialName;
                        oMaterial.Quantity = ConstantValues.DefaultQuntity;
                        oMaterial.sno = Sno;
                        oMaterialList.Add(oMaterial);
                        Sno++;
                    }
                }
                return oMaterialList;
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> GetSugectionMaterial(int MaterialId)
        {
            try
            {
                List<int> oList = aQuoteEntities.SugectionMaterialMappings.Where(x => x.MaterialId == MaterialId && x.IsActive == true && x.IsDelete == false).Select(x => x.MappringMaterialId.Value).ToList();
                return CommonGetMaterialList(oList);
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> GetOfferedMaterial(int MaterialId)
        {
            try
            {
                List<int> oList = aQuoteEntities.tbOfferedMaterialMappings.Where(x => x.MaterialId == MaterialId && x.IsActive == true && x.IsDelete == false).Select(x => x.MappingMaterialId.Value).ToList();
                return CommonGetMaterialList(oList);
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> CommonGetMaterialList(List<int> MaterialId)
        {
            List<MaterialProperty> oMaterialList = new List<MaterialProperty>();
            try
            {
                if (MaterialId.Count > 0)
                {
                    foreach (var oObj in MaterialId)
                    {
                        MaterialProperty oMaterial = new MaterialProperty();
                        var item = aQuoteEntities.tbMaterials.Where(x => x.MaterialID == oObj && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                        if (item != null)
                        {
                            oMaterial.MaterialAmount = Extension.MathRound(item.Amount);
                            oMaterial.MaterialCode = item.MaterialCode;
                            oMaterial.MaterialDescription = item.MaterialDescription;
                            oMaterial.MaterialDiscount = item.Discount.Value;
                            oMaterial.MaterialId = item.MaterialID;
                            oMaterial.MaterialName = item.MaterialName;
                            oMaterial.Quantity = ConstantValues.DefaultQuntity;
                            oMaterialList.Add(oMaterial);
                        }
                    }
                }
                return oMaterialList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetBundleListBasedOnCategory method returns collection of bundle property based on category name
        /// </summary>
        /// <param name="CategoryName">Category name string nullable</param>
        /// <returns>List of bundleproperty</returns>
        public List<BundleProperty> GetBundleListBasedOnCategory(string CategoryName, decimal? BudgetTargetRate)
        {
            List<BundleProperty> oBundleList = new List<BundleProperty>();
            BundleProperty oBundle = null;
            int Sno = 1;
            try
            {
                var oBundleCategoryBase = aQuoteEntities.spBundleCategoryBaseSelect(CategoryName).ToList();
                if (oBundleCategoryBase != null)
                {
                    foreach (var item in oBundleCategoryBase)
                    {
                        oBundle = new BundleProperty();
                        oBundle.BundleDescription = item.BundleDescription;
                        oBundle.BundleDiscount = item.Discount.Value;
                        oBundle.BundleId = item.BundleID;
                        oBundle.BundleName = item.BundleName;
                        oBundle.BundleNetPrice = Extension.MathRound(item.NetPrice.Value);
                        oBundle.BundleUnitPrice = Extension.MathRound(item.UnitPrice.Value);
                        oBundle.sno = Sno;
                        oBundle.Quantity = ConstantValues.DefaultQuntity;
                        oBundleList.Add(oBundle);
                        Sno++;
                    }
                }
                return oBundleList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get category name list
        /// </summary>
        /// <returns>Collection of string</returns>
        public List<string> GetCategoryNameList()
        {
            List<string> oCategoryName = new List<string>();
            try
            {
                var ooCategoryName = aQuoteEntities.tbCategories.Where(x => x.IsActive == true && x.IsDelete == false).ToList();
                if (ooCategoryName != null)
                {
                    foreach (var item in ooCategoryName)
                    {
                        oCategoryName.Add(item.CategoryName);
                    }
                }
                return oCategoryName;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update material and bundle quote based mapping table
        /// </summary>
        /// <param name="QuoteID">Quote id integer</param>
        /// <param name="Type">Sting (Bundle or material identify)</param>
        /// <param name="MaterialId">MaterialID integer nullable </param>
        /// <param name="BundleId">BundleID integer nullable</param>
        /// <param name="Quantity">Qunaity integer</param>
        /// <param name="NetPrice">Netprice decimal</param>
        /// <param name="Delete">Check Delete this record or not(boolian)</param>
        /// <returns>string</returns>
        public string UpdateMaterialBundleRecord(int QuoteID, string Type, int? MaterialId, int? BundleId, int? Quantity, decimal? NetPrice, bool Delete)
        {
            try
            {
                decimal bedgetTargetRate = GetBudgetTargetRate(QuoteID);
                if (Type == ConstantValues.BundleName)
                {
                    if (BundleId != null && BundleId != Convert.ToInt32(ConstantValues.Zero))
                    {
                        var oBundleRecord = (from o in aQuoteEntities.tbQuoteBundleMappings
                                             where
                                             o.QuoteID == QuoteID && o.BundleID == BundleId.Value && o.IsActive == true && o.IsDelete == false
                                             select o).FirstOrDefault();
                        if (oBundleRecord != null)
                        {
                            if (!Delete)
                            {
                                oBundleRecord.Quantity = Quantity.Value;
                                oBundleRecord.NetPrice = NetPrice.Value;
                            }
                            else
                            {
                                oBundleRecord.IsDelete = true;
                            }
                        }
                    }
                }
                else
                {
                    if (MaterialId != null && MaterialId != Convert.ToInt32(ConstantValues.Zero))
                    {
                        var oMaterialRecord = (from o in aQuoteEntities.tbQuoteMaterialMappings
                                               where
                                               o.QuoteID == QuoteID && o.MaterialID == MaterialId.Value && o.IsActive == true && o.IsDelete == false
                                               select o).FirstOrDefault();
                        if (oMaterialRecord != null)
                        {
                            if (!Delete)
                            {
                                oMaterialRecord.Quantity = Quantity.Value;
                                oMaterialRecord.NetPrice = NetPrice.Value;
                            }
                            else
                            {
                                oMaterialRecord.IsDelete = true;
                            }
                        }
                    }
                }
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public string UpdateBundleMaterialRecordDiscount(int QuoteID, string Type, int? MaterialID, int? BundleID, int Discount)
        {
            try
            {
                if (Type == ConstantValues.BundleName)
                {
                    if (BundleID != null && BundleID != Convert.ToInt32(ConstantValues.Zero))
                    {
                        var oBundleRecord = aQuoteEntities.tbQuoteBundleMappings.Where(x => x.QuoteID == QuoteID && x.BundleID == BundleID.Value && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                        if (oBundleRecord != null)
                        {
                            oBundleRecord.Discount = Discount;
                            oBundleRecord.NetPrice = (oBundleRecord.UnitPrice - (oBundleRecord.UnitPrice * Convert.ToDecimal((double)Discount / (double)100))) * oBundleRecord.Quantity;
                        }
                    }
                }
                else
                {
                    if (MaterialID != null && MaterialID != Convert.ToInt32(ConstantValues.Zero))
                    {
                        var oMaterialRecord = aQuoteEntities.tbQuoteMaterialMappings.Where(x => x.QuoteID == QuoteID && x.MaterialID == MaterialID.Value && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                        if (oMaterialRecord != null)
                        {
                            oMaterialRecord.Discount = Discount;
                            oMaterialRecord.NetPrice = (oMaterialRecord.UnitPrice - (oMaterialRecord.UnitPrice * Convert.ToDecimal((double)Discount / (double)100))) * oMaterialRecord.Quantity;
                        }
                    }
                }
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add material and bundle list to Quote
        /// </summary>
        /// <param name="QuoteID">Integer QuoteID</param>
        /// <param name="MaterialIDlist">Collection of material list</param>
        /// <param name="BundleIDlist">Collection of bundle list</param>
        /// <returns>string</returns>
        public string AddBundleMaterialRecordToQuote(int QuoteID, List<MaterialBundleValue> MaterialObject, List<MaterialBundleValue> BundleObject)
        {
            try
            {
                List<int> MaterialIDlist = new List<int>();
                List<int> BundleIDlist = new List<int>();
                List<int> qMaterialList = new List<int>();
                List<int> qBundleList = new List<int>();
                List<int> oMaterialList = new List<int>();
                List<int> oBundleList = new List<int>();

                if (MaterialObject.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    MaterialIDlist = MaterialObject.Select(x => x.Code).ToList();
                }

                if (BundleObject.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    BundleIDlist = BundleObject.Select(x => x.Code).ToList();
                }

                qMaterialList = (from o in aQuoteEntities.tbQuoteMaterialMappings
                                 where
                                 o.QuoteID == QuoteID && o.IsDelete == false && o.IsActive == true
                                 select new { Data = o.MaterialID }).Select(t => t.Data).ToList();

                qBundleList = (from o in aQuoteEntities.tbQuoteBundleMappings
                               where
                               o.QuoteID == QuoteID && o.IsDelete == false && o.IsActive == true
                               select new { Data = o.BundleID }).Select(t => t.Data).ToList();

                if (MaterialIDlist.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    if (qMaterialList.Count > Convert.ToInt32(ConstantValues.Zero))
                    {
                        oMaterialList = MaterialIDlist.Except(qMaterialList).ToList();
                    }
                    else
                    {
                        oMaterialList = MaterialIDlist;
                    }
                }


                if (oMaterialList.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    foreach (var item in oMaterialList)
                    {
                        var oMaterialDetail = aQuoteEntities.MaterialSelect(item.ToString()).FirstOrDefault();
                        if (oMaterialDetail != null)
                        {
                            tbQuoteMaterialMapping otbQuoteMaterialMapping = new tbQuoteMaterialMapping();
                            otbQuoteMaterialMapping.Discount = oMaterialDetail.Discount;
                            otbQuoteMaterialMapping.IsActive = true;
                            otbQuoteMaterialMapping.IsDelete = false;
                            otbQuoteMaterialMapping.MaterialID = item;
                            otbQuoteMaterialMapping.NetPrice = (oMaterialDetail.Amount - (oMaterialDetail.Amount * Convert.ToDecimal((double)oMaterialDetail.Discount / (double)100))) * MaterialObject.Where(x => x.Code == item).Select(x => x.Quantity).FirstOrDefault();
                            otbQuoteMaterialMapping.Quantity = MaterialObject.Where(x => x.Code == item).Select(x => x.Quantity).FirstOrDefault();
                            otbQuoteMaterialMapping.QuoteID = QuoteID;
                            otbQuoteMaterialMapping.UnitPrice = oMaterialDetail.Amount;
                            otbQuoteMaterialMapping.Warrenty = oMaterialDetail.Warranty;
                            aQuoteEntities.tbQuoteMaterialMappings.Add(otbQuoteMaterialMapping);
                        }
                    }
                }

                if (BundleIDlist.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    if (qBundleList.Count > Convert.ToInt32(ConstantValues.Zero))
                    {
                        oBundleList = BundleIDlist.Except(qBundleList).ToList();
                    }
                    else
                    {
                        oBundleList = BundleIDlist;
                    }
                }


                if (oBundleList.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    foreach (var item in oBundleList)
                    {
                        var oBundleDetail = aQuoteEntities.spBundleSelect(item).FirstOrDefault();
                        if (oBundleDetail != null)
                        {
                            tbQuoteBundleMapping otbQuoteBundleMapping = new tbQuoteBundleMapping();
                            otbQuoteBundleMapping.BundleID = item;
                            otbQuoteBundleMapping.Discount = oBundleDetail.Discount;
                            otbQuoteBundleMapping.IsActive = true;
                            otbQuoteBundleMapping.IsDelete = false;
                            otbQuoteBundleMapping.NetPrice = (oBundleDetail.UnitPrice - (oBundleDetail.UnitPrice * Convert.ToDecimal((double)oBundleDetail.Discount / (double)100))) * BundleObject.Where(x => x.Code == item).Select(x => x.Quantity).FirstOrDefault();
                            otbQuoteBundleMapping.Quantity = BundleObject.Where(x => x.Code == item).Select(x => x.Quantity).FirstOrDefault();
                            otbQuoteBundleMapping.QuoteID = QuoteID;
                            otbQuoteBundleMapping.UnitPrice = oBundleDetail.UnitPrice;
                            otbQuoteBundleMapping.Warrent = oBundleDetail.Warrenty;
                            aQuoteEntities.tbQuoteBundleMappings.Add(otbQuoteBundleMapping);
                        }
                    }
                }

                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get currency symbol
        /// </summary>
        /// <param name="QuoteID">Integer quoteID</param>
        /// <param name="CurrencyName">string currency Name</param>
        /// <returns>string of symbol</returns>
        public string GetSymbol(int? QuoteID, string CurrencyName)
        {
            try
            {
                string Symbol;
                string CultureName = null;
                string Name = null;
                if (QuoteID != null && QuoteID != Convert.ToInt32(ConstantValues.Zero))
                {
                    var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID.Value).Select(x => x.CurrencyName).FirstOrDefault();
                    if (oQuote != null)
                    {
                        Name = oQuote;
                    }
                }
                else
                {
                    Name = CurrencyName;
                }

                if (Name == ConstantValues.Dollar)
                {
                    CultureName = ConstantValues.DollarCode;
                }
                else if (Name == ConstantValues.Pound)
                {
                    CultureName = ConstantValues.PoundCode;
                }
                else if (Name == ConstantValues.JapaniseYen)
                {
                    CultureName = ConstantValues.JapaniseYenCode;
                }
                else if (Name == ConstantValues.IndianRupee)
                {
                    CultureName = ConstantValues.IndianRupeeCode;
                }

                RegionInfo myRI = new RegionInfo(CultureName);
                Symbol = String.Format("{0}", myRI.CurrencySymbol);
                return Symbol;
            }
            catch
            {
                throw;
            }
        }

        public string AssignedApproverToQuote(int QuoteID)
        {
            try
            {
                List<string> ExistRole = new List<string>();
                List<string> Roles = new List<string>();
                List<int> AthorizedUserList = new List<int>();
                List<int> ExistUserList = new List<int>();

                var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oQuote != null)
                {
                    if (oQuote.NetPrice == null)
                    {
                        return ConstantValues.SuccessMessage;
                    }

                    List<string> RoleNameList = RoleNameListSelect(oQuote.NetPrice.Value);
                    var oQuoteAuthorized = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.IsDelete == false && x.IsActive == true).ToList();
                    if (oQuoteAuthorized.Count > ConstantValues.Zero)
                    {
                        foreach (var aItem in oQuoteAuthorized)
                        {
                            var oRoleName = aQuoteAdminEntities.tbCustomerDetails.Where(x => x.UserID == aItem.AuthorizedUserId && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                            if (oRoleName != null)
                            {
                                ExistRole.Add(oRoleName.RoleName.Trim());
                            }
                        }
                    }

                    ExistUserList.Add(oQuote.CreatedBy);

                    var oQuoteCollabrator = aQuoteEntities.tbQuoteCollabrators.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).ToList();
                    if (oQuoteCollabrator.Count > ConstantValues.Zero)
                    {
                        foreach (var cItem in oQuoteCollabrator)
                        {
                            if (cItem.Status != ConstantValues.QuoteCollaborationCompleted)
                            {
                                return ConstantValues.ErrorCollaboratorAssign;
                            }

                            ExistUserList.Add(cItem.ColabratorID);
                            var oRoleName = aQuoteAdminEntities.tbCustomerDetails.Where(x => x.UserID == cItem.ColabratorID && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                            if (oRoleName != null)
                            {
                                ExistRole.Add(oRoleName.RoleName.Trim());
                            }
                        }
                    }

                    if (ExistRole.Count > ConstantValues.Zero)
                    {
                        Roles = RoleNameList.Except(ExistRole).ToList();
                    }
                    else
                    {
                        Roles = RoleNameList;
                    }

                    foreach (var rRole in Roles)
                    {
                        int userID = AuthorizedUserID(rRole, oQuote.CreatedBy, QuoteID, ExistUserList);
                        if (userID != ConstantValues.Zero)
                        {
                            AthorizedUserList.Add(userID);
                        }
                    }

                    InsertAuthorizedUserToQuote(AthorizedUserList, QuoteID);

                }
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public int AuthorizedUserID(string RoleName, int CreatedBy, int QuoteID, List<int> ExistUserList)
        {
            try
            {
                int UserId = Convert.ToInt32(ConstantValues.Zero);
                Dictionary<int, int> ApproveUser = new Dictionary<int, int>();
                var oCompany = aQuoteAdminEntities.tbCustomerDetails.Where(x => x.UserID == CreatedBy && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oCompany != null)
                {
                    var oUserList = aQuoteAdminEntities.tbCustomerDetails.Where(x => x.RoleName == RoleName && x.IsActive == true && x.IsDelete == false && x.CompanyCode == oCompany.CompanyCode).ToList();
                    if (oUserList.Count > ConstantValues.Zero)
                    {
                        foreach (var item in oUserList)
                        {
                            var rResult = ExistUserList.Contains(item.UserID);
                            if (!rResult)
                            {
                                var oAthorizedList = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.AuthorizedUserId == item.UserID && x.IsActive == true && x.IsDelete == false && x.Status != "Authorized").ToList();
                                if (oAthorizedList.Count > ConstantValues.Zero)
                                {
                                    ApproveUser.Add(item.UserID, oAthorizedList.Count);
                                }
                                else
                                {
                                    ApproveUser.Add(item.UserID, Convert.ToInt32(ConstantValues.Zero));
                                }
                            }
                        }
                        var oUserID = ApproveUser.OrderBy(x => x.Value).FirstOrDefault();
                        UserId = oUserID.Key;
                    }
                }
                return UserId;
            }
            catch
            {
                throw;
            }
        }

        public List<string> RoleNameListSelect(decimal NetPrice)
        {
            try
            {
                List<string> oRoleNameList = new List<string>();

                var oRoleList = aQuoteEntities.tbApproverRules.Where(x => x.MinimumAmount <= NetPrice && x.MaximumAmount >= NetPrice && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oRoleList != null)
                {
                    var oRoles = oRoleList.RoleId;
                    var oRolesList = oRoles.Split(',');
                    foreach (var item in oRolesList)
                    {
                        int oRoleId = Convert.ToInt32(item);
                        var oRole = aQuoteAdminEntities.tbCustomerRoles.Where(x => x.RoleID == oRoleId && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                        if (oRole != null)
                        {
                            oRoleNameList.Add(oRole.RoleName.Trim());
                        }
                    }
                }

                return oRoleNameList;
            }
            catch
            {
                throw;
            }
        }

        public void InsertAuthorizedUserToQuote(List<int> oAthorizedUserList, int QuoteID)
        {
            try
            {
                tbQuoteAuthorize oQuoteAuthorize = null;
                foreach (var item in oAthorizedUserList)
                {
                    oQuoteAuthorize = new tbQuoteAuthorize();
                    oQuoteAuthorize.AuthorizedUserId = item;
                    oQuoteAuthorize.IsActive = true;
                    oQuoteAuthorize.IsDelete = false;
                    oQuoteAuthorize.QuoteID = QuoteID;
                    oQuoteAuthorize.Status = ConstantValues.QuotePendingApproval;
                    aQuoteEntities.tbQuoteAuthorizes.Add(oQuoteAuthorize);
                }
                aQuoteEntities.SaveChanges();

            }
            catch
            {
                throw;
            }
        }

        public string ApprovedQuote(int QuoteID, int UserID, string Description)
        {
            try
            {
                var oApproveQuote = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.AuthorizedUserId == UserID && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                if (oApproveQuote != null)
                {
                    oApproveQuote.Status = ConstantValues.QuoteApproveStatus;
                    oApproveQuote.Description = Description;
                    aQuoteEntities.SaveChanges();
                }
                AuthorizedQuote(QuoteID);
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public string RejectQuote(int QuoteID, int UserID, string Description)
        {
            try
            {
                var oApproveQuote = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.AuthorizedUserId == UserID && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                if (oApproveQuote != null)
                {
                    oApproveQuote.Status = ConstantValues.QuoteRejectStatus;
                    oApproveQuote.Description = Description;
                    aQuoteEntities.SaveChanges();
                }
                AuthorizedQuote(QuoteID);
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public string RejectCreatorQuote(int QuoteID, int UserID)
        {
            try
            {
                var oApproveQuote = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.IsDelete == false && x.IsActive == true).ToList();
                if (oApproveQuote.Count > ConstantValues.Zero)
                {
                    List<string> Status = oApproveQuote.Select(x => x.Status).ToList();
                    if (Status.Contains(ConstantValues.QuotePendingApproval))
                    {
                        return ConstantValues.ErrorRejectQuoteMessage;
                    }
                }

                var oCollabrator = aQuoteEntities.tbQuoteCollabrators.Where(x => x.QuoteID == QuoteID && x.IsDelete == false && x.IsActive == true).ToList();
                if (oCollabrator.Count > ConstantValues.Zero)
                {
                    List<string> Status = oCollabrator.Select(x => x.Status).ToList();
                    if (Status.Contains(ConstantValues.QuoteWaitingCollaborate))
                    {
                        return ConstantValues.ErrorRejectQuoteMessage;
                    }
                }

                var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                if (oQuote != null)
                {
                    oQuote.IsActive = false;
                    oQuote.IsDelete = true;
                    oQuote.DeletedBy = UserID;
                    oQuote.DeletedDate = System.DateTime.Now;
                    aQuoteEntities.SaveChanges();
                    return ConstantValues.SuccessMessage;
                }
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public void AuthorizedQuote(int QuoteID)
        {
            try
            {
                string Status = string.Empty;
                if (aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).Any(x => x.Status == ConstantValues.QuoteRejectStatus))
                {
                    Status = ConstantValues.QuoteRejectStatus;
                }
                else
                {
                    if (aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).Any(x => x.Status == ConstantValues.QuotePendingApproval))
                    {
                        Status = ConstantValues.QuotePendingApproval;
                    }
                    else
                    {
                        Status = ConstantValues.QuoteApproveStatus;
                    }
                }



                var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oQuote != null)
                {
                    oQuote.QuoteStatus = Status;
                    aQuoteEntities.SaveChanges();
                }

            }
            catch
            {
                throw;
            }
        }

        public string InsertShippingToQuote(InstallCharges oShippingCharges, int QuoteID)
        {
            try
            {
                if (oShippingCharges != null)
                {
                    decimal budgetPrice = GetBudgetTargetRate(QuoteID);

                    tbQuoteService otbQuoteService = new tbQuoteService();
                    otbQuoteService.IsActive = true;
                    otbQuoteService.IsDelete = false;
                    otbQuoteService.NoDays = oShippingCharges.NoDays;
                    otbQuoteService.NoEmp = oShippingCharges.NoEmployees;
                    otbQuoteService.PerdayCost = Extension.StringToDecimal(oShippingCharges.CostPerDay);
                    otbQuoteService.QuoteID = QuoteID;
                    otbQuoteService.TotalCost = Extension.StringToDecimal(oShippingCharges.TotalCost);
                    aQuoteEntities.tbQuoteServices.Add(otbQuoteService);
                    aQuoteEntities.SaveChanges();
                }
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }


        public string InsertQualitativeInformationDL(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {
            var RecordFound = aQuoteEntities.tbQuoteQualitativeInformations.Where(x => x.QuoteID == QuoteID).Any();
            if (!RecordFound)
            {
                tbQuoteQualitativeInformation otbQuoteQualitativeInformation = new tbQuoteQualitativeInformation();
                otbQuoteQualitativeInformation.QuoteID = QuoteID;
                otbQuoteQualitativeInformation.Leadtime = Leadtime;
                otbQuoteQualitativeInformation.WinProbability = WinProbability;
                otbQuoteQualitativeInformation.ScopeofWork = ScopeofWork;
                otbQuoteQualitativeInformation.ExecutiveSummary = ExecutiveSummary;
                otbQuoteQualitativeInformation.PrimaryCompetitor = PrimaryCompetitor;
                otbQuoteQualitativeInformation.SellingPrice = SellingPrice;
                otbQuoteQualitativeInformation.PaymentTerms = PaymentTerms;
                otbQuoteQualitativeInformation.RiskMitigation = RiskMitigation;
                otbQuoteQualitativeInformation.NewRepeatBusiness = NewRepeatBusiness;
                otbQuoteQualitativeInformation.AnyOtherComments = AnyOtherComments;
                aQuoteEntities.tbQuoteQualitativeInformations.Add(otbQuoteQualitativeInformation);
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            else
            {
                return InsertQualitativeInformationUpdateDL(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);
            }
        }

        public string InsertQualitativeInformationUpdateDL(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {
            var updateQI = aQuoteEntities.tbQuoteQualitativeInformations.Where(x => x.QuoteID == QuoteID).FirstOrDefault();
            if (updateQI != null)
            {
                updateQI.QuoteID = QuoteID;
                updateQI.Leadtime = Leadtime;
                updateQI.WinProbability = WinProbability;
                updateQI.ScopeofWork = ScopeofWork;
                updateQI.ExecutiveSummary = ExecutiveSummary;
                updateQI.PrimaryCompetitor = PrimaryCompetitor;
                updateQI.SellingPrice = SellingPrice;
                updateQI.PaymentTerms = PaymentTerms;
                updateQI.RiskMitigation = RiskMitigation;
                updateQI.NewRepeatBusiness = NewRepeatBusiness;
                updateQI.AnyOtherComments = AnyOtherComments;
                aQuoteEntities.SaveChanges();
            }
            else
            {
                InsertQualitativeInformationDL(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);
            }
            return ConstantValues.SuccessMessage;
        }

        public System.Globalization.NumberFormatInfo GetFormatInfo(int QuoteID)
        {
            try
            {
                System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                nfi.CurrencySymbol = GetSymbol(QuoteID, null);
                nfi.CurrencyDecimalDigits = Convert.ToInt32(ConstantValues.Zero);
                return nfi;
            }
            catch
            {
                throw;
            }
        }

        public InstallCharges GetQuoteShippingDetails(int QuoteID)
        {
            try
            {

                System.Globalization.NumberFormatInfo nfi = GetFormatInfo(QuoteID);
                decimal budgetPrice = GetBudgetTargetRate(QuoteID);

                InstallCharges sDetails = new InstallCharges();
                var oShipObject = aQuoteEntities.tbQuoteServices.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oShipObject != null)
                {
                    sDetails.TotalCost = Extension.FormatCurrency(nfi, Extension.MathRound(oShipObject.TotalCost));
                    sDetails.NoEmployees = oShipObject.NoEmp;
                    sDetails.NoDays = oShipObject.NoDays;
                    sDetails.CostPerDay = Extension.FormatCurrency(nfi, Extension.MathRound(oShipObject.PerdayCost));
                }
                return sDetails;
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteBundleMaterial> GetQuoteMaterialBundleListwithwarranty(int QuoteID)
        {
            try
            {
                List<QuoteBundleMaterial> oQuoteBundleMaterialList = new List<QuoteBundleMaterial>();
                QuoteBundleMaterial oQuoteBundleMaterial = null;
                var oBundleMaterial = aQuoteEntities.spQuoteBasedMaterialBundleWithoutConfigured(QuoteID).ToList();
                decimal oBundgetTargetRate = GetBudgetTargetRate(QuoteID);
                if (oBundleMaterial != null)
                {
                    foreach (var item in oBundleMaterial)
                    {
                        oQuoteBundleMaterial = new QuoteBundleMaterial();
                        oQuoteBundleMaterial.Quantity = item.Quantity.Value;
                        oQuoteBundleMaterial.Code = item.Code;
                        oQuoteBundleMaterial.Description = item.Discription;
                        oQuoteBundleMaterial.Discount = item.Discount.Value;
                        oQuoteBundleMaterial.Name = item.Name;
                        oQuoteBundleMaterial.NetPrice = Extension.MathRound(item.NetPrice.Value);
                        oQuoteBundleMaterial.GrossPrice = Extension.MathRound(item.UnitPrice.Value * item.Quantity.Value);
                        oQuoteBundleMaterial.UnitPrice = Extension.MathRound(item.UnitPrice.Value);
                        oQuoteBundleMaterial.Type = item.Type;
                        oQuoteBundleMaterial.Warrenty = item.Warrenty == null ? ConstantValues.Zero : Extension.MonthConverter(item.Warrenty.Value);
                        oQuoteBundleMaterial.OverRideWarrenty = item.OverrideWarrenty == null ? ConstantValues.Zero : Extension.MonthConverter(item.OverrideWarrenty.Value);
                        if (oQuoteBundleMaterial.OverRideWarrenty > ConstantValues.Zero)
                        {
                            oQuoteBundleMaterial.Warrenty = oQuoteBundleMaterial.OverRideWarrenty;
                        }
                        oQuoteBundleMaterialList.Add(oQuoteBundleMaterial);
                    }
                }

                return oQuoteBundleMaterialList;
            }
            catch
            {
                throw;
            }
        }

        public string UpdateMaterialBundleWarrentyRecord(int QuoteID, string Type, int? MaterialId, int? BundleId, int OverrideWarrenty)
        {
            try
            {
                decimal bedgetTargetRate = GetBudgetTargetRate(QuoteID);
                if (Type == ConstantValues.BundleName)
                {
                    if (BundleId != null && BundleId != Convert.ToInt32(ConstantValues.Zero))
                    {
                        var oBundleRecord = (from o in aQuoteEntities.tbQuoteBundleMappings
                                             where
                                             o.QuoteID == QuoteID && o.BundleID == BundleId.Value && o.IsActive == true && o.IsDelete == false
                                             select o).FirstOrDefault();
                        if (oBundleRecord != null)
                        {
                            oBundleRecord.OverrideWarrent = Extension.YearConverter(OverrideWarrenty);
                        }
                    }
                }
                else
                {
                    if (MaterialId != null && MaterialId != Convert.ToInt32(ConstantValues.Zero))
                    {
                        var oMaterialRecord = (from o in aQuoteEntities.tbQuoteMaterialMappings
                                               where
                                               o.QuoteID == QuoteID && o.MaterialID == MaterialId.Value && o.IsActive == true && o.IsDelete == false
                                               select o).FirstOrDefault();
                        if (oMaterialRecord != null)
                        {
                            oMaterialRecord.OverrideWarrenty = Extension.YearConverter(OverrideWarrenty);
                        }
                    }
                }
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public string InsertQuoteBoughtOutItem(int QuoteID, string ItemName, decimal UnitPrice, int Quantity)
        {
            try
            {
                decimal budgetPrice = GetBudgetTargetRate(QuoteID);
                decimal TotalCost = UnitPrice * Quantity;
                decimal quoteUnitPrice = UnitPrice + (UnitPrice * ConstantValues.Ten / ConstantValues.Hundred);
                decimal quoteTotalCost = quoteUnitPrice * Quantity;

                tbQuoteBoughtOutItem otbQuoteBoughtOutItem = new tbQuoteBoughtOutItem();
                otbQuoteBoughtOutItem.boughtItem = ItemName;
                otbQuoteBoughtOutItem.IsActive = true;
                otbQuoteBoughtOutItem.IsDelete = false;
                otbQuoteBoughtOutItem.Quantity = Quantity;
                otbQuoteBoughtOutItem.QuoteID = QuoteID;
                otbQuoteBoughtOutItem.UnitPrice = UnitPrice;
                otbQuoteBoughtOutItem.TotalCost = TotalCost;
                otbQuoteBoughtOutItem.UnitPriceQuoted = quoteUnitPrice;
                otbQuoteBoughtOutItem.TotalPriceQuoted = quoteTotalCost;
                aQuoteEntities.tbQuoteBoughtOutItems.Add(otbQuoteBoughtOutItem);
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteBoughtOutItemModel> GetQuoteBasedBoughtoutItemList(int QuoteID)
        {
            try
            {
                List<QuoteBoughtOutItemModel> oBoughtoutItemlist = new List<QuoteBoughtOutItemModel>();
                QuoteBoughtOutItemModel oBoughtOutItem = null;
                decimal budgetPrice = GetBudgetTargetRate(QuoteID);
                var oList = aQuoteEntities.tbQuoteBoughtOutItems.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).ToList();
                if (oList.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    foreach (var item in oList)
                    {
                        oBoughtOutItem = new QuoteBoughtOutItemModel();
                        oBoughtOutItem.ItemId = item.ItemID;
                        oBoughtOutItem.ItemName = item.boughtItem;
                        oBoughtOutItem.NetPrice = Extension.MathRound(item.TotalCost);
                        oBoughtOutItem.Quantity = item.Quantity;
                        oBoughtOutItem.quotedNetPrice = Extension.MathRound(item.TotalPriceQuoted);
                        oBoughtOutItem.quotedUnitPrice = Extension.MathRound(item.UnitPriceQuoted);
                        oBoughtOutItem.UnitPrice = Extension.MathRound(item.UnitPrice);
                        oBoughtoutItemlist.Add(oBoughtOutItem);
                    }
                }
                return oBoughtoutItemlist;
            }
            catch
            {
                throw;
            }
        }

        public QuoteBoughtOutItemModel GetQuoteBasedBoughtoutItemRecord(int QuoteID, int ItemID)
        {
            try
            {
                QuoteBoughtOutItemModel oQuoteBoughtoutItemModel = new QuoteBoughtOutItemModel();
                decimal budgetPrice = GetBudgetTargetRate(QuoteID);
                var oModel = aQuoteEntities.tbQuoteBoughtOutItems.Where(x => x.QuoteID == QuoteID && x.ItemID == ItemID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oModel != null)
                {
                    oQuoteBoughtoutItemModel.ItemId = oModel.ItemID;
                    oQuoteBoughtoutItemModel.ItemName = oModel.boughtItem;
                    oQuoteBoughtoutItemModel.NetPrice = Extension.MathRound(oModel.TotalCost);
                    oQuoteBoughtoutItemModel.Quantity = oModel.Quantity;
                    oQuoteBoughtoutItemModel.quotedNetPrice = Extension.MathRound(oModel.TotalPriceQuoted);
                    oQuoteBoughtoutItemModel.quotedUnitPrice = Extension.MathRound(oModel.UnitPriceQuoted);
                    oQuoteBoughtoutItemModel.UnitPrice = Extension.MathRound(oModel.UnitPrice);
                }
                return oQuoteBoughtoutItemModel;
            }
            catch
            {
                throw;
            }
        }

        public string UpdateQuoteBasedBoughtoutItemRecord(int QuoteID, decimal UnitPrice, int Quantity, int ItemId)
        {
            try
            {
                decimal budgetPrice = GetBudgetTargetRate(QuoteID);
                decimal TotalCost = UnitPrice * Quantity;
                decimal quoteUnitPrice = UnitPrice + (UnitPrice * ConstantValues.Ten / ConstantValues.Hundred);
                decimal quoteTotalCost = quoteUnitPrice * Quantity;

                var oModel = aQuoteEntities.tbQuoteBoughtOutItems.Where(x => x.QuoteID == QuoteID && x.ItemID == ItemId && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oModel != null)
                {
                    oModel.UnitPrice = UnitPrice;
                    oModel.TotalCost = TotalCost;
                    oModel.UnitPriceQuoted = quoteUnitPrice;
                    oModel.TotalPriceQuoted = quoteTotalCost;
                    oModel.Quantity = Quantity;
                    aQuoteEntities.SaveChanges();
                }
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public string DeleteQuoteBasedBoughtoutItemRecord(int QuoteID, int ItemId)
        {
            try
            {
                var oModel = aQuoteEntities.tbQuoteBoughtOutItems.Where(x => x.QuoteID == QuoteID && x.ItemID == ItemId && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oModel != null)
                {
                    oModel.IsDelete = true;
                    oModel.IsActive = false;
                    aQuoteEntities.SaveChanges();
                }
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public TotalUnitDiscount GetTotalCostBoughtoutItem(int QuoteID)
        {
            try
            {
                decimal unitPrice = ConstantValues.Zero;
                decimal netPrice = ConstantValues.Zero;
                int quantity = Convert.ToInt32(ConstantValues.Zero);
                decimal qUnitPrice = ConstantValues.Zero;
                decimal qNetPrice = ConstantValues.Zero;
                System.Globalization.NumberFormatInfo nfi = GetFormatInfo(QuoteID);

                TotalUnitDiscount oTotalUnitDiscount = new TotalUnitDiscount();
                var bList = GetQuoteBasedBoughtoutItemList(QuoteID);
                if (bList.Count > ConstantValues.Zero)
                {
                    foreach (var item in bList)
                    {
                        unitPrice = unitPrice + item.UnitPrice;
                        netPrice = netPrice + item.NetPrice;
                        quantity = quantity + item.Quantity;
                        qUnitPrice = qUnitPrice + item.quotedUnitPrice;
                        qNetPrice = qNetPrice + item.quotedNetPrice;
                    }
                    oTotalUnitDiscount.TotalUnitPrice = Extension.FormatCurrency(nfi, Extension.MathRound(unitPrice));
                    oTotalUnitDiscount.TotalNetPrice = Extension.FormatCurrency(nfi, Extension.MathRound(netPrice));
                    oTotalUnitDiscount.TotalQuantity = quantity;
                    oTotalUnitDiscount.TotalQuotedUnitPrice = Extension.FormatCurrency(nfi, Extension.MathRound(qUnitPrice));
                    oTotalUnitDiscount.TotalQuotedNetPrice = Extension.FormatCurrency(nfi, Extension.MathRound(qNetPrice));
                }
                return oTotalUnitDiscount;
            }
            catch
            {
                throw;
            }
        }

        public List<string> GetRoleNameList()
        {
            try
            {
                List<string> oRoleNameList = new List<string>();
                var rList = aQuoteAdminEntities.tbCustomerRoles.Where(x => x.IsActive == true && x.IsDelete == false).ToList();
                if (rList.Count > ConstantValues.Zero)
                {
                    foreach (var item in rList)
                    {
                        if (!item.RoleName.Contains(ConstantValues.salesVP))
                        {
                            oRoleNameList.Add(item.RoleName);
                        }
                    }
                }
                return oRoleNameList;
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteCollabrationModel> GetcollabratorListOnQuote(int QuoteID)
        {
            try
            {
                List<QuoteCollabrationModel> oQuoteCollabrationList = new List<QuoteCollabrationModel>();
                QuoteCollabrationModel oQuoteCollabration = null;
                var cList = aQuoteEntities.tbQuoteCollabrators.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).ToList();
                if (cList.Count > ConstantValues.Zero)
                {
                    foreach (var item in cList)
                    {
                        oQuoteCollabration = new QuoteCollabrationModel();
                        oQuoteCollabration.UserID = item.ColabratorID;
                        oQuoteCollabration.Status = item.Status;
                        oQuoteCollabration.StatusDescription = item.Description;
                        oQuoteCollabrationList.Add(oQuoteCollabration);
                    }
                }
                return oQuoteCollabrationList;
            }
            catch
            {
                throw;
            }
        }

        public string InsertCollabratorQuote(int QuoteID, Dictionary<int, string> UserList)
        {
            try
            {
                List<int> ExistUserId = new List<int>();
                List<int> InsertUserId = new List<int>();
                List<int> UserID = UserList.Select(x => x.Key).ToList();

                var eList = GetcollabratorListOnQuote(QuoteID).ToList();
                if (eList.Count > ConstantValues.Zero)
                {
                    foreach (var item in eList)
                    {
                        ExistUserId.Add(item.UserID);
                    }

                    if (ExistUserId.Count > ConstantValues.Zero)
                    {
                        InsertUserId = UserID.Except(ExistUserId).ToList();
                    }
                }
                else
                {
                    InsertUserId = UserID;
                }

                if (InsertUserId.Count > ConstantValues.Zero)
                {
                    tbQuoteCollabrator oQuotecollabrator = null;
                    foreach (var item in InsertUserId)
                    {
                        oQuotecollabrator = new tbQuoteCollabrator();
                        var Remarks = UserList.Where(x => x.Key == item).Select(x => x.Value).FirstOrDefault();
                        oQuotecollabrator.QuoteID = QuoteID;
                        oQuotecollabrator.ColabratorID = item;
                        oQuotecollabrator.IsActive = true;
                        oQuotecollabrator.IsDelete = false;
                        oQuotecollabrator.Status = ConstantValues.QuoteWaitingCollaborate;
                        oQuotecollabrator.Remarks = Remarks;
                        aQuoteEntities.tbQuoteCollabrators.Add(oQuotecollabrator);
                    }

                    aQuoteEntities.SaveChanges();
                }
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public List<int> GetInsertAuthorizeList(int QuoteID, List<int> InsertUserID)
        {
            List<int> AthorizedUserList = new List<int>();
            List<int> ExistUserID = new List<int>();

            try
            {
                var oQuoteAuthorized = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.IsDelete == false && x.IsActive == true).ToList();
                if (oQuoteAuthorized.Count > ConstantValues.Zero)
                {
                    foreach (var aItem in oQuoteAuthorized)
                    {
                        ExistUserID.Add(aItem.AuthorizedUserId);
                    }
                }

                if (ExistUserID.Count > ConstantValues.Zero)
                {
                    AthorizedUserList = InsertUserID.Except(ExistUserID).ToList();
                }
                else
                {
                    AthorizedUserList = InsertUserID;
                }

                return AthorizedUserList;
            }
            catch
            {
                throw;
            }
        }

        public List<ApproverQuoteStatus> GetApproverQuoteStatusList(int QuoteID)
        {
            try
            {
                List<ApproverQuoteStatus> oApproverQuoteStatusList = new List<ApproverQuoteStatus>();
                ApproverQuoteStatus oApproverQuoteStatus = null;
                var aList = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).ToList();
                if (aList.Count > 0)
                {
                    foreach (var item in aList)
                    {
                        oApproverQuoteStatus = new ApproverQuoteStatus();
                        oApproverQuoteStatus.AuthorizerID = item.AuthorizedUserId;
                        oApproverQuoteStatus.QuoteStatus = item.Status;
                        var oRoleName = aQuoteAdminEntities.tbCustomerDetails.Where(x => x.UserID == item.AuthorizedUserId && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                        if (oRoleName != null)
                        {
                            oApproverQuoteStatus.RoleName = oRoleName.RoleName;
                            oApproverQuoteStatus.EmailAddress = oRoleName.EmailID;
                        }
                        oApproverQuoteStatus.StatusDescription = item.Description;
                        oApproverQuoteStatusList.Add(oApproverQuoteStatus);
                    }
                }
                return oApproverQuoteStatusList;
            }
            catch
            {
                throw;
            }
        }

        public string UpdateCollabratorStatusForQuote(int QuoteID, int UserID, string Status, string StatusDescription, bool Delete)
        {
            try
            {
                var oQuoteCollabrate = aQuoteEntities.tbQuoteCollabrators.Where(x => x.ColabratorID == UserID && x.QuoteID == QuoteID && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                if (!Delete)
                {
                    if (oQuoteCollabrate != null)
                    {
                        oQuoteCollabrate.Status = Status;
                        oQuoteCollabrate.Description = StatusDescription;

                        var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                        if (oQuote != null)
                        {
                            oQuote.QuoteStatus = ConstantValues.QuotePendingApproval;
                        }
                    }
                }
                else
                {
                    if (oQuoteCollabrate != null)
                    {
                        oQuoteCollabrate.Status = Status;
                        oQuoteCollabrate.Description = StatusDescription;

                        var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                        if (oQuote != null)
                        {
                            oQuote.QuoteStatus = Status;
                        }
                    }
                }
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public bool QuoteAuthorizeStatus(int QuoteID, int UserID, string Type)
        {
            try
            {
                bool Status = false;
                if (Type == ConstantValues.Authorize)
                {
                    var oQuoteAuthorize = aQuoteEntities.tbQuoteAuthorizes.Where(x => x.QuoteID == QuoteID && x.AuthorizedUserId == UserID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                    if (oQuoteAuthorize != null)
                    {
                        if (oQuoteAuthorize.Status == ConstantValues.QuoteApproveStatus)
                        {
                            Status = true;
                        }
                    }
                }
                else
                {
                    var oQuoteCollabrator = aQuoteEntities.tbQuoteCollabrators.Where(x => x.QuoteID == QuoteID && x.ColabratorID == UserID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                    if (oQuoteCollabrator != null)
                    {
                        if (oQuoteCollabrator.Status == ConstantValues.QuoteApproveStatus)
                        {
                            Status = true;
                        }
                    }
                }
                return Status;
            }
            catch
            {
                throw;
            }
        }

        public string QuoteShippingInsert(int QuoteID, decimal TruckCost, decimal DieselCost, decimal OtherCost)
        {
            try
            {
                tbQuoteShipping oQuoteShipping = new tbQuoteShipping();
                oQuoteShipping.DieselCost = DieselCost;
                oQuoteShipping.IsActive = true;
                oQuoteShipping.IsDelete = false;
                oQuoteShipping.QuoteID = QuoteID;
                oQuoteShipping.TruckCost = TruckCost;
                oQuoteShipping.OtherCost = OtherCost;
                aQuoteEntities.tbQuoteShippings.Add(oQuoteShipping);
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteShipping> GetQuoteShippingList(int QuoteID)
        {
            try
            {
                List<QuoteShipping> ShippingDetails = new List<QuoteShipping>();
                QuoteShipping oQuoteShipping = null;
                var oShipping = aQuoteEntities.tbQuoteShippings.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).ToList();
                if (oShipping.Count > Convert.ToInt32(ConstantValues.Zero))
                {
                    int Sno = Convert.ToInt32(ConstantValues.One);
                    foreach (var item in oShipping)
                    {
                        oQuoteShipping = new QuoteShipping();
                        oQuoteShipping.Sno = item.ShippingID;
                        oQuoteShipping.truckCost = Extension.MathRound(item.TruckCost);
                        oQuoteShipping.dieselCost = Extension.MathRound(item.DieselCost);
                        oQuoteShipping.otherCost = Extension.MathRound(item.OtherCost);
                        Sno++;
                        ShippingDetails.Add(oQuoteShipping);
                    }
                }
                return ShippingDetails;
            }
            catch
            {
                throw;
            }
        }

        public string UpdateQuoteShipping(int QuoteID, int ShippingID, decimal TruckCost, decimal DieselCost, bool Delete, decimal OtherCost)
        {
            try
            {
                var oShipping = aQuoteEntities.tbQuoteShippings.Where(x => x.QuoteID == QuoteID && x.ShippingID == ShippingID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oShipping != null)
                {
                    if (!Delete)
                    {
                        oShipping.TruckCost = TruckCost;
                        oShipping.DieselCost = DieselCost;
                        oShipping.OtherCost = OtherCost;
                    }
                    else
                    {
                        oShipping.IsActive = false;
                        oShipping.IsDelete = true;
                    }
                }
                aQuoteEntities.SaveChanges();
                return ConstantValues.SuccessMessage;
            }
            catch
            {
                throw;
            }
        }

        public QuoteShipping GetQuoteShippingChargesDetails(int QuoteID, int ShippingID)
        {
            try
            {
                QuoteShipping oQuoteShipping = new QuoteShipping();
                var oShipping = aQuoteEntities.tbQuoteShippings.Where(x => x.QuoteID == QuoteID && x.ShippingID == ShippingID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oShipping != null)
                {
                    oQuoteShipping.ShippingID = oShipping.ShippingID;
                    oQuoteShipping.dieselCost = oShipping.DieselCost;
                    oQuoteShipping.truckCost = oShipping.TruckCost;
                    oQuoteShipping.otherCost = oShipping.OtherCost;
                }
                return oQuoteShipping;
            }
            catch
            {
                throw;
            }
        }

        public List<MaterialProperty> GetBundleMappingMaterial(int BundleID, decimal BudgetTargetRate)
        {
            try
            {
                List<MaterialProperty> oMaterialList = new List<MaterialProperty>();
                MaterialProperty oMaterial = null;
                var oMaterialObject = (from o in aQuoteEntities.tbMaterials
                                       join oo in aQuoteEntities.tbBundleMaterialMappings
                                       on
                                       o.MaterialID equals oo.MaterialID
                                       join ooo in aQuoteEntities.tbBundles
                                       on oo.BundleID equals ooo.BundleID
                                       where ooo.BundleID == BundleID && ooo.IsActive == true && ooo.IsDelete == false
                                       && oo.IsActive == true && oo.IsDelete == false && o.IsActive == true && o.IsDelete == false
                                       select new { o.MaterialID, o.MaterialName, o.MaterialCode, o.MaterialDescription, o.Discount, o.Amount, oo.Quantity }).ToList();
                if (oMaterialObject.Count > 0)
                {
                    foreach (var item in oMaterialObject)
                    {
                        oMaterial = new MaterialProperty();
                        oMaterial.MaterialId = item.MaterialID;
                        oMaterial.MaterialCode = item.MaterialCode;
                        oMaterial.MaterialName = item.MaterialName;
                        oMaterial.MaterialDescription = item.MaterialDescription;
                        oMaterial.MaterialDiscount = item.Discount.Value;
                        oMaterial.MaterialAmount = Extension.MathRound(item.Amount);
                        oMaterial.Quantity = item.Quantity;
                        oMaterialList.Add(oMaterial);
                    }
                }
                return oMaterialList;
            }
            catch
            {
                throw;
            }
        }

        public List<QuoteCollabrationModel> GetCollaboratorList(int QuoteID)
        {
            try
            {
                List<QuoteCollabrationModel> oCollaboratorList = new List<QuoteCollabrationModel>();
                QuoteCollabrationModel oCollaborator = null;
                var oQuote = aQuoteEntities.tbQuotes.Where(x => x.QuoteID == QuoteID && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                if (oQuote != null)
                {
                    var oCollaboratorObject = aQuoteAdminEntities.spGetCollaborators(oQuote.CompanyCode).ToList();
                    foreach (var item in oCollaboratorObject)
                    {
                        oCollaborator = new QuoteCollabrationModel();
                        oCollaborator.EmailAddress = item.EmailID;
                        oCollaborator.RoleName = item.RoleName;
                        oCollaborator.UserID = item.UserID;
                        oCollaboratorList.Add(oCollaborator);
                    }
                }
                return oCollaboratorList;
            }
            catch
            {
                throw;
            }
        }

        public string GetCollaboratorRemarks(int QuoteID, int UserID)
        {
            try
            {
                return aQuoteEntities.tbQuoteCollabrators.Where(x => x.QuoteID == QuoteID && x.ColabratorID == UserID && x.IsActive == true && x.IsDelete == false).Select(x => x.Remarks).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public List<decimal> GetMinimumMaximumAmount()
        {
            try
            {
                List<decimal> Amounts = new List<decimal>();
                var oMaterialList = aQuoteEntities.tbMaterials.Where(x => x.IsActive == true && x.IsDelete == false).ToList();
                Amounts.Add(Extension.MathRound(oMaterialList.Min(x => x.Amount)));
                Amounts.Add(Extension.MathRound(oMaterialList.Max(x => x.Amount)));
                return Amounts;
            }
            catch
            {
                throw;
            }
        }

    }
}
