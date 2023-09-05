(function ($) {
    var _salaryProcessService = abp.services.app.salaryProcess,
        _$modal = $('#Modal');
    function create() {
        var _$form = _$modal.find('form');
       
        validateSalaryProcess(_$form);
        if(!_$form.valid()) {
            return;
        }
        var data = _$form.serializeFormToObject();
        data.PersonId = parseInt($("#PersonId").val());   
        abp.ui.setBusy(_$form);
        _salaryProcessService.create(data)
            .done(function () {
                abp.notify.info('Thêm mới thành công!');
                _$modal.modal('hide');
                abp.event.trigger("salaryProcess.updated");
            }).always(function () {
                abp.ui.clearBusy(_$form);
            });
    }
    function validateSalaryProcess(form) {
        form.addClass("was-validated");
        form.validate({
            invalidHandler: function (form, validator) {
                validator.errorList[0].element.focus();
            },
            errorClass: "invalid",
            validClass: "valid",
            rules: {
                "DecisionNumber": {
                    required: true
                },
                "IssuedTime": {
                    required: true,
                    date: false
                },
                "SalaryIncreaseTime": {
                    required: true,
                    date: false
                }
              
            },
            messages: {
                "DecisionNumber": {
                    required: "Không được để trống số quyết định"
                },
                "IssuedTime": {
                    required: "Không được để trống ngày quyết định"
                },
                "SalaryIncreaseTime": {
                    required: "Không được để trống ngày nâng lương"
                }
               
            }
        });
    }
    $(document).on('click', '.create-salaryProcesses #SaveButton', function (e) {
        create();
    });
})(jQuery);
