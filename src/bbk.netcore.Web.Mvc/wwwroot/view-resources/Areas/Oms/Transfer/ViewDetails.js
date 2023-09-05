(function () {
    var _$tranferItemTable = $('#TransferItemTable');
    var _transferService = abp.services.app.transfer;
    var _warehouseService = abp.services.app.wareHouse;
    var _transferDetailService = abp.services.app.transferDetail;
    var _exportRequests = abp.services.app.exportRequests;
    var _exportRequestsdetail = abp.services.app.exportRequestDetails;
    var _importRequestService = abp.services.app.importRequest;
    var _importRequestsdetail = abp.services.app.importRequestDetail;

    moment.locale(abp.localization.currentLanguage.name);

    var dataId_wh = [];
    var IdSub = [];

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Transfer/CreateTransfer',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Transfer/_CreateModal.js',
        modalClass: 'TransferCreateModal',
        modalType: 'modal-xl'

    });

    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Transfer/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Transfer/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });


    _warehouseService.get({ id: $("#IdWareHouseEx").val() }).done(function (result) {
        $(".NameWareExport").text(result.name);
    })
    //var _EditModal = new app.ModalManager({
    //    viewUrl: abp.appPath + 'Inventorys/Supplier/EditSupplierModal',
    //    scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Suppliers/_EditModal.js',
    //    modalClass: 'SupplierEditModal',
    //    modalType: 'modal-xl'
    //});

    //var _ViewDetails = new app.ModalManager({
    //    viewUrl: abp.appPath + 'Inventorys/Supplier/ViewDetails',
    //    modalClass: 'ViewModal',
    //    modalType: 'modal-xl'
    //});

    $("#btnSubmit").on("click", function () {
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize(''),
            app.localize('Bạn có chắc Gửi không?'),
            function (isConfirmed) {
                if (isConfirmed) {
                    var dataUpdate = {};
                    dataUpdate.Status = 2;
                    dataUpdate.Id = $("#Id").val();
                    dataUpdate.email = $("#email").val();
                    dataUpdate.link = window.location.href;
                    dataUpdate.name = $("#name").val();
                    _transferService.updateTransferStatus(dataUpdate).done(function () {
                        abp.notify.success(app.localize('Đã gửi thành công!'));
                        window.setTimeout('location.reload()', 1000);
                    })
                }
            }
        );
    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = "";
        dataFilter.id = $("#Id").val();
        return dataFilter;
    }


    var dataTable = _$tranferItemTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,
        //"dom": 'Rltip',
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _transferDetailService.getAll,
            inputFilter: getFilter
        },

        columnDefs: [

            {
                orderable: false,
                targets: 0,
                width: "5%",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                render: function (data, type, row, meta) {
                    return `<span class="info-item" data-itemId="` + row.itemId + `" >` + row.itemName + `</span>`;
                }

            },
            {
                orderable: false,
                targets: 2,
                className: "quotePrice",
                data: "quotePrice"

            },
            {
                orderable: false,
                targets: 3,
                render: function (data, type, row, meta) {
                    return `<span class="info-unit" data-unitId="` + row.idUnit + `" >` + row.unitName + `</span>`;
                }

            },
            {
                orderable: false,
                targets: 4,
                data: "quantityInStock",
            },
            {
                orderable: false,
                targets: 5,
                className: "Quantity",
                data: "quantityTransfer",

            },
            {
                orderable: false,
                targets: 6,
                render: function (data, type, row, meta) {
                    return `<span class="id-ware" data-IdWhReceiving="` + row.idWarehouseReceiving + `" >` + row.warehouseReceivingName + `</span>`;
                }
            },

        ]
    });

    abp.event.on('app.reloadTransferTable', function () {

        window.setTimeout('location.reload()', 1000);
    });


    //$('#TranferTable').on('click', '.Approve', function (e) {
    //    var btnClick = $(this);
    //    let dataFilter = {};
    //    dataFilter.status = 2;
    //    dataFilter.id = btnClick[0].dataset.quotesyn;
    //    _transferService.update(dataFilter).done(function () {
    //        abp.notify.success(app.localize('Phê duyệt phiếu chuyển giao thành công'));
    //        getDocs();

    //    });
    //});

    //$('#TranferTable').on('click', '.Reject', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.quotesyn };
    //    _createModalReject.open(dataFilter);

    //});viewDetail

    $('#TranferTable').on('click', '.viewDetail', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quotesyn };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Transfer/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Transfer/ViewDetails?id=" + results.id;
            },
        });

    });

    //function Loại đi phần tử trùng nhau
    function uniqueArray(orinalArray) {
        return orinalArray.filter((elem, position, arr) => {
            return arr.indexOf(elem) == position;
        });
    }


    //--Duyệt
    $("#btn-contract_Approve").on("click", function () {
        
        

        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize(''),
            app.localize('Bạn có chắc Duyệt không?'),
            function (isConfirmed) {
                if (isConfirmed) {

                    //------------------- Tạo mới ------------------
                    $('#TransferItemTable tbody tr').each(function (index, value) {

                        dataId_wh.push($(value).find("td").children("span.id-ware").attr("data-IdWhReceiving"));
                    });

                    var ArrNew = uniqueArray(dataId_wh);


                    //Tạo mới phiếu xuất

                    var dataObject = {};
                    var dataDertail = {};
                    for (var i = 0; i < ArrNew.length; i++) {

                        dataObject.WarehouseDestinationId = $("#IdWareHouseEx").val();
                        dataObject.IdWarehouseReceiving = ArrNew[i];
                        dataObject.RequestType = 0;
                        dataObject.CodeRequirement = "";
                        dataObject.Status = 3;
                        dataObject.TotalAmount = 3;
                        dataObject.ExportStatus = 1;
                        dataObject.TransferId = $("#Id").val();
                        dataObject.CodeTransfer = $("#Transfercode").val();

                        _exportRequests.createToTransfer(dataObject).done(function (result) {

                            $('#TransferItemTable tbody tr').each(function (index, value) {
                                var testUO = $(value).find("td").children("span.id-ware").attr("data-IdWhReceiving");

                                if (testUO == result.idWarehouseReceiving) {

                                    dataDertail.ExportRequestId = result.id;
                                    dataDertail.ItemId = $(value).find("td").children("span.info-item").attr("data-itemId");
                                    dataDertail.Quantity = $(value).find("td.Quantity").text();
                                    dataDertail.UnitId = $(value).find("td").children("span.info-unit").attr("data-unitId");
                                    dataDertail.UnitName = $(value).find("td").children("span.info-unit").text();
                                    dataDertail.ExportPrice = $(value).find("td.quotePrice").text();
                                    dataDertail.WarehouseSourceId = result.idWarehouseReceiving;
                                    _exportRequestsdetail.create(dataDertail).done(function () { });

                                }
                            })

                        });
                    }

                    var d = new Date();

                    var month = d.getMonth() + 1;
                    var day = d.getDate();

                    var output = d.getFullYear() + '/' +
                        (month < 10 ? '0' : '') + month + '/' +
                        (day < 10 ? '0' : '') + day;


                    // Tạo mới phiếu nhập
                    var dataImport = {};
                    var dataimportDetail = {};
                    for (var j = 0; j < ArrNew.length; j++) {
                        dataImport.WarehouseDestinationId = ArrNew[j];
                        dataImport.TransferId = $("#Id").val();
                        dataImport.Status = 0;
                        dataImport.RequestDate = output;

                        _importRequestService.createToTranssfer(dataImport).done(function (result1) {
                            $('#TransferItemTable tbody tr').each(function (index, value) {
                                var testUO = $(value).find("td").children("span.id-ware").attr("data-IdWhReceiving");

                                if (testUO == result1.warehouseDestinationId) {

                                    dataimportDetail.ImportRequestId = result1.id;
                                    dataimportDetail.ItemId = $(value).find("td").children("span.info-item").attr("data-itemId");
                                    dataimportDetail.ItemCode = $(value).find("td").children("span.info-item").attr("data-itemId").split("-")[0];
                                    dataimportDetail.Quantity = $(value).find("td.Quantity").text();
                                    dataimportDetail.UnitId = $(value).find("td").children("span.info-unit").attr("data-unitId");
                                    dataimportDetail.UnitName = $(value).find("td").children("span.info-unit").text();
                                    dataimportDetail.ImportPrice = $(value).find("td.quotePrice").text();
                                    _importRequestsdetail.create(dataimportDetail).done(function () { });

                                }
                            })
                        })
                    }
                    //------------------- / tạo mới-----------------


                    var dataUpdate = {};
                    dataUpdate.Status = 3;
                    dataUpdate.Id = $("#Id").val();
                    dataUpdate.link = window.location.href;
                    _transferService.updateTransferStatus(dataUpdate).done(function () {
                        abp.notify.success(app.localize('Đã gửi Phiếu điều chuyển thành công!'));

                        if (ArrNew.length > 1) {
                            _exportRequests.updateCodeSame($("#Id").val()).done(function () {

                            });
                            window.setTimeout('location.reload()', 1000);
                        }
                        else {
                            window.setTimeout('location.reload()', 1000);
                        }
                    })


                }
            }
        );
    })

    //-Từ chối
    $("#btn-contract_Reject").on("click", function () {
        var dataFilter = { id: $("#Id").val() };
        _createModalReject.open(dataFilter);
    });

    //$('#supplierTable').on('click', '.doceditfunc', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.objid };
    //    _EditModal.open(dataFilter);
    //});


    //$('#supplierTable').on('click', '.viewsupplier', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.viewid };
    //    _ViewDetails.open(dataFilter);
    //});



    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {

        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });


})(jQuery);