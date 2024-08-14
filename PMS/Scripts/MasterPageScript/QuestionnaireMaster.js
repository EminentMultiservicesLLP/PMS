$('#QuestionnaireName').prop('disabled', true);



$("#btnAdd").click(function () {
    ClearForm();
    $('#btnSave').prop('disabled', false);
    $("#btnAdd").prop('disabled', true);
    $('#QuestionnaireName').prop('disabled', false);
    $("#QuestionnaireName").focus();
});


function LoadQuestnSearchGrid() {
    
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/Masters/Questionnaire/GetAllQuestionnaire",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $Questionnairesearchgrid.pqGrid("showLoading");
        },
        complete: function () {
            $Questionnairesearchgrid.pqGrid("hideLoading");
        },
        success: function (response) {
            $("#Questionnairesearchgrid").pqGrid("option", "dataModel.data", response);
            $("#Questionnairesearchgrid").pqGrid("refreshDataAndView");
        }
    });
}


var dataQuestnSearchGrid = { location: "local" };
var colQuestnSearchGrid = [
    { title: "", dataIndx: "QuestionnaireId", dataType: "integer", hidden: true },
    {
        title: "Questionnaire Name", dataIndx: "QuestionnaireName", width: 400,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setQuestntSearchGrid = {
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
    colModel: colQuestnSearchGrid,
    dataModel: dataQuestnSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
        if (ui.rowData) {
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#QuestionnaireId").val(details.QuestionnaireId);
            $("#QuestionnaireName").val(details.QuestionnaireName);
            $("#Deactive").prop('checked', details.Deactive);
            $('#btnAdd').prop('disabled', true);
            $('#btnSave').prop('disabled', false);
            $('#QuestionnaireName').prop('disabled', false);
            $("#QuestionnaireName").focus();
        }
    }
}

var $Questionnairesearchgrid = $("#Questionnairesearchgrid").pqGrid(setQuestntSearchGrid);

$("#btnSave").click(function () {

    if ((jQuery.trim($("#QuestionnaireName").val())).length === 0) {
        ShowAlert("warning", "Questionnaire Name is missing !!");
        return;
    }

    var details = JSON.stringify({
        QuestionnaireName: $("#QuestionnaireName").val(),
        QuestionnaireId: $("#QuestionnaireId").val(),
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

        url: '/Questionnaire/CreateNewQUestnMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
               
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                $('#QuestionnaireName').prop('disabled', true);

            }
            else {
                ShowAlert("warning", msg.message);
            }
            LoadQuestnSearchGrid();
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
    $('#QuestionnaireName').prop('disabled', true);
});

function ClearForm() {
    $("#QuestionnaireName").val(""),
    $("#QuestionnaireId").val("");
    $("#Deactive").prop('checked', false);
 
}
LoadQuestnSearchGrid();