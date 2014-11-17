using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;
using AgileQuoteWebApplication.Models;
using System.Net;
using System.Net.Configuration;
using System.Configuration;
using System.Net.Mail;

namespace AgileQuoteWebApplication.Controllers
{
    public class PdfController : Controller
    {

        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();
        pdfDataLogic oPdfData = new pdfDataLogic();

        public ActionResult GeneratePdf()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }



            int QuoteId = (int)Session["QuoteID"];
            var mStream = new MemoryStream();
            var document = new Document(PageSize.A4, 0, 0, 0, 40f);
            PdfWriter writer = PdfWriter.GetInstance(document, mStream);
            writer.PageEvent = new PDFFooter();
            document.Open();

            string QuoteName = oPdfData.GetQuoteNameForPdf(QuoteId, (int)Session["UserID"]);

            document.Add(oPdfData.PdfHeaderQuoteDetails());
            document.Add(oPdfData.QuoteDetailsTable(QuoteId, (int)Session["UserID"]));
            document.Add(oPdfData.QuoteAddressHeaderTable());
            document.Add(oPdfData.QuoteAddressTable(QuoteId, (int)Session["UserID"]));
            document.Add(oPdfData.QuoteBundleMaterialHeader());
            document.Add(oPdfData.QuoteBundleMaterialTable(QuoteId));
            document.Add(oPdfData.QuoteBundleMaterialTotalTable(QuoteId));
            document.Add(oPdfData.QuoteBoughtOutItemHeaderTable());
            document.Add(oPdfData.QuoteBoughtOutItemTable(QuoteId));
            document.Add(oPdfData.QuoteBoughtOutItemTotalTable(QuoteId));
            document.Add(oPdfData.QuoteMaterialBundleWarrentyHeader());
            document.Add(oPdfData.QuoteMaterialBundleWarrenty(QuoteId));
            document.Add(oPdfData.QuoteMaterialBundleInstallationHeader());
            document.Add(oPdfData.QuoteMaterialBundleInstallation(QuoteId));
            document.Add(oPdfData.QuoteShippingTableHeader());
            document.Add(oPdfData.QuoteShippingTable(QuoteId));
            document.Add(oPdfData.QuoteQualitativeInformationTableHeader());
            document.Add(oPdfData.QuoteQualitativeInformationTable(QuoteId));
            document.Add(oPdfData.QuotePriceTabelHeader());
            document.Add(oPdfData.QuotePriceTable(QuoteId));
            document.Close();
            writer.Flush();

            return new DownloadFile(mStream.GetBuffer(), QuoteName + ".pdf");
        }

        [HttpGet]
        public ActionResult Proposal(int QuoteId)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.QuoteId = QuoteId;
            return View();
        }

        [HttpPost]
        public JsonResult Proposal(string ArrangeString, int QuoteId)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                ContentPdfGenerate(QuoteId, (int)Session["UserID"]);
                string coverPage = Request.PhysicalApplicationPath + "Content\\pdf\\coverpage\\Cover Page.pdf";
                string aboutUs = Request.PhysicalApplicationPath + "Content\\pdf\\aboutus\\About Us.pdf";
                string contentPage = Request.PhysicalApplicationPath + "Content\\pdf\\contentpage\\Content_" + QuoteId + ".pdf";
                string productInformation = Request.PhysicalApplicationPath + "Content\\pdf\\productinformation\\Product Information.pdf";
                string overview = Request.PhysicalApplicationPath + "Content\\pdf\\overview\\Overview.pdf";
                string sapErpIntegration = Request.PhysicalApplicationPath + "Content\\pdf\\saperpintegration\\SAP - ERP Integration.pdf";
                //string mergeFile = Request.PhysicalApplicationPath + "Content\\pdf\\mergedocument\\merge.pdf";
                string originalFile = Request.PhysicalApplicationPath + "Content\\pdf\\proposal\\proposal_" + QuoteId + ".pdf";
                bool coverpageCheck = false;
                List<string> fileList = new List<string>();

                if (ArrangeString != null)
                {
                    var ArrangeList = ArrangeString.Split(',');
                    foreach (var item in ArrangeList)
                    {
                        switch (item)
                        {
                            case "CoverPage":
                                coverpageCheck = true;
                                fileList.Add(coverPage);
                                break;
                            case "AboutUs":
                                fileList.Add(aboutUs);
                                break;
                            case "ProductInformation":
                                fileList.Add(productInformation);
                                break;
                            case "Overview":
                                fileList.Add(overview);
                                break;
                            case "QuoteDetails":
                                fileList.Add(contentPage);
                                break;
                            case "SapErpIntegration":
                                fileList.Add(sapErpIntegration);
                                break;
                        }
                    }
                }

                ProposalGeneratePdf(fileList, false, originalFile);

                //if (coverpageCheck)
                //{
                //    fileList.Clear();
                //    fileList.Add(coverPage);
                //    fileList.Add(mergeFile);
                //    ProposalGeneratePdf(fileList, false, originalFile);
                //}

                //fileList.Clear();
                //fileList.Add(contentPage);
                //fileList.Add(mergeFile);
                //foreach (var item in fileList)
                //{
                //    if (System.IO.File.Exists(item))
                //    {
                //        System.IO.File.Delete(item);
                //    }
                //}

                //if (System.IO.File.Exists(contentPage))
                //{
                //    System.IO.File.Delete(contentPage);
                //}

                redirectAction = Url.Action("SendProposal", "Pdf", new { QuoteId = QuoteId });
                redirect = true;

                //SendMail();
                Result = "Proposal Created successfully";
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
        public ActionResult SendProposal(int QuoteId)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.QuoteId = QuoteId;
            ViewBag.name = "proposal_" + QuoteId + ".pdf";
            return View();
        }

        [HttpPost]
        public JsonResult SendproposalMail(string From, string To, string CC, string BCC, string Subject, string Body, string Attachment)
        {
            string redirectAction = string.Empty;
            bool redirect = false;
            string Result = string.Empty;

            if (Session["UserID"] != null)
            {
                Body = Body.Replace('(', '<');
                Body = Body.Replace(')', '>');

                int QuoteId = (int)Session["QuoteID"];
                string path = Request.PhysicalApplicationPath + "Content\\pdf\\proposal\\proposal_" + QuoteId + ".pdf";

                SmtpSection settings = (SmtpSection)(ConfigurationManager.GetSection("system.net/mailSettings/smtp"));
                SmtpClient client = new SmtpClient();
                MailAddress sendTo = new MailAddress(To);
                MailAddress fromAddress = new MailAddress(From);
                MailMessage message = new MailMessage(fromAddress, sendTo);
                if (!string.IsNullOrWhiteSpace(CC))
                {
                    message.CC.Add(CC);
                }

                if (!string.IsNullOrWhiteSpace(BCC))
                {
                    message.Bcc.Add(BCC);
                }

                message.IsBodyHtml = true;
                message.Subject = Subject;
                message.Body = Body;
                message.Attachments.Add(new Attachment(path));

                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(settings.Network.UserName, settings.Network.Password);
                client.Port = settings.Network.Port;
                client.Host = settings.Network.Host;
                client.UseDefaultCredentials = false;
                client.Credentials = basicAuthenticationInfo;
                //client.EnableSsl = true;
                client.Send(message);
                client.Dispose();
                redirect = true;
                redirectAction = Url.Action("QuoteBoard", "QuoteBoard");
                string deleteFile = Request.PhysicalApplicationPath + "Content\\pdf\\contentpage\\Content_" + QuoteId + ".pdf";
                if (System.IO.File.Exists(deleteFile))
                {
                    System.IO.File.Delete(deleteFile);
                }
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

        public void ContentPdfGenerate(int QuoteId, int UserId)
        {
            string path = Request.PhysicalApplicationPath + "Content\\pdf\\contentpage\\Content_" + QuoteId + ".pdf";

            using (var document = new Document(PageSize.A4))
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    using (var writer = PdfWriter.GetInstance(document, fs))
                    {
                        writer.PageEvent = new PDFFooter();
                        document.Open();
                        string QuoteName = oPdfData.GetQuoteNameForPdf(QuoteId, UserId);
                        document.Add(oPdfData.PdfHeaderQuoteDetails());
                        document.Add(oPdfData.QuoteDetailsTable(QuoteId, UserId));
                        document.Add(oPdfData.QuoteAddressHeaderTable());
                        document.Add(oPdfData.QuoteAddressTable(QuoteId, UserId));
                        document.Add(oPdfData.QuoteBundleMaterialHeader());
                        document.Add(oPdfData.QuoteBundleMaterialTable(QuoteId));
                        document.Add(oPdfData.QuoteBundleMaterialTotalTable(QuoteId));
                        document.Add(oPdfData.QuoteBoughtOutItemHeaderTable());
                        document.Add(oPdfData.QuoteBoughtOutItemTable(QuoteId));
                        document.Add(oPdfData.QuoteBoughtOutItemTotalTable(QuoteId));
                        document.Add(oPdfData.QuoteMaterialBundleWarrentyHeader());
                        document.Add(oPdfData.QuoteMaterialBundleWarrenty(QuoteId));
                        document.Add(oPdfData.QuoteMaterialBundleInstallationHeader());
                        document.Add(oPdfData.QuoteMaterialBundleInstallation(QuoteId));
                        document.Add(oPdfData.QuoteShippingTableHeader());
                        document.Add(oPdfData.QuoteShippingTable(QuoteId));
                        document.Add(oPdfData.QuoteQualitativeInformationTableHeader());
                        document.Add(oPdfData.QuoteQualitativeInformationTable(QuoteId));
                        document.Dispose();
                    }
                }
            }
        }

        public void SendMail()
        {
            SmtpSection settings = (SmtpSection)(ConfigurationManager.GetSection("system.net/mailSettings/smtp"));
            SmtpClient client = new SmtpClient();
            MailAddress sendTo = new MailAddress("antonysamy.j@changepond.com");
            MailAddress fromAddress = new MailAddress("antonysamy.j@changepond.com");
            MailMessage message = new MailMessage(fromAddress, sendTo);
            //message.CC.Add();
            //message.Bcc.Add();
            message.IsBodyHtml = true;
            message.Subject = "AgileQuote | Not Just Quotes - Winning Quotes";
            message.Body = "Welcome Sir";
            message.Attachments.Add(new Attachment(Request.PhysicalApplicationPath + "Content\\pdf\\proposal\\proposal.pdf"));

            System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(settings.Network.UserName, settings.Network.Password);
            client.Port = settings.Network.Port;
            client.Host = settings.Network.Host;
            client.UseDefaultCredentials = false;
            client.Credentials = basicAuthenticationInfo;
            //client.EnableSsl = true;
            client.Send(message);
        }

        public void ProposalGeneratePdf(List<string> Files, bool header, string destinationFile)
        {

            string[] sourceFiles = Files.ToArray();

            int f = 0;
            // we create a reader for a certain document
            PdfReader reader = new PdfReader(sourceFiles[f]);
            int n = reader.NumberOfPages;

            using (var document = new Document(reader.GetPageSizeWithRotation(1)))
            {
                using (var fs = new FileStream(destinationFile, FileMode.Create))
                {
                    using (var writer = PdfWriter.GetInstance(document, fs))
                    {
                        if (header)
                        {
                            writer.PageEvent = new PDFFooter();
                        }
                        // step 3: we open the document
                        document.Open();
                        PdfContentByte cb = writer.DirectContent;
                        PdfImportedPage page;
                        int rotation;
                        // step 4: we add content
                        while (f < sourceFiles.Length)
                        {
                            int i = 0;
                            while (i < n)
                            {
                                i++;
                                document.SetPageSize(reader.GetPageSizeWithRotation(i));
                                //document.SetPageSize(new iTextSharp.text.Rectangle(900, 1300));
                                document.NewPage();
                                page = writer.GetImportedPage(reader, i);
                                rotation = reader.GetPageRotation(i);
                                if (rotation == 90 || rotation == 270)
                                {
                                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                                }
                                else
                                {
                                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                                }
                                //Console.WriteLine("Processed page " + i);
                            }
                            f++;
                            if (f < sourceFiles.Length)
                            {
                                reader = new PdfReader(sourceFiles[f]);
                                // we retrieve the total number of pages
                                n = reader.NumberOfPages;
                                //Console.WriteLine("There are " + n + " pages in the original file.");
                            }
                        }
                        // step 5: we close the document
                        document.Close();
                    }
                }
            }

            //int f = 0;
            //// we create a reader for a certain document
            //PdfReader reader = new PdfReader(sourceFiles[f]);
            //// we retrieve the total number of pages
            //int n = reader.NumberOfPages;
            ////Console.WriteLine("There are " + n + " pages in the original file.");
            //// step 1: creation of a document-object
            //Document document = new Document(reader.GetPageSizeWithRotation(1));
            //// step 2: we create a writer that listens to the document
            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));
            //if (header)
            //{
            //    writer.PageEvent = new PDFFooter();
            //}
            //// step 3: we open the document
            //document.Open();
            //PdfContentByte cb = writer.DirectContent;
            //PdfImportedPage page;
            //int rotation;
            //// step 4: we add content
            //while (f < sourceFiles.Length)
            //{
            //    int i = 0;
            //    while (i < n)
            //    {
            //        i++;
            //        document.SetPageSize(reader.GetPageSizeWithRotation(i));
            //        //document.SetPageSize(new iTextSharp.text.Rectangle(900, 1300));
            //        document.NewPage();
            //        page = writer.GetImportedPage(reader, i);
            //        rotation = reader.GetPageRotation(i);
            //        if (rotation == 90 || rotation == 270)
            //        {
            //            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
            //        }
            //        else
            //        {
            //            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
            //        }
            //        //Console.WriteLine("Processed page " + i);
            //    }
            //    f++;
            //    if (f < sourceFiles.Length)
            //    {
            //        reader = new PdfReader(sourceFiles[f]);
            //        // we retrieve the total number of pages
            //        n = reader.NumberOfPages;
            //        //Console.WriteLine("There are " + n + " pages in the original file.");
            //    }
            //}
            //writer.Close();
            //// step 5: we close the document
            //document.Close();           
        }


        /// <summary>
        /// Header Footer shown
        /// </summary>
        public class PDFFooter : PdfPageEventHelper
        {
            iTextSharp.text.Font ContentFont = iTextSharp.text.FontFactory.GetFont("helvetica", 13, iTextSharp.text.Font.NORMAL);

            // write on top of document
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                base.OnOpenDocument(writer, document);
            }

            // write on start of each page
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                var fontColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#9DD4FF"));
                iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("helvetica", 24, iTextSharp.text.Font.BOLD, fontColor);
                iTextSharp.text.Font HeadFontOne = iTextSharp.text.FontFactory.GetFont("helvetica", 24, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

                iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "Content\\images\\agile-quote-logo.png");
                imgPDF.ScaleAbsolute(115, 25);

                Paragraph first = new Paragraph("Agile", HeadFont);
                Paragraph second = new Paragraph("Quote", HeadFontOne);

                PdfPTable tabFot = new PdfPTable(1);
                tabFot.TotalWidth = document.PageSize.Width;
                tabFot.LockedWidth = true;
                tabFot.KeepTogether = true;

                PdfPCell cell2 = new PdfPCell(imgPDF);
                cell2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#10588F"));
                cell2.Border = 0;
                cell2.MinimumHeight = 30f;
                cell2.PaddingLeft = 30f;
                cell2.PaddingTop = 10f;
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;

                tabFot.AddCell(cell2);

                document.Add(new Phrase(Environment.NewLine));
                document.Add(new Phrase(Environment.NewLine));

                tabFot.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);

            }

            // write on end of each page
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                /*Header Content*/

                //var fontColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#9DD4FF"));
                //iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("helvetica", 24, iTextSharp.text.Font.BOLD, fontColor);
                //iTextSharp.text.Font HeadFontOne = iTextSharp.text.FontFactory.GetFont("helvetica", 24, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

                //iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "Content\\images\\agile-quote-logo.png");
                //imgPDF.ScaleAbsolute(115, 25);

                //Paragraph first = new Paragraph("Agile", HeadFont);
                //Paragraph second = new Paragraph("Quote", HeadFontOne);

                //PdfPTable tabFot = new PdfPTable(1);
                //tabFot.TotalWidth = document.PageSize.Width;
                //tabFot.LockedWidth = true;
                //tabFot.KeepTogether = true;

                //PdfPCell cell2 = new PdfPCell(imgPDF);
                //cell2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#10588F"));
                //cell2.Border = 0;
                //cell2.MinimumHeight = 30f;
                //cell2.PaddingLeft = 30f;
                //cell2.PaddingTop = 10f;
                //cell2.HorizontalAlignment = Element.ALIGN_LEFT;

                //tabFot.AddCell(cell2);

                //document.Add(new Phrase(Environment.NewLine));
                //document.Add(new Phrase(Environment.NewLine));

                //tabFot.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);


                ///*Footer Content*/
                //Paragraph footer = new Paragraph(writer.PageNumber.ToString(), ContentFont);
                //Paragraph footerComent = new Paragraph("© Copyright 2013, AgileQuote, All Rights Reserved", ContentFont);
                //footer.Alignment = Element.ALIGN_RIGHT;
                //footerComent.Alignment = Element.ALIGN_CENTER;

                //PdfPTable footerTbl = new PdfPTable(2);
                //footerTbl.TotalWidth = document.PageSize.Width;
                //footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
                //footerTbl.SpacingBefore = 30f;

                //PdfPCell cell = new PdfPCell(footer);
                //cell.PaddingRight = 30f;
                //cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                //PdfPCell cell1 = new PdfPCell(footerComent);
                //cell1.PaddingLeft = 30f;
                //cell1.Border = 0;
                //cell1.HorizontalAlignment = Element.ALIGN_LEFT;

                //footerTbl.AddCell(cell1);
                //footerTbl.AddCell(cell);

                //footerTbl.WriteSelectedRows(0, -1, 0, 40, writer.DirectContent);

            }

            //write on close of document
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
            }
        }
    }
}
