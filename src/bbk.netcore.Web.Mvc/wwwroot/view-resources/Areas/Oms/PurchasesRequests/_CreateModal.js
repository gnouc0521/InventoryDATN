          (function ($) {

    app.modals.PurchasesRequestCreateModal = function () {
        var _$itemTable = $('#ItemTable');
        var _purchasesRequestService = abp.services.app.purchasesRequestsService;
        var _purchasesRequestsdetail = abp.services.app.purchasesRequestDetailService;
        var _subsidiaryService = abp.services.app.subsidiaryService;
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var _SupplierService = abp.services.app.supplier;
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

            $('#SubsidiaryCompanyId').on('change', function () {
                $('#ItemTable tbody tr').remove();
                $('#addRow').attr('disabled', false)
                $('#fileupload').attr('disabled', false)
                document.getElementById('fileupload').addEventListener('change', handleFileSelect, false);
                var getFilter = function () {
                    let dataFilter = {};
                    dataFilter.subsidiaryCompanyId = $('#SubsidiaryCompanyId').find(':selected').val();
                    dataFilter.subsidiaryCompanyId = $('#SubsidiaryCompanyId').find(':selected').val();
                    return dataFilter;
                }
                _subsidiaryService.getAllPurchese(getFilter()).done(function (result) {
                    $.each(result.items, function (index, value) {
                        console.log(result.items)
                        fullPathProvince = 'province.json';
                        fullPathDistrict = 'district.json';
                        fullPathVillage = 'village.json';
                        var html = "";
                        function LoadAddress(filePath, idAddress, idSet, divview) {
                            _subsidiaryService.getAddress(filePath, idAddress).done((result) => {

                                for (let i = 0; i < result.addresses.length; i++) {
                                    if (result.addresses[i].id == idSet) {
                                        html += result.addresses[i].name + " , ";
                                        /*$(divview).html(result.addresses[i].name);*/
                                        $(divview).val(html);
                                    }
                                }
                            })
                        }

                        LoadAddress(fullPathProvince, "", value.cityId, "#Address");
                        LoadAddress(fullPathDistrict, value.cityId, value.districtId, "#Address");
                        LoadAddress(fullPathVillage, value.districtId, value.wardsId, "#Address");

                        $("#PhoneNumber").val(value.phoneNumber);
                        $("#EmailAddress").val(value.emailAddress)

                    })
                })
            });

            function tbodytr(length) {
                var stt = length + 1
                return html = `<tr>
                                <th>`+ stt + `</th>
                                <th><select  class="form-control selectExport ItemId" style="width:100%"  required>
                                <option value="" selected=""> Chọn hàng hóa </option>
                                </select></th>
                                <th><select  class="form-control selectSupplier SupplierId" style="width:100%"  required>
                                <option value="" selected=""> Chọn NCC </option>
                                </select></th>
                                <th><select size="1" id="row-1-office" class="form-control selectUnit UnitId" name="UnitId"  required >
                                <option value="" selected=""> Chọn đơn vị tính </option>
                                </select></th>
                                <th><input type="number" id="row-1-age" name="Quantity" class="form-control Quantity" value="" required></th>
                                <th><input type="text" id="Uses" name="Uses" class="form-control Uses" value="" required></th>
                                <th><input type="text" autocomplete="off" class="form-control date-picker requestDate" value="" placeholder="Nhập ngày" id="requestDate" name="requestDate" required></th>
                                <th><input type="text" id="Note" name="Note" class="form-control Note" value="" required></th>
                               <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;
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


            $('#addRow').click(function () {
                _itemsServiceService.getItemImportList().done(function (result) {
                    _SupplierService.getSupplierList().done(function (results) {
                        var dataselectItems = $.map(result, function (obj) {
                            obj.id = obj.id;
                            obj.text = obj.itemCode + "/" + obj.name;
                            return obj;
                        });

                        var dataselectSupplier = $.map(results, function (obj) {
                            obj.id = obj.id;
                            obj.text = obj.code; // replace name with the property used for the text
                            return obj;
                        });

                        var length = $('#ItemTable tbody tr').length
                        $('#ItemTable tbody ').append(tbodytr(length))
                        delete_row()
                        unit()

                        $(".selectSupplier").eq(0).trigger('change');
                        $('.selectSupplier').select2({
                            width: "100%",
                            dropdownParent: $('#ItemsCreateModal'),
                            placeholder: 'Chọn hàng hóa',
                            data: dataselectSupplier,
                        }).on('select2:select', function (e) {
                        }).trigger('change');


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
                    })

                    delete_row()
                })
            })


            var ExcelToJSON = function () {
                this.parseExcel = function (file) {

                    if (file.size > 10485760) {
                        abp.message.warn(app.localize('File_SizeLimit_Error'));
                        return false;
                    }
                    else {
                      
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var data = e.target.result;
                            var workbook = XLSX.read(data, {
                                type: 'binary'
                            });

                            workbook.SheetNames.forEach(function (sheetName) {
                                var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName], { raw: false });
                                var productList = JSON.parse(JSON.stringify(XL_row_object));
                                var sumColum = $('#ItemTable thead tr').find("th").length - 1;
                                var sumExcel = Object.keys(productList[0]).length;
                                var ColumExcel2 = productList[0].__EMPTY_1;
                                var ColumExcel3 = productList[0].__EMPTY_6;
                                var cloumnname1 = $('#ItemTable thead tr').find("th")[1].innerText;
                                var cloumnname2 = $('#ItemTable thead tr').find("th")[6].innerText;

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
                                            { targets: 7 },
                                            {
                                                targets: 8,
                                                render: function (data, type, row, meta) {
                                                    return ` <th class="text-center"></th>`;
                                                }
                                            }

                                        ],

                                    });
                                    for (i = 1; i < productList.length; i++) {
                                        var columns = Object.values(productList[i])
                                        table.rows.add([columns]).draw();
                                    }
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
                }
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


            document.getElementById("PhoneNumber").addEventListener("input", function () {
                var valueChange = funcChanePhoneNumber();
                _frmIMP.find('input[name=PhoneNumber]').val(valueChange);
            });
            function funcChanePhoneNumber() {
                var valueChange = null;
                var valueInputPhone = _frmIMP.find('input[name=PhoneNumber]').val();
                if (valueInputPhone.substring(0, 1) == 0) {
                    valueChange = _frmIMP.find('input[name=PhoneNumber]').val().replace('0', '');
                } else {
                    valueChange = _frmIMP.find('input[name=PhoneNumber]').val().replace(/[^0-9]/g, '');
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
                app.localize('Đóng'),
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
                abp.message.warn(app.localize('Chưa nhập dữ liệu bảng hàng hoá'));
                $('#ItemTable').DataTable().destroy();
            }
            else {
                var data = _frmIMP.serializeFormToObject();
                data.requestStatus = 2;
                _modalManager.setBusy(true);
                _purchasesRequestService.create(data)
                    .done(function (result) {
                        _modalManager.close();
                        abp.notify.info('Thêm mới phiếu yêu cầu thành công!');
                        abp.event.trigger('app.reloadDocTable');

                        if (document.getElementById("fileupload").files.length == 0) {
                            $('#ItemTable tbody tr').each(function (index, value) {
                                data.PurchasesRequestId = result;
                                data.SubsidiaryCompanyId = $('#SubsidiaryCompanyId').find(':selected').val();
                                data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                                data.SupplierId = $(value).children('th').find('.SupplierId option:selected').val();
                                data.Quantity = $(value).children('th').find('.Quantity ').val();
                                data.UnitId = $(value).children('th').find('.UnitId option:selected').val();
                                data.Uses = $(value).children('th').find('.Uses').val();
                                data.TimeNeeded = $(value).children('th').find('.requestDate ').val();
                                data.Note = $(value).children('th').find('.Note').val();
                                _purchasesRequestsdetail.create(data)
                            })
                        }
                        else {
                            var dataa = $('#ItemTable').DataTable().rows().data();
                            dataa.each(function (value, index) {
                                //get Item
                                _itemsServiceService.getItemByCode(value[1]).done(function (result3) {
                                    _unitService.getUnitByText(value[3]).done(function (unit) {
                                        _SupplierService.getSupByCode(value[2]).done(function (sup) {
                                            data.PurchasesRequestId = result;
                                            data.SubsidiaryCompanyId = $('#SubsidiaryCompanyId').find(':selected').val();
                                            data.ItemId = result3.id;
                                            data.SupplierId = sup.id;
                                            data.Quantity = value[4];
                                            data.UnitId = unit.id;
                                            data.Uses = value[5]
                                            data.TimeNeeded = value[6];
                                            data.Note = value[7]
                                            _purchasesRequestsdetail.create(data)
                                        })
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