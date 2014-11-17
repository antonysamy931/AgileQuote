
//var dataArray = new Array();
//var singleArray = new Array();
$(document).ready(function () {
    ReloadMaterialBundleWarrentyDataTable();

    $("#QuoteMaterialBundleWarrent_length").empty();

    //for (var i = 6; i <= 60; i = i + 6) {
    //    singleArray.push(i+':'+i);
    //    var month = i % 12;
    //    var year = i / 12;

    //    var fixedYear = parseInt(year);
    //    if (i === 6) {
    //        dataArray.push("key=" + i + ":value=" + month + " Months");
    //    }
    //    else {
    //        if (month === 0) {
    //            dataArray.push("key=" + i + ":value=" + fixedYear + " Year");
    //        }
    //        else {
    //            dataArray.push("key=" + i + ":value=" + fixedYear + " Year " + month + " Months");
    //        }
    //    }
    //}
});

function ReloadMaterialBundleWarrentyDataTable() {
    var quoteID = $('#QuoteID').val();
    $('table').css('width', '100%');
    if (quoteID != '') {
        $('#QuoteMaterialBundleWarrent').dataTable({
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
            "sAjaxSource": "/QuoteMaterialBundleWarrenty/GetQuoteMaterialBundleListWarrenty?QuoteID=" + quoteID,
            "bProcessing": true,
            "sPaginationType": "full_numbers",
            //"iDisplayLength": 1,
            "bDestroy": true,
            "aoColumns": [
                        { "sTitle": "Code", "sName": "Code" },
                        { "sTitle": "Name", "sName": "Name" },
                        { "sTitle": "Description", "sName": "Description", "bSearchable": false, "bSortable": false },
                        { "sTitle": "Quantity", "sName": "Quantity" },
                        { "sTitle": "Unit Price", "sName": "UnitPrice" },
                        { "sTitle": "Warranty", "sName": "Warrenty", "sClass": "EditableClass EditableFieldBgColor" },
                        { "sTitle": "Type", "sName": "Type" }
            ],
            "fnDrawCallback": function () {
                $("td.EditableClass").editable(function (e) {
                    var code = $(this).closest('tr').find('td:eq(0)').text();
                    var type = $(this).closest('tr').find('td:eq(6)').text();
                    var TypeOf;
                    if (type === 'B') {
                        TypeOf = 'Bundle';
                    }
                    else {
                        TypeOf = 'Material';
                    }
                    $.ajax({
                        url: '/QuoteMaterialBundleWarrenty/UpdateBundleMaterialWarrent?QuoteID=' + quoteID + '&Code=' + code + '&Type=' + TypeOf + '&Warrenty=' + e,
                        type: 'post',
                        dataType: 'json',
                        success: function (data) {
                            if (data.redirect) {
                                window.location.replace(redirectAction);
                            }
                            //else {
                            //    ReloadMaterialBundleOverrideWarrentyDataTable();
                            //    $('#OverrideWarrentyPopup').css('display', 'none');
                            //    $('#mask').css('display', 'none');
                            //}
                        },
                        error: function (e) { },
                        async: false
                    });                    
                    return e;
                }, {
                    data: " {'6':'6','12':'12','18':'18', '24':'24','30':'30','36':'36','42':'42', '48':'48','54':'54','60':'60','66':'66', '72':'72'}",
                    //data: singleArray,
                    //cancel: 'Cancel',
                    placeholder: '---',
                    submit: 'Save',
                    tooltip: 'Click to edit',
                    type: 'select',
                });
            }
        });

        $("#QuoteMaterialBundleWarrent").qtip({
            content: 'Warrenty Column Editable',
            style: {
                //width: 400,
                padding: 5,
                background: '#1971B5',
                color: '#FFFFFF',
                textAlign: 'center',
                border: {
                    width: 7,
                    radius: 5,
                    color: '#1971B5'
                },
                tip: 'bottomLeft',
                name: 'dark' // Inherit the rest of the attributes from the preset dark style
            },
            position: {
                corner: {
                    target: 'topLeft',
                    tooltip: 'bottomLeft'
                }
            },
            show: 'mouseover',
            //show: {
            //    when: false, // Don't specify a show event
            //    ready: true // Show the tooltip when ready
            //},
            //hide: { when: { event: 'click' } }
            //hide: false
            hide:'mouseout'
        });

    }

}
