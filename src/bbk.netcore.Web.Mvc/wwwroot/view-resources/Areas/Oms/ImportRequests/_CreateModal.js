(function ($) {

    app.modals.ImportRequestCreateModal = function () {

        var _$itemTable = $('#ItemTable');
        var _importRequestService = abp.services.app.importRequest;
        var _importRequestsdetail = abp.services.app.importRequestDetail;
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var _transferService = abp.services.app.transfer;
        var _transferDetailService = abp.services.app.transferDetail;
        var _modalManager;
        var _frmIMP = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');

            $("#WarehouseDestinationId").val($('#IdWarehouseReceiving').val());

            //var getFilter = function () {
            //    let dataFilter = {};
            //    dataFilter.id = $('#TransferId').val().trim();
            //    return dataFilter;
            //}


            //var DataTable = _$itemTable.DataTable({
            //    paging: true,
            //    serverSide: false,
            //    processing: true,
            //    "searching": false,
            //    "language": {
            //        "emptyTable": "Không tìm thấy dữ liệu",
            //        "lengthMenu": "Hiển thị _MENU_ bản ghi",
            //        "zeroRecords": "Không tìm thấy dữ liệu",
            //        searchPlaceholder: "Tìm kiếm"
            //    },
            //    "bInfo": false,
            //    "bLengthChange": false,
            //    //"dom": 'Rltip',
            //    lengthMenu: [
            //        [5, 10, 25, 50, -1],
            //        [5, 10, 25, 50, 'Tất cả'],
            //    ],


            //    pageLength: 10,
            //    listAction: {
            //        ajaxFunction: _transferDetailService.getAll,
            //        inputFilter: getFilter
            //    },
            //    order: [[0, 'asc']],
            //    columnDefs: [
            //        {
            //            orderable: false,
            //            className: 'dt-body-center text-center',
            //            targets: 0,
            //            data: null,
            //            render: function (data, type, row, meta) {
            //                return meta.row + 1;
            //            }
            //        },
            //        {
            //            orderable: false,
            //            targets: 1,
            //            data: "itemName"
            //        },
            //        {
            //            orderable: false,
            //            targets: 2,
            //            data: "quotePrice"
            //        },
            //        {
            //            orderable: false,
            //            targets: 3,
            //            data: "quantityInStock"
            //        },

            //        {
            //            orderable: false,
            //            targets: 4,
            //            data: "quantityTransfer"
            //        },
            //        {
            //            orderable: false,
            //            targets: 5,
            //            data: "unitName"
            //        },

            //        {

            //            orderable: false,
            //            targets: 6,
            //            data: 'creationTime',
            //            render: function (creationTime) {
            //                return moment(creationTime).format('L');
            //            }

            //        }
            //    ]
            //});

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
                        data: "itemCode"
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
                        data: "expireDate",
                        render: function (expireDate) {
                            return moment(expireDate).format('L');
                        }
                    },
                    {
                        orderable: false,
                        targets: 7,
                        className: "QR-code",
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<div class="QR-content" data-importDetai="` + row.id + `">
                                <span class="itemcode" data-ttemcode="`+ row.itemCode + `"></span>
                                <div id="barcode_`+ row.id + `"></div>
                            </div>`;
                        }

                    },
                    {
                        orderable: false,
                        targets: 8,
                        data: "locationName",

                    },
                    {
                        orderable: false,
                        targets: 9,
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
                    var qrcode = new QRCode(document.getElementById("barcode_" + id), {
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

            document.getElementById("ShipperPhone").addEventListener("input", function () {
                var valueChange = funcChanePhoneNumber();
                _frmIMP.find('input[name=ShipperPhone]').val(valueChange);
            });
            function funcChanePhoneNumber() {
                var valueChange = null;
                var valueInputPhone = _frmIMP.find('input[name=ShipperPhone]').val();
                if (valueInputPhone.substring(0, 1) == 0) {
                    valueChange = _frmIMP.find('input[name=ShipperPhone]').val().replace('0', '');
                } else {
                    valueChange = _frmIMP.find('input[name=ShipperPhone]').val().replace(/[^0-9]/g, '');
                }
                return valueChange;
            }
        }
        //sự kiện khi đóng modal
        $('.cancel-work-button').click(function () {
            abp.libs.sweetAlert.config = {
                confirm: {
                    icon: 'warning',
                    buttons: ['Không', 'Có']
                }
            };

            abp.message.confirm(
                app.localize('Đóng phiếu nhập'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _modalManager.close();
                        return true;

                    }
                }
            );

        });
        this.save = function () {
            _frmIMP.addClass('was-validated');
            if (_frmIMP[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmIMP.serializeFormToObject();
            _modalManager.setBusy(true);
            _importRequestService.create(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới loại kho thành công!');
                    abp.event.trigger('app.reloadTranferTable');

                    var dataa = $('#ItemTable').DataTable().rows().data();
                    dataa.each(function (value, index) {
                        var abc = $(".ExpireDate");
                        _itemsServiceService.getItemByCode(value.itemCode).done(function (result3) {
                            data.ItemId = result3.id;
                            data.ExpireDate = $($(abc)[index]).val();
                            data.ImportRequestId = result;
                            data.ImportPrice = value.quotePrice;
                            data.Quantity = value.quantityTransfer;
                            data.UnitName = value.unitName;
                            data.UnitId = value.idUnit;
                            data.id = $('#TransferId').val();
                            data.Status = $('#Status').val();
                            data.BrowsingTime = $('#BrowsingTime').val();
                            data.IdWarehouseExport = $('#IdWarehouseExport').val();
                            data.TransferCode = $('#TransferCode').val();
                            data.statusImportPrice = true;
                            _importRequestsdetail.create(data);
                            _transferService.update(data);
                            abp.event.trigger('app.reloadDocTable');
                        });
                    });
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);