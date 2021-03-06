﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgileQuoteData.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class AgileQuoteEntities : DbContext
    {
        public AgileQuoteEntities()
            : base("name=AgileQuoteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tbBundle> tbBundles { get; set; }
        public DbSet<tbBundleMaterialMapping> tbBundleMaterialMappings { get; set; }
        public DbSet<tbBundleType> tbBundleTypes { get; set; }
        public DbSet<tbCategory> tbCategories { get; set; }
        public DbSet<tbCurrency> tbCurrencies { get; set; }
        public DbSet<tbMaterial> tbMaterials { get; set; }
        public DbSet<tbQuote> tbQuotes { get; set; }
        public DbSet<tbQuoteAuthorize> tbQuoteAuthorizes { get; set; }
        public DbSet<tbQuoteBundleMapping> tbQuoteBundleMappings { get; set; }
        public DbSet<tbQuoteMaterialMapping> tbQuoteMaterialMappings { get; set; }
        public DbSet<tbQuoteRentalProductMapping> tbQuoteRentalProductMappings { get; set; }
        public DbSet<tbQuoteRentalSparsMapping> tbQuoteRentalSparsMappings { get; set; }
        public DbSet<tbQuoteStatu> tbQuoteStatus { get; set; }
        public DbSet<tbRentalProduct> tbRentalProducts { get; set; }
        public DbSet<tbRentalSpar> tbRentalSpars { get; set; }
        public DbSet<tbSalesOrganization> tbSalesOrganizations { get; set; }
        public DbSet<tbQuoteBundleInstalation> tbQuoteBundleInstalations { get; set; }
        public DbSet<tbQuoteMaterialInstalation> tbQuoteMaterialInstalations { get; set; }
        public DbSet<tbQuoteService> tbQuoteServices { get; set; }
        public DbSet<tbQuoteBoughtOutItem> tbQuoteBoughtOutItems { get; set; }
        public DbSet<tbQuoteCollabrator> tbQuoteCollabrators { get; set; }
        public DbSet<tbQuoteShipping> tbQuoteShippings { get; set; }
        public DbSet<tbQuoteBusinessList> tbQuoteBusinessLists { get; set; }
        public DbSet<tbQuoteQualitativeInformation> tbQuoteQualitativeInformations { get; set; }
        public DbSet<SugectionMaterialMapping> SugectionMaterialMappings { get; set; }
        public DbSet<tbApproverRule> tbApproverRules { get; set; }
        public DbSet<tbOfferedMaterialMapping> tbOfferedMaterialMappings { get; set; }
    
        public virtual ObjectResult<CurrencySelect_Result> CurrencySelect(string value)
        {
            var valueParameter = value != null ?
                new ObjectParameter("value", value) :
                new ObjectParameter("value", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CurrencySelect_Result>("CurrencySelect", valueParameter);
        }
    
        public virtual ObjectResult<MaterialSelect_Result> MaterialSelect(string value)
        {
            var valueParameter = value != null ?
                new ObjectParameter("value", value) :
                new ObjectParameter("value", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MaterialSelect_Result>("MaterialSelect", valueParameter);
        }
    
        public virtual ObjectResult<SalesOrgSelect_Result> SalesOrgSelect(string value)
        {
            var valueParameter = value != null ?
                new ObjectParameter("value", value) :
                new ObjectParameter("value", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SalesOrgSelect_Result>("SalesOrgSelect", valueParameter);
        }
    
        public virtual ObjectResult<selectQuote_Result> selectQuote(string quoteID, string currencyName, string salesOrg, string status, string prepareBy, string createDate, string requiredDate, string userId, string customerName)
        {
            var quoteIDParameter = quoteID != null ?
                new ObjectParameter("quoteID", quoteID) :
                new ObjectParameter("quoteID", typeof(string));
    
            var currencyNameParameter = currencyName != null ?
                new ObjectParameter("currencyName", currencyName) :
                new ObjectParameter("currencyName", typeof(string));
    
            var salesOrgParameter = salesOrg != null ?
                new ObjectParameter("salesOrg", salesOrg) :
                new ObjectParameter("salesOrg", typeof(string));
    
            var statusParameter = status != null ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(string));
    
            var prepareByParameter = prepareBy != null ?
                new ObjectParameter("prepareBy", prepareBy) :
                new ObjectParameter("prepareBy", typeof(string));
    
            var createDateParameter = createDate != null ?
                new ObjectParameter("createDate", createDate) :
                new ObjectParameter("createDate", typeof(string));
    
            var requiredDateParameter = requiredDate != null ?
                new ObjectParameter("requiredDate", requiredDate) :
                new ObjectParameter("requiredDate", typeof(string));
    
            var userIdParameter = userId != null ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(string));
    
            var customerNameParameter = customerName != null ?
                new ObjectParameter("customerName", customerName) :
                new ObjectParameter("customerName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<selectQuote_Result>("selectQuote", quoteIDParameter, currencyNameParameter, salesOrgParameter, statusParameter, prepareByParameter, createDateParameter, requiredDateParameter, userIdParameter, customerNameParameter);
        }
    
        public virtual ObjectResult<spBundleCategoryBaseSelect_Result> spBundleCategoryBaseSelect(string categoryName)
        {
            var categoryNameParameter = categoryName != null ?
                new ObjectParameter("categoryName", categoryName) :
                new ObjectParameter("categoryName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spBundleCategoryBaseSelect_Result>("spBundleCategoryBaseSelect", categoryNameParameter);
        }
    
        public virtual ObjectResult<spBundleSelect_Result> spBundleSelect(Nullable<int> bundleID)
        {
            var bundleIDParameter = bundleID.HasValue ?
                new ObjectParameter("BundleID", bundleID) :
                new ObjectParameter("BundleID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spBundleSelect_Result>("spBundleSelect", bundleIDParameter);
        }
    
        public virtual ObjectResult<spMaterialCategoryBaseSelect_Result> spMaterialCategoryBaseSelect(string categoryName)
        {
            var categoryNameParameter = categoryName != null ?
                new ObjectParameter("categoryName", categoryName) :
                new ObjectParameter("categoryName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spMaterialCategoryBaseSelect_Result>("spMaterialCategoryBaseSelect", categoryNameParameter);
        }
    
        public virtual ObjectResult<spQuoteBasedMaterialBundle_Result> spQuoteBasedMaterialBundle(Nullable<int> quoteID, Nullable<int> createdby)
        {
            var quoteIDParameter = quoteID.HasValue ?
                new ObjectParameter("QuoteID", quoteID) :
                new ObjectParameter("QuoteID", typeof(int));
    
            var createdbyParameter = createdby.HasValue ?
                new ObjectParameter("Createdby", createdby) :
                new ObjectParameter("Createdby", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spQuoteBasedMaterialBundle_Result>("spQuoteBasedMaterialBundle", quoteIDParameter, createdbyParameter);
        }
    
        public virtual ObjectResult<spQuoteBasedMaterialBundleRecordSelect_Result> spQuoteBasedMaterialBundleRecordSelect(string type, Nullable<int> quoteId, Nullable<int> materialID, Nullable<int> bundleID)
        {
            var typeParameter = type != null ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(string));
    
            var quoteIdParameter = quoteId.HasValue ?
                new ObjectParameter("QuoteId", quoteId) :
                new ObjectParameter("QuoteId", typeof(int));
    
            var materialIDParameter = materialID.HasValue ?
                new ObjectParameter("MaterialID", materialID) :
                new ObjectParameter("MaterialID", typeof(int));
    
            var bundleIDParameter = bundleID.HasValue ?
                new ObjectParameter("BundleID", bundleID) :
                new ObjectParameter("BundleID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spQuoteBasedMaterialBundleRecordSelect_Result>("spQuoteBasedMaterialBundleRecordSelect", typeParameter, quoteIdParameter, materialIDParameter, bundleIDParameter);
        }
    
        public virtual ObjectResult<spRentalProductSelect_Result> spRentalProductSelect(Nullable<int> quoteID, Nullable<int> rentalProductID)
        {
            var quoteIDParameter = quoteID.HasValue ?
                new ObjectParameter("QuoteID", quoteID) :
                new ObjectParameter("QuoteID", typeof(int));
    
            var rentalProductIDParameter = rentalProductID.HasValue ?
                new ObjectParameter("RentalProductID", rentalProductID) :
                new ObjectParameter("RentalProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spRentalProductSelect_Result>("spRentalProductSelect", quoteIDParameter, rentalProductIDParameter);
        }
    
        public virtual ObjectResult<spRentalSparsSelect_Result> spRentalSparsSelect(Nullable<int> quoteID, Nullable<int> rentalSparsID)
        {
            var quoteIDParameter = quoteID.HasValue ?
                new ObjectParameter("QuoteID", quoteID) :
                new ObjectParameter("QuoteID", typeof(int));
    
            var rentalSparsIDParameter = rentalSparsID.HasValue ?
                new ObjectParameter("RentalSparsID", rentalSparsID) :
                new ObjectParameter("RentalSparsID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spRentalSparsSelect_Result>("spRentalSparsSelect", quoteIDParameter, rentalSparsIDParameter);
        }
    
        public virtual ObjectResult<spQuoteBasedMaterialBundleWithoutConfigured_Result> spQuoteBasedMaterialBundleWithoutConfigured(Nullable<int> quoteID)
        {
            var quoteIDParameter = quoteID.HasValue ?
                new ObjectParameter("QuoteID", quoteID) :
                new ObjectParameter("QuoteID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spQuoteBasedMaterialBundleWithoutConfigured_Result>("spQuoteBasedMaterialBundleWithoutConfigured", quoteIDParameter);
        }
    
        public virtual ObjectResult<spSelectQuote_Result> spSelectQuote(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spSelectQuote_Result>("spSelectQuote", userIDParameter);
        }
    
        public virtual ObjectResult<spSelectCreateQuote_Result> spSelectCreateQuote(Nullable<int> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spSelectCreateQuote_Result>("spSelectCreateQuote", useridParameter);
        }
    
        public virtual ObjectResult<spAllAuthorizedRejectedQuote_Result> spAllAuthorizedRejectedQuote(Nullable<int> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spAllAuthorizedRejectedQuote_Result>("spAllAuthorizedRejectedQuote", useridParameter);
        }
    
        public virtual ObjectResult<spSaveAsNewQuote_Result> spSaveAsNewQuote(Nullable<int> quoteId, Nullable<int> createdBy, Nullable<int> companyCode, string preparedBy, Nullable<int> requestedDateConstant)
        {
            var quoteIdParameter = quoteId.HasValue ?
                new ObjectParameter("quoteId", quoteId) :
                new ObjectParameter("quoteId", typeof(int));
    
            var createdByParameter = createdBy.HasValue ?
                new ObjectParameter("createdBy", createdBy) :
                new ObjectParameter("createdBy", typeof(int));
    
            var companyCodeParameter = companyCode.HasValue ?
                new ObjectParameter("companyCode", companyCode) :
                new ObjectParameter("companyCode", typeof(int));
    
            var preparedByParameter = preparedBy != null ?
                new ObjectParameter("preparedBy", preparedBy) :
                new ObjectParameter("preparedBy", typeof(string));
    
            var requestedDateConstantParameter = requestedDateConstant.HasValue ?
                new ObjectParameter("requestedDateConstant", requestedDateConstant) :
                new ObjectParameter("requestedDateConstant", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spSaveAsNewQuote_Result>("spSaveAsNewQuote", quoteIdParameter, createdByParameter, companyCodeParameter, preparedByParameter, requestedDateConstantParameter);
        }
    
        public virtual ObjectResult<spCollaboratorQuote_Result> spCollaboratorQuote(Nullable<int> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spCollaboratorQuote_Result>("spCollaboratorQuote", useridParameter);
        }
    
        public virtual ObjectResult<spApproveQuote_Result> spApproveQuote(Nullable<int> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spApproveQuote_Result>("spApproveQuote", useridParameter);
        }
    }
}
