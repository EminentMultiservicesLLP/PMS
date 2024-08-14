$('#OutletName').prop('disabled', true);



$("#btnAdd").click(function () {
    ClearForm();
    $('#btnSave').prop('disabled', false);
    $("#btnAdd").prop('disabled', true);
    $('#OutletName').prop('disabled', false);
    $("#OutletName").focus();
});


function LoadOutletSearchGrid() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/Masters/OutletMaster/GetAllOutlet",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $OutletSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $OutletSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) {
            $("#OutletMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#OutletMastersearchgrid").pqGrid("refreshDataAndView");
        }
    });
}


var dataOutletSearchGrid = { location: "local" };
var colOutletSearchGrid = [
    { title: "", dataIndx: "OutletId", dataType: "integer", hidden: true },
    {
        title: "Outlet Name", dataIndx: "OutletName", width: 400,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setOutletSearchGrid = {
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
    colModel: colOutletSearchGrid,
    dataModel: dataOutletSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
        if (ui.rowData) {
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#OutletId").val(details.OutletId);
            $("#OutletName").val(details.OutletName);
            $("#Deactive").prop('checked', details.Deactive);
            $('#btnAdd').prop('disabled', true);
            $('#btnSave').prop('disabled', false);
            $('#OutletName').prop('disabled', false);
            $("#OutletName").focus();
        }
    }
}

var $OutletSearchGrid = $("#OutletMastersearchgrid").pqGrid(setOutletSearchGrid);

$("#btnSave").click(function () {

    if ((jQuery.trim($("#OutletName").val())).length === 0) {
        ShowAlert("warning", "Outlet Name is missing !!");
        return;
    }

    var details = JSON.stringify({
        OutletName: $("#OutletName").val(),
        OutletId: $("#OutletId").val(),
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

        url: '/OutletMaster/CreateNewOutletMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
                LoadOutletSearchGrid();
                ClearForm();
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                $('#OutletName').prop('disabled', true);

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
    $('#OutletName').prop('disabled', true);
});

function ClearForm() {
    $("#OutletName").val(""),
    $("#OutletId").val("");
    $("#Deactive").prop('checked', false);
    LoadOutletSearchGrid();
}
LoadOutletSearchGrid();