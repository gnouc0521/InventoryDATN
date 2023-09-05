(function ($) {
    app.modals.ImportRequestEditModal = function () {

        var _$itemTable = $('#ItemTable');
        var _importRequestService = abp.services.app.importRequest;
        var _importRequestsdetail = abp.services.app.importRequestDetail;
        var _itemsServiceService = abp.services.app.itemsService;
        var _modalManager;
        var _unitService = abp.services.app.unitService;
        var _frmIMP = null;
        var arrdetails = [];
        var arrchange = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=FrmEdit]');
            function delete_row() {
                $('.delete_row').click(function () {
                    $(this).parents('tr').remove();
                })
            }

            $('.date-picker').datepicker({
                rtl: false,
                format: 'dd/mm/yyyy',
                orientation: "left",
                autoclose: true,
                language: abp.localization.currentLanguage.name,

            });

            var getFilter = function () {
                let dataFilter = {};
                dataFilter.importRequestId = $('#Id').val();
                return dataFilter;
            }


            function unit() {
                _unitService.getAll({}).done(function (results) {
                    $.each(results.items, function (index, value) {
                        if (value.parrentId == null) {
                            optgroup = `<optgroup data-parrent=` + value.id + ` label="` + value.name + `">` + `</optgroup>`
                            $('.UnitId option:last').after(optgroup)
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

            _itemsServiceService.getItemList().done(function (result) {
                function tbodytr(length) {
                    var stt = length + 1
                    return html = `<tr>
                                <th>`+ stt + `</th>
                                <th><select class="form-control selectExport ItemId `+ length + `"  required>
                                <option value="" selected=""></option>
                                </select></th>
                                <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control ImportPrice" value="" fdprocessedid="trygc" required></th>
                                <th><input type="number" id="row-1-age" name="Quantity_`+ length + `" class="form-control Quantity" value="" fdprocessedid="7ch0d" required></th>
                                <th><select size="1" id="row-1-office" class="form-control selectUnit UnitId" name="UnitId"  fdprocessedid="ee8fyd" >
                                <option value="" selected=""> Chọn đơn vị tính </option>
                                </select></th>
                                <th><input type="text" autocomplete="off" class="form-control date-picker MFG" value="" placeholder="Nhập ngày" id="MFG" name="MFG" required></th>
                                <th><input type="text" autocomplete="off" class="form-control date-picker ExpireDate" value="" placeholder="Nhập ngày" id="ExpireDate" name="ExpireDate" required ></th>
                                 <th class="text-center"><a class="delete_row"  href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;
                }

                _importRequestsdetail.getAll(getFilter()).done(function (result) {
                    $.each(result.items, function (index, value) {
                        arrdetails.push(value.id.toString());

                        var length = $('#ItemTable tbody tr').length
                        $('#ItemTable tbody').append(tbodytr(length))

                        delete_row()

                    })
                    $('#ItemTable').paging({
                        limit: 5,
                        rowDisplayStyle: 'block',
                        activePage: 0,
                        rows: []
                    });

                    _itemsServiceService.getItemImportList().done(function (result1) {
                        _unitService.getUnitList().done(function (unit) {
                            $.each($('.selectExport'), function (index1, value1) {
                                var sel = $('.selectExport');
                                $.each(result1, function (index, value) {
                                    abc = `<option value=` + value.id + `>` + value.itemCode + '/' + value.name + `</option>`
                                    $(sel[index1]).children('option:last').after(abc)
                                })
                            })

                            $.each($('.selectUnit'), function (index2, value2) {
                                var sel = $('.selectUnit');
                                $.each(unit, function (index, value) {
                                    abc = `<option value=` + value.id + `>` + value.name + `</option>`
                                    $(sel[index2]).children('option:last').after(abc)
                                })
                            })

                            $.each(result.items, function (index, value) {
                                var tr = $('#ItemTable tbody tr');
                                $(tr[index]).children('th:first').attr('data-id', value.id);
                                $(tr[index]).children('th').find('.ItemId').val(value.itemId);
                                $(tr[index]).children('th').find('.Quantity').val(value.quantity);
                                $(tr[index]).children('th').find('.ImportPrice').val(value.importPrice);
                                $(tr[index]).children('th').find('.UnitId').val(value.unitId);
                                $(tr[index]).children('th').find('.MFG').val(moment(value.mfg).format('L'));
                                $(tr[index]).children('th').find('.ExpireDate').val(moment(value.expireDate).format('L'));
                            })
                        })
                    })
                    $('.date-picker').datepicker({
                        rtl: false,
                        format: 'dd/mm/yyyy',
                        orientation: "left",
                        autoclose: true,
                        language: abp.localization.currentLanguage.name,

                    });

                    $("#MFG").datepicker({
                        todayBtn: 1,
                        autoclose: true,
                    }).on('changeDate', function (selected) {
                        var minDate = new Date(selected.date.valueOf());
                        $('#ExpireDate').datepicker('setStartDate', minDate);

                    });

                    $("#ExpireDate").datepicker()
                        .on('changeDate', function (selected) {
                            var maxDate = new Date(selected.date.valueOf());
                            $('#MFG').datepicker('setEndDate', maxDate);
                        });

                })




                $('#addRow').click(function () {
                    _frmIMP.addClass('was-validated');
                    if (_frmIMP[0].checkValidity() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                        return;
                    }
                    _itemsServiceService.getItemImportList().done(function (result1) {
                        var dataselectItems = $.map(result1, function (obj) {
                            obj.id = obj.id;
                            obj.text = obj.itemCode + "/" + obj.name;
                            return obj;
                        });

                        var length = $('#ItemTable tbody tr').length
                        $('#ItemTable tbody tr:last').after(tbodytr(length))
                        delete_row();
                        unit();

                        $("#ItemTable tbody .selectExport").change(function () {
                            var selVal = [];
                            $("#ItemTable tbody .selectExport").each(function () {
                                selVal.push(this.value);
                            });
                            var abc = $(this).parents('th').find('select')// .find("option")
                            $(abc).find("option").removeAttr("disabled").filter(function () {
                                var a = $(this).parent("select").val();
                                return (($.inArray(this.value, selVal) > -1) && (this.value != a))
                            }).attr("disabled", "disabled");

                        });

                        $(".selectExport").eq(0).trigger('change');

                        $('.selectExport').select2({
                            width: "100%",
                            dropdownParent: $('#ItemsCreateModal'),
                            placeholder: 'Chọn hàng hóa',
                            data: dataselectItems,
                        }).on('select2:select', function (e) {
                        }).trigger('change');

                        $('.date-picker').datepicker({
                            rtl: false,
                            format: 'dd/mm/yyyy',
                            orientation: "left",
                            autoclose: true,
                            language: abp.localization.currentLanguage.name,

                        });

                        $(".MFG").datepicker({
                            todayBtn: 1,
                            autoclose: true,
                        }).on('changeDate', function (selected) {
                            var minDate = new Date(selected.date.valueOf());
                            $('.ExpireDate').datepicker('setStartDate', minDate);

                        });

                        $(".ExpireDate").datepicker()
                            .on('changeDate', function (selected) {
                                var maxDate = new Date(selected.date.valueOf());
                                $('.MFG').datepicker('setEndDate', maxDate);
                            });
                    })
                });

            });


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
            _importRequestService.update(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Sửa thành công!');
                    abp.event.trigger('app.reloadDoneTable');
                    $('#ItemTable tbody tr').each(function (index, value) {
                        data.Id = $(value).children('th:first').attr('data-id');
                        if (data.Id != undefined) {
                            arrchange.push(data.Id);
                            data.ImportRequestId = result;
                            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                            data.Quantity = $(value).children('th').find('.Quantity ').val();
                            data.ImportPrice = $(value).children('th').find('.ImportPrice ').val();
                            data.UnitId = $(value).children('th').find('.UnitId option:selected').val();
                            data.UnitName = $(value).children('th').find('.UnitId option:selected').text();
                            data.MFG = $(value).children('th').find('.MFG ').val();
                            data.ExpireDate = $(value).children('th').find('.ExpireDate ').val();
                            _importRequestsdetail.update(data)
                        } else
                        {
                            data.ImportRequestId = result;
                            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                            data.Quantity = $(value).children('th').find('.Quantity ').val();
                            data.ImportPrice = $(value).children('th').find('.ImportPrice ').val();
                            data.UnitId = $(value).children('th').find('.UnitId option:selected').val();
                            data.UnitName = $(value).children('th').find('.UnitId option:selected').text();
                            data.MFG = $(value).children('th').find('.MFG ').val();
                            data.ExpireDate = $(value).children('th').find('.ExpireDate ').val();
                            _importRequestsdetail.create(data)
                        }
                    })
                    let difference = arrdetails.filter(x => !arrchange.includes(x));
                    difference.forEach(myFunction);
                    function myFunction(value, index, array) {
                        _importRequestsdetail.delete(value)
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);