$(document).ready(function () {

    var quoteID = $('#QuoteID').val();
    GetQualitativeInformationTotalService();    

    $("#QualitativeInformationUpdate").unbind("click").click(function () {

        var quoteID = $('#QuoteID').val();

        var leadtime = $('#txtLeadTime').val();

        var winProbability = $('#txtWinProbability').val();

        var scopeofWork = $('#txtareaScopeofWork').val();

        var executiveSummary = $('#txtareaExecutiveSummary').val();

        var primaryCompetitor = $('#txtareaPrimaryCompetitor').val();

        var sellingPrice = $('#txtareaSellingPrice').val();

        var paymentTerms = $('#txtareaPaymentTerms').val();

        var riskMitigation = $('#txtareaRiskMitigation').val();

        var newRepeatBusiness = $('#Business').val();

        var newRepeatBusiness = $('select#Business option:selected').val();


        var anyOtherComments = $('#txtareaAnyOtherComments').val();


        $.ajax({
            url: '/Qualitative/QualitativeInformationsUpdate?QuoteID=' + quoteID + '&Leadtime=' + leadtime + '&WinProbability=' + winProbability + '&ScopeofWork=' + scopeofWork + '&ExecutiveSummary=' + executiveSummary + '&PrimaryCompetitor=' + primaryCompetitor + '&SellingPrice=' + sellingPrice + '&PaymentTerms=' + paymentTerms + '&RiskMitigation=' + riskMitigation + '&NewRepeatBusiness=' + newRepeatBusiness + '&AnyOtherComments=' + anyOtherComments,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data.Redirect) {
                    window.location.replace(redirectAction);                    
                }
                else {
                    AssignApprover(quoteID);
                    $('div.accordionContent').slideUp('normal').prev().removeClass('active-accord');                    
                    $("div.accordionButton").last().addClass('active-accord');
                    $('div.accordionContent').last().slideDown('normal');                    
                    return;
                }
            },
            error: function (e) {
            },
            async: false
        });
        return false;
    });

    $("#QualitativeInformation").unbind("click").click(function () {

        var quoteID = $('#QuoteID').val();

        var leadtime = $('#txtLeadTime').val();

        var winProbability = $('#txtWinProbability').val();

        var scopeofWork = $('#txtareaScopeofWork').val();

        var executiveSummary = $('#txtareaExecutiveSummary').val();

        var primaryCompetitor = $('#txtareaPrimaryCompetitor').val();

        var sellingPrice = $('#txtareaSellingPrice').val();

        var paymentTerms = $('#txtareaPaymentTerms').val();

        var riskMitigation = $('#txtareaRiskMitigation').val();

        var newRepeatBusiness = $('#Business').val();
        var newRepeatBusiness = $('select#Business option:selected').val();


        var anyOtherComments = $('#txtareaAnyOtherComments').val();


        $.ajax({
            url: '/Qualitative/QualitativeInformations?QuoteID=' + quoteID + '&Leadtime=' + leadtime + '&WinProbability=' + winProbability + '&ScopeofWork=' + scopeofWork + '&ExecutiveSummary=' + executiveSummary + '&PrimaryCompetitor=' + primaryCompetitor + '&SellingPrice=' + sellingPrice + '&PaymentTerms=' + paymentTerms + '&RiskMitigation=' + riskMitigation + '&NewRepeatBusiness=' + newRepeatBusiness + '&AnyOtherComments=' + anyOtherComments,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data.Redirect) {
                    window.location.replace(redirectAction);                    
                }
                else {                    
                    return false;
                }
            },
            error: function (e) {
            },
            async: false
        });
        return false;
    });
});

function GetQualitativeInformationTotalService() {
    var quoteID = $('#QuoteID').val();
    $.ajax({
        url: "/Qualitative/QualitativeInformationQuotedValue?QuoteID=" + quoteID,
        type: "POST",
        dataType: "JSON",
        success: function (data) {
            if (data.Redirect) {
                window.location.replace(redirectAction);
            }
            else {
                $('#txtQuoteValue').val(data.Result.txtQuoteTotalValue);
                $('#txtGrossMarginAmount').val(data.Result.txtMarginAmountPercentageValue);               
            }
        },
        error: function (e) {
        },
        async: false
    });
}