using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;
using AgileQuoteWebApplication.Models;

namespace AgileQuoteWebApplication.Controllers
{
    public class ProposalController : Controller
    {
        //
        // GET: /Proposal/

        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        public ActionResult ProposalCoverPage()
        {
            var imgPath = HttpRuntime.AppDomainAppPath + "Content\\images\\sample.jpg";

            string convertPagestring = "<html><head></head><body style='border: 0px none; margin: 0px; width: 100%;'><table style='width: 100%; border: 0px none; padding: 0px;'><tbody><tr><td><div><img style='width:100%;' src='" + imgPath + "'></div></td></tr></tbody></table></body></html>";

            String htmlText = convertPagestring.ToString();
            Bitmap image = new Bitmap(imgPath);
            float width = image.Width;
            float height = image.Height;

            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            document.SetPageSize(new iTextSharp.text.Rectangle(width + 5, height + 5));
            PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "Content\\pdf\\coverpage\\Coverpage.pdf", FileMode.Create));
            document.Open();

            iTextSharp.text.html.simpleparser.HTMLWorker hw =
                         new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(htmlText));
            document.Close();

            return View();
        }

        public ActionResult ProposalContentPage()
        {
            ViewBag.QuoteID = 1500;
            decimal QuotePrice = ConstantValues.Zero;

            AgileQuoteProperty.Model.QuoteSummary oQuoteSummary = new QuoteSummary();

            QuoteBundleMaterialList qQutoeBundleMaterialList = new QuoteBundleMaterialList();
            qQutoeBundleMaterialList = agileQuote.GetQuoteBaseMaterialBundleList(1500);

            QuotePrice = QuotePrice + Extension.StringToDecimal(qQutoeBundleMaterialList.qQuoteBundleMaterial.qTotalNetPrice);

            Quote qQuote = new Quote();
            qQuote = agileQuote.GetQuoteDetails(1500, 2001);

            ViewBag.symbol = agileQuote.GetSymbol(1500, null);

            QuoteQualitativeInformation qQuoteQualitativeInformation = new QuoteQualitativeInformation();
            qQuoteQualitativeInformation = agileQuote.GetQualitativeInfomation(1500);

            oQuoteSummary.summaryQuote = qQuote;
            oQuoteSummary.summaryQuoteBundle = qQutoeBundleMaterialList;
            oQuoteSummary.summaryQuoteInstallationCharges = agileQuote.GetQuoteShippingDetails(1500);
            oQuoteSummary.summaryQuoteBoughtoutItem = agileQuote.GetQuoteBasedBoughtoutItemList(1500);
            oQuoteSummary.summaryQuoteBoughtoutItemTotal = agileQuote.GetTotalCostBoughtoutItem(1500);
            oQuoteSummary.summaryQuoteBundleWarrenty = agileQuote.GetQuoteMaterialBundleListwithwarranty(1500);
            oQuoteSummary.summaryQuoteShipping = agileQuote.GetQuoteShippingList(1500);
            oQuoteSummary.summaryQuoteShippingTotalCost = agileQuote.GetQuoteShippingTotal(1500);
            QuotePrice = QuotePrice + Extension.StringToDecimal(oQuoteSummary.summaryQuoteInstallationCharges.TotalCost);
            QuotePrice = QuotePrice + Extension.StringToDecimal(oQuoteSummary.summaryQuoteBoughtoutItemTotal.TotalQuotedNetPrice);
            QuotePrice = QuotePrice + oQuoteSummary.summaryQuoteShippingTotalCost;
            oQuoteSummary.summaryQuotePrice = QuotePrice;
            oQuoteSummary.summaryQuoteQualitativeInformation = qQuoteQualitativeInformation;

            int QuoteId = 1500;
            Session["UserID"] = 2001;

            var document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "Content\\pdf\\contentpage\\Content.pdf", FileMode.Create));
            document.SetPageSize(new iTextSharp.text.Rectangle(900, 1300));
            //writer.PageEvent = new PDFFooter();

            document.Open();

            string symbol = agileQuote.GetSymbol(QuoteId, null);
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.CurrencySymbol = symbol;

            pdfDataLogic oPdfData = new pdfDataLogic();
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
            document.Close();

            var path = Request.PhysicalApplicationPath + "Content\\pdf\\contentpage\\Content.pdf";
            PdfReader pReader = new PdfReader(path);
            int number = pReader.NumberOfPages;

            return View(oQuoteSummary);
        }

        public ActionResult PdfAboutUs()
        {
            string Title = "About Us";
            string ParagraphOne = "AgileQuote is being developed as a product in agile way to be used for Quote creation." +
                                  "In the competitive world where snatching order with best possible factors implied," +
                                  "AgileQuote provides the cutting edge to Configure, Process and Quote deliverables (CPQ)." +
                                  "AgileQuote is being developed as a product in agile way to be used for Quote creation." +
                                  "In the competitive world where snatching order with best possible factors implied," +
                                  "AgileQuote provides the cutting edge to Configure, Process and Quote deliverables (CPQ).";

            iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("helvetica", 20, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#5C5C5F")));
            iTextSharp.text.Font ContentFont = iTextSharp.text.FontFactory.GetFont("helvetica", 13, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#5C5C5F")));
            iTextSharp.text.Font TableHeadFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#5C5C5F")));

            iTextSharp.text.Font h1 = iTextSharp.text.FontFactory.GetFont("Arial", 24, iTextSharp.text.Font.BOLD);

            iTextSharp.text.Font h2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD);

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Request.PhysicalApplicationPath + "Content\\pdf\\aboutus\\aboutus.pdf", FileMode.Create));
            doc.SetPageSize(new iTextSharp.text.Rectangle(900, 1300));
            doc.Open();

            PdfPTable aTable = new PdfPTable(1);
            aTable.TotalWidth = 900;
            aTable.LockedWidth = true;
            aTable.DefaultCell.Border = 0;
            aTable.SpacingAfter = 30f;

            PdfPCell oPdfCell = new PdfPCell(new Phrase("About Us", HeadFont));
            oPdfCell.MinimumHeight = ConstantValues.Float30;
            oPdfCell.PaddingLeft = ConstantValues.Float45;
            oPdfCell.PaddingTop = ConstantValues.Float7;
            oPdfCell.Border = ConstantValues.Int0;
            aTable.AddCell(oPdfCell);


            doc.Add(aTable);

            Paragraph p = new Paragraph(new Phrase(ParagraphOne, ContentFont));
            p.FirstLineIndent = 30f;
            doc.Add(p);
            doc.Add(new Chunk(Environment.NewLine));

            Paragraph p1 = new Paragraph(new Phrase("It provides the following functions:", ContentFont));
            p1.FirstLineIndent = 30f;
            doc.Add(p1);
            doc.Add(new Chunk(Environment.NewLine));

            Paragraph p2 = new Paragraph(new Phrase("1. Create Quotes", ContentFont));
            p2.FirstLineIndent = 45f;
            doc.Add(p2);
            doc.Add(new Chunk(Environment.NewLine));

            Paragraph p3 = new Paragraph(new Phrase("2. Approval WorkFlow - Sequential and Parallel", ContentFont));
            p3.FirstLineIndent = 45f;
            doc.Add(p3);
            doc.Add(new Chunk(Environment.NewLine));

            Paragraph p4 = new Paragraph(new Phrase("3. Refer market price connecting with external providers", ContentFont));
            p4.FirstLineIndent = 45f;
            doc.Add(p4);
            doc.Add(new Chunk(Environment.NewLine));

            Paragraph p5 = new Paragraph(new Phrase("4. Refer potential customer connecting with Linked In profiles", ContentFont));
            p5.FirstLineIndent = 45f;
            doc.Add(p5);
            doc.Add(new Chunk(Environment.NewLine));

            Paragraph p6 = new Paragraph(new Phrase("AgileQuote is being developed as a product in agile way to be used for Quote creation. In the competitive world where snatching order with best possible factors implied,AgileQuote provides the cutting edge to Configure, Process and Quote deliverables (CPQ). ", ContentFont));
            p6.FirstLineIndent = 30f;
            doc.Add(p6);

            doc.Add(new Chunk(Environment.NewLine));

            doc.Close();
            return View();
        }

        public ActionResult MergePdf()
        {
            string[] sourceFiles = { Request.PhysicalApplicationPath + "Content\\pdf\\coverpage\\Cover Page.pdf", Request.PhysicalApplicationPath + "Content\\pdf\\aboutus\\About Us.pdf", Request.PhysicalApplicationPath + "Content\\pdf\\contentpage\\Content.pdf" };
            
            try
            {
                string destinationFile = Request.PhysicalApplicationPath + "Content\\pdf\\1.pdf";
                int f = 0;
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(sourceFiles[f]);
                // we retrieve the total number of pages
                int n = reader.NumberOfPages;
                //Console.WriteLine("There are " + n + " pages in the original file.");
                // step 1: creation of a document-object
                Document document = new Document(reader.GetPageSizeWithRotation(1));
                // step 2: we create a writer that listens to the document
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));
                writer.PageEvent = new PDFFooter();
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
                writer.Close();
                // step 5: we close the document
                document.Close();


            }
            catch (Exception e)
            {
                string strOb = e.Message;
            }

            return View();
        }


        public ActionResult copy()
        {
            PdfReader reader1 = new PdfReader(Request.PhysicalApplicationPath + "Content\\pdf\\1.pdf");
            Document doc = new Document();
            PdfWriter w = PdfWriter.GetInstance(doc, new FileStream(Request.PhysicalApplicationPath + "Content\\pdf\\2.pdf", FileMode.Create));
            w.PageEvent = new PDFFooter();
            doc.Open();
            doc.AddDocListener(w);
            for (int i = 1; i <= reader1.NumberOfPages; i++)
            {
                doc.SetPageSize(reader1.GetPageSize(i));
                doc.NewPage();
                PdfContentByte cb1 = w.DirectContent;
                PdfImportedPage page1 = w.GetImportedPage(reader1, i);
                int oRotation = reader1.GetPageRotation(i);
                if (oRotation == 90 || oRotation == 270)
                {
                    cb1.AddTemplate(page1, 0, -1f, 1f, 0, 0, reader1.GetPageSizeWithRotation(i).Height);
                }
                else
                {
                    cb1.AddTemplate(page1, 1f, 0, 0, 1f, 0, 0);
                }
            }

            doc.Close();
            return View();
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
            }

            // write on end of each page
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                /*Header Content*/

                var fontColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#9DD4FF"));
                iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("helvetica", 24, iTextSharp.text.Font.BOLD, fontColor);
                iTextSharp.text.Font HeadFontOne = iTextSharp.text.FontFactory.GetFont("helvetica", 24, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

                iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "\\Content\\images\\agile-quote-logo.png");
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


                /*Footer Content*/
                Paragraph footer = new Paragraph(writer.PageNumber.ToString(), ContentFont);
                Paragraph footerComent = new Paragraph("© Copyright 2013, AgileQuote, All Rights Reserved", ContentFont);
                footer.Alignment = Element.ALIGN_RIGHT;
                footerComent.Alignment = Element.ALIGN_CENTER;

                PdfPTable footerTbl = new PdfPTable(2);
                footerTbl.TotalWidth = document.PageSize.Width;
                footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell = new PdfPCell(footer);
                cell.PaddingRight = 30f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                PdfPCell cell1 = new PdfPCell(footerComent);
                cell1.PaddingLeft = 30f;
                cell1.Border = 0;
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;

                footerTbl.AddCell(cell1);
                footerTbl.AddCell(cell);

                footerTbl.WriteSelectedRows(0, -1, 0, 40, writer.DirectContent);

            }

            //write on close of document
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
            }
        }
    }
}


#region html to pdf


//var imgPath = HttpRuntime.AppDomainAppPath + "\\Content\\images\\qoute-ion.png";

//string sampleString = "<table bgcolor='#F5F5F5' width='100%' cellspacing='0' cellpadding='0'>" +
//        "<tr>" +
//            "<td width='2%'></td>" +
//            "<td width='6%'><img alt='img1' src='" + imgPath + "' width='30' height='30'></td>" +
//            "<td><p><font size='2' color='#555555'><b>Quote Details</b></font></p></td>" +
//        "</tr>" +
//    "</table><br/>";
//if (qQuote != null)
//{
//    sampleString += "<table bgcolor='#FFFFFF' width='100%' cellspacing='0' cellpadding='0'>" +
//           "<tr>" +
//                "<td width='4%' height='30'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Quote Name</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='20%'><font size='1' color='#555555'>" + qQuote.QuoteName + "</font></td>" +
//                "<td width='4%'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Sales Organization Name</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='24%'><font size='1' color='#555555'>" + qQuote.qQuoteSalesOrgName + "</font></td>" +
//            "</tr>" +
//            "<tr>" +
//                "<td width='4%'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Customer Code</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='20%'><font size='1' color='#555555'>" + qQuote.CustomerCode + "</font></td>" +
//                "<td width='4%'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Customer Name</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='24%'><font size='1' color='#555555'>" + qQuote.CustomerName + "</font></td>" +
//            "</tr>" +
//            "<tr>" +
//                "<td width='4%'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Requested Date</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='20%'><font size='1' color='#555555'>" + qQuote.RequrestedDate + "</font></td>" +
//                "<td width='4%'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Currency Name</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='24%'><font size='1' color='#555555'>" + qQuote.currencyName + "</font></td>" +
//            "</tr>" +
//            "<tr>" +
//                "<td width='4%'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Budget Value</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='20%'><font size='1' color='#555555'>" + qQuote.BudgetValue + "</font></td>" +
//                "<td width='4%'></td>" +
//                "<td width='19%'><font size='1' color='#555555'>Prepared By</font></td>" +
//                "<td width='1%'><font size='1' color='#555555'>:</font></td>" +
//                "<td width='24%'><font size='1' color='#555555'>" + qQuote.PreparedBy + "</font></td>" +
//            "</tr>" +
//        "</table>";
//}


//Document document1 = new Document(PageSize.A4, 0, 0, 0, 0);
//// document1.SetPageSize(new iTextSharp.text.Rectangle(900, 1300));
//var writer=PdfWriter.GetInstance(document1, new FileStream(Request.PhysicalApplicationPath + "Content\\pdf\\contentpage\\Coverpage.pdf", FileMode.Create));
//writer.PageEvent = new PDFFooter();
//document1.Open();

//iTextSharp.text.html.simpleparser.HTMLWorker hw1 =
//             new iTextSharp.text.html.simpleparser.HTMLWorker(document1);
////var sw = new StringReader(ProposalContentString);
////List<IElement> parselist = HTMLWorker.ParseToList(sw, null);
////foreach (var item in parselist)
////{
////    document1.Add(item);
////}
//document1.Add(new Phrase(Environment.NewLine));
//hw1.Parse(new StringReader(sampleString));
//document1.Close();
#endregion