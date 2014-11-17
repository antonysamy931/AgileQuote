using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileQuoteProperty.Common
{
    public static class ConstantValues
    {
        public const string salesVP = "Reginal Sales Manager";       

        public const string ConfigureBundle = "ClientConfigured";

        public const string Dollar = "USD";
        public const string Pound = "EUR";
        public const string JapaniseYen = "CNY";
        public const string IndianRupee = "INR";

        public const string DollarCode = "en-US";
        public const string PoundCode = "de-DE";
        public const string JapaniseYenCode = "ja-JP";
        public const string IndianRupeeCode = "hi-IN";

        public const int DefaultQuntity = 1;
        public const int DefaultWarrenty = 1;

        public const string SuccessMessage = "success";

        public const decimal Zero = 0;
        public const decimal One = 1;
        public const decimal TwentyFiveThousand = 25000;
        public const decimal SeventyFiveThousand = 75000;
        public const decimal OneTwentyFileThousand = 125000;
        public const decimal OneFiftyThousand = 150000;
        public const decimal OneSeventyFiveThousand = 175000;
        public const decimal TwoHundredThousand = 200000;
        public const int Two = 2;
        public const int Three = 3;
        public const int Four = 4;
        public const int Five = 5;
        public const int Six = 6;
        public const int Seven = 7;
        public const int Eight = 8;
        public const int Nine = 9;
        public const int TenThousand = 10000;
        public const int NintyNineThousand = 99999;

        public const string QuoteProcessingStatus = "In Progress";
        public const string QuoteApproveStatus = "Approved";
        public const string QuoteRejectStatus = "Rejected";
        public const string QuotePendingApproval = "Pending Approval";
        public const string QuoteWaitingCollaborate = "Waiting Collaboration";
        public const string QuoteCollaborationCompleted = "Collaboration Completed";

        public const int QuoteRequestDateConstant = 10;
        public const int Hundred = 100;
        public const int Ten = 10;

        public const string Billing = "Billing";
        public const string Shipping = "Shipping";
        public const string TypeBundle = "B";
        public const string BundleName = "Bundle";

        public const string ErrorBundleNameExist = "Bundle name already exist.";
        public const string ErrorBundleNameExistWithName = "Bundle Already exist. Bundle name is ";
        public const string ErrorCollaboratorAssign = "Collabrator Not Authorize This Quote";
        public const string ErrorRejectQuoteMessage = "Not able to reject this quote. Because quote waiting for approval";

        public const string Authorize = "Authorize";

        public const string Random = "Random";
        public const string recaptcha_challenge_field = "recaptcha_challenge_field";
        public const string recaptcha_response_field = "recaptcha_response_field";
        public const string ReCaptchaPrivateKey = "ReCaptchaPrivateKey";

        public const float CellMinimumHeight = 30f;

        public const float Float7 = 7f;
        public const float Float10 = 10f;
        public const float Float15 = 15f;
        public const float Float20 = 20f;
        public const float Float25 = 25f;        
        public const float Float30 = 30f;
        public const float Float45 = 45f;
        
        public const int Int0 = 0;
        public const int Width900 = 900;
        public const int Width800 = 800;
        public const int Width810 = 810;
        public const int Width850 = 850;
        public const int Width750 = 750;
        public const int Width650 = 650;

        public const string QuoteDetails = "Quote Details";
        public const string QuoteName = "Quote Name";
        public const string SalesOrganizationName = "Sales Organization Name";
        public const string CustomerCode = "Customer Code";
        public const string CustomerName = "Customer Name";
        public const string RequestedDate = "Requested Date";
        public const string CurrencyName = "Currency Name";
        public const string BudgetValue = "Budget Value";
        public const string PreparedBy = "Prepared By";

        public const string BillingAddress = "Billing Address";
        public const string ShippingAddress = "Shipping Address";
        public const string AddressOne = "Address Line One";
        public const string AddressTwo = "Address Line Two";
        public const string City = "City";
        public const string State = "State";
        public const string Country = "Country";
        public const string ZipCode = "Zip Code";
        public const string PhoneNumber = "Phone Number";
        public const string MobileNumber = "Mobile Number";

        public const string BundleMaterial = "Bundle Material";
        public const string Code = "Code";
        public const string Name = "Name";
        public const string Description = "Description";
        public const string UnitPrice = "Unit Price";
        public const string Quantity = "Quantity";
        public const string TotalGrossPrice = "Total Gross Price";
        public const string Discount = "Discount";
        public const string TotalNetPrice = "Total Net Price";
        public const string Amount = "Amount";
        public const string GrandTotalGrossPrice = "Grand Total Gross Price";
        public const string OverallDiscount = "Overall Discount";
        public const string GrandTotalNetPrice = "Grand Total Net Price";

        public const string BoughtOutItem = "Bought Out Item";
        public const string UnitCost = "Unit Cost";
        public const string TotalCost = "Total Cost";
        public const string TotalPrice = "Total Price";
        public const string GrandTotalCost = "Grand Total Cost";
        public const string GrandTotalPrice = "Grand Total Price";

        public const string MaterialBundleWarrenty = "Material Bundle Warranty";
        public const string Warrenty = "Warranty";
        public const string OverrideWarrenty = "Override Warranty";

        public const string MaterialBundleInstallation = "Material Bundle Installation";
        public const string FTERequired = "No of FTE's required";
        public const string PerDayCost = "Per Day Cost";
        public const string NoDaysRequired = "No of Days Required";
        public const string TotalFTECost = "Total No of FTE's Costs";

        public const string PdfShipping = "Shipping";
        public const string TruckCost = "Truck Cost";
        public const string DieselCost = "Diesel Cost";

        public const string QuotePrice = "Quote Price";
        public const string Item = "Item";        
        public const string MaterialBundlePrice = "Material / Bundle Price";
        public const string BoughtoutItemPrice = "Bought Out Item Price";
        public const string InstallationPrice = "Installation Price";
        public const string ShippingCharges = "Shipping Charges";

        public const string QuoteQualitativeInformation = "Quote Qualitative Information";
        public const string QuoteValue = "Quote Value";
        public const string GrossMarginAmountPercentage = "Gross Margin Amount / Percentage";
        public const string Leadtime = "Leadtime";
        public const string WinProbability = "Win Probability %";
        public const string ScopOfWork = "Scop Of Work";
        public const string ExecutiveSummary = "Executive Summary";
        public const string PrimaryCompetitor = "Primary Competitor";
        public const string HowWasSellingPriceSet = "How was Selling Price Set?";
        public const string PaymentTerms = "Payment Terms";
        public const string RiskMitigation = "Risk & Mitigation";
        public const string NewRepeatBusiness = "New / Repeat Business";
        public const string AnyOtherComments = "Any Other Comments";

        public const string Separater = ":";
    }
}
