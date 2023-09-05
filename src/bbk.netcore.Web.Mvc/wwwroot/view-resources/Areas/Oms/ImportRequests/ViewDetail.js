(function () {
    var _$itemTable = $('#ItemsTable');
    var _printTable = $('#PrintTable');
    var _$ItemViewLoaction = $("#ItemsTableView");
    var _importRequestService = abp.services.app.importRequest;
    var _importRequestsdetail = abp.services.app.importRequestDetail;
    var _warehouseLocation = abp.services.app.warehouseLocationItemService;
    var _inventoryService = abp.services.app.inventoryService;

    var _print = new app.ModalManager({
        viewUrl: abp.appPath + 'PersonalProfile/ImportRequest/Print',
        modalClass: 'PrintModal',
        modalType: 'modal-xl'
    });



    var getFilter = function () {
        let dataFilter = {};
        var url = window.location.href;
        var id = url.substring(url.lastIndexOf('=') + 1);
        dataFilter.importRequestId = id;
        return dataFilter;
    }


    var dataTable = _$itemTable.DataTable({
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
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
     
        pageLength: 10,
        listAction: {
            ajaxFunction: _importRequestsdetail.getAll,
            inputFilter: getFilter
        },

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
                data: "codeItem"
            },
            {
                orderable: false,
                targets: 2,
                data: "nameItem"
            },
            {
                orderable: false,
                targets: 3,
                data: "unitName"
            },
            {
                orderable: false,
                targets: 4,
                data: "quantity",
                 render: $.fn.dataTable.render.number(',', ',','')
            },
            {
                orderable: false,
                targets: 5,
                data: "importPrice",
                render: $.fn.dataTable.render.number(',', ',','')
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

    var dataTable1 = _printTable.DataTable({
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
            ajaxFunction: _importRequestsdetail.getAll,
            inputFilter: getFilter
        },

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
                data: "codeItem"
            },
            {
                orderable: false,
                targets: 2,
                data: "nameItem"
            },
            {
                orderable: false,
                targets: 3,
                data: "unitName"
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
                data: "importPrice",
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

            total2 = api
                .column(6)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal2 = api
                .column(6, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            $(api.column(4).footer()).html(numFormat(pageTotal));
            $(api.column(5).footer()).html(numFormat(pageTotal1));
            $(api.column(6).footer()).html(numFormat(pageTotal2));
        },
    });

     $('#btnPrint').click(function () {
        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + $('#Id').val(),
            success: function (results) {
                window.location.href =
                    "/Inventorys/ImportRequest/Print?id=" + results.id;
            },
        });
    });

    // ------------------------------- Table Item Location --------------------------------

    var getFilter1 = function () {
        let dataFilter = {};
        dataFilter.importId = $("#Id").val();
        return dataFilter;
    }

    var dataItemLocation = _$ItemViewLoaction.DataTable({
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
            ajaxFunction: _warehouseLocation.getByInImport,
            inputFilter: getFilter1
        },

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
                className: "ItemId",
                render: function (data, type, row, meta) {
                    return `<span data-itemId="` + row.itemId + `" data-wareId="` + row.warehouseId +`">` + row.itemCode +`</span>`;
                }
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
                data: "quantity",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                orderable: false,
                targets: 5,
                data: "price",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                orderable: false,
                targets: 6,
                className: "QR-code",
                data: null,
                render: function (data, type, row, meta) {
                    return `<div class="QR-content" data-importDetai="`+ row.id+`">
                                <span class="itemcode" data-ttemcode="`+ row.itemCode +`"></span>
                                <div id="barcode_`+row.id+`"></div>
                            </div>`;
                }
                
            },
            {
                orderable: false,
                targets: 7,
                data: "locationName",
               
            },
            {
                orderable: false,
                targets: 8,
                className: "quantityRe",
                data: "quantityReality",

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

            total2 = api
                .column(6)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal2 = api
                .column(6, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            $(api.column(4).footer()).html(numFormat(pageTotal));
            $(api.column(5).footer()).html(numFormat(pageTotal1));
            $(api.column(6).footer()).html(numFormat(pageTotal2));
        },
        "initComplete": function (settings, json) {
            ViewQRCode();

        },
    });


    var ViewQRCode = function (e) {
        $('#ItemsTableView tbody tr').each(function (index, value) {
            var ItemCode = $(value).children('td.QR-code').children('.QR-content').find("span.itemcode").attr("data-ttemcode");
            var id = $(value).children('td.QR-code').children('.QR-content').attr("data-importDetai");

            //Qr Code
            var qrcode = new QRCode(document.getElementById("barcode_"+id), {
                text: "textma",
                width: 100,
                height: 100,
                colorDark: "#000000",
                colorLight: "#ffffff",
                /*correctLevel: QRCode.CorrectLevel.M*/
            });

            qrcode.makeCode(ItemCode);
        })

    }

    // ------------------------------- 3/ Table Item Location -------------------------------


    ///gửi cho chuyên viên
    $("#btnSend").on("click", function () {
        var dataupdateImport = {};
        dataupdateImport.Id = $("#Id").val();
        dataupdateImport.ImportStatus = 1;
        _importRequestService.updateStatusImport(dataupdateImport).done(function () {
            abp.notify.info('Gửi thành công!');
            $("#btnSend").hide();
        })
    });

    //Xác nhận lần cuối
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
                    var dataUpdate = {};
                    dataUpdate.ImportStatus = 5;
                    dataUpdate.Id = $("#Id").val();

                    _importRequestService.updateStatusImport(dataUpdate).done(function () {
                        abp.notify.success(app.localize('Đã duyệt thành công!'));

                        $("#btn-contract_Approve").hide();

                    });

                    var dataUpdateInven = {};
                    $('#ItemsTableView tbody tr').each(function (index, value) {
                        dataUpdateInven.ItemId = $(value).children("td.ItemId").find("span").attr("data-itemId");
                        dataUpdateInven.WarehouseId = $(value).children("td.ItemId").find("span").attr("data-wareId");
                        dataUpdateInven.ItemCode = $(value).children("td.ItemId").find("span").text();
                        dataUpdateInven.Quantity = $(value).children("td.quantityRe").text();

                        _inventoryService.create(dataUpdateInven);
                    });
                
                }
            }
        );
    })


})(jQuery);