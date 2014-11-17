var notyAlert = null;
$(document).ready(function () {
    ReloadStatusDataTable();

    $("#QuoteStatusDataTable_length").empty();


    $('#AssignApprover').click(function () {
        var quoteID = $('#QuoteID').val();
        AssignApprover(quoteID);
        //$.ajax({
        //    url: '/QuoteStatus/AssignApproverToQuote?QuoteID=' + quoteID,
        //    type: 'post',
        //    dataType: 'json',
        //    success: function (data) {
        //        if (data.redirect) {
        //            window.location.replace(redirectAction);
        //        }
        //        else {
        //            if (data.Result !== "success") {        
        //            }
        //            else {
        //                ReloadStatusDataTable();
        //                $('#AddCollabratorList').css('display', 'none');
        //            }
        //        }
        //    },
        //    error: function (e) {
        //    },
        //    async: false
        //});
    });

    $('#Summary').click(function () {
        var QuoteID = $('#QuoteID').val();
        window.location.replace("/Summary/QuoteSummary?QuoteID=" + QuoteID);
    });

    $('#CreateProposal').click(function () {
        var QuoteID = $('#QuoteID').val();
        window.location.replace('/pdf/proposal?QuoteId=' + QuoteID);
    });

});

function AssignApprover(quoteID) {
    $.ajax({
        url: '/QuoteStatus/AssignApproverToQuote?QuoteID=' + quoteID,
        type: 'post',
        dataType: 'json',
        success: function (data) {
            if (data.redirect) {
                window.location.replace(redirectAction);
            }
            else {
                if (data.Result !== "success") {
                    $.noty.closeAll();
                    notyAlert = noty({
                        text: data.Result,
                        type: 'error',
                        buttons: [{
                            addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]
                    });                    
                }
                else {
                    ReloadStatusDataTable();
                    $('#AddCollabratorList').css('display', 'none');
                }
            }
        },
        error: function (e) {
        },
        async: false
    });
}

function ReloadStatusDataTable() {
    var quoteID = $('#QuoteID').val();
    $('table').css('width', '100%');
    if (quoteID != '') {
        $('#QuoteStatusDataTable').dataTable({
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
            "sAjaxSource": "/QuoteStatus/GetQuoteStatusList?QuoteID=" + quoteID,
            "bProcessing": true,
            "sPaginationType": "full_numbers",
            //"iDisplayLength": 1,
            "bDestroy": true,
            "aoColumns": [
                        { "sTitle": "User ID", "sName": "AuthorizerID" },
                        { "sTitle": "Role Name", "sName": "RoleName" },
                        { "sTitle": "Email Address", "sName": "EmailAddress" },
                        { "sTitle": "Status", "sName": "QuoteStatus" },
                        {
                            "sTitle": "Status Description", "sName": "StatusDescription", "bSearchable": false,
                            "bSortable": false
                        }
            ]
        });
    }

}