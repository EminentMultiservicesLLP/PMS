var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
var Grades = [];
$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/CommonMaster/GetAllGrade",
    success: function (response) {
        Grades = [];
        $.each(response.data, function (index, value) {
            Grades.push({
                gradeId: value.GradeId, gradeName: value.GradeName, gradePoints: value.GradePoints
            })
        });
    }
});


$.ajax({
    type: "GET",
    headers: {
        "__RequestVerificationToken": antiForgeryToken
    },
    traditional: true,
    url: "/CommonMaster/GetAllDesignation",
    success: function (response) {
        $('#ddlDesignation').val("");
        //$('#ddlDesignation').html("");
        //$('#ddlDesignation').append('<option value="0">Select</option>');
        $.each(response.data, function (index, value) {
            $('#ddlDesignation').append('<option value="' + value.DesignationId + '">' + value.DesignationName + '</option>');
        });
        
        
    }
});