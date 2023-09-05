(function ($) {
    var _commendationService = abp.services.app.commendation,
        _$modal = $('#Modal');

    function update() {
        var _$form = _$modal.find('form');
        validateCommendation(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _commendationService.update(data)
                .done(function () {
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("commendation.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function create() {
        var _$form = _$modal.find('form');
        validateCommendation(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _commendationService.create(data)
                .done(function () {
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("commendation.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '#Modal .create-commendation #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '#Modal .edit-commendation #SaveButton', function (e) {
        update();
    });

    function validateCommendation(form) {
        form.addClass("was-validated");
        form.validate({
            invalidHandler: function (form, validator) {
                validator.errorList[0].element.focus();
            },
            errorClass: "invalid",
            validClass: "valid",
            rules: {
                "CommendationYear": {
                    required: true
                },
                "DecisionNumber": {
                    required: true
                },
                "CommendationTitleId": {
                    required: true
                }
            },
            messages: {
                "CommendationYear": {
                    required: "Không được để trống năm khen thưởng"
                },
                "DecisionNumber": {
                    required: "Không được để trống số quyết định"
                },
                "CommendationTitleId": {
                    required: "Bắt buộc nhập chi tiết khen thưởng"
                }
            }
        });
    }
})(jQuery);
