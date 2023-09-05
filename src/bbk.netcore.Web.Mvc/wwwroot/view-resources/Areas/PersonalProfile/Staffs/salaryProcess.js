(function ($) {
    moment.locale(abp.localization.currentLanguage.name);
    var _salaryProcessService = abp.services.app.salaryProcess;
    var _$table = $('#SalaryProcessTable');
    var _civilServantService = abp.services.app.civilServant;
    var _$salaryProcessTable = _$table.DataTable({
        dom: 't',
        order: [],
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _salaryProcessService.getAll(personId)
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$table);
                });
        },      
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
                sortable: false,
                render: (data, type, row) => {
                    return `${moment(row.issuedTime).format('L')}`
                }
            },
            {
                targets: 2,
                sortable: false,
                render: (data, type, row) => {
                    return `${moment(row.salaryIncreaseTime).format('L')}`
                }
            },
            {
                targets: 3,
                data: 'gloneCode',
                sortable: false
            },
            {
                targets: 4,
                data: 'glone',
                sortable: false
            },
            {
                targets: 5,
                data: 'payRate',
                sortable: false
            },
            {
                targets: 6,
                data: 'coefficientsSalary',
                sortable: false
            },
            {
                targets: 7,
                data: 'leadershipPositionAllowance',
                sortable: false
            },
            {
                targets: 8,
                data: 'toxicAllowance',
                sortable: false
            },
            {
                targets: 9,
                data: 'areaAllowance',
                sortable: false
            },
            {
                targets: 10,
                data: 'responsibilityAllowance',
                sortable: false
            },
            {
                targets: 11,
                data: 'mobileAllowance',
                sortable: false
            },
        ]
    });
    function Glone() {
     _$selectGlone = $('#Glone');
        $(document).ready(function () {
            _civilServantService.getAllCivilServant().done(function (result) {           
             _$selectGlone.empty();
             for (let i = 0; i < result.length; i++) {
                 _$selectGlone.append(`<option data-id="${result[i].id}" value="${result[i].name}">${result[i].name}</option>`);
                }
                if ($("#GloneDiv").text()) {
                    _$selectGlone.val($("#GloneDiv").text());
                    $("#GloneDiv").text("");
                }
                onLoadCivilServant();
            });
            
      });      
    }
    function onLoadCivilServant() {
        let value = _$selectGlone.val();
        var selectedOption = $('#Glone option[value="'+value+'"]');
        _civilServantService.getCivilServant(selectedOption.attr('data-id')).done(function (result) {
            $('#GloneCode').val(result.code);              
            _civilServantService.getAllSalaryLevelByGroup(result.group).done(function (rs2) {
                $('#PayRates').empty();
                for (let i = 0; i < rs2.length; i++) {
                    $('#PayRates').append(`<option data-id=${rs2[i].id} value="${rs2[i].level}">${rs2[i].level}</option>`);
                }
                if ($("#PayRateDiv").text()) {
                    $('#PayRates').val($("#PayRateDiv").text());
                    $("#PayRateDiv").text("");
                }
                onLoadSalary();
            });
        });
        
    }

    function onLoadSalary() {
        let value = $("#PayRates").val();
        var selectedOption = $('#PayRates option[value="' + value + '"]');
        _civilServantService.getSalaryLevel(selectedOption.attr('data-id')).done(function (result) {
            $('#Coefficientssalaries').val(result.coefficientsSalary);
        });
    }
     $(document).on('change', '#Glone', function (e) {
        onLoadCivilServant();
    });

    $(document).on('change', '#PayRates', function (e) {
        onLoadSalary();
    });
    
    $('#SalaryProcessTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$salaryProcessTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    abp.event.on('salaryProcess.updated', (data) => {
        _$salaryProcessTable.ajax.reload();
    });

    $(document).on('click', '.create-salaryProcess', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateSalaryProcessModal',
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
                Glone();
            },
            error: function (e) { }
        });
    });
    $('.delete-salaryProcess').click(function () {
        var salaryProcessId = _$salaryProcessTable.row('.selected').id();
        if (!salaryProcessId) {
            abp.notify.error("Chọn trước khi muốn xóa!");
            return;
        }
        abp.message.confirm(
            abp.utils.formatString("Bạn có chắc chắn muốn xóa!"),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    abp.ui.setBusy(_$salaryProcessTable);
                    _salaryProcessService.deleteById(salaryProcessId)
                        .done(function () {
                            abp.notify.info('Xóa thành công!');
                            _$salaryProcessTable.ajax.reload();
                        }).always(function () {
                            abp.ui.clearBusy(_$salaryProcessTable);
                        });
                }
            }
        );
    });
    
    $(document).on('click', '.edit-salaryProcess', function (e) {
        var id = _$salaryProcessTable.row('.selected').id();
        if (!id) {
            abp.notify.error("Chọn trước khi muốn sửa!");
            return;
        }
        abp.ajax({
            url: abp.appPath + `PersonalProfile/Staffs/EditSalaryProcessModal?id=${id}`,
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
                Glone();
                
            },
            error: function (e) {
                $('#Modal div.modal-dialog').html(null);
                abp.message.error('Vui lòng thử lại!', 'Có lỗi');
            }
        });
    });
    $(document).on('click', '.refresh-salaryProcess', function (e) {
        abp.event.trigger("salaryProcess.updated");
    });
})(jQuery);
