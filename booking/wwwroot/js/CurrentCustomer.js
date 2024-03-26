$(document).ready(function () {
    $('#tableRole').DataTable({
        "autoWidth": false,
        "responsive": false
    });
});


Edit = (CustomerId, CustomerName, Gender, Address, NationalId, MobileNumber) => {
    document.getElementById("CustomerId").value = CustomerId;
    document.getElementById("CustomerName").value = CustomerName;
    document.getElementById("gender").value = Gender;
    document.getElementById("Address").value = Address;
    document.getElementById("NationalId").value = NationalId;
    document.getElementById("MobileNumber").value = MobileNumber;


}



