(function ($) {
    var _staffPlainningService = abp.services.app.staffPlainning,
        _$modal = $('#Modal');

    function update() {
        var _$form = _$modal.find('form');
        validateStaffplainning(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.personId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _staffPlainningService.update(data)
                .done(function () {
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("staffPlainning.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function create() {
        var _$form = _$modal.find('form');
        validateStaffplainning(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _staffPlainningService.create(data)
                .done(function () {
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("staffPlainning.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '#Modal .create-staffPlainning #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '#Modal .edit-staffPlainning #SaveButton', function (e) {
        update();
    });

    function validateStaffplainning(form) {
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
                "IssuedDate": {
                    required: true
                },
                "IssuedLevels": {
                    required: true,
                    maxlength: 100
                },
                "WorkingTitle": {
                    required: true,
                    maxlength: 100
                },
                "FromDate": {
                    required: true
                },
                "ToDate": {
                    required: true
                }
            },
            messages: {
                "DecisionNumber": {
                    required: "Không được để trống số quyết định"
                },
                "IssuedDate": {
                    required: "Không được để trống ngày quyết định"
                },
                "IssuedLevels": {
                    required: "Không được để trống cấp ban hành",
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "WorkingTitle": {
                    required: "Không được để trống chức danh quy hoạch",
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "FromDate": {
                    required: "Không được để trống thời điểm bắt đầu"
                },
                "ToDate": {
                    required: "Không được để trống thời điểm kết thúc"
                }
            }
        });
    }
})(jQuery);
