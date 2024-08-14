
  




$('#LoginName').prop('disabled', true);
$('#ddlRole').prop('disabled', true);
$('#btnSave').prop('disabled', true);
$('#btnChange').prop('disabled', true);
$('#Password').prop('disabled', true);

$("#btnReset").click(function () {
    
    ClearForm();
    //$('#Password').prop('disabled', true);
    //$('#btnSave').prop('disabled', true);
    //$('#btnChange').prop('disabled', true);
    //$('#LoginName').prop('disabled', true);
    //$('#ddlRole').prop('disabled', true);
    //$("#ddlRole").val('');
    //$("#LoginName").val('');
    //$("#Password").val('');
    
});

    
function LoadUserSearchGrid() {
    //debugger;
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/UserMaster/GetAllUser",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
           // debugger;
            $UserSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $UserSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) { 
            $("#UserMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#UserMastersearchgrid").pqGrid("refreshDataAndView");
        }
    });
}

//debugger;
var dataUserSearchGrid = { location: "local" };
var colUserSearchGrid = [
    { title: "", dataIndx: "UserId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "EmpId", dataType: "integer", hidden: true },
    {
        title: "Emp Code", dataIndx: "EmpCode", width: 100,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "First Name", dataIndx: "FirstName", width: 300,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
        
    {
        title: "Last Name", dataIndx: "LastName", width: 300,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setUserSearchGrid = {
    width: '50%',
    height: 525,
    sortable: false,
    numberCell: { show: true },
    hoverMode: 'cell',
    showTop: true,
    resizable: true,
    scrollModel: { autoFit: true },
    draggable: false,
    wrap: false,
    editable: false,
    filterModel: { on: true, mode: "AND", header: true },
    selectionModel: { type: 'row', subtype: 'incr', cbHeader: true, cbAll: true },
    colModel: colUserSearchGrid,
    dataModel: dataUserSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
        if (ui.rowData) {//debugger;
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#UserId").val(details.UserId);
            $("#EmpId").val(details.EmpId);
            $("#ddlRole").val(details.RoleId);
            $("#LoginName").val(details.LoginName);
            $("#Password").val(details.Password);
            $("#Deactive").prop('checked', details.Deactive);
            $('#Password').prop('disabled', true);
            $('#btnSave').prop('disabled', true);
            $('#LoginName').prop('disabled', true);
            $('#ddlRole').prop('disabled', true);
            $('#btnChange').prop('disabled', false);
            $('#Deactive').prop('disabled', true);

            $("#Password").focus();
        }
    }
}

var $UserSearchGrid = $("#UserMastersearchgrid").pqGrid(setUserSearchGrid);
   


var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/Masters/EmployeeMaster/GetAllRole",
    success: function (response) {
        $('#ddlRole').val("");
        //$('#ddlDesignation').html("");
        //$('#ddlDesignation').append('<option value="0">Select</option>');
        $.each(response, function (index, value) {
          
            $('#ddlRole').append('<option value="' + value.RoleId + '">' + value.RoleName + '</option>');
        });


    }
});

$("#btnChange").click(function () {
    $('#Password').prop('disabled', false);
    $('#btnSave').prop('disabled', false);
    $('#Deactive').prop('disabled', false);
});

$("#btnSave").click(function () {

    if ((jQuery.trim($("#LoginName").val())).length === 0) {
        ShowAlert("warning", "Login Name is missing !!");
        return;
    }

    if ((jQuery.trim($("#Password").val())).length === 0) {
        ShowAlert("warning", "Password is missing !!");
        return;
    }
    //if ((jQuery.trim($("#ddlRole").val())).length === 0) {
    //    ShowAlert("warning", "Role is missing !!");
    //    return;
    //}
    var details = JSON.stringify({
       // LoginName: $("#LoginName").val(),
        Password: $("#Password").val(),
        UserId: $("#UserId").val(),
        EmpId:$("#EmpId").val(),
       // RoleId: $("#ddlRole").val(),
        Deactive: $("#Deactive").prop('checked')
    });
    
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: "POST", //HTTP POST Method
        traditional: true,
        contentType: 'application/json; charset=utf-8',
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },

        url: '/Masters/UserMaster/UpdateUserMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
                
                $("#btnSave").prop('disabled', true);
                $("#btnChange").prop('disabled', true);
                $('#LoginName').prop('disabled', true);
                $('#ddlRole').prop('disabled', true);
                $('#Password').prop('disabled', true);
                


            }
            else {
                ShowAlert("warning", msg.message);
            }
            LoadUserSearchGrid();
            ClearForm();
        },

        error: function (jqXhr, exception) {
            ShowAlert("warning", "Something went wrong! please Contact to Administrator");
        }


    });
   // LoadUserSearchGrid()
});
LoadUserSearchGrid();

function ClearForm()
{
    $('#LoginName').val('');
    $('#ddlRole').val('');
    $('#Password').val('');
    $("#btnSave").prop('disabled', true);
    $("#btnChange").prop('disabled', true);
    $('#LoginName').prop('disabled', true);
    $('#ddlRole').prop('disabled', true);
    $('#Password').prop('disabled', true);
    $("#Deactive").prop('checked', 0)
    $('#Deactive').prop('disabled', true);

}

