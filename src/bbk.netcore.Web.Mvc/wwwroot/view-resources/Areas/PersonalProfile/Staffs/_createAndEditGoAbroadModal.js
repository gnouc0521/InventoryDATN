(function ($) {
    var _goAbroadService = abp.services.app.goAbroad,
        _$modal = $('#Modal');

    function update() {
        var _$form = _$modal.find('form');
        _$form.validateGoAbroad();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.personId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _goAbroadService.update(data)
                .done(function () {
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("goAbroad.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function create() {
        var _$form = _$modal.find('form');
        _$form.validateGoAbroad();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _goAbroadService.create(data)
                .done(function () {
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("goAbroad.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '#Modal .create-goAbroad #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '#Modal .edit-goAbroad #SaveButton', function (e) {
        update();
    });

    $.fn.validateGoAbroad = function () {
        this.addClass("was-validated");
        this.validate({
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
                "FromDate": {
                    required: true
                },
                "ToDate": {
                    required: true
                },
                "Location": {
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
                "FromDate": {
                    required: "Không được để trống thời điểm bắt đầu"
                },
                "ToDate": {
                    required: "Không được để trống thời điểm kết thúc"
                },

                "Location": {
                    required: "Không được để trống địa điểm"
                }
            }
        });
    }
})(jQuery);
