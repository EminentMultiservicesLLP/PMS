$('#DesignationName').prop('disabled', true);



$("#btnAdd").click(function () {
    ClearForm();
    $('#btnSave').prop('disabled', false);
    $("#btnAdd").prop('disabled', true);
    $('#DesignationName').prop('disabled', false);
    $("#DesignationName").focus();
});


function LoadDsgSearchGrid() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/Masters/DesignationMaster/GetAllDesignation",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $DsgSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $DsgSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) {
            $("#DsgMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#DsgMastersearchgrid").pqGrid("refreshDataAndView");
        }
    });
}


var dataDsgSearchGrid = { location: "local" };
var colDsgSearchGrid = [
    { title: "", dataIndx: "DesignationId", dataType: "integer", hidden: true },
    {
        title: "Designation Name", dataIndx: "DesignationName", width: 400,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setDsgSearchGrid = {
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
    colModel: colDsgSearchGrid,
    dataModel: dataDsgSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
        if (ui.rowData) {
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#DesignationId").val(details.DesignationId);
            $("#DesignationName").val(details.DesignationName);
            $("#Deactive").prop('checked', details.Deactive);
            $('#btnAdd').prop('disabled', true);
            $('#btnSave').prop('disabled', false);
            $('#DesignationName').prop('disabled', false);
            $("#DesignationName").focus();
        }
    }
}

var $DsgSearchGrid = $("#DsgMastersearchgrid").pqGrid(setDsgSearchGrid);

$("#btnSave").click(function () {

    if ((jQuery.trim($("#DesignationName").val())).length === 0) {
        ShowAlert("warning", "Designation Name is missing !!");
        return;
    }

    var details = JSON.stringify({
        DesignationName: $("#DesignationName").val(),
        DesignationId: $("#DesignationId").val(),
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

        url: '/DesignationMaster/CreateNewDesignationMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
              
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                $('#DesignationName').prop('disabled', true);

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
    $("#DesignationName").val(""),
    $("#DesignationId").val("");
    $("#Deactive").prop('checked', false);
    LoadDsgSearchGrid();
}
LoadDsgSearchGrid();
