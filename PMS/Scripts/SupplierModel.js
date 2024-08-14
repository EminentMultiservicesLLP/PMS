var colSupplierM = [
                    { title: "", dataIndx: "ID", dataType: "integer", hidden: true },
                    { title: "Code", dataIndx: "Code", width: 200 },
                    { title: "Supplier Name", dataIndx: "Name", width: 200, filter: { type: 'textbox', condition: 'begin', listeners: ['keyup'] } }
];

dataSupplierM = {
    location: 'local',
    sorting: 'local',
    paging: 'local',
    dataType: 'JSON'
};

function ShowSupplierListPoup(gridName, modelName, setObject, getUrl) {
    var SupplierListGrid = $("#" + gridName);
    var ModelWindow = $("#" + modelName);
    if (SupplierListGrid != undefined) {

        SupplierListGrid.pqGrid(setObject);
        SupplierListGrid.pqGrid("option", "dataModel.data", []);

        ModelWindow.dialog({
            height: 500,
            width: 700,
            modal: true,
            open: function (evt, ui) {

                $.ajax({
                    type: "GET",
                    url: getUrl,
                    datatype: "Json",
                    beforeSend: function () {
                        SupplierListGrid.pqGrid("showLoading");
                    },
                    complete: function () {
                        SupplierListGrid.pqGrid("hideLoading");
                    },
                    success: function (data) {
                        SupplierListGrid.pqGrid("hideLoading");
                        SupplierListGrid.pqGrid("option", "dataModel.data", data);
                        SupplierListGrid.pqGrid("refreshDataAndView");
                    },
                    error: function (request, status, error) {
                        SupplierListGrid.pqGrid("hideLoading");
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