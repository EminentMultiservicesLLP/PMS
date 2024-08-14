/*************** Item Grid which willbe available for ALl item list **********/
colItemList = [
        {
            dataIndx: "state", Width: 25, align: "center", type: 'checkBoxSelection', cls: 'ui-state-default', sortable: false,
            editor: false, dataType: 'bool',
            title: "<input type='checkbox' />",
            cb: { select: true, all: false, header: true }
        },
       { dataIndx: "ID", hidden: true },
       { dataIndx: "ExpiryDate", hidden: true },
       { title: "Code", dataIndx: "Code", width: 100, dataType: "string", filter: { type: 'textbox', condition: 'begin', listeners: ['keyup'] }, hidden: true },
       { title: "Item Name", dataIndx: "Name", minWidth: 200, dataType: "string", filter: { type: 'textbox', condition: 'begin', listeners: ['keyup'] } },
        { title: "Batch Name", dataIndx: "BatchName", dataType: "string",  editable: false ,minWidth:100},
       { title: "HSN/SAC Code", dataIndx: "HSNCode", minWidth: 100, editable: false },
       { title: "Unit Name", dataIndx: "UnitName", minWidth: 90 },
       { title: "PackSize", dataIndx: "PackSize", minWidth: 80 },
       { title: "Mrp", dataIndx: "MRP", dataType: "string", minWidth: 60 },
       { title: "Current Stock", dataIndx: "CurrentQty", minWidth: 50, dataType: "double", editable: false }
];

colGRNItemList = [
                    {
                        dataIndx: "state", Width: 25, align: "center", type: 'checkBoxSelection', cls: 'ui-state-default', sortable: false,
                        editor: false, dataType: 'bool',
                        cb: { select: true, all: false, header: true }
                    },
                    { title: "", dataIndx: "ID", editable: false, hidden: true },
                    { title: "", dataIndx: "MarkupPercentage", editable: false, hidden: true },
                    { title: "", dataIndx: "PackSizeID", editable: false, hidden: true },
                    { title: "Code", dataIndx: "Code", width: 150, hidden: true },
                    { title: "Item Name", dataIndx: "Name", width: 200, dataType: "string", editable: false, filter: { type: 'textbox', condition: 'begin', listeners: ['keyup'] } },
                    { title: "HSN/SAC Code", dataIndx: "HSNCode", width: 100, editable: false },
                    { title: "Unit", dataIndx: "UnitName", width: 100, editable: false },
                    { title: "MRP", dataIndx: "MRP", width: 100, editable: false },
                    { title: "Rate", dataIndx: "StandardRate", width: 100, editable: false }
];

dataItemList = {
    location: 'local',
    sorting: 'local',
    paging: 'local',
    dataType: 'JSON'
};


function ShowItemListPoup(gridName, modelName, setObject, getUrl, searchStoreId, issueDate) {
    //debugger;
    var ItemListGrid = $("#" + gridName);
    var ModelWindow = $("#" + modelName);
    if (ItemListGrid != undefined) {

        ItemListGrid.pqGrid(setObject);
        ItemListGrid.pqGrid("option", "dataModel.data", []);

        ModelWindow.dialog({
            height: 500,
            width: 700,
            modal: true,
            open: function (evt, ui) {

                $.ajax({
                    type: "GET",
                    url: getUrl,
                    data: { storeId: searchStoreId, issueDate: issueDate },
                    datatype: "Json",
                    beforeSend: function () {
                        ItemListGrid.pqGrid("showLoading");
                    },
                    complete: function () {
                        ItemListGrid.pqGrid("hideLoading");
                    },
                    success: function (data) {
                        ItemListGrid.pqGrid("hideLoading");
                        ItemListGrid.pqGrid("option", "dataModel.data", data);
                        ItemListGrid.pqGrid("refreshDataAndView");
                    },
                    error: function (request, status, error) {
                        ItemListGrid.pqGrid("hideLoading");
                        ShowAlert("error", "Error while loading Itemlist");
                        return;
                    }
                });
            },
            close: function (event, ui) {
            },
            show: {
                effect: "blind",
                duration: 500
            }
        });
    }
}
/*********************************************************************/