(function ($) {
    app.modals.ExportRequestsEditModal = function () {

        var _$ExportRequestModal = $('#ExportRequestModal');
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var _exportRequests = abp.services.app.exportRequests;
        var _exportRequestsdetail = abp.services.app.exportRequestDetails;
        var _warehouseItem = abp.services.app.warehouseItem;
        var _warehouseLocationItemService = abp.services.app.warehouseLocationItemService;
        var _inventory = abp.services.app.inventoryService;
        var _modalManager;
        var _frmIMP = null;
        var arrdetails = [];
        var arrchange = [];

        //html = `<tr>
        //                        <th>`+ stt + `</th>
        //                        <th><select size="1" id="row-1-office" class="form-control selectExport ItemId" name="ItemId_`+ length + `"  required>
        //                        <option value="" selected=""> Chọn hàng hóa </option>
        //                        </select></th>
        //                        <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control ExportPrice"  required></th>
        //                        <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control QuantityTotal"  disabled></th>
        //                        <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control Quantity" required></th>
        //                        <th><select size="1" id="row-1-office" class="form-control UnitId" name="row-1-office">
        //                        <option value="" selected=""> Chọn đơn vị </option>
        //                        </select></th>
        //                        <th><select size="1" id="row-1-office" class="form-control ShelfId" name="row-1-office"  ></select></th>
        //                        <th><select size="1" id="row-1-office" class="form-control FloorId" name="row-1-office"  ></select></th>
        //                        <th><select size="1" id="row-1-office" class="form-control BlockId" name="row-1-office"></select></th>
        //                        <th><p class=" LocationId mb-0 text-wrap" name="row-1-office" ></p></th>
        //                        <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
        //                    </tr>`;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=FrmEdit]');

            function delete_row() {
                $('.delete_row').click(function () {
                    $(this).parents('tr').remove();
                })
            }
            $("#WarehouseDestinationId").val($("#WarehouseDestinationId_hidden").val());

            var getFilter = function () {
                let dataFilter = {};
                dataFilter.exportRequestId = $('#Id').val();
                dataFilter.wareHouseId = $('#WarehouseDestinationId').find(':selected').val();
                dataFilter.warehouseId = $('#WarehouseDestinationId').find(':selected').val();
                dataFilter.itemId = $(this).find(':selected').val();
                return dataFilter;
            }

            $('#WarehouseDestinationId').on('change', function () {
                if ($("#WarehouseDestinationId_hidden").val() == $('#WarehouseDestinationId').find(':selected').val()) {
                    loadtable()
                }
            });

            var dataTable = _$ExportRequestModal.DataTable({
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
                    ajaxFunction: _exportRequestsdetail.getAll,
                    inputFilter: getFilter
                },
                columnDefs: [

                    {
                        targets: 0,
                        width: 1,
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
                            return `<select size="1" id="row-1-office" data-itemId=` + row.itemId + ` class="form-control selectExport ItemId" name="ItemId_` + meta.row + 1 + `"  required>
                                <option value="" selected=""> Chọn hàng hóa </option>
                                </select>`
                            // `<span>` + row.itemsCode + `-` + row.itemsName + `</span>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 2,
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<input type="number" id="row-1-age" name="ExportPrice_` + meta.row + 1 + `" class="form-control ExportPrice"  value="` + row.exportPrice + `" required>`
                        }
                    },

                    {
                        orderable: false,
                        targets: 3,
                        data: 'quantityTotal',
                        render: function (data, type, row, meta) {
                            return `<input type="number" id="row-1-age" name="ExportPrice_` + meta.row + 1 + `" class="form-control QuantityTotal"  value="` + row.quantityTotal + `" required>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 4,
                        data: 'quantityExport',
                        render: function (data, type, row, meta) {
                            return `<input type="number" id="row-1-age" name="ExportPrice_` + meta.row + 1 + `" class="form-control Quantity"  value="` + row.quantityExport + `" required>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 5,
                        data: 'unitName',
                        render: function (data, type, row, meta) {
                            return `<select size="1" id="row-1-office" data-unitid=` + row.unitId +` class="form-control UnitId" name="row-1-office">
                                <option value="" selected=""> Chọn đơn vị </option>
                                </select>`
                        }
                    }, {
                        orderable: false,
                        targets: 6,
                        width: 1,
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a>`
                            // `<span>` + row.itemsCode + `-` + row.itemsName + `</span>`
                        }
                    },

                ],
                "initComplete": function (settings, json) {
                    delete_row();
                    //  select2()
                    loadItemAndUnit()
                }
            })






            function unit(tr) {
                _unitService.getAll({}).done(function (results) {
                    $.each(results.items, function (index, value) {
                        if (value.parrentId == null) {
                            optgroup = `<optgroup data-parrent=` + value.id + ` label="` + value.name + `">` + `</optgroup>`
                            $(tr).find('.UnitId option:last').after(optgroup)
                        }
                    })
                    var listopgroup = $('.UnitId optgroup')
                    $.each(listopgroup, function (index, value) {
                        let dataparrent = value.dataset.parrent;
                        $.each(results.items, function (index, valueoption) {
                            if (valueoption.parrentId == dataparrent) {
                                option = `<option value="` + valueoption.id + `">` + valueoption.name + `</option>`
                                $(value).append(option)
                            }
                        })
                    })
                })
            }
            function tbodytr(length) {
                var stt = length + 1
                return html = `<tr>
                                <th class='text-center'>`+ stt + `</th>
                                <th><select size="1" id="row-1-office" class="form-control selectExport ItemId" name="ItemId_`+ length + `"  required>
                                <option value="" selected=""> Chọn hàng hóa </option>
                                </select></th>
                                <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control ExportPrice"  required></th>
                                <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control QuantityTotal"  disabled></th>
                                <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control Quantity" required></th>
                                <th><select size="1" id="row-1-office" class="form-control UnitId" name="row-1-office">
                                <option value="" selected=""> Chọn đơn vị </option>
                                </select></th>
                                <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;
            }

            function loadItemAndUnit() {
                _inventory.getAll(getFilter()).done(function (result) {
                    var dataselectItems = $.map(result.items, function (obj) {
                        obj.id = obj.itemId;
                        obj.text = obj.nameCode; // replace name with the property used for the text
                        return obj;

                    });
                    // add option after add row
                    $("#ExportRequestModal tbody .selectExport").change(function () {
                        var selVal = [];
                        $("#ExportRequestModal tbody .selectExport").each(function () {
                            selVal.push(this.value);
                        });

                        var abc = $(this).parents('th').find('select')
                        $(abc).find("option").removeAttr("disabled").filter(function () {

                            var a = $(this).parent("select").val();
                            return (($.inArray(this.value, selVal) > -1) && (this.value != a))
                        }).attr("disabled", "disabled");

                    });
                    $(".selectExport").eq(0).trigger('change');
                    // select Items
                    $('.selectExport').select2({
                        width: "100%",
                        dropdownParent: $('#ExportRequestsEditModal'),
                        placeholder: 'Chọn hàng hóa',
                        data: dataselectItems,
                    }).on('select2:select', function (e) {
                    }).trigger('change');
                })

                _unitService.getAll({}).done(function (results) {
                    $.each(results.items, function (index, value) {
                        if (value.parrentId == null) {
                            $(' #ExportRequestModal tbody .UnitId').each(function (index1, value1) {
                                optgroup = `<optgroup data-parrent=` + value.id + ` label="` + value.name + `">` + `</optgroup>`;
                                $(value1).find('option:last').after(optgroup)
                            })
                        }

                    })
                    var listopgroup = $(' #ExportRequestModal tbody .UnitId optgroup')
                    $.each(listopgroup, function (index, value) {
                        let dataparrent = value.dataset.parrent;
                        $.each(results.items, function (index, valueoption) {
                            if (valueoption.parrentId == dataparrent) {
                                option = `<option value="` + valueoption.id + `">` + valueoption.name + `</option>`
                                $(value).append(option)
                            }
                        })
                    })

                    debugger
                   
                    var tr = $('#ExportRequestModal tbody tr');
                    $.each(tr, function (index, value) {
                        $(tr[index]).children('td').find('.ItemId').val($(tr[index]).children('td').find('.ItemId').attr('data-itemid'));
                        $(tr[index]).children('td').find('.ItemId').select2( { id: $(tr[index]).children('td').find('.ItemId').attr('data-itemid'), a_key: 'Lorem Ipsum' });
                        debugger
                        $(tr[index]).children('td').find('.UnitId').val($(tr[index]).children('td').find('.UnitId').attr('data-unitId'));
                    })
                })


            }


            //function select2() {
            //    _inventory.getAll(getFilter()).done(function (result) {
            //        var dataselectItems = $.map(result.items, function (obj) {
            //            obj.id = obj.itemId;
            //            obj.text = obj.nameCode; // replace name with the property used for the text
            //            return obj;

            //        });
            //        // add option after add row
            //        $("#ExportRequestModal tbody .selectExport").change(function () {
            //            var selVal = [];
            //            $("#ExportRequestModal tbody .selectExport").each(function () {
            //                selVal.push(this.value);
            //            });

            //            var abc = $(this).parents('th').find('select')
            //            $(abc).find("option").removeAttr("disabled").filter(function () {

            //                var a = $(this).parent("select").val();
            //                return (($.inArray(this.value, selVal) > -1) && (this.value != a))
            //            }).attr("disabled", "disabled");

            //        });
            //        $(".selectExport").eq(0).trigger('change');
            //        // select Items
            //        $('.selectExport').select2({
            //            width: "100%",
            //            dropdownParent: $('#ExportRequestsEditModal'),
            //            placeholder: 'Chọn hàng hóa',
            //            data: dataselectItems,
            //        }).on('select2:select', function (e) {
            //        }).trigger('change');
            //    })
            //    }

            //function loadtable() {
            //    _exportRequestsdetail.getAll(getFilter()).done(function (result) {
            //        $.each(result.items, function (index, value) {
            //            arrdetails.push(value.id.toString());
            //            var length = $('#ExportRequestModal tbody tr').length
            //            $('#ExportRequestModal tbody').append(tbodytr(length))
            //            debugger
            //            _unitService.getAll({}).done(function (results) {
            //                $.each(results.items, function (index, value) {
            //                    if (value.parrentId == null) {
            //                        $(' #ExportRequestModal tbody .UnitId').each(function (index1, value1) {
            //                            optgroup = `<optgroup data-parrent=` + value.id + ` label="` + value.name + `">` + `</optgroup>`;
            //                            $(value1).find('option:last').after(optgroup)
            //                        })
            //                    }

            //                })
            //                var listopgroup = $(' #ExportRequestModal tbody .UnitId optgroup')
            //                $.each(listopgroup, function (index, value) {
            //                    let dataparrent = value.dataset.parrent;
            //                    $.each(results.items, function (index, valueoption) {
            //                        if (valueoption.parrentId == dataparrent) {
            //                            option = `<option value="` + valueoption.id + `">` + valueoption.name + `</option>`
            //                            $(value).append(option)
            //                        }
            //                    })
            //                })

            //                debugger
            //                $.each(result.items, function (index, value) {
            //                    var tr = $('#ExportRequestModal tbody tr');
            //                    $(tr[index]).children('th:first').attr('data-id', value.id);
            //                    $(tr[index]).children('th').find('.ItemId').val(value.itemId);
            //                    $(tr[index]).children('th').find('.UnitId').val(value.unitId);
            //                    $(tr[index]).children('th').find('.Quantity').val(value.quantityExport);
            //                    $(tr[index]).children('th').find('.ExportPrice').val(value.exportPrice);
            //                    $(tr[index]).children('th').find('.QuantityTotal').val(value.quantityTotal);
            //                    $(tr[index]).children('th').find('.QuantityTotal').val(value.quantityTotal);
            //                    var data = value
            //                })
            //            })
            //            delete_row();

            //        })


            //        _inventory.getAll(getFilter()).done(function (result) {
            //            var dataselectItems = $.map(result.items, function (obj) {
            //                obj.id = obj.itemId;
            //                obj.text = obj.nameCode; // replace name with the property used for the text
            //                return obj;

            //            });
            //            // add option after add row
            //            $("#ExportRequestModal tbody .selectExport").change(function () {
            //                var selVal = [];
            //                $("#ExportRequestModal tbody .selectExport").each(function () {
            //                    selVal.push(this.value);
            //                });

            //                var abc = $(this).parents('th').find('select')
            //                $(abc).find("option").removeAttr("disabled").filter(function () {

            //                    var a = $(this).parent("select").val();
            //                    return (($.inArray(this.value, selVal) > -1) && (this.value != a))
            //                }).attr("disabled", "disabled");

            //            });
            //            $(".selectExport").eq(0).trigger('change');
            //            // select Items
            //            $('.selectExport').select2({
            //                width: "100%",
            //                dropdownParent: $('#ExportRequestsEditModal'),
            //                placeholder: 'Chọn hàng hóa',
            //                data: dataselectItems,
            //            }).on('select2:select', function (e) {
            //            }).trigger('change');
            //        })
            //    })
            //}
            //loadtable();

            $('#addRow').click(function () {

                _inventory.getAll(getFilter()).done(function (result) {
                    var length = $('#ExportRequestModal tbody tr').length
                    $('#ExportRequestModal tbody').append(tbodytr(length))
                    var trlast = $('#ExportRequestModal tbody tr:last')
                    unit(trlast);
                    delete_row();
                    var dataselectItems = $.map(result.items, function (obj) {
                        obj.id = obj.itemId;
                        obj.text = obj.nameCode; // replace name with the property used for the text
                        return obj;

                    });
                    // add option after add row
                    $("#ExportRequestModal tbody .selectExport").change(function () {
                        var selVal = [];
                        $("#ExportRequestModal tbody .selectExport").each(function () {
                            selVal.push(this.value);
                        });

                        var abc = $(this).parents('th').find('select')
                        $(abc).find("option").removeAttr("disabled").filter(function () {

                            var a = $(this).parent("select").val();
                            console.log(a)
                            return (($.inArray(this.value, selVal) > -1) && (this.value != a))
                        }).attr("disabled", "disabled");

                    });


                    $(".selectExport").eq(0).trigger('change');


                    // select Items
                    $('.selectExport').select2({
                        width: "100%",
                        dropdownParent: $('#ExportRequestsEditModal'),
                        placeholder: 'Chọn hàng hóa',
                        data: dataselectItems,
                    }).on('select2:select', function (e) {

                        var data = e.params.data
                        var InQuaTotal = $(this).parents('tr').find('.QuantityTotal').val(data.quantity)
                    }).trigger('change');
                })
            })

            $('.date-picker').datepicker({
                format: 'dd/mm/yyyy',
                orientation: "left",
                autoclose: true,
                language: abp.localization.currentLanguage.name,

            });
            document.getElementById("phone").addEventListener("input", function () {
                var valueChange = funcChanePhoneNumber();
                _frmIMP.find('input[name=Phone]').val(valueChange);
            });
            function funcChanePhoneNumber() {
                var valueChange = null;
                var valueInputPhone = _frmIMP.find('input[name=Phone]').val();
                if (valueInputPhone.substring(0, 1) == 0) {
                    valueChange = _frmIMP.find('input[name=Phone]').val().replace('0', '');
                } else {
                    valueChange = _frmIMP.find('input[name=Phone]').val().replace(/[^0-9]/g, '');
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
            _exportRequests.update(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Sửa thành công!');
                    abp.event.trigger('app.reloadDocTable');
                    $('#ExportRequestModal tbody tr').each(function (index, value) {
                        data.Id = $(value).children('th:first').attr('data-id');
                        if (data.Id != undefined) {
                            arrchange.push(data.Id);
                            data.ExportRequestId = result;
                            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                            data.Quantity = $(value).children('th').find('.Quantity ').val();
                            data.ExportPrice = $(value).children('th').find('.ExportPrice ').val();
                            data.UnitId = $(value).children('th').find('.UnitId option:selected').val()
                            data.WarehouseSourceId = data.WarehouseDestinationId
                            if ($(value).children('th').find('.BlockId option:selected').val() != "") {
                                data.BlockId = $(value).children('th').find('.BlockId option:selected').val();

                            }
                            data.FloorId = $(value).children('th').find('.FloorId option:selected').val();
                            data.ShelfId = $(value).children('th').find('.ShelfId option:selected').val();
                            _exportRequestsdetail.update(data)
                        } else {
                            data.ExportRequestId = result;
                            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                            data.Quantity = $(value).children('th').find('.Quantity ').val();
                            data.ExportPrice = $(value).children('th').find('.ExportPrice ').val();
                            data.WarehouseSourceId = data.WarehouseDestinationId
                            data.UnitId = $(value).children('th').find('.UnitId option:selected').val()
                            if ($(value).children('th').find('.BlockId option:selected').val() != "") {
                                data.BlockId = $(value).children('th').find('.BlockId option:selected').val();

                            }
                            data.FloorId = $(value).children('th').find('.FloorId option:selected').val();
                            data.ShelfId = $(value).children('th').find('.ShelfId option:selected').val();
                            _exportRequestsdetail.create(data)
                        }
                    })
                    let difference = arrdetails.filter(x => !arrchange.includes(x));
                    difference.forEach(myFunction);
                    function myFunction(value, index, array) {
                        _exportRequestsdetail.delete(value)
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);