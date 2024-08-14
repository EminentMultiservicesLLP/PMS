
    $('#JoiningDate').datetimepicker({ format: 'DD-MMM-YYYY h:mm a' })
$('#ConfirmationDate').datetimepicker({ format: 'DD-MMM-YYYY h:mm a' })

$('#LastPromoDate').datetimepicker({ format: 'DD-MMM-YYYY h:mm a' })
$("#ddlDepartment").prop('disabled', true),
$("#ddlQuestionnaire").prop('disabled', true),
$("#ddlDsg").prop('disabled', true);
$("#ddlOutlet").prop('disabled', true);
$("#ddlRole").prop('disabled', true);
$("#FirstName").prop('disabled', true);
$("#LastName").prop('disabled', true);
$("#Salary").prop('disabled', true);
$("#autocomplete_Employee").prop('disabled', true);
$("#autocomplete_Employee2").prop('disabled', true);
$("#EmpCode").prop('disabled', true);
$("#JoiningDate1").prop('disabled', true);
$("#ConfirmationDate1").prop('disabled', true);
$("#LastPromoDate1").prop('disabled', true);
$("#btnSave").prop('disabled', true);

var dataEmpSearchGrid = { location: "local" };
var colEmpSearchGrid = [
    { title: "", dataIndx: "EmpId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "DeptId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "RoleId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "QuestionnaireId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "DesignationId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "AppraiserEmpId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "AppraiserTwoEmpId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "OutletId", dataType: "integer", hidden: true },
    { title: "", dataIndx: "Salary", dataType: "double", hidden: true },
 //   { title: "", dataIndx: "StrJoiningDate", dataType: "string", hidden: true },
    {
        title: "Employee Code", dataIndx: "EmpCode", width: 250,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },

     {
         title: "Department", dataIndx: "DeptName", width: 250,
         filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
     },
      {
          title: "Designation", dataIndx: "DesignationName", width: 250,
          filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
      },
    {
        title: "First Name", dataIndx: "FirstName", width: 250,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
    {
        title: "Last Name", dataIndx: "LastName", width: 250,
        filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
    },
     {
         title: "Outlet Name", dataIndx: "OutletName", width: 250,
         filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] },
     },
    {
        title: "Deactive", dataIndx: "Deactive", width: 100, hidden: true
    }
];
//
var setEmployeeSearchGrid = {
    width: '100%',
    height: 200,
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
    colModel:colEmpSearchGrid ,
    dataModel: dataEmpSearchGrid,
    rowClick: function (evt, ui) {
        if (ui.rowData) {
            var rowIndx = parseInt(ui.rowIndx);
            var details = ui.rowData;
            $("#EmpId").val(details.EmpId);
            $("#DeptId").val(details.DeptId);
            $("#DesignationId").val(details.DesignationId);
            $("#OutletId").val(details.OutletId);
            $("#Salary").val(details.Salary);
            $("#EmpCode").val(details.EmpCode);
            $("#FirstName").val(details.FirstName);
            $("#LastName").val(details.LastName);
            $("#DesignationName").val(details.DesignationName);
            $("#DeptName").val(details.DeptName);
            $("#OutletName").val(details.OutletName);
            $("#ddlDepartment").val(details.DeptId);
            $("#ddlDsg").val(details.DesignationId);
            $("#ddlOutlet").val(details.OutletId);
            $("#JoiningDate1").val(details.StrJoiningDate);
            $("#ConfirmationDate1").val(details.StrConfirmationDate);
            $("#LastPromoDate1").val(details.StrLastPromoDate);
            $("#ddlQuestionnaire").val(details.QuestionnaireId);
            $("#ddlRole").val(details.RoleId);
            $("#Deactive").prop('checked', details.Deactive);
            $("#ddlDepartment").prop('disabled', false),
            $("#ddlQuestionnaire").prop('disabled', false),
            $("#ddlDsg").prop('disabled', false);
            $("#ddlOutlet").prop('disabled', false);
            $("#ddlRole").prop('disabled', false);
            $("#FirstName").prop('disabled', false);
            $("#LastName").prop('disabled', false);
            $("#Salary").prop('disabled', false);
            $("#autocomplete_Employee").prop('disabled', false);
            $("#autocomplete_Employee2").prop('disabled', false);
            $("#EmpCode").prop('disabled', false);
            $("#JoiningDate1").prop('disabled', true);
            $("#ConfirmationDate1").prop('disabled', true);
            $("#LastPromoDate1").prop('disabled', true);
            $('#btnAdd').prop('disabled', true);
            $('#btnSave').prop('disabled', false);
            set1Val(details.AppraiserEmpId);
            set2Val(details.AppraiserTwoEmpId)
            //$('#autocomplete_Employee').val(details.AppraiserEmpId);

        }
    }
}
var check = $("#EmpMastersearchgrid");

var $EmployeeSearchGrid = $("#EmpMastersearchgrid").pqGrid(setEmployeeSearchGrid);

//
$("#btnReset").click(function () {
    $("#ddlQuestionnaire").prop('disabled', true),
    $("#ddlDepartment").prop('disabled', true),
    $("#ddlDsg").prop('disabled', true);
    $("#ddlOutlet").prop('disabled', true);
    $("#ddlRole").prop('disabled', true);
    $("#FirstName").prop('disabled', true);
    $("#LastName").prop('disabled', true);
    $("#Salary").prop('disabled', true);
    $("#autocomplete_Employee").prop('disabled', true);
    $("#autocomplete_Employee2").prop('disabled', true);
    $("#EmpCode").prop('disabled', true);
    $("#JoiningDate1").prop('disabled', true);
    $("#ConfirmationDate1").prop('disabled', true);
    $("#LastPromoDate1").prop('disabled', true);
    $('#btnAdd').prop('disabled', false);
    $('#btnSave').prop('disabled', true);
    ClearForm();
     
    
});
$("#btnAdd").click(function () {
    $("#ddlDepartment").prop('disabled', false),
    $("#ddlDsg").prop('disabled', false);
    $("#ddlOutlet").prop('disabled', false);
    $("#ddlQuestionnaire").prop('disabled', false);
    $("#ddlRole").prop('disabled', false);
    $("#FirstName").prop('disabled', false);
    $("#LastName").prop('disabled', false);
    $("#Salary").prop('disabled', false);
    $("#autocomplete_Employee").prop('disabled', false);
    $("#autocomplete_Employee2").prop('disabled', false);
    $("#EmpCode").prop('disabled', false);
    $("#JoiningDate1").prop('disabled', false);
    $("#ConfirmationDate1").prop('disabled', false);
    $("#LastPromoDate1").prop('disabled', false);
    $('#btnAdd').prop('disabled', true);
    $('#btnSave').prop('disabled', false);
    ClearForm();


});

LoadEmployeeSearchGrid();

$("#btnSave").click(function () {
    var JoiningDate = $("#JoiningDate").data('date');
    var ConfirmationDate = $("#ConfirmationDate").data('date');
    var LastPromoDate = $("#LastPromoDate").data('date');

    if ((jQuery.trim($("#FirstName").val())).length === 0) {
        ShowAlert("warning", "First Name is missing !!");
        return;
    }
    if ((jQuery.trim($("#ddlDepartment").val())).length === 0) {
        ShowAlert("warning", "Select Department !!");
        return;
    }
    if ((jQuery.trim($("#ddlDsg").val())).length === 0) {
        ShowAlert("warning", "Select Designation !!");
        return;
    }

    if ((jQuery.trim($("#ddlOutlet").val())).length === 0) {
        ShowAlert("warning", "Select Outlet !!");
        return;
    }

    if ((jQuery.trim($("#ddlRole").val())).length === 0) {
        ShowAlert("warning", "Select Role !!");
        return;
    }

    if ((jQuery.trim($("#JoiningDate1").val())).length === 0) {
        ShowAlert("warning", "Joining Date is missing !!");
        return;
    }

    if ((jQuery.trim($("#ConfirmationDate1").val())).length === 0) {
        ShowAlert("warning", "Confirmation Date is missing !!");
        return;
    }

    if ((jQuery.trim($("#LastPromoDate1").val())).length === 0) {
        ShowAlert("warning", "Last Promotion Date is missing !!");
        return;
    }

    if ((jQuery.trim($("#EmpCode").val())).length === 0) {
        ShowAlert("warning", "Employee Code is missing !!");
        return;
    }

    if ((jQuery.trim($("#ddlQuestionnaire").val())).length === 0) {
        ShowAlert("warning", "Questionnaire is missing !!");
        return;
    }

    
    var details = JSON.stringify({
        Salary: $("#Salary").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        JoiningDate: JoiningDate,
        ConfirmationDate: ConfirmationDate,
        LastPromoDate: LastPromoDate,
        StrJoiningDate: $("#JoiningDate1").val(),
        StrConfirmationDate: $("#ConfirmationDate1").val(),
        StrLastPromoDate: $("#LastPromoDate1").val(),
        DeptId: $("#ddlDepartment").val(),
        DesignationId: $("#ddlDsg").val(),
        EmpId: $("#EmpId").val(),
        AppraiserEmpId: $("#AppraiserEmpId").val(),
        AppraiserTwoEmpId: $("#AppraiserTwoEmpId").val(),
        OutletId: $("#ddlOutlet").val(),
        EmpCode: $("#EmpCode").val(),
        RoleId: $("#ddlRole").val(),
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

        url: '/EmployeeMaster/CreateNewEmployeeMasters',
        data: details,
        success: function (msg) {
            if (msg.success) {
                
           
                $("#btnSave").prop('disabled', true);
                $("#btnAdd").prop('disabled', false);
                ShowAlert("success", msg.message);


            }
            else {
                ShowAlert("warning", msg.message);
            }
            LoadEmployeeSearchGrid();
            ClearForm();
        },

        error: function (jqXhr, exception) {
            ShowAlert("warning", "Something went wrong! please Contact to Administrator");
        }


    });
});
   
    
function LoadEmployeeSearchGrid() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
            
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        url: "/EmployeeMaster/GetAllEmployee",
            
        cache: false,
        async: true,
        method: "GET",
        dataType: "JSON",
        beforeSend: function () {
            $EmployeeSearchGrid.pqGrid("showLoading");
        },
        complete: function () {
            $EmployeeSearchGrid.pqGrid("hideLoading");
        },
        success: function (response) {
            
            $("#EmpMastersearchgrid").pqGrid("option", "dataModel.data", response);
            $("#EmpMastersearchgrid").pqGrid("refreshDataAndView");
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
    url: "/Masters/DepartmentMaster/GetAllDepartment",
    success: function (response) {
        $('#ddlDepartment').val("");
        //$('#ddlDesignation').html("");
        //$('#ddlDesignation').append('<option value="0">Select</option>');
        $.each(response, function (index, value) {

            $('#ddlDepartment').append('<option value="' + value.DeptId + '">' + value.DeptName + '</option>');
        });


    }
});
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
var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/Masters/DesignationMaster/GetAllDesignation",
    success: function (response) {
        $('#ddlDsg').val("");
        //$('#ddlDesignation').html("");
        //$('#ddlDesignation').append('<option value="0">Select</option>');
        $.each(response, function (index, value) {
                
            $('#ddlDsg').append('<option value="' + value.DesignationId + '">' + value.DesignationName + '</option>');
        });


    }
});
var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/Masters/OutletMaster/GetAllOutlet",
    success: function (response) {
        $('#ddlOutlet').val("");
        //$('#ddlDesignation').html("");
        //$('#ddlDesignation').append('<option value="0">Select</option>');
        $.each(response, function (index, value) {
               
            $('#ddlOutlet').append('<option value="' + value.OutletId + '">' + value.OutletName + '</option>');
        });


    }
});
var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();

$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    dataType: "json",
    url: "/ReviewReport/GetAllEmployee",
    success: function (response) {
        
        EmployeeList = [];
        for (var i = 0; i < response.data.length; i++) {
            var SearchData = response.data[i].EmpName;
            EmployeeList.push({ label: SearchData, value: SearchData, EmpId: response.data[i].EmpId });
        }
        $('#autocomplete_Employee').autocomplete({
            source: EmployeeList,
            messages: {
                noResults: '',
                results: function () { }
            },
            minLength: 0,
            scroll: true,
            html: true,
            select: function (event, ui) {
                event.preventDefault();
                setAutoComplete1Val(ui.item);
                 
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                setAutoComplete1Val(ui.item);
                return false;
            }
        })
        $('#autocomplete_Employee2').autocomplete({
            source: EmployeeList,
            messages: {
                noResults: '',
                results: function () { }
            },
            minLength: 0,
            scroll: true,
            html: true,
            select: function (event, ui) {
                event.preventDefault();
                setAutoComplete2Val(ui.item);
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                setAutoComplete2Val(ui.item);
                return false;
            }
        })
    }
});

function SelectedEmployee(event, ui) {
    event.preventDefault();
    if (ui.item !== null) {
        $(event.target).val(ui.item.label);
        $("#EmpId").val(ui.item.EmpId);
        
    }
    else {
        $("#EmpId").val("");
    }
}
   
function setAutoComplete1Val(AppraiserOne) {
    if (AppraiserOne !== null) {
        
        $('#autocomplete_Employee').val(AppraiserOne.label)
        $("#AppraiserEmpId").val(AppraiserOne.EmpId);
            
    }
    else {
        $('#autocomplete_Employee').val('')
        $("#AppraiserEmpId").val('');
    }
}    
        

function setAutoComplete2Val(AppraiserTwo) {
    if (AppraiserTwo !== null ) {
        $('#autocomplete_Employee2').val(AppraiserTwo.label)
        $("#AppraiserTwoEmpId").val(AppraiserTwo.EmpId);
    }
    else {
        $('#autocomplete_Employee2').val('')
        $("#AppraiserTwoEmpId").val('');
    
    }
}
    
function set1Val(AppraiserEmpId) {        
    var appraiserOne = $.grep(EmployeeList, function (item) { return item.EmpId == AppraiserEmpId });
    AppraiserEmpId != 0 ? setAutoComplete1Val(appraiserOne[0]) : setAutoComplete1Val(null);
}
                

function set2Val(AppraiserTwoEmpId) {        
    var appraiserTwo = $.grep(EmployeeList, function (item) { return item.EmpId == AppraiserTwoEmpId });
    AppraiserTwoEmpId != 0 ? setAutoComplete2Val(appraiserTwo[0]) : setAutoComplete2Val(null);
}

    
    
function ClearForm() {
    $("#ddlDepartment").val(""),
    $("#ddlQuestionnaire").val(""),
    $("#ddlDsg").val("");
    $("#ddlOutlet").val("");
    $("#ddlRole").val("");
    $("#FirstName").val("");
    $("#LastName").val("");
    $("#Salary").val("");
    $("#autocomplete_Employee").val("");
    $("#autocomplete_Employee2").val("");
    $("#EmpCode").val("");
    $("#JoiningDate1").val("");
    $("#ConfirmationDate1").val("");
    $("#LastPromoDate1").val("");
    $("#Deactive").prop('checked', false);
    $("#EmpId").val("");
  //  LoadDeptSearchGrid();
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
