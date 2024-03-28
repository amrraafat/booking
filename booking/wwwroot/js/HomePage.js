
   
    document.getElementById("reservationButton").addEventListener("click", function() {
    var packageId = document.getElementById("packageSelect").value;
    var hotelId = 'your_hotel_id_here';
    console.log("Selected Package ID:", packageId);
    console.log("Hotel ID:", hotelId);
    });

function setPackageValue(element, packageId, PackageName) {
    var card = element.closest('.card');
    var packageNameInput = card.querySelector('.package-name');
    var packageInput = card.querySelector('.package-id');

    packageInput.value = packageId;
    packageNameInput.value = PackageName;


}


