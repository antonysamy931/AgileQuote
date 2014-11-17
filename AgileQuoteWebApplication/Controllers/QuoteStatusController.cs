using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteStatusController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region Quote Status

        public ActionResult QuoteStatus(int QuoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.QuoteID = QuoteID;
            return PartialView();
        }

        public JsonResult GetQuoteStatusList(jQueryDataTableParamModel param, int QuoteID)
        {
            IList<ApproverQuoteStatus> qAllList = new List<ApproverQuoteStatus>();
            qAllList = agileQuote.GetApproverQuoteStatusList(QuoteID);
            IEnumerable<ApproverQuoteStatus> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var UserIdFilter = Convert.ToString(Request["sSearch_1"]);
                var RoleNameFilter = Convert.ToString(Request["sSearch_2"]);
                var EmailAddressFilter = Convert.ToString(Request["sSearch_3"]);
                var StatusFilter = Convert.ToString(Request["sSearch_4"]);

                //Optionally check whether the columns are searchable at all
                var isUserSearchable = true; //Convert.ToBoolean(Request["bSearchable_1"]);
                var isStatusSearchable = true; //Convert.ToBoolean(Request["bSearchable_2"]);
                var isRoleNameSearchable = true;
                var isEmailAddressSearchable = true;

                qFillter = qAllList.Where(c => isUserSearchable && Convert.ToString(c.AuthorizerID).ToLower().Contains(param.sSearch.ToLower())
                    || isStatusSearchable && c.QuoteStatus.ToLower().Contains(param.sSearch.ToLower())
                    || isRoleNameSearchable && c.RoleName.ToLower().Contains(param.sSearch.ToLower())
                    || isEmailAddressSearchable && c.EmailAddress.ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillter = qAllList;
            }

            var isUserSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isRoleNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isEmailAddressSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isStatusSortable = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<ApproverQuoteStatus, string> orderingFunction = (c => sortColumnIndex == 1 && isUserSortable ? c.AuthorizerID.ToString() :
                sortColumnIndex == 2 && isStatusSortable ? c.QuoteStatus :
                sortColumnIndex == 3 && isRoleNameSortable ? c.RoleName :
                sortColumnIndex == 4 && isEmailAddressSortable ? c.EmailAddress :
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
                         select new[] { Convert.ToString(m.AuthorizerID),
                                        m.RoleName,m.EmailAddress,m.QuoteStatus,m.StatusDescription
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AssignApproverToQuote(int QuoteID)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.AssignedApproverToQuote(QuoteID);
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
