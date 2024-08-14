
//$('input[name="date"]').mask('00/00/0000');
//$('input[name="PaperHours"]').mask('00:00');
//$('input[name="postal-code"]').mask('000 000');
//$('input[name="phone-number"]').mask('(000) 000 0000');
//$('input[name="number"]').mask('0*', { 'translation': { 0: { pattern: /[0-9*]/ } } });
var mailformat = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
var letterNumber = /^[0-9a-zA-Z]+$/;
function clearTabSelection(curElement) {
    var parentUl = curElement.parent('li').closest('ul');
    parentUl.find('li').each(function (i, item) {
        $(this).removeClass('active');
    });
    curElement.closest("li").addClass('active');
}



var gridDateEditor = function(ui) {
    var $cell = ui.$cell,
        rowData = ui.rowData,
        dataIndx = ui.dataIndx,
        cls = ui.cls,
        dc = $.trim(rowData[dataIndx]);
    $cell.css('padding', '0');

    var $inp = $("<input type='text' id='" +
            dataIndx +
            "' name='" +
            dataIndx +
            "' class='" +
            cls +
            " pq-date-editor' style='position: relative; z-index: 100;border: 3px solid rgba(0,0,0,0);'/>")
        .appendTo($cell)
        .val(dc)
        .datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd/mm/yy",
            onClose: function() {
                $inp.focus();
            }
        });
    window.setTimeout(function() {
            var dc = $.trim(ui.column.render(ui));
            $inp.val(dc);
        },
        0);
};



function ShowAlert(type, message)
{
    //type = 'warning', 'info', 'success', 'error';
    Lobibox.alert(type,
    {
        msg: message
    });
}

function ConfirmMessage(message)
{
    var lobibox = Lobibox.confirm({
        msg: message
    });

    return lobibox;
}

function ShowProgress(title, message)
{
    Lobibox.progress({
        title: title,
        label: message,
        onShow: function ($this) {
            var i = 0;
            var inter = setInterval(function () {
                if (i > 100) {
                    inter = clearInterval(inter);
                }
                i = i + 0.1;
                $this.setProgress(i);
            }, 10);
        }
    });
}

//type = 'warning', 'info', 'success', 'error';
function Notify(title, type, message)
{
    Lobibox.notify(type, {
        title: title,
        msg: message,
        sound: false,
        delay: false,
    });
}

function Notify(title, type, message, delay) {
    Lobibox.notify(type, {
        title: title,
        msg: message,
        sound: false,
        delay: delay,
    });
}

function ClearParamGrid(gridName) {
    var gr = $("#" + gridName);
    if (gr != undefined) {
        gr.pqGrid("option", "dataModel.data", { data: [] });
        gr.pqGrid("refresh");
        gr.pqGrid("refreshDataAndView");
    }
}
function ClearParamGridByObject(grid) {
    if (grid != null && grid != undefined) {
        grid.pqGrid("option", "dataModel.data", []);
        grid.pqGrid("refreshDataAndView");
    }
}

function ClosePopupWindow(WindowName) {
    var popupWindow = $("#" + WindowName);
    if(popupWindow != undefined && popupWindow != null)
        popupWindow.dialog('close');
}


var exportToExcelToolbar = {
                items: [
                {
                    type: 'select',
                    label: 'Export To: ',
                    attr: 'id="export_format"',
                    options: [{ csv: 'Csv', htm: 'Html', json: 'Json' }]
                },
                {
                    type: 'button',
                    label: "Export",
                    icon: 'ui-icon-arrowthickstop-1-s',
                    attr: 'style="margin:0"',
                    listener: function () {
                        var format = $("#export_format").val(),
                            blob = this.exportData({
                                format: format,
                                render: true
                            });
                        if (typeof blob === "string") {
                            blob = new Blob([blob]);
                        }
                        saveAs(blob, "pqGrid." + format);
                    }
                }]
};

var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
var newDate = new Date();
var d = newDate.getDate();
var m = monthNames[newDate.getMonth()];
var y = newDate.getFullYear();
var today = d + '-' + m + '-' + y;

function ResetDate(datepicker) {
    var datetpickercontrol = $("#" + datepicker);
    if (datetpickercontrol != undefined) {
        datetpickercontrol.data({ date: today });
        datetpickercontrol.datetimepicker('update');
        datetpickercontrol.datetimepicker().children('input').val(today);
    }
}

function SaveScandoc(file, NewId, location, sublocation) {
    var filecontrol = $("#" + file);
    var fdata = new FormData();
    var files = filecontrol.get(0).files;
    for (i = 0; i < files.length; i++) {
        fdata.append("files" + i, files[i]);
    }
    fdata.append("NewId", NewId);
    fdata.append("AreaLocation", location);
    fdata.append("SubAreaLocation", sublocation);

    if (files.length > 0) {
        $.ajax({
            type: 'POST',
            url: '/FileUploadDownload/Upload',
            data: fdata,
            contentType: false,
            processData: false,
            success: function (data) {
                //alert("Successfully Uploaded!");
            },
            error: function () {
                alert("An error occurred!");
            },
        });
    }
}

function DisplayUploadedImages(objectName, filePath) {
   
    var FileDetail = filePath.split('\\');
    FileDetail.push(filePath);
    //debugger;
    var newImageLI = '<li>';
    newImageLI = newImageLI +
        '<a class="title" id="' +
       parseInt(FileDetail[2]) +
        '" href= "' +
        appSetting +
        FileDetail[4] +
        '" target="_blank"> ' +
        FileDetail[3]+
        '</a>';
    newImageLI = newImageLI +
        '<a href="javascript:void(0);" data-id="' +
        parseInt(FileDetail[2]) +
        '"  class="deleteItem" onclick="Deletefile(' +
        parseInt(FileDetail[2]) +
        ');return false;"></a>';
    newImageLI = newImageLI + "</li>";

    $("#" + objectName).append(newImageLI);
}
function InsertDefaultSelect(objName) {
    if (objName != undefined) {
        if ($(objName).is("select")) {
            var ddl = $(objName);
            if (ddl.has("option").length > 0)
                ddl.children("option").eq(1).before($("<option></option>").val("").text("-- Select from List --"));
            else
                ddl.append("<option value=\"\">-- Select from List --</option>");
        }
    }
}

function CollapsePqGrid(grid) {
    $(".pq-slider-icon .ui-icon-circle-triangle-n", grid).click();
}
function ExpandPqGrid(grid) {
    $(".pq-slider-icon .ui-icon-circle-triangle-s", grid).click();
}

function TogglePqGridFullSize(grid) {
    $(".pq-slider-icon .ui-icon", grid).click();
}

function DisableClick(buttonName) {
    var btn = "#" + buttonName;
    $(btn).attr('disabled', true);
    var t = setTimeout(function () { $(btn).attr('disabled', false); }, 10000);
}

function EnableClicked(button) {
    $(button).attr('disabled', false);
}

function ServerJsonDateFormat(jsonDate) {
    var regex = /-?\d+/;
    var newDate = regex.exec(jsonDate);
    var dateOriginal = new Date(parseInt(newDate[0]));

    return dateOriginal;
}
function ToJavaScriptDate(value) { //To Parse Date from the Returned Parsed Date
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function GridRenderDate (ui) {
    if (ui.cellData==null)
    {
        return '';
    }
    else {
        var res = ui.cellData.split("/");
        var date = new Date(res[1]+"/"+res[0]+"/"+res[2]);
        var xdd = date.getDate();
        var xmm = (date.getMonth() + 1);
        var xyy = date.getFullYear();
							
        if (parseInt(xdd)<10){xdd='0'+xdd;}
        if (parseInt(xmm)<10){xmm='0'+xmm;}
							
        return xdd + '/' + xmm + '/' +  xyy;
    }
}

var loadingDiv =
    "<div id='divLoading' style='padding: 0px; position: fixed; width: 80%; height: 100%; z-index: 30001; opacity: 0.8;'><img src='../Images/gif-load.gif' /></p></div>'";

var ajaxObj = {
    dataType: "JSON",
    beforeSend: function () {
        this.pqGrid("showLoading");
    },
    complete: function () {
        this.pqGrid("hideLoading");
    },
    error: function () {
        this.pqGrid("hideLoading");
        this.pqGrid("rollback");
    }
};



function ClearModalControl(elementName, isClass) {
    var eltname = (isClass == true ? "." : "#") + elementName;
    var allInputs = $(eltname).find(':input');
    if (allInputs != null && allInputs.length > 0) {
        allInputs.each(function (index, obj) {
            if (obj.type === "text" || obj.type === "select-one" || obj.type === "select" || obj.type === "textarea" || obj.type === "hidden")
                obj.value = "";
            else if (obj.type === "checkbox")
                obj.check = false;
        });
    }

    var allGrid = $(eltname).find('.pq-grid');
    if (allGrid != null && allGrid.length > 0) {
        allGrid.each(function (index, obj) {
            ClearParamGrid(obj.id);
        });
    }

}


function ClearAllControl(elementName , isclass) {
    var eltname = (isclass == true ? "." : "#") + elementName;
    //var eltname =  "#" + elementName;
        var allInputs = $(eltname).find(':input');
        if (allInputs != null && allInputs.length > 0) {
            allInputs.each(function (index, obj) {
                if (obj.type === "text" || obj.type === "select-one" || obj.type === "select" || obj.type === "textarea" || obj.type === "hidden")
                    obj.value = "";
                else if (obj.type === "checkbox")
                    obj.check = false;
            });
        }

        var allGrid = $(eltname).find('.pq-grid');
        if (allGrid != null && allGrid.length > 0) {
            debugger;
            allGrid.each(function (index, obj) {
                    ClearParamGrid(obj.id);
            });
    }

}

function ClearAllControlSkip(elementName, skiplist) {
    if (skiplist.length > 0) {
        var eltname = "#" + elementName;

        var allTextInputs = $(eltname).find(':input');
        if (allTextInputs != null && allTextInputs.length > 0) {
            allTextInputs.each(function (index, obj) {
                if (jQuery.inArray(obj.id, skiplist) === -1) {
                    if (obj.type === "text" ||
                        obj.type === "select-one" ||
                        obj.type === "select" ||
                        obj.type === "textarea" || obj.type === "hidden")
                        obj.value = "";
                    else if (obj.type === "checkbox")
                        obj.check = false;
                }
            });
        }

        var allGrid = $(eltname).find('.pq-grid');
        if (allGrid != null && allGrid.length > 0) {
            allGrid.each(function (index, obj) {
                if (jQuery.inArray(obj.id, skiplist) === -1)
                    ClearParamGrid(obj.id);
            });
        }
    }

}

function restrictSpecialChar(code,name) {
    if (/^[a-zA-Z0-9- ]*$/.test(code, name) === false)
    ShowAlert("warning", "Charecter not allowed !!");
}


function isNullOrWhitespace(input) {
    if ((jQuery.trim(input)).length === 0) {
        ShowAlert("warning", "Standard Name is missing !!");
        return true;
    }
    else
        return false;
}

function showAlertOnBlank(input, alertMessage) {
    if (typeof input === "object") {
        input = input.val();
    }

    if (input !== undefined && jQuery.trim(input).length === 0) {
        ShowAlert("warning", alertMessage);
        return false;
    }
    else
        return true;
}

function ajaxCall(url, params, timeOut, type, ajxSuccessFn, ajxFailFn, showLoading) {
    timeOut = timeOut === "" || timeOut === "undefined" || timeOut === null ? 6000 : timeOut;
    type = type.toUpperCase() !== "GET" && type.toUpperCase() !== "POST" && type.toUpperCase() !== "PUT" && type.toUpperCase() !== "DELETE" ? "POST" : type;
    $.ajax({
        type: type.toUpperCase(),
        cache: false,
        url: url,
        data: params,
        //beforeSend: function () {
            
        //    if (showLoading) {
        //        $('#divLoading').show();
        //    }
        //},
        success: function (result) {
            if (typeof ajxSuccessFn === 'function') {
                var successFn = partial(ajxSuccessFn, result);
                successFn();
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            if (typeof ajxFailFn === 'function') {
                var errorFn = partial(ajxFailFn, xhr, thrownError);
                errorFn();
            }
        },
        timeout: timeOut
    }).complete(function (xhr, status) {
        if (showLoading) $("#divLoading").hide();
    });
}

function partial(fn) {
    var args = Array.prototype.slice.call(arguments);
    args.shift();
    return function () {
        var new_args = Array.prototype.slice.call(arguments);
        args = args.concat(new_args);
        return fn.apply(window, args);
    };
}
$p = partial;

//function restrictSpecialChar(input) {
//    var eltname = "#" + input;
//    var allInputs = $(eltname).find(':input[type=text]');
//    if (allInputs != null && allInputs.length > 0) {
//        allInputs.each(function (index, obj) {
//            if (/^[a-zA-Z0-9- ]*$/.test(input) === false)
//                ShowAlert("warning", "Charecter not allowed !!");
//        });
//    }
//}

var gridCommonObject = {
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
    pageModel: { type: "local", rPP: 100 }
}

function collapseDiv(element) {
    var eltname = "#" + element;
    $(eltname).collapse();
}


function Encryptfunction(stringvalue) {
    var encryptedpss = "";
        $.ajax({
            type: 'GET',
            url: '/CommonArea/Encryptfunction',
            data: { strPassword: stringvalue },
            contentType: false,
            processData: false,
            success: function (data) {
                encryptedpss = data;
            },
            error: function () {
                alert("An error occurred!");
            },
        });
        return encryptedpss;
}
function Decryptfunction(stringvalue) {
    var encryptedpss = "";
    $.ajax({
        type: 'GET',
        url: '/CommonArea/Decryptfunction',
        data: { strPassword: stringvalue },
        contentType: false,
        processData: false,
        success: function (data) {
            encryptedpss = data;
        },
        error: function () {
            alert("An error occurred!");
        },
    });
    return encryptedpss;
}

function IsNullOrUndefined(object) {
    if (typeof (object) !== "undefined" && object)
        return false;
    else
        return true;
}

function CollapsePqGrid(grid) {
    $(".pq-slider-icon .ui-icon-circle-triangle-n", grid).click();
}
function ExpandPqGrid(grid) {
    $(".pq-slider-icon .ui-icon-circle-triangle-s", grid).click();
}
function PqGridRefreshClick(grid) {
    $(".pq-grid-footer .ui-icon-refresh", grid).click();
}
function TogglePqGridFullSize(grid) {
    $(".pq-slider-icon .ui-icon", grid).click();
}

var gridDateTimeEditor = function (ui) {
    debugger;
    var $cell = ui.$cell,
        rowData = ui.rowData,
        dataIndx = ui.dataIndx,
        cls = ui.cls,
        dc = $.trim(rowData[dataIndx]);
    $cell.css('padding', '0');
    var $inp = $("<input type='text' id='" +
            dataIndx +
            "' name='" +
            dataIndx +
            "' class='" +
            cls +
            "' style='position: relative; z-index: 100;border: 3px solid rgba(0,0,0,0);'/>")
        .appendTo($cell)
        .val(dc)
        .datetimepicker({
            format: 'MMM DD YYYY hh:mm A'
         

        });
    window.setTimeout(function () {
        var dc = $.trim(ui.column.render(ui));
        $inp.val(dc);
    },
        0);
};




function setClientDateTime(timeStamptobeConverted, datepicker) {
    let setTime = timeStamptobeConverted.split("(");
    let splitTime = setTime[1].split(")");
    let splitedDateTime = parseInt(splitTime[0]);
    let datetime = new Date(splitedDateTime);
    let licensedate = getFormattedDate(datetime); //datetime.toISOString().slice(0, 10);
    var datetpickercontrol = $("#" + datepicker);
    datetpickercontrol.data({ date: licensedate });
    datetpickercontrol.datetimepicker().children('input').val(licensedate);
}

function getFormattedDate(date) {

    var year = date.getFullYear();
    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return day + '-' + month + '-' + year;
}


// copyright Stephen Chapman, 15th Nov 2004,14th Sep 2005
function valRadioBtn(grpName) {
    var cnt = -1;
    for (var i = grpName.length - 1; i > -1; i--) {
        if (grpName[i].checked) { cnt = i; i = -1; }
    }
    if (cnt > -1) return grpName[cnt].value;
    else return null;
}


function GetNetworkProviderDetails() {
    var request = new XMLHttpRequest();
    request.open('GET', 'https://api.ipdata.co/?api-key=c124bf18ac67c0ef8a53ccc84b6f3b9db1d0f7f6e9ed5f0493e89c3e', false);
    request.setRequestHeader('Accept', 'application/json');

    /*request.onreadystatechange = function () {
		if(this.readyState === 4) {
			var jsonData = JSON.parse(request.responseText);
			return jsonData.organisation + ' ('+ jsonData.ip +')';
		}
		return "";
	};*/
    request.send();

    var jsonData = JSON.parse(request.responseText);
    return jsonData.organisation + ' (' + jsonData.ip + ')';

    /*let IP, City, Provider;
	$.get("https://ipinfo.io", function(response) {
		console.log(response);
		--response.city,response.country,response.ip,response.loc,response.org,response.region
		console.log(response.ip, response.city, response.org);
		IP = response.ip;
		City = response.city;
		Provider = response.Org;
	}, "jsonp")*/
}

function validatenumerics(key) {
    //getting key code of pressed key
    var keycode = (key.which) ? key.which : key.keyCode;
    //comparing pressed keycodes

    if (keycode > 31 && (keycode < 48 || keycode > 57)) {
        //alert(" You can enter only characters 0 to 9 ");
        return false;
    }
    else return true;
}


