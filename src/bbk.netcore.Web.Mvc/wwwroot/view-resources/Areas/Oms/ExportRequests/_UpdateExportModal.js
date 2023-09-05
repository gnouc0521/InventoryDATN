(function ($) {

    app.modals.ExportRequestsCreateModal = function () {


        var _exportRequests = abp.services.app.exportRequests;
        var _exportRequestsdetail = abp.services.app.exportRequestDetails;
        var _transferDetail = abp.services.app.transferDetail;
        var _location = abp.services.app.warehouseLocationItemService;
        var _inventory = abp.services.app.inventoryService;
        var _modalManager;
        var _frmDelivery = null;
        var _$ExportTable = $('#ExportRequestModal')



        this.init = function (modalManager) {
            $("#WarehouseDestinationId").val($('#WarehouseDestinationId_hidden').val())

            function delete_row() {
                $('.delete_row').click(function () {
                    $(this).parents('tr').remove();
                })
            }

            var getFilter = function () {
                let dataFilter = {};
                dataFilter.exportRequestId = $('#Id').val();
                dataFilter.warehouseId = $('#WarehouseDestinationId_hidden').val();
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
                    ajaxFunction: _exportRequestsdetail.getAllExport,
                    inputFilter: getFilter
                },
                order: [[1, 'asc']],
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
                        data: 'itemName',
                        class: 'ItemId',
                        render: function (data, type, row, meta) {
                            return `<div class=''> 
                                <span class='itemName'  data-id='` + row.id + `' data-objid='` + row.itemId + `'href='javascript:void(0); ' > ` + row.itemsName + ` </span>
                            </div>`;
                        }

                    },
                    {
                        orderable: false,
                        targets: 2,
                        data: 'quotePrice',
                        class: 'ExportPrice',
                        render: function (data, type, row, meta) {
                            return `<div class=''> 
                                <span class='quotePrice'  data-objid='` + row.exportPrice + `' > ` + row.exportPrice + ` </span>
                            </div>`;
                        }
                    },
                    {
                        orderable: false,
                        class: 'text-center',
                        targets: 3,
                        data: 'quantityTotal',

                    },
                    {
                        orderable: false,
                        targets: 4,
                        data: "quantityTransfer",
                        class: 'Quantity',
                        render: function (data, type, row, meta) {
                            return `<div class=''> 
                                <span class='quantityTransfer' data-objid='` + row.quantityExport + `'href='javascript:void(0); ' > ` + row.quantityExport + ` </span>
                            </div>`;
                        }

                    },
                    {

                        targets: 5,
                        data: 'unitName',
                        class: 'text-center',
                        orderable: false,
                        autoWidth: false,
                        class: 'UnitId',
                        render: function (data, type, row, meta) {
                            return `<div class=''> 
                                <span class='UnitId' data-objid='` + row.unitId + `'href='javascript:void(0); ' > ` + row.unitName + ` </span>
                            </div>`;
                        }
                    },
                    {

                        targets: 6,
                        data: null,

                        class: 'text-center',
                        orderable: false,
                        autoWidth: false,
                        render: function (data, type, row, meta) {
                            return `<div class=''> 
                                <span class='' href='javascript:void(0); ' >  `+ row.quantityLocation + ` </span>
                            </div>`;
                        }
                    },
                    {

                        targets: 7,
                        width: 200,
                        data: 'warehouseReceivingName',
                        class: 'text-center numberInput',
                        orderable: false,
                        autoWidth: false,
                        render: function (data, type, row, meta) {
                            return ` <input type="number"  class="form-control totalInput">`;
                        }
                    },
                    {

                        targets: 8,
                        data: 'warehouseReceivingName',
                        class: 'text-center selectNumber',
                        orderable: false,
                        autoWidth: false,
                        render: function (data, type, row, meta) {
                            if (row.floorName == null) {
                                return `<span data-BlockId='` + row.blockId + `' data-FloorId='` + row.floorId + `' data-ShelfId='` + row.shelfId + `' data-objid="` + row.locationId + `">` + row.shelfName + ` <\span>`
                            } else if (row.blockName == null && row.floorName != null) {
                                return `<span data-BlockId='` + row.blockId + `' data-FloorId='` + row.floorId + `' data-ShelfId='` + row.shelfId + `' data-objid="` + row.locationId + `">` + row.floorName + ` <\span>`
                            } else {
                                return `<span data-BlockId='` + row.blockId + `' data-FloorId='` + row.floorId + `' data-ShelfId='` + row.shelfId + `' data-objid="` + row.locationId + `">` + row.blockName + ` <\span>`
                            }
                        }
                    }, {

                        targets: 9,
                        data: null,
                        class: 'text-center',
                        orderable: false,
                        autoWidth: false,
                        render: function (data, type, row, meta) {
                            return `<a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a>`;
                        }
                    },

                ],
                "initComplete": function (settings, json) {
                    //addlocation();
                    // addrow();
                    // $('select', dataTable.rows().nodes()).select2();
                },

            })

            function initInputchange() {
                $('#ExportRequestModal tbody tr td input').each(function (index, value) {
                    inputchange(value)
                })
            }

            function reloadInputchange(_inputchange) {
                inputchange(_inputchange)
            }

            /// parm : DOM input change
            function inputchange(_inputchange) {
                //debugger
                let select = $(_inputchange).parents('tr').find('td select')
                let quantityRequiment = $(_inputchange).parents('tr').children('td.Quantity').find('.quantityTransfer').attr('data-objid')
                $(_inputchange).change(function () {

                    var quantityExport = $(this).val();
                    //if (parseInt(quantityRequiment) > parseInt(quantityExport)) {
                    if (4 > 2) {
                        $(select).select2("destroy")
                            .end()

                        var trNew = $($(select).parents('tr')).clone();
                        // remove option selected

                        var selected = $(select).find(':selected').val();
                        debugger
                        var tr = $(trNew[0].childNodes[8].childNodes[1]).find('option')
                        $.each(tr, function (index, value) {
                            if ($(this).val() == selected) {
                                $(this).remove();
                            }
                        })
                        // console.log(tr)
                        if (tr.length > 2) {
                            $('#ExportRequestModal tbody').append(trNew);
                            delete_row()
                            //  reloadInputchange($(trNew[0].childNodes[8].childNodes[1]))
                            debugger
                            select.select2({
                                tags: true,
                                width: "10em",
                                dropdownParent: $('#ExportRequestsCreateModal'),
                                placeholder: 'Chọn vị trí',
                            }).on('select2:select', function (e) {
                                // delete các dòng
                                select2change($(this))
                                //settingdata($(this), e)
                            })
                            $(trNew[0].childNodes[8].childNodes[1]).select2({
                                tags: true,
                                width: "10em",
                                dropdownParent: $('#ExportRequestsCreateModal'),
                                placeholder: 'Chọn vị trí',
                            }).on('select2:select', function (e) {
                                // delete các dòng
                                // settingdata($(this), e)
                                select2change($(this))
                            })
                        } else {
                            select.select2({
                                tags: true,
                                width: "10em",
                                dropdownParent: $('#ExportRequestsCreateModal'),
                                placeholder: 'Chọn vị trí',
                            }).on('select2:select', function (e) {
                                // delete các dòng
                                //settingdata($(this), e)
                                select2change($(this))
                            })
                        }

                    }
                })
            }

            function settingdata(select, e) {
                var data = e.params.data
                var select = $(select)

                // số lượng xuất
                var quantityexport = $(select).parents('tr').children('td.Quantity').find('.quantityTransfer').attr('data-objid');
                var idlocation = select.find(':selected').val();

                var tdquantitylocation = $(select).parents('tr').children('td:eq(6)').empty();
                // số lượng ở vị trí lấy ra 
                var quantitylocation = $(select).parents('tr').children('td:eq(6)').text();
                // số lượng lấy ra 

                select.parents('tr').find('td.numberInput input').removeAttr('disabled');

                select.parents('tr').find('td.numberInput input').val("");
                var tr = select.parents('tr');
                // inputchange(select.parents('tr').find('td.numberInput input'))
                _location.get({ id: idlocation }).done(function (result) {
                    tdquantitylocation.html(result.quantity)
                    var maxInput = Math.abs(parseInt(quantityexport) - result.quantity)
                    if (maxInput > parseInt(quantityexport)) {
                        maxInput = parseInt(quantityexport)
                    }
                    select.parents('tr').find('td.numberInput input').attr('max', maxInput);
                    $('input[type="number"]').on('keyup', function () { if ($(this).val() > $(this).attr('max') * 1) { $(this).val($(this).attr('max')); } });
                })
            }

            function initSelect2() {
                debugger
                $('#ExportRequestModal tbody tr').each(function (index, value) {
                    var select = $(value).children('td').find('.BlockId')
                    $.each(select, function (indexselect, valueselect) {
                        $(valueselect).select2({
                            tags: true,
                            width: "10em",
                            dropdownParent: $('#ExportRequestsCreateModal'),
                            placeholder: 'Chọn vị trí',
                        }).on('select2:select', function (e) {
                            settingdata(select, e)
                        })

                    })

                })
            }

            function select2change(_selectDOM) {
                $(_selectDOM).select2({
                    tags: true,
                    width: "10em",
                    dropdownParent: $('#ExportRequestsCreateModal'),
                    placeholder: 'Chọn vị trí',
                }).on('select2:select', function (e) {
                    // delete các dòng
                    settingdata(_selectDOM, e)
                })
                var inputDOM = $(_selectDOM).parents('tr').find('td input');
                inputchange(inputDOM)
            }

            function addlocation() {
                delete_row();
                // inputchange()
                $('#ExportRequestModal tbody tr').each(function (index, value) {
                    var select = $(value).children('td').find('.BlockId')
                    var inputitemId = $(value).children('td.ItemId').find('.itemName').attr('data-objid')
                    var inputwarehouseId = $('#WarehouseDestinationId').find(':selected').val()
                    $.each(select, function (indexselect, valueselect) {
                        _location.getLocationItems({ warehouseId: inputwarehouseId, itemId: inputitemId }).done(function (result) {
                            if ($(valueselect).find('option').length == 1) {
                                $.each(result.items, function (index1, value1) {
                                    if (value1.block == null && value1.floor == null) {
                                        var selectName = value1.shelfName;
                                    } else if (value1.block == null && value1.floor != null) {
                                        selectName = value1.floorName
                                    } else {
                                        selectName = value1.blockName
                                    }
                                    option = `<option value="` + value1.id + `" data-BlockId='` + value1.block + `' data-FloorId='` + value1.floor + `' data-ShelfId='` + value1.shelf + `' >` + selectName + `</option>`
                                    $(valueselect).find('option:last').after(option)
                                })
                            }
                        })
                    })
                })
                //addrow()
                initInputchange()
                initSelect2()
            }

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');
        }



        //sự kiện khi đóng modal
        $(".close-modal").on("click", function () {
            abp.libs.sweetAlert.config = {
                confirm: {
                    icon: 'warning',
                    buttons: ['Không', 'Có']
                }
            };
            abp.message.confirm(
                app.localize('Đóng'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _modalManager.close();
                        return true;

                    }
                }
            );
        })
        this.save = function () {


            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            //  var data = _frmDelivery.serializeFormToObject();
            _modalManager.setBusy(true);
            $('#ExportRequestModal tbody tr').each(function (index, value) {
                // var select = $(value).children('td').find('.BlockId')
                var id = $(value).find('td.selectNumber').find('span').attr('data-objid');
                var Quantity = $(value).children('td.Quantity').find('.quantityTransfer').attr('data-objid');
                var QuantityLocation = $(value).find('td.text-center.numberInput input').val();
                if (QuantityLocation =="") {
                    QuantityLocation = 0
                }
                debugger
                var ItemId = $(value).children('td.ItemId').find('.itemName').attr('data-objid');
                var WarehouseId = $('#WarehouseDestinationId_hidden').val();
                _exportRequests.updateStatus({ id: $('#Id').val(), status: 4 });
                _exportRequests.updateExportStatus({ id: $('#Id').val(), exportStatus: 2 });
                debugger
                $('#ExportRequestModal tbody tr').each(function (index, value) {
                    var data = {};
                    data.id = $(value).children('td.ItemId').find('.itemName').attr('data-id');
                   // data.WarehouseSourceId = $('#WarehouseDestinationId').find(':selected').val()
                    data.ExportRequestId = $('#Id').val();
                    data.ItemId = $(value).children('td.ItemId').find('.itemName').attr('data-objid');
                    data.Quantity = $(value).children('td.Quantity').find('.quantityTransfer').attr('data-objid');

                    data.ExportPrice = $(value).children('td.ExportPrice').find('.quotePrice').attr('data-objid');

                    //if ($(value).children('td.selectNumber').find('span').attr('data-BlockId') == 0) {
                    //    data.BlockId = 0;
                    //} else {
                    data.BlockId = $(value).children('td.selectNumber').find('span').attr('data-BlockId');
                    //}
                    // if ($(value).children('td.selectNumber').find('span').attr('data-FloorId') == 0) {
                    //     data.FloorId = 0;
                    //  } else {
                    data.FloorId = $(value).children('td.selectNumber').find('span').attr('data-FloorId');
                    //}
                    data.ShelfId = $(value).children('td.selectNumber').find('span').attr('data-ShelfId');
                    data.UnitId = $(value).children('td.UnitId').find('.UnitId').attr('data-objid')
                    _exportRequestsdetail.update(data)
                    debugger
                })
                console.log(QuantityLocation)
                _inventory.updateExport({ warehouseId: WarehouseId, itemId: ItemId, quantity: Quantity })
                _location.update({ id: id, quantity: QuantityLocation })
                    .done(function (result) {
                        _modalManager.close();
                        abp.notify.info('Cập nhật phiếu xuất thành công!');
                        abp.event.trigger('app.reloadDocTable');

                    }).always(function () {
                        _modalManager.setBusy(false);
                    });


            })


        }
    };
})(jQuery);




