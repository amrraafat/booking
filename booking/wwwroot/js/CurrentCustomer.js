$(document).ready(function () {
    $('#tableRole').DataTable({
        "autoWidth": false,
        "responsive": false
    });
});
let table = new DataTable('#tableRole');

Edit = (CustomerId, CustomerName, Gender, Address, NationalId, MobileNumber,NationalIdImage) => {
    document.getElementById("CustomerId").value = CustomerId;
    document.getElementById("CustomerName").value = CustomerName;
    document.getElementById("gender").value = Gender;
    document.getElementById("Address").value = Address;
    document.getElementById("NationalId").value = NationalId;
    document.getElementById("MobileNumber").value = MobileNumber;
    document.getElementById("ImageId").value = NationalIdImage;
    
}
