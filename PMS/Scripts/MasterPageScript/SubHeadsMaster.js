
$('#SubHeadName').prop('disabled', true);
//$('#ddlHead').prop('disabled', true);
$('#QuestionnaireName').prop('disabled', true);



$("#btnAdd").click(function () {
    ClearForm();
    $('#btnSave').prop('disabled', false);
    $("#btnAdd").prop('disabled', true);
    $('#SubHeadName').prop('disabled', false);
    $('#ddlHead').prop('disabled', false);
    $('#QuestionnaireName').prop('disabled', true);
    getAllHeadName();
    $("#SubHeadId").val("");
    $("#SubHeadName").focus();
});


function LoadSubHeadSearchGrid() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/SubHeadsMaster/GetAllSubHeads",
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $SubHeadSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $SubHeadSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) {
            $("#SubHeadMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#SubHeadMastersearchgrid").pqGrid("refreshDataAndView");
        }
    });
}


var dataSubHeadSearchGrid = { location: "local" };
var colSubHeadSearchGrid = [
    { title: "", dataIndx: "HeadId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "QuestionnaireId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "SubHeadId", dataType: "integer", hidden: true },


     {
         title: "Questionnaire Name", dataIndx: "QuestionnaireName", width: 200,
         filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
     },
    {
        title: "Head Name", dataIndx: "HeadName", width: 200,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "SubHead Name", dataIndx: "SubHeadName", width: 200,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];

var setSubHeadSearchGrid = {
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
    colModel: colSubHeadSearchGrid,
    dataModel: dataSubHeadSearchGrid,
    pageModel: { type: "local", rPP: 20 },
    rowClick: function (evt, ui) {
      //  debugger;
        if (ui.rowData) {
       //     debugger;
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#HeadId").val(details.HeadId);
            $("#SubHeadId").val(details.SubHeadId);
            // $("#ddlHead").val(details.HeadId);            
            $('#ddlHead').prop('disabled', false);
            $('#SubHeadName').prop('disabled', false);
            $('#btnSave').prop('disabled', false);

            var QuestionnaireId = details.QuestionnaireId;
            var active = false;
            $('#ddlQuestionnaire').find('option').each(function (index, element) {
                var ddlElement = element.value;

                if (ddlElement == QuestionnaireId) {
                    active = true;
                }

            });

            if (!active) {
                ShowAlert("warning", "Questionnaire is Deactivated");
                $('#ddlHead').prop('disabled', true);
                $('#SubHeadName').prop('disabled', true);
                $('#btnSave').prop('disabled', true);
            }

            $("#QuestionnaireId").val(details.QuestionnaireId);
            $("#QuestionnaireName").val(details.QuestionnaireName);
            $("#SubHeadName").val(details.SubHeadName);
            $("#Deactive").prop('checked', details.Deactive);
            $('#btnAdd').prop('disabled', true);
            
           // $('#SubHeadName').prop('disabled', false);
            
            ddlQuestionnaireChange(details.QuestionnaireId, details.HeadId);
            $('#QuestionnaireName').prop('disabled', true);
           // $("#ddlHead").val(details.HeadId);
            $("#SubHeadName").focus();
        }
    }
}

var $SubHeadSearchGrid = $("#SubHeadMastersearchgrid").pqGrid(setSubHeadSearchGrid);

$("#btnSave").click(function () {

    if ((jQuery.trim($("#SubHeadName").val())).length === 0) {
        ShowAlert("warning", "Sub Head Name is missing !!");
        return;
    }
    if ((jQuery.trim($("#ddlHead").val())).length === 0) {
        ShowAlert("warning", "Select Head Name  !!");
        return;
    }

    var details = JSON.stringify({
        SubHeadName: $("#SubHeadName").val(),
        SubHeadId: $("#SubHeadId").val(),
        HeadId: $("#ddlHead").val(),
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

        url: '/SubHeadsMaster/CreateNewSubHeadsMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                ShowAlert("success", msg.message);
              
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                $('#HeadName').prop('disabled', true);
                $('#ddlHead').prop('disabled', true);


            }
            else {
                ShowAlert("warning", msg.message);
            }
            LoadSubHeadSearchGrid();
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
    $('#SubHeadName').prop('disabled', true);
    $('#ddlHead').prop('disabled', true);    
    $('#QuestionnaireName').prop('disabled', true);


});

function ClearForm() {
    $("#SubHeadName").val(""),
   $("#ddlHead").val("");
    $('#ddlHead').text("");
    $('#QuestionnaireName').val("");
    $("#HeadId").val("");
    $("#Deactive").prop('checked', false);
   
}
LoadSubHeadSearchGrid();

function ddlQuestionnaireChange(questionnaireId,headId) {
  //  debugger;
    $('#ddlHead').text("");

    //let questionnaireId = $("#ddlQuestionnaire").val();

    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    //   var headsArr = new Array();
    $.ajax({
        type: "GET",
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        traditional: true,
        data: { QuestionnaireId: questionnaireId },
        url: "/SubHeadsMaster/GetAllHeadName",
        success: function (response) {
            $('#ddlHead').val("");
            debugger;
            //$('#ddlDesignation').html("");
            //$('#ddlDesignation').append('<option value="0">Select</option>');
            $.each(response, function (index, value) {
                // if ($("#ddlDepartment").val() == response.DeptId) {
                // headsArr.push(value.DeptId);
              //  $('#ddlHead').append('<option value="' + value.HeadId + '">' + value.HeadName + '</option>');
                $('#ddlHead').append('<option value="' + value.HeadId + '"> <column>'
                             + value.HeadName + '</column> &nbsp - &nbsp <column>' + value.QuestionnaireName + '</column></option>');
                //}
            });
            $("#ddlHead").val(headId);
        }
    });
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

getAllHeadName();


//   var headsArr = new Array();

function getAllHeadName() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: "GET",
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        traditional: true,
        data: { QuestionnaireId: 0 },
        url: "/SubHeadsMaster/GetAllHeadName",
        success: function (response) {
            $('#ddlHead').val("");
            //$('#ddlDesignation').html("");
            //$('#ddlDesignation').append('<option value="0">Select</option>');
            $.each(response, function (index, value) {
                // if ($("#ddlDepartment").val() == response.DeptId) {
                // headsArr.push(value.DeptId);
                $('#ddlHead').append('<option value="' + value.HeadId + '"> <column>'
                   + value.HeadName + '</column> &nbsp - &nbsp <column>' + value.QuestionnaireName + '</column></option>');

                //}
            });
        }
    });
}