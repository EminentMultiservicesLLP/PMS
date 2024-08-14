
OnPageLoad();
function OnPageLoad() {
    document.getElementById('btnApprRpt').style.visibility = 'hidden';
$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/CommonMaster/GetAllOutlet",
    success: function (response) {
        $('#ddlOutlet').val("");
        $.each(response.data, function (index, value) {
            $('#ddlOutlet').append('<option value="' + value.OutletId + '">' + value.OutletName + '</option>');
        });
    }
});

$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/CommonMaster/GetAllGrade",
    success: function (response) {
        $('#ddlGrade').val("");
        var getList = $.grep(response.data, function (item) { return item.GradeId != 1 });
        $.each(getList, function (index, value) {
            $('#ddlGrade').append('<option value="' + value.GradeId + '">' + value.GradeName + '</option>');
        });
    }
});


$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    dataType: "json",
    url: "/ReviewReport/GetAllEmployee",
    success: function (response) {

        document.getElementById('btnApprRpt').style.visibility = response.Role == 4 ? 'visible' : 'hidden';
        
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
            select: SelectedEmployee,
            change: SelectedEmployee
        })
    }
});

}

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
$("#btnShow").click(function () {
    GetReviewData(1);
})

$("#btnReset").click(function () {
    ClearReportForm();
})

$("#btnApprRpt").click(function () {
    $("#iframeReviewReportsViewer").contents().find("body").html(""); //Clearing content on fresh load    
    GetReviewResult(2);
})

$("#btnStatus").click(function () {
    $("#iframeReviewReportsViewer").contents().find("body").html(""); //Clearing content on fresh load    
    GetReviewResult(3);
})



function GetReviewData(RptType) {
    $("#iframeReviewReportsViewer").contents().find("body").html(""); //Clearing content on fresh load    
    GetReviewResult(RptType);
}

function GetReviewResult(RptType) {
    try {

        let rvwtype = $('#ddlRvwType').val() == "" ? 0 : $('#ddlRvwType').val();
        let outletId = $('#ddlOutlet').val() == "" ? 0 : $('#ddlOutlet').val();
        let gradeId = $('#ddlGrade').val() == "" ? 0 : $('#ddlGrade').val();
        let desgId = $('#ddlDesignation').val() == "" ? 0 : $('#ddlDesignation').val();
        let empId = $("#EmpId").val() == "" ? 0 : $("#EmpId").val();
        
        $("#ReviewReportsModal").dialog({
            cache: false,
            position: {
                my: "center",
                at: "center",
                of: window
            },
            height: 600,
            width: 800,
            open: function (evt, ui) {

            },
            close: function () {
                $("#ReviewReportsModal").dialog("destroy");
            }
        })
        var url = "";
        debugger;
        url = "../../Reports/ReportViewer.aspx?reportid="+RptType + "&otherParam=" + rvwtype + "," + outletId + "," + gradeId + "," + desgId + "," + empId;

        console.log(url);
        var myframe = document.getElementById("iframeReviewReportsViewer");
        if (myframe != null) {
            $("#iframeReviewReportsViewer").empty(); //Clearing content on fresh load
            if (myframe.src) {
                myframe.src = url;
            } else if (myframe.contentWindow != null && myframe.contentWindow.location != null) {
                myframe.contentWindow.location = url;
            } else {
                myframe.setAttribute('src', url);
            }
            return false;
        }
    } catch (err) {
    }
}

function ClearReportForm() {
    $('#ddlRvwType').val('');
    $('#ddlOutlet').val('');
    $('#ddlGrade').val('');
    $('#ddlDesignation').val('');
    $('#autocomplete_Employee').val('');
    $("#EmpId").val("");
}


