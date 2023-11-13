(function () {
    var _$importRequestTable = $('#ImportRequestTable');
    var _$transferRequestTable = $('#TransferRequestTable');
    var _$UpdateimportRequestTable = $('#UpdateImportRequestTable');
    var _importRequestService = abp.services.app.importRequest;
    var _transferService = abp.services.app.transfer;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ImportRequest/CreateImportRequest',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ImportRequests/_CreateModal.js',
        modalClass: 'ImportRequestCreateModal',
        modalType: 'modal-xl'
    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ImportRequest/EditImportRequestModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ImportRequests/_EditModal.js',
        modalClass: 'ImportRequestEditModal',
        modalType: 'modal-xl'
    });

    $('.date-picker').datepicker({
        rtl: false,
        format: "dd/mm/yyyy",
        orientation: "left",
        autoclose: true,
        language: abp.localization.currentLanguage.name,

    });


    var DataTable = _$transferRequestTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
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
            ajaxFunction: _transferService.getTransferImp,
        },
        order: [[0, 'asc']],
        columnDefs: [

            {
                orderable: false,
                className: 'dt-body-center text-center',
                targets: 0,
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "transferCode"
            },
            {
                orderable: false,
                targets: 2,
                data: "nameWareHouseReceiving"
            },
            {
                orderable: false,
                targets: 3,
                data: "nameWareHouseExport"
            },

            {
                orderable: false,
                targets: 4,
                data: "creationTime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 5,
                data: "browsingTime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },

            {

                targets: 6,
                class: 'text-center',
                orderable: false,
                render: function (data, type, row, meta) {
                    return row.statusImportPrice == true ? 
                         "<a type='button' class='btn btn-primary btn-sm disabled' style='margin-left:5px'><i class='fal fa-check'></i> Tạo phiếu nhập</a>":
                        ` <a type = 'button' class='btn btn-primary btn-sm impcreatefunc' data-objid='` + row.id + `'href = 'javascript:void(0);' style = 'margin-left:5px'><i class='fa fa-plus'></i> Tạo phiếu nhập</a>`;
                }
            }
        ]
    });



    var dataTable = _$importRequestTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
        },
        "bInfo": false,
        "bLengthChange": true,
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
       

        pageLength: 10,
        listAction: {
            ajaxFunction: _importRequestService.getAll,
        },
        order: [[0,'asc']],
        columnDefs: [

            {
                orderable: false,
                className: 'dt-body-center text-center',
                targets: 0,
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "code",
                render: function (data, type, row, meta) {
                    return `
                        <a class="viewwork" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.code + `</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "nameWareHouse"
            },
            {
                orderable: false,
                targets: 3,
                data: "createdBy"
            },
           
            {
                orderable: false,
                targets: 4,
                data: "requestDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 5,
                data: "importStatus",
                render: function (data, type, row, meta) {
                    if (row.importStatus == 0) {

                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Đã nhận hàng</span>`
                    } else {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Chờ nhận hàng</span>`
                    }
                }
            },
        ]
    });


    var getFilter10 = function () {
        var dataFilter = {};
        dataFilter.status = 1;

        return dataFilter;
    }
    var UdataTable = _$UpdateimportRequestTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
        },
        "bInfo": false,
        "bLengthChange": true,
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],


        pageLength: 10,
        listAction: {
            ajaxFunction: _importRequestService.getAll,
            inputFilter: getFilter10
        },
        order: [[0, 'asc']],
        columnDefs: [

            {
                orderable: false,
                className: 'dt-body-center text-center',
                targets: 0,
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "code",
                render: function (data, type, row, meta) {
                    return `
                        <a class="viewwork" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.code + `</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "nameWareHouse"
            },
            {
                orderable: false,
                targets: 3,
                data: "createdBy"
            },

            {
                orderable: false,
                targets: 4,
                data: "requestDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 5,
                data: "requestDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 6,
                data: "importStatus",
                render: function (data, type, row, meta) {
                    if (row.importStatus == 1) {

                        return `<span class="span_status span-defaut">Chờ xử lý</span>`
                    }
                    if (row.importStatus == 3) {
                        return `<span class="span_status span-subcontract">Đã Gửi</span>`
                    }
                    if (row.importStatus == 5) {
                        return `<span class="span_status span-contract">Hoàn thành</span>`
                    }
                }
            },
            {

                targets: 7,
                class: 'text-center',
                orderable: false,
                render: function (data, type, row, meta) {
                    return row.importStatus == 3 || row.importStatus == 5 ?
                        "<a type='button' class='btn btn-primary btn-sm disabled ' style='margin-left:5px'><i class='fal fa-check'></i> Cập nhật</a>" :
                        ` <a type = 'button' class='btn btn-primary btn-sm impeditfunc' data-objid='` + row.id + `'href = 'javascript:void(0);' style = 'margin-left:5px'><i class='fa fa-edit'></i> Cập nhật</a>`;
                }
            }
        ]
    });


    $('#TransferRequestTable').on('click', '.impcreatefunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _createModal.open(dataFilter);
    })


    $('#UpdateImportRequestTable').on('click', '.impeditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });


    $('#ImportRequestTable').on('click', '.viewwork', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ImportRequest/ViewDetails?id=" + results.id;
            },
        });
    });

    $('#UpdateImportRequestTable').on('click', '.viewwork', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ImportRequest/ViewUpdateDetails?id=" + results.id;
            },
        });
    });

    function getTranferDone() {

        UdataTable.ajax.reload();
    }

    function getTranfer() {

        DataTable.ajax.reload();
    }

    function getDocs() {

        dataTable.ajax.reload();
    }
    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });
  
    abp.event.on('app.reloadTranferTable', function () {
        getTranfer();
    });

    abp.event.on('app.reloadDoneTable', function () {
        getTranferDone();
    });

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