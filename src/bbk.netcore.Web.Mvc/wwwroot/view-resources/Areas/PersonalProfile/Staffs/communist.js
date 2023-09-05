var validatorRelationShip;
(function ($) {
    var _service = abp.services.app.communistPartyProcess;
    var _$table = $('#CommunistTable');
    var _$modal = $('#Modal');

    var _$communistTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _service.getAll({ personId })
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
                data: 'year',
                sortable: false
            },
            {
                targets: 1,
                data: 'partyMemberBackground',
                sortable: false,
            },
            {
                targets: 2,
                data: 'evaluatePartyMember',
                sortable: false,
            }
        ]
    });

    abp.event.on('communist.updated', (data) => {
        _$communistTable.ajax.reload();
    });

    $('#CommunistTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$communistTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $(document).on('click', '.communist .refresh', function (e) {
        _$communistTable.ajax.reload();
    });
     
    $('.communist .delete').click(function () {
        var id = _$communistTable.row('.selected').id();
        if (id) {
            var personId = $("#PersonId").val();
            abp.message.confirm(
                abp.utils.formatString("Thông tin công tác Đảng sẽ bị xóa!"),
                "Bạn có chắc chắn muốn xóa",
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$table);
                        _service.deleteById({ id: id, personId })
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                abp.event.trigger("communist.updated");
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

    $(document).on('click', '.communist .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditCommunistnModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                _$modal.modal('toggle');
                $('#Modal div.modal-dialog').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.communist .edit', function (e) {
        var id = _$communistTable.row('.selected').id();
        if (id) {
            console.log(id);
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditCommunistnModal/${id}`,
                type: 'Get',
                dataType: 'html',
                success: function (content) {
                    $('#Modal div.modal-dialog').html(content);
                    $('#Modal').modal('toggle');
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
