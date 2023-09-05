(function ($) {
    app.modals.ExportRequirementEditModal = function () {

        var _$ExportRequestModal = $('#ItemTable');
        var _exportRequests = abp.services.app.exportRequests;
        var _exportRequestsdetail = abp.services.app.exportRequestDetails;
        var _modalManager;
        var _frmIMP = null;
        var arrdetails = [];
        var arrchange = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=FrmEdit]');
           
            $("#WarehouseDestinationId").val($("#WarehouseDestinationId_hidden").val());
            $("#SubsidiaryId").val($("#SubsidiaryId_hidden").val());            


            var getFilter = function () {
                let dataFilter = {};
                dataFilter.exportRequestId = $('#Id').val()
                dataFilter.warehouseId = $('#WarehouseDestinationId').val()
                return dataFilter;
            }

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
                            return `<span>` + row.itemsCode + `-` + row.itemsName + `</span>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 2,
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<span>` + row.exportPrice + `</span>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 3,
                        data: 'quantityTotal',
                    },
                    {
                        orderable: false,
                        targets: 4,
                        data: 'quantityExport',
                    },
                     {
                        orderable: false,
                        targets: 5,
                         data: 'unitName',
                    },


                ],

            })

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
               
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);