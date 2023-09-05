(function ($) {
    var _workingProcessService = abp.services.app.workingProcess,
        _$modal = $('#Modal');

    function update() {
        var _$form = _$modal.find('form');
        validateWorkingProcess(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _workingProcessService.update(data)
                .done(function () {
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("workingProcess.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function create() {
        var _$form = _$modal.find('form');
        validateWorkingProcess(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _workingProcessService.create(data)
                .done(function () {
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("workingProcess.created");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '#Modal .create-workingProcess #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '#Modal .edit-workingProcess #SaveButton', function (e) {
        update();
    });

    function validateWorkingProcess(form) {
        form.addClass("was-validated");
        form.validate({
            invalidHandler: function (form, validator) {
                validator.errorList[0].element.focus();
            },
            errorClass: "invalid",
            validClass: "valid",
            rules: {
                "TypeOfChange": {
                    required: true,
                    maxlength: 100
                },
                "JobPosition": {
                    maxlength: 256
                },
                "OtherOrg": {
                    required: true,
                    maxlength: 256
                },
                "DepartmentName": {
                    maxlength: 100
                },
                "IssuedDate": {
                    date: false,
                },
                "FromDate": {
                    required: true,
                    date: false,
                },
                "ToDate": {
                    date: false,
                },
                "WorkingTitleId": {
                    required: true
                }
            },
            messages: {
                "OtherOrg": {
                    required: "Bắt buộc nhập đơn vị",
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "DepartmentName": {
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "TypeOfChange": {
                    required: "Bắt buộc nhập hình thức",
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "JobPosition": {
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "WorkingTitleId": {
                    required: "Bắt buộc nhập vị trí"
                },
                "FromDate": {
                    required: "Bắt buộc nhập ngày bắt đầu"
                }
            }
        });
    }
})(jQuery);
