using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class ShippingQuoteController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region QuoteShipping

        public ActionResult ShippingQuote(int QuoteID)
        {
            ViewBag.QuoteID = QuoteID;
            ViewBag.symbol = agileQuote.GetSymbol(QuoteID, null);
            return PartialView();
        }

        public JsonResult GetQuoteShippingDatatableList(jQueryDataTableParamModel param, int QuoteID)
        {
            IList<QuoteShipping> qAllList = new List<QuoteShipping>();
            qAllList = agileQuote.GetQuoteShippingList(QuoteID);
            IEnumerable<QuoteShipping> qFillterQuoteShipping = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered                 
                var TruckFilter = Convert.ToString(Request["sSearch_1"]);
                var DieselFilter = Convert.ToString(Request["sSearch_2"]);
                var OtherFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all                 
                var isTruckSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isDieselSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isOtherSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                qFillterQuoteShipping = qAllList.Where(c => isTruckSearchable && c.truckCost.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isDieselSearchable && c.dieselCost.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isOtherSearchable && c.otherCost.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillterQuoteShipping = qAllList;
            }

            var isTruckSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isDieselSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isOtherSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteShipping, string> orderingFunction = (c => sortColumnIndex == 1 && isTruckSortable ? c.truckCost.ToString() :
                sortColumnIndex == 2 && isDieselSortable ? c.dieselCost.ToString() :
                sortColumnIndex == 3 && isOtherSortable ? c.otherCost.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillterQuoteShipping = qFillterQuoteShipping.OrderBy(orderingFunction);
            }
            else
            {
                qFillterQuoteShipping = qFillterQuoteShipping.OrderByDescending(orderingFunction);
            }

            var displayQuoteShipping = qFillterQuoteShipping.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol(QuoteID, null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;
            var result = from m in displayQuoteShipping
                         select new[] {Convert.ToString(m.Sno),                                        
                                        Extension.FormatCurrency(nfi,m.truckCost),
                                        Extension.FormatCurrency(nfi,m.dieselCost),
                                        Extension.FormatCurrency(nfi,m.otherCost)
                                      };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterQuoteShipping.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsetQuoteShippingDetails(int QuoteID, decimal truckCost, decimal dieselCost, decimal otherCost)
        {
            bool Redirect = false;
            string RedirectAction = string.Empty;
            if (Session["UserID"] != null)
            {
                var result = agileQuote.QuoteShippingInsert(QuoteID, truckCost, dieselCost, otherCost);
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
        public JsonResult GetQuoteShippingListCount(int QuoteID)
        {
            bool Redirect = false;
            string RedirectAction = string.Empty;
            if (Session["UserID"] != null)
            {
                var result = agileQuote.GetQuoteShippingList(QuoteID).Count;
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
        public JsonResult GetQuoteShippingDetails(int QuoteID, int ShippingID)
        {
            bool Redirect = false;
            string RedirectAction = string.Empty;
            if (Session["UserID"] != null)
            {
                var result = agileQuote.GetQuoteShippingChargesDetails(QuoteID, ShippingID);
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
        public JsonResult UpdateShippingChargesDetails(int QuoteID, int ShippingID, decimal TruckCost, decimal DieselCost, bool Delete, decimal otherCost)
        {
            bool Redirect = false;
            string RedirectAction = string.Empty;
            if (Session["UserID"] != null)
            {
                var result = agileQuote.UpdateQuoteShipping(QuoteID, ShippingID, TruckCost, DieselCost, Delete, otherCost);
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
