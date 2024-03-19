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
    debugger
    var packageId = $('#packageSelect').val();
    if (packageId >0 ) {
        $.ajax({
            url: '/Reservations/GetPackageData',
            type: 'GET',
            data: { packageId: packageId },
            success: function (data) {
                debugger

            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }
}