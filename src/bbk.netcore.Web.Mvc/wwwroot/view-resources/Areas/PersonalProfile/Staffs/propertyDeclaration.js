var validatorRelationShip;
(function ($) {
    var _propertyDeclarationService = abp.services.app.propertyDeclaration;
    var _$table = $('#PropertyDeclarationTable');
    var _$modal = $('#Modal');

    var _$propertyDeclarationTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _propertyDeclarationService.getAll({ personId })
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
                data: 'title',
                sortable: false,
                render: function (data, type, row) {
                    if (row.fileUrl) {
                        return `${data} <a href="` + row.fileUrl + `" target="_blank" >
                                <i class="fal fa-file mr-1 color-danger-700"></i>
                            </a>`;
                    } else {
                        return data;
                    }
                }
            }
        ]
    });

    abp.event.on('propertyDeclaration.updated', (data) => {
        _$propertyDeclarationTable.ajax.reload();
    });

    $('#PropertyDeclarationTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$propertyDeclarationTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $(document).on('click', '.property-declaration .refresh', function (e) {
        _$propertyDeclarationTable.ajax.reload();
    });
     
    $('.property-declaration .delete').click(function () {
        var id = _$propertyDeclarationTable.row('.selected').id();
        if (id) {
            var personId = $("#PersonId").val();
            abp.message.confirm(
                abp.utils.formatString("Thông tin kê khai tài sản sẽ bị xóa!"),
                "Bạn có chắc chắn muốn xóa",
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$table);
                        _propertyDeclarationService.deleteById({ id: id, personId })
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                abp.event.trigger("propertyDeclaration.updated");
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

    $(document).on('click', '.property-declaration .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditPropertyDeclarationModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                _$modal.modal('toggle');
                $('#Modal div.modal-dialog').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.property-declaration .edit', function (e) {
        var id = _$propertyDeclarationTable.row('.selected').id();
        if (id) {
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditPropertyDeclarationModal/${id}`,
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

    $(document).on('click', '#DeletePropertyDeclarationFile', function () {
        var id = _$propertyDeclarationTable.row('.selected').id();
        if (!id) {
            return;
        }
        abp.message.confirm(
            abp.utils.formatString("Thông tin về file kê khai tài sản sẽ bị xóa!"), "Bạn có chắc chắn?",
            (isConfirmed) => {
                if (isConfirmed) {
                    _propertyDeclarationService.deleteFile(id)
                        .done(function () {
                            _$propertyDeclarationTable.ajax.reload();
                            abp.event.trigger("propertydeclaration.resetLabel");
                            abp.notify.info('Xóa file kê khai tài sản thành công!');
                            $("#DeletePropertyDeclarationFile").hide();
                        });
                }
            }
        );
    });

})(jQuery);
