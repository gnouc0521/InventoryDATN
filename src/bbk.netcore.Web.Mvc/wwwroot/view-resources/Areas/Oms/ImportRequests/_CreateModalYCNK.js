(function ($) {

    app.modals.ImportRequestCreateModal = function () {


         var _$itemTable = $('#ItemTable');
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var impdSup = abp.services.app.importRequestDetailSubidiaryService;
        var _importRequestService = abp.services.app.importRequest;
        var _importRequestsdetail = abp.services.app.importRequestDetail;
        var _modalManager;
        var _frmIMP = null;
        var _impsService = abp.services.app.importRequestSubidiaryService;


        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');

            
            var test = $('#WarehouseId').val();
            $("#WarehouseDestinationId").val(test).change();

            var getFilter = function () {
                let dataFilter = {};
                dataFilter.importRequesSubtId = $('#Id').val().trim();
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
                    ajaxFunction: impdSup.getAll,
                    inputFilter: getFilter
                },
                order: [[0, 'asc']],
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
                        className: "nameItem",
                        data: "itemcode"
                    },
                    {
                        orderable: false,
                        targets: 2,
                        className: "orderPrice",
                        data: "price"
                    },
                    {
                        orderable: false,
                        targets: 3,
                        className: "quantity",
                        data: "quantity"
                    },

                    {
                        orderable: false,
                        targets: 4,
                        className: "dateTimeNeed",
                        data: "dateTimeNeed",
                        render: function (creationTime) {
                            return moment(creationTime).format('L');
                        }
                    },
                    {
                        orderable: false,
                        targets: 5,
                        className: "unitName",
                        data: "unitName"
                    },
                ]
            });
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
            var id = $('#Id').val();
            data.importRequestSubsidiaryId = id;
            _modalManager.setBusy(true);
            _importRequestService.create(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới yêu cầu thành công!');
                  
                    $('#ItemTable tbody tr').each(function (index, value) {
                        var code = $(value).children('td.nameItem').text().split("-")[0];
                        _itemsServiceService.getItemByCode(code).done(function (result3) {
                            _unitService.getUnitByText($(value).children('td.unitName').text()).done(function (unit) {
                                data.ItemId = result3.id;
                                data.ImportRequestId = result;
                                data.Quantity = $(value).children('td.quantity').text();
                                data.UnitName = $(value).children('td.unitName').text();
                                data.UnitId = unit.id;
                                data.ImportPrice = $(value).children('td.orderPrice').text();
                                data.Status = $('#Status').val();
                                data.TimeNeeded = $(value).children('td.dateTimeNeed').text();;;
                                data.StatusImport = false;
                                _impsService.updateStatusIMP(data);
                                _importRequestsdetail.create(data);
                                abp.event.trigger('app.reloadTranferTable');
                                abp.event.trigger('app.reloadDocTable');
                            });
                        });
                    });
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);