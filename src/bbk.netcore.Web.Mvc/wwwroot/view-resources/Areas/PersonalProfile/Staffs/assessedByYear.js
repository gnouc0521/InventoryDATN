(function ($) {
    var _assessedByYearService = abp.services.app.assessedByYear;

    var _$summaryTable = $('#SummaryAssessTable');
    var _$civilTable = $('#CivilAssessedByYearTable');
    var _$publicTable = $('#PublicAssessedByYearTable');
    var _$laborTable = $('#LaborAssessedByYearTable');
    var _$modal = $('#Modal');

    var _$civilAssessTable = _$civilTable.DataTable({
        dom: 't',
        order: [],
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$summaryTable);
            _assessedByYearService.getAll({ personId: personId, type : "civilServant" })
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$summaryTable);
                });
        },
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
                data: 'selfAssessment',
                sortable: false
            },
            {
                targets: 2,
                data: 'assessmentByLeader',
                sortable: false
            },
            {
                targets: 3,
                data: 'collectiveFeedback',
                sortable: false
            },
            {
                targets: 4,
                data: 'evaluationOfAuthorizedPerson',
                sortable: false
            },
            {
                targets: 5,
                data: 'resultsOfClassification',
                sortable: false
            }

        ]
    }); 

    var _$publicAssessTable = _$publicTable.DataTable({
        dom: 't',
        order: [],
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$summaryTable);
            _assessedByYearService.getAll({ personId: personId, type: "publicServant" })
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$summaryTable);
                });
        },
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
                data: 'selfAssessment',
                sortable: false
            },
            {
                targets: 2,
                data: 'assessmentByLeader',
                sortable: false
            },
            {
                targets: 3,
                data: 'collectiveFeedback',
                sortable: false
            },
            {
                targets: 4,
                data: 'evaluationOfAuthorizedPerson',
                sortable: false
            },
            {
                targets: 5,
                data: 'resultsOfClassification',
                sortable: false
            }

        ]
    }); 

    var _$laborAssessTable = _$laborTable.DataTable({
        dom: 't',
        order: [],
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$summaryTable);
            _assessedByYearService.getAll({ personId: personId, type: "laborContract" })
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$summaryTable);
                });
        },
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
                data: 'selfAssessment',
                sortable: false
            },
            {
                targets: 2,
                data: 'assessmentByLeader',
                sortable: false
            },
            {
                targets: 3,
                data: 'collectiveFeedback',
                sortable: false
            },
            {
                targets: 4,
                data: 'evaluationOfAuthorizedPerson',
                sortable: false
            },
            {
                targets: 5,
                data: 'resultsOfClassification',
                sortable: false
            }

        ]
    }); 

    $('#SummaryAssessTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$civilAssessTable.$('tr.selected').removeClass('selected');
            _$publicAssessTable.$('tr.selected').removeClass('selected');
            _$laborAssessTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    abp.event.on('assessedByYear.updated', () => {
        _$civilAssessTable.ajax.reload();
        _$publicAssessTable.ajax.reload();
        _$laborAssessTable.ajax.reload();
    });

    $('.delete-assessedByYear').click(function () {
        var assessedByYearId = _$civilAssessTable.row('.selected').id() || _$publicAssessTable.row('.selected').id() || _$laborAssessTable.row('.selected').id();
        if (!assessedByYearId) {
            abp.notify.error("Chọn trước khi muốn xóa!");
            return;
        }
        abp.message.confirm(
            abp.utils.formatString("Bạn có chắc chắn muốn xóa!"),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    abp.ui.setBusy(_$summaryTable);
                    _assessedByYearService.deleteById(assessedByYearId)
                        .done(function () {
                            abp.notify.info('Xóa thành công!');
                            abp.event.trigger("assessedByYear.updated");
                        }).always(function () {
                            abp.ui.clearBusy(_$summaryTable);
                        });
                }
            }
        );
    });

    $(document).on('click', '.create-assessedByYear', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateAssessedByYearModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                $('#Modal div.modal-dialog').html(content);
                $('#Modal').modal('show');
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.edit-assessedByYear', function (e) {
        var assessedByYearId = _$civilAssessTable.row('.selected').id() || _$publicAssessTable.row('.selected').id() || _$laborAssessTable.row('.selected').id();
        if (!assessedByYearId) {
            abp.notify.error("Chọn trước khi muốn sửa!");
            return;
        }
        abp.ajax({
            url: abp.appPath + `PersonalProfile/Staffs/EditAssessedByYearModal?id=${assessedByYearId}`,
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                $('#Modal div.modal-dialog').html(content);
                $('#Modal').modal('show');
            },
            error: function (e) {
                $('#Modal div.modal-dialog').html(null);
                abp.message.error('Vui lòng thử lại!', 'Có lỗi');
            }
        });
    });

    $(document).on('click', '.refresh-assessedByYear', function (e) {
        abp.event.trigger("assessedByYear.updated");
    });

})(jQuery);
