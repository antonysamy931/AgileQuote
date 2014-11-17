using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;
using AgileQuoteProperty.Model;
using AgileQuoteProperty.Common;

namespace AgileQuoteWebApplication.Models
{
    public class pdfDataLogic
    {
        AgileQuoteService.IService1 agileQuote = new AgileQuoteService.Service1Client();

        Font HeadFont = iTextSharp.text.FontFactory.GetFont("HelveticaNeueLTStd77CnBold", 15, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#555555")));
        Font ContentFont = iTextSharp.text.FontFactory.GetFont("proximanova", 11, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#5C5C5F")));
        Font TableHeadFont = iTextSharp.text.FontFactory.GetFont("proximanova", 11, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#597FA6")));

        public PdfPTable PdfHeaderQuoteDetails()
        {
            try
            {
                PdfPTable headerSubTableOne = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                headerSubTableOne.AddCell(PdfHeaderCellGenerator(ConstantValues.QuoteDetails));
                return headerSubTableOne;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable PdfHeaderTableCommanProperty(int Column, int Width)
        {
            try
            {
                PdfPTable oPdfPTable = new PdfPTable(Column);

                //oPdfPTable.TotalWidth = 100;
                oPdfPTable.WidthPercentage = 100;
                //oPdfPTable.LockedWidth = true;
                oPdfPTable.DefaultCell.Border = ConstantValues.Int0;
                oPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SpacingAfter = ConstantValues.Float30;
                return oPdfPTable;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable PdfTableCommanProperty(int Column, int Width, int[] Widths)
        {
            try
            {
                PdfPTable oPdfPTable = new PdfPTable(Column);
                //oPdfPTable.TotalWidth = Width;
                //oPdfPTable.LockedWidth = true;
                oPdfPTable.WidthPercentage = 90;
                oPdfPTable.SetWidths(Widths);
                oPdfPTable.DefaultCell.Border = ConstantValues.Int0;
                oPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SpacingAfter = ConstantValues.Float30;
                return oPdfPTable;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell PdfCellGenerator(string Value)
        {
            try
            {
                PdfPCell oPdfCell = new PdfPCell(new Phrase(Value, ContentFont));
                oPdfCell.Border = Convert.ToInt32(ConstantValues.Zero);
                oPdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfCell.MinimumHeight = ConstantValues.CellMinimumHeight;
                return oPdfCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell PdfHeaderCellGenerator(string Value)
        {
            try
            {
                PdfPCell oPdfCell = new PdfPCell(new Phrase(Value, HeadFont));
                oPdfCell.MinimumHeight = ConstantValues.Float30;
                oPdfCell.PaddingLeft = ConstantValues.Float30;
                oPdfCell.PaddingTop = ConstantValues.Float7;
                oPdfCell.Border = ConstantValues.Int0;
                oPdfCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F5F5F5"));
                return oPdfCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteDetailsTable(int QuoteId, int UserId)
        {
            try
            {
                Quote qQuote = new Quote();
                qQuote = agileQuote.GetQuoteDetails(QuoteId, UserId);

                int[] widths = new int[] { 180, 40, 180, 180, 40, 180 };
                PdfPTable QuoteDetailTable = PdfTableCommanProperty(6, ConstantValues.Width800, widths);

                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.QuoteName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(qQuote.QuoteName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.SalesOrganizationName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(qQuote.qQuoteSalesOrgName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.CustomerCode));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(qQuote.CustomerCode));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.CustomerName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(qQuote.CustomerName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.RequestedDate));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(qQuote.RequrestedDate));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.CurrencyName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(qQuote.qQuoteCurrencyName));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.BudgetValue));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(Math.Truncate(qQuote.BudgetValue).ToString()));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.PreparedBy));
                QuoteDetailTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteDetailTable.AddCell(PdfCellGenerator(qQuote.PreparedBy));

                return QuoteDetailTable;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteAddressHeaderTable()
        {
            try
            {
                PdfPTable AddressTable = PdfHeaderTableCommanProperty(2, ConstantValues.Width900);
                AddressTable.AddCell(PdfHeaderCellGenerator(ConstantValues.BillingAddress));
                AddressTable.AddCell(PdfHeaderCellGenerator(ConstantValues.ShippingAddress));
                return AddressTable;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteAddressTable(int QuoteId, int UserId)
        {
            try
            {
                Quote qQuote = new Quote();
                qQuote = agileQuote.GetQuoteDetails(QuoteId, UserId);

                int[] addressWidths = new int[] { 180, 40, 180, 180, 40, 180 };
                PdfPTable QuoteAddressTable = PdfTableCommanProperty(6, ConstantValues.Width800, addressWidths);

                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.AddressOne));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.BillingAddressOne));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.AddressOne));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.ShippingAddressOne));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.AddressTwo));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.BillingAddressTwo));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.AddressTwo));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.ShippingAddressTwo));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.City));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.BillingCity));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.City));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.ShippingCity));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.State));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.BillingState));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.State));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.ShippingState));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Country));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.BillingCountry));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Country));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.ShippingCountry));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.ZipCode));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.BillingPostalCode));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.ZipCode));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.ShippingPostalCode));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.PhoneNumber));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.BillingPhoneNumber));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.PhoneNumber));
                QuoteAddressTable.AddCell(PdfCellGenerator(ConstantValues.Separater));
                QuoteAddressTable.AddCell(PdfCellGenerator(qQuote.ShippingPhoneNumber));

                return QuoteAddressTable;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteBundleMaterialHeader()
        {
            try
            {
                PdfPTable bundleMaterialheader = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                bundleMaterialheader.AddCell(PdfHeaderCellGenerator(ConstantValues.BundleMaterial));
                return bundleMaterialheader;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable TableCommonStyle(int Column, int Width)
        {
            try
            {
                PdfPTable oPdfPTable = new PdfPTable(Column);
                //oPdfPTable.TotalWidth = Width;
                //oPdfPTable.LockedWidth = true;
                oPdfPTable.WidthPercentage = 90;
                oPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SpacingAfter = ConstantValues.Float10;
                return oPdfPTable;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell HeaderTableCommonStyle(string Value)
        {
            try
            {
                PdfPCell oPdfPCell = new PdfPCell(new Phrase(Value, TableHeadFont));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.PaddingTop = ConstantValues.Float7;
                oPdfPCell.MinimumHeight = ConstantValues.Float25;
                oPdfPCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#8CB2D9"));
                oPdfPCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C6D9EC"));
                return oPdfPCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell HeaderTableCommonStyleWithoutBorder(string Value)
        {
            try
            {
                PdfPCell oPdfPCell = new PdfPCell(new Phrase(Value, TableHeadFont));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.PaddingTop = ConstantValues.Float7;
                oPdfPCell.MinimumHeight = ConstantValues.Float25;
                oPdfPCell.Border = ConstantValues.Int0;
                oPdfPCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C6D9EC"));
                return oPdfPCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell HeaderTableCommonStyleWithoutBorderQuotePrice(string Value)
        {
            try
            {
                PdfPCell oPdfPCell = new PdfPCell(new Phrase(Value, TableHeadFont));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.PaddingTop = ConstantValues.Float7;
                oPdfPCell.PaddingLeft = ConstantValues.Float10;
                oPdfPCell.MinimumHeight = ConstantValues.Float25;
                oPdfPCell.Border = ConstantValues.Int0;
                oPdfPCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C6D9EC"));
                return oPdfPCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell TableContentCommonStyle(string Value)
        {
            try
            {
                PdfPCell oPdfPCell = new PdfPCell(new Phrase(Value, ContentFont));
                oPdfPCell.MinimumHeight = ConstantValues.Float25;
                oPdfPCell.PaddingTop = ConstantValues.Float7;
                oPdfPCell.PaddingLeft = ConstantValues.Float10;
                oPdfPCell.PaddingBottom = ConstantValues.Float7;
                oPdfPCell.PaddingRight = ConstantValues.Float10;
                oPdfPCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#8CB2D9"));                
                return oPdfPCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell TableContentCommonStyleWithoutBorder(string Value)
        {
            try
            {
                PdfPCell oPdfPCell = new PdfPCell(new Phrase(Value, ContentFont));
                oPdfPCell.MinimumHeight = ConstantValues.Float25;
                oPdfPCell.PaddingTop = ConstantValues.Float7;
                oPdfPCell.PaddingLeft = ConstantValues.Float10;
                oPdfPCell.PaddingBottom = ConstantValues.Float7;
                oPdfPCell.PaddingRight = ConstantValues.Float10;
                oPdfPCell.Border = ConstantValues.Int0;
                return oPdfPCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPCell TableTotalSeparatorCommonStyleWithoutBorder(string Value)
        {
            try
            {
                PdfPCell oPdfPCell = new PdfPCell(new Phrase(Value, TableHeadFont));
                oPdfPCell.MinimumHeight = ConstantValues.Float20;
                oPdfPCell.PaddingTop = ConstantValues.Float7;
                oPdfPCell.Border = ConstantValues.Int0;
                return oPdfPCell;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteBundleMaterialTable(int QuoteId)
        {
            try
            {
                PdfPTable oBundleMaterial = TableCommonStyle(8, ConstantValues.Width850);
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Code));
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Name));
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Description));
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.UnitPrice));
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Quantity));
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.TotalGrossPrice));
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Discount));
                oBundleMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.TotalNetPrice));
                QuoteBundleMaterialList qQutoeBundleMaterialList = new QuoteBundleMaterialList();
                qQutoeBundleMaterialList = agileQuote.GetQuoteBaseMaterialBundleList(QuoteId);

                if (qQutoeBundleMaterialList.ListQuoteBundleMaterial.Count > 0)
                {
                    foreach (var item in qQutoeBundleMaterialList.ListQuoteBundleMaterial)
                    {
                        oBundleMaterial.AddCell(TableContentCommonStyle(item.Code.ToString()));
                        oBundleMaterial.AddCell(TableContentCommonStyle(item.Name));
                        oBundleMaterial.AddCell(TableContentCommonStyle(item.Description));
                        oBundleMaterial.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.UnitPrice, QuoteId)));
                        oBundleMaterial.AddCell(TableContentCommonStyle(item.Quantity.ToString()));
                        oBundleMaterial.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.GrossPrice, QuoteId)));
                        oBundleMaterial.AddCell(TableContentCommonStyle(item.Discount.ToString()));
                        oBundleMaterial.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.NetPrice, QuoteId)));
                        if (item.Type == "B")
                        {
                            PdfPTable oMaterial = TableCommonStyle(4, ConstantValues.Width650);
                            PdfPCell oSubCell = new PdfPCell(new Phrase("Mapped Material", TableHeadFont));
                            oSubCell.Colspan = 4;
                            oSubCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            oSubCell.PaddingTop = ConstantValues.Float7;
                            oSubCell.MinimumHeight = ConstantValues.Float25;
                            oSubCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#8CB2D9"));
                            oSubCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C6D9EC"));

                            oMaterial.AddCell(oSubCell);

                            oMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Code));
                            oMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Name));
                            oMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Quantity));
                            oMaterial.AddCell(HeaderTableCommonStyle(ConstantValues.Amount));

                            foreach (var material in item.ListOfMaterialMappedBundle)
                            {
                                oMaterial.AddCell(TableContentCommonStyle(material.MaterialCode));
                                oMaterial.AddCell(TableContentCommonStyle(material.MaterialName));
                                oMaterial.AddCell(TableContentCommonStyle(material.Quantity.ToString()));
                                oMaterial.AddCell(TableContentCommonStyle(FormatingCurrencyValue(material.MaterialAmount, QuoteId)));
                            }

                            PdfPCell imgCell = new PdfPCell(new Phrase(""));
                            imgCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            imgCell.Border = ConstantValues.Int0;

                            PdfPCell tableCell = new PdfPCell(oMaterial);
                            tableCell.Colspan = 7;
                            tableCell.Border = ConstantValues.Int0;
                            tableCell.PaddingBottom = ConstantValues.Float10;
                            tableCell.PaddingLeft = ConstantValues.Float10;
                            tableCell.PaddingRight = ConstantValues.Float10;

                            oBundleMaterial.AddCell(imgCell);
                            oBundleMaterial.AddCell(tableCell);
                        }
                    }
                }

                return oBundleMaterial;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteBundleMaterialTotalTable(int QuoteId)
        {
            try
            {
                int[] BundleMaterialTotalWidths = new int[] { 220, 20, 120, 150, 20, 120, 190, 20, 120 };
                PdfPTable oQuoteBundleTotal = PdfTableCommanProperty(9, ConstantValues.Width810, BundleMaterialTotalWidths);
                oQuoteBundleTotal.HorizontalAlignment = Element.ALIGN_RIGHT;

                TotalUnitDiscount oTotalUnitDiscount = agileQuote.GetQuoteMaterialBundleTotal(QuoteId);
                if (oTotalUnitDiscount != null)
                {

                    oQuoteBundleTotal.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.GrandTotalGrossPrice));
                    oQuoteBundleTotal.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQuoteBundleTotal.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(oTotalUnitDiscount.TotalGrossPrice, QuoteId)));

                    oQuoteBundleTotal.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.OverallDiscount));
                    oQuoteBundleTotal.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQuoteBundleTotal.AddCell(TableContentCommonStyleWithoutBorder(oTotalUnitDiscount.TotalDiscount.ToString() + "%"));

                    oQuoteBundleTotal.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.GrandTotalNetPrice));
                    oQuoteBundleTotal.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQuoteBundleTotal.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(oTotalUnitDiscount.TotalNetPrice, QuoteId)));
                }
                return oQuoteBundleTotal;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteBoughtOutItemHeaderTable()
        {
            try
            {
                PdfPTable boughtOutItemHeader = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                boughtOutItemHeader.AddCell(PdfHeaderCellGenerator(ConstantValues.BoughtOutItem));
                return boughtOutItemHeader;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteBoughtOutItemTable(int QuoteId)
        {
            try
            {
                PdfPTable oBoughtoutitem = TableCommonStyle(7, ConstantValues.Width850);
                oBoughtoutitem.AddCell(HeaderTableCommonStyle(ConstantValues.Code));
                oBoughtoutitem.AddCell(HeaderTableCommonStyle(ConstantValues.Name));
                oBoughtoutitem.AddCell(HeaderTableCommonStyle(ConstantValues.UnitCost));
                oBoughtoutitem.AddCell(HeaderTableCommonStyle(ConstantValues.Quantity));
                oBoughtoutitem.AddCell(HeaderTableCommonStyle(ConstantValues.TotalCost));
                oBoughtoutitem.AddCell(HeaderTableCommonStyle(ConstantValues.UnitPrice));
                oBoughtoutitem.AddCell(HeaderTableCommonStyle(ConstantValues.TotalPrice));

                List<QuoteBoughtOutItemModel> qBoughtOutItemModel = agileQuote.GetQuoteBasedBoughtoutItemList(QuoteId);

                if (qBoughtOutItemModel.Count > 0)
                {
                    foreach (var item in qBoughtOutItemModel)
                    {
                        oBoughtoutitem.AddCell(TableContentCommonStyle(item.ItemId.ToString()));
                        oBoughtoutitem.AddCell(TableContentCommonStyle(item.ItemName));
                        oBoughtoutitem.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.UnitPrice, QuoteId)));
                        oBoughtoutitem.AddCell(TableContentCommonStyle(item.Quantity.ToString()));
                        oBoughtoutitem.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.NetPrice, QuoteId)));
                        oBoughtoutitem.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.quotedUnitPrice, QuoteId)));
                        oBoughtoutitem.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.quotedNetPrice, QuoteId)));
                    }
                }
                return oBoughtoutitem;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteBoughtOutItemTotalTable(int QuoteId)
        {
            try
            {
                int[] BoughtOutItemWidths = new int[] { 290, 20, 90, 290, 20, 90 };
                PdfPTable boughtoutitemTotal = PdfTableCommanProperty(6, ConstantValues.Width650, BoughtOutItemWidths);
                boughtoutitemTotal.HorizontalAlignment = Element.ALIGN_RIGHT;

                TotalUnitDiscount oTotalUnitDiscountBoughtOutItem = agileQuote.GetTotalCostBoughtoutItem(QuoteId);
                if (oTotalUnitDiscountBoughtOutItem != null)
                {
                    boughtoutitemTotal.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.GrandTotalCost));
                    boughtoutitemTotal.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    boughtoutitemTotal.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(oTotalUnitDiscountBoughtOutItem.TotalNetPrice, QuoteId)));

                    boughtoutitemTotal.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.GrandTotalPrice));
                    boughtoutitemTotal.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    boughtoutitemTotal.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(oTotalUnitDiscountBoughtOutItem.TotalQuotedNetPrice, QuoteId)));
                }
                return boughtoutitemTotal;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteMaterialBundleWarrentyHeader()
        {
            try
            {
                PdfPTable oWarrenty = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                oWarrenty.AddCell(PdfHeaderCellGenerator(ConstantValues.MaterialBundleWarrenty));
                return oWarrenty;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteMaterialBundleWarrenty(int QuoteId)
        {
            try
            {
                PdfPTable oWarrenty = TableCommonStyle(5, ConstantValues.Width850);
                oWarrenty.AddCell(HeaderTableCommonStyle(ConstantValues.Code));
                oWarrenty.AddCell(HeaderTableCommonStyle(ConstantValues.Name));
                oWarrenty.AddCell(HeaderTableCommonStyle(ConstantValues.Description));
                oWarrenty.AddCell(HeaderTableCommonStyle(ConstantValues.Warrenty));
                oWarrenty.AddCell(HeaderTableCommonStyle(ConstantValues.OverrideWarrenty));

                List<QuoteBundleMaterial> oQuoteBundleMaterialWarrenty = agileQuote.GetQuoteMaterialBundleListwithwarranty(QuoteId);

                if (oQuoteBundleMaterialWarrenty.Count > 0)
                {

                    foreach (var item in oQuoteBundleMaterialWarrenty)
                    {
                        oWarrenty.AddCell(TableContentCommonStyle(item.Code.ToString()));
                        oWarrenty.AddCell(TableContentCommonStyle(item.Name));
                        oWarrenty.AddCell(TableContentCommonStyle(item.Description));
                        oWarrenty.AddCell(TableContentCommonStyle(item.Warrenty.ToString()));
                        oWarrenty.AddCell(TableContentCommonStyle(item.OverRideWarrenty == 0 ? "-" : item.OverRideWarrenty.ToString()));
                    }
                }
                return oWarrenty;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteMaterialBundleInstallationHeader()
        {
            try
            {
                PdfPTable oInstallation = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                oInstallation.AddCell(PdfHeaderCellGenerator(ConstantValues.MaterialBundleInstallation));
                return oInstallation;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteMaterialBundleInstallation(int QuoteId)
        {
            try
            {
                PdfPTable oInstall = TableCommonStyle(4, ConstantValues.Width850);
                oInstall.AddCell(HeaderTableCommonStyle(ConstantValues.FTERequired));
                oInstall.AddCell(HeaderTableCommonStyle(ConstantValues.PerDayCost));
                oInstall.AddCell(HeaderTableCommonStyle(ConstantValues.NoDaysRequired));
                oInstall.AddCell(HeaderTableCommonStyle(ConstantValues.TotalFTECost));

                InstallCharges oQuoteShipping = agileQuote.GetQuoteShippingDetails(QuoteId);

                if (oQuoteShipping != null)
                {
                    oInstall.AddCell(TableContentCommonStyle(oQuoteShipping.NoEmployees.ToString()));
                    oInstall.AddCell(TableContentCommonStyle(FormatingCurrencyValue(oQuoteShipping.CostPerDay, QuoteId)));
                    oInstall.AddCell(TableContentCommonStyle(oQuoteShipping.NoDays.ToString()));
                    oInstall.AddCell(TableContentCommonStyle(FormatingCurrencyValue(oQuoteShipping.TotalCost, QuoteId)));
                }

                return oInstall;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteShippingTableHeader()
        {
            try
            {
                PdfPTable oShipping = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                oShipping.AddCell(PdfHeaderCellGenerator(ConstantValues.PdfShipping));
                return oShipping;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteShippingTable(int QuoteId)
        {
            try
            {
                PdfPTable oShipping = TableCommonStyle(3, ConstantValues.Width850);
                oShipping.AddCell(HeaderTableCommonStyle(ConstantValues.Code));
                oShipping.AddCell(HeaderTableCommonStyle(ConstantValues.TruckCost));
                oShipping.AddCell(HeaderTableCommonStyle(ConstantValues.DieselCost));

                List<QuoteShipping> oQuoteShippingCharges = agileQuote.GetQuoteShippingList(QuoteId);

                if (oQuoteShippingCharges.Count > 0)
                {
                    foreach (var item in oQuoteShippingCharges)
                    {
                        oShipping.AddCell(TableContentCommonStyle(item.Sno.ToString()));
                        oShipping.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.truckCost, QuoteId)));
                        oShipping.AddCell(TableContentCommonStyle(FormatingCurrencyValue(item.dieselCost, QuoteId)));
                    }
                }
                return oShipping;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuotePriceTabelHeader()
        {
            try
            {
                PdfPTable oQuotePrice = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                oQuotePrice.AddCell(PdfHeaderCellGenerator(ConstantValues.QuotePrice));
                return oQuotePrice;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuotePriceTable(int QuoteId)
        {
            try
            {
                int[] quotePriceWidths = new int[] { 600, 50, 200 };
                decimal TotalAmount = ConstantValues.Zero;

                PdfPTable oQuotePrice = TableCommonStyle(3, ConstantValues.Width850);
                oQuotePrice.SetWidths(quotePriceWidths);
                //oQuotePrice.LockedWidth = true;

                oQuotePrice.AddCell(HeaderTableCommonStyleWithoutBorderQuotePrice(ConstantValues.Item));
                oQuotePrice.AddCell(HeaderTableCommonStyleWithoutBorderQuotePrice(string.Empty));
                oQuotePrice.AddCell(HeaderTableCommonStyleWithoutBorderQuotePrice(ConstantValues.Amount));

                TotalUnitDiscount oTotalUnitDiscount = agileQuote.GetQuoteMaterialBundleTotal(QuoteId);
                if (oTotalUnitDiscount != null)
                {
                    oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.MaterialBundlePrice));
                    oQuotePrice.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(oTotalUnitDiscount.TotalNetPrice, QuoteId)));
                    TotalAmount = TotalAmount + Extension.StringToDecimal(oTotalUnitDiscount.TotalNetPrice);
                }

                TotalUnitDiscount oTotalUnitDiscountBoughtOutItem = agileQuote.GetTotalCostBoughtoutItem(QuoteId);
                if (oTotalUnitDiscountBoughtOutItem != null)
                {
                    oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.BoughtoutItemPrice));
                    oQuotePrice.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(oTotalUnitDiscountBoughtOutItem.TotalQuotedNetPrice, QuoteId)));
                    TotalAmount = TotalAmount + Extension.StringToDecimal(oTotalUnitDiscountBoughtOutItem.TotalQuotedNetPrice);
                }

                InstallCharges oQuoteShipping = agileQuote.GetQuoteShippingDetails(QuoteId);

                if (oQuoteShipping != null)
                {
                    oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.InstallationPrice));
                    oQuotePrice.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(oQuoteShipping.TotalCost, QuoteId)));
                    TotalAmount = TotalAmount + Extension.StringToDecimal(oQuoteShipping.TotalCost);
                }

                List<QuoteShipping> oQuoteShippingCharges = agileQuote.GetQuoteShippingList(QuoteId);

                if (oQuoteShippingCharges.Count > 0)
                {
                    foreach (var item in oQuoteShippingCharges)
                    {
                        oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.ShippingCharges));
                        oQuotePrice.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                        oQuotePrice.AddCell(TableContentCommonStyleWithoutBorder(FormatingCurrencyValue(item.truckCost + item.dieselCost, QuoteId)));
                        TotalAmount = TotalAmount + item.truckCost + item.dieselCost;
                    }
                }

                oQuotePrice.AddCell(HeaderTableCommonStyleWithoutBorderQuotePrice(ConstantValues.QuotePrice));
                PdfPCell separateCell = HeaderTableCommonStyleWithoutBorderQuotePrice(ConstantValues.Separater);
                separateCell.PaddingLeft = 0f;
                oQuotePrice.AddCell(separateCell);
                oQuotePrice.AddCell(HeaderTableCommonStyleWithoutBorderQuotePrice(FormatingCurrencyValue(TotalAmount, QuoteId)));

                return oQuotePrice;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteQualitativeInformationTableHeader()
        {
            try
            {
                PdfPTable oQualitative = PdfHeaderTableCommanProperty(1, ConstantValues.Width900);
                oQualitative.AddCell(PdfHeaderCellGenerator(ConstantValues.QuoteQualitativeInformation));
                return oQualitative;
            }
            catch
            {
                throw;
            }
        }

        public PdfPTable QuoteQualitativeInformationTable(int QuoteId)
        {
            try
            {
                int[] Widths = new int[] { 300, 50, 500 };
                PdfPTable oQualitative = TableCommonStyle(3, ConstantValues.Width850);
                oQualitative.SetWidths(Widths);
                //oQualitative.LockedWidth = true;

                var qQuoteQualitativeInformation = agileQuote.GetQualitativeInfomation(QuoteId);
                if (qQuoteQualitativeInformation != null)
                {
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.QuoteValue));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtQuoteValue));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.GrossMarginAmountPercentage));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtMagrinAmountPercentage));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.Leadtime));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtLeadTime));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.WinProbability));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtWinProbability));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.ScopOfWork));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtareaScopeofWork));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.ExecutiveSummary));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtareaExecutiveSummary));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.PrimaryCompetitor));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtareaPrimaryCompetitor));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.HowWasSellingPriceSet));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtareaSellingPrice));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.PaymentTerms));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtareaPaymentTerms));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.RiskMitigation));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtareaRiskMitigation));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.NewRepeatBusiness));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.Business));

                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(ConstantValues.AnyOtherComments));
                    oQualitative.AddCell(TableTotalSeparatorCommonStyleWithoutBorder(ConstantValues.Separater));
                    oQualitative.AddCell(TableContentCommonStyleWithoutBorder(qQuoteQualitativeInformation.txtareaAnyOtherComments));
                }
                return oQualitative;
            }
            catch
            {
                throw;
            }
        }

        public string GetQuoteNameForPdf(int QuoteId, int UserId)
        {
            try
            {
                Quote qQuote = new Quote();
                qQuote = agileQuote.GetQuoteDetails(QuoteId, UserId);
                return qQuote.QuoteName.Replace(" ", "-");
            }
            catch
            {
                throw;
            }
        }

        public string FormatingCurrencyValue(object Value, int QuoteId)
        {
            try
            {
                string symbol = agileQuote.GetSymbol(QuoteId, null);
                System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                nfi.CurrencySymbol = symbol;
                nfi.CurrencyDecimalDigits = Convert.ToInt32(ConstantValues.Zero);
                return Extension.FormatCurrency(nfi, Value);
            }
            catch
            {
                throw;
            }
        }

    }
}