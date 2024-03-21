
   
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