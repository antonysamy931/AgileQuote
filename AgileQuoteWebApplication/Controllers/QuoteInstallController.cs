using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteInstallController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region Quote Installation

        public ActionResult QuoteInstall(int QuoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.QuoteID = QuoteID;
            ViewBag.symbol = agileQuote.GetSymbol(QuoteID, null);
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetQuoteInstallationDetails(int QuoteID)
        {
            bool Redirect = false;
            string RedirectAction = string.Empty;
            if (Session["UserID"] != null)
            {
                var result = agileQuote.GetQuoteShippingDetails(QuoteID);
                return Json(new
                {
                    Redirect,
                    RedirectAction,
                    result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Redirect = true;
                RedirectAction = Url.Action("Login", "Account");
                return Json(new
                {
                    Redirect,
                    RedirectAction
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InsertQuoteInstallationDetails(int NoEmp, decimal PerDayCost, int NoDays, decimal TotalCost, int QuoteID)
        {
            InstallCharges oShippingCharges = new InstallCharges();
            oShippingCharges.CostPerDay = PerDayCost.ToString();
            oShippingCharges.NoDays = NoDays;
            oShippingCharges.NoEmployees = NoEmp;
            oShippingCharges.TotalCost = TotalCost.ToString();

            bool Redirect = false;
            string RedirectAction = string.Empty;
            if (Session["UserID"] != null)
            {
                var result = agileQuote.InsertShippingToQuote(oShippingCharges, QuoteID);
                return Json(new
                {
                    Redirect,
                    RedirectAction,
                    result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Redirect = true;
                RedirectAction = Url.Action("Login", "Account");
                return Json(new
                {
                    Redirect,
                    RedirectAction
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
