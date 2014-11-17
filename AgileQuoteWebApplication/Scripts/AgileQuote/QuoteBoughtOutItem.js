

$(document).ready(function () {

 
    ReloadBoughtoutItemDataTable();

    $("#QuoteBoughtItemDatatable_length").remove();

    GetTotalBoughtoutItemPrice();
    GetDataTableLength();

    $('#AddItem').click(function () {
        var popMargTop = ($('#AddBoughtItemPopup').height() + 24) / 2;
        var popMargLeft = ($('#AddBoughtItemPopup').width() + 24) / 2;

        $('#AddBoughtItemPopup').css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        $('#AddBoughtItemRecordId').val('');
        $('#AddBoughtItemRecordName').val('');
        $('#AddBoughtItemRecordName').removeAttr('disabled');
        $('#AddBoughtItemRecordUnitprice').val('');
        $('#AddBoughtItemRecordQuantity').val('');
        $('#AddBoughtItemRecordUpdate').val("Insert");

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#mask').fadeIn(300);
        //Fade in the Popup
        $('#AddBoughtItemPopup').fadeIn(300);
    });

    $('#AddBoughtItemPopup_close').click(function () {
        $('#mask').css('display', 'none');
        $('#AddBoughtItemPopup').css('display', 'none');
    });

    $('#AddBoughtItemRecordUnitprice').keyup(function (e) {
        if (!/^[0-9]+$/.test($(this).val())) {
            if ($(this).val().length != 0) {
                $(this).val($(this).val().slice(0, -1));
            }
        }
    });

    $('#AddBoughtItemRecordQuantity').keyup(function (e) {
        if (!/^[0-9]+$/.test($(this).val())) {
            if ($(this).val().length != 0) {
                $(this).val($(this).val().slice(0, -1));
            }
        }
    });

    $('#AddBoughtItemRecordUpdate').click(function () {
        var quoteID = $('#QuoteID').val();
        var name = $('#AddBoughtItemRecordName').val();
        var unitPrice = $('#AddBoughtItemRecordUnitprice').val();
        var quantity = $('#AddBoughtItemRecordQuantity').val();
        var itemID = $('#AddBoughtItemRecordId').val();

        if (name != '' && unitPrice != '' && quantity != '') {
            $('#NameError').text('');
            $('#UnitError').text('');
            $('#QuantityError').text('');
            if (itemID == '') {
                $.ajax({
                    url: '/QuoteBoughtOutItem/InsertBoughtoutItem?QuoteID=' + quoteID + '&Name=' + name + '&UnitPrice=' + unitPrice + '&Quantity=' + quantity,
                    type: 'post',
                    dataType: 'json',
                    success: function (data) {
                        if (data.redirect) {
                            window.location.replace(redirectAction);
                        }
                        else {
                            ReloadBoughtoutItemDataTable();
                            GetTotalBoughtoutItemPrice();
                            GetDataTableLength();
                            $('#AddBoughtItemPopup').css('display', 'none');
                            $('#mask').css('display', 'none');
                        }
                    },
                    error: function (e) { },
                    async: false
                });
            }
            else {
                $.ajax({
                    url: '/QuoteBoughtOutItem/UpdateQuoteBasedBoughtoutItemRecord?QuoteId=' + quoteID + '&Itemid=' + itemID + '&UnitPrice=' + unitPrice + '&Quantity=' + quantity,
                    type: 'post',
                    dataType: 'json',
                    success: function (data) {
                        if (data.redirect) {
                            window.location.replace(redirectAction);
                        }
                        else {
                            ReloadBoughtoutItemDataTable();
                            GetTotalBoughtoutItemPrice();
                            GetDataTableLength();
                            $('#AddBoughtItemPopup').css('display', 'none');
                            $('#mask').css('display', 'none');
                        }
                    },
                    error: function (e) { },
                    async: false
                });
            }            
        }
        else {
            if (name == '' && unitPrice == '' && quantity == '') {
                $('#NameError').text('*');
                $('#UnitError').text('*');
                $('#QuantityError').text('*');
            }
            else if (name != '' && unitPrice != '' && quantity == '') {
                $('#NameError').text('');
                $('#UnitError').text('');
                $('#QuantityError').text('*');
            }
            else if (name != '' && unitPrice == '' && quantity != '') {
                $('#NameError').text('');
                $('#UnitError').text('*');
                $('#QuantityError').text('');
            }
            else if (name == '' && unitPrice != '' && quantity != '') {
                $('#NameError').text('*');
                $('#UnitError').text('');
                $('#QuantityError').text('');
            }
            else if (name != '' && unitPrice == '' && quantity == '') {
                $('#NameError').text('');
                $('#UnitError').text('*');
                $('#QuantityError').text('*');
            }
            else if (name == '' && unitPrice != '' && quantity == '') {
                $('#NameError').text('*');
                $('#UnitError').text('');
                $('#QuantityError').text('*');
            }
            else if (name == '' && unitPrice == '' && quantity != '') {
                $('#NameError').text('*');
                $('#UnitError').text('*');
                $('#QuantityError').text('');
            }
        }
    });
});



function QuoteBoughtoutItemEditFunction(data) {
    var quoteID = $('#QuoteID').val();

    //var EditStatus = $('#QuoteEditAutho').val();

    //if (EditStatus === "True") {
        $.ajax({
            url: '/QuoteBoughtOutItem/GetQuoteBoughtoutItemRecord?QuoteId=' + quoteID + '&ItemId=' + data.id,
            type: 'post',
            dataType: 'json',
            success: function (d) {
                if (d.Redirect) {
                    window.location.replace(d.RedirectAction);
                }
                else {
                    $('#AddBoughtItemRecordId').val(d.Result.ItemId);
                    $('#AddBoughtItemRecordName').val(d.Result.ItemName);
                    $('#AddBoughtItemRecordName').attr('disabled', 'disabled');
                    $('#AddBoughtItemRecordUnitprice').val(d.Result.UnitPrice);
                    $('#AddBoughtItemRecordQuantity').val(d.Result.Quantity);
                    $('#AddBoughtItemRecordUpdate').val('Update');

                    var popMargTop = ($('#AddBoughtItemPopup').height() + 24) / 2;
                    var popMargLeft = ($('#AddBoughtItemPopup').width() + 24) / 2;

                    $('#AddBoughtItemPopup').css({
                        'margin-top': -popMargTop,
                        'margin-left': -popMargLeft
                    });

                    // Add the mask to body
                    $('body').append('<div id="mask"></div>');
                    $('#mask').fadeIn(300);
                    //Fade in the Popup
                    $('#AddBoughtItemPopup').fadeIn(300);

                }
            },
            error: function (e) {
            },
            async: false
        });
    //}    
}

function QuoteBoughtoutItemDeleteFuction(data) {
    var quoteID = $('#QuoteID').val();
    //var EditStatus = $('#QuoteEditAutho').val();
    //if (EditStatus === "True") {
        $.ajax({
            url: '/QuoteBoughtOutItem/DeleteQuoteBasedBoughtoutItemRecord?QuoteID=' + quoteID + '&ItemId=' + data.id,
            type: 'post',
            dataType: 'json',
            success: function (d) {
                if (d.Redirect) {
                    window.location.replace(d.RedirectAction);
                }
                else {
                    ReloadBoughtoutItemDataTable();
                    GetTotalBoughtoutItemPrice();
                    GetDataTableLength();
                }
            },
            error: function (e) {
            },
            async: false
        });
    //}    
}

function ReloadBoughtoutItemDataTable() {
    var quoteID = $('#QuoteID').val();
    $('table').css('width', '100%');
    if (quoteID != '') {
        $('#QuoteBoughtItemDatatable').dataTable({
            "aaData": [
            /* Reduced data set */
            ["Trident", "Internet Explorer 4.0", "Win 95+", 4, "X"],
            ["Trident", "Internet Explorer 5.0", "Win 95+", 5, "C"],
            ["Trident", "Internet Explorer 5.5", "Win 95+", 5.5, "A"],
            ["Trident", "Internet Explorer 6.0", "Win 98+", 6, "A"],
            ["Trident", "Internet Explorer 7.0", "Win XP SP2+", 7, "A"],
            ["Gecko", "Firefox 1.5", "Win 98+ / OSX.2+", 1.8, "A"],
            ["Gecko", "Firefox 2", "Win 98+ / OSX.2+", 1.8, "A"],
            ["Gecko", "Firefox 3", "Win 2k+ / OSX.3+", 1.9, "A"],
            ["Webkit", "Safari 1.2", "OSX.3", 125.5, "A"],
            ["Webkit", "Safari 1.3", "OSX.3", 312.8, "A"],
            ["Webkit", "Safari 2.0", "OSX.4+", 419.3, "A"],
            ["Webkit", "Safari 3.0", "OSX.4+", 522.1, "A"]
            ],
            "bServerSide": true,
            "sAjaxSource": "/QuoteBoughtOutItem/GetQuoteBoughtOutItemDatatable?QuoteID=" + quoteID,
            "bProcessing": true,
            "sPaginationType": "full_numbers",
            //"iDisplayLength": 1,
            "bDestroy": true,
            "aoColumns": [
                        { "sTitle": "Code", "sName": "ItemId" },
                        { "sTitle": "Name", "sName": "ItemName" },
                        { "sTitle": "Unit Cost", "sName": "UnitPrice" },
                        { "sTitle": "Quantity", "sName": "Quantity" },
                        { "sTitle": "Total Cost", "sName": "NetPrice" },
                        { "sTitle": "Unit Price", "sName": "quotedUnitPrice" },
                        { "sTitle": "Total Price", "sName": "quotedNetPrice" },
                        {
                            "sTitle": "Action",
                            "sName": "Code",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (obj) {
                                return '<a href="#" id=' + obj.aData[0] + ' onclick="javascript:QuoteBoughtoutItemEditFunction(this)"  style="padding-left: 20px;"><img src="/Content/images/edit.png" alt="EDIT" title="Edit" /></a><a href="#" id=' + obj.aData[0] + ' onclick="javascript:QuoteBoughtoutItemDeleteFuction(this)" style="float:left"><img alt="DELETE" src="/Content/images/error.png" title="Delete"></a>'
                            }
                        }
            ]
        });       

    }    
}

function GetTotalBoughtoutItemPrice() {
    var quoteID = $('#QuoteID').val();
    $.ajax({
        url: '/QuoteBoughtOutItem/GetTotalQuoteBoughtoutPrice?QuoteID=' + quoteID,
        type: 'post',
        dataType: 'json',
        success: function (result) {
            if (result.oTotalUnitDiscount != '') {
                $('#BoughtUnitPrice').val(result.oTotalUnitDiscount.TotalUnitPrice);
                $('#BoughtQuantity').val(result.oTotalUnitDiscount.TotalQuantity);
                $('#BoughtNetPrice').val(result.oTotalUnitDiscount.TotalNetPrice);
                $('#BoughtQuoteUnitPrice').val(result.oTotalUnitDiscount.TotalQuotedUnitPrice);
                $('#BoughtQuoteNetPrice').val(result.oTotalUnitDiscount.TotalQuotedNetPrice);
                //$('#BoughtUnitPrice').val(formatCurrency(result.oTotalUnitDiscount.TotalUnitPrice));
                //$('#BoughtQuantity').val(result.oTotalUnitDiscount.TotalQuantity);
                //$('#BoughtNetPrice').val(formatCurrency(result.oTotalUnitDiscount.TotalNetPrice));
                //$('#BoughtQuoteUnitPrice').val(formatCurrency(result.oTotalUnitDiscount.TotalQuotedUnitPrice));
                //$('#BoughtQuoteNetPrice').val(formatCurrency(result.oTotalUnitDiscount.TotalQuotedNetPrice));
            }
        },
        error: function (e) { },
        async: false
    });
}

function GetDataTableLength() {
    var quoteID = $('#QuoteID').val();
    $.ajax({
        url: '/QuoteBoughtOutItem/GetQuoteBoughtOutItemListLength?QuoteID=' + quoteID,
        type: 'post',
        dataType: 'json',
        success: function (d) {
            if (d.Redirect) {
                window.location.replace(d.RedirectAction);
            }
            else {
                if (d.Result === 0) {                    
                    $('#BoughtOutItemTotal').css('display', 'none');
                }
                else {
                    $('#BoughtOutItemTotal').css('display', 'block');
                }
            }
        },
        error: function (e) {
        },
        async: false
    });
}

function formatCurrency(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num)) {
        num = "0";
    }

    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();

    if (cents < 10) {
        cents = "0" + cents;
    }
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++) {
        num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
    }

    return (((sign) ? '' : '-') + num + '.' + cents);
}