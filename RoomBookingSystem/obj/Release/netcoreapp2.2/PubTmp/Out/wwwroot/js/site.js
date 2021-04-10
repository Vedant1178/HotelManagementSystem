jQuery(document).ready(function ($) {
    $('#dtCheckIn').change(function () {
        var checkindate = new Date($(this).val());
        var checkoutdate = new Date($('#dtCheckOut').val());
        var diff = new Date(checkoutdate - checkindate);
        // get days
        var days = diff / 1000 / 60 / 60 / 24;
        if (parseInt(days) > 0) {
            var rent = parseInt($("#hiddenPricePerDay").val());
            rent * days;
            $("#lblBookingAmount").text(rent * days);
            $("#hiddenBillAmount").val(rent * days);
            $(':input[type="submit"]').prop('disabled', false);
        }
        else
        {
            $("#hiddenBillAmount").val(0);
            $(':input[type="submit"]').prop('disabled', true);
        }
    });
    $('#dtCheckOut').change(function ()
    {
        var checkoutdate = new Date($(this).val());
        var checkindate = new Date($('#dtCheckIn').val());
        var diff = new Date(checkoutdate - checkindate);
        var days = diff / 1000 / 60 / 60 / 24;
        if (parseInt(days) > 0)
        {
            var rent = parseInt($("#hiddenPricePerDay").val());
            $("#lblBookingAmount").text(rent * days);
            $("#hiddenBillAmount").val(rent * days);
            $(':input[type="submit"]').prop('disabled', false);
        }
        else
        {
            $(':input[type="submit"]').prop('disabled', true);
            $("#hiddenBillAmount").val(0);
        }
    });

}); 