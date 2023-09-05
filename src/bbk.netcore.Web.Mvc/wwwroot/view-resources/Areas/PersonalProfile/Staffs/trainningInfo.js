(function ($) {
    moment.locale(abp.localization.currentLanguage.name);
    var _trainningInfoService = abp.services.app.trainningInfo;
    var _$table = $('#TrainningInfoTable');

    var _$trainningInfoTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _trainningInfoService.getAll({ personId })
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
                data: 'schoolName',
                sortable: false
            },
            {
                targets: 1,
                data: 'majoringName',
                sortable: false
            },
            {
                targets: 2,
                render: (data, type, row) => {
                    return `${moment(row.fromDate).format('L')}`
                },
                sortable: false
            },
            {
                targets: 3,
                render: (data, type, row) => {
                    return `${moment(row.toDate).format('L')}`
                },
                sortable: false
            },
            {
                targets: 4,
                data: 'trainningType',
                sortable: false
            },
            {
                targets: 5,
                data: 'diploma',
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

    abp.event.on('trainningInfo.updated', (data) => {
        _$trainningInfoTable.ajax.reload();
    });

    $('#TrainningInfoTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$trainningInfoTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $(document).on('click', '.trainning-info .refresh', function (e) {
        _$trainningInfoTable.ajax.reload();
    });

    $('.trainning-info .delete').click(function () {
        var trainningInfoId = _$trainningInfoTable.row('.selected').id();
        var personId = $("#PersonId").val();
        if (!trainningInfoId) {
            abp.notify.error("Chọn trước khi muốn xóa!");
            return;
        }
        abp.message.confirm(
            abp.utils.formatString("Thông tin về quá trình đào tạo sẽ bị xóa!"),"Bạn có chắc chắn?",
            (isConfirmed) => {
                if (isConfirmed) {
                    abp.ui.setBusy(_$table);
                    _trainningInfoService.deleteById({ id: trainningInfoId, personId })
                        .done(function () {
                            abp.notify.info('Xóa thành công!');
                            _$trainningInfoTable.ajax.reload();
                        }).always(function () {
                            abp.ui.clearBusy(_$table);
                        });
                }
            }
        );
    });

    $(document).on('click', '#DeleteDiplomaFile', function () {
        var trainningInfoId = _$trainningInfoTable.row('.selected').id();
        if (!trainningInfoId) {
            return;
        }
        abp.message.confirm(
            abp.utils.formatString("Thông tin về file văn bằng, chứng chỉ sẽ bị xóa!"), "Bạn có chắc chắn?",
            (isConfirmed) => {
                if (isConfirmed) {
                    _trainningInfoService.deleteDiplomaFile(trainningInfoId)
                        .done(function () {
                            _$trainningInfoTable.ajax.reload();
                            abp.event.trigger("trainningInfo.resetLabel");
                            abp.notify.info('Xóa file văn bằng thành công!');
                            $("#DeleteDiplomaFile").hide();
                        });
                }
            }
        );
    });

    $(document).on('click', '.trainning-info .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditTrainningInfoModal',
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
                    $("#Modal .date-picker").datepicker().on('show.bs.modal', function (event) {
                        event.stopPropagation();
                    });
                    $("#Modal .date-picker").datepicker().on('hide.bs.modal', function (event) {
                        event.stopPropagation();
                    });
                }
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.trainning-info .edit', function (e) {
        var id = _$trainningInfoTable.row('.selected').id();
        if (!id) {
            abp.notify.error("Chọn trước khi muốn sửa!");
            return;
        }
        abp.ajax({
            url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditTrainningInfoModal/${id}`,
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
                    $("#Modal .date-picker").datepicker().on('show.bs.modal', function (event) {
                        event.stopPropagation();
                    });
                    $("#Modal .date-picker").datepicker().on('hide.bs.modal', function (event) {
                        event.stopPropagation();
                    });
                }
            },
            error: function (e) {
                $('#Modal div.modal-dialog').html(null);
                abp.message.error('Vui lòng thử lại!', 'Có lỗi');
            }
        });
    });

})(jQuery);
