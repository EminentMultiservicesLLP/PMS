$('#GradeName').prop('disabled', true);
$('#GradePoints').prop('disabled', true);

$("#btnAdd").click(function () {
   
    ClearForm();
    $('#btnSave').prop('disabled', false);
    $("#btnAdd").prop('disabled', true);
    $('#GradeName').prop('disabled', false);
    $('#GradePoints').prop('disabled', false);
    $("#GradeName").focus();
    $("#GradePoints").focus();
});


function LoadGradeSearchGrid() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/Masters/GradeMaster/GetAllGrade",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $GradeSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $GradeSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) {
            $("#GradeMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#GradeMastersearchgrid").pqGrid("refreshDataAndView");
        }
    });
}


var dataGradeSearchGrid = { location: "local" };
var colGradeSearchGrid = [
    { title: "", dataIndx: "GradeId", dataType: "integer", hidden: true },
    {
        title: "Grade Name", dataIndx: "GradeName", width: 200,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Grade Points", dataIndx: "GradePoints", width: 200,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setGradeSearchGrid = {
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
    colModel: colGradeSearchGrid,
    dataModel: dataGradeSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
        if (ui.rowData) {
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#GradeId").val(details.GradeId);
            $("#GradeName").val(details.GradeName);
            $("#GradePoints").val(details.GradePoints);
            $("#Deactive").prop('checked', details.Deactive);
            $('#btnAdd').prop('disabled', true);
            $('#btnSave').prop('disabled', false);
            $('#GradeName').prop('disabled', false);
            $('#GradePoints').prop('disabled', false);
            $("#GradeName").focus();
        }
    }
}

var $GradeSearchGrid = $("#GradeMastersearchgrid").pqGrid(setGradeSearchGrid);

$("#btnSave").click(function () {

    if ((jQuery.trim($("#GradeName").val())).length === 0) {
        ShowAlert("warning", "Grade Name is missing !!");
        return;
    }
    if ((jQuery.trim($("#GradePoints").val())).length === 0) {
        ShowAlert("warning", "Grade Points is missing !!");
        return;
    }

    var details = JSON.stringify({
        GradeName: $("#GradeName").val(),
        GradeId: $("#GradeId").val(),
        GradePoints: $("#GradePoints").val(),
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

        url: 'Masters/GradeMaster/CreateNewGradeMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
               
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                $('#GradeName').prop('disabled', true);
                $('#GradePoints').prop('disabled', true);

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
    $('#GradeName').prop('disabled', true);
    $('#GradePoints').prop('disabled', true);
});

function ClearForm() {
    $("#GradeName").val(""),
    $("#GradePoints").val(""),
    $("#GradeId").val("");
    $("#Deactive").prop('checked', false);
    LoadGradeSearchGrid();
}
LoadGradeSearchGrid();