$(document).ready(function () {
    $('#tableRole').DataTable({
        "autoWidth": false,
        "responsive": false
    });
});

Edit = (id,name,image,Email,ActiveUser,Role) => {
    document.getElementById("userId").value = id;
    document.getElementById("userName").value = name;
    document.getElementById("userEmail").value = Email;
    document.getElementById("image").hidden = false;
    document.getElementById("image").src = "/Images/Users/" + image;
    document.getElementById("imageHide").value = image;
    document.getElementById("ddluserRole").value = Role;
    $('#grPassword').hide();
    $('#grcomPassword').hide();
    var Active = document.getElementById("userActive");
    if (ActiveUser == "True")
        Active.checked = true;
    else Active.checked = false;
    document.getElementById("userPassword").value = "********";
    document.getElementById("userCompare").value = "********";
    document.getElementById("image").hidden = false;
   
}

ChangePassword = (Id) => {
    document.getElementById("ChangePasswordId").value = Id;
}