using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Controllers
{
    public class QuoteCollabratorController : Controller
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        #region Quote Collabrator

        public ActionResult QuoteCollabrator(int QuoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.QuoteID = QuoteID;

            ViewBag.symbol = agileQuote.GetSymbol(QuoteID, null);

            return PartialView();
        }

        public JsonResult GetQuoteCollabratorList(jQueryDataTableParamModel param, int QuoteID)
        {
            IList<QuoteCollabrationModel> qAllList = new List<QuoteCollabrationModel>();
            qAllList = agileQuote.GetcollabratorListOnQuote(QuoteID);
            IEnumerable<QuoteCollabrationModel> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var UserFilter = Convert.ToString(Request["sSearch_1"]);
                var StatusFilter = Convert.ToString(Request["sSearch_2"]);

                //Optionally check whether the columns are searchable at all
                var isUserSearchable = true; //Convert.ToBoolean(Request["bSearchable_1"]);
                var isStatusSearchable = true; //Convert.ToBoolean(Request["bSearchable_2"]);

                qFillter = qAllList.Where(c => isUserSearchable && Convert.ToString(c.UserID).ToLower().Contains(param.sSearch.ToLower())
                    || isStatusSearchable && c.Status.ToLower().Contains(param.sSearch.ToLower())
                    );

            }
            else
            {
                qFillter = qAllList;
            }

            var isUserSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isStatusSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteCollabrationModel, string> orderingFunction = (c => sortColumnIndex == 1 && isUserSortable ? c.UserID.ToString() :
                sortColumnIndex == 2 && isStatusSortable ? c.Status :
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
                         select new[] { Convert.ToString(m.UserID),
                                        m.Status, m.StatusDescription                                   
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleNameList()
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            object Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.GetRoleNameList();
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

        public JsonResult InsertCollabratorToQuote(string UserID, int QuoteID)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            List<int> UserIDList = new List<int>();
            Dictionary<int, string> UserIDRemarksList = new Dictionary<int, string>();

            if (!string.IsNullOrEmpty(UserID))
            {
                var oList = UserID.Split(',');
                foreach (var item in oList)
                {
                    var uObject = item.Split('_');
                    UserIDRemarksList.Add(Convert.ToInt32(uObject[0]), Convert.ToString(uObject[1]));
                    //UserIDList.Add(Convert.ToInt32(item));
                }
            }

            if (Session["UserID"] != null)
            {
                Result = agileQuote.InsertCollabratorQuote(QuoteID, UserIDRemarksList);
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

        public JsonResult GetQuoteCollabratorListLength(int QuoteID)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            IList<QuoteCollabrationModel> qAllList = new List<QuoteCollabrationModel>();
            qAllList = agileQuote.GetcollabratorListOnQuote(QuoteID);

            if (Session["UserID"] != null)
            {
                Result = qAllList.Count.ToString();
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

        public JsonResult GetCollabratorRemarks(int QuoteID)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.GetCollaboratorRemarks(QuoteID, (int)Session["UserID"]);
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

        public JsonResult UpdateCollabratorToQuote(int QuoteID, string QuoteStatusDescription)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                redirect = true;
                Result = agileQuote.UpdateCollabratorStatusForQuote(QuoteID, (int)Session["UserID"], ConstantValues.QuoteCollaborationCompleted, QuoteStatusDescription, false);
                redirectAction = Url.Action("QuoteBoard", "QuoteBoard");
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

        public JsonResult DeleteCollabratorToQuote(int QuoteID, string QuoteStatusDescription)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Result = agileQuote.UpdateCollabratorStatusForQuote(QuoteID, (int)Session["UserID"], ConstantValues.QuoteRejectStatus, QuoteStatusDescription, true);
                redirectAction = Url.Action("QuoteBoard", "QuoteBoard");
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

        public JsonResult GetApproverListLength(int QuoteID)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                IList<ApproverQuoteStatus> qAllList = new List<ApproverQuoteStatus>();
                qAllList = agileQuote.GetApproverQuoteStatusList(QuoteID);
                Result = qAllList.Count.ToString();
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

        public JsonResult GetCollaboratorList(jQueryDataTableParamModel param, int QuoteID)
        {
            IList<QuoteCollabrationModel> qAllList = new List<QuoteCollabrationModel>();
            qAllList = agileQuote.GetCollaboratorList(QuoteID);
            IEnumerable<QuoteCollabrationModel> qFillter = null;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var UserFilter = Convert.ToString(Request["sSearch_1"]);
                var RollNameFilter = Convert.ToString(Request["sSearch_2"]);
                var EmailAddressFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all
                var isUserSearchable = true; //Convert.ToBoolean(Request["bSearchable_1"]);
                var isRoleNameSearchable = true; //Convert.ToBoolean(Request["bSearchable_2"]);
                var isEmailAddressSearchable = true;

                qFillter = qAllList.Where(c => isUserSearchable && Convert.ToString(c.UserID).ToLower().Contains(param.sSearch.ToLower())
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
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            Func<QuoteCollabrationModel, string> orderingFunction = (c => sortColumnIndex == 1 && isUserSortable ? c.UserID.ToString() :
                sortColumnIndex == 2 && isRoleNameSortable ? c.RoleName :
                sortColumnIndex == 3 && isEmailAddressSortable ? c.EmailAddress :
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
                         select new[] { Convert.ToString(m.UserID),Convert.ToString(m.UserID),
                                        m.RoleName, m.EmailAddress                                   
                                       };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = qAllList.Count(),
                iTotalDisplayRecords = qFillter.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
