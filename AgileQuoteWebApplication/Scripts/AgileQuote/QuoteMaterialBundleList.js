var MaterialIDList = new Array();
var BundleIDList = new Array();
var MaterialIDQuantityList = new Array();
var SugectionMaterialID = new Array();
var OfferedMaterialID = new Array();
var BundleIDQuantityList = new Array();
var anOpen = [];
var oTable = [];
var budgetTargetRate;
var minimumAmount;
var maximumAmount;
var GuidMaterialIDQuantityList = new Array();
var notyAlert = null;

$(document).ready(function () {

    $('.total-text span').each(function () {
        $(this).html($(this).text());
    });

    $.ajax({
        url: '/QuoteMaterialBundleList/GetMinimumMaximumAmount',
        type: 'post',
        dataType: 'json',
        success: function (result) {
            minimumAmount = result.Amounts[0];
            maximumAmount = result.Amounts[1];                        
        },
        error: function (e) {
        },
        async: false
    });

    $("#slider-range").slider({
        range: true,
        min: minimumAmount,
        max: maximumAmount,
        step: 50,
        values: [minimumAmount, maximumAmount],
        slide: function (event, ui) {
            $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
            $('#Dynamic_guidMaterial').html('');
            LoadGuidMaterial('', '', ui.values[0], ui.values[1]);
        }
    });

    $("#amount").val("$" + $("#slider-range").slider("values", 0) +
    " - $" + $("#slider-range").slider("values", 1));

    //$('#QuoteBundleMaterialDataTable tbody tr td p').tooltip("option", "position", { my: "left+15 center", at: "right center" });

    budgetTargetRate = $('#BudgetValue').val();

    loadFunction();

    ReloadMaterialBundleDataTable();

    $("#QuoteBundleMaterialDataTable_length").empty();


    setInterval(function () { TotalUnitNetPriceTrimForMaterialBundle() }, 2000);

    $('#AddBundleMaterial').click(function () {
        AddBundleMaterialLightBoxFadeIn();
        var notyBox = noty({ text: 'Quatity column editable', model: false, type: 'success', layout: 'top' });
        return false;
    });

    $('#BundleMaterialPopup_close').click(function () {
        SugectionMaterialID = [];
        OfferedMaterialID = [];
        MaterialIDQuantityList = [];
        BundleIDQuantityList = [];
        MaterialIDList = [];
        BundleIDList = [];
        GuidMaterialIDQuantityList = [];
        $('#mask').css('display', 'none');
        $('#BundleMaterialPopup').css('display', 'none');
        $.noty.closeAll();
    });

    function AddBundleMaterialLightBoxFadeInSeggectionOfferClose() {
        $("#CategoryMaterialAccordion").accordion();
        $("#CategoryMaterialAccordion").css('height', '360px');
        $("#CategoryMaterialAccordion > div").css('height', '330px');

        $('#CategoryBundleAccordion').accordion();
        $('#CategoryBundleAccordion').css('height', '360px');
        $("#CategoryBundleAccordion > div").css('height', '330px');

        var popMargTop = ($('#BundleMaterialPopup').height() + 24) / 2;
        var popMargLeft = ($('#BundleMaterialPopup').width() + 24) / 2;

        $('#BundleMaterialPopup').css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#mask').fadeIn(300);
        //Fade in the Popup
        $('#BundleMaterialPopup').fadeIn(300);
    }

    function AddBundleMaterialLightBoxFadeIn() {
        $("#CategoryMaterialAccordion").accordion();
        $("#CategoryMaterialAccordion").css('height', '360px');
        $("#CategoryMaterialAccordion > div").css('height', '330px');

        $('#CategoryBundleAccordion').accordion();
        $('#CategoryBundleAccordion').css('height', '360px');
        $("#CategoryBundleAccordion > div").css('height', '330px');

        var popMargTop = ($('#BundleMaterialPopup').height() + 24) / 2;
        var popMargLeft = ($('#BundleMaterialPopup').width() + 24) / 2;

        $('#BundleMaterialPopup').find('input[type=checkbox]:checked').removeAttr('checked');
        $('table tr').find('td:eq(9) a').text('');
        $('table tr').find('td:eq(5)').editable('disable');
        $('table tr').find('td:eq(5)').removeClass('EditableFieldBgColor');

        $('#BundleMaterialPopup').css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#mask').fadeIn(300);
        //Fade in the Popup
        $('#BundleMaterialPopup').fadeIn(300);
    }

    function loadFunction() {
        /*Material Start*/
        $('table').css('width', '100%');
        /*Dynamic Accordion*/
        $('#DynamicMaterialDataTable').append('<div id="CategoryMaterialAccordion"></div><div class="clear"></div>');
        $('#DynamicBundleDataTable').append('<div id="CategoryBundleAccordion"></div><div class="clear"></div>');
        /*Dynamic Accordion*/

        /*Category get ajax method*/
        $.ajax({
            url: '/QuoteMaterialBundleList/GetCategoryNameList',
            type: 'post',
            dataType: 'json',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var category = data[i];
                    /*Dinamic Accordion h3 and div add*/
                    $('#CategoryMaterialAccordion').append('<h3 class="active-accord">' + data[i] + '</h3><div><p id="CategoryMaterial_' + data[i] + '"></p></div>');
                    $('#CategoryMaterial_' + data[i]).append('<div class="quote_list"><table cellpadding="0" cellspacing="0" border="0" class="display" id="Material_' + data[i] + '"></table></div>');
                    /*End*/

                    /*Material Data table*/

                    $('#Material_' + data[i]).dataTable({
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
                        "sAjaxSource": "/QuoteMaterialBundleList/GetMaterialList?Category=" + data[i] + "&BudgetTargetRate=" + budgetTargetRate,
                        "bProcessing": true,
                        "sPaginationType": "full_numbers",
                        //"iDisplayLength": 1,
                        "aoColumns": [
                                    { "sName": "MaterialId", "bSearchable": false, "bSortable": false, "sClass": "DisplayNone" },

                                    { "sTitle": "Material Code", "sName": "MaterialCode" },
                                    { "sTitle": "Name", "sName": "MaterialName" },
                                    { "sTitle": "Discount", "sName": "MaterialDiscount", "sClass": "DisplayNone" },
                                    { "sTitle": "Cost", "sName": "MaterialAmount" },
                                    { "sTitle": "Quantity", "sName": "Quantity" },
                                    {
                                        "sTitle": "View",
                                        "sName": "MaterialId",
                                        "bSearchable": false,
                                        "bSortable": false,
                                        "fnRender": function (obj) {
                                            return '<a href="#" id=' + obj.aData[0] + ' onclick="javascript:MaterialViewFunction(this)" style="float:left"><img alt="EDIT" src="/Content/images/edit.png" title="Edit"></a>'
                                        }
                                    },
                                     {
                                         "sName": "MaterialId",
                                         "bSearchable": false,
                                         "bSortable": false,
                                         "fnRender": function (obj) {
                                             return '<input type="checkbox" value=' + obj.aData[0] + ' onclick="javascript:MaterialAddFunction(this)"/>'
                                         }
                                     },

                                    {
                                        "sName": "Code",
                                        "bSearchable": false,
                                        "bSortable": false,
                                        "fnRender": function (obj) {
                                            var imgSrc = "http://172.17.0.147:8060/Content/Material/" + obj.aData[0] + "/icon.jpg";
                                            return '<img src="' + imgSrc + '"/>';
                                        }
                                    },

                                    {
                                        "sName": "Code",
                                        "bSearchable": false,
                                        "bSortable": false,
                                        "fnRender": function (obj) {
                                            return '<a href="#" id=' + obj.aData[0] + ' onclick="javascript:GuidMaterial(this)"></a>';
                                        }
                                    }

                        ]
                    });

                    /*Material data table end*/

                    /*Dynamic accordion h3 and div add*/
                    $('#CategoryBundleAccordion').append('<h3 class="active-accord">' + data[i] + '</h3><div><p id="CategoryBundle_' + data[i] + '"></p></div>');
                    $('#CategoryBundle_' + data[i]).append('<div class="quote_list"><table cellpadding="0" cellspacing="0" border="0" class="display" id="Bundle_' + data[i] + '"></table></div>');
                    /*End*/

                    /*Bundle data table start*/
                    oTable[i] = $('#Bundle_' + data[i]).dataTable({
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
                        "sAjaxSource": "/QuoteMaterialBundleList/GetBundleList?Category=" + data[i] + "&BudgetTargetRate=" + budgetTargetRate,
                        "bProcessing": true,
                        "sPaginationType": "full_numbers",
                        //"iDisplayLength": 1,
                        "aoColumns": [
                                    {
                                        "sTitle": "", "sName": "BundleId",
                                        "bSearchable": false, "bSortable": false,
                                        "sClass": "control",
                                        "fnRender": function (obj) {
                                            return '<img src="/Content/images/plus.gif"/>';
                                        }
                                    },
                                    { "sTitle": "Code", "sName": "BundleId" },
                                    { "sTitle": "Name", "sName": "BundleName" },
                                    { "sTitle": "Discount", "sName": "BundleDiscount", "sClass": "DisplayNone" },
                                    { "sTitle": "Cost", "sName": "BundleNetPrice" },
                                    { "sTitle": "Quantity", "sName": "Quantity" },
                                    {
                                        "sTitle": "View",
                                        "sName": "BundleId",
                                        "bSearchable": false,
                                        "bSortable": false,
                                        "fnRender": function (obj) {
                                            return '<a href="#" id=' + obj.aData[1] + ' onclick="javascript:BundleViewFunction(this)" style="float:left"><img alt="EDIT" src="/Content/images/edit.png" title="Edit"></a>'
                                        }
                                    },
                                    //{
                                    //    "sName": "BundleId",
                                    //    "bSearchable": false,
                                    //    "bSortable": false,
                                    //    "fnRender": function (obj) {
                                    //        return '<input type="text" value=' + obj.aData[0] + ' onclick="javascript:BundleAddFunction(this)"/>'
                                    //    }
                                    //},
                                    {
                                        "sName": "BundleId",
                                        "bSearchable": false,
                                        "bSortable": false,
                                        "fnRender": function (obj) {
                                            return '<input type="checkbox" value=' + obj.aData[1] + ' onclick="javascript:BundleAddFunction(this)"/>'
                                        }
                                    }

                        ]
                    });

                    /*Bundle data table end*/
                }
            },
            error: function (error) {
            }
        });

    }

    $('table td.control').live('click', function () {
        var tableIndex = null;
        var tableID = $(this).parents('table').attr('id');

        for (var item = 0; item < oTable.length; item++) {
            if (oTable[item].attr('id') === tableID) {
                tableIndex = item;
            }
        }

        var bundleID = $(this).find('a').attr('id');
        var nTr = this.parentNode;
        var i = $.inArray(nTr, anOpen);
        if (i === -1) {
            $('img', this).attr('src', "/Content/images/minus.png");
            var nDetailsRow = oTable[tableIndex].fnOpen(nTr, fnFormatDetails(oTable[tableIndex], nTr), 'details');
            $('div.innerDetails', nDetailsRow).slideDown();
            anOpen.push(nTr);
        }
        else {
            $('img', this).attr('src', "/Content/images/plus.gif");
            $('div.innerDetails', $(nTr).next()[0]).slideUp(function () {
                oTable[tableIndex].fnClose(nTr);
                anOpen.splice(i, 1);
            });
        }
    });

    function fnFormatDetails(oTable, nTr) {
        var oData = oTable.fnGetData(nTr);
        var sample = '<div class="innerDetails">' +
             '<table><thead><tr><th>Code</th><th>Name</th><th>Quantity</th></tr></thead>' +
             '<tbody>';
        $.ajax({
            url: '/QuoteMaterialBundleList/GetBundleMappingMaterial?BundleID=' + oData[1] + '&BudgetTargetRate=' + budgetTargetRate,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data.Redirect) {
                    window.location.replace(data.RedirectAction);
                }
                else {


                    for (var i = 0; i < data.Result.length; i++) {
                        if (i % 2 === 0) {
                            sample += '<tr class="odd"><td>' + data.Result[i].MaterialCode + '</td>' +
                                    '<td>' + data.Result[i].MaterialName + '</td>' +
                                    '<td>' + data.Result[i].Quantity + '</td></tr>';
                        }
                        else {
                            sample += '<tr><td>' + data.Result[i].MaterialCode + '</td>' +
                                    '<td>' + data.Result[i].MaterialName + '</td>' +
                                    '<td>' + data.Result[i].Quantity + '</td></tr>';
                        }
                    }
                    sample += '</tbody></table>';
                }

            },
            error: function (e) {
            },
            async: false
        });
        var sOut =
          '<div class="innerDetails">' +
            '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
              '<thead><tr><th>HEAD LINE</th><th>Head line 2</th></tr></thead>' +
              '<tr><td>Rendering engine:</td><td>Antony</td></tr>' +
              '<tr><td>Browser:</td><td>samy</td></tr>' +
              '<tr><td>Platform:</td><td>joseph</td></tr>' +
              '<tr><td>Version:</td><td>mam</td></tr>' +
              '<tr><td>Grade:</td><td>sir</td></tr>' +
            '</table>' +
          '</div>';
        return sample;
    }

    $('#BundleMaterialRecordPopup_close').click(function () {
        $('#mask').css('display', 'none');
        $('#BundleMaterialRecordPopup').css('display', 'none');
    });

    $('#BundleMaterialRecordQuantity').keyup(function (e) {
        if (!/^[0-9]+$/.test($(this).val())) {
            if ($(this).val().length != 0) {
                $(this).val($(this).val().slice(0, -1));
            }
            else {
                var unitprice = $('#BundleMaterialRecordUnitprice').val();
                var discount = $('#BundleMaterialRecordDiscount').val();
                var Quantity = 1;
                var netprice = (parseInt(unitprice) - (parseInt(unitprice) * parseInt(discount) / 100)) * parseInt(Quantity);
                $('#BundleMaterialRecordNetprice').val(netprice);
            }
        }
        else {
            var unitprice = $('#BundleMaterialRecordUnitprice').val();
            var discount = $('#BundleMaterialRecordDiscount').val();
            var Quantity = $(this).val();
            //(parseInt(unitprice) * parseInt(Quantity)) - ((parseInt(unitprice) * parseInt(discount) / 100) * parseInt(Quantity));
            var netprice = (parseInt(unitprice) - (parseInt(unitprice) * parseInt(discount) / 100)) * parseInt(Quantity);
            $('#BundleMaterialRecordNetprice').val(netprice);
        }

    });

    $('#MaterialPopup_close').click(function () {
        $('#MaterialRecordPopup').css('display', 'none');
        AddBundleMaterialLightBoxFadeIn();
    });

    $('#SugectionOfferedPopup_close').click(function () {
        $('#SugectionOfferedPopup').css('display', 'none');
        AddBundleMaterialLightBoxFadeInSeggectionOfferClose();
    });

    $('#BundlePopup_close').click(function () {
        $('#BundleRecordPopup').css('display', 'none');
        AddBundleMaterialLightBoxFadeIn();
    });

    $('#AddtoquoteList').click(function () {
        var QuoteID = $('#QuoteID').val();        
        var tempMaterialIDlist = new Array();
        var tempBundleIDlist = new Array();
        var tempSugectionIDlist = new Array();
        var tempOfferedIDlist = new Array();
        var tempGuidMaterialIDlist = new Array();

        $.each(MaterialIDQuantityList, function (i) {
            var CodeValue = MaterialIDQuantityList[i].ID + '_' + MaterialIDQuantityList[i].Quantity;
            tempMaterialIDlist.push(CodeValue);
        });

        $.each(BundleIDQuantityList, function (i) {
            var CodeValue = BundleIDQuantityList[i].ID + '_' + BundleIDQuantityList[i].Quantity;
            tempBundleIDlist.push(CodeValue);
        });

        $.each(SugectionMaterialID, function (i) {
            var sugValue = SugectionMaterialID[i].MaterialId + ':' + SugectionMaterialID[i].ID + '_' + SugectionMaterialID[i].Quantity;
            tempSugectionIDlist.push(sugValue);
        });

        $.each(OfferedMaterialID, function (i) {
            var offerValue = OfferedMaterialID[i].MaterialId + ':' + OfferedMaterialID[i].ID + '_' + OfferedMaterialID[i].Quantity;
            tempOfferedIDlist.push(offerValue);
        });

        $.each(GuidMaterialIDQuantityList, function (i) {
            var GuidMaterialIDValue = GuidMaterialIDQuantityList[i].ID + '_' + GuidMaterialIDQuantityList[i].Quantity;
            tempGuidMaterialIDlist.push(GuidMaterialIDValue);
        });        

        $.ajax({
            //url: '/QuoteList/AddMaterialBundleToQuote?QuoteID=' + QuoteID + '&MaterialList=' + MaterialIDList + '&BundleList=' + BundleIDList,
            url: '/QuoteMaterialBundleList/AddMaterialBundleToQuote?QuoteID=' + QuoteID + '&MaterialList=' + tempMaterialIDlist + '&BundleList=' + tempBundleIDlist + '&SugectionMaterial=' + tempSugectionIDlist + '&OfferedMaterial=' + tempOfferedIDlist + '&GuidMaterial=' + tempGuidMaterialIDlist,
            type: 'post',
            dataType: 'json',
            success: function (result) {
                if (result.result == 'success') {
                    ReloadMaterialBundleDataTable();
                    MaterialBundleTotalPrice();
                    ReloadMaterialBundleWarrentyDataTable();
                    //ReloadMaterialBundleOverrideWarrentyDataTable();
                    GetQualitativeInformationTotalService();
                    //ReloadMaterialBundleInstallDataTable();
                    //InstallTotalPrice();
                    //TotalPriceTrimForInstall();

                    //LoadClientConfigureBundleList();
                    //BundleConfigureTotalPrice();
                    //loadBundleConfigureFunction();
                    //TotalUnitNetPriceTrimForConfigureBundle();

                    SugectionMaterialID = [];
                    OfferedMaterialID = [];
                    MaterialIDQuantityList = [];
                    BundleIDQuantityList = [];
                    MaterialIDList = [];
                    BundleIDList = [];

                    $('#BundleMaterialPopup').css('display', 'none');
                    $('#mask').css('display', 'none');

                }
            },
            error: function () { },
            async: false
        });
        $.noty.closeAll();
    });

    $('#BundleMaterialRecordUpdate').click(function () {
        var type = $('#BundleMaterialRecordTitle').text();
        var quantity = $('#BundleMaterialRecordQuantity').val();
        var netprice = $('#BundleMaterialRecordNetprice').val();
        var bundleMaterialId = $('#BundleMaterialRecordId').val();
        var quoteid = $('#QuoteID').val();
        var materialid = null;
        var bundleid = null;
        if (type == 'Bundle') {
            bundleid = bundleMaterialId;
        }
        else {
            materialid = bundleMaterialId;
        }

        $.ajax({
            url: '/QuoteMaterialBundleList/UpdateMaterialBundle?QuoteID=' + quoteid + '&MaterialID=' + materialid + '&BundleID=' + bundleid + '&Quantity=' + quantity + '&NetPrice=' + netprice + '&Type=' + type,
            type: 'post',
            dataType: 'json',
            success: function (result) {
                if (result.result == 'success') {
                    ReloadMaterialBundleDataTable();
                    MaterialBundleTotalPrice();
                    ReloadMaterialBundleWarrentyDataTable();
                    GetQualitativeInformationTotalService();
                    //ReloadMaterialBundleOverrideWarrentyDataTable();
                    //ReloadMaterialBundleInstallDataTable();
                    //InstallTotalPrice();
                    //TotalPriceTrimForInstall();
                    $('#BundleMaterialRecordPopup').css('display', 'none');
                    $('#mask').css('display', 'none');
                }
            },
            error: function (e) { },
            async: false
        });
    });

});

function removeA(arr) {
    var what, a = arguments, L = a.length, ax;
    while (L > 1 && arr.length) {
        what = a[--L];
        while ((ax = arr.indexOf(what)) !== -1) {
            arr.splice(ax, 1);
        }
    }
    return arr;
}

function QuoteBundleMaterialEditFunction(data) {

    var budgetTargetRate = $('#BudgetValue').val();
    var string = data.id.split('_');
    var quoteID = $('#QuoteID').val();
    var MaterialID = null;
    var BundleID = null;
    var type = null;
    $('#MaterialMappingBundleItem').remove();
    var sample = '<div class="quote_list" style="padding: 2%;">' +
             '<table id="MaterialMappingBundleItem"><thead><tr><th>Code</th><th>Name</th><th>Quantity</th></tr></thead>' +
             '<tbody>';

    if (string[1] === 'B') {
        type = "Bundle";
        BundleID = string[0];
        $.ajax({
            url: '/QuoteMaterialBundleList/GetBundleMappingMaterial?BundleID=' + BundleID + '&BudgetTargetRate=' + budgetTargetRate,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data.Redirect) {
                    window.location.replace(data.RedirectAction);
                }
                else {


                    for (var i = 0; i < data.Result.length; i++) {
                        sample += '<tr><td>' + data.Result[i].MaterialCode + '</td>' +
                                '<td>' + data.Result[i].MaterialName + '</td>' +
                                '<td>' + data.Result[i].Quantity + '</td></tr>';
                    }
                    sample += '</tbody></table>';
                }

            },
            error: function (e) {
            },
            async: false
        });
    }
    else {
        type = "Material";
        MaterialID = string[0];
    }




    //var EditStatus = $('#QuoteEditAutho').val();
    //if (EditStatus === "True") {
    $.ajax({
        url: '/QuoteMaterialBundleList/GetQuoteBasedMaterialBundleRecord?QuoteID=' + quoteID + '&MaterialID=' + MaterialID + '&BundleID=' + BundleID + '&Type=' + type,
        dataType: 'json',
        type: 'post',
        success: function (result) {
            var UnitPrice = null;
            var NetPrice = null;

            var oUnitPrice = '' + result.oQuoteBundleMaterial.UnitPrice;
            var oUnitPriceNew = oUnitPrice.split('.');
            if (typeof oUnitPriceNew[1] === 'undefined') {
                UnitPrice = result.oQuoteBundleMaterial.UnitPrice;
            }
            else {
                var oUnitPriceSecond = oUnitPriceNew[1].substring(0, 2);
                UnitPrice = oUnitPriceNew[0] + '.' + oUnitPriceSecond;
            }

            var oNetPrice = '' + result.oQuoteBundleMaterial.NetPrice;
            var oNetPriceNew = oNetPrice.split('.');
            if (typeof oNetPriceNew[1] === 'undefined') {
                NetPrice = result.oQuoteBundleMaterial.NetPrice;
            }
            else {
                var oNetPriceSecond = oNetPriceNew[1].substring(0, 2);
                NetPrice = oNetPriceNew[0] + '.' + oNetPriceSecond;
            }
            $('#RentalMaterialRecordImage').attr('src', 'http://172.17.0.147:8060/Content/Material/' + result.oQuoteBundleMaterial.Code + '/menu.jpg');
            $('#BundleMaterialRecordTitle').text(type);
            $('#BundleMaterialRecordId').val(result.oQuoteBundleMaterial.Code);
            $('#BundleMaterialRecordName').text(result.oQuoteBundleMaterial.Name);
            if (type === "Bundle") {
                $('#RentalMaterialRecordImage').css('display', 'none');
                $('#RentalMaterialRecordImage').after(sample);
            }
            else {
                $('#RentalMaterialRecordImage').css('display', 'block');
                $('#MaterialMappingBundleItem').remove();
            }
            $('#BundleMaterialRecordUnitprice').val(UnitPrice);
            $('#BundleMaterialRecordDiscount').val(result.oQuoteBundleMaterial.Discount);
            $('#BundleMaterialRecordQuantity').val(result.oQuoteBundleMaterial.Quantity);
            $('#BundleMaterialRecordNetprice').val(NetPrice);

            var popMargTop = ($('#BundleMaterialRecordPopup').height() + 24) / 2;
            var popMargLeft = ($('#BundleMaterialRecordPopup').width() + 24) / 2;

            $('#BundleMaterialRecordPopup').css({
                'margin-top': -popMargTop,
                'margin-left': -popMargLeft
            });

            // Add the mask to body
            $('body').append('<div id="mask"></div>');
            $('#mask').fadeIn(300);
            //Fade in the Popup
            $('#BundleMaterialRecordPopup').fadeIn(300);
        },
        error: function (error) {
        },
        async: false
    });
    //}
}

function QuoteBundleMaterialDeleteFuction(data) {
    debugger;
    var string = data.id.split('_');
    var QuoteID = $('#QuoteID').val();
    var Type = null;
    var MaterialID = null;
    var BundleID = null;
    if (string[1] === 'B') {
        BundleID = string[0];
        Type = 'Bundle';
    }
    else {
        MaterialID = string[0];
        Type = 'Material';
    }

    //var EditStatus = $('#QuoteEditAutho').val();
    //if (EditStatus === "True") {
    $.ajax({
        url: '/QuoteMaterialBundleList/DeleteMaterialBundleList?QuoteID=' + QuoteID + '&MaterialID=' + MaterialID + '&BundleID=' + BundleID + '&Type=' + Type,
        type: 'post',
        dataType: 'json',
        success: function (result) {
            if (result.result == 'success') {
                //$('#QuoteBundleMaterialDataTable').dataTable().fnClearTable();
                ReloadMaterialBundleDataTable();
                MaterialBundleTotalPrice();
                ReloadMaterialBundleWarrentyDataTable();
                //ReloadMaterialBundleOverrideWarrentyDataTable();
                //ReloadMaterialBundleInstallDataTable();
                //InstallTotalPrice();
                //TotalPriceTrimForInstall();
                GetQualitativeInformationTotalService();
            }
        },
        error: function (e) { },
        async: false
    });
    //}
}

function ReloadMaterialBundleDataTable() {
    var quoteID = $('#QuoteID').val();
    $('table').css('width', '100%');
    if (quoteID != '') {
        $('#QuoteBundleMaterialDataTable').dataTable({
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
            "sAjaxSource": "/QuoteMaterialBundleList/GetQuoteMaterialBundleList?QuoteID=" + quoteID,
            "bProcessing": true,
            "sPaginationType": "full_numbers",
            //"iDisplayLength": 1,
            "bDestroy": true,
            "aoColumns": [
                        {
                            "sTitle": "S No",
                            "sName": "Sno",
                            "bSearchable": false,
                            "bSortable": false
                        },
                        { "sTitle": "Code", "sName": "Code" },
                        { "sTitle": "Name", "sName": "Name" },
                        { "sTitle": "Description", "sName": "Description", "bSearchable": false, "bSortable": false },
                        { "sTitle": "Quantity", "sName": "Quantity", "sClass": "QuantityEditableClass" },
                        { "sTitle": "Unit Price", "sName": "UnitPrice" },
                        { "sTitle": "Total Gross Price", "sName": "GrossPrice" },
                        { "sTitle": "Discount", "sName": "Discount", "sClass": "DiscountEditableClass EditableFieldBgColor" },
                        { "sTitle": "Total Net Price", "sName": "NetPrice" },
                        { "sTitle": "Type", "sName": "Type" },
                        {
                            "sTitle": "Action",
                            "sName": "Code",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (obj) {
                                return '<a href="#" id=' + obj.aData[1] + '_' + obj.aData[9] + ' onclick="javascript:QuoteBundleMaterialEditFunction(this)" ><img src="/Content/images/edit.png" alt="EDIT" title="Edit" /></a><a href="#" id=' + obj.aData[1] + '_' + obj.aData[9] + ' onclick="javascript:QuoteBundleMaterialDeleteFuction(this)" style="float:left"><img alt="DELETE" src="/Content/images/error.png" title="Delete"></a>'
                            }
                        },
                        {
                            "sTitle": "Image",
                            "sName": "Code",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (obj) {
                                if (obj.aData[9] === "B") {
                                    return '';
                                }
                                else {
                                    var imgSrc = "http://172.17.0.147:8060/Content/Material/" + obj.aData[1] + "/icon.jpg";
                                    return '<img src="' + imgSrc + '"/>';
                                }
                            }
                        }

            ],
            "fnDrawCallback": function () {
                $("td.DiscountEditableClass").editable(function (e) {
                    var code = $(this).closest('tr').find('td:eq(1)').text();
                    var type = $(this).closest('tr').find('td:eq(9)').text();
                    var TypeOf;
                    var MaterialID = null;
                    var BundleID = null;

                    if (type === 'B') {
                        TypeOf = 'Bundle';
                        BundleID = code;
                    }
                    else {
                        TypeOf = 'Material';
                        MaterialID = code;
                    }

                    if (!/^[0-9]+$/.test(e)) {
                        $.noty.closeAll();
                        notyAlert = noty({
                            text: "Enter numbers only",
                            type: 'error',
                            buttons: [{
                                addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                                    $noty.close();
                                }
                            }]
                        });
                        return this.revert;
                    }
                    else {
                        if (e.length >= 3) {
                            $.noty.closeAll();
                            notyAlert = noty({
                                text: "Over discount",
                                type: 'error',
                                buttons: [{
                                    addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                                        $noty.close();
                                    }
                                }]
                            });                            
                            return this.revert;
                        }
                        else {
                            $.ajax({
                                url: '/QuoteMaterialBundleList/UpdateMaterialBundleDiscount?QuoteID=' + quoteID + '&MaterialID=' + MaterialID + '&BundleID=' + BundleID + '&Type=' + TypeOf + '&Discount=' + e,
                                type: 'post',
                                dataType: 'json',
                                success: function (data) {
                                    if (data.result === 'success') {
                                        ReloadMaterialBundleDataTable();
                                        MaterialBundleTotalPrice();
                                        TotalUnitNetPriceTrimForMaterialBundle();
                                    }
                                },
                                error: function (e) {
                                },
                                async: false
                            });                            
                            return e + '%';
                        }
                    }
                }, {
                    data: function (value, settings) {
                        return value.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '');                        
                    },
                    tooltip: 'Click to edit',
                    type: 'text',
                });

            }
        });

        //$("#QuoteBundleMaterialDataTable").qtip({
        //    content: 'Discount Column Editable',
        //    style: {
        //        width: 200,
        //        padding: 5,
        //        background: '#1971B5',
        //        color: '#FFFFFF',
        //        textAlign: 'center',
        //        border: {
        //            width: 7,
        //            radius: 5,
        //            color: '#1971B5'
        //        },
        //        tip: 'bottomLeft',
        //        name: 'dark' // Inherit the rest of the attributes from the preset dark style
        //    },
        //    position: {
        //        corner: {
        //            target: 'topLeft',
        //            tooltip: 'bottomLeft'
        //        }
        //    },
        //    show:'mouseover',
        //    //show: {
        //    //    when: false, // Don't specify a show event
        //    //    ready: true // Show the tooltip when ready
        //    //},
        //    //hide: { when: { event: 'click' } }
        //    //hide:false
        //    hide:'mouseout'
        //});


    }

}

function MaterialViewFunction(data) {
    var budgetTargetRate = $('#BudgetValue').val();
    $('#BundleMaterialPopup').css('display', 'none');
    $.ajax({
        url: '/QuoteMaterialBundleList/GetMaterialDetail?MaterialID=' + data.id + '&BudgetTargetRate=' + budgetTargetRate,
        type: 'post',
        dataType: 'json',
        success: function (result) {
            var Amount = null;

            var oAmount = '' + result.mMaterialProperty.MaterialAmount;
            var oAmountNew = oAmount.split('.');
            if (typeof oAmountNew[1] === 'undefined') {
                Amount = result.mMaterialProperty.MaterialAmount;
            }
            else {
                var oAmountSecond = oAmountNew[1].substring(0, 2);
                Amount = oAmountNew[0] + '.' + oAmountSecond;
            }

            $('#MaterialName').text(result.mMaterialProperty.MaterialName);
            $('#MaterialImage').attr('src', 'http://172.17.0.147:8060/Content/Material/' + data.id + '/menu.jpg');
            $('#MaterialDiscount').val(result.mMaterialProperty.MaterialDiscount);
            $('#MaterialAmount').val(Amount);
            $('#MaterialDescription').text(result.mMaterialProperty.MaterialDescription);

            if ($('#MaterialDescription').text() === 'null') {
                $('#MaterialDescription').text('');
            }

            var popMargTop = ($('#MaterialRecordPopup').height() + 24) / 2;
            var popMargLeft = ($('#MaterialRecordPopup').width() + 24) / 2;

            $('#MaterialRecordPopup').css({
                'margin-top': -popMargTop,
                'margin-left': -popMargLeft
            });

            //Fade in the Popup
            $('#MaterialRecordPopup').fadeIn(300);

        },
        error: function (e) { },
        async: false
    });
}

function MaterialAddFunction(data) {
    if ($(data).prop('checked') === true) {
        $(data).closest('tr').find('td:eq(5)').editable('enable');
        $(data).closest('tr').find('td:eq(5)').addClass('EditableFieldBgColor');
        var category = $(data).parents('table').attr('id').split('_')[1];
        $('#GuidCategoryName').val(category);
        $(data).closest('tr').find('td:eq(9) a').text('OFFER');
        $(data).closest('tr').find('td:eq(5)').editable(function (e) {
            var Code = $(data).closest('tr').find('td:eq(0)').text();
            if (!/^[0-9]+$/.test(e)) {
                $.noty.closeAll();
                notyAlert = noty({
                    text: "Enter numbers only",
                    type: 'error',
                    buttons: [{
                        addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                            $noty.close();
                        }
                    }]
                });                
                return this.revert;
            }
            else {
                if (MaterialIDQuantityList.length > 0) {
                    $.each(MaterialIDQuantityList, function (i) {
                        if (MaterialIDQuantityList[i].ID === Code) {
                            MaterialIDQuantityList[i].Quantity = e;
                        }
                    });
                }
                else {
                    MaterialIDQuantityList.push({ ID: Code, Quantity: e });
                }                
                return e;
            }
        }, {
            data: function (value, settings) {
                //return value.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '');
                return '';
            },
            tooltip: 'Click to edit',
            type: 'text',
        });

        if (!$.inArray(data.value, MaterialIDList) > -1) {
            MaterialIDList.push(data.value);
        }

        if (MaterialIDQuantityList.length > 0) {
            $.each(MaterialIDQuantityList, function (i) {
                if (MaterialIDQuantityList[i].ID !== data.value) {
                    MaterialIDQuantityList.push({ ID: data.value, Quantity: '1' });
                    return false;
                }
            });
        }
        else {
            MaterialIDQuantityList.push({ ID: data.value, Quantity: '1' });
            return false;
        }
    }
    else {
        $(data).closest('tr').find('td:eq(5)').editable('disable');
        $(data).closest('tr').find('td:eq(5)').removeClass('EditableFieldBgColor');
        $('#GuidCategoryName').val('');
        $(data).closest('tr').find('td:eq(9) a').text('');
        $(data).closest('tr').find('td:eq(5)').text('1');

        if ($.inArray(data.value, MaterialIDList) > -1) {
            removeA(MaterialIDList, data.value);
        }

        if (MaterialIDQuantityList.length > 0) {
            $.each(MaterialIDQuantityList, function (i) {
                if (MaterialIDQuantityList[i].ID === data.value) {
                    MaterialIDQuantityList.splice(i, 1);
                    return false;
                }
            });
        }
    }
}

function BundleViewFunction(data) {
    var budgetTargetRate = $('#BudgetValue').val();
    $('#BundleMaterialPopup').css('display', 'none');
    $.ajax({
        url: '/QuoteMaterialBundleList/GetBundleDetails?BundleID=' + data.id + '&BudgetTargetRate=' + budgetTargetRate,
        type: 'post',
        dataType: 'json',
        success: function (result) {
            var UnitPrice = null;
            var NetPrice = null;

            var oUnitPrice = '' + result.bBundleProperty.BundleUnitPrice;
            var oUnitPriceNew = oUnitPrice.split('.');
            if (typeof oUnitPriceNew[1] === 'undefined') {
                UnitPrice = result.bBundleProperty.BundleUnitPrice;
            }
            else {
                var oUnitPriceSecond = oUnitPriceNew[1].substring(0, 2);
                UnitPrice = oUnitPriceNew[0] + '.' + oUnitPriceSecond;
            }

            var oNetPrice = '' + result.bBundleProperty.BundleNetPrice;
            var oNetPriceNew = oNetPrice.split('.');
            if (typeof oNetPriceNew[1] === 'undefined') {
                NetPrice = result.bBundleProperty.BundleNetPrice;
            }
            else {
                var oNetPriceSecond = oNetPriceNew[1].substring(0, 2);
                NetPrice = oNetPriceNew[0] + '.' + oNetPriceSecond;
            }

            $('#BundleName').text(result.bBundleProperty.BundleName);
            $('#BundleImage').attr('src', data.id);
            $('#BundleUnitprice').val(UnitPrice);
            $('#BundleDiscount').val(result.bBundleProperty.BundleDiscount);
            $('#BundleNetprice').val(NetPrice);
            $('#BundleDescription').text(result.bBundleProperty.BundleDescription);

            if ($('#BundleDescription').text() === 'null') {
                $('#BundleDescription').text('');
            }

            var popMargTop = ($('#BundleRecordPopup').height() + 24) / 2;
            var popMargLeft = ($('#BundleRecordPopup').width() + 24) / 2;

            $('#BundleRecordPopup').css({
                'margin-top': -popMargTop,
                'margin-left': -popMargLeft
            });

            //Fade in the Popup
            $('#BundleRecordPopup').fadeIn(300);

        },
        error: function (e) { },
        async: false
    });
}

function BundleAddFunction(data) {
    if ($(data).prop('checked') === true) {

        $(data).closest('tr').find('td:eq(5)').editable('enable');
        $(data).closest('tr').find('td:eq(5)').addClass('EditableFieldBgColor');
        $(data).closest('tr').find('td:eq(5)').editable(function (e) {
            var Code = $(data).closest('tr').find('td:eq(1)').text();
            if (!/^[0-9]+$/.test(e)) {
                $.noty.closeAll();
                notyAlert = noty({
                    text: "Enter numbers only",
                    type: 'error',
                    buttons: [{
                        addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                            $noty.close();
                        }
                    }]
                });                
                return this.revert;
            }
            else {
                if (BundleIDQuantityList.length > 0) {
                    $.each(BundleIDQuantityList, function (i) {
                        if (BundleIDQuantityList[i].ID === Code) {
                            BundleIDQuantityList[i].Quantity = e;
                        }
                    });
                }
                else {
                    BundleIDQuantityList.push({ ID: Code, Quantity: e });
                }                
                return e;
            }
        }, {
            data: function (value, settings) {                
                return '';
            },
            tooltip: 'Click to edit',
            type: 'text',
        });

        if (BundleIDQuantityList.length > 0) {
            $.each(BundleIDQuantityList, function (i) {
                if (BundleIDQuantityList[i].ID !== data.value) {
                    BundleIDQuantityList.push({ ID: data.value, Quantity: '1' });
                    return false;
                }
            });
        }
        else {
            BundleIDQuantityList.push({ ID: data.value, Quantity: '1' });
            return false;
        }


        if (!$.inArray(data.value, BundleIDList) > -1) {
            BundleIDList.push(data.value);
        }
    }
    else {

        $(data).closest('tr').find('td:eq(5)').editable('disable');
        $(data).closest('tr').find('td:eq(5)').removeClass('EditableFieldBgColor');
        $(data).closest('tr').find('td:eq(5)').text('1');

        if (BundleIDQuantityList.length > 0) {
            $.each(BundleIDQuantityList, function (i) {
                if (BundleIDQuantityList[i].ID === data.value) {
                    BundleIDQuantityList.splice(i, 1);
                    return false;
                }
            });
        }

        if ($.inArray(data.value, BundleIDList) > -1) {
            removeA(BundleIDList, data.value);
        }
    }
}

function MaterialBundleTotalPrice() {
    var QuoteID = $('#QuoteID').val();
    $.ajax({
        url: '/QuoteMaterialBundleList/GetTotalQuoteMaterialBundlePrice?QuoteID=' + QuoteID,
        type: 'post',
        dataType: 'json',
        success: function (result) {
            if (result.oTotalUnitDiscount != '') {
                $('#MaterialBundleTotalUnitPrice').val(result.oTotalUnitDiscount.TotalUnitPrice);
                $('#MaterialBundleTotalDiscount').val(result.oTotalUnitDiscount.TotalDiscount);
                $('#MaterialBundleTotalNetPrice').val(result.oTotalUnitDiscount.TotalNetPrice);
                $('#MaterialBundleTotalGrossPrice').val(result.oTotalUnitDiscount.TotalGrossPrice);
            }
        },
        error: function (e) { },
        async: false
    });
}

function TotalUnitNetPriceTrimForMaterialBundle() {
    //Net Price trim method

    var oNetPrice = $('#MaterialBundleTotalNetPrice').val();
    $('#MaterialBundleTotalNetPrice').val(oNetPrice);
    //$('#MaterialBundleTotalNetPrice').val(formatCurrency(oNetPrice));
    //var oNetPriceNew = oNetPrice.split('.');
    //if (typeof oNetPriceNew[1] === 'undefined') {
    //    $('#MaterialBundleTotalNetPrice').val(oNetPrice);
    //}
    //else {
    //    var oNetPriceSecond = oNetPriceNew[0] + '.' + oNetPriceNew[1].substring(0, 2);
    //    $('#MaterialBundleTotalNetPrice').val(oNetPriceSecond);
    //}

    //Unit Price trim method

    //var oUnitPrice = $('#MaterialBundleTotalUnitPrice').val();
    //var oUnitPriceNew = oUnitPrice.split('.');
    //if (typeof oUnitPriceNew[1] === 'undefined') {
    //    $('#MaterialBundleTotalUnitPrice').val(oUnitPrice);
    //}
    //else {
    //    var oUnitPriceSecond = oUnitPriceNew[0] + '.' + oUnitPriceNew[1].substring(0, 2);
    //    $('#MaterialBundleTotalUnitPrice').val(oUnitPriceSecond);
    //}

    //Gross price trim method

    var oGrossPrice = $('#MaterialBundleTotalGrossPrice').val();
    $('#MaterialBundleTotalGrossPrice').val(oGrossPrice);
    //$('#MaterialBundleTotalGrossPrice').val(formatCurrency(oGrossPrice));
    //var oGrossPriceNew = oGrossPrice.split('.');
    //if (typeof oGrossPriceNew[1] === 'undefined') {
    //    $('#MaterialBundleTotalGrossPrice').val(oGrossPrice);
    //}
    //else {
    //    var oGrossPriceSecond = oGrossPriceNew[0] + '.' + oGrossPriceNew[1].substring(0, 2);
    //    $('#MaterialBundleTotalGrossPrice').val(oGrossPriceSecond);
    //}

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

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{1,}))$/;
    return re.test(email);
}

function loadFunctionSugectionOffered(MaterialId) {
    $('#DynamicSugectionMaterialDataTable').html('');
    $('#DynamicOfferedMaterialDataTable').html('');

    /*Material Start*/
    $('table').css('width', '100%');
    /*Dynamic Accordion*/
    $('#DynamicSugectionMaterialDataTable').append('<div class="quote_list"><table cellpadding="0" cellspacing="0" border="0" class="display" id="SugectionMaterialDataTable_table"></table></div>');
    /*End*/

    /*Material Data table*/

    $('#SugectionMaterialDataTable_table').dataTable({
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
        "sAjaxSource": "/QuoteMaterialBundleList/GetSugectionMaterialList?MaterialId=" + MaterialId,
        "bProcessing": true,
        "sPaginationType": "full_numbers",
        //"iDisplayLength": 1,
        "aoColumns": [
                    { "sName": "MaterialId", "bSearchable": false, "bSortable": false, "sClass": "DisplayNone" },

                    { "sTitle": "Material Code", "sName": "MaterialCode" },
                    { "sTitle": "Name", "sName": "MaterialName" },
                    { "sTitle": "Discount", "sName": "MaterialDiscount", "sClass": "DisplayNone" },
                    { "sTitle": "Cost", "sName": "MaterialAmount" },
                    { "sTitle": "Quantity", "sName": "Quantity" },
                    {
                        "sName": "MaterialId",
                        "bSearchable": false,
                        "bSortable": false,
                        "fnRender": function (obj) {
                            return '<input type="checkbox" value=' + obj.aData[0] + ' onclick="javascript:SugectionMaterialAddFunction(this)"/>'
                        }
                    },
                    {
                        "sName": "Code",
                        "bSearchable": false,
                        "bSortable": false,
                        "fnRender": function (obj) {
                            var imgSrc = "http://172.17.0.147:8060/Content/Material/" + obj.aData[0] + "/icon.jpg";
                            return '<img src="' + imgSrc + '"/>';
                        }
                    }

        ]
    });

    /*Material data table end*/

    /*Dynamic accordion h3 and div add*/
    $('#DynamicOfferedMaterialDataTable').append('<div class="quote_list"><table cellpadding="0" cellspacing="0" border="0" class="display" id="OfferedMaterialDataTable_table"></table></div>');
    /*End*/


    $('#OfferedMaterialDataTable_table').dataTable({
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
        "sAjaxSource": "/QuoteMaterialBundleList/GetSugectionMaterialList?MaterialId=" + MaterialId,
        "bProcessing": true,
        "sPaginationType": "full_numbers",
        //"iDisplayLength": 1,
        "aoColumns": [
                    { "sName": "MaterialId", "bSearchable": false, "bSortable": false, "sClass": "DisplayNone" },

                    { "sTitle": "Material Code", "sName": "MaterialCode" },
                    { "sTitle": "Name", "sName": "MaterialName" },
                    { "sTitle": "Discount", "sName": "MaterialDiscount", "sClass": "DisplayNone" },
                    { "sTitle": "Cost", "sName": "MaterialAmount" },
                    { "sTitle": "Quantity", "sName": "Quantity" },
                    {
                        "sName": "MaterialId",
                        "bSearchable": false,
                        "bSortable": false,
                        "fnRender": function (obj) {
                            return '<input type="checkbox" value=' + obj.aData[0] + ' onclick="javascript:OfferedMaterialAddFunction(this)"/>'
                        }
                    },
                    {
                        "sName": "Code",
                        "bSearchable": false,
                        "bSortable": false,
                        "fnRender": function (obj) {
                            var imgSrc = "http://172.17.0.147:8060/Content/Material/" + obj.aData[0] + "/icon.jpg";
                            return '<img src="' + imgSrc + '"/>';
                        }
                    }

        ]
    });


}

function GuidMaterial(data) {
    $('#Dynamic_guidMaterial').html('');
    loadFunctionSugectionOffered(data.id);
    $('#SugectionOfferedMaterialId').val(data.id);
    LoadGuidMaterial('', '', null, null);
    $('#BundleMaterialPopup').css('display', 'none');
    var popMargTop = ($('#SugectionOfferedPopup').height() + 24) / 2;
    var popMargLeft = ($('#SugectionOfferedPopup').width() + 24) / 2;

    $('#SugectionOfferedPopup').find('input[type=checkbox]:checked').removeAttr('checked');

    $('#SugectionOfferedPopup').css({
        //'margin-top': -popMargTop,
        'margin-top': '-270px',
        'margin-left': -popMargLeft
    });

    // Add the mask to body
    $('body').append('<div id="mask"></div>');
    $('#mask').fadeIn(300);
    //Fade in the Popup
    $('#SugectionOfferedPopup').fadeIn(300);
}

function SugectionMaterialAddFunction(data) {
    var matId = $('#SugectionOfferedMaterialId').val();
    var $inputs = $('#DynamicSugectionMaterialDataTable input[type="checkbox"]');
    if ($(data).prop('checked') === true) {
        $inputs.not(data).prop('disabled', true);
        $(data).closest('tr').find('td:eq(5)').editable('enable');
        $(data).closest('tr').find('td:eq(5)').addClass('EditableFieldBgColor');
        $(data).closest('tr').find('td:eq(5)').editable(function (e) {
            var Code = $(data).closest('tr').find('td:eq(0)').text();
            if (!/^[0-9]+$/.test(e)) {
                $.noty.closeAll();
                notyAlert = noty({
                    text: "Enter numbers only",
                    type: 'error',
                    buttons: [{
                        addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                            $noty.close();
                        }
                    }]
                });                
                return this.revert;
            }
            else {
                if (SugectionMaterialID.length > 0) {
                    $.each(SugectionMaterialID, function (i) {
                        if (SugectionMaterialID[i].ID === Code) {
                            SugectionMaterialID[i].Quantity = e;
                        }
                    });
                }
                else {
                    SugectionMaterialID.push({ MaterialId: matId, ID: Code, Quantity: e });
                }                
                return e;
            }
        }, {
            data: function (value, settings) {                
                return '';
            },
            tooltip: 'Click to edit',
            type: 'text',
        });

        if (SugectionMaterialID.length > 0) {
            $.each(SugectionMaterialID, function (i) {
                if (SugectionMaterialID[i].ID !== data.value) {
                    SugectionMaterialID.push({ MaterialId: matId, ID: data.value, Quantity: '1' });
                    return false;
                }
            });
        }
        else {
            SugectionMaterialID.push({ MaterialId: matId, ID: data.value, Quantity: '1' });
            return false;
        }
    }
    else {
        $inputs.prop('disabled', false);
        $(data).closest('tr').find('td:eq(5)').editable('disable');
        $(data).closest('tr').find('td:eq(5)').removeClass('EditableFieldBgColor');
        $(data).closest('tr').find('td:eq(9) a').text('');
        $(data).closest('tr').find('td:eq(5)').text('1');

        if (SugectionMaterialID.length > 0) {
            $.each(SugectionMaterialID, function (i) {
                if (SugectionMaterialID[i].ID === data.value) {
                    SugectionMaterialID.splice(i, 1);
                    return false;
                }
            });
        }
    }    
}

function OfferedMaterialAddFunction(data) {
    var matId = $('#SugectionOfferedMaterialId').val();
    //var $inputs = $('#DynamicOfferedMaterialDataTable input[type="checkbox"]');

    if ($(data).prop('checked') === true) {
        //$inputs.not(data).prop('disabled', true);
        $(data).closest('tr').find('td:eq(5)').editable('enable');
        $(data).closest('tr').find('td:eq(5)').addClass('EditableFieldBgColor');
        $(data).closest('tr').find('td:eq(5)').editable(function (e) {
            var Code = $(data).closest('tr').find('td:eq(0)').text();
            if (!/^[0-9]+$/.test(e)) {
                $.noty.closeAll();
                notyAlert = noty({
                    text: "Enter numbers only",
                    type: 'error',
                    buttons: [{
                        addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                            $noty.close();
                        }
                    }]
                });                
                return this.revert;
            }
            else {
                if (OfferedMaterialID.length > 0) {
                    $.each(OfferedMaterialID, function (i) {
                        if (OfferedMaterialID[i].ID === Code) {
                            OfferedMaterialID[i].Quantity = e;
                        }
                    });
                }
                else {
                    OfferedMaterialID.push({ MaterialId: matId, ID: Code, Quantity: e });
                }
                return e;
            }
        }, {
            data: function (value, settings) {
                return '';
            },
            tooltip: 'Click to edit',
            type: 'text',
        });

        if (OfferedMaterialID.length > 0) {
            $.each(OfferedMaterialID, function (i) {
                if (OfferedMaterialID[i].ID !== data.value) {
                    OfferedMaterialID.push({ MaterialId: matId, ID: data.value, Quantity: '1' });
                    return false;
                }
            });
        }
        else {
            OfferedMaterialID.push({ MaterialId: matId, ID: data.value, Quantity: '1' });
            return false;
        }
    }
    else {
        //$inputs.prop('disabled', false);
        $(data).closest('tr').find('td:eq(5)').editable('disable');
        $(data).closest('tr').find('td:eq(5)').removeClass('EditableFieldBgColor');
        $(data).closest('tr').find('td:eq(9) a').text('');
        $(data).closest('tr').find('td:eq(5)').text('1');

        if (OfferedMaterialID.length > 0) {
            $.each(OfferedMaterialID, function (i) {
                if (OfferedMaterialID[i].ID === data.value) {
                    OfferedMaterialID.splice(i, 1);
                    return false;
                }
            });
        }
    }
}

function DropDownShow() {
    $('#Dynamic_guidMaterial').html('');
    $('#BenweenOpr').css('display', 'block');
    resetSlider();
    LoadGuidMaterial('', '', null, null);
}

function MinValueMaterial() {
    $('#Dynamic_guidMaterial').html('');
    $('#BenweenOpr').css('display', 'none');
    resetSlider();
    LoadGuidMaterial('MIN', '', null, null);
}

function MaxValueMaterial() {
    $('#Dynamic_guidMaterial').html('');
    $('#BenweenOpr').css('display', 'none');
    resetSlider();
    LoadGuidMaterial('', 'MAX', null, null);
}

function LoadGuidMaterial(Min, Max, MinValue, MaxValue) {
    var Category = $('#GuidCategoryName').val();

    $('#Dynamic_guidMaterial').append('<div class="quote_list"><table cellpadding="0" cellspacing="0" border="0" class="display" id="Material_GuidTable"></table></div>');
    /*End*/

    /*Material Data table*/

    $('#Material_GuidTable').dataTable({
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
        "sAjaxSource": "/QuoteMaterialBundleList/GetMaterialListGuid?MinValue=" + Min + "&MaxValue=" + Max + "&minAmount=" + MinValue + "&maxAmount=" + MaxValue + "&Category=" + Category + "&BudgetTargetRate=" + budgetTargetRate,
        "bProcessing": true,
        "sPaginationType": "full_numbers",
        //"iDisplayLength": 1,
        "aoColumns": [
                    { "sName": "MaterialId", "bSearchable": false, "bSortable": false, "sClass": "DisplayNone" },

                    { "sTitle": "Material Code", "sName": "MaterialCode" },
                    { "sTitle": "Name", "sName": "MaterialName" },
                    { "sTitle": "Discount", "sName": "MaterialDiscount" },
                    { "sTitle": "Cost", "sName": "MaterialAmount" },
                    { "sTitle": "Quantity", "sName": "Quantity" },
                    //{
                    //    "sTitle": "View",
                    //    "sName": "MaterialId",
                    //    "bSearchable": false,
                    //    "bSortable": false,
                    //    "fnRender": function (obj) {
                    //        return '<a href="#" id=' + obj.aData[0] + ' onclick="javascript:MaterialViewFunction(this)" style="float:left"><img alt="EDIT" src="/Content/images/edit.png" title="Edit"></a>'
                    //    }
                    //},
                     {
                         "sName": "MaterialId",
                         "bSearchable": false,
                         "bSortable": false,
                         "fnRender": function (obj) {
                             return '<input type="checkbox" value=' + obj.aData[0] + ' onclick="javascript:GuidMaterialAddFunction(this)"/>'
                         }
                     },

                    {
                        "sName": "Code",
                        "bSearchable": false,
                        "bSortable": false,
                        "fnRender": function (obj) {
                            var imgSrc = "http://172.17.0.147:8060/Content/Material/" + obj.aData[0] + "/icon.jpg";
                            return '<img src="' + imgSrc + '"/>';
                        }
                    }

        ]
    });

}

function resetSlider() {
    var $slider = $("#slider-range");
    $slider.slider("values", 0, minimumAmount);
    $slider.slider("values", 1, maximumAmount);
}

function GuidMaterialAddFunction(data) {
    if ($(data).prop('checked') === true) {

        $(data).closest('tr').find('td:eq(5)').editable('enable');
        $(data).closest('tr').find('td:eq(5)').addClass('EditableFieldBgColor');

        $(data).closest('tr').find('td:eq(5)').editable(function (e) {
            var Code = $(data).closest('tr').find('td:eq(0)').text();
            if (!/^[0-9]+$/.test(e)) {
                $.noty.closeAll();
                notyAlert = noty({
                    text: "Enter numbers only",
                    type: 'error',
                    buttons: [{
                        addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                            $noty.close();
                        }
                    }]
                });
                return this.revert;
            }
            else {
                if (GuidMaterialIDQuantityList.length > 0) {
                    $.each(GuidMaterialIDQuantityList, function (i) {
                        if (GuidMaterialIDQuantityList[i].ID === Code) {
                            GuidMaterialIDQuantityList[i].Quantity = e;
                        }
                    });
                }
                else {
                    GuidMaterialIDQuantityList.push({ ID: Code, Quantity: e });
                }
                return e;
            }
        }, {
            data: function (value, settings) {
                return '';
            },
            tooltip: 'Click to edit',
            type: 'text',
        });

        if (GuidMaterialIDQuantityList.length > 0) {
            $.each(GuidMaterialIDQuantityList, function (i) {
                if (GuidMaterialIDQuantityList[i].ID !== data.value) {
                    GuidMaterialIDQuantityList.push({ ID: data.value, Quantity: '1' });
                    return false;
                }
            });
        }
        else {
            GuidMaterialIDQuantityList.push({ ID: data.value, Quantity: '1' });
            return false;
        }
    }
    else {
        $(data).closest('tr').find('td:eq(5)').editable('disable');
        $(data).closest('tr').find('td:eq(5)').removeClass('EditableFieldBgColor');
        $(data).closest('tr').find('td:eq(5)').text('1');

        if (GuidMaterialIDQuantityList.length > 0) {
            $.each(GuidMaterialIDQuantityList, function (i) {
                if (GuidMaterialIDQuantityList[i].ID === data.value) {
                    GuidMaterialIDQuantityList.splice(i, 1);
                    return false;
                }
            });
        }
    }
}