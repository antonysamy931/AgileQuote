using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteMaterialBundleListController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region Quote material bundle list

        public ActionResult QuoteMaterialBundleList(int QuoteID)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.QuoteID = QuoteID;
            QuoteBundleMaterialList qQutoeBundleMaterialList = new QuoteBundleMaterialList();
            qQutoeBundleMaterialList = agileQuote.GetQuoteBaseMaterialBundleList(QuoteID);
            ViewBag.symbol = agileQuote.GetSymbol(QuoteID, null);
            return PartialView(qQutoeBundleMaterialList);
        }

        public JsonResult GetQuoteMaterialBundleList(jQueryDataTableParamModel param, int QuoteID)
        {
            QuoteBundleMaterialList qQutoeBundleMaterialList = new QuoteBundleMaterialList();
            qQutoeBundleMaterialList = agileQuote.GetQuoteBaseMaterialBundleList(QuoteID);
            IList<QuoteBundleMaterial> qAllList = new List<QuoteBundleMaterial>();
            qAllList = qQutoeBundleMaterialList.ListQuoteBundleMaterial;
            IEnumerable<QuoteBundleMaterial> qFillterMaterialBundle = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var QuantityFilter = Convert.ToString(Request["sSearch_4"]);
                var GrossFilter = Convert.ToString(Request["sSearch_6"]);
                var UnitPriceFilter = Convert.ToString(Request["sSearch_5"]);
                var DiscountFilter = Convert.ToString(Request["sSearch_7"]);
                var NetPriceFilter = Convert.ToString(Request["sSearch_8"]);
                var TypeFilter = Convert.ToString(Request["sSearch_9"]);


                //Optionally check whether the columns are searchable at all 
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isQuantitySearchable = Convert.ToBoolean(Request["bSearchable_4"]);
                var isUnitPriceSearchable = Convert.ToBoolean(Request["bSearchable_5"]);
                var isGrossPriceSearchable = Convert.ToBoolean(Request["bSearchable_6"]);
                var isDiscountSearchable = Convert.ToBoolean(Request["bSearchable_7"]);
                var isNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_8"]);
                var isTypeSearchable = Convert.ToBoolean(Request["bSearchable_9"]);

                qFillterMaterialBundle = qAllList.Where(c => isCodeSearchable && Convert.ToString(c.Code).ToLower().Contains(param.sSearch.ToLower())
                    || isNameSearchable && c.Name.ToLower().Contains(param.sSearch.ToLower())
                    || isQuantitySearchable && c.Quantity.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isUnitPriceSearchable && c.UnitPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isDiscountSearchable && c.Discount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isNetPriceSearchable && c.NetPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isTypeSearchable && c.Type.Contains(param.sSearch.ToLower())
                    || isGrossPriceSearchable && c.GrossPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
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
            var isGrossPriceSortable = Convert.ToBoolean(Request["bSortable_6"]);
            var isDiscountSortable = Convert.ToBoolean(Request["bSortable_7"]);
            var isNetPriceSortable = Convert.ToBoolean(Request["bSortable_8"]);
            var isTypeSortable = Convert.ToBoolean(Request["bSortable_9"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteBundleMaterial, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.Code.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.Name :
                sortColumnIndex == 4 && isQuantitySortable ? c.Quantity.ToString() :
                sortColumnIndex == 5 && isUnitPriceSortable ? c.UnitPrice.ToString() :
                sortColumnIndex == 6 && isGrossPriceSortable ? c.GrossPrice.ToString() :
                sortColumnIndex == 7 && isDiscountSortable ? c.Discount.ToString() :
                sortColumnIndex == 8 && isNetPriceSortable ? c.NetPrice.ToString() :
                sortColumnIndex == 9 && isTypeSortable ? c.Type :
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
                         select new[] { Convert.ToString(m.Sno), Convert.ToString(m.Code),m.Name,m.Description,
                                        Convert.ToString(m.Quantity),
                                        Extension.FormatCurrency(nfi,m.UnitPrice),
                                        Extension.FormatCurrency(nfi,m.GrossPrice),                                        
                                        Convert.ToString(m.Discount)+"%",
                                        Extension.FormatCurrency(nfi,m.NetPrice),
                                        m.Type};

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterMaterialBundle.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetQuoteBasedMaterialBundleRecord(int QuoteID, int? MaterialID, int? BundleID, string Type)
        {
            QuoteBundleMaterial oQuoteBundleMaterial = new QuoteBundleMaterial();
            oQuoteBundleMaterial = agileQuote.GetQuoteBaseMaterialBundle(QuoteID, MaterialID, BundleID, Type);

            return Json(new
            {
                oQuoteBundleMaterial
            });
        }

        #endregion

        #region Get Material Bundle List & Single Record

        public JsonResult GetMaterialList(string Category, decimal BudgetTargetRate, jQueryDataTableParamModel param)
        {
            MaterialListProperty oMaterialPropertyList = new MaterialListProperty();
            oMaterialPropertyList.MaterialList = agileQuote.GetMaterialListBasedOnCategory(Category, BudgetTargetRate);
            IList<MaterialProperty> qAllList = new List<MaterialProperty>();
            qAllList = oMaterialPropertyList.MaterialList;
            IEnumerable<MaterialProperty> qFillterMaterial = null;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var DiscountFilter = Convert.ToString(Request["sSearch_3"]);
                var NetPriceFilter = Convert.ToString(Request["sSearch_4"]);

                //Optionally check whether the columns are searchable at all 
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDiscountSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_4"]);

                qFillterMaterial = qAllList.Where(c => isCodeSearchable && Convert.ToString(c.MaterialCode).ToLower().Contains(param.sSearch.ToLower())
                    || isNameSearchable && c.MaterialName.ToLower().Contains(param.sSearch.ToLower())
                    || isDiscountSearchable && c.MaterialDiscount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isNetPriceSearchable && c.MaterialAmount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillterMaterial = qAllList;
            }

            var isCodeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDiscountSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isNetPriceSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<MaterialProperty, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.MaterialCode.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.MaterialName :
                sortColumnIndex == 8 && isDiscountSortable ? c.MaterialDiscount.ToString() :
                sortColumnIndex == 9 && isNetPriceSortable ? c.MaterialAmount.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillterMaterial = qFillterMaterial.OrderBy(orderingFunction);
            }
            else
            {
                qFillterMaterial = qFillterMaterial.OrderByDescending(orderingFunction);
            }

            var displayMaterial = qFillterMaterial.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol((int)Session["QuoteID"], null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.MaterialId), Convert.ToString(m.MaterialCode), m.MaterialName, Convert.ToString(m.MaterialDiscount) + "%", 
                             Extension.FormatCurrency(nfi, m.MaterialAmount), Convert.ToString(m.Quantity) 
                             };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterMaterial.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaterialListGuid(string MinValue, string MaxValue, decimal? minAmount, decimal? maxAmount, string Category, decimal BudgetTargetRate, jQueryDataTableParamModel param)
        {
            MaterialListProperty oMaterialPropertyList = new MaterialListProperty();
            oMaterialPropertyList.MaterialList = agileQuote.GetMaterialListBasedOnCategory(Category, BudgetTargetRate);
            IList<MaterialProperty> qAllList = new List<MaterialProperty>();

            if (string.IsNullOrEmpty(MinValue) && string.IsNullOrEmpty(MaxValue) && minAmount == null && maxAmount == null)
            {
                qAllList = oMaterialPropertyList.MaterialList;
            }
            else if (!string.IsNullOrEmpty(MinValue) && string.IsNullOrEmpty(MaxValue) && minAmount == null && maxAmount == null)
            {
                var allList = oMaterialPropertyList.MaterialList;
                var minValue = allList.Min(x => x.MaterialAmount);
                var oMinList = allList.Where(x => x.MaterialAmount == minValue).ToList();
                qAllList = oMinList;
            }
            else if (string.IsNullOrEmpty(MinValue) && !string.IsNullOrEmpty(MaxValue) && minAmount == null && maxAmount == null)
            {
                var allList = oMaterialPropertyList.MaterialList;
                var maxValue = allList.Max(x => x.MaterialAmount);
                var oMaxList = allList.Where(x => x.MaterialAmount == maxValue).ToList();
                qAllList = oMaxList;
            }
            else
            {
                var allList = oMaterialPropertyList.MaterialList;
                var oRangeList = allList.Where(x => x.MaterialAmount >= minAmount.Value && x.MaterialAmount <= maxAmount.Value).ToList();
                qAllList = oRangeList;
            }

            IEnumerable<MaterialProperty> qFillterMaterial = null;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var DiscountFilter = Convert.ToString(Request["sSearch_3"]);
                var NetPriceFilter = Convert.ToString(Request["sSearch_4"]);

                //Optionally check whether the columns are searchable at all 
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDiscountSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_4"]);

                qFillterMaterial = qAllList.Where(c => isCodeSearchable && Convert.ToString(c.MaterialCode).ToLower().Contains(param.sSearch.ToLower())
                    || isNameSearchable && c.MaterialName.ToLower().Contains(param.sSearch.ToLower())
                    || isDiscountSearchable && c.MaterialDiscount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isNetPriceSearchable && c.MaterialAmount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillterMaterial = qAllList;
            }

            var isCodeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDiscountSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isNetPriceSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<MaterialProperty, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.MaterialCode.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.MaterialName :
                sortColumnIndex == 8 && isDiscountSortable ? c.MaterialDiscount.ToString() :
                sortColumnIndex == 9 && isNetPriceSortable ? c.MaterialAmount.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillterMaterial = qFillterMaterial.OrderBy(orderingFunction);
            }
            else
            {
                qFillterMaterial = qFillterMaterial.OrderByDescending(orderingFunction);
            }

            var displayMaterial = qFillterMaterial.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol((int)Session["QuoteID"], null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.MaterialId), Convert.ToString(m.MaterialCode), m.MaterialName, Convert.ToString(m.MaterialDiscount) + "%", 
                             Extension.FormatCurrency(nfi, m.MaterialAmount), Convert.ToString(m.Quantity) 
                             };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterMaterial.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBundleList(string Category, decimal BudgetTargetRate, jQueryDataTableParamModel param)
        {
            BundleListProperty oBundlePropertyList = new BundleListProperty();
            oBundlePropertyList.BundleList = agileQuote.GetBundleListBasedOnCategory(Category, BudgetTargetRate);

            IList<BundleProperty> qAllList = new List<BundleProperty>();
            qAllList = oBundlePropertyList.BundleList;
            IEnumerable<BundleProperty> qFillterBundle = null;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var DiscountFilter = Convert.ToString(Request["sSearch_3"]);
                var NetPriceFilter = Convert.ToString(Request["sSearch_4"]);

                //Optionally check whether the columns are searchable at all 
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDiscountSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_4"]);

                qFillterBundle = qAllList.Where(c => isCodeSearchable && Convert.ToString(c.BundleId).ToLower().Contains(param.sSearch.ToLower())
                    || isNameSearchable && c.BundleName.ToLower().Contains(param.sSearch.ToLower())
                    || isDiscountSearchable && c.BundleDiscount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isNetPriceSearchable && c.BundleNetPrice.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillterBundle = qAllList;
            }

            var isCodeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDiscountSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isNetPriceSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<BundleProperty, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.BundleId.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.BundleName :
                sortColumnIndex == 8 && isDiscountSortable ? c.BundleDiscount.ToString() :
                sortColumnIndex == 9 && isNetPriceSortable ? c.BundleNetPrice.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillterBundle = qFillterBundle.OrderBy(orderingFunction);
            }
            else
            {
                qFillterBundle = qFillterBundle.OrderByDescending(orderingFunction);
            }

            var displayMaterial = qFillterBundle.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol((int)Session["QuoteID"], null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.BundleId), Convert.ToString(m.BundleId), m.BundleName, Convert.ToString(m.BundleDiscount) + "%", 
                             Extension.FormatCurrency(nfi, m.BundleNetPrice), Convert.ToString(m.Quantity) };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterBundle.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSugectionMaterialList(int MaterialId, jQueryDataTableParamModel param)
        {
            MaterialListProperty oMaterialPropertyList = new MaterialListProperty();
            oMaterialPropertyList.MaterialList = agileQuote.GetSugectionMaterial(MaterialId);
            IList<MaterialProperty> qAllList = new List<MaterialProperty>();
            qAllList = oMaterialPropertyList.MaterialList;
            IEnumerable<MaterialProperty> qFillterMaterial = null;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var DiscountFilter = Convert.ToString(Request["sSearch_3"]);
                var NetPriceFilter = Convert.ToString(Request["sSearch_4"]);

                //Optionally check whether the columns are searchable at all 
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDiscountSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_4"]);

                qFillterMaterial = qAllList.Where(c => isCodeSearchable && Convert.ToString(c.MaterialCode).ToLower().Contains(param.sSearch.ToLower())
                    || isNameSearchable && c.MaterialName.ToLower().Contains(param.sSearch.ToLower())
                    || isDiscountSearchable && c.MaterialDiscount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isNetPriceSearchable && c.MaterialAmount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillterMaterial = qAllList;
            }

            var isCodeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDiscountSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isNetPriceSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<MaterialProperty, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.MaterialCode.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.MaterialName :
                sortColumnIndex == 8 && isDiscountSortable ? c.MaterialDiscount.ToString() :
                sortColumnIndex == 9 && isNetPriceSortable ? c.MaterialAmount.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillterMaterial = qFillterMaterial.OrderBy(orderingFunction);
            }
            else
            {
                qFillterMaterial = qFillterMaterial.OrderByDescending(orderingFunction);
            }

            var displayMaterial = qFillterMaterial.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol((int)Session["QuoteID"], null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.MaterialId), Convert.ToString(m.MaterialCode), m.MaterialName, Convert.ToString(m.MaterialDiscount) + "%", 
                             Extension.FormatCurrency(nfi, m.MaterialAmount), Convert.ToString(m.Quantity) 
                             };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterMaterial.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOfferedMaterialList(int MaterialId, jQueryDataTableParamModel param)
        {
            MaterialListProperty oMaterialPropertyList = new MaterialListProperty();
            oMaterialPropertyList.MaterialList = agileQuote.GetOfferedMaterial(MaterialId);
            IList<MaterialProperty> qAllList = new List<MaterialProperty>();
            qAllList = oMaterialPropertyList.MaterialList;
            IEnumerable<MaterialProperty> qFillterMaterial = null;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var CodeFilter = Convert.ToString(Request["sSearch_1"]);
                var NameFilter = Convert.ToString(Request["sSearch_2"]);
                var DiscountFilter = Convert.ToString(Request["sSearch_3"]);
                var NetPriceFilter = Convert.ToString(Request["sSearch_4"]);

                //Optionally check whether the columns are searchable at all 
                var isCodeSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDiscountSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isNetPriceSearchable = Convert.ToBoolean(Request["bSearchable_4"]);

                qFillterMaterial = qAllList.Where(c => isCodeSearchable && Convert.ToString(c.MaterialCode).ToLower().Contains(param.sSearch.ToLower())
                    || isNameSearchable && c.MaterialName.ToLower().Contains(param.sSearch.ToLower())
                    || isDiscountSearchable && c.MaterialDiscount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isNetPriceSearchable && c.MaterialAmount.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillterMaterial = qAllList;
            }

            var isCodeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDiscountSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isNetPriceSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<MaterialProperty, string> orderingFunction = (c => sortColumnIndex == 1 && isCodeSortable ? c.MaterialCode.ToString() :
                sortColumnIndex == 2 && isNameSortable ? c.MaterialName :
                sortColumnIndex == 8 && isDiscountSortable ? c.MaterialDiscount.ToString() :
                sortColumnIndex == 9 && isNetPriceSortable ? c.MaterialAmount.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
            {
                qFillterMaterial = qFillterMaterial.OrderBy(orderingFunction);
            }
            else
            {
                qFillterMaterial = qFillterMaterial.OrderByDescending(orderingFunction);
            }

            var displayMaterial = qFillterMaterial.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            string symbol = agileQuote.GetSymbol((int)Session["QuoteID"], null);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;
            nfi.CurrencyDecimalDigits = 0;

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.MaterialId), Convert.ToString(m.MaterialCode), m.MaterialName, Convert.ToString(m.MaterialDiscount) + "%", 
                             Extension.FormatCurrency(nfi, m.MaterialAmount), Convert.ToString(m.Quantity) 
                             };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillterMaterial.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMinimumMaximumAmount()
        {
            List<decimal> Amounts = agileQuote.GetMinimumMaximumAmount();
            return Json(new
            {
                Amounts
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMaterialDetail(int MaterialID, decimal BudgetTargetRate)
        {
            MaterialProperty mMaterialProperty = new MaterialProperty();
            mMaterialProperty = agileQuote.GetMaterialSingleRecord(MaterialID, BudgetTargetRate);
            return Json(new
            {
                mMaterialProperty
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBundleDetails(int BundleID, decimal BudgetTargetRate)
        {
            BundleProperty bBundleProperty = new BundleProperty();
            bBundleProperty = agileQuote.GetBundleSingleRecord(BundleID, BudgetTargetRate);
            return Json(new
            {
                bBundleProperty
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBundleMappingMaterial(int BundleID, decimal BudgetTargetRate)
        {
            string redirectAction = string.Empty;
            bool redirect = false;


            if (Session["UserID"] != null)
            {
                var Result = agileQuote.GetBundleMappingMaterial(BundleID, BudgetTargetRate);
                return Json(new
                {
                    redirect,
                    redirectAction,
                    Result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                redirectAction = Url.Action("Login", "Account");
                redirect = true;
                return Json(new
                {
                    redirect,
                    redirectAction
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        [HttpPost]
        public JsonResult GetCategoryNameList()
        {
            List<string> CategoryName = new List<string>();
            CategoryName = agileQuote.GetCategoryNameList();
            return Json(CategoryName);
        }

        #region Add Material Bundle to Quote

        [HttpPost]
        public JsonResult AddMaterialBundleToQuote(int QuoteID, string MaterialList, string BundleList, string SugectionMaterial, string OfferedMaterial, string GuidMaterial)
        {
            List<MaterialBundleValue> mMaterialList = new List<MaterialBundleValue>();
            List<MaterialBundleValue> bBundleList = new List<MaterialBundleValue>();
            List<MaterialBundleValue> oOfferedMaterialList = new List<MaterialBundleValue>();

            MaterialBundleValue oMaterialBundleValue = null;

            if (!string.IsNullOrEmpty(MaterialList))
            {
                var MaterialQuantityList = MaterialList.Split(',');
                foreach (var item in MaterialQuantityList)
                {
                    oMaterialBundleValue = new MaterialBundleValue();
                    var mObject = item.Split('_');
                    oMaterialBundleValue.Code = Convert.ToInt32(mObject[0]);
                    oMaterialBundleValue.Quantity = Convert.ToInt32(mObject[1]);

                    mMaterialList.Add(oMaterialBundleValue);
                }
            }

            if (!string.IsNullOrEmpty(BundleList))
            {
                var BundleQuantityList = BundleList.Split(',');
                foreach (var item in BundleQuantityList)
                {
                    oMaterialBundleValue = new MaterialBundleValue();
                    var bObject = item.Split('_');
                    oMaterialBundleValue.Code = Convert.ToInt32(bObject[0]);
                    oMaterialBundleValue.Quantity = Convert.ToInt32(bObject[1]);

                    bBundleList.Add(oMaterialBundleValue);
                }
            }

            if (!string.IsNullOrEmpty(SugectionMaterial))
            {
                var oSugMaterial = SugectionMaterial.Split(',');
                foreach (var item in oSugMaterial)
                {
                    var sugIds = item.Split(':');
                    var mId = Convert.ToInt32(sugIds[0]);
                    var obj = mMaterialList.Where(x => x.Code == mId).FirstOrDefault();
                    if (obj != null)
                    {
                        mMaterialList.Remove(obj);
                    }
                    var oSugMaterialQuanity = sugIds[1].Split('_');

                    oMaterialBundleValue = new MaterialBundleValue();
                    oMaterialBundleValue.Code = Convert.ToInt32(oSugMaterialQuanity[0]);
                    oMaterialBundleValue.Quantity = Convert.ToInt32(oSugMaterialQuanity[1]);
                    mMaterialList.Add(oMaterialBundleValue);
                }
            }

            if (!string.IsNullOrEmpty(OfferedMaterial))
            {
                var oOfferMaterial = OfferedMaterial.Split(',');
                foreach (var item in oOfferMaterial)
                {
                    var offIds = item.Split(':');
                    var oOfferMaterialQuantity = offIds[1].Split('_');
                    int oOfferId = Convert.ToInt32(oOfferMaterialQuantity[0]);

                    var obj = mMaterialList.Where(x => x.Code == oOfferId).FirstOrDefault();
                    if (obj != null)
                    {
                        mMaterialList.Remove(obj);
                    }
                    oMaterialBundleValue = new MaterialBundleValue();
                    oMaterialBundleValue.Code = Convert.ToInt32(oOfferMaterialQuantity[0]);
                    oMaterialBundleValue.Quantity = Convert.ToInt32(oOfferMaterialQuantity[1]);
                    mMaterialList.Add(oMaterialBundleValue);
                }
            }

            if (!string.IsNullOrEmpty(GuidMaterial))
            {
                var oGuidMaterial = GuidMaterial.Split(',');
                foreach (var item in oGuidMaterial)
                {
                    var ooGuidMaterial = item.Split('_');
                    int GuidMaterialId = Convert.ToInt32(ooGuidMaterial[0]);
                    var obj = mMaterialList.Where(x => x.Code == GuidMaterialId).FirstOrDefault();
                    if (obj != null)
                    {
                        mMaterialList.Remove(obj);
                    }

                    oMaterialBundleValue = new MaterialBundleValue();
                    oMaterialBundleValue.Code = GuidMaterialId;
                    oMaterialBundleValue.Quantity = Convert.ToInt32(ooGuidMaterial[1]);
                    mMaterialList.Add(oMaterialBundleValue);
                }
            }

            var result = agileQuote.AddBundleMaterialToQuote(QuoteID, mMaterialList, bBundleList);
            //var resultAssign = agileQuote.AssignedApproverToQuote(QuoteID);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete Material Bundle to Quote

        [HttpPost]
        public JsonResult DeleteMaterialBundleList(int QuoteID, int? MaterialID, int? BundleID, string Type)
        {
            var result = agileQuote.DeleteBundleMaterialRecord(QuoteID, MaterialID, BundleID, Type);
            return Json(new
            {
                result
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update Material Bundle to Quote

        [HttpPost]
        public JsonResult UpdateMaterialBundle(int QuoteID, int? MaterialID, int? BundleID, int Quantity, decimal NetPrice, string Type)
        {
            var result = agileQuote.UpdateBundleMaterialRecord(QuoteID, MaterialID, BundleID, Type, Quantity, NetPrice);
            //var resultAssign = agileQuote.AssignedApproverToQuote(QuoteID);
            return Json(new
            {
                result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMaterialBundleDiscount(int QuoteID, int? MaterialID, int? BundleID, string Type, int Discount)
        {
            var result = agileQuote.UpdateBundleMaterialRecordDiscount(QuoteID, Type, MaterialID, BundleID, Discount);
            return Json(new
            {
                result
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Total UnitPrice Discount NetPrice

        public JsonResult GetTotalQuoteMaterialBundlePrice(int QuoteID)
        {
            TotalUnitDiscount oTotalUnitDiscount = agileQuote.GetQuoteMaterialBundleTotal(QuoteID);
            var result = agileQuote.InsertQuotePriceDetails(QuoteID);
            //var resultAssign = agileQuote.AssignedApproverToQuote(QuoteID);
            return Json(new
            {
                oTotalUnitDiscount
            }, JsonRequestBehavior.AllowGet);

        }

        #endregion

    }
}
