using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileQuoteProperty.Model;
using Recaptcha;
using System.Configuration;
using LogWriter;
using System.ServiceModel;
using AgileQuoteProperty.Common;


namespace AgileQuoteWebApplication.Controllers
{
    public class AccountController : Controller
    {
        AgileQuoteService.IService1 QuoteService = new AgileQuoteService.Service1Client();

        [HttpGet]
        public ActionResult Login()
        {
            int random;
            Random rNumber = new Random();
            Login oLogin = new AgileQuoteProperty.Model.Login();
            try
            {
                random = rNumber.Next(ConstantValues.TenThousand, ConstantValues.NintyNineThousand);
                ViewBag.RandomNumber = random;
                Session[ConstantValues.Random] = random;
                oLogin.CompanyList = QuoteService.CompanyList();
            }
            catch (Exception ex)
            {
                LogWriter.LogWriter.Log(ex.StackTrace);
            }
            return View(oLogin);
        }

        [HttpPost]
        public ActionResult Login(Login oLogin)
        {
            try
            {
                RecaptchaValidator recaptchaValidator = new RecaptchaValidator();
                recaptchaValidator.Challenge = HttpContext.Request.Form[ConstantValues.recaptcha_challenge_field];
                recaptchaValidator.Response = HttpContext.Request.Form[ConstantValues.recaptcha_response_field];
                recaptchaValidator.PrivateKey = ConfigurationManager.AppSettings[ConstantValues.ReCaptchaPrivateKey].ToString();
                recaptchaValidator.RemoteIP = HttpContext.Request.UserHostAddress;
                RecaptchaResponse recaptchaResponse = !string.IsNullOrEmpty(recaptchaValidator.Challenge) ? (!string.IsNullOrEmpty(recaptchaValidator.Response) ? recaptchaValidator.Validate() : RecaptchaResponse.InvalidResponse) : RecaptchaResponse.InvalidChallenge;
                oLogin.CompanyList = QuoteService.CompanyList();
                bool isValid = recaptchaResponse.IsValid;

                if (ModelState.IsValid)
                {
                    //if (isValid)
                    //{
                    //int Random = (int)Session["Random"];
                    //if (Random == oLogin.RandomNumber)
                    //{
                    if (QuoteService.CheckAuthentication(oLogin))
                    {
                        var User = QuoteService.GetUserID(oLogin);
                        foreach (var item in User)
                        {
                            Session["UserID"] = item.Key;
                            Session["UserName"] = item.Value;
                            Session["CompanyName"] = oLogin.CompanyName;
                        }
                        //string RoleName = QuoteService.GetUserRole(oLogin);

                        //if (RoleName == ConstantValues.financeVP || RoleName == ConstantValues.salesVP)
                        //{
                        //    //return RedirectToAction("ApproverQuote", "QuoteList");
                        //    //return RedirectToAction("QuoteSummary", "QuoteList", new { QuoteID = 1500 });
                        //    //return RedirectToAction("ApproveQuoteForUser", "QuoteList");
                        //    return RedirectToAction("MyQuoteForUser", "QuoteList");
                        //}
                        //else
                        //{
                        //    return RedirectToAction("ApproveQuoteForUser", "QuoteList");
                        //}

                        return RedirectToAction("QuoteBoard", "QuoteBoard");

                    }
                    else
                    {
                        if (QuoteService.CheckUsername(oLogin))
                        {
                            ModelState.AddModelError(string.Empty, "Password provide incorrect");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User name provide incorrect");
                        }

                    }
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelError(string.Empty, "Secret number provide incorrect");
                    //    }
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError(string.Empty, "Recaptcha text mismatch");
                    //}
                }
                int random;
                Random rNumber = new Random();
                random = rNumber.Next(10000, 99999);
                ViewBag.RandomNumber = random;
                Session[ConstantValues.Random] = random;
                return View(oLogin);

            }
            catch (FaultException<Error> e)
            {
                LogWriter.LogWriter.Log("Error message:- " + e.Detail.ErrorMessage + " Error Details:-" + e.Detail.ErrorDetails);
                Error error = new Error();
                error.ErrorDetails = e.Detail.ErrorDetails;
                error.ErrorMessage = e.Detail.ErrorMessage;
                return RedirectToAction("ShowError", "ErrorPage", error);
            }
        }

        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["QuoteID"] = null;
            return RedirectToAction("Login", "Account");
        }

    }
}
