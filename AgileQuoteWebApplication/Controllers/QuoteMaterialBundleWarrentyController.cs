using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteMaterialBundleWarrentyController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region Quote Material Bundle warrenty

        public ActionResult QuoteMaterialBundleWarrenty(int QuoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.QuoteID = QuoteID;

            ViewBag.symbol = agileQuote.GetSymbol(QuoteID, null);
            return PartialView();
        }

        public JsonResult GetQuoteMaterialBundleListWarrenty(jQueryDataTableParamModel param, int QuoteID)
        {
            IList<QuoteBundleMaterial> qAllList = new List<QuoteBundleMaterial>();
            qAllList = agileQuote.GetQuoteMaterialBundleListwithwarranty(QuoteID);
            IEnumerable<QuoteBundleMaterial> qFillterMaterialBundle = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var QuantityFilter = Convert.ToString(Request["sSearch_4"]);
                var UnitPriceFilter = Convert.ToString(Request["sSearch_5"]);
                var WarrentyFilter = Convert.ToString(Request["sSearch_6"]);
                var TypeFilter = Convert.ToString(Request["sSearch_7"]);


                //Optionally check whether the columns are searchable at all 
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isQuantitySearchable = Convert.ToBoolean(Request["bSearchable_4"]);
                var isUnitPriceSearchable = Convert.ToBoolean(Request["bSearchable_5"]);
                var isWarrentySearchable = Convert.ToBoolean(Request["bSearchable_6"]);
                var isTypeSearchable = Convert.ToBoolean(Request["bSearchable_7"]);

                qFillterMaterialBundle = qAllList.Where(c => isCodeSearchable && Convert.ToString(c.Code).ToLower().Contains(param.sSearch.ToLower())
                    || isNameSearchable && c.Name.ToLower().Contains(param.sSearch.ToLower())
                    || isQuantitySearchable && c.Quantity.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isUnitPriceSearchable && c.UnitPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isTypeSearchable && c.Type.Contains(param.sSearch.ToLower())
                    || isWarrentySearchable && c.Warrenty.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillterMaterialBundle = qAllList;
            }

            var isCodeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isQuantitySortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isUnitPriceSortable = Convert.ToBoolean(Request["bSortable_5"]);
            var isWarrentySortable = Convert.ToBoolean(Request["bSortable_6"]);
            var isTypeSortable = Convert.ToBoolean(Request["bSortable_7"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteBundleMaterial, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.Code.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.Name :
                sortColumnIndex == 4 && isQuantitySortable ? c.Quantity.ToString() :
                sortColumnIndex == 5 && isUnitPriceSortable ? c.UnitPrice.ToString() :
                sortColumnIndex == 6 && isWarrentySortable ? c.Warrenty.ToString() :
                sortColumnIndex == 7 && isTypeSortable ? c.Type :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillterMaterialBundle = qFillterMaterialBundle.OrderBy(orderingFunction);
            }
            else
            {
                qFillterMaterialBundle = qFillterMaterialBundle.OrderByDescending(orderingFunction);
            }

            var displayMaterial = qFillterMaterialBundle.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol(QuoteID, null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.Code),m.Name,m.Description,
                                        Convert.ToString(m.Quantity),Extension.FormatCurrency(nfi,m.UnitPrice),
                                        Convert.ToString(m.Warrenty),m.Type};

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterMaterialBundle.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdateBundleMaterialWarrent(int QuoteID, int Code, string Type, int Warrenty)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                int? MaterialID = null;
                int? BundleID = null;
                if (Type == "Material")
                {
                    MaterialID = Code;
                }
                else
                {
                    BundleID = Code;
                }

                Result = agileQuote.UpdateMaterialBundleWarrentyRecord(QuoteID, Type, MaterialID, BundleID, Warrenty);
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

    }
}
