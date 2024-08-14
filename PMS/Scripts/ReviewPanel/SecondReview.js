var selEmpforReview = [];
var SubmitSecFlag = false;
var subheadCnt = 0;
var sumOfRvwPoints = 0.0;


loadSecRvwSearchGrid()
/***** Search Grid  *****/
var dataSecRvwSearchGrid = { location: "local", sorting: "local" };
var colSecRvwSearchGrid = [
      { title: "", dataIndx: "EmpReviewId", dataType: "integer", hidden: true },
      { title: "Emp Id", dataIndx: "EmpCode", editable: false, width: 70, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Name", dataIndx: "EmpName", editable: false, width: 400, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Designation", dataIndx: "DesignationName", editable: false, width: 200, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Grade", dataIndx: "GradeName", editable: false, width: 200, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } },
      { title: "Appraiser 2 Status", dataIndx: "ApprTwoStatus", editable: false, width: 200, filter: { type: 'textbox', condition: 'contain', listeners: ['keyup'] } }
];

var setSecRvwSearchGrid = gridCommonObject;
setSecRvwSearchGrid.width = '100%';
setSecRvwSearchGrid.height = 500;
setSecRvwSearchGrid.colModel = colSecRvwSearchGrid;
setSecRvwSearchGrid.dataModel = dataSecRvwSearchGrid;
setSecRvwSearchGrid.title = 'Reviewed Employess';
setSecRvwSearchGrid.rowClick = function (evt, ui) {
    if (ui.rowData) {
          selEmpforReview = [];
          var selEmployee = ui.rowData;
  
          $('#EmpFinalReviewId').val(selEmployee.EmpFinalReviewId);
          $('#SecEmpReviewId').val(selEmployee.EmpReviewId);
          sumOfRvwPoints = selEmployee.GradeScore;
          AddSelEmpData(selEmployee);          
          GetSecHeads(selEmployee.EmpReviewId);
          CollapsePqGrid($("#SecRvwSearchGrid"));
          ShowSecGrade(selEmployee.GradePoints);
          BindSecConclusionData(selEmployee);
          ShwBtns(selEmployee.IsShwBtn);
    }
};
$("#SecRvwSearchGrid").pqGrid(setSecRvwSearchGrid);



function loadSecRvwSearchGrid() {
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: "GET",
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },      
        url: "/SecondReview/GetRvwdEmps",
        beforeSend: function () {
            $("#divPartialLoading").show();
        },
        datatype: "Json",
        success: function (data) {

            $("#SecRvwSearchGrid").pqGrid("hideLoading");
            $("#SecRvwSearchGrid").pqGrid("option", "dataModel.data", data.rvdEmployees);
            $("#SecRvwSearchGrid").pqGrid("refreshDataAndView");
            PqGridRefreshClick($("#SecRvwSearchGrid"));
            $("#divPartialLoading").hide();
        },
        error: function (f, e, m) {
           
        }
    });
}

function AddSelEmpData(EmployeeData) {
    selEmpforReview.push({
        EmpId: EmployeeData.EmpId, EmpName: EmployeeData.EmpName, DeptId: EmployeeData.DeptId, DesgId: EmployeeData.DesignationId
        , Salary: EmployeeData.Salary, DesignationName: EmployeeData.DesignationName, JoiningDate: EmployeeData.StrJoiningDate, ConfirmationDate: EmployeeData.StrConfirmationDate
        , LastPromoDate: EmployeeData.StrLastPromoDate, AppraiserName: EmployeeData.AppraiserName, AppraiserDesigName: EmployeeData.AppraiserDesigName
        , AppraiserEmpId: EmployeeData.AppraiserEmpId, AppraiserTwoEmpId: EmployeeData.AppraiserTwoEmpId, AppraiserDesgId: EmployeeData.AppraiserDesgId
        , QuestionnaireId: EmployeeData.QuestionnaireId
    });
}

function GetSecHeads(EmpReviewId) {

    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: "GET",
        headers: {
            "__RequestVerificationToken": antiForgeryToken
        },
        data: { QuestionnaireId: selEmpforReview[0].QuestionnaireId, EmpReviewId: EmpReviewId },
        url: "/FirstReview/GetHeads",
        datatype: "Json",
        success: function (data) {          
            BindComFields();
            let Columns = CreateCol(data.SubHeads[0].ReviewPoints);
            CreateReviewGrid(Columns);
            $("#SecReviewGrid").pqGrid("hideLoading");
            $("#SecReviewGrid").pqGrid("option", "dataModel.data", data.SubHeads);
            $("#SecReviewGrid").pqGrid("refreshDataAndView");
            subheadCnt = data.subheadCount;
            PqGridRefreshClick($("#SecReviewGrid"));
        },
        error: function (f, e, m) {
            $("#SecReviewGrid").pqGrid("hideLoading");
        }
    });

}



function BindComFields() {
    BindEmpDtlsFlds(selEmpforReview);
    //disableEmpDtlsFlds();
    $('#SecRevGridDiv').show();
}

function BindEmpDtlsFlds(selectedEmp) {
    $("#SecFirstName").val(selectedEmp[0].EmpName);
    $("#SecSalary").val(selectedEmp[0].Salary);
    $("#SecDesignationName").val(selectedEmp[0].DesignationName);
    $("#SecJoiningDate").val(selectedEmp[0].JoiningDate);
    $("#SecConfirmationDate").val(selectedEmp[0].ConfirmationDate);
    $("#SecLastPromoDate").val(selectedEmp[0].LastPromoDate);
    $("#SecAppraiserName").val(selectedEmp[0].AppraiserName);
    $("#SecAppraiserDesigName").val(selectedEmp[0].AppraiserDesigName);
}


function CreateCol(reviewData) {
    let columns = [];

    for (var x = 0; x < reviewData.length; x++) {
        let reviewpoint = reviewData[x].Point;
        var column = {
            title: '' + reviewData[x].Name + '', sortable: false, align: "center", editable: false,
            render: function (ui) {
                let subheadid = ui.rowData.SubHeadId;
                let checkedAttr = ui.rowData.PointGiven == reviewpoint ? "checked" : " ";
                if (subheadid != 0) {
                    var renderrb = '<input type="radio"  ' + checkedAttr + '    name="group_' + subheadid + '" value="' + reviewpoint + '" onchange="calulateRPoints(' + ui.rowIndx + ' , ' + reviewpoint + ')">';
                    return renderrb;
                }
            }
        }
        columns.push(column);
    }
    var RvrViewCol = {
        title: "Appraiser 1 Review", dataIndx: "HeadsRevView", width: 400, align: "center", editable: false
    }
    var ManagerViewCol = {
        title: "Manager Review", dataIndx: "HeadsManView", width: 400, align: "center"
    }
    columns.push(RvrViewCol);
    columns.push(ManagerViewCol);
    return columns;
}

function CreateReviewGrid(Columns) {
    var dataRevGrid = { location: "local", sorting: "local" };
   
    var colRevGrid = [
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
        colRevGrid.push(Columns[i]);
    } 

    var setRevGrid = {
        width: '100%',
        height: 200,
        title: 'Employee Review',
        sortable: false,
        numberCell: { show: false },
        hoverMode: 'cell',
        showTop: false,
        resizable: true,
        scrollModel: { autoFit: true },
        draggable: false,
        wrap: false,
        editable: true,
        selectionModel: { type: 'cell' },
        colModel: colRevGrid,
        dataModel: dataRevGrid,
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

    $("#SecReviewGrid").pqGrid(setRevGrid);
}

function onSecRecommendChange(divId) {
    ClearAllControl('RecommendationsDiv', true);
    $('.RecommendationsDiv').hide();
    $('#' + divId + '').show();
}





function BindSecConclusionData(EmployeeData) {

    let recommdVal = EmployeeData.Recommendations.RecommendationValue;
    $("input[name=FinalOutcome][value=" + EmployeeData.OutcomeValue + "]").prop('checked', true);
    $("input[name=Recommendtion][value=" + recommdVal + "]").prop('checked', true);
    onSecRecommendChange(recommdVal);
    $("#TrainingNeeds").val(EmployeeData.Recommendations.TrainingNeeds);
    $("#ddlDesignation").val(EmployeeData.Recommendations.RecDesignationId);
    $("#RecSalary").val(EmployeeData.Recommendations.RecSalary);
    $("#RecIncrment").val(EmployeeData.Recommendations.RecIncrment);
    $("#Comments").val(EmployeeData.Comments);
    $("#OutComeManView").val(EmployeeData.OutComeManView);
    $("#RecommendManView").val(EmployeeData.Recommendations.RecManView);
    $("#ManagerComm").val(EmployeeData.AppraiserTwoComments);
    $("#GrdRemark").val(EmployeeData.GradeRemark);
}

/**** Points Calculation   ****/

function calulateRPoints(rowIndx, points) {
    $("#SecReviewGrid").pqGrid('updateRow', { rowIndx: rowIndx, newRow: { 'PointGiven': points }, refresh: false });
    let reviewGridData = $("#SecReviewGrid").pqGrid("option", "dataModel.data");
    let totalPoints = 0.0;
    $.each(reviewGridData, function (key, value) {
        totalPoints = totalPoints + value.PointGiven;
    });
    sumOfRvwPoints = totalPoints / subheadCnt;
    let calPoints = Math.round(totalPoints / subheadCnt);
    ShowSecGrade(calPoints)
}

function ShowSecGrade(calPoints) {
    let selectedGrade = $.grep(Grades, function (item) { return item.gradePoints == calPoints });
    $("#Grade").val(selectedGrade[0].gradeName);
    $("#GradeId").val(selectedGrade[0].gradeId);
}




$("#btnSave").click(function () {
    
    SubmitSecFlag = false;
    CreateSecReview();
});


$("#btnSubmit").click(function () {
    //ShwBtns(false);
    SubmitSecFlag = true;
    CreateSecReview();
});

$("#btnReset").click(function () {
    ClearSecForm();
    loadSecRvwSearchGrid();
    ExpandPqGrid($("#EmpRvwSearchGrid"));
});

function CreateSecReview() {    
    if (RvwValidate()) {
        ShwBtns(false);
        try {
            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            $.ajax({
                type: "POST",
                traditional: true,
                headers: {
                    "__RequestVerificationToken": antiForgeryToken
                },
                contentType: 'application/json; charset=utf-8',
                url: '/SecondReview/CreateFinalReview', // Controller/View
                data: reviewJson(),
                success: function (res) {
                    if (res.success) {
                        ClearSecForm();
                        ShowAlert("success", res.message);
                        loadSecRvwSearchGrid();
                        ExpandPqGrid($("#SecRvwSearchGrid"));
                    }
                },
                error: function (a, b, c) {

                },               
                complete: function (data) {
                    ShwBtns(true);
                }
            });
        } catch (err) {
        }
    }
}

function RvwValidate() {
    let reviewGridData = $("#SecReviewGrid").pqGrid("option", "dataModel.data");
    let headRvw = $.grep(reviewGridData, function (item) { return (jQuery.trim(item.HeadsManView).length === 0 && item.SubHeadId != 0) });
    //if (headRvw.length > 0) { ShowAlert('warning', 'Kindly give Manager review for all Descriptive Goals'); return; }
    if (!showAlertOnBlank($("#GrdRemark"), "Kindly give grade remarks")) { $("#GrdRemark").focus(); return };

    if (valRadioBtn(FinalOutcome) == null) { ShowAlert('warning', 'Kindly Select Final Outcome'); return }
    let Recommendval = valRadioBtn(Recommendtion)
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
    if (!showAlertOnBlank($("#OutComeManView"), "Kindly enter Manager views on outcome ")) { $("#OutComeManView").focus(); return; }
    if (!showAlertOnBlank($("#RecommendManView"), "Kindly enter Recommendation Manager view ")) { $("#RecommendManView").focus(); return; }
    if (!showAlertOnBlank($("#ManagerComm"), "Kindly give Manager Comments ")) { $("#ManagerComm").focus(); return; }
    return true;
}

function reviewJson() {

    let reviewGridData = $("#SecReviewGrid").pqGrid("option", "dataModel.data");
    reviewGridData = $.grep(reviewGridData, function (item) { return item.SubHeadId != 0 })
    let recommendations = {
        RecommendationValue: valRadioBtn(Recommendtion),
        TrainingNeeds: $("#TrainingNeeds").val(),
        RecDesignationId: $("#ddlDesignation").val(),
        RecSalary: $("#RecSalary").val(),
        RecIncrment: $("#RecIncrment").val(),
        RecManView: $("#RecommendManView").val()
    };
    
    let newDesgID = $("#ddlDesignation").val() == "" || $("#ddlDesignation").val() == 0 ? selEmpforReview[0].DesgId : $("#ddlDesignation").val();
    let salaryComponents = {     
        LastGross: $("#SecSalary").val(),        
        NewDesgID : newDesgID
    }
    var jdata = JSON.stringify({
        EmpFinalReviewId: $('#EmpFinalReviewId').val(),
        EmpReviewId: $('#SecEmpReviewId').val(),
        EmpId: selEmpforReview[0].EmpId,
        AppraiserTwoEmpId: selEmpforReview[0].AppraiserTwoEmpId,
        HeadList: reviewGridData,
        OutcomeValue: valRadioBtn(FinalOutcome),
        Recommendations: recommendations,
        OutComeManView: $("#OutComeManView").val(),       
        AppraiserTwoComments: $("#ManagerComm").val(),
        GradeId: $("#GradeId").val(),
        IsSubmit: SubmitSecFlag,
        GradeRemark: $("#GrdRemark").val(),
        GradeScore: sumOfRvwPoints,
        Components: salaryComponents,
        AppraiserDesgId: selEmpforReview[0].AppraiserDesgId
    })

    return jdata;
}

function ClearSecForm() {
    SubmitSecFlag = false;
    selEmpforReview = [];
    subheadCnt = 0;
    ClearAllControl('ConclusionSecDiv', true);
    $('.RecommendationsDiv').hide();
    $("input[name=FinalOutcome]").prop('checked', false);
    $("input[name=Recommendtion]").prop('checked', false);
    $("#EmpFinalReviewId").val('');
    $("#SecEmpReviewId").val('');
    $("#GradeId").val('');
    $("#SecFirstName").val('');
    $("#SecSalary").val('');
    $("#SecDesignationName").val('');
    $("#SecJoiningDate").val('');
    $("#SecConfirmationDate").val('');
    $("#SecLastPromoDate").val('');
    $("#SecAppraiserName").val('');
    $("#SecAppraiserDesigName").val('');    
    $('#SecRevGridDiv').hide();
    $("#OutComeManView").val('');
    $("#RecommendManView").val('');
    $("#ManagerComm").val('');
    $("#Grade").val('');
    $("#GrdRemark").val('');
    sumOfRvwPoints = 0.0;
}

function ShwBtns(IsVisible) {    
    document.getElementById('btnSave').style.visibility = IsVisible ? 'visible' : 'hidden';
    document.getElementById('btnSubmit').style.visibility = IsVisible ? 'visible' : 'hidden';
}
