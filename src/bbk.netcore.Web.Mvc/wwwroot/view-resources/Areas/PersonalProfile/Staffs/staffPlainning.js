(function ($) {
    moment.locale(abp.localization.currentLanguage.name);
    var _staffPlainningService = abp.services.app.staffPlainning;
    var _$table = $('#StaffPlainningTable');
    var _$modal = $('#Modal');
    var _$staffPlainningTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _staffPlainningService.getAll({ personId })
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$table);
                });
        },
        order: [],
        rowId: "id",
        columnDefs: [
            {
                cellType: 'th',
                targets: 0,
                data: 'decisionNumber',
                sortable: false
            },
            {
                targets: 1,
                render: (data, type, row) => {
                    return `${moment(row.issuedDate).format('L')}`
                },
                sortable: false
            },
            {
                targets: 2,
                data: 'issuedLevels',
                sortable: false
            },
            {
                targets: 3,
                render: (data, type, row) => (`${row.fromDate}-${row.toDate}`),
                sortable: false
            },
            {
                targets: 4,
                data: 'workingTitle',
                sortable: false
            },
            {
                targets: 5,
                data: 'workingTitle1',
                sortable: false
            },
            {
                targets: 6,
                data: 'workingTitle2',
                sortable: false
            }
        ]
    });

    abp.event.on('staffPlainning.updated', (data) => {
        _$staffPlainningTable.ajax.reload();
    });

    $('#StaffPlainningTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$staffPlainningTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $(document).on('click', '.staff-plainning .refresh', function (e) {
        abp.event.trigger('staffPlainning.updated');
    });

    $('.staff-plainning .delete').click(function () {
        var staffPlainningId = _$staffPlainningTable.row('.selected').id();
        if (staffPlainningId) {
            var personId = $("#PersonId").val();
            abp.message.confirm(
                abp.utils.formatString("Thông tin quy hoạch sẽ bị xóa!"),
                "Bạn có chắc chắn muốn xóa",
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$table);
                        _staffPlainningService.deleteById({ id: staffPlainningId, personId })
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                abp.event.trigger("staffPlainning.updated");
                            }).always(function () {
                                abp.ui.clearBusy(_$table);
                            });
                    }
                }
            );
        } else {
            abp.notify.error("Chọn trước khi xóa!");
        }
    });

    $(document).on('click', '.staff-plainning .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditStaffPlainningModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                _$modal.modal('toggle');
                $('#Modal div.modal-dialog').html(content);
                if (jQuery().datepicker) {
                    $('#Modal .date-picker').datepicker({
                        rtl: false,
                        orientation: "left",
                        autoclose: true,
                        language: abp.localization.currentLanguage.name
                    });
                }
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.staff-plainning .edit', function (e) {
        var id = _$staffPlainningTable.row('.selected').id();
        if (id) {
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditStaffPlainningModal/${id}`,
                type: 'Get',
                dataType: 'html',
                success: function (content) {
                    $('#Modal div.modal-dialog').html(content);
                    $('#Modal').modal('toggle');
                    if (jQuery().datepicker) {
                        $('#Modal .date-picker').datepicker({
                            rtl: false,
                            orientation: "left",
                            autoclose: true,
                            language: abp.localization.currentLanguage.name
                        });
                    }
                },
                error: function (e) {
                    $('#Modal div.modal-dialog').html(null);
                }
            });
        } else {
            abp.notify.error("Chọn trước khi sửa!");
        }
    });

})(jQuery);
