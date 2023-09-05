(function ($) {
    let resultObject = {};
    var groupBy = function (xs, key) {
        return xs.reduce(function (rv, x) {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    };
    var _commendationService = abp.services.app.commendation;
    var _categoryService = abp.services.app.category;
    var _$summaryCommendationTable = $('#SummaryCommendationTable');
    var _$emulationTable = $('#EmulationTitleTable');
    var _$commendationTable = $('#CommendationFormTable');
    var _$modal = $('#Modal');
    var _$emulationTitleTable = _$emulationTable.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$summaryCommendationTable);
            _commendationService.getAll({ personId, type:"emulationTitle" })
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$summaryCommendationTable);
                });
        },
        order: [],
        rowId: "id",
        columnDefs: [
            {
                cellType: 'th',
                targets: 0,
                data: 'commendationYear',
                sortable: false
            },
            {
                targets: 1,
                data: 'commendationTitle',
                sortable: false
            },
            {
                targets: 2,
                data: 'decisionNumber',
                sortable: false
            }
        ]
    });

    var _$commendationFormTable = _$commendationTable.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$summaryCommendationTable);
            _commendationService.getAll({ personId, type: "commendationForm" })
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$summaryCommendationTable);
                });
        },
        order: [],
        rowId: "id",
        columnDefs: [
            {
                cellType: 'th',
                targets: 0,
                data: 'commendationYear',
                sortable: false
            },
            {
                targets: 1,
                data: 'commendationTitle',
                sortable: false
            },
            {
                targets: 2,
                data: 'decisionNumber',
                sortable: false
            }
        ]
    });

    abp.event.on('commendation.updated', (data) => {
        _$emulationTitleTable.ajax.reload();
        _$commendationFormTable.ajax.reload();
    }); 

    $('#SummaryCommendationTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$emulationTitleTable.$('tr.selected').removeClass('selected');
            _$commendationFormTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    function genCommendatioOption(id = null) {
        _categoryService.getListByMultiType(["emulationTitle", "commendationForm"])
            .done(result => {
                resultObject = groupBy(result, "categoryType");

                for (var i in resultObject) {
                    optionText = resultObject[i][0].categoryName;
                    optionValue = resultObject[i][0].categoryType;

                    $('#CommendationType').append($('<option>').val(optionValue).text(optionText));
                }

                // gán giá trị chi tiết cho loại chức danh
                if (id) {

                    let detail = result.find(element => element.id == id);

                    let categoryType = detail.categoryType;

                    $("#CommendationType").val(categoryType);
                    let commendationValues = resultObject[categoryType];

                    for (var i of commendationValues) {
                        optionText = i.title;
                        optionValue = i.id;

                        $('#CommendationTitleId').append($('<option>').val(optionValue).text(optionText));
                    }
                    $('#CommendationTitleId').val(id);
                }
                else {
                    let categoryType = $("#CommendationType").val();

                    let commendationValues = resultObject[categoryType];
                    for (var i of commendationValues) {
                        optionText = i.title;
                        optionValue = i.id;

                        $('#CommendationTitleId').append($('<option>').val(optionValue).text(optionText));
                    }
                }
            });
    }

    $('#Modal').on('change', "#CommendationType", function (e) {
        $('#CommendationTitleId').empty();
        let categoryType = $("#CommendationType").val();
        let commendationValues = resultObject[categoryType];
        for (var i of commendationValues) {
            optionText = i.title;
            optionValue = i.id;
            $('#CommendationTitleId').append($('<option>').val(optionValue).text(optionText));
        }
    });

    $(document).on('click', '.commendation .refresh', function (e) {
        _$emulationTitleTable.ajax.reload();
        _$commendationFormTable.ajax.reload();
    });

    $('.commendation .delete').click(function () {
        var commendationId = _$emulationTitleTable.row('.selected').id() || _$commendationFormTable.row('.selected').id();
        if (commendationId) {
            var personId = $("#PersonId").val();
            abp.message.confirm(
                abp.utils.formatString("Thông tin khen thưởng sẽ bị xóa!"),
                "Bạn có chắc chắn muốn xóa",
                (isConfirmed) => {
                    if (isConfirmed) {
                        _commendationService.deleteById({ id: commendationId, personId })
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                abp.event.trigger("commendation.updated");
                            }).always(function () {
                                abp.ui.clearBusy(_$summaryCommendationTable);
                            });
                    }
                }
            );
        } else {
            abp.notify.error("Chọn trước khi xóa!");
        }
    });

    $(document).on('click', '.commendation .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditCommendationModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                _$modal.modal('toggle');
                $('#Modal div.modal-dialog').html(content);
                genCommendatioOption();
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.commendation .edit', function (e) {
        var id = _$emulationTitleTable.row('.selected').id() || _$commendationFormTable.row('.selected').id();

        if (id) {
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditCommendationModal/${id}`,
                type: 'Get',
                dataType: 'html',
                success: function (content) {
                    $('#Modal div.modal-dialog').html(content);
                    $('#Modal').modal('toggle');
                    let commendationTitleId = $('#divCommendationTitleId').text()
                    genCommendatioOption(commendationTitleId);
                },
                error: function (e) {
                    $('#Modal div.modal-dialog').html(null);
                }
            });
        } else {
            abp.notify.error("Chọn thông tin khen thưởng trước khi sửa!");
        }
    });

})(jQuery);
