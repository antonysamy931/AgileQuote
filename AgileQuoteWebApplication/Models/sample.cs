





////Reference files





///////*
//////* This class is part of the book "iText in Action - 2nd Edition"
//////* written by Bruno Lowagie (ISBN: 9781935182610)
//////* For more info, go to: http://itextpdf.com/examples/
//////* This example only works with the AGPL version of iText.
//////*/
//////using System;
//////using System.IO;
//////using System.Collections.Generic;
//////using System.Data;
//////using System.Data.Common;
//////using System.Linq;
//////using iTextSharp.text;
//////using iTextSharp.text.pdf;
//////using kuujinbo.iTextInAction2Ed.Intro_1_2;
//////namespace kuujinbo.iTextInAction2Ed.Chapter05 {
//////public class MovieCountries1 : IWriter {
//////// ===========================================================================
///////**
//////* Inner class to add a table as header.
//////*/
//////protected class TableHeader : PdfPageEventHelper {
///////** The template with the total number of pages. */
//////PdfTemplate total;
///////** The header text. */
//////public string Header { get; set; }
///////**
//////* Creates the PdfTemplate that will hold the total number of pages.
//////* @see com.itextpdf.text.pdf.PdfPageEventHelper#onOpenDocument(
//////* com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
//////*/
//////public override void OnOpenDocument(PdfWriter writer, Document document) {
//////total = writer.DirectContent.CreateTemplate(30, 16);
//////}
///////**
//////* Adds a header to every page
//////* @see com.itextpdf.text.pdf.PdfPageEventHelper#onEndPage(
//////* com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
//////*/
//////public override void OnEndPage(PdfWriter writer, Document document) {
//////PdfPTable table = new PdfPTable(3);
//////try {
//////table.SetWidths(new int[]{24, 24, 2});
//////table.TotalWidth = 527;
//////table.LockedWidth = true;
//////table.DefaultCell.FixedHeight = 20;
//////table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
//////table.AddCell(Header);
//////table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
//////table.AddCell(string.Format("Page {0} of", writer.PageNumber));
//////PdfPCell cell = new PdfPCell(Image.GetInstance(total));
//////cell.Border = Rectangle.BOTTOM_BORDER;
//////table.AddCell(cell);
//////table.WriteSelectedRows(0, -1, 34, 803, writer.DirectContent);
//////}
//////catch(DocumentException de) {
//////throw de;
//////}
//////}
///////**
//////* Fills out the total number of pages before the document is closed.
//////* @see com.itextpdf.text.pdf.PdfPageEventHelper#onCloseDocument(
//////* com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
//////*/
//////public override void OnCloseDocument(PdfWriter writer, Document document) {
//////ColumnText.ShowTextAligned(
//////total, Element.ALIGN_LEFT,
///////*
//////* NewPage() already called when closing the document; subtract 1
//////*/
//////new Phrase((writer.PageNumber - 1).ToString()),
//////2, 2, 0
//////);
//////}
//////}
//////// ---------------------------------------------------------------------------
//////protected readonly string SQL =
//////@"SELECT country, id FROM film_country
//////ORDER BY country
//////";
//////public virtual void Write(Stream stream) {
//////// step 1
//////using (Document document = new Document(PageSize.A4, 36, 36, 54, 36)) {
//////// step 2
//////PdfWriter writer = PdfWriter.GetInstance(document, stream);
//////TableHeader tevent = new TableHeader();
//////writer.PageEvent = tevent;
//////// step 3
//////document.Open();
//////// step 4
//////using (var c = AdoDB.Provider.CreateConnection()) {
//////c.ConnectionString = AdoDB.CS;
//////using (DbCommand cmd = c.CreateCommand()) {
//////cmd.CommandText = SQL;
//////c.Open();
//////using (var r = cmd.ExecuteReader()) {
//////while (r.Read()) {
//////tevent.Header = r["country"].ToString();
//////// LINQ allows us to sort on any movie object property inline;
//////// let's sort by Movie.Year, Movie.Title
//////var by_year = from m in PojoFactory.GetMovies(
//////r["id"].ToString()
//////)
//////orderby m.Year, m.Title
//////select m
//////;
//////foreach (Movie movie in by_year) {
//////document.Add(new Paragraph(
//////movie.MovieTitle, FilmFonts.BOLD
//////));
//////if (!string.IsNullOrEmpty(movie.OriginalTitle)) document.Add(
//////new Paragraph(movie.OriginalTitle, FilmFonts.ITALIC)
//////);
//////document.Add(new Paragraph(
//////String.Format("Year: {0}; run length: {1} minutes",
//////movie.Year, movie.Duration
//////), FilmFonts.NORMAL
//////));
//////document.Add(PojoToElementFactory.GetDirectorList(movie));
//////}
//////document.NewPage();
//////}
//////}
//////}
//////}
//////}
//////}
//////// ===========================================================================
//////}

/////*
////* This class is part of the book "iText in Action - 2nd Edition"
////* written by Bruno Lowagie (ISBN: 9781935182610)
////* For more info, go to: http://itextpdf.com/examples/
////* This example only works with the AGPL version of iText.
////*/
////using System;
////using System.IO;
////using iTextSharp.text;
////using iTextSharp.text.pdf;
////namespace kuujinbo.iTextInAction2Ed.Chapter05 {
////public class Hero2 : Hero1 {
////// ===========================================================================
////public override void Write(Stream stream) {
////float w = PageSize.A4.Width;
////float h = PageSize.A4.Height;
////Rectangle rect = new Rectangle(-2*w, -2*h, 2*w, 2*h);
////Rectangle crop = new Rectangle(-2 * w, h, -w, 2 * h);
////// step 1
////using (Document document = new Document(rect)) {
////// step 2
////PdfWriter writer = PdfWriter.GetInstance(document, stream);
////writer.CropBoxSize = crop;
////// step 3
////document.Open();
////// step 4
////PdfContentByte content = writer.DirectContent;
////PdfTemplate template = CreateTemplate(content, rect, 4);
////float adjust;
////while(true) {
////content.AddTemplate(template, -2*w, -2*h);
////adjust = crop.Right + w;
////if (adjust > 2 * w) {
////adjust = crop.Bottom - h;
////if (adjust < - 2 * h)
////break;
////crop = new Rectangle(-2*w, adjust, -w, crop.Bottom);
////}
////else {
////crop = new Rectangle(crop.Right, crop.Bottom, adjust, crop.Top);
////}
////writer.CropBoxSize = crop;
////document.NewPage();
////}
////}
////}
////// ---------------------------------------------------------------------------
/////**
////* Creates a template based on a stream of PDF syntax.
////* @param content The direct content
////* @param rect The dimension of the templare
////* @param factor A magnification factor
////* @return A PdfTemplate
////*/
////public override PdfTemplate CreateTemplate(
////PdfContentByte content, Rectangle rect, int factor
////)
////{
////PdfTemplate template = content.CreateTemplate(rect.Width, rect.Height);
////template.ConcatCTM(factor, 0, 0, factor, 0, 0);
////string hero = Path.Combine(Utility.ResourceText, "hero.txt");
////if (!File.Exists(hero)) {
////throw new ArgumentException(hero + " NOT FOUND!");
////}
////var fi = new FileInfo(hero);
////using ( var sr = fi.OpenText() ) {
////while (sr.Peek() >= 0) {
////template.SetLiteral((char) sr.Read());
////}
////}
////return template;
////}
////// ===========================================================================
////}


/////
///// Summary description for HeaderF
/////
//public class HeaderF : PdfPageEventHelper
//{
//    PdfContentByte pdfContent;
//    public override void OnEndPage(PdfWriter writer, Document document)
//    {
//        //Font headerFont = new Font(Font.HELVETICA, 18, Font.BOLD);
//        //Rectangle pageSize = document.PageSize;

//        //PdfPTable headerTbl = new PdfPTable(1);
//        //headerTbl.TotalWidth = pageSize;
//        //headerTbl.HorizontalAlignment = Element.ALIGN_LEFT;
//        //headerTbl.LockedWidth = true;
//        //headerTbl.DefaultCell.Border = 0;

//        //PdfPCell headerCell = new PdfPCell(new Phrase("Agile Quote", headerFont));
//        //headerCell.MinimumHeight = 45f;
//        //headerCell.PaddingLeft = 30f;
//        ////headerCell.PaddingTop = 15f;
//        //headerCell.Border = 0;
//        //headerCell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#10588F"));

//        //headerTbl.AddCell(headerCell);
//        //headerTbl.SpacingAfter = 30.0F;
//        //headerTbl.WriteSelectedRows(0, -1, 10, document.PageSize.Height - 15, writer.DirectContent);

//        //We are going to add two strings in header. Create separate Phrase object with font setting and string to be included
//        //Phrase p1Header = new Phrase("BlueLemonCode generated page", FontFactory.GetFont("verdana", 8));
//        //Phrase p2Header = new Phrase("confidential", FontFactory.GetFont("verdana", 8));
//        ////create iTextSharp.text Image object using local image path
//        //iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "\\Content\\images\\Agile-logo.png");

//        ////Create PdfTable object
//        //PdfPTable pdfTab = new PdfPTable(3);
//        //pdfTab.DefaultCell.FixedHeight = 50f;
//        ////We will have to create separate cells to include image logo and 2 separate strings
//        //PdfPCell pdfCell1 = new PdfPCell(imgPDF);
//        //PdfPCell pdfCell2 = new PdfPCell(p1Header);
//        //PdfPCell pdfCell3 = new PdfPCell(p2Header);
//        ////set the alignment of all three cells and set border to 0
//        //pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
//        //pdfCell2.HorizontalAlignment = Element.ALIGN_LEFT;
//        //pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
//        //pdfCell1.Border = 0;
//        //pdfCell2.Border = 0;
//        //pdfCell2.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#10588F"));
//        ////pdfCell2.MinimumHeight = 55f;
//        //pdfCell3.Border = 0;
//        ////add all three cells into PdfTable
//        //pdfTab.AddCell(pdfCell1);
//        //pdfTab.AddCell(pdfCell2);
//        //pdfTab.AddCell(pdfCell3);
//        //pdfTab.TotalWidth = document.PageSize.Width - 20;
//        ////call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
//        ////first param is start row. -1 indicates there is no end row and all the rows to be included to write
//        ////Third and fourth param is x and y position to start writing
//        //pdfTab.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);
//        ////set pdfContent value
//        //pdfContent = writer.DirectContent;
//        ////Move the pointer and draw line to separate header section from rest of page
//        //pdfContent.MoveTo(30, document.PageSize.Height - 170);
//        ////pdfContent.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 35);
//        //pdfContent.Stroke();

//        iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "\\Content\\images\\Agile-logo.png");
//        PdfPTable tabFot = new PdfPTable(2);
//        tabFot.SpacingAfter = 10f;
//        tabFot.TotalWidth = 900;

//        PdfPCell cell = new PdfPCell(imgPDF);
//        cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#10588F"));
//        cell.Border = 0;
//        cell.PaddingLeft = 30f;
//        cell.HorizontalAlignment = Element.ALIGN_LEFT;

//        PdfPCell cell1 = new PdfPCell(imgPDF);
//        cell1.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#10588F"));
//        cell1.Border = 0;

//        tabFot.AddCell(cell);
//        tabFot.AddCell(cell1);

//        tabFot.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);

//    }
//}

//public partial class Footer : PdfPageEventHelper
//{
//    public override void OnEndPage(PdfWriter writer, Document doc)
//    {
//        Paragraph footer = new Paragraph(writer.PageNumber.ToString(), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
//        footer.Alignment = Element.ALIGN_RIGHT;
//        PdfPTable footerTbl = new PdfPTable(1);
//        footerTbl.TotalWidth = 300;
//        footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
//        PdfPCell cell = new PdfPCell(footer);
//        cell.Border = 0;
//        cell.PaddingLeft = 10;
//        footerTbl.AddCell(cell);
//        footerTbl.WriteSelectedRows(0, -1, 415, 30, writer.DirectContent);
//    }
//}



//protected class TableHeader : PdfPageEventHelper
//{
//    /** The template with the total number of pages. */
//    PdfTemplate total;
//    /** The header text. */
//    public string Header { get; set; }
//    /**
//    * Creates the PdfTemplate that will hold the total number of pages.
//    * @see com.itextpdf.text.pdf.PdfPageEventHelper#onOpenDocument(
//    * com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
//    */
//    public override void OnOpenDocument(PdfWriter writer, Document document)
//    {
//        total = writer.DirectContent.CreateTemplate(30, 500);
//    }
//    /**
//    * Adds a header to every page
//    * @see com.itextpdf.text.pdf.PdfPageEventHelper#onEndPage(
//    * com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
//    */
//    public override void OnEndPage(PdfWriter writer, Document document)
//    {
//        PdfPTable table = new PdfPTable(3);
//        try
//        {
//            table.SetWidths(new int[] { 24, 24, 2 });
//            table.TotalWidth = 900;
//            table.LockedWidth = true;
//            //  table.DefaultCell.FixedHeight = 200;
//            table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
//            table.AddCell(Header);
//            table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
//            table.AddCell(string.Format("Page {0} of", writer.PageNumber));
//            PdfPCell cell = new PdfPCell(Image.GetInstance(total));
//            cell.Border = Rectangle.BOTTOM_BORDER;
//            table.AddCell(cell);
//            table.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);
//        }
//        catch (DocumentException de)
//        {
//            throw de;
//        }
//    }
//    /**
//    * Fills out the total number of pages before the document is closed.
//    * @see com.itextpdf.text.pdf.PdfPageEventHelper#onCloseDocument(
//    * com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
//    */
//    public override void OnCloseDocument(PdfWriter writer, Document document)
//    {
//        ColumnText.ShowTextAligned(
//        total, Element.ALIGN_LEFT,
//            /*
//            * NewPage() already called when closing the document; subtract 1
//            */
//        new Phrase((writer.PageNumber - 1).ToString()),
//        2, 2, 0
//        );
//    }
//}

//public class PageEvents : IPdfPageEvent
//{
//    PdfContentByte _pdfContent;

//    /// <summary>
//    /// To get the header for PDF
//    /// </summary>
//    /// <param name="writer">pdfwriter</param>
//    /// <param name="document">document in which header to be written</param>
//    public void OnStartPage(PdfWriter writer, Document document)
//    {
//        //try
//        //{
//        //Logger.LogInfoMessage("Method Name:OnStartPage  - Started ");
//        //create iTextSharp.text Image object using local image path
//        //iTextSharp.text.Image imgPdf = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + Constant.pdfLogoPath);
//        iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "\\Content\\images\\Agile-logo.png");
//        //Create PdfTable object
//        var pdfTab = new PdfPTable(1);
//        pdfTab.WidthPercentage = 100.00f;
//        //We will have to create separate cells to include image logo
//        //var pdfCell1 = new PdfPCell(imgPdf);
//        var pdfCell1 = new PdfPCell(imgPDF);
//        //set the alignment of all cells and set border to 0
//        pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
//        pdfCell1.Border = 0;
//        //add all three cells into PdfTable
//        pdfTab.AddCell(pdfCell1);
//        pdfTab.TotalWidth = document.PageSize.Width - 20;
//        //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
//        //first param is start row. -1 indicates there is no end row and all the rows to be included to write
//        //Third and fourth param is x and y position to start writing
//        pdfTab.WriteSelectedRows(0, -1, 10, document.PageSize.Height - 15, writer.DirectContent);
//        //set pdfContent value
//        _pdfContent = writer.DirectContent;
//        //Move the pointer and draw line to separate header section from rest of page
//        _pdfContent.MoveTo(30, document.PageSize.Height - 35);
//        _pdfContent.Stroke();
//        //Logger.LogInfoMessage("Method Name:OnStartPage  - Ended ");
//        //}
//        //catch (Exception ex)
//        //{
//        //    Logger.LogErrorMessage("Method Name:OnStartPage  - Error " + ex.Message + ex.StackTrace);
//        //}
//    }

//    public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title) { }
//    public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition) { }
//    public void OnCloseDocument(PdfWriter writer, Document document) { }
//    public void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text) { }
//    public void OnOpenDocument(PdfWriter writer, Document document) { }
//    public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition) { }
//    public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition) { }
//    public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title) { }
//    public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition) { }
//    public void OnEndPage(PdfWriter writer, Document document) { }
//}
