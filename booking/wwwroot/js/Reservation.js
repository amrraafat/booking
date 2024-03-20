function LoadPachages() {
    var hotelId = $('#hotelSelect').val();
    $.ajax({
        url: '/Reservations/GetPackagesByHotelId',
        type: 'GET',
        data: { hotelId: hotelId },
        success: function (data) {
            $('#packageSelect').empty();
            $('#packageSelect').append($('<option>').text('Select Package').attr('value', '0'));
            $.each(data, function (index, package) {
                $('#packageSelect').append($('<option>').text(package.packageName).attr('value', package.packageId));
            });
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}
function packageChange() {
    //debugger
    var AdultsCount = $("#AdultNo").val();
    var KidsCount = $("#KidNo").val();

    var packageId = $('#packageSelect').val();
    if (packageId > 0) {
        $.ajax({
            url: '/Reservations/GetPackageData',
            type: 'GET',
            data: { packageId: packageId },
            success: function (data) {
                //debugger
                $('#AdultPrice').val(data.adultPrice)
                $('#KidPrice').val(data.kidPrice)
                $('#TotalPrice').val(data.adultPrice * AdultsCount + data.kidPrice * KidsCount)
                caluclateDiscount();
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }
}
function caluclateDiscount() {
    //debugger
    var discount = $('#Discount').val();
    var totalPrice = $('#TotalPrice').val();
    if (totalPrice - discount < 0) {
        $('#Discount').val(0);
        alert("discount must be less than total price");
    } else {
        $('#priceAfterDiscount').val(totalPrice - discount)
    }
}

function changeCounts() {
    var AdultsCount = $("#AdultNo").val();
    var KidsCount = $("#KidNo").val();
    var adultPrice = $('#AdultPrice').val()
    var kidPrice = $('#KidPrice').val()
    var discount = $('#Discount').val();
    var totalPrice = adultPrice * AdultsCount + kidPrice * KidsCount;
    $('#TotalPrice').val(totalPrice - discount)
    caluclateDiscount();
}

function changePaid() {
    var priceAfterDiscount = $('#priceAfterDiscount').val();
    var paid = $('#Paid').val();
    if (priceAfterDiscount - paid < 0) {
        $('#Paid').val(0);
        alert("Paid must be less that the total price");

    } else {
        $('#Remain').val(priceAfterDiscount - paid)
    }
}