(function () {
    var _$importRequestTable = $('#ImportRequestTable');
    var _$StaffTable = $('#StaffTable');
    var _orderDetail = abp.services.app.ordersDetail;
    var _impsService = abp.services.app.importRequestSubidiaryService;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ImportRequestSubsidiary/CreateImportRequest',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ImportRequestDetailSubsidiarys/_CreateModal.js',
        modalClass: 'ImportRequestCreateModal',
        modalType: 'modal-xl'
    });
   

    $('.date-picker').datepicker({
        rtl: false,
        format: "dd/mm/yyyy",
        orientation: "left",
        autoclose: true,
        language: abp.localization.currentLanguage.name,

    });


    var dataTable = _$StaffTable.DataTable({
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
        "bLengthChange": false,
        //"dom": 'Rltip',
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 5,
        listAction: {
            ajaxFunction: _orderDetail.getYCNK,
        },

        columnDefs: [
            {
                orderable: false,
                targets: 0,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: true,
                targets: 1,
                data: "orderCode"
            },
            {
                orderable: false,
                targets: 2,
                data: "contractCode"
            },
            {
                orderable: false,
                targets: 3,
                data: "supplierName"
            },
            {
                orderable: false,
                targets: 4,
                data: "createdBy"
            },
            {
                orderable: false,
                targets: 5,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                targets: 6,
                class: 'text-center',
                orderable: false,
                render: function (data, type, row, meta) {
                    return row.statusCreateYCNK == true ?
                        "<a type='button' class='btn btn-primary btn-sm disabled' style='margin-left:5px'><i class='fal fa-check'></i> Tạo YCNK</a>" :
                        ` <a type = 'button' class='btn btn-primary btn-sm impcreatefunc' data-objid='` + row.id + `'href = 'javascript:void(0);' style = 'margin-left:5px'><i class='fa fa-plus'></i> Tạo YCNK</a>`;
                }
            }
        ],
    });


    $('#StaffTable').on('click', '.impcreatefunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _createModal.open(dataFilter);
    })



    var DataTable = _$importRequestTable.DataTable({
        paging: true,
        serverSide: false,
        processing: false,
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
            ajaxFunction: _impsService.getAll,
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
                data: "requestDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 4,
                data: "nameSup"
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
                orderable: false,
                targets: 6,
                data: "importStatus",
                render: function (data, type, row, meta) {
                    if (row.importStatus == 0) {
                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa xử lý</span>`
                    } else if (row.importStatus == 2) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã phê duyệt</span>`
                    } else if (row.importStatus == 1) {
                        if (abp.session.userId != row.creatorUserId) {
                            return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa xử lý</span>`

                        } else {
                            return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Đã gửi</span>`
                        }
                    } else if (row.importStatus == 3) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-reject">Bị từ chối</span>`
                    } 
                }
            },
        ]
    });

    $('#ImportRequestTable').on('click', '.viewwork', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequestSubsidiary/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ImportRequestSubsidiary/ViewDetails?id=" + results.id;
            },
        });
    });
   

   

   
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
})(jQuery);