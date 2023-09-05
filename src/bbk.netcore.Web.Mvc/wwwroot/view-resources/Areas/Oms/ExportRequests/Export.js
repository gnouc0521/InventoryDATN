(function () {
    var _$ExportTable = $('#ExportTable');
    var _$ExportTableDone = $('#ExportTableDone');
    var _ExportService = abp.services.app.exportRequests;
    var _TransferService = abp.services.app.transfer;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/CreateExport',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_CreateExport.js',
        modalClass: 'ExportRequestsCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/EditExportRequests',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_EditModal.js',
        modalClass: 'ExportRequestsEditModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });


    $('.date-picker').datepicker({
        rtl: false,
        format: 'dd/mm/yyyy',
        orientation: "left",
        autoclose: true,
        language: abp.localization.currentLanguage.name,

    });

    ///* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        //  var Date = $("#RequestDate").datepicker("getDate");

        //  dataFilter.searchTerm = $('#SearchTerm').val().trim();
        dataFilter.exportStatus = 0;
        return dataFilter;
    }



    var dataTable = _$ExportTable.DataTable({
        paging: true,
        serverSide: false,
        processing: false,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
        pageLength: 10,
        listAction: {
            ajaxFunction: _TransferService.getTransferExp,
             inputFilter: getFilter
        },
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: 'transferCode',
            },
            {
                orderable: false,
                targets: 2,
                data: "nameWareHouseReceiving",
            },
            {
                orderable: false,
                class: 'text-center',
                targets: 3,
                data: "nameWareHouseExport",

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

                targets: 5,
                data: 'browsingTime',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (browsingTime) {
                    return moment(browsingTime).format('L');
                }
            }, {

                targets: 6,
                data: 'id',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {

                    return `
                            <button class="btn btn-primary btn-create" data-objid="`+ row.id + `">Tạo phiếu xuất </button>
                           `;
                },
            },

        ]
    })




    abp.event.on('app.reloadDocTable', function () {
        getDocs();
        getExport

    });

    $('#ExportTable').on('click', '.docview', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ExportRequests/ViewDetails?id=" + results.id;
            },
        });
    });
    $('#ExportTable').on('click', '.btn-create', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _createModal.open(dataFilter);
    });

    $('#ExportTable').on('click', '.docprintfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ExportRequests/Print?id=" + results.id;
            },
        });
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


    ///* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        //var Date = $("#RequestDate").datepicker("getDate");
        //if (Date != null) {
        //    dateStart = moment(Date).format('L');
        //    dataFilter.requestDate = dateStart
        //}
        //dataFilter.searchTerm = $('#SearchTerm').val().trim();
        //dataFilter.warehouseDestinationId = $('#ProducerCode').val();
        return dataFilter;
    }



    var dataTable1 = _$ExportTableDone.DataTable({
        paging: true,
        serverSide: false,
        processing: false,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
        pageLength: 10,
        listAction: {
            ajaxFunction: _ExportService.getAll,
            inputFilter: getFilter
        },
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: true,
                targets: 1,
                data: 'code',
                render: function (data, type, row, meta) {
                    return `
                            <a class='Exportviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>` + row.code + ` </a>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "warehouseDestinationName",
            },
            {
                orderable: false,
                class: 'text-center',
                targets: 3,
                data: "createBy",

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

                targets: 5,
                data: 'id',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    if (row.status == 0) {

                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chờ phê duyệt</span>`
                    } else if (row.status == 1) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã phê duyệt</span>`
                    } else if (row.status == 2) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-reject">Bị từ chối</span>`
                    } else {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-contract">Hoàn thành</span>`
                    }
                }
            }
        ]
    })

    function getExport() {

        dataTable1.ajax.reload();
    }

})(jQuery);