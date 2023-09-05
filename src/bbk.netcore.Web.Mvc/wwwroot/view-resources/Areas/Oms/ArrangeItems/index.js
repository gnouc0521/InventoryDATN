(function () {
    var _$ArrangeItemsTable = $('#ArrangeItemsTable');
    var _$ArrangeItemsTables = $('#ImportRequestTable');
    var _warehouseLocationItemService = abp.services.app.warehouseLocationItemService;
    var _importRequestService = abp.services.app.importRequest;
    var _warehouseItemService = abp.services.app.warehouseItem;
    var _inventoryService = abp.services.app.inventoryService;
    moment.locale(abp.localization.currentLanguage.name);

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val();
        return dataFilter;
    }
    var dataTable = _$ArrangeItemsTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
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
            ajaxFunction: _warehouseLocationItemService.getAllItems,
            inputFilter: getFilter
        },
        rowGroup: {
            dataSrc: "warehouseName"
        },
        columnDefs: [
            {
                orderable: false,
                targets: 1,
                data: 'importRequestCode',
                render: function (data, type, row, meta) {
                    return `<span class="importRequestCode"  data-ImportRequestDetailId='` + row.importRequestDetailId + `' data-importRequestId="` + row.importRequestId + `">` + row.importRequestCode + ` </span>`
                }
            },
            {
                orderable: false,
                targets: 0,
                data: "itemCode",
                render: function (data, type, row, meta) {
                    return `<span class="itemCode"  data-warehouseId='` + row.warehouseId + `' data-ItemsId='` + row.itemId + `' data-ItemCode="` + row.itemCode + `">` + row.itemCode + ` </span>`
                }
            },
            {
                targets: 2,
                render: function (data, type, row, meta) {
                    return `<input type="number" class="form-control Quantity " name="Quantity" max="` + row.quantityImport + `" min="1" required >`
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "quantityImport",
                render: function (row) {
                    return `<input class="form-control" value="` + row + `" disabled>`
                }
            },
            {
                orderable: false,
                targets: 4,
                data: null,
                render: function () {
                    return `<input class="form-control" disabled>`
                }
            },
            {
                orderable: false,
                targets: 5,
                data: "importDate",
                class: 'importDate',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 6,
                data: null,
                render: function (data, type, row, meta) {
                    return `<select class="form-control ShelfId ShelfId` + meta.row + `" id="ShelfId" name="Block" data-warehouseId="` + row.warehouseId + `" required  >
                                <option value="" selected="" disabled>Chọn Khối</option>
                            </select>`
                }
            },
            {
                orderable: false,
                targets: 7,
                data: null,
                render: function (row) {
                    return `<select class="form-control FloorId" id="FloorId" data-warehouseId="` + row.warehouseId + `" name="FloorId" required >
                                <option value="" selected="" disabled>Chọn Khay</option>
                            </select>`
                }
            },
            {
                orderable: false,
                targets: 8,
                data: null,
                render: function (row) {
                    return `<select class="form-control BlockId" id="BlockId" data-warehouseId="` + row.warehouseId + `"  name="BlockId" required>
                                <option value="" selected="" disabled>Chọn Giá</option>
                            </select>`
                }
            },
            {
                orderable: false,
                targets: 9,
                data: null,
                class: "location",
                render: function () {
                    return `<span class="location"></span>`
                }
            },
            {
                orderable: false,
                targets: 10,
                data: null,
                render: function () {
                    return `<button class="btn btn-primary  waves-effect waves-themed">Lưu </button>`;
                }
            },
        ],
        "drawCallback": function (settings, json) {
            SelectLocations();
        },
    });

    //$('#ArrangeItemsTable').on('page.dt', function () {
    //    SelectLocations()
    //});



    var SelectLocations = function (e) {

        var selectBlock = $('#ArrangeItemsTable select.ShelfId')
        $.each(selectBlock, function (index, value1) {
            var warehouseId = $(value1).attr('data-warehouseId');
            _warehouseItemService.getAll(warehouseId).done(function (result) {
                $.each(result.items, function (index, value) {
                    html = `<option value=` + value.id + `>` + value.name + `</option>`;
                    $(value1).children('option:last').after(html).on('select2:select', function (e) {

                    });

                })
            })

            $(value1).change(function () {
                var idlocation = $(value1).find(':selected').val();
                $(value1).parents('tr').find('select.FloorId').remove();
                $(value1).parents('tr').find('select.BlockId').remove();
                var tdselectFloorid = $(value1).parents('tr').children('td:eq(7)')
                var tdquantitylocation = $(value1).parents('tr').children('td:eq(5)').find('input');
                var tdquantitylocationFree = $(value1).parents('tr').children('td:eq(4)').find('input');
                var tdselectBlockid = $(value1).parents('tr').children('td:eq(8)')
                var location = $(value1).parents('tr').children('td:eq(9)').empty();
                $(tdselectFloorid).append(`<select class="form-control FloorId" id="FloorId" data-warehouseId="` + warehouseId + `" name="FloorId" required  >
                                <option value="" selected="" disabled>Chọn Khay</option>
                            </select>`)
                $(tdselectBlockid).append(`<select class="form-control BlockId" id="BlockId" data-warehouseId="` + warehouseId + `"  name="BlockId" required>
                                <option value="" selected="" disabled>Chọn Giá</option>
                            </select>`)

                _warehouseItemService.getAllItemRoot($(this).val()).done(function (result) {
                    if (result.items == 0) {
                        _warehouseLocationItemService.countInLocation({ block: idlocation }).done(function (result) {
                            tdquantitylocation.val(result)
                        })
                        _warehouseLocationItemService.countInLocationFree({ block: idlocation }).done(function (result) {
                            tdquantitylocationFree.val(result)
                        })
                        _warehouseItemService.getinfoBin(id = idlocation).done(function (result) {
                            location.empty();
                            location.append(result)
                        })

                        tdselectFloorid.empty();
                        tdselectBlockid.empty();
                    } else {
                        $.each(result.items, function (index, value) {
                            html = `<option value=` + value.id + `>` + value.name + `</option>`;
                            $(tdselectFloorid).find('select.FloorId').children('option:last').after(html)

                        })
                    }

                })
                $(tdselectFloorid.find('select.FloorId')).change(function () {
                    var idlocation = $(this).find(':selected').val();
                    var ShelfId = $(this).parents('tr').find('select.BlockId')
                    _warehouseItemService.getAllItemSub($(this).val()).done(function (result) {
                        if (result.items == 0) {
                            _warehouseItemService.getinfoBin(id = idlocation).done(function (result) {
                                location.empty();
                                location.append(result)
                            })
                            _warehouseLocationItemService.countInLocation({ block: idlocation }).done(function (result) {
                                tdquantitylocation.val(result)
                            })
                            _warehouseLocationItemService.countInLocationFree({ block: idlocation }).done(function (result) {
                                tdquantitylocationFree.val(result)
                            })
                            tdselectBlockid.empty();
                        } else {
                            $.each(result.items, function (index, value) {
                                html = `<option data-unitmax=` + value.unitMax + ` value=` + value.id + `>` + value.name + `</option>`;
                                $(ShelfId).children('option:last').after(html)

                            })
                        }

                    })
                    var select = $('#ArrangeItemsTable select.BlockId')
                    $.each(select, function (index, value1) {
                        $(value1).on('change', function () {
                            var location = $(this).parents('tr').find('td.location');
                            var idlocation = $(value1).find(':selected').val();
                            _warehouseLocationItemService.countInLocation({ block: idlocation }).done(function (result) {
                                tdquantitylocation.val(result)
                            })
                            _warehouseLocationItemService.countInLocationFree({ block: idlocation }).done(function (result) {
                                tdquantitylocationFree.val(result)
                            })
                            _warehouseItemService.getinfoBin(id = idlocation).done(function (result) {
                                location.empty();
                                location.append(result)
                            })
                        })
                    })
                })

            })


        })



        $('input[type="number"]').on('keyup', function () { if ($(this).val() > $(this).attr('max') * 1) { $(this).val($(this).attr('max')); } });

        var button = $('#ArrangeItemsTable button')
        $(button).on('click', function () {
            var trrow = $(this).parents('tr');

            trrow.addClass('was-validated');
            var data = {};
            data.Quantity = trrow.find('.Quantity').val()
            data.ItemId = trrow.find('.itemCode').attr('data-ItemsId')
            data.ItemCode = trrow.find('.itemCode').attr('data-ItemCode')
            data.WarehouseId = trrow.find('.itemCode').attr('data-warehouseId')
            data.ImportRequestId = trrow.find('.importRequestCode').attr('data-importRequestId')
            data.ImportRequestDetailId = trrow.find('.importRequestCode').attr('data-ImportRequestDetailId')
            data.ImportDate = trrow.find('.importDate').text()
            data.Floor = trrow.find('.FloorId').val();
            data.Shelf = trrow.find('.ShelfId').val();
            data.Block = trrow.find('.BlockId').val();
            data.IsItems = false;
            data.QuantityReality = 0;
            data.Note = "";
            trrow.addClass('was-validated');


            //if (trrow.checkValidity() === false) {
            //    event.preventDefault();
            //    event.stopPropagation();
            //    return;
            //}
            var k = 0;
            $.each($(trrow[0]).find('input,select'), function (index, value) {
                k = 0;
                if (value.checkValidity() === false) {
                    k = k + 1;
                    event.preventDefault();
                    event.stopPropagation();
                    return ;
                }
            })

            if (k > 0) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            //_warehouseLocationItemService.create(data).done(function () {
            //    _inventoryService.create(data);
            //    getDocs();

            //});

            var dataFilter = {};
            dataFilter.warehouseId = trrow.find('.itemCode').attr('data-warehouseId')
            dataFilter.itemId = trrow.find('.itemCode').attr('data-ItemsId')
            dataFilter.block = trrow.find('.BlockId').val();
            dataFilter.shelf = trrow.find('.ShelfId').val();
            dataFilter.floor = trrow.find('.FloorId').val();;

            dataFilter.importRequestDetailId = trrow.find('.importRequestCode').attr('data-ImportRequestDetailId')


            // -------------------------- Code Ha update ------------------------------

            //update Phieu nhap
            var dataImport = {};
            dataImport.ImportStatus = 0;
            dataImport.Id = data.ImportRequestId;
            _importRequestService.updateStatusImport(dataImport).done(function () { });

            _warehouseLocationItemService.getItemSameInWare(dataFilter).done(function (result1) {

                if (result1.items.length == 0) {
                    _warehouseLocationItemService.create(data).done(function () {
                        /*_inventoryService.create(data);*/
                        getDocs();

                    });
                }
                else {
                    var dataUpdate = {};

                    dataUpdate.Id = result1.items[0].id;
                    dataUpdate.Quantity = result1.items[0].quantity + parseInt(data.Quantity);


                    _warehouseLocationItemService.updateItemQuantity(dataUpdate).done(function () {
                        /*_inventoryService.create(data);*/
                        getDocs();

                    });

                }

            })



        })
    }

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {

        dataTable.ajax.reload(SelectLocations);


    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });

    ////////// Select2 khối khay giá //////////////////





})(jQuery);

