function LoadPachages() {
    var hotelId = $('#hotelSelect').val();
    $.ajax({
        url: '/Reservations/GetPackagesByHotelId',
        type: 'GET',
        data: { hotelId: hotelId },
        success: function (data) {
            $('#packageSelect').empty();
            $.each(data, function (index, package) {
                $('#packageSelect').append($('<option>').text(package.packageName).attr('value', package.packageId));
            });
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}