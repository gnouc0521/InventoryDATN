(function ($) {

    app.modals.ImportRequestCreateModal = function () {

        var _$itemTable = $('#ItemTable');
        var _importRequestService = abp.services.app.importRequest;
        var _importRequestsdetail = abp.services.app.importRequestDetail;
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var _modalManager;
        var _frmIMP = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');

            function delete_row() {
                $('.delete_row').click(function () {
                    $(this).parents('tr').remove();
                })
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



            function tbodytr(length) {
                var stt = length + 1
                return html = `<tr>
                                <th>`+ stt + `</th>
                                <th><select  class="form-control selectExport ItemId" style="width:100%"  required>
                                <option value="" selected=""> Chọn hàng hóa </option>
                                </select></th>
                                <th><input type="number" id="row-1-age" name="ExportPrice_`+ length + `" class="form-control ImportPrice" value="" fdprocessedid="trygc" required></th>
                                <th><input type="number" id="row-1-age" name="Quantity_`+ length + `" class="form-control Quantity" value="" fdprocessedid="7ch0d" required></th>
                                <th><select size="1" id="row-1-office" class="form-control selectUnit UnitId" name="UnitId"  required >
                                <option value="" selected=""> Chọn đơn vị tính </option>
                                </select></th>
                                <th><input type="text" autocomplete="off" class="form-control date-picker MFG" value="" placeholder="Nhập ngày" id="MFG" name="MFG" required></th>
                                <th><input type="text" autocomplete="off" class="form-control date-picker ExpireDate" value="" placeholder="Nhập ngày" id="ExpireDate" name="ExpireDate" required></th>
                               <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;
            }

            $('#addRow').click(function () {
                _itemsServiceService.getItemImportList().done(function (result) {
                    var dataselectItems = $.map(result, function (obj) {
                        obj.id = obj.id;
                        obj.text = obj.itemCode + "/" + obj.name;
                        return obj;
                    });

                    var length = $('#ItemTable tbody tr').length
                    $('#ItemTable tbody ').append(tbodytr(length))
                    delete_row()
                    unit()

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
                delete_row()
            })


            var ExcelToJSON = function () {
                this.parseExcel = function (file) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var data = e.target.result;
                        var workbook = XLSX.read(data, {
                            type: 'binary'
                        });



                        workbook.SheetNames.forEach(function (sheetName) {
                            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName], { raw: false });
                            debugger
                            var productList = JSON.parse(JSON.stringify(XL_row_object));
                            var sumColum = $('#ItemTable thead tr').find("th").length - 1;
                            var sumExcel = Object.keys(productList[0]).length;
                            var ColumExcel2 = productList[0].__EMPTY_1;
                            var ColumExcel3 = productList[0].__EMPTY_5;
                            var cloumnname1 = $('#ItemTable thead tr').find("th")[1].innerText;
                            var cloumnname2 = $('#ItemTable thead tr').find("th")[5].innerText;


                            if (sumExcel == sumColum && cloumnname1.indexOf(ColumExcel2) != 1 && cloumnname2.indexOf(ColumExcel3) != 1 && ColumExcel2 != undefined) {
                                var table = $('#ItemTable').DataTable({
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
                                    pageLength: 5,
                                    columnDefs: [
                                        { targets: 0 },
                                        { targets: 1 },
                                        { targets: 2 },
                                        { targets: 3 },
                                        { targets: 4 },
                                        { targets: 5 },
                                        { targets: 6 },
                                        {
                                            targets: 7,
                                            render: function (data, type, row, meta) {
                                                return ` <th class="text-center"><a class="delete_row" href='javascript: void (0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>`;
                                            }
                                        }
                                    ],

                                });

                                for (i = 1; i < productList.length; i++) {
                                    var columns = Object.values(productList[i])
                                    table.rows.add([columns]).draw();
                                }
                                delete_row();
                            }
                            else {
                                abp.message.error('File không đúng định dạng', 'Mời chọn lại file theo mẫu');
                                return false;
                            }
                        })
                    };
                    reader.onerror = function (ex) {
                    };

                    reader.readAsBinaryString(file);
                };
            };

            function handleFileSelect(evt) {
                var files = evt.target.files; // FileList object
                var xl2json = new ExcelToJSON();
                xl2json.parseExcel(files[0]);
            }

            document.getElementById('fileupload').addEventListener('change', handleFileSelect, false);
            $('#fileupload').on('change', function () {
                $('#ItemTable').DataTable().destroy();
            })


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
            var length = $('#ItemTable tbody tr').length


            if (length == 0) {
                abp.message.warn(app.localize('Chưa nhập dữ liệu bảng'));
                $('#ItemTable').DataTable().destroy();
            }
            else {
                var data = _frmIMP.serializeFormToObject();
                _modalManager.setBusy(true);
                _importRequestService.create(data)
                    .done(function (result) {
                        _modalManager.close();
                        abp.notify.info('Thêm mới loại kho thành công!');
                        abp.event.trigger('app.reloadDocTable');
                        if (document.getElementById("fileupload").files.length == 0) {
                            $('#ItemTable tbody tr').each(function (index, value) {
                                data.ImportRequestId = result;
                                data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                                data.ImportPrice = $(value).children('th').find('.ImportPrice ').val();
                                data.Quantity = $(value).children('th').find('.Quantity ').val();
                                data.UnitId = $(value).children('th').find('.UnitId option:selected').val();
                                data.UnitName = $(value).children('th').find('.UnitId option:selected').text();
                                data.MFG = $(value).children('th').find('.MFG ').val();
                                data.ExpireDate = $(value).children('th').find('.ExpireDate ').val();
                                _importRequestsdetail.create(data)
                            })
                        }
                        else {
                            var dataa = $('#ItemTable').DataTable().rows().data();
                            dataa.each(function (value, index) {
                                //get Item
                                _itemsServiceService.getItemByCode(value[1]).done(function (result3) {
                                    _unitService.getUnitByText(value[4]).done(function (unit) {
                                        data.ItemId = result3.id;
                                        data.ImportRequestId = result;
                                        data.ImportPrice = value[2];
                                        data.Quantity = value[3];
                                        data.UnitName = value[4];
                                        data.UnitId = unit.id;
                                        data.MFG = value[5];
                                        data.ExpireDate = value[6]
                                        _importRequestsdetail.create(data);
                                    })
                                });
                            });
                        }
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            };
        }

    };
})(jQuery);