(function ($) {
    moment.locale(abp.localization.currentLanguage.name);
    var _goAbroadService = abp.services.app.goAbroad;
    var _$table = $('#GoAbroadTable');
    var _$modal = $('#Modal');
    var _$goAbroadTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _goAbroadService.getAll({ personId })
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
                render: (data, type, row) => (`${row.fromDate}-${row.toDate}`),
                sortable: false
            },
            {
                targets: 3,
                data: 'location',
                sortable: false
            },
            {
                targets: 4,
                data: 'summary',
                sortable: false
            }
        ]
    });

    abp.event.on('goAbroad.updated', (data) => {
        _$goAbroadTable.ajax.reload();
    });

    $('#GoAbroadTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$goAbroadTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $(document).on('click', '.go-abroad .refresh', function (e) {
        abp.event.trigger('goAbroad.updated');
    });

    $('.go-abroad .delete').click(function () {
        var id = _$goAbroadTable.row('.selected').id();
        if (id) {
            var personId = $("#PersonId").val();
            abp.message.confirm(
                abp.utils.formatString("Thông tin đi nước ngoài sẽ bị xóa!"),
                "Bạn có chắc chắn muốn xóa",
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$table);
                        _goAbroadService.deleteById({ id: id, personId })
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                abp.event.trigger("goAbroad.updated");
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

    $(document).on('click', '.go-abroad .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditGoAbroadModal',
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

    $(document).on('click', '.go-abroad .edit', function (e) {
        var id = _$goAbroadTable.row('.selected').id();
        if (id) {
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditGoAbroadModal/${id}`,
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
