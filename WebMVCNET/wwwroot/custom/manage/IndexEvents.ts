/// <reference path="managerole.ts" />
var manageObj = new ManageRole("", "");

$("#btnAddRole").on("click", function () {
    var selectRoleList = document.getElementById("selectRoleList") as HTMLSelectElement;

    if (selectRoleList == undefined || selectRoleList == null)
        throw "Element not exists";

    manageObj.roleId = selectRoleList.value;

    $.blockUI();

    $.ajax({
        url: '/Account/AddRole',
        method: 'POST',
        data: manageObj,
        async: false,
        success: function (data) {
            window.location.href = `/Account/ViewUser/?idUser=${data.idUser}`;
            $.unblockUI();
        },
        error: function (data) {
            $.unblockUI();
            throw data.responseJSON;
        }
    });
});