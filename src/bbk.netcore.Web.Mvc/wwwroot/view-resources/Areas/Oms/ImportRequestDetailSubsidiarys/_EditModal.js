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
          
            var getFilter = function () {
                let dataFilter = {};
                dataFilter.importRequestId = $('#Id').val();
                return dataFilter;
            }

            var DataTable = _$itemTable.DataTable({
                paging: true,
                serverSide: false,
                processing: true,
                "searching": false,
                "language": {
                    "emptyTable": "Không tìm thấy dữ liệu",
                    "lengthMenu": "Hiển thị _MENU_ bản ghi",
                    "zeroRecords": "Không tìm thấy dữ liệu",
                    searchPlaceholder: "Tìm kiếm"
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
                order: [[0, 'asc']],
                columnDefs: [

                    {
                        orderable: false,
                        targets: 0,
                        className: 'dt-body-center text-center',
                        data: "nameItem"
                    },
                    {
                        orderable: false,
                        targets: 1,
                        className: 'dt-body-center text-center',
                        data: "unitName"
                    },
                    {
                        orderable: false,
                        targets: 2,
                        className: 'dt-body-center text-center',
                        data: "quantity"
                    },

                    {
                        orderable: false,
                        className: 'dt-body-center text-center',
                        targets: 3,
                        data: "quantityHT",
                        render: function (data, type, row, meta) {

                            return html = `<th><input type="number" id="QuantityHT" name="QuantityHT" required class="form-control  QuantityHT" ></th>`;
                        }
                    },
                    {
                        orderable: false,
                        targets: 4,
                        render: function (data, type, row, meta) {
                            $('.QuantityHT').on("input", function () {
                                var dInput = $(this).val();
                                var output = $(this).parents('tr').find('td #QuantityCL');
                                output.val(dInput - row.quantity);
                            });
                            return html = `<th><input type="number" id="QuantityCL"  name="QuantityCL" disabled  class="form-control QuantityCL" ></th>`;
                        }
                    },
                ],
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
                    var dataa = $('#ItemTable').DataTable().rows().data();
                    var QHT = $(".QuantityHT");
                    dataa.each(function (value, index) {
                        _itemsServiceService.getItemByCode(value.codeItem).done(function (result3) {
                            _unitService.getUnitByText(value.unitName).done(function (unit) {
                                data.Id = value.id;
                                data.ItemId = result3.id;
                                data.ImportRequestId = result;
                                data.ImportPrice = value.quotePrice;
                                data.Quantity = value.quantity;
                                data.UnitName = value.unitName;
                                data.UnitId = unit.id;
                                data.QuantityHT = $($(QHT)[index]).val()
                                data.ImportStatus = 0;
                                data.TransferId = $('#TransferId').val();
                                _importRequestsdetail.update(data);
                            })
                        });
                    });
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);