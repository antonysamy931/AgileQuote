using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;

namespace AgileQuoteWebApplication.Controllers
{
    public class HomeController : Controller
    {
        AgileQuoteService.IService1 QuoteService = new AgileQuoteService.Service1Client();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();

        }
        public ActionResult SalesManagerDashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return PartialView();
        }
        public ActionResult SalesmanDashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return PartialView();
        }

        public ActionResult Reports()
        {
            return View();
        }

        [HttpGet]
        public ActionResult QuotestoCloseReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PendingQuotesReport()
        {
            return View();

        }

        [HttpGet]
        public ActionResult OrphanedQuotesReport()
        {
            return View();

        }

        [HttpGet]
        public ActionResult QuotationSummaryReport()
        {
            return View();

        }

        [HttpGet]
        public ActionResult QuotationListReport()
        {
            return View();
        }

        public JsonResult GeQuoteToClosetManager(jQueryDataTableParamModel param)
        {
            return null;
        }
    }
}
