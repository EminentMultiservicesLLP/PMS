
    $('#DeptName').prop('disabled', true);



$("#btnAdd").click(function () {
    ClearForm();
    $('#btnSave').prop('disabled', false);
    $("#btnAdd").prop('disabled', true);
    $('#DeptName').prop('disabled', false);
    $("#DeptName").focus();
});


function LoadDeptSearchGrid() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/Masters/DepartmentMaster/GetAllDepartment",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $DepSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $DepSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) {
            $("#DeptMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#DeptMastersearchgrid").pqGrid("refreshDataAndView");
        }
    });
}


var dataDepSearchGrid = { location: "local" };
var colDepSearchGrid = [
    { title: "", dataIndx: "DeptId", dataType: "integer", hidden: true },
    {
        title: "Department Name", dataIndx: "DeptName", width: 400,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setDepSearchGrid = {
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
    colModel: colDepSearchGrid,
    dataModel: dataDepSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
        if (ui.rowData) {
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#DeptId").val(details.DeptId);
            $("#DeptName").val(details.DeptName);
            $("#Deactive").prop('checked', details.Deactive);
            $('#btnAdd').prop('disabled', true);
            $('#btnSave').prop('disabled', false);
            $('#DeptName').prop('disabled', false);
            $("#DeptName").focus();
        }
    }
}

var $DepSearchGrid = $("#DeptMastersearchgrid").pqGrid(setDepSearchGrid);

$("#btnSave").click(function () {

    if ((jQuery.trim($("#DeptName").val())).length === 0) {
        ShowAlert("warning", "Department Name is missing !!");
        return;
    }

    var details = JSON.stringify({
        DeptName: $("#DeptName").val(),
        DeptId: $("#DeptId").val(),
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

        url: 'Masters/DepartmentMaster/CreateNewDepartmentMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
            
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                $('#DeptName').prop('disabled', true);

            }
            else {
                ShowAlert("warning", msg.message);
            }
           
            ClearForm();
        },

        error: function (jqXhr, exception) {
            ShowAlert("warning", "Something went wrong! please Contact to Administrator");
        }


    });
});

$("#btnReset").click(function () {
    ClearForm();
    $('#btnAdd').prop('disabled', false);
    $('#btnSave').prop('disabled', true);
    $('#BatchName').prop('disabled', true);
});

function ClearForm() {
    $("#DeptName").val(""),
    $("#DeptId").val("");
    $("#Deactive").prop('checked', false);
    LoadDeptSearchGrid();
}
LoadDeptSearchGrid();


