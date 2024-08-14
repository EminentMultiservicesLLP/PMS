

$('#HeadName').prop('disabled', true);
$('#ddlQuestionnaire').prop('disabled', true);




$("#btnAdd").click(function () {
    ClearForm();
    $('#btnSave').prop('disabled', false);
    $("#btnAdd").prop('disabled', true);
    $('#HeadName').prop('disabled', false);
    $('#ddlQuestionnaire').prop('disabled', false);

    $("#HeadName").focus();
});





var dataHeadSearchGrid = { location: "local" };
var colHeadSearchGrid = [
    { title: "", dataIndx: "HeadId", dataType: "integer", hidden: true },
    {
        title: "Questionnaire Name", dataIndx: "QuestionnaireName", width: 400,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Head Name", dataIndx: "HeadName", width: 400,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setHeadSearchGrid = {
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
    colModel: colHeadSearchGrid,
    dataModel: dataHeadSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
        if (ui.rowData) {
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#HeadId").val(details.HeadId);
            $("#QuestionnaireId").val(details.QuestionnaireId);
            //debugger;
            var QuestionnaireId = details.QuestionnaireId;
            var active = false;
            $('#ddlQuestionnaire').find('option').each(function (index, element) {
                var ddlElement = element.value;
                
                if (ddlElement == QuestionnaireId)
                {
                    active = true;
                }            

            });
                       
            if (!active) {
                ShowAlert("warning", "Questionnaire is Deactivated");
            }


            $("#ddlQuestionnaire").val(details.QuestionnaireId);
            $("#HeadName").val(details.HeadName);
            $("#Deactive").prop('checked', details.Deactive);
            $('#btnAdd').prop('disabled', true);
            $('#btnSave').prop('disabled', false);
            $('#HeadName').prop('disabled', false);
            $('#ddlQuestionnaire').prop('disabled', false);

            $("#HeadName").focus();
        }
    }
}


var $HeadSearchGrid = $("#HeadMastersearchgrid").pqGrid(setHeadSearchGrid);
LoadHeadSearchGrid();
function LoadHeadSearchGrid() {
    debugger;
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/Masters/HeadsMaster/GetAllHeads",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $HeadSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $HeadSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) {
            $("#HeadMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#HeadMastersearchgrid").pqGrid("refreshDataAndView");
        }
    });
}
$("#btnSave").click(function () {

    if ((jQuery.trim($("#HeadName").val())).length === 0) {
        ShowAlert("warning", "Head Name is missing !!");
        return;
    }

    var details = JSON.stringify({
        HeadName: $("#HeadName").val(),
        HeadId: $("#HeadId").val(),
        QuestionnaireId: $("#ddlQuestionnaire").val(),
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

        url: '/Masters/HeadsMaster/CreateNewHeadsMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
               
               
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                $('#HeadName').prop('disabled', true);
                $('#ddlQuestionnaire').prop('disabled', true);


            }
            else {
                ShowAlert("warning", msg.message);
            }
            LoadHeadSearchGrid();
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
    $('#HeadName').prop('disabled', true);
    $('#ddlQuestionnaire').prop('disabled', true);


});

function Check()
{

}

function ClearForm() {
    $("#HeadName").val(""),
    $("#ddlQuestionnaire").val("");
    $("#HeadId").val("");
    $("#Deactive").prop('checked', false);
   //LoadHeadSearchGrid();
}

var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/Masters/Questionnaire/GetAllActiveQuestion",
    success: function (response) {
        $('#ddlQuestionnaire').val("");
        //$('#ddlDesignation').html("");
        //$('#ddlDesignation').append('<option value="0">Select</option>');
        $.each(response, function (index, value) {
            $('#ddlQuestionnaire').append('<option value="' + value.QuestionnaireId + '">' + value.QuestionnaireName + '</option>');
        });


    }
});