using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteBoughtOutItemController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region Quote Bought Out Item

        public ActionResult QuoteBoughtOutItem(int QuoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.QuoteID = QuoteID;

            ViewBag.symbol = agileQuote.GetSymbol(QuoteID, null);

            return PartialView();
        }

        public JsonResult GetQuoteBoughtOutItemDatatable(jQueryDataTableParamModel param, int QuoteID)
        {
            IList<QuoteBoughtOutItemModel> qAllList = new List<QuoteBoughtOutItemModel>();
            qAllList = agileQuote.GetQuoteBasedBoughtoutItemList(QuoteID);
            IEnumerable<QuoteBoughtOutItemModel> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var UnitPriceFilter = Convert.ToString(Request["sSearch_3"]);
                var QuantityFilter = Convert.ToString(Request["sSearch_4"]);
                var NetPriceFilter = Convert.ToString(Request["sSearch_5"]);
                var quotedUnitPriceFilter = Convert.ToString(Request["sSearch_6"]);
                var quotedNetPriceFilter = Convert.ToString(Request["sSearch_7"]);


                //Optionally check whether the columns are searchable at all
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isUnitPriceSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isQuantitySearchable = Convert.ToBoolean(Request["bSearchable_4"]);
                var isNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_5"]);
                var isquotedUnitPriceSearchable = Convert.ToBoolean(Request["bSearchable_6"]);
                var isquotedNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_7"]);

                qFillter = qAllList.Where(c => isNameSearchable && Convert.ToString(c.ItemName).ToLower().Contains(param.sSearch.ToLower())
                    || isUnitPriceSearchable && c.UnitPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isQuantitySearchable && c.Quantity.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isNetPriceSearchable && c.NetPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isquotedUnitPriceSearchable && c.quotedUnitPrice.ToString().Contains(param.sSearch.ToLower())
                    || isquotedNetPriceSearchable && c.quotedNetPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isCodeSearchable && c.ItemId.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillter = qAllList;
            }

            var isCodeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isUnitPriceSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isQuantitySortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isNetPriceSortable = Convert.ToBoolean(Request["bSortable_5"]);
            var isquotedUnitPriceSortable = Convert.ToBoolean(Request["bSortable_6"]);
            var isquotedNetPriceSortable = Convert.ToBoolean(Request["bSortable_7"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteBoughtOutItemModel, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.ItemId.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.ItemName :
                sortColumnIndex == 3 && isUnitPriceSortable ? c.UnitPrice.ToString() :
                sortColumnIndex == 4 && isQuantitySortable ? c.Quantity.ToString() :
                sortColumnIndex == 5 && isNetPriceSortable ? c.NetPrice.ToString() :
                sortColumnIndex == 6 && isquotedUnitPriceSortable ? c.quotedUnitPrice.ToString() :
                sortColumnIndex == 7 && isquotedNetPriceSortable ? c.quotedNetPrice.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillter = qFillter.OrderBy(orderingFunction);
            }
            else
            {
                qFillter = qFillter.OrderByDescending(orderingFunction);
            }

            var displayMaterial = qFillter.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol(QuoteID, null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;
            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.ItemId),
                                        m.ItemName,
                                        Extension.FormatCurrency(nfi,m.UnitPrice),
                                        Convert.ToString(m.Quantity),
                                        Extension.FormatCurrency(nfi,m.NetPrice),
                                        Extension.FormatCurrency(nfi,m.quotedUnitPrice),
                                        Extension.FormatCurrency(nfi,m.quotedNetPrice)
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InsertBoughtoutItem(int QuoteID, string Name, decimal UnitPrice, int Quantity)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.InsertQuoteBoughtOutItem(QuoteID, Name, UnitPrice, Quantity);
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

        public JsonResult GetQuoteBoughtoutItemRecord(int QuoteId, int ItemId)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            QuoteBoughtOutItemModel Result = new QuoteBoughtOutItemModel();

            if (Session["UserID"] != null)
            {
                Result = agileQuote.GetQuoteBasedBoughtoutItemRecord(QuoteId, ItemId);
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

        public JsonResult UpdateQuoteBasedBoughtoutItemRecord(int QuoteId, int Itemid, decimal UnitPrice, int Quantity)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.UpdateQuoteBasedBoughtoutItemRecord(QuoteId, UnitPrice, Quantity, Itemid);
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

        public JsonResult DeleteQuoteBasedBoughtoutItemRecord(int QuoteID, int ItemId)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.DeleteQuoteBasedBoughtoutItemRecord(QuoteID, ItemId);
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

        public JsonResult GetQuoteBoughtOutItemListLength(int QuoteID)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int Result = Convert.ToInt32(ConstantValues.Zero);

            if (Session["UserID"] != null)
            {
                Result = agileQuote.GetQuoteBasedBoughtoutItemList(QuoteID).Count;
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

        public JsonResult GetTotalQuoteBoughtoutPrice(int QuoteID)
        {
            TotalUnitDiscount oTotalUnitDiscount = agileQuote.GetTotalCostBoughtoutItem(QuoteID);
            return Json(new
            {
                oTotalUnitDiscount
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
