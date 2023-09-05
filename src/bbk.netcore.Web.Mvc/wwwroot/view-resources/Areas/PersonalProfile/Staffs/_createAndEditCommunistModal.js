(function ($) {
    var _service = abp.services.app.communistPartyProcess,
        _$modal = $('#Modal');

    function update() {
        var _$form = _$modal.find('form');
        _$form.validateCommunist();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.personId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _service.update(data)
                .done(function (result) {
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("communist.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function create() {
        var _$form = _$modal.find('form');
        _$form.validateCommunist();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _service.create(data) 
                .done(function (result) {
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("communist.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '#Modal .create-communist #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '#Modal .edit-communist #SaveButton', function (e) {
        update();
    });

    $.fn.validateCommunist = function () {
        $(this).addClass("was-validated");
        
        validatorRelationShip = $(this).validate({
            invalidHandler: function (form, validator) {
                validator.errorList[0].element.focus();
            },
            errorClass: "invalid",
            validClass: "valid",
            rules: {
                "Year": {
                    required: true
                },
                "PartyMemberBackground": {
                    required: true
                },
                "EvaluatePartyMember": {
                    required: true
                }
            },
            messages: {
                "Year": {
                    required: "Không được để trống năm"
                },
                "PartyMemberBackground": {
                    required: "Không được để trống lý lịch Đảng viên"
                },
                "EvaluatePartyMember": {
                    required: "Không được để trống đánh giá, phân loại Đảng viên"
                }
            }
        });
    }

})(jQuery);
