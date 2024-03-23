
   
    document.getElementById("reservationButton").addEventListener("click", function() {
    var packageId = document.getElementById("packageSelect").value;
    var hotelId = 'your_hotel_id_here';
    console.log("Selected Package ID:", packageId);
    console.log("Hotel ID:", hotelId);
    });

function setPachageValue() {
    var changesValue = $('#packageSelect').val();
    $('#packageSelectForController').val(changesValue);
}
function CheckPackage() {
    debugger
    var hotelId = $('#HotelId').val()
    var packageSelect = $('#packageSelect').val()
    if (packageSelect <= 0) {
        alert("Select Package")
    } else {
        window.location.href = '@Url.Action("Create", "Reservations")' + '?hotelId=' + hotelId + '&packageId=' + packageSelect;
    }
}

