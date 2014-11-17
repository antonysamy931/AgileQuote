var firstOject = null;
var secondOject = null;
$(document).ready(function () {

    //var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    
    //if (hashes[2] != "") {
    //    if (hashes[2] == "Create") {            
    //        $('#QualitativeInformationUpdate').hide();
    //    }
    //    else {
    //        $('#QualitativeInformation').show();
    //        $('#QualitativeInformationUpdate').hide();
    //    }        
    //}


    //if (hashes[2] != "") {
    //    if (hashes[2] == "Edit") {            
    //        $('#QualitativeInformation').hide();
    //    }
    //    else {
    //        $('#QualitativeInformationUpdate').show();
    //    }        
    //}

    var QuoteError = 0;
    var BillingError = 0;
    var ShippingError = 0;


    //var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    //for (var i = 0; i < url.length; i++) {
    //    var param = url[i].split('=');

    //    if (param[0] === "Create") {
    //        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
    //        $('#createQuote').css('display', 'none');
    //        $('#createClear').css('display', 'none');
    //        $('#approveQuote').css('display', 'none');
    //        $('#rejectQuote').css('display', 'none');
    //        $('.submit_box').css('width', '29%');
    //    }
    //    else if (param[0] === "Authorize") {
    //        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
    //        $('#createQuote').css('display', 'none');
    //        $('#createClear').css('display', 'none');
    //        $('#approveQuote').css('display', 'block');
    //        $('#rejectQuote').css('display', 'block');
    //        $('.submit_box').css('width', '29%');
    //    }
    //    else if (param[0] === "Collaborate") {
    //        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
    //        $('#createQuote').css('display', 'none');
    //        $('#createClear').css('display', 'none');
    //        $('#approveQuote').css('display', 'none');
    //        $('#rejectQuote').css('display', 'none');
    //        $('.submit_box').css('width', '29%');
    //    }
    //    else {
    //        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').removeAttr('disabled');
    //        $('#createQuote').css('display', 'block');
    //        $('#createClear').css('display', 'block');
    //        $('#approveQuote').css('display', 'none');
    //        $('#rejectQuote').css('display', 'none');
    //        $('.submit_box').css('width', '29%');
    //    }

    //    //if (param[0] == 'QuoteID') {
    //    //    $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
    //    //    //$('#createQuote').css('display', 'none');
    //    //    //$('#createClear').css('display', 'none');
    //    //    //$('#approveQuote').css('display', 'block');
    //    //    //$('#rejectQuote').css('display', 'block');
    //    //    //$('.submit_box').css('width', '29%');
    //    //    return false;
    //    //}
    //    //else {
    //    //    $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').removeAttr('disabled');
    //    //    //$('#approveQuote').css('display', 'none');
    //    //    //$('#rejectQuote').css('display', 'none');
    //    //    $('.submit_box').css('width', '20%');
    //    //    return false;
    //    //}
    //}

    //$('#QualitativeInformation').hide();
    $('#QualitativeInformationUpdate').show();

    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    if (hashes[2] != '') {
        if (hashes[2] === 'saveas') {
            $('#EditQuoteDetailsList').css('display', 'block');
        }
        //else {
        //    $('#SaveAsNewQuote').css('display', 'block');
        //    $('#AddBundleMaterial').css('display', 'none');
        //    $('#AddItem').css('display', 'none');
        //    $('#QualitativeInformation').css('display', 'none');
        //    $('#Summary').css('display', 'none');
        //    $('#AssignApprover').css('display', 'none');
        //}
    }

    $('#EditQuoteDetails').click(function () {
        if ($(this).prop('checked') === true) {
            $('#UpdateQuoteDetails').css('display', 'block');
            $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').removeAttr('disabled');
            //$('#CurrencyNameField').attr('disabled', 'disabled');
            $('#PreparedBy').attr('readonly', true);
            $('#Status').attr('disabled', 'disabled');
        }
        else {
            $('#UpdateQuoteDetails').css('display', 'none');
            $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
        }
    });

    $('#UpdateQuoteDetails').click(function () {
        $("#CreateQuoteFormID").submit();
    });

    if ($('#Status').val() === 'Approved') {
        $('#SaveAsNewQuote').css('display', 'block');
    }
    else {
        $('#SaveAsNewQuote').css('display', 'none');
    }

    var BillCountry = $('#BillingCountryHidden').val();
    var BillState = $('#BillingStateHidden').val();
    if (BillCountry !== '') {
        $('#BillCountry').val(BillCountry);
        var selectedIndex = document.getElementById('BillCountry').selectedIndex;
        print_state('BillState', selectedIndex);
        $('#BillState').val(BillState);
    }

    var ShipCountry = $('#ShippingCountryHidden').val();
    var ShipState = $('#ShippingStateHidden').val();
    if (ShipCountry !== '') {
        $('#ShipCountry').val(ShipCountry);
        var selectedIndex = document.getElementById('ShipCountry').selectedIndex;
        print_state('ShipState', selectedIndex);
        $('#ShipState').val(ShipState);
    }

    if ($('#BudgetValue').val() == 0) {
        $('#BudgetValue').val('');
    }
    else {
        var string = $('#BudgetValue').val();
        var string_split = string.split('.');
        if (typeof string_split[1] === 'undefined') {
            $('#BudgetValue').val(string);
        }
        else {
            if (string_split[1].length > 2) {
                var new_string = string.slice(0, -2);
                $('#BudgetValue').val(new_string);
            }
        }
    }

    var EditStatus = $('#QuoteEditAutho').val();
    if (EditStatus !== "True") {
        $('.quout_accordion input[type="button"]').css('display', 'none');
    }

    if ($('#quoteType').val() === "Create") {
        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
        $('#createQuote').css('display', 'none');
        $('#createClear').css('display', 'none');
        $('#approveQuote').css('display', 'none');
        $('#rejectQuote').css('display', 'none');
        $('#ColabratorUpdate').css('display', 'none');
        $('#ColabratorCancel').css('display', 'none');
        $('#QuoteTitle').text('Update Quote');
        $('#SummarryPage').css('display', 'none');
        $('.submit_box').css('width', '20%');
        $('#SaveAsNewQuote').css('display', 'none');
    }

    else if ($('#quoteType').val() === "AllQuote") {
        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
        $('#createQuote').css('display', 'none');
        $('#createClear').css('display', 'none');
        $('#approveQuote').css('display', 'none');
        $('#rejectQuote').css('display', 'none');
        $('#ColabratorUpdate').css('display', 'none');
        $('#ColabratorCancel').css('display', 'none');
        $('#QuoteTitle').text('Modify Quote');
        $('#SummarryPage').css('display', 'none');
        $('.submit_box').css('width', '20%');
        $('#AddBundleMaterial').css('display', 'none');
        $('#AddItem').css('display', 'none');
        $('#QualitativeInformation').css('display', 'none');
        $('#Summary').css('display', 'none');
        $('#AssignApprover').css('display', 'none');
        $('#SaveAsNewQuote').css('display', 'block');
    }
    else if ($('#quoteType').val() === "SaveAs") {
        $('#EditQuoteDetailsList').css('display', 'block');
        $('.accord_btns input[type="button"]').css('display', 'block');
        $('.popup-data .submit_box input[type="button"]').css('display', 'block');
        $('.popup-data .submit_boxQI input[type="button"]').css('display', 'block');
        $('#QualitativeInformation').css('display', 'block');
        $('.submit_box').css('width', '20%');

        $('#EditQuoteDetails').prop('checked', true);
        $('#UpdateQuoteDetails').css('display', 'block');
        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').removeAttr('disabled');
        //$('#CurrencyNameField').attr('disabled', 'disabled');
        $('#PreparedBy').attr('readonly', true);
        $('#Status').attr('disabled', 'disabled');

        $('#createQuote').css('display', 'none');
        $('#createClear').css('display', 'none');
        $('#approveQuote').css('display', 'none');
        $('#rejectQuote').css('display', 'none');
        $('#ColabratorUpdate').css('display', 'none');
        $('#ColabratorCancel').css('display', 'none');
        $('#QuoteTitle').text('Modify Quote');
        $('#SummarryPage').css('display', 'none');

    }
    else if ($('#quoteType').val() === "Authorize") {
        $.ajax({
            url: '/QuoteList/QuoteAuthorizeStatus?QuoteID=' + $('#QuoteID').val() + '&Type=Authorize',
            type: 'post',
            dataType: 'json',
            success: function (result) {
                if (result.redirect) {
                    window.location.replace(result.redirectAction);
                }
                else {
                    if (result.Result) {
                        $('#approveQuote').css('display', 'none');
                        $('#rejectQuote').css('display', 'none');
                        $('.submit_box').css('width', '15%');
                    }
                    else {
                        $('#approveQuote').css('display', 'block');
                        $('#rejectQuote').css('display', 'block');
                        $('.submit_box').css('width', '39%');
                    }
                }
            },
            error: function (e) {
            },
            async: false
        });
        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
        $('#createQuote').css('display', 'none');
        $('#createClear').css('display', 'none');
        $('#ColabratorUpdate').css('display', 'none');
        $('#SummarryPage').css('display', 'none');
        $('#ColabratorCancel').css('display', 'none');
        $('#QuoteTitle').text('Authorize Quote');
    }
    else if ($('#quoteType').val() === "Collaborate") {
        $.ajax({
            url: '/QuoteList/QuoteAuthorizeStatus?QuoteID=' + $('#QuoteID').val() + '&Type=Collaborate',
            type: 'post',
            dataType: 'json',
            success: function (result) {
                if (result.redirect) {
                    window.location.replace(result.redirectAction);
                }
                else {
                    if (result.Result) {
                        $('#ColabratorUpdate').css('display', 'none');
                        $('#ColabratorCancel').css('display', 'none');
                        $('.submit_box').css('width', '15%');
                    }
                    else {
                        $('#ColabratorUpdate').css('display', 'block');
                        $('#ColabratorCancel').css('display', 'block');
                        $('.submit_box').css('width', '30%');
                    }
                }
            },
            error: function (e) {
            },
            async: false
        });
        $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
        $('#createQuote').css('display', 'none');
        $('#createClear').css('display', 'none');
        $('#approveQuote').css('display', 'none');
        $('#rejectQuote').css('display', 'none');
        $('#SummarryPage').css('display', 'none');
        $('#QuoteTitle').text('Update Quote');
    }
    else {
        var quoteid = $('#QuoteID').val();        
        if (quoteid === '0' || typeof quoteid === 'undefined') {
            $('#QuoteAddressTab').find('input[type=text],input:checkbox').removeAttr('disabled');
            $('#createQuote').css('display', 'block');
            $('#createClear').css('display', 'block');
            $('#approveQuote').css('display', 'none');
            $('#rejectQuote').css('display', 'none');
            $('#ColabratorUpdate').css('display', 'none');
            $('#ColabratorCancel').css('display', 'none');
            $('#QuoteTitle').text('Create New Quote');
            $('#SummarryPage').css('display', 'none');
            $('.submit_box').css('width', '15%');
            //$('#Status').attr('disabled', 'disabled');
        }
        else {            
            $('#QuoteAddressTab').find('input[type=text],input:checkbox,select').attr('disabled', 'disabled');
            $('#createQuote').css('display', 'none');
            $('#createClear').css('display', 'none');
            $('#approveQuote').css('display', 'none');
            $('#rejectQuote').css('display', 'none');
            $('#ColabratorUpdate').css('display', 'none');
            $('#ColabratorCancel').css('display', 'none');
            $('#QuoteTitle').text('Update Quote');
            $('.submit_box').css('width', '20%');
            $('.accord_btns input[type="button"]').css('display', 'block');
            $('.popup-data .submit_box input[type="button"]').css('display', 'block');
            $('.popup-data .submit_boxQI input[type="button"]').css('display', 'block');
            $('#QualitativeInformation').css('display', 'block');
            $('#SaveAsNewQuote').css('display', 'none');
            $('#SummarryPage').css('display', 'none');
        }
    }


    $('#SaveAsNewQuote').click(function () {
        if (confirm("This will create new Quote with same data of selected Quote. Wish to continue?")) {
            $.ajax({
                url: '/QuoteList/SaveAsNewQuote?QuoteId=' + $('#QuoteID').val(),
                type: 'post',
                dataType: 'json',
                success: function (result) {

                    if (result.redirect) {                        
                        window.location.replace(result.redirectAction);
                    }
                    else {
                        $.noty.closeAll();
                        notyAlert = noty({
                            text: "The new Quote has been created with Quote ID: " + result.QuoteID,
                            type: 'success',
                            buttons: [{
                                addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                                    $noty.close();
                                }
                            }]
                        });                        
                        window.location.replace(result.redirectAction + '&saveas');
                    }
                },
                error: function (e) {
                },
                async: false
            });
        }
        return false;
    });


    $('#approveQuote').click(function () {
        notyAlert = noty({
            text: "Are you sure to approve this quote?",
            type: 'success',
            buttons: [{
                addClass: 'btn btn-primary', text: 'YES', onClick: function ($noty) {
                    $noty.close();
                    $.ajax({
                        url: '/QuoteList/ApproveQuote?QuoteID=' + $('#QuoteID').val(),
                        type: 'post',
                        dataType: 'json',
                        success: function (result) {
                            if (result.redirect) {
                                window.location.replace(result.redirectAction);
                            }
                        },
                        error: function (e) {
                        },
                        async: false
                    });
                }
            },
            {
                addClass: 'btn btn-danger', text: 'CANCEL', onClick: function ($noty) {
                    $noty.close();
                }
            }]
        });

        //if (confirm("Are you sure to Approve this quote?")) {
        //    $.ajax({
        //        url: '/QuoteList/ApproveQuote?QuoteID=' + $('#QuoteID').val(),
        //        type: 'post',
        //        dataType: 'json',
        //        success: function (result) {
        //            if (result.redirect) {
        //                window.location.replace(result.redirectAction);
        //            }
        //        },
        //        error: function (e) {
        //        },
        //        async: false
        //    });            
        //}
        return false;
    });

    $('#rejectQuote').click(function () {
        notyAlert = noty({
            text: "Are you sure to reject this quote?",
            type: 'error',
            buttons: [{
                addClass: 'btn btn-primary', text: 'YES', onClick: function ($noty) {
                    $noty.close();
                    var description = prompt("Enter your comments");
                    if (description != null) {
                        $.ajax({
                            url: '/QuoteList/RejectQuote?QuoteID=' + $('#QuoteID').val() + '&Description=' + description,
                            type: 'post',
                            dataType: 'json',
                            success: function (result) {
                                if (result.redirect) {
                                    window.location.replace(result.redirectAction);
                                }
                            },
                            error: function (e) {
                            },
                            async: false
                        });
                    }
                }
            },
            {
                addClass: 'btn btn-danger', text: 'CANCEL', onClick: function ($noty) {
                    $noty.close();
                }
            }]
        });

        //if (confirm("Are you sure to reject this quote?")) {
        //    var description = prompt("Enter your comments");
        //    if (description != null) {
        //        $.ajax({
        //            url: '/QuoteList/RejectQuote?QuoteID=' + $('#QuoteID').val() + '&Description=' + description,
        //            type: 'post',
        //            dataType: 'json',
        //            success: function (result) {
        //                if (result.redirect) {
        //                    window.location.replace(result.redirectAction);
        //                }
        //            },
        //            error: function (e) {
        //            },
        //            async: false
        //        });                
        //    }
        //}
        return false;
    });

    $('#Quote span.field-validation-error').each(function () {
        if ($(this).text() != '') {
            QuoteError = QuoteError + 1;
        }
    });

    $('#BillingAddress span.field-validation-error').each(function () {
        if ($(this).text() != '') {
            BillingError = BillingError + 1;
        }
    });

    $('#ShippingAddress span.field-validation-error').each(function () {
        if ($(this).text() != '') {
            ShippingError = ShippingError + 1;
        }
    });

    if (QuoteError != 0) {
        $("ul.tabs li").removeClass("active");
        $("ul.tabs li:first").addClass("active").show(); //Activate first tab
        $(".tab_content").hide(); //Hide all content
        $(".tab_content:first").show(); //Show first tab content
    }
    else if (QuoteError == 0 && BillingError != 0) {
        $("ul.tabs li").removeClass("active");
        $("ul.tabs li:eq(1)").addClass("active").show(); //Activate first tab
        $(".tab_content").hide(); //Hide all content
        $(".tab_content:eq(1)").show(); //Show first tab content
    }
    else if (QuoteError == 0 && BillingError == 0 && ShippingError != 0) {
        $("ul.tabs li").removeClass("active");
        $("ul.tabs li:eq(2)").addClass("active").show(); //Activate first tab
        $(".tab_content").hide(); //Hide all content
        $(".tab_content:eq(2)").show(); //Show first tab content
    }
    else {
        $(".tab_content").hide(); //Hide all content
        $("ul.tabs li").removeClass("active");
        $("ul.tabs li:first").addClass("active").show(); //Activate first tab
        $(".tab_content:first").show(); //Show first tab content
    }

    //$('#ProductsTab').tabs({ active: 0 });

    //$('#verticalTab').tabs({ active: 0, activate: function (event, ui) { } });

    //$("#verticalTab").on("tabsactivate", function (event, ui) {
    //    $('#RentalProductQuoteSingleRecord').css('display', 'none');
    //    $('#RentalProductSingleRecord').css('display', 'none');
    //    $('#RentalProductPopup_lightbox').css('display', 'none');
    //    $('#RentalSparsQuoteBasedSingleRecord').css('display', 'none');
    //    $('#RentalSparsSingleRecord').css('display', 'none');
    //    $('#RentalSparPopup_LightBox').css('display', 'none');
    //});

    $('#RequestedDate').datepicker({
        //changeMonth: true,
        //changeYear: true,
        dateFormat: 'd-M-y'
        //showOn: 'button',
        //buttonText: 'Calender'
    });

    $('#SameBillingAddress').click(function () {
        if ($(this).prop('checked') == true) {
            $('#ShipAddressOne').val($('#BillAddressOne').val());
            $('#ShipAddressTwo').val($('#BillAddressTwo').val());
            $('#ShipCity').val($('#BillCity').val());
            var index = document.getElementById('BillCountry').selectedIndex;
            print_state('ShipState', index);
            $('#ShipState').val($('#BillState').val());
            $('#ShipCountry').val($('#BillCountry').val());
            $('#ShipPostalCode').val($('#BillPostalCode').val());
            $('#ShipPhone').val($('#BillPhone').val());
            $('#ShipMobile').val($('#BillMobile').val());
            $('#CustomerNameShip').val($('#CustomerNameBill').val());
        }
        else {
            $('#ShipAddressOne').val('');
            $('#ShipAddressTwo').val('');
            $('#ShipCity').val('');
            $('#ShipState').val('');
            $('#ShipCountry').val('');
            $('#ShipPostalCode').val('');
            $('#ShipPhone').val('');
            $('#ShipMobile').val('');
        }
    });


    $('#ColabratorUpdate').click(function () {
        $('#collabratorCommentbox').val('');
        LoadCollabratorComments('Approve');
    });

    $('#ColabratorCancel').click(function () {
        $('#collabratorCommentbox').val('');
        LoadCollabratorComments('Cancel');
    });

    function LoadCollabratorComments(status) {
        $('#ColabratorStatus').val(status);
        var popMargTop = ($('#CollabratorComments').height() + 24) / 2;
        var popMargLeft = ($('#CollabratorComments').width() + 24) / 2;

        $('#CollabratorComments').css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#mask').fadeIn(300);
        //Fade in the Popup
        $('#CollabratorComments').fadeIn(300);
    }

    $('#CollabratorComments_close').click(function () {
        $('#CollabratorComments').fadeOut(300);
        $('#mask').fadeOut(300);
    });

    $('#CollabratorCommentsButton').click(function () {
        var comments = $('#collabratorCommentbox').val();
        if ($('#ColabratorStatus').val() === 'Approve') {
            if (comments !== '') {
                var QuoteID = $('#QuoteID').val();
                var StatusDescription = comments;
                if (StatusDescription != '') {
                    $.ajax({
                        url: '/QuoteList/UpdateCollabratorToQuote?QuoteID=' + QuoteID + '&QuoteStatusDescription=' + StatusDescription,
                        type: 'post',
                        dataType: 'json',
                        success: function (result) {
                            if (result.redirect) {
                                window.location.replace(result.redirectAction);
                            }
                            else {
                                if (result.Result === 'success') {
                                    $('#CollabratorComments').fadeOut(300);
                                    $('#mask').fadeOut(300);
                                }
                            }
                        },
                        error: function (e) {
                        },
                        async: false
                    });                    
                }                
            }
        }
        else {
            if (comments !== '') {                
                var QuoteID = $('#QuoteID').val();
                if (confirm("Are you sure to reject this quote?")) {
                    var StatusDescription = comments;
                    if (StatusDescription != null) {
                        $.ajax({
                            url: '/QuoteList/DeleteCollabratorToQuote?QuoteID=' + QuoteID + '&QuoteStatusDescription=' + StatusDescription,
                            type: 'post',
                            dataType: 'json',
                            success: function (result) {
                                if (result.redirect) {
                                    window.location.replace(result.redirectAction);
                                }
                                else {
                                    if (result.Result === 'success') {
                                        $('#CollabratorComments').fadeOut(300);
                                        $('#mask').fadeOut(300);
                                    }
                                }
                            },
                            error: function (e) {
                            },
                            async: false
                        });                        
                    }
                }
            }
        }
    });

    $('#SummarryPage').click(function () {
        var QuoteID = $('#QuoteID').val();
        window.location.replace("/QuoteList/QuoteSummary?QuoteID=" + QuoteID);
    });

    $('#createClear').click(function () {
        $('#ShipAddressOne').val('');
        $('#ShipAddressTwo').val('');
        $('#ShipCity').val('');
        $('#ShipState').val('');
        $('#ShipCountry').val('');
        $('#ShipPostalCode').val('');
        $('#ShipPhone').val('');
        $('#ShipMobile').val('');
        $('#CustomerNameShip').val('');


        $('#BillAddressOne').val('');
        $('#BillAddressTwo').val('');
        $('#BillCity').val('');
        $('#BillState').val('');
        $('#BillCountry').val('');
        $('#BillPostalCode').val('');
        $('#BillPhone').val('');
        $('#BillMobile').val('');
        $('#CustomerNameBill').val('');

        $('#CustomerCode').val('');
        $('#CustomerName').val('');
        $('#RequestedDate').val('');
        $('#QuoteName').val('');
        $('#BudgetValue').val('');
        $('#SalesOrgName').prop('selectedIndex', 0);
        $('#CurrencyNameField').prop('selectedIndex', 0);

        $('#SameBillingAddress').prop('checked', false);

        $("ul.tabs li").removeClass("active");
        $("ul.tabs li:first").addClass("active").show(); //Activate first tab
        $(".tab_content").hide(); //Hide all content
        $(".tab_content:first").show(); //Show first tab content

    });
});

function SalesChange(data) {
    $('#CustomerCode').val(data.value);
}

function CurrencyChange(data) {
    $('#BudgetValue').val(data.value.slice(0, -5));
}