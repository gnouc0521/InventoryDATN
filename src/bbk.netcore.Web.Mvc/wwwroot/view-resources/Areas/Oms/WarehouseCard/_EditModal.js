(function ($) {
    app.modals.ImportRequestEditModal = function () {

        var _$itemTable = $('#ItemTable');
        var _$ItemViewLoaction = $("#ItemsTableView");
        var _importRequestService = abp.services.app.importRequest;
        var _importRequestsdetail = abp.services.app.importRequestDetail; 
        var _itemsServiceService = abp.services.app.itemsService;
        var _warehouseLocation = abp.services.app.warehouseLocationItemService;
        var _modalManager;
        var _unitService = abp.services.app.unitService;
        var _frmIMP = null;
        var arrdetails = [];
        var arrchange = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=FrmEdit]');
          
            //var getFilter = function () {
            //    let dataFilter = {};
            //    dataFilter.importRequestId = $('#Id').val();
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
            //        ajaxFunction: _importRequestsdetail.getAll,
            //        inputFilter: getFilter
            //    },
            //    columnDefs: [

            //        {
            //            orderable: false,
            //            targets: 0,
            //            className: 'dt-body-center text-center',
            //            data: "nameItem"
            //        },
            //        {
            //            orderable: false,
            //            targets: 1,
            //            className: 'dt-body-center text-center',
            //            data: "unitName"
            //        },
            //        {
            //            orderable: false,
            //            targets: 2,
            //            className: 'dt-body-center text-center quantity',
            //            data: "quantity"
            //        },

            //        {
            //            orderable: false,
            //            className: 'dt-body-center text-center',
            //            targets: 3,
            //            data: "quantityHT",
            //            render: function (data, type, row, meta) {
            //                return html = `<th><input type="number" id="QuantityHT" name="QuantityHT" required class="form-control  QuantityHT" ></th>`;
            //            }
            //        },
            //        {
            //            orderable: false,
            //            targets: 4,
            //            render: function (data, type, row, meta) {
            //                //console.log(data, type, row, meta)
            //                //$('.QuantityHT').on("input", function () {
            //                //    var dInput = $(this).val();
            //                //    var output = $(this).parents('tr').find('td #QuantityCL');
            //                //    debugger
            //                //    output.val(dInput - row.quantity);
            //                //});
            //                return html = `<th><input type="number" id="QuantityCL"  name="QuantityCL" disabled  class="form-control QuantityCL" ></th>`;
            //            }
            //        },
            //    ],
            //    "initComplete": function (settings, json) {
            //        Minus()
            //    }
            //});

           // _importRequestsdetail.getAll(getFilter).done(functio)

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
                        data: "quantity",
                        render: $.fn.dataTable.render.number(',', ',', '')
                    },
                    {
                        orderable: false,
                        targets: 4,
                        className: "QR-code",
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<div class="QR-content" data-importDetai="` + row.id + `">
                                <span class="itemcode" data-ttemcode="`+ row.itemCode + `"></span>
                                <div id="barcode_`+ row.id + `" class="qr-img"></div>
                            </div>`;
                        }

                    },
                    {
                        orderable: false,
                        targets: 5,
                        data: "locationName",

                    },
                    {
                        orderable: false,
                        targets: 6,
                        className: "quantityInput",
                        render: function (data, type, row, meta) {
                            return `<input type="number" class="form-control custom-hiden-arrows QuantityInput w-75" min="1" max="` + row.quantity +`" required/>`;
                        }

                    },
                    {
                        orderable: false,
                        targets: 7,
                        className: "numberCompares",
                        render: function (data, type, row, meta) {
                            return `<input type="text" class="form-control Quantitycompare w-75" disabled />`;
                        }

                    },
                    {
                        orderable: false,
                        targets: 8,
                        className: "noteIn",
                        render: function (data, type, row, meta) {
                            return `<input type="text" class="form-control NoteInput" required/>`;
                        }

                    },

                ],
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


                    //Thay đổi khi nhập số
                    $(value).children('td.quantityInput').find("input").on("change", function () {
                        var valueNew = $(this).attr("max") - $(this).val();

                        $(value).children('td.numberCompares').find("input").val(valueNew);


                    })

                });

                $('input[type="number"]').on('keyup', function () { if ($(this).val() > $(this).attr('max') * 1) { $(this).val($(this).attr('max')); } });

            }

            function Minus() {
                $('#ItemTable tbody tr td input.QuantityHT').each(function (index, value) {
                    $(value).on("input", function () {
                        var dInput = parseInt($(this).val());
                        var output = $(this).parents('tr').find('td #QuantityCL');
                        var output1 = parseInt($(this).parents('tr').find('td.quantity').text());
                        output.val(dInput - output1);
                    });
                })
               
            }

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

            $('#ItemsTableView tbody tr').each(function (index, value) {
                $(value).addClass('was-validated');

                $(value).find('input'), function (index, value) {
                    if (value.checkValidity() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                        return;
                    }
                }
            });

            debugger

            var data = _frmIMP.serializeFormToObject();
            data.TransferId = $('#TransferId').val();
            data.ImportRequestSubsidiaryId = $('#ImportRequestSubsidiaryId').val();
            data.ImportStatus = 3;

            var dataImport = {};
            
            $('#ItemsTableView tbody tr').each(function (index, value) {
                dataImport.Id = $(value).children('td.QR-code').find(".QR-content").attr("data-importDetai");
                dataImport.QuantityReality = $(value).children('td.quantityInput').find(".QuantityInput").val();
                dataImport.Note = $(value).children('td.noteIn').find(".NoteInput").val();
                dataImport.IsItems = true;

                _warehouseLocation.updateIpoSubmit(dataImport).done(function () { })
            })
          
            _modalManager.setBusy(true);
            _importRequestService.updateSunmit(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Lưu thành công!');
                    abp.event.trigger('app.reloadDoneTable');
                    
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);