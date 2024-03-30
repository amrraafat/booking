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
                $('#TotalPriceHidden').val(data.adultPrice * AdultsCount + data.kidPrice * KidsCount)
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
    changePaid();
}

function changeCounts() {
    //debugger
    var AdultsCount = $("#AdultNo").val();
    var KidsCount = $("#KidNo").val();
    var adultPrice = $('#AdultPrice').val()
    var kidPrice = $('#KidPrice').val()
    var discount = $('#Discount').val();
    var totalPrice = adultPrice * AdultsCount + kidPrice * KidsCount;
    $('#TotalPrice').val(totalPrice - discount)
    $('#TotalPriceHidden').val(totalPrice - discount)
    caluclateDiscount();
    changePaid();
}

function changePaid() {
    var priceAfterDiscount = $('#priceAfterDiscount').val();
    var discount = $('#Discount').val();
    var totalPrice = $('#TotalPrice').val();
    var paid = $('#Paid').val();
    if (paid > 0) {
        if ((totalPrice - discount) - paid < 0) {
            $('#Paid').val(0);
            alert("Paid must be less that the total price");

        } else {
            $('#Remain').val(priceAfterDiscount - paid)
            $('#RemainHidden').val(priceAfterDiscount - paid)
        }
    }
}

 SelectCustomer = (CustomerName, NationalId,CustomerId) => {
     document.getElementById("CustomerId").value = CustomerId;
    document.getElementById("CustomerName").value = CustomerName;
     document.getElementById("NationalId").value = NationalId;
     $('#CrateNew').modal('hide');


}

Edit = (ReservationId, CustomerName, Discount, AdultNo, KidNo, EmployeeId) => {
    document.getElementById("ReservationId").value = ReservationId;
    document.getElementById("EmployeeId").value = EmployeeId;
    document.getElementById("CustomerName").value = CustomerName;
    document.getElementById("Discount").value = Discount;
    document.getElementById("AdultNo").value = AdultNo;
    document.getElementById("KidNo").value = KidNo;


}

function printReservation(id) {
    
    // Get the ID of the reservation you want to print
    var reservationId = id; // Replace with the actual reservation ID

    // Make an AJAX request to the PrintReservation action
    $.ajax({
        url: '/Reservations/PrintReservation', // Replace with the correct URL
        type: 'GET',
        data: { id: reservationId }, // Pass the reservation ID as a parameter
        success: function (response) {
            // Open the returned view in a new window
            var newWindow = window.open("", "_blank");
            newWindow.document.write(response);
            newWindow.document.close();
        },
        error: function (xhr, status, error) {
            
            console.error(xhr.responseText); // Log the responseText for debugging
            console.error(status); // Log the status for debugging
            console.error(error); // Log the error for debugging
            alert('Error occurred while printing reservation.');
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    
    packageChange();


});








function validateForm() {
    debugger
    var customerName = document.getElementById("CustomerName").value;
    var paid = document.getElementById("Paid").value;
    var hotelSelect = document.getElementById("hotelSelect").value;
    var packageSelect = document.getElementById("packageSelect").value;
    var AdultNo = document.getElementById("AdultNo").value;

    if (customerName.trim() === "") {
        document.getElementById("VCustomerName").textContent = "يجب تحديد العميل ";



    if (paid.trim() === "") {
        document.getElementById("Vpaid").textContent = "يجب إدخال المبلغ المدفوع ";
           
    }


    if (hotelSelect.trim() === "0") {
            document.getElementById("VhotelSelect").textContent = "يجب تحديد الفندق ";

    }


    if (packageSelect.trim() === "0") {
            document.getElementById("VpackageSelect").textContent = "يجب تحديد العرض ";

    }

       

    if (AdultNo.trim() === "") {
            document.getElementById("VAdultNo").textContent = "يجب إدخال عدد الافراد ";

    }


        return false;
    }

    if (customerName.trim() !== "") {
        document.getElementById("VCustomerName").textContent = "";


    if (paid.trim() !== "") {
        document.getElementById("Vpaid").textContent = "";

    }


    if (paid.trim() !== "0") {
        document.getElementById("VhotelSelect").textContent = "";

    }

    if (paid.trim() !== "0") {
        document.getElementById("VpackageSelect").textContent = "";

    }

    if (paid.trim() !== "") {
        document.getElementById("VAdultNo").textContent = "";

    }

   

        return true;

    }
    
   

    


    
     
}