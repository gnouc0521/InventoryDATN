(function () {
    var _$iNVENTable   = $('#InvenTicketTable');
    var _importRequestService = abp.services.app.importRequest;
    var _importRequestsdetail = abp.services.app.importRequestDetail;
    var _inventoryTicketService = abp.services.app.inventoryTicketService;
    var _inventoryTicketdetail = abp.services.app.inventoryTicketDetailService;

    var _print = new app.ModalManager({
        viewUrl: abp.appPath + 'PersonalProfile/InventoryTicket/Print',
        modalClass: 'PrintModal',
        modalType: 'modal-xl'
    });

    $('#btnPrint').click(function () {
        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "PersonalProfile/InventoryTicket/OverView?Id=" + $('#Id').val(),
            success: function (results) {
                window.location.href =
                    "/PersonalProfile/InventoryTicket/Print?id=" + results.id;
            },
        });
    });

    var getFilter = function () {
        let dataFilter = {};
        dataFilter.inventoryTicketsId = $('#Id').val();
        dataFilter.warehouseId = $('#WarehouseId').val();
        return dataFilter;
    }


    var dataTable = _$iNVENTable.DataTable({
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
            ajaxFunction: _inventoryTicketdetail.getAll,
            inputFilter: getFilter
        },

        columnDefs: [
            {
                orderable: false,
                targets: 0,
                data: "stt"
            },
            {
                orderable: false,
                targets: 1,
                data: "nameCode"
            },
            {
                orderable: false,
                targets: 2,
                data: "unitName"
            },
            {
                orderable: false,
                targets: 3,
                data: "soluongHT",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                orderable: false,
                targets: 4,
                data: "quantity",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                orderable: false,
                targets: 5,
                data: "soLuongTru",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            //{
            //    orderable: false,
            //    targets: 6,
            //    data: "soLuongTru"
            //},
            //{
            //    orderable: false,
            //    targets: 7,
            //    data: "dongia"
            //},
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

            // Total over this page
            pageTotal = api
                .column(3, { page: 'current' })
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

            // Total over this page
            pageTotal1 = api
                .column(4, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            total2 = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal2 = api
                .column(5, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            //total3 = api
            //    .column(7)
            //    .data()
            //    .reduce(function (a, b) {
            //        return intVal(a) + intVal(b);
            //    }, 0);

            //// Total over this page
            //pageTotal3 = api
            //    .column(7, { page: 'current' })
            //    .data()
            //    .reduce(function (a, b) {
            //        return intVal(a) + intVal(b);
            //    }, 0);

            // Update footer
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            $(api.column(3).footer()).html(numFormat(pageTotal));
            $(api.column(4).footer()).html(numFormat(pageTotal1));
            $(api.column(5).footer()).html(numFormat(pageTotal2));
           /* $(api.column(7).footer()).html(pageTotal3);*/
        },
    });
})(jQuery);