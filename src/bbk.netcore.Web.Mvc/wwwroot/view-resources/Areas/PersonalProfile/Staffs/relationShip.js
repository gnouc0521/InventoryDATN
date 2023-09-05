(function ($) {
    var _relationShipService = abp.services.app.relationShip;
    var _$table = $('#RelationShipTable');
    var _$otherTable = $('#OtherRelationShipTable');
    var _$modal = $('#Modal');

    var _$relationShipTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _relationShipService.getAll({ personId, type:"self" })
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
                data: 'relationName',
                sortable: false
            },
            {
                targets: 1,
                data: 'fullName',
                sortable: false
            },
            {
                targets: 2,
                data: 'yearBirth',
                sortable: false
            },
            {
                targets: 3,
                data: 'info',
                sortable: false
            }
        ]
    });

    var _$otherRelationShipTable = _$otherTable.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _relationShipService.getAll({ personId, type: "other" })
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
                data: 'relationName',
                sortable: false
            },
            {
                targets: 1,
                data: 'fullName',
                sortable: false
            },
            {
                targets: 2,
                data: 'yearBirth',
                sortable: false
            },
            {
                targets: 3,
                data: 'info',
                sortable: false
            }
        ]
    });

    abp.event.on('relationShip.updated', (data) => {
        _$relationShipTable.ajax.reload();
        _$otherRelationShipTable.ajax.reload();
    });

    abp.event.on('relationShip.created', (data) => {
        _$relationShipTable.ajax.reload();
        _$otherRelationShipTable.ajax.reload();
    }); 

    $('#SummaryTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$relationShipTable.$('tr.selected').removeClass('selected');
            _$otherRelationShipTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    function changeRequired() {
        var _$type = $("#Type");
        var _$labelYearBirth = $('label[for="YearBirth"]');
        var _$labelInfo = $('label[for="Info"]');

        if (_$type.val() == "Self") {
            $("#another-option").hide();
            $('#YearBirth').attr('required', true);
            $('#Info').attr('required', true);
            _$labelYearBirth.find('span').show();
            _$labelInfo.find('span').show();
        } else {
            if ($('#YearBirth option[value=""]').length == 0) {
                $('#YearBirth').prepend($('<option id="another-option">').val("").text("Chưa rõ"));
            }
            $('#YearBirth').removeAttr('required');
            $('#Info').removeAttr('required');
            _$labelYearBirth.find('span').hide();
            _$labelInfo.find('span').hide();
        }
    }

    $(document).on('click', '.relation-ship .refresh', function (e) {
        _$relationShipTable.ajax.reload();
        _$otherRelationShipTable.ajax.reload();
    });

    $('.relation-ship .delete').click(function () {
        var relationShipId = _$relationShipTable.row('.selected').id() || _$otherRelationShipTable.row('.selected').id();
        if (relationShipId) {
            var personId = $("#PersonId").val();
            abp.message.confirm(
                abp.utils.formatString("Mối quan hệ sẽ bị xóa!"),
                "Bạn có chắc chắn muốn xóa",
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$table);
                        _relationShipService.deleteById({ id: relationShipId, personId })
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                abp.event.trigger("relationShip.updated");
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

    $(document).on('click', '.relation-ship .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditRelationShipModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                _$modal.modal('toggle');
                $('#Modal div.modal-dialog').html(content);
                changeRequired();
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.relation-ship .edit', function (e) {
        var id = _$relationShipTable.row('.selected').id() || _$otherRelationShipTable.row('.selected').id();

        if (id) {
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditRelationShipModal?id=${id}`,
                type: 'Get',
                dataType: 'html',
                success: function (content) {
                    $('#Modal div.modal-dialog').html(content);
                    $('#Modal').modal('toggle');
                    changeRequired();
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
