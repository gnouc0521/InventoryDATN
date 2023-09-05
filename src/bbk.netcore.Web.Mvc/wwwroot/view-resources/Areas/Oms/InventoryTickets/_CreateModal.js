(function ($) {

    app.modals.ImportRequestCreateModal = function () {


        var _inventoryTicketService = abp.services.app.inventoryTicketService;
        var _inventoryTicketdetail = abp.services.app.inventoryTicketDetailService;
        var _itemsServiceService = abp.services.app.itemsService;
        var _importRequestsdetail = abp.services.app.importRequestDetail;
        var _unitService = abp.services.app.unitService;
        var _modalManager;
        var _frmIMP = null;
        var _inventory = abp.services.app.inventoryService;


        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');
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

            $("#StartDate").datepicker({
                todayBtn: 1,
                autoclose: true,
            }).on('changeDate', function (selected) {
                var minDate = new Date(selected.date.valueOf());
                $('#EndDate').datepicker('setStartDate', minDate);

            });

            $("#EndDate").datepicker()
                .on('changeDate', function (selected) {
                    var maxDate = new Date(selected.date.valueOf());
                    $('#StartDate').datepicker('setEndDate', maxDate);
                });



            var getFilter = function () {
                let dataFilter = {};
                /*dataFilter.wareHouseId = $('#WarehouseId').find(':selected').val();*/
                dataFilter.wareHouseId = $('#WarehouseId').val();
                return dataFilter;
            }
            $('#WarehouseId').on('change', function () {
                $('#InvenTicketTable tbody tr').remove();
                $('#addRow').attr('disabled', false)
            })

            function AddRow() {
                _inventory.getAllItem(getFilter()).done(function (result) {
                    debugger
                    var dataselectItems = $.map(result.items, function (obj) {
                        obj.id = obj.itemId;
                        obj.text = obj.codeItem + "/" + obj.nameItem; // replace name with the property used for the text
                        return obj;

                    });
                    function tbodytr(length) {
                        var stt = length + 1
                        return html = `<tr class"row">
                                <th>`+ stt + `</th>
                                <th><select  class="form-control selectExport ItemId"  required>
                                <option value="" selected=""> Chọn hàng hóa </option>
                                </select></th>
                                <th><input type="number" id="" name="" class="form-control QuantityIN" disabled required></th>
                                <th><input type="number" id="QuantityOUT" name="QuantityOUT" class="form-control QuantityOUT" disabled required></th>
                                <th><input type="number" id="QuantityHT" name="QuantityHT" class="form-control QuantityHT" disabled required></th>
                                <th><input type="number" id="" name="" class="form-control Quantity" value="" required></th>
                                <th><input type="number" id="" name="" class="form-control QuantityCL" disabled value="" required></th>
                               <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;
                    }


                    var length = $('#InvenTicketTable tbody tr').length
                    $('#InvenTicketTable tbody ').append(tbodytr(length));
                    delete_row();

                    $("#InvenTicketTable tbody .selectExport").change(function () {
                        var selVal = [];
                        $("#InvenTicketTable tbody .selectExport").each(function () {
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
                        var data = e.params.data
                        var quanin = $(this).parents('tr').find('.QuantityIN').val(data.quantityIN)
                        var quanout = $(this).parents('tr').find('.QuantityOUT').val(data.quantityOUT)
                        var InQuaTotal = $(this).parents('tr').find('.QuantityHT').val(data.quantityIN - data.quantityOUT)

                        var getFilter = function () {
                            let dataFilter = {};
                            dataFilter.wareHouseId = $('#WarehouseId').find(':selected').val();
                            dataFilter.warehouseId = $('#WarehouseId').find(':selected').val();
                            dataFilter.itemId = data.itemId;
                            return dataFilter;
                        }
                        _inventory.getAllItem(getFilter()).done(function (result1) {
                            $.each(result1.items, function (index, value) {
                                $('.Quantity').on("input", function () {
                                    var dInput = this.value;
                                    var quancl = $(this).parents('tr').find('.QuantityCL')
                                    quancl.val(dInput - (data.quantityIN - data.quantityOUT))
                                });

                            })
                        })
                    }).trigger('change');
                })
            }

            if ($('#WarehouseId :selected').val() != undefined) {
                $('#addRow').click(function () {
                    AddRow();
                })
                delete_row();
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
                app.localize('Đóng phiếu kiểm kê'),
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
            var length = $('#InvenTicketTable tbody tr').length


            if (length == 0) {
                abp.message.warn(app.localize('Chưa nhập dữ liệu bảng'));
                $('#InvenTicketTable').DataTable().destroy();
            }
            else {
                var data = _frmIMP.serializeFormToObject();
                _modalManager.setBusy(true);
                _inventoryTicketService.create(data)
                    .done(function (result) {
                        _modalManager.close();
                        abp.notify.info('Thêm mới phiếu kiểm kê thành công!');
                        abp.event.trigger('app.reloadDocTable');
                        $('#InvenTicketTable tbody tr').each(function (index, value) {
                            data.InventoryTicketsId = result;
                            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                            data.QuantityIN = $(value).children('th').find('.QuantityIN ').val();
                            data.QuantityOUT = $(value).children('th').find('.QuantityOUT ').val();
                            data.QuantityHT = $(value).children('th').find('.QuantityHT ').val();
                            data.Quantity = $(value).children('th').find('.Quantity').val();
                            data.QuantityCL = $(value).children('th').find('.QuantityCL ').val();
                            data.WarehouseSourceId = $('#WarehouseId').val();
                            _inventoryTicketdetail.create(data)

                        })

                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            };
        };
    };
})(jQuery);