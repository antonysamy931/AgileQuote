using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;

namespace AgileQuoteWebApplication.Controllers
{
    public class QualitativeController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();
        
        public ActionResult QuoteAuthorisationRequest(int QuoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            QuoteQualitativeInformation qQuoteQualitativeInformation = new QuoteQualitativeInformation();
            qQuoteQualitativeInformation = agileQuote.GetQualitativeInfomation(QuoteID);
            qQuoteQualitativeInformation.BusinessList = agileQuote.LoadBusinessStatus();
            return PartialView(qQuoteQualitativeInformation);
        }

        [HttpPost]
        public JsonResult QualitativeInformations(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.InsertQualitativeInformation(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);
            }
            else
            {
                redirectAction = Url.Action("Login", "Account");
                redirect = true;
            }

            return Json(new
            {
                redirect,
                redirectAction,
                Result
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult QualitativeInformationsUpdate(int QuoteID, string Leadtime, string WinProbability, string ScopeofWork, string ExecutiveSummary, string PrimaryCompetitor, string SellingPrice, string PaymentTerms, string RiskMitigation, string NewRepeatBusiness, string AnyOtherComments)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.InsertQualitativeInformationUpdate(QuoteID, Leadtime, WinProbability, ScopeofWork, ExecutiveSummary, PrimaryCompetitor, SellingPrice, PaymentTerms, RiskMitigation, NewRepeatBusiness, AnyOtherComments);
            }
            else
            {
                redirectAction = Url.Action("Login", "Account");
                redirect = true;
            }

            return Json(new
            {
                redirect,
                redirectAction,
                Result
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult QualitativeInformationQuotedValue(int QuoteID)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            QuoteQualitativeInformationValues Result = null;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.GetTotalQuoteQualitativeTotalService(QuoteID);
            }
            else
            {
                redirectAction = Url.Action("Login", "Account");
                redirect = true;
            }

            return Json(new
            {
                redirect,
                redirectAction,
                Result
            }, JsonRequestBehavior.AllowGet);
        }



    }
}
