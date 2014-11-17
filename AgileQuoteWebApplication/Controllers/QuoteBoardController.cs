using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteBoardController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        public ActionResult QuoteBoard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public JsonResult GetApproveWithCollabrationQuoteDatatableList(jQueryDataTableParamModel param)
        {
            QuoteDetails qDetails = new QuoteDetails();
            IList<QuoteDetails> qAllList = new List<QuoteDetails>();
            qAllList = agileQuote.GetQuoteListForApprove((int)Session["UserID"]);
            //qAllList = agileQuote.GetQuoteList(qDetails, (int)Session["UserID"]);
            IEnumerable<QuoteDetails> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var QuoteIdFilter = Convert.ToString(Request["sSearch_1"]);
                var QuoteNameFilter = Convert.ToString(Request["sSearch_2"]);
                var SalesOrganizationFilter = Convert.ToString(Request["sSearch_3"]);
                var CustomerNameFilter = Convert.ToString(Request["sSearch_4"]);
                var PrepareByFilter = Convert.ToString(Request["sSearch_5"]);
                var CreateDateFilter = Convert.ToString(Request["sSearch_6"]);
                var RequireDateFilter = Convert.ToString(Request["sSearch_7"]);
                var StatusFilter = Convert.ToString(Request["sSearch_8"]);

                //Optionally check whether the columns are searchable at all
                var isQuoteIdSearchable = true; //Convert.ToBoolean(Request["bSearchable_1"]);
                var isQuoteNameSearchable = true; //Convert.ToBoolean(Request["bSearchable_2"]);
                var isSalesOrganizationSearchable = true;
                var isCustomerNameSearchable = true;
                var isPrepareBySearchable = true;
                var isCreateDateSearchable = true;
                var isRequireDateSearchable = true;
                var isStatusSearchable = true;

                qFillter = qAllList.Where(c => isQuoteIdSearchable && Convert.ToString(c.qQuoteID).ToLower().Contains(param.sSearch.ToLower())
                    || isQuoteNameSearchable && c.qQuoteName.ToLower().Contains(param.sSearch.ToLower())
                    || isSalesOrganizationSearchable && c.qSalesOrganization.ToLower().Contains(param.sSearch.ToLower())
                    || isCustomerNameSearchable && c.qCustomerName.ToLower().Contains(param.sSearch.ToLower())
                    || isPrepareBySearchable && c.qPrepareBy.ToLower().Contains(param.sSearch.ToLower())
                    || isCreateDateSearchable && Convert.ToString(c.displayCreateDate).ToLower().Contains(param.sSearch.ToLower())
                    || isRequireDateSearchable && Convert.ToString(c.displayRequireDate).ToLower().Contains(param.sSearch.ToLower())
                    || isStatusSearchable && c.Status.ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillter = qAllList;
            }

            var isQuoteIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isQuoteNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isSalesOrganizationSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isCustomerNameSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isPrepareBySortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isCreateDateSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isRequireDateSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isStatusSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteDetails, string> orderingFunction = (c => sortColumnIndex == 1 && isQuoteIdSortable ? c.qQuoteID.ToString() :
                sortColumnIndex == 2 && isQuoteNameSortable ? c.qQuoteName :
                sortColumnIndex == 3 && isSalesOrganizationSortable ? c.qSalesOrganization :
                sortColumnIndex == 4 && isCustomerNameSortable ? c.qCustomerName :
                sortColumnIndex == 5 && isPrepareBySortable ? c.qPrepareBy :
                sortColumnIndex == 6 && isCreateDateSortable ? c.displayCreateDate.ToString() :
                sortColumnIndex == 7 && isRequireDateSortable ? c.displayRequireDate.ToString() :
                sortColumnIndex == 8 && isStatusSortable ? c.Status :
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

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.qQuoteID),
                                        m.qQuoteName,m.qSalesOrganization,m.qCustomerName,m.qPrepareBy,
                                        Convert.ToString(m.displayCreateDate),Convert.ToString(m.displayRequireDate),m.Status,m.AccessType,
                                        Convert.ToString(m.qLevel)
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyQuoteQuoteDatatableList(jQueryDataTableParamModel param)
        {
            QuoteDetails qDetails = new QuoteDetails();
            IList<QuoteDetails> qAllList = new List<QuoteDetails>();
            qAllList = agileQuote.GetQuoteListForCreate((int)Session["UserID"]);
            //qAllList = agileQuote.GetQuoteList(qDetails, (int)Session["UserID"]);
            IEnumerable<QuoteDetails> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var QuoteIdFilter = Convert.ToString(Request["sSearch_1"]);
                var QuoteNameFilter = Convert.ToString(Request["sSearch_2"]);
                var SalesOrganizationFilter = Convert.ToString(Request["sSearch_3"]);
                var CustomerNameFilter = Convert.ToString(Request["sSearch_4"]);
                var PrepareByFilter = Convert.ToString(Request["sSearch_5"]);
                var CreateDateFilter = Convert.ToString(Request["sSearch_6"]);
                var RequireDateFilter = Convert.ToString(Request["sSearch_7"]);
                var StatusFilter = Convert.ToString(Request["sSearch_8"]);

                //Optionally check whether the columns are searchable at all
                var isQuoteIdSearchable = true; //Convert.ToBoolean(Request["bSearchable_1"]);
                var isQuoteNameSearchable = true; //Convert.ToBoolean(Request["bSearchable_2"]);
                var isSalesOrganizationSearchable = true;
                var isCustomerNameSearchable = true;
                var isPrepareBySearchable = true;
                var isCreateDateSearchable = true;
                var isRequireDateSearchable = true;
                var isStatusSearchable = true;

                qFillter = qAllList.Where(c => isQuoteIdSearchable && Convert.ToString(c.qQuoteID).ToLower().Contains(param.sSearch.ToLower())
                    || isQuoteNameSearchable && c.qQuoteName.ToLower().Contains(param.sSearch.ToLower())
                    || isSalesOrganizationSearchable && c.qSalesOrganization.ToLower().Contains(param.sSearch.ToLower())
                    || isCustomerNameSearchable && c.qCustomerName.ToLower().Contains(param.sSearch.ToLower())
                    || isPrepareBySearchable && c.qPrepareBy.ToLower().Contains(param.sSearch.ToLower())
                    || isCreateDateSearchable && Convert.ToString(c.displayCreateDate).ToLower().Contains(param.sSearch.ToLower())
                    || isRequireDateSearchable && Convert.ToString(c.displayRequireDate).ToLower().Contains(param.sSearch.ToLower())
                    || isStatusSearchable && c.Status.ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillter = qAllList;
            }

            var isQuoteIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isQuoteNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isSalesOrganizationSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isCustomerNameSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isPrepareBySortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isCreateDateSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isRequireDateSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isStatusSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteDetails, string> orderingFunction = (c => sortColumnIndex == 1 && isQuoteIdSortable ? c.qQuoteID.ToString() :
                sortColumnIndex == 2 && isQuoteNameSortable ? c.qQuoteName :
                sortColumnIndex == 3 && isSalesOrganizationSortable ? c.qSalesOrganization :
                sortColumnIndex == 4 && isCustomerNameSortable ? c.qCustomerName :
                sortColumnIndex == 5 && isPrepareBySortable ? c.qPrepareBy :
                sortColumnIndex == 6 && isCreateDateSortable ? c.displayCreateDate.ToString() :
                sortColumnIndex == 7 && isRequireDateSortable ? c.displayRequireDate.ToString() :
                sortColumnIndex == 8 && isStatusSortable ? c.Status :
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

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.qQuoteID),
                                        m.qQuoteName,m.qSalesOrganization,m.qCustomerName,m.qPrepareBy,
                                        Convert.ToString(m.displayCreateDate),Convert.ToString(m.displayRequireDate),m.Status,m.AccessType
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCollaboratorQuoteDatatableList(jQueryDataTableParamModel param)
        {
            QuoteDetails qDetails = new QuoteDetails();
            IList<QuoteDetails> qAllList = new List<QuoteDetails>();
            qAllList = agileQuote.GetQuoteListForCollaborate((int)Session["UserID"]);
            //qAllList = agileQuote.GetQuoteList(qDetails, (int)Session["UserID"]);
            IEnumerable<QuoteDetails> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var QuoteIdFilter = Convert.ToString(Request["sSearch_1"]);
                var QuoteNameFilter = Convert.ToString(Request["sSearch_2"]);
                var SalesOrganizationFilter = Convert.ToString(Request["sSearch_3"]);
                var CustomerNameFilter = Convert.ToString(Request["sSearch_4"]);
                var PrepareByFilter = Convert.ToString(Request["sSearch_5"]);
                var CreateDateFilter = Convert.ToString(Request["sSearch_6"]);
                var RequireDateFilter = Convert.ToString(Request["sSearch_7"]);
                var StatusFilter = Convert.ToString(Request["sSearch_8"]);

                //Optionally check whether the columns are searchable at all
                var isQuoteIdSearchable = true; //Convert.ToBoolean(Request["bSearchable_1"]);
                var isQuoteNameSearchable = true; //Convert.ToBoolean(Request["bSearchable_2"]);
                var isSalesOrganizationSearchable = true;
                var isCustomerNameSearchable = true;
                var isPrepareBySearchable = true;
                var isCreateDateSearchable = true;
                var isRequireDateSearchable = true;
                var isStatusSearchable = true;

                qFillter = qAllList.Where(c => isQuoteIdSearchable && Convert.ToString(c.qQuoteID).ToLower().Contains(param.sSearch.ToLower())
                    || isQuoteNameSearchable && c.qQuoteName.ToLower().Contains(param.sSearch.ToLower())
                    || isSalesOrganizationSearchable && c.qSalesOrganization.ToLower().Contains(param.sSearch.ToLower())
                    || isCustomerNameSearchable && c.qCustomerName.ToLower().Contains(param.sSearch.ToLower())
                    || isPrepareBySearchable && c.qPrepareBy.ToLower().Contains(param.sSearch.ToLower())
                    || isCreateDateSearchable && Convert.ToString(c.displayCreateDate).ToLower().Contains(param.sSearch.ToLower())
                    || isRequireDateSearchable && Convert.ToString(c.displayRequireDate).ToLower().Contains(param.sSearch.ToLower())
                    || isStatusSearchable && c.Status.ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillter = qAllList;
            }

            var isQuoteIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isQuoteNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isSalesOrganizationSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isCustomerNameSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isPrepareBySortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isCreateDateSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isRequireDateSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isStatusSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteDetails, string> orderingFunction = (c => sortColumnIndex == 1 && isQuoteIdSortable ? c.qQuoteID.ToString() :
                sortColumnIndex == 2 && isQuoteNameSortable ? c.qQuoteName :
                sortColumnIndex == 3 && isSalesOrganizationSortable ? c.qSalesOrganization :
                sortColumnIndex == 4 && isCustomerNameSortable ? c.qCustomerName :
                sortColumnIndex == 5 && isPrepareBySortable ? c.qPrepareBy :
                sortColumnIndex == 6 && isCreateDateSortable ? c.displayCreateDate.ToString() :
                sortColumnIndex == 7 && isRequireDateSortable ? c.displayRequireDate.ToString() :
                sortColumnIndex == 8 && isStatusSortable ? c.Status :
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

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.qQuoteID),
                                        m.qQuoteName,m.qSalesOrganization,m.qCustomerName,m.qPrepareBy,
                                        Convert.ToString(m.displayCreateDate),Convert.ToString(m.displayRequireDate),m.Status,m.AccessType,
                                        Convert.ToString(m.qLevel)
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubmitedQuoteDatatableList(jQueryDataTableParamModel param)
        {
            QuoteDetails qDetails = new QuoteDetails();
            IList<QuoteDetails> qAllList = new List<QuoteDetails>();
            qAllList = agileQuote.GetAuthorizedRejectedQuoteList((int)Session["UserID"]);
            //qAllList = agileQuote.GetQuoteList(qDetails, (int)Session["UserID"]);
            IEnumerable<QuoteDetails> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var QuoteIdFilter = Convert.ToString(Request["sSearch_1"]);
                var QuoteNameFilter = Convert.ToString(Request["sSearch_2"]);
                var SalesOrganizationFilter = Convert.ToString(Request["sSearch_3"]);
                var CustomerNameFilter = Convert.ToString(Request["sSearch_4"]);
                var PrepareByFilter = Convert.ToString(Request["sSearch_5"]);
                var CreateDateFilter = Convert.ToString(Request["sSearch_6"]);
                var RequireDateFilter = Convert.ToString(Request["sSearch_7"]);
                var StatusFilter = Convert.ToString(Request["sSearch_8"]);

                //Optionally check whether the columns are searchable at all
                var isQuoteIdSearchable = true; //Convert.ToBoolean(Request["bSearchable_1"]);
                var isQuoteNameSearchable = true; //Convert.ToBoolean(Request["bSearchable_2"]);
                var isSalesOrganizationSearchable = true;
                var isCustomerNameSearchable = true;
                var isPrepareBySearchable = true;
                var isCreateDateSearchable = true;
                var isRequireDateSearchable = true;
                var isStatusSearchable = true;

                qFillter = qAllList.Where(c => isQuoteIdSearchable && Convert.ToString(c.qQuoteID).ToLower().Contains(param.sSearch.ToLower())
                    || isQuoteNameSearchable && c.qQuoteName.ToLower().Contains(param.sSearch.ToLower())
                    || isSalesOrganizationSearchable && c.qSalesOrganization.ToLower().Contains(param.sSearch.ToLower())
                    || isCustomerNameSearchable && c.qCustomerName.ToLower().Contains(param.sSearch.ToLower())
                    || isPrepareBySearchable && c.qPrepareBy.ToLower().Contains(param.sSearch.ToLower())
                    || isCreateDateSearchable && Convert.ToString(c.displayCreateDate).ToLower().Contains(param.sSearch.ToLower())
                    || isRequireDateSearchable && Convert.ToString(c.displayRequireDate).ToLower().Contains(param.sSearch.ToLower())
                    || isStatusSearchable && c.Status.ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillter = qAllList;
            }

            var isQuoteIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isQuoteNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isSalesOrganizationSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isCustomerNameSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isPrepareBySortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isCreateDateSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isRequireDateSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isStatusSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteDetails, string> orderingFunction = (c => sortColumnIndex == 1 && isQuoteIdSortable ? c.qQuoteID.ToString() :
                sortColumnIndex == 2 && isQuoteNameSortable ? c.qQuoteName :
                sortColumnIndex == 3 && isSalesOrganizationSortable ? c.qSalesOrganization :
                sortColumnIndex == 4 && isCustomerNameSortable ? c.qCustomerName :
                sortColumnIndex == 5 && isPrepareBySortable ? c.qPrepareBy :
                sortColumnIndex == 6 && isCreateDateSortable ? c.displayCreateDate.ToString() :
                sortColumnIndex == 7 && isRequireDateSortable ? c.displayRequireDate.ToString() :
                sortColumnIndex == 8 && isStatusSortable ? c.Status :
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

            var result = from m in displayMaterial
                         select new[] { Convert.ToString(m.qQuoteID),
                                        m.qQuoteName,m.qSalesOrganization,m.qCustomerName,m.qPrepareBy,
                                        Convert.ToString(m.displayCreateDate),Convert.ToString(m.displayRequireDate),m.Status,m.AccessType
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCreateQuoteListCount()
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int Result = 0;

            if (Session["UserID"] != null)
            {
                IList<QuoteDetails> qAllList = new List<QuoteDetails>();
                qAllList = agileQuote.GetQuoteListForCreate((int)Session["UserID"]);
                Result = qAllList.Count;
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

        public JsonResult GetApproverQuoteListCount()
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int Result = 0;

            if (Session["UserID"] != null)
            {
                IList<QuoteDetails> qAllList = new List<QuoteDetails>();
                qAllList = agileQuote.GetQuoteListForApprove((int)Session["UserID"]);
                Result = qAllList.Count;
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

        public JsonResult GetCollaboraterQuoteListCount()
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            int Result = 0;

            if (Session["UserID"] != null)
            {
                IList<QuoteDetails> qAllList = new List<QuoteDetails>();
                qAllList = agileQuote.GetQuoteListForCollaborate((int)Session["UserID"]);
                Result = qAllList.Count;
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

        [HttpGet]
        public ActionResult BrandNewQuote()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

    }
}
