
    $(document).ready(function () {
        GetInstallDetails();
        $('#AddInstallationDetails').click(function () {
            var popMargTop = ($('#AddInstallPopup').height() + 24) / 2;
            var popMargLeft = ($('#AddInstallPopup').width() + 24) / 2;

            $('#AddInstallPopup').css({
                'margin-top': -popMargTop,
                'margin-left': -popMargLeft
            });

            // Add the mask to body
            $('body').append('<div id="mask"></div>');
            $('#mask').fadeIn(300);
            //Fade in the Popup
            $('#AddInstallPopup').fadeIn(300);
        });

        $('#AddInstallPopup_close').click(function () {
            $('#mask').fadeOut(300);
            $('#AddInstallPopup').fadeOut(300);
        });

        $('#NoEmployees').keyup(function (e) {
            if (!/^[0-9]+$/.test($(this).val())) {
                if ($(this).val().length != 0) {
                    $(this).val($(this).val().slice(0, -1));
                    $('#PerDayCose').val('');
                    $('#NoDays').val('');
                    $('#TotalCost').val('');
                    $('#PerDayCose').attr('disabled', 'disabled');
                    $('#NoDays').attr('disabled', 'disabled');
                }
            }
            else {
                $('#PerDayCose').removeAttr('disabled', 'disabled');
                if ($('#PerDayCose').val() !== '' && $('#NoDays').val() !== '') {
                    var noEmp = $(this).val();
                    var perDayCost = $('#PerDayCose').val();
                    var noDay = $('#NoDays').val();
                    var totalCost = parseInt(noEmp) * parseInt(perDayCost) * parseInt(noDay);
                    $('#TotalCost').val(totalCost);
                }
            }
        });

        $('#PerDayCose').keyup(function (e) {
            if (!/^[0-9]+$/.test($(this).val())) {
                if ($(this).val().length != 0) {
                    $(this).val($(this).val().slice(0, -1));
                    $('#NoDays').val('');
                    $('#TotalCost').val('');
                    $('#NoDays').attr('disabled', 'disabled');
                }
            }
            else {
                $('#NoDays').removeAttr('disabled', 'disabled');
                if ($('#NoEmployees').val() !== '' && $('#NoDays').val() !== '') {
                    var noEmp = $('#NoEmployees').val();
                    var perDayCost = $(this).val();
                    var noDay = $('#NoDays').val();
                    var totalCost = parseInt(noEmp) * parseInt(perDayCost) * parseInt(noDay);
                    $('#TotalCost').val(totalCost);
                }
            }
        });

        $('#NoDays').keyup(function (e) {
            if (!/^[0-9]+$/.test($(this).val())) {
                if ($(this).val().length != 0) {
                    $(this).val($(this).val().slice(0, -1));
                }
            }
            else {
                var noEmp = $('#NoEmployees').val();
                var perDayCost = $('#PerDayCose').val();
                var noDay = $(this).val();
                var totalCost = parseInt(noEmp) * parseInt(perDayCost) * parseInt(noDay);
                $('#TotalCost').val(totalCost);
            }
        });

        $('#InsertInstallationDetails').click(function () {            
            if ($('#NoEmployees').val() !== '' && $('#NoDays').val() !== '' && $('#PerDayCose').val() !== '' && $('#TotalCost').val() !== '') {
                var noEmp = $('#NoEmployees').val();
                var perDayCost = $('#PerDayCose').val();
                var noDay = $('#NoDays').val();
                var totalCost = $('#TotalCost').val();
                var QuoteID = $('#QuoteID').val();
                $.ajax({
                    url: '/QuoteInstall/InsertQuoteInstallationDetails?NoEmp=' + noEmp + '&PerDayCost=' + perDayCost + '&NoDays=' + noDay + '&TotalCost=' + totalCost + '&QuoteID=' + QuoteID,
                    type: 'post',
                    dataType: 'json',
                    success: function (data) {
                        if (data.Redirect) {
                            window.location.replace(data.RedirectAction);
                        }
                        else {
                            if (data.result === 'success') {
                                GetInstallDetails();
                                $('#mask').fadeOut(300);
                                $('#AddInstallPopup').fadeOut(300);
                            }
                        }
                    },
                    error: function (e) {
                    },
                    async: false
                });
            }
            else {
                if ($('#NoEmployees').val() === '' && $('#NoDays').val() === '' && $('#PerDayCose').val() === '') {
                    $('#NoEmpError').text('*');
                    $('#PerDayCostError').text('*');
                    $('#NoDayError').text('*');
                }
                else if ($('#NoEmployees').val() !== '' && $('#NoDays').val() !== '' && $('#PerDayCose').val() === '') {
                    $('#NoEmpError').text('');
                    $('#PerDayCostError').text('*');
                    $('#NoDayError').text('');
                }
                else if ($('#NoEmployees').val() !== '' && $('#NoDays').val() === '' && $('#PerDayCose').val() !== '') {
                    $('#NoEmpError').text('');
                    $('#PerDayCostError').text('');
                    $('#NoDayError').text('*');
                }
                else if ($('#NoEmployees').val() === '' && $('#NoDays').val() !== '' && $('#PerDayCose').val() !== '') {
                    $('#NoEmpError').text('*');
                    $('#PerDayCostError').text('');
                    $('#NoDayError').text('');
                }
                else if ($('#NoEmployees').val() !== '' && $('#NoDays').val() === '' && $('#PerDayCose').val() === '') {
                    $('#NoEmpError').text('');
                    $('#PerDayCostError').text('*');
                    $('#NoDayError').text('*');
                }
                else if ($('#NoEmployees').val() === '' && $('#NoDays').val() !== '' && $('#PerDayCose').val() === '') {
                    $('#NoEmpError').text('*');
                    $('#PerDayCostError').text('*');
                    $('#NoDayError').text('');
                }
                else if ($('#NoEmployees').val() === '' && $('#NoDays').val() === '' && $('#PerDayCose').val() !== '') {
                    $('#NoEmpError').text('');
                    $('#PerDayCostError').text('*');
                    $('#NoDayError').text('*');
                }
                else {
                    $('#NoEmpError').text('');
                    $('#PerDayCostError').text('');
                    $('#NoDayError').text('');
                }                
            }
            return false;
        });
    });

function GetInstallDetails() {
    var QuoteID = $('#QuoteID').val();
    $.ajax({
        url: '/QuoteInstall/GetQuoteInstallationDetails?QuoteID=' + QuoteID,
        type: 'post',
        dataType: 'json',
        success: function (data) {
            if (data.Redirect) {
                window.location.replace(data.RedirectAction);
            }
            else {
                if (data.result != null) {
                    if (data.result.NoEmployees == 0 && data.result.CostPerDay == null && data.result.NoDays == 0 && data.result.TotalCost == null) {
                        $('#InstallDetailsTable tbody tr').addClass('odd');
                        $('#InstallDetailsTable tbody tr').append('<td colspan="4" style="font-weight:bold;">No data available in table</td>');
                    }
                    else {
                        $('#InstallDetailsTable tbody tr td').remove();
                        $('#InstallDetailsTable tbody tr').addClass('odd');
                        $('#InstallDetailsTable tbody tr').append('<td>' + data.result.NoEmployees + '</td>');
                        $('#InstallDetailsTable tbody tr').append('<td>' + data.result.CostPerDay + '</td>');
                        $('#InstallDetailsTable tbody tr').append('<td>' + data.result.NoDays + '</td>');
                        $('#InstallDetailsTable tbody tr').append('<td>' + data.result.TotalCost + '</td>');
                        $('#AddInstallationDetails').css('display', 'none');
                    }
                }
                else {
                    $('#InstallDetailsTable').css('display', 'none');
                }
            }
        },
        error: function (e) {
        },
        async: false
    });
}
