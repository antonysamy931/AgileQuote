using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;


namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteListController : Controller
    {

        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region Create Quote

        [HttpGet]
        public ActionResult CreateQuote(int? QuoteID, string Type)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Type != null)
            {
                ViewBag.QuoteType = Type;
            }

            Quote qQuote = new Quote();
            QuoteQualitativeInformation dQuoteQualitativeInformation = new QuoteQualitativeInformation();
            if (QuoteID == null)
            {
                if (Session["UserName"] != null)
                {
                    qQuote.PreparedBy = Session["UserName"].ToString();
                }
                qQuote.Currency = agileQuote.LoadCurrency();
                qQuote.SalesOrg = agileQuote.LoadSalesOrg();
                qQuote.StatusList = agileQuote.LoadStatus();
                dQuoteQualitativeInformation.BusinessList = agileQuote.LoadBusinessStatus();
            }
            else
            {
                Session["QuoteID"] = QuoteID.Value;
                qQuote = agileQuote.GetQuoteDetails(QuoteID.Value, (int)Session["UserID"]);
            }
            return View(qQuote);
        }

        [HttpPost]
        public ActionResult CreateQuote(Quote qQuote)
        {
            if (Session["UserID"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            qQuote.Currency = agileQuote.LoadCurrency();
            qQuote.SalesOrg = agileQuote.LoadSalesOrg();
            qQuote.StatusList = agileQuote.LoadStatus();

            if (Request.Form["QuoteID"] != ConstantValues.Zero.ToString() && Request.Form["QuoteID"] != null)
            {
                
                if (ModelState.IsValid)
                {
                    ViewBag.QuoteType = "Create";
                    qQuote.CreatedBy = (int)Session["UserID"];
                    qQuote.salesOrgName = qQuote.SalesOrg.Where(x => x.customerCode == qQuote.CustomerCode).Select(x => x.salesOrgName).FirstOrDefault();
                    qQuote.currencyName = qQuote.Currency.Where(x => x.ValueCurrency == Convert.ToDecimal(qQuote.currencyName)).Select(x => x.CurrencyName).FirstOrDefault();
                    string QuoteID = Request.Form["QuoteID"].ToString();
                    agileQuote.UpdateQuoteDetails(qQuote, Convert.ToInt32(QuoteID));
                    qQuote = agileQuote.GetQuoteDetails(Convert.ToInt32(QuoteID), (int)Session["UserID"]);
                    qQuote.QuoteID = Convert.ToInt32(QuoteID);
                }
                else
                {
                    ViewBag.QuoteType = "SaveAs";
                    qQuote.defaultCurrency = qQuote.currencyName;
                    qQuote.defaultSalesOrg = qQuote.salesOrgName;
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    qQuote.CreatedBy = (int)Session["UserID"];
                    qQuote.salesOrgName = qQuote.SalesOrg.Where(x => x.customerCode == qQuote.CustomerCode).Select(x => x.salesOrgName).FirstOrDefault();
                    qQuote.currencyName = qQuote.Currency.Where(x => x.ValueCurrency == Convert.ToDecimal(qQuote.currencyName)).Select(x => x.CurrencyName).FirstOrDefault();
                    int QuoteID = agileQuote.InsertQuoteDetails(qQuote);
                    Session["QuoteID"] = QuoteID;
                    qQuote = agileQuote.GetQuoteDetails(QuoteID, (int)Session["UserID"]);
                    qQuote.QuoteID = QuoteID;
                }
                else
                {
                    qQuote.defaultCurrency = qQuote.currencyName;
                    qQuote.defaultSalesOrg = qQuote.salesOrgName;
                }
            }
            return View(qQuote);
        }

        [HttpPost]
        public JsonResult SaveAsNewQuote(int QuoteId)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int UserID = 0;
            int QuoteID = 0;

            if (Session["UserID"] != null)
            {
                UserID = (int)Session["UserID"];
                QuoteID = agileQuote.QuoteSaveAsNewQuote(QuoteId, UserID);
                redirectAction = Url.Action("CreateQuote", "QuoteList", new { QuoteID = QuoteID, Type = "Create" });
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
                QuoteID
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion        

        #region Approve Reject Function

        public JsonResult ApproveQuote(int QuoteID, string Description)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int UserID = 0;

            if (Session["UserID"] != null)
            {
                UserID = (int)Session["UserID"];
                var result = agileQuote.ApprovedQuote(QuoteID, UserID, Description);
                if (result == ConstantValues.SuccessMessage)
                {
                    redirectAction = Url.Action("QuoteBoard", "QuoteBoard");
                    redirect = true;
                }
            }
            else
            {
                redirectAction = Url.Action("Login", "Account");
                redirect = true;
            }


            return Json(new
            {
                redirect,
                redirectAction
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RejectQuote(int QuoteID, string Description)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int UserID = 0;

            if (Session["UserID"] != null)
            {
                UserID = (int)Session["UserID"];
                var result = agileQuote.RejectQuote(QuoteID, UserID, Description);
                if (result == ConstantValues.SuccessMessage)
                {
                    redirectAction = Url.Action("QuoteBoard", "QuoteBoard");
                    redirect = true;
                }
            }
            else
            {
                redirectAction = Url.Action("Login", "Account");
                redirect = true;
            }

            return Json(new
            {
                redirect,
                redirectAction
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreatorRejectQuote(int QuoteID, string Description)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int UserID = 0;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                UserID = (int)Session["UserID"];
                var result = agileQuote.RejectCreatorQuote(QuoteID, UserID);
                if (result == ConstantValues.SuccessMessage)
                {
                    redirectAction = Url.Action("QuoteBoard", "QuoteBoard");
                    redirect = true;
                }
                else
                {
                    redirect = false;
                    Result = result;
                }
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

        #endregion

        public JsonResult QuoteAuthorizeStatus(int QuoteID, string Type)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            bool Result = false;
            int UserID = 0;

            if (Session["UserID"] != null)
            {
                UserID = (int)Session["UserID"];
                Result = agileQuote.QuoteAuthorizeStatus(QuoteID, UserID, Type);
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
