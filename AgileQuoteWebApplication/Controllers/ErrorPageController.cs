using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;

namespace AgileQuoteWebApplication.Controllers
{
    public class ErrorPageController : Controller
    {      
        [HttpGet]
        public ActionResult ShowError(Error eError)
        {
            return View(eError);
        }

    }
}
