(function ($) {
    moment.locale(abp.localization.currentLanguage.name);
    //let oldValue = "";
    //let resultObject;
    //var groupBy = function (xs, key) {
    //    return xs.reduce(function (rv, x) {
    //        (rv[x[key]] = rv[x[key]] || []).push(x);
    //        return rv;
    //    }, {});
    //};

    var _workingProcessService = abp.services.app.workingProcess;
    var _categoryService = abp.services.app.category;
    var _$table = $('#WorkingProcessTable');
    var _$modal = $('#Modal');

    var _$workingProcessTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _workingProcessService.getAll({ personId })
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
                    return data ? `${moment(row.issuedDate).format('L')}` : ""
                },
                sortable: false
            },
            {
                targets: 2,
                data: 'typeOfChange',
                sortable: false
            },
            {
                targets: 3,
                data: 'decisionMaker', 
                sortable: false
            },
            {
                targets: 4,
                render: (data, type, row) => {
                    return `${moment(row.fromDate).format('L')}`
                },
                sortable: false
            },
            {
                targets: 5,
                data: 'toDate',
                render: (data, type, row) => {
                    return data ? `${moment(row.toDate).format('L')}` : ""
                },
                sortable: false
            },
            {
                targets: 6,
                data: 'workingTitle',
                sortable: false
            },
            {
                targets: 7,
                data: 'jobPosition',
                sortable: false
            },
            {
                targets: 8,
                data: 'departmentName',
                sortable: false
            },
            {
                targets: 9,
                data: 'organName',
                sortable: false
            }
        ]
    });

    abp.event.on('workingProcess.updated', (data) => {
        _$workingProcessTable.ajax.reload();
    });

    abp.event.on('workingProcess.created', (data) => {
        _$workingProcessTable.ajax.reload();
    });

    $('#WorkingProcessTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$workingProcessTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });


    $(document).on('click', '.working-process .refresh', function (e) {
        _$workingProcessTable.ajax.reload();
    });

    $('.working-process .delete').click(function () {

        var workingProcessId = _$workingProcessTable.row('.selected').id();
        var personId = $("#PersonId").val();
        if (!workingProcessId) {
            abp.notify.error('Chọn trước khi muốn xóa!');
            return;
        };
        abp.message.confirm(
            abp.utils.formatString("Thông tin công tác sẽ bị xóa!"),
            "Bạn có chắc chắn muốn xóa",
            (isConfirmed) => {
                if (isConfirmed) {
                    abp.ui.setBusy(_$table);
                    _workingProcessService.deleteById({ id: workingProcessId, personId })
                        .done(function () {
                            abp.notify.info('Xóa thành công!');
                            _$workingProcessTable.ajax.reload();
                        }).always(function () {
                            abp.ui.clearBusy(_$table);
                        });
                }
            }
        );
    });

    //function genWorkingTitleOption(id = null) {
    //    _categoryService.getListByMultiType(["civilServant", "publicServant"])
    //        .done(result => {
    //            resultObject = groupBy(result, "categoryType");

    //            for (var i in resultObject) {
    //                optionText = resultObject[i][0].categoryName;
    //                optionValue = resultObject[i][0].categoryType;

    //                $('#WorkingTitleType').append($('<option>').val(optionValue).text(optionText));
    //            }

    //            // gán giá trị chi tiết cho loại chức danh
    //            if (id) {
                    
    //                let detail = result.find(element => element.id == id);

    //                let categoryType = detail.categoryType;

    //                $("#WorkingTitleType").val(categoryType);
    //                let workingTitleValues = resultObject[categoryType];

    //                for (var i of workingTitleValues) {
    //                    optionText = i.title;
    //                    optionValue = i.id;

    //                    $('#WorkingTitleId').append($('<option>').val(optionValue).text(optionText));
    //                }
    //                $('#WorkingTitleId').val(id);
    //            }
    //            else {
    //                let categoryType = $("#WorkingTitleType").val();

    //                let workingTitleValues = resultObject[categoryType];
    //                for (var i of workingTitleValues) {
    //                    optionText = i.title;
    //                    optionValue = i.id;

    //                    $('#WorkingTitleId').append($('<option>').val(optionValue).text(optionText));
    //                }
    //            }
    //        });
    //}
    //
    //$(document).on('change', '#WorkingTitleType', function (e) {
    //    $('#WorkingTitleId').empty();
    //    let categoryType = $("#WorkingTitleType").val();
    //    let workingTitleValues = resultObject[categoryType];
    //    for (var i of workingTitleValues) {
    //        optionText = i.title;
    //        optionValue = i.id;
    //        $('#WorkingTitleId').append($('<option>').val(optionValue).text(optionText));
    //    }
    //});

    $(document).on('change', '#OrgId', function (e) {
        $("#OtherOrg").val($("#OrgId").val() ? $("#OrgId option:selected").html() : "");
    });

    //$(document).on('change', '#TypeOfChangeId', function () {
    //    if ($("#TypeOfChangeId").val()=="select-another") {
    //        $("#TypeOfChangeId").hide();
    //        $("#TypeOfChangeText").show();
    //        $("#TypeOfChangeText").focus();
    //    } else {
    //        oldValue = $("#TypeOfChangeId").val();
    //    }
    //});

    //$(document).on('keyup', '#TypeOfChangeText', function (e) {
    //    if (e.keyCode == 13) {
    //        $("#TypeOfChangeText").focusout();
    //    }
    //});

    //$(document).on("focusout", "#TypeOfChangeText", function (e) {
    //    let value = $("#TypeOfChangeText").val();
    //    if (value) {
    //        let optionText = value;
    //        if ($("#another-option").text()) {
    //            $("#another-option").val("").text(value);
    //        } else {
    //            $('#TypeOfChangeId').prepend($('<option id="another-option">').val("").text(optionText));
    //        }
    //        $('#TypeOfChangeId').val("");
    //    } else {
    //        $('#TypeOfChangeId').val(oldValue);
    //    }
    //    $("#TypeOfChangeId").show();
    //    $("#TypeOfChangeText").hide();
    //    $('#TypeOfChangeId option').filter(function () {
    //        return (!this.value || $.trim(this.value).length == 0) && this.id != "select-another" && this.id != "another-option";
    //    }).remove();
    //});

    $('#Modal').on('hide.bs.modal', () => {
        //oldValue = "";
        resultObject = undefined;
    });
    
    $(document).on('click', '.working-process .add', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateOrEditWorkingProcessModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                _$modal.modal('toggle');
                $('#Modal div.modal-dialog').html(content);
                //$('#TypeOfChangeId').append($('<option id="select-another">').val("select-another").text("Khác ....."));
                //oldValue = $("#TypeOfChangeId").val()
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

    $(document).on('click', '.working-process .edit', function (e) {
        var id = _$workingProcessTable.row('.selected').id();
        if (id) {
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/CreateOrEditWorkingProcessModal?id=${id}`,
                type: 'Get',
                dataType: 'html',
                success: function (content) {
                    $('#Modal').modal('toggle');
                    $('#Modal div.modal-dialog').html(content);
                    //$('#TypeOfChangeId').append($('<option id="select-another">').val("select-another").text("Khác ....."));
                    //oldValue = $("#TypeOfChangeId").val()
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
                }
            });
        } else {
            abp.notify.error("Chọn trước khi sửa!");
        }
    });

})(jQuery);
