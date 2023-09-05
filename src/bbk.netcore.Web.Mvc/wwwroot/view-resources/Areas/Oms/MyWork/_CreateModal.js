(function ($) {

    app.modals.ExportCreateModal = function () {

        var _$ItemAssTable = $('#ItemAssTable');
        var _PurchasesSyn = abp.services.app.purchasesSynthesise;
        var _puS = abp.services.app.purchaseAssignmentService;
        var _useworkcount = abp.services.app.userWorkCountService;
        moment.locale(abp.localization.currentLanguage.name);
        var selected = new Array();
        var _modalManager;
        var _frmIMP = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');

            var getFilterQuoteApprove = function () {
                let dataFilter = {};
                dataFilter.id = $('#puchasesSynId').val();
                return dataFilter;
            }

            debugger

            var ContractdataTable = _$ItemAssTable.DataTable({
                paging: false,
                serverSide: false,
                processing: true,
                "searching": false,
                searching: false,
                "language": {
                    "emptyTable": "Không tìm thấy dữ liệu",
                    "lengthMenu": "Hiển thị _MENU_ bản ghi",
                },
                "bInfo": false,
                "bLengthChange": false,

                lengthMenu: [
                    [5, 10, 25, 50, -1],
                    [5, 10, 25, 50, 'Tất cả'],
                ],

                pageLength: 10,
                listAction: {
                    ajaxFunction: _PurchasesSyn.getItemsAssignments,
                    inputFilter: getFilterQuoteApprove
                },
                columnDefs: [
                    {
                        targets: 0,
                        orderable: false,
                        className: 'dt-body-center text-center',
                        render: function (data, type, row, meta) {
                            return '<input type="checkbox" name="" value="' + row.itemsId + '">';
                        }                    },

                    {
                        orderable: false,
                        targets: 1,
                        render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },
                    {
                        orderable: false,
                        targets: 2,
                        data: 'itemsName'
                    },
                    {
                        orderable: false,
                        targets: 3,
                        data: "unitName"
                    },
                    {
                        orderable: false,
                        targets: 4,
                        data: "quantity",
                        render: $.fn.dataTable.render.number(',', ',', '')
                    },
                    {
                        orderable: false,
                        targets: 5,
                        data: "note",
                    },
                ],
            });

            $('#example-select-all').on('click', function () {
                // Get all rows with search applied
                var rows = ContractdataTable.rows({ 'search': 'applied' }).nodes();
                // Check/uncheck checkboxes for all rows in the table
                $('input[type="checkbox"]', rows).prop('checked', this.checked);
                $('#ItemAssTable tbody input[type="checkbox"]:checked').each(function () {
                    selected.push($(this).val());
                });
            });

            // Handle click on checkbox to set state of "Select all" control
            $('#ItemAssTable tbody').on('change', 'input[type="checkbox"]', function () {
                // If checkbox is not checked
                $('#ItemAssTable tbody input[type="checkbox"]:checked').each(function () {
                    selected.push($(this).val());
                });
                if (!this.checked) {
                    var el = $('#example-select-all').get(0);
                    // If "Select all" control is checked and has 'indeterminate' property
                    if (el && el.checked && ('indeterminate' in el)) {
                        // Set visual state of "Select all" control
                        // as 'indeterminate'
                        el.indeterminate = true;
                    }
                }
            });

            abp.event.on('app.reloadgetDocContact', function () {
                getDocContact();
            });

            function getDocContact() {
                ContractdataTable.ajax.reload();
            }

        }


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



            _frmIMP.addClass('was-validated');
            if (_frmIMP[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }



            var data = {};
            _modalManager.setBusy(true);

            console.log(selected)

            exportReport($('#puchasesSynId').val())

            var varsupplierId = $("#SupplierId option:selected").val();

            function exportReport(id) {
                var filterObj = {
                    id: id,
                    supplierId: $("#SupplierId option:selected").val(),
                    listItems: selected
                };
                console.log(filterObj)
                abp.services.app.purchasesSynthesise.getPurchaseAssignmentListDto(filterObj)
                    .done(function (fileResult) {
                        location.href = abp.appPath + 'File/DownloadReportFile?fileType=' + fileResult.fileType + '&fileToken=' + fileResult.fileToken + '&fileName=' + fileResult.fileName;
                        _modalManager.close();
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });;
            }

        };
    };
})(jQuery);