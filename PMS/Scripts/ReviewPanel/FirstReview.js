var selectedEmpforReview = [];
var subheadCount = 0;
var sumOfPoints = 0.0;
var SubmitFlag = false;

/***** Load on Page ****/

loadEmpRevSearchdata();

//****************** Employee Review Search Grid ******************//
var dataEmpRvwSearchGrid = { location: "local", sorting: "local" };
var colEmpRvwSearchGrid = [
      { title: "", dataIndx: "EmpReviewId", dataType: "integer", hidden: true },
      { title: "Emp Id", dataIndx: "EmpCode", editable: false, width: 70, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Name", dataIndx: "EmpName", editable: false, width: 400, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Designation", dataIndx: "DesignationName", editable: false, width: 200, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Grade", dataIndx: "GradeName", editable: false, width: 200, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Appraiser 1 Status", dataIndx: "ApprOneStatus", editable: false, width: 200, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Appraiser 2 Status", dataIndx: "ApprTwoStatus", editable: false, width: 200, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
];

var setEmpRvwSearchGrid = gridCommonObject;
setEmpRvwSearchGrid.width = '100%';
setEmpRvwSearchGrid.height = 500;
setEmpRvwSearchGrid.colModel= colEmpRvwSearchGrid;
setEmpRvwSearchGrid.dataModel = dataEmpRvwSearchGrid;
setEmpRvwSearchGrid.title = 'Reviewed Employess';
setEmpRvwSearchGrid.rowClick = function (evt, ui) {
    if (ui.rowData) {
        ClearForm();
        selectedEmpforReview = [];
        var selEmployee = ui.rowData;
        $('#FirstEmpReviewId').val(selEmployee.EmpReviewId);
        AddSelectedEmployeeData(selEmployee);
        sumOfPoints = selEmployee.GradeScore;
        GetHeads(selEmployee.EmpReviewId);
        CollapsePqGrid($("#EmpRvwSearchGrid"));
        ShowGrade(selEmployee.GradePoints);
        BindConclusionData(selEmployee);
        ShowButtons(selEmployee.IsShwBtn);
    }
};
$("#EmpRvwSearchGrid").pqGrid(setEmpRvwSearchGrid);

function loadEmpRevSearchdata() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: "GET",
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },      
        url: "/FirstReview/GetRvwdEmployees",
        datatype: "Json",
        beforeSend: function () {
            $("#divPartialLoading").show();
        },
        success: function (data) {

            $("#EmpRvwSearchGrid").pqGrid("hideLoading");
            $("#EmpRvwSearchGrid").pqGrid("option", "dataModel.data", data.rvdEmployees);
            $("#EmpRvwSearchGrid").pqGrid("refreshDataAndView");
            PqGridRefreshClick($("#EmpRvwSearchGrid"));
            $("#divPartialLoading").hide();
        },
        error: function (f, e, m) {
           
        }
    });
}

   
//****************** Employee Grid ************************//

var dataEmployeeGrid = { location: "local", sorting: "local" };
var colEmployeeGrid = [
                
    {
        dataIndx: "State", Width: 15, align: "center", type: 'checkBoxSelection', cls: 'ui-state-default', editor: false, dataType: 'bool',
        title: "<input type='checkbox' />",
        cb: { select: true, all: false, header: true },
    },
    { title: "", dataIndx: "EmpId", dataType: "integer", hidden: true },
    { title: "Emp Code", dataIndx: "EmpCode", editable: false, width: 100, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
    { title: "Employee Name", dataIndx: "EmpName", editable: false, width: 300, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
    { title: "Outlet", dataIndx: "OutletName", editable: false, width: 100, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } }
];

var setpopupEmployeeGrid = {
    width: '100%',
    height: 510,
    sortable: false,
    numberCell: { show: true },
    hoverMode: 'cell',
    showTop: false,
    title: 'Employee for Review',
    scrollModel: { autoFit: true },
    draggable: true,
    wrap: false,
    filterModel: { off: false, mode: "AND", header: true },
    pageModel: { type: "local", rPP: 100 },
    editable: true,
    selectionModel: { type: 'cell' },
    colModel: colEmployeeGrid,
    dataModel: dataEmployeeGrid,   
}
$("#EmpPopupModalGrid").pqGrid(setpopupEmployeeGrid);


//$("#btnAdd").click(function () {

//    ClearForm();
//    CollapsePqGrid($("#EmpRvwSearchGrid"));  
//    loadEmpGrid();
//});
$("#btnPrint").click(function () {
    
     $.ajax({
         type: "GET",
         headers: {
             "__RequestVerificationToken": antiForgeryToken
         },
         data: { EmpId: selectedEmpforReview[0].EmpId},
         url: "/FirstReview/PrintRvwData",
         datatype: "Json",
         success: function (data) {
            
         },
         error: function (f, e, m) {
            
         }
     });

}); 

function loadEmpGrid() {
    $("#EmpPopupModal").dialog({
        height: 550,
        width: 600,
        modal: true,            
        open: function (evt, ui) {
          
            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            $.ajax({
                type: "GET",                
                headers: {
                    "__RequestVerificationToken": antiForgeryToken
                },
                url: "/FirstReview/GetEmpForReview",
                datatype: "Json",
                success: function (data) {                                      
                    $("#EmpPopupModalGrid").pqGrid("option", "dataModel.data", data.Employees);                    
                    $("#EmpPopupModalGrid").pqGrid("refreshDataAndView");
                 
                },
                error: function (f, e, m) {                    
                    $("#EmpPopupModalGrid").pqGrid("hideLoading");
                }
            });
        },
        close: function () {                       
            $("#EmpPopupModal").dialog("destroy");
        }
    })

}

//************** Review Grid ********************/
function CreateReviewGrid(Columns) {
    var dataReviewGrid = { location: "local", sorting: "local" };
    var colReviewGrid = [
        { title: "", dataIndx: "SubHeadId", dataType: "integer", hidden: true },
        { title: "", dataIndx: "PointGiven", dataType: "float", hidden: true },
        {
            title: "Descriptive Goals", dataIndx: "HeadName", width: 400, editable: false,
            render: function (ui) {
                if (ui.rowData.SubHeadId == 0) {
                    return {
                        //can also return attr (for attributes), cls (for css classes) and text (for plain or html string) properties.
                        style: 'font-size:13px;font-style:bold;'
                    };
                }               
            }

        }
      ];    
    for (var i = 0; i < Columns.length; i++) {
        colReviewGrid.push(Columns[i]);
    }
    
    var setReviewGrid = {
        width: '100%',
        height: 200,
        title: 'Employee Review',
        sortable: true,
        numberCell: { show: false },
        hoverMode: 'cell',
        showTop: false,
        resizable: true,
        scrollModel: { autoFit: true },
        draggable: false,
        wrap: false,
        editable: true,        
        selectionModel: { type: 'cell' },
        colModel: colReviewGrid,
        dataModel: dataReviewGrid,
        rowInit: function (ui) {
            if (ui.rowData.SubHeadId == 0) {
                return {
                    style: "background:yellow;" //can also return attr (for attributes) and cls (for css classes) properties.
                };
            }
            else {
                return { style: "text-align: center;" }
            }
        }
    }

    $("#ReviewGrid").pqGrid(setReviewGrid);
}

//**** Fecth Heads according  to employee Department ******/

$("#btnAddEmp").click(function () {
    let EmployeeData = $("#EmpPopupModalGrid").pqGrid("option", "dataModel.data");
    selectedEmpforReview = [];   

    EmployeeData = $.grep(EmployeeData, function (item) { return item.State == true });

    if (EmployeeData.length == 0) {
        ShowAlert('info', 'Please select atleast one Employee');
        return;
    }
    if (EmployeeData.length > 1) {
        ShowAlert('info', 'Please select only one Employee for review');
        return;
    }
    $('#FirstEmpReviewId').val(0);
    AddSelectedEmployeeData(EmployeeData[0]);
    GetHeads(0);
        
    
});

function AddSelectedEmployeeData(EmployeeData) {

    selectedEmpforReview.push({
        EmpId: EmployeeData.EmpId, EmpName: EmployeeData.EmpName, DeptId: EmployeeData.DeptId, DesgId: EmployeeData.DesignationId
        , Salary: EmployeeData.Salary, DesignationName: EmployeeData.DesignationName, JoiningDate: EmployeeData.StrJoiningDate, ConfirmationDate: EmployeeData.StrConfirmationDate
        , LastPromoDate: EmployeeData.StrLastPromoDate, AppraiserName: EmployeeData.AppraiserName, AppraiserDesigName: EmployeeData.AppraiserDesigName
        , AppraiserEmpId: EmployeeData.AppraiserEmpId, PreviousIncrement: EmployeeData.Components.PreviousIncrement, PreviousGross: EmployeeData.Components.PreviousGross,
        AppraiserDesgId: EmployeeData.AppraiserDesgId, QuestionnaireId: EmployeeData.QuestionnaireId
    });
}

function GetHeads(EmpReviewId) {

    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: "GET",
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        data: { QuestionnaireId: selectedEmpforReview[0].QuestionnaireId, EmpReviewId: EmpReviewId },
        url: "/FirstReview/GetHeads",
        datatype: "Json",
        success: function (data) {
            BindCommonFields();
            let Columns = CreateColumns(data.SubHeads[0].ReviewPoints);
            CreateReviewGrid(Columns);
            $("#ReviewGrid").pqGrid("hideLoading");
            $("#ReviewGrid").pqGrid("option", "dataModel.data", data.SubHeads);
            $("#ReviewGrid").pqGrid("refreshDataAndView");
            PqGridRefreshClick($("#ReviewGrid"));
            subheadCount = data.subheadCount;
            //if (EmpReviewId == 0) {
            //    //let CloseBtn = document.getElementsByClassName("ui-dialog-titlebar-close");
            //    //CloseBtn[0].click();             
            //    ClosePopupWindow("EmpPopupModal");
            //}
        },
        error: function (f, e, m) {
            $("#ReviewGrid").pqGrid("hideLoading");
        }
    });

}

function BindCommonFields() {
    BindEmpDtlsFields(selectedEmpforReview);
    //disableEmpDtlsFields();
    $('#ReviewGridDiv').show();
}

function BindEmpDtlsFields(selectedEmp) {
    $("#FirstName").val(selectedEmp[0].EmpName);
    $("#Salary").val(selectedEmp[0].Salary);
    $("#DesignationName").val(selectedEmp[0].DesignationName);
    $("#JoiningDate").val(selectedEmp[0].JoiningDate);
    $("#ConfirmationDate").val(selectedEmp[0].ConfirmationDate);
    $("#LastPromoDate").val(selectedEmp[0].LastPromoDate);
    $("#AppraiserName").val(selectedEmp[0].AppraiserName);
    $("#AppraiserDesigName").val(selectedEmp[0].AppraiserDesigName);   
}

function disableEmpDtlsFields() {
    $("#FirstName").attr("disabled", "disabled");
    $("#Salary").attr("disabled", "disabled");
    $("#DesignationName").attr("disabled", "disabled");
    $("#JoiningDate").attr("disabled", "disabled");
    $("#ConfirmationDate").attr("disabled", "disabled");
    $("#LastPromoDate").attr("disabled", "disabled");
    $("#AppraiserName").attr("disabled", "disabled");
    $("#AppraiserDesigName").attr("disabled", "disabled");
    $("#Grade").attr("disabled", "disabled");
}

function CreateColumns(reviewData) {
    let columns = [];

    for (var x = 0; x < reviewData.length; x++) {
        let reviewpoint = reviewData[x].Point;
        var column = {
            title: '' + reviewData[x].Name + '', sortable: false,align: "center",editable:false,
                        render: function (ui) {
                            let subheadid = ui.rowData.SubHeadId;
                            let checkedAttr = ui.rowData.PointGiven == reviewpoint ? "checked" : " ";
                            if (subheadid != 0) {
                                var renderrb = '<input type="radio"  '+checkedAttr+'   name="group_' + subheadid + '" value="' + reviewpoint + '" onchange="calulatePoints(' + ui.rowIndx + ' , ' + reviewpoint + ')">';
                                return renderrb;
                            }                            
                        }
                    }
        columns.push(column);
    }

    var RvwrViewCol = {
        title: "Appraiser 1 Review", dataIndx: "HeadsRevView", width: 400, align: "center"
    }
    columns.push(RvwrViewCol);

    return columns;       
}

function onRecommendChange(divId)
{
    ClearAllControl('RecommendationsDiv', true);
    $('.RecommendationsDiv').hide();    
    $('#' + divId + '').show();
}

function BindConclusionData(EmployeeData) {

    let recommdVal = EmployeeData.Recommendations.RecommendationValue;
    $("input[name=FinalOutcome][value=" + EmployeeData.OutcomeValue + "]").prop('checked', true);
    $("input[name=Recommendtion][value=" + recommdVal + "]").prop('checked', true);
    onRecommendChange(recommdVal);
    $("#TrainingNeeds").val(EmployeeData.Recommendations.TrainingNeeds);
    $("#ddlDesignation").val(EmployeeData.Recommendations.RecDesignationId);
    $("#RecSalary").val(EmployeeData.Recommendations.RecSalary);
    $("#RecIncrment").val(EmployeeData.Recommendations.RecIncrment);
    $("#Comments").val(EmployeeData.Comments);
    $("#GrdRemark").val(EmployeeData.GradeRemark);
}

/**** Points Calculation   ****/

function calulatePoints(rowIndx, points) {    
    $("#ReviewGrid").pqGrid('updateRow', { rowIndx: rowIndx, newRow: { 'PointGiven': points }, refresh:false });
    let reviewGridData = $("#ReviewGrid").pqGrid("option", "dataModel.data");
    let totalPoints =0.0;
    $.each(reviewGridData, function (key, value) {
        totalPoints = totalPoints +value.PointGiven;
    });
    sumOfPoints = totalPoints / subheadCount;
    let calPoints = Math.round(totalPoints / subheadCount);
    ShowGrade(calPoints)
}

function ShowGrade(calPoints) {
    let selectedGrade = $.grep(Grades, function (item) { return item.gradePoints == calPoints });
    $("#Grade").val(selectedGrade[0].gradeName);
    $("#GradeId").val(selectedGrade[0].gradeId);
}


/**** Save functionality ****/

$("#btnSave").click(function () {
    
    SubmitFlag = false;
    CreateReview();
});

$("#btnSubmit").click(function () {
    //ShowButtons(false);
    SubmitFlag = true;
    CreateReview();

});

$("#btnReset").click(function () {
    ClearForm();
    loadEmpRevSearchdata();
    ExpandPqGrid($("#EmpRvwSearchGrid"));
});

function CreateReview() {
    if (Validate()) {
        ShowButtons(false);
        try {
            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            $.ajax({
                type: "POST",
                traditional: true,
                headers: {
                    "__RequestVerificationToken": antiForgeryToken
                },
                contentType: 'application/json; charset=utf-8',
                url: '/FirstReview/CreateReview', // Controller/View
                data: reviewJson(),
                success: function (res) {
                    if (res.success) {
                        ClearForm();
                        ShowAlert("success", res.message);
                        loadEmpRevSearchdata();
                        ExpandPqGrid($("#EmpRvwSearchGrid"));
                    }
                },
                error: function (a, b, c) {
    
                },                
                complete: function (data) {
                    ShowButtons(true);
                }
            });
        } catch (err) {
        }
    }
}

function Validate() {

    let reviewGridData = $("#ReviewGrid").pqGrid("option", "dataModel.data");
    let headNotSel = $.grep(reviewGridData, function (item) { return (item.PointGiven == 0 && item.SubHeadId != 0) });
    let headRvw = $.grep(reviewGridData, function (item) { return (jQuery.trim(item.HeadsRevView).length === 0 && item.SubHeadId != 0) });

    //if (headRvw.length > 0) { ShowAlert('warning', 'Kindly give Appraiser 1 review for all Descriptive Goals'); return; }
    if (headNotSel.length > 0) { ShowAlert('warning', 'Kindly Select All Descriptive Goals and Proceed'); return; }
    if (!showAlertOnBlank($("#GrdRemark"), "Kindly give grade remarks")) { $("#GrdRemark").focus(); return; }

    if (valRadioBtn(FinalOutcome) == null) { ShowAlert('warning', 'Kindly Select Final Outcome');  return }
    let Recommendval =  valRadioBtn(Recommendtion) 
    if (Recommendval == null) { ShowAlert('warning', 'Kindly give Recommendations '); return }
    else {
        if (Recommendval == "1") {
            if (!showAlertOnBlank($("#TrainingNeeds"), "Kindly Suggest Training Needs")) { $("#TrainingNeeds").focus(); return; }
        }
        else if (Recommendval == "2") {
            if (!showAlertOnBlank($("#ddlDesignation"), "Kindly Recommend Designation")) { $("#ddlDesignation").focus(); return; }
            if (!showAlertOnBlank($("#RecSalary"), "Kindly Recommend Salary ")) { $("#RecSalary").focus(); return; }
        }
        else {            
            if (!showAlertOnBlank($("#RecIncrment"), "Kindly Recommend Increment")) { $("#RecIncrment").focus(); return; }
        }
    }
    if (!showAlertOnBlank($("#Comments"), "Kindly give Comments ")) { $("#Comments").focus(); return; }
    

    return true;
}

function reviewJson() {

    let reviewGridData = $("#ReviewGrid").pqGrid("option", "dataModel.data");
    reviewGridData = $.grep(reviewGridData, function (item) { return item.SubHeadId != 0 })

    let Recommendations = {
        RecommendationValue: valRadioBtn(Recommendtion),
        TrainingNeeds: $("#TrainingNeeds").val(),
        RecDesignationId: $("#ddlDesignation").val(),
        RecSalary: $("#RecSalary").val(),
        RecIncrment: $("#RecIncrment").val()
    };

    let newDesgID = $("#ddlDesignation").val() == "" || $("#ddlDesignation").val() == 0 ? selectedEmpforReview[0].DesgId : $("#ddlDesignation").val();
    let salaryComponents = {
        PreviousGross: selectedEmpforReview[0].PreviousGross,
        PreviousIncrement: selectedEmpforReview[0].PreviousIncrement,
        StrPreviousDate: selectedEmpforReview[0].LastPromoDate,
        LastGross: $("#Salary").val(),
        LastDesgId: selectedEmpforReview[0].DesgId,
        NewDesgID: newDesgID,
        LastDeptId: selectedEmpforReview[0].DeptId
    }
  
    var jdata = JSON.stringify({
        EmpReviewId : $('#FirstEmpReviewId').val(),
        EmpId: selectedEmpforReview[0].EmpId,
        AppraiserEmpId :selectedEmpforReview[0].AppraiserEmpId,
        HeadList: reviewGridData,
        OutcomeValue: valRadioBtn(FinalOutcome),
        Recommendations: Recommendations,
        Comments: $("#Comments").val(),
        GradeId: $("#GradeId").val(),
        GradeRemark : $("#GrdRemark").val(),
        IsSubmit: SubmitFlag,
        GradeScore: sumOfPoints,
        Components: salaryComponents,
        AppraiserDesgId: selectedEmpforReview[0].AppraiserDesgId,
        QuestionnaireId: selectedEmpforReview[0].QuestionnaireId
    })

    return jdata;
}


/*** Clear All fields*****/
function ClearForm() {
     SubmitFlag = false;
     selectedEmpforReview = []; 
     subheadCount = 0 
     ClearAllControl('ConclusionDiv', true);     
     $('.RecommendationsDiv').hide();
     $("input[name=FinalOutcome]").prop('checked', false);
     $("input[name=Recommendtion]").prop('checked', false);
     $("#FirstEmpReviewId").val('');
     $("#FirstName").val('');
     $("#Salary").val('');
     $("#DesignationName").val('');
     $("#JoiningDate").val('');
     $("#ConfirmationDate").val('');
     $("#LastPromoDate").val('');
     $("#AppraiserName").val('');
     $("#AppraiserDesigName").val('');
     $("#GradeId").val('');
     $('#ReviewGridDiv').hide();
     $("#Grade").val('');
     $("#GrdRemark").val('');
     sumOfPoints = 0.0;
     //$("#EmpPopupModalGrid").pqGrid("reset", { filter: true });
}

function ShowButtons(IsVisible) {
    
    document.getElementById('btnSave').style.visibility = IsVisible ? 'visible' :'hidden'  ;
    document.getElementById('btnSubmit').style.visibility = IsVisible ? 'visible' : 'hidden' ;
}



