using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class SummaryController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        [HttpGet]
        public ActionResult QuoteSummary(int QuoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            Session["QuoteID"] = QuoteID;
            ViewBag.QuoteID = QuoteID;
            decimal QuotePrice = ConstantValues.Zero;

            AgileQuoteProperty.Model.QuoteSummary oQuoteSummary = new QuoteSummary();

            QuoteBundleMaterialList qQutoeBundleMaterialList = new QuoteBundleMaterialList();
            qQutoeBundleMaterialList = agileQuote.GetQuoteBaseMaterialBundleList(QuoteID);

            QuotePrice = QuotePrice + Extension.StringToDecimal(qQutoeBundleMaterialList.qQuoteBundleMaterial.qTotalNetPrice);

            Quote qQuote = new Quote();
            qQuote = agileQuote.GetQuoteDetails(QuoteID, (int)Session["UserID"]);

            ViewBag.symbol = agileQuote.GetSymbol(QuoteID, null);

            QuoteQualitativeInformation qQuoteQualitativeInformation = new QuoteQualitativeInformation();
            qQuoteQualitativeInformation = agileQuote.GetQualitativeInfomation(QuoteID);

            oQuoteSummary.summaryQuote = qQuote;
            oQuoteSummary.summaryQuoteBundle = qQutoeBundleMaterialList;
            oQuoteSummary.summaryQuoteInstallationCharges = agileQuote.GetQuoteShippingDetails(QuoteID);
            oQuoteSummary.summaryQuoteBoughtoutItem = agileQuote.GetQuoteBasedBoughtoutItemList(QuoteID);
            oQuoteSummary.summaryQuoteBoughtoutItemTotal = agileQuote.GetTotalCostBoughtoutItem(QuoteID);
            oQuoteSummary.summaryQuoteBundleWarrenty = agileQuote.GetQuoteMaterialBundleListwithwarranty(QuoteID);
            oQuoteSummary.summaryQuoteShipping = agileQuote.GetQuoteShippingList(QuoteID);
            oQuoteSummary.summaryQuoteShippingTotalCost = agileQuote.GetQuoteShippingTotal(QuoteID);
            QuotePrice = QuotePrice + Extension.StringToDecimal(oQuoteSummary.summaryQuoteInstallationCharges.TotalCost);
            QuotePrice = QuotePrice + Extension.StringToDecimal(oQuoteSummary.summaryQuoteBoughtoutItemTotal.TotalQuotedNetPrice);
            QuotePrice = QuotePrice + oQuoteSummary.summaryQuoteShippingTotalCost;
            oQuoteSummary.summaryQuotePrice = QuotePrice;
            oQuoteSummary.summaryQuoteQualitativeInformation = qQuoteQualitativeInformation;

            string symbol = agileQuote.GetSymbol(QuoteID, null);
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;
            ViewBag.Nfi = nfi;

            return View(oQuoteSummary);
        }       

    }
}
