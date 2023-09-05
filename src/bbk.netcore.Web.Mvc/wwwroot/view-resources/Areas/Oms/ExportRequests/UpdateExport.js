(function () {
    var _$ExportTable = $('#ExportTable');
    var _ExportService = abp.services.app.exportRequests;
    var _TransferService = abp.services.app.transfer;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/UpdateExportModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_UpdateExportModal.js',
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
        dataFilter.status = 3;
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
            ajaxFunction: _ExportService.getAllExport,
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
                data: 'warehouseDestinationName',
            },
            {
                orderable: false,
                class: 'text-center',
                targets: 3,
                data: "subsidiaryName",
                render: function (data, type, row, meta) {
                    if (row.subsidiaryName != null) {
                        return `
                            <span> `+ row.subsidiaryName + `</span>`;
                    } else {
                        return `<span><span>`
                    }

                }
            },
            {
                orderable: false,
                class: 'text-center',
                targets: 4,
                data: "listWarehouseSourceName",
                render: function (data, type, row, meta) {
                    if (row.listWarehouseSourceName != null) {
                        return `
                            <span> `+ row.listWarehouseSourceName + `</span>`;
                    } else {
                        return `<span><span>`
                    }

                }
            },
            {
                orderable: false,
                targets: 5,
                data: "creationTime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {

                targets: 6,
                data: 'lastModificationTime',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (lastModificationTime) {
                    return moment(lastModificationTime).format('L');
                }
            }, {

                targets: 7,
                data: null,
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (lastModificationTime) {
                    return '<span></span>';
                }
            }, {

                targets: 8,
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                data: "status",
                render: function (data, type, row, meta) {
                    return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-contract">Đã phê duyệt</span>`

                }
            }, {

                targets: 9,
                data: 'id',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    if (row.exportStatus == 1) {
                        return `
                            <button class="btn btn-primary btn-create" data-objid="`+ row.id + `">Cập nhật</button>
                           `;
                    } else {
                        return `
                            <button class="btn btn-primary btn-create" data-objid="`+ row.id + `" disabled>Cập nhật</button>
                           `;
                    }
                
                },
            },

        ]
    })

   


    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#ExportTable').on('click', '.Exportviewfunc', function (e) {
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

})(jQuery);