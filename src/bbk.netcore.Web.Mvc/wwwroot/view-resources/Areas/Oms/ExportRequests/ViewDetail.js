(function () {
    var _$printTable = $('#PrintTable');
    var _$ExportTable = $('#ExportTable');
    var _ExportService = abp.services.app.exportRequestDetails;
    var _exportRequests = abp.services.app.exportRequests;
    var _warehouseItem = abp.services.app.warehouseItem;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    var _print = new app.ModalManager({
        viewUrl: abp.appPath + 'PersonalProfile/ExportRequests/Print',
        modalClass: 'PrintModal',
        modalType: 'modal-xl'
    });

    $('#btnPrint').click(function () {
        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "PersonalProfile/ExportRequests/OverView?Id=" + $('#Id').val(),
            success: function (results) {
                window.location.href =
                    "/PersonalProfile/ExportRequests/Print?id=" + results.id;
            },
        });
    });

    ///* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.exportRequestId = $('#Id').val()
        dataFilter.warehouseId = $('#WarehouseId').val()
        return dataFilter;
    }

    var dataTable = _$ExportTable.DataTable({
        paging: false,
        serverSide: false,
        processing: false,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
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
                    var stt = meta.row + 1;
                    return `<span>` + stt + `</span>`;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: 'itemsCode',
                render: function (data, type, row, meta) {
                    return `<span>` + row.itemsCode + `-` + row.itemsName + `</span>`
                }
            },
            {
                orderable: false,
                targets: 2,
                data: null,
                render: function (data, type, row, meta) {
                    return `<span>` + row.unitName +`</span>`
                }
            },
            //{
            //    orderable: false,
            //    class: 'text-center',
            //    targets: 3,
            //    data: null,
            //    render: function (data, type, row, meta) {
            //        return `<span>` + row.shelfName + `</span>`
            //    }
            //},
            //{
            //    orderable: false,
            //    targets: 4,
            //    data: null,
            //    render: function (data, type, row, meta) {
            //        return `<span>` + row.floorName + `</span>`
            //    }
            //},
            //{
            //    orderable: false,
            //    targets: 5,
            //    data: null,
            //    render: function (data, type, row, meta) {
            //        return `<span>` + row.blockName + `</span>`
            //    }
            //},
           
            {
                orderable: false,
                targets: 3,
                data: 'quantityTotal',
            },
            {
                orderable: false,
                targets: 4,
                data: 'quantityExport',
            },
            {
                orderable: false,
                targets: 5,
                data: 'exportPrice',
            },
            
        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // Total over all pages
            total = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

          
            total1 = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            // Total over all pages
            total2 = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(3, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal2 = api
                .column(4, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal1 = api
                .column(5, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(3).footer()).html(pageTotal);
            $(api.column(5).footer()).html(pageTotal1);
            $(api.column(4).footer()).html(pageTotal2);
        },
        "initComplete": function (settings, json) {
            loadinforBin()
        }
    })

    var dataTable1 = _$printTable.DataTable({
        paging: false,
        serverSide: false,
        processing: true,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": false,
        //"dom": 'Rltip',
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
                    var stt = meta.row + 1;
                    return `<span>` + stt + `</span>`;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "itemsCode"
            },
            {
                orderable: false,
                targets: 2,
                data: "itemsName"
            },
            {
                orderable: false,
                targets: 3,
                data: "unitName"
            },
            {
                orderable: false,
                targets: 4,
                data: "quantityExport",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                orderable: false,
                targets: 5,
                data: "exportPrice",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                orderable: false,
                targets: 6,
                data: "thanhtien",
                render: $.fn.dataTable.render.number(',', ',', '')
            },

        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // Total over all pages
            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(4, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            total1 = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal1 = api
                .column(5, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            $(api.column(4).footer()).html(numFormat(pageTotal));
            $(api.column(5).footer()).html(numFormat(pageTotal1));
        },
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    var loadinforBin = function () {
        var spanblockId = _$ExportTable.find('.blockId');
        spanblockId.each(function (index, value) {
            let dataBolckId = spanblockId.attr('data-blockId');
            _warehouseItem.getinfoBin(dataBolckId).done(function (result) {
                $(value).text(result)
            });
        })
     

    }

    $(".btn-browse").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.status = 1;
        dataFilter.id = btnClick[0].dataset.quo;
        _exportRequests.updateStatus(dataFilter).done(function () {
            abp.notify.info('Phê duyệt phiếu xuất thành công!');
            getDocs();
           $(".btn-browse").prop("disabled", true);
            $(".btn-reject").prop("disabled", true);
            getDocs();
        })
    })

    $(".btn-reject").click(function () {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quo };
        _createModalReject.open(dataFilter);
    });


    $('#ExportTable').on('click', '.docview', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "PersonalProfile/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/PersonalProfile/ExportRequests/ViewDetails?id=" + results.id;
            },
        });
    });
    $('#ExportTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
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