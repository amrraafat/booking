$(document).ready(function () {
    $('#tableRole').DataTable({
        "autoWidth": false,
        "responsive": false
    });
});


Edit = (CustomerId, CustomerName, Gender, Address, NationalId) => {
    document.getElementById("CustomerId").value = CustomerId;
    document.getElementById("CustomerName").value = CustomerName;
    document.getElementById("gender").value = Gender;
    document.getElementById("Address").value = Address;
    document.getElementById("NationalId").value = NationalId
}


Rest = () => {
    document.getElementById("title").innerHTML = lbAddNewRole;
    document.getElementById("btnSave").value = lbbtnSave;
    document.getElementById("userId").value = "";
    document.getElementById("userName").value = "";
    document.getElementById("userEmail").value = "";
    //document.getElementById("userImage").value = "";
    document.getElementById("ddluserRole").value = "";
    document.getElementById("userActive").checked = false;
    $('#grPassword').show();
    $('#grcomPassword').show();
    document.getElementById("userPassword").value = "";
    document.getElementById("userCompare").value = "";
    document.getElementById("image").hidden = true;
    document.getElementById("imgeHide").value = "";


}



ChangePassword = (Id) => {
    document.getElementById("ChangePasswordId").value = Id;
}