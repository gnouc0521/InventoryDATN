(function () {
    var _$importRequestTable = $('#ImportRequestTable');
    var _$YCNKTable = $('#YCNKTable');
    var _$UpdateimportRequestTable = $('#UpdateImportRequestTable');
    var _importRequestService = abp.services.app.importRequest;
    var _impsService = abp.services.app.importRequestSubidiaryService;
    var _transferService = abp.services.app.transfer;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ImportRequest/CreateImportYCNK',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ImportRequests/_CreateModalYCNK.js',
        modalClass: 'ImportRequestCreateModal',
        modalType: 'modal-xl'
    });

    var DataTable = _$YCNKTable.DataTable({
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
            ajaxFunction: _impsService.getAllDone,
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
                data: "nameSup"
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
                data: "browsingtime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },

            {

                targets: 6,
                class: 'text-center',
                orderable: false,
                render: function (data, type, row, meta) {
                    return row.statusImport == true ? 
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
                render: function (data, type, row, meta) {
                    debugger
                    if (row.transferId == 0) {
                        return row.ycnkCode
                    }
                    else {
                        return row.tranferCode
                    }
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "nameWareHouse"
            },
            {
                orderable: false,
                targets: 4,
                data: "createdBy"
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
                    if (row.importStatus == 0) {

                        return `<span class="span_status span-defaut">Đã xắp xếp</span>`
                    }
                    if (row.importStatus == 2) {
                        return `<span class="span_status span-defaut">Chờ sắp xếp</span>`
                    }
                    if (row.importStatus == 1) {
                        return `<span class="span_status span-subcontract">Chờ nhận hàng</span>`
                    }
                    if (row.importStatus == 3) {
                        return `<span class="span_status span-contract">Đã nhận hàng</span>`
                    }
                    if (row.importStatus == 5) {
                        return `<span class="span_status span-approve">Hoàn thành</span>`
                    }
                }
            },
        ]
    });
  


    $('#YCNKTable').on('click', '.impcreatefunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _createModal.open(dataFilter);
    })

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
    

    function getTranfer() {

        DataTable.ajax.reload();
    }

    abp.event.on('app.reloadTranferTable', function () {
        getTranfer();
    });

    function getDocs() {

        dataTable.ajax.reload();
    }

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });
})(jQuery);