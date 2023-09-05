(function ($) {
    var validatorRelationShip;
    var _relationShipService = abp.services.app.relationShip,
        _$modal = $('#Modal');

    $('#Modal').on('hide.bs.modal', () => {
        validatorRelationShip = null;
    });

    $(document).on('change', '#Type', function () {
        var _$form = _$modal.find('form');

        var _$labelYearBirth = $('label[for="YearBirth"]');
        var _$labelInfo = $('label[for="Info"]');

        if ($(this).val() == "Self") {
            if (validatorRelationShip) {
                $("#Info").rules("add", {
                    required: true,
                    messages: {
                        required: "Bắt buộc nhập thông tin quan hệ",
                    }
                });
                $("#YearBirth").rules("add", {
                    required: true,
                    messages: {
                        required: "Bắt buộc nhập năm sinh",
                    }
                });
                $(this).blur();
                _$form.valid();
            }

            $('#YearBirth').attr('required', true);
            $('#Info').attr('required', true);
            _$labelYearBirth.find('span').show();
            _$labelInfo.find('span').show();
        } else {
            if (validatorRelationShip) {
                $("#Info").rules("remove"); 
                $("#YearBirth").rules("remove");
                $(this).blur();
                _$form.valid();
                $('#Info-error').hide();
            }
            if ($('#YearBirth option[value=""]').length == 0) {
                $('#YearBirth').prepend($('<option id="another-option">').val("").text("Chưa rõ"));
            }
            $('#YearBirth').removeAttr('required');
            $('#Info').removeAttr('required');
            _$labelYearBirth.find('span').hide();
            _$labelInfo.find('span').hide();
        }
    });

    function update() {
        var _$form = _$modal.find('form');
        _$form.validateRelationShip();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.personId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _relationShipService.update(data)
                .done(function () {
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("relationShip.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function create() {
        var _$form = _$modal.find('form');
        _$form.validateRelationShip();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _relationShipService.create(data)
                .done(function () {
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("relationShip.created");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '#Modal .create-relationShip #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '#Modal .edit-relationShip #SaveButton', function (e) {
        update();
    });

    $.fn.validateRelationShip = function () {
        $(this).addClass("was-validated");
        if ($("#Type").val() == "Self") {
            validatorRelationShip = validatorRelationShip = $(this).validate({
                errorClass: "invalid",
                validClass: "valid",
                rules: {
                    "RelationName": {
                        required: true,
                        maxlength: 50
                    },
                    "FullName": {
                        required: true,
                        maxlength: 50
                    },
                    "Info": {
                        required: true
                    },
                    "YearBirth": {
                        required: true
                    },
                    "Type": {
                        required: true
                    }
                },
                messages: {
                    "RelationName": {
                        required: "Bắt buộc nhập tên mối qua hệ",
                        maxlength: "Hãy nhập tối đa 50 ký tự"
                    },
                    "FullName": {
                        required: "Bắt buộc nhập họ tên",
                        maxlength: "Hãy nhập tối đa 50 ký tự"
                    },
                    "Info": {
                        required: "Bắt buộc nhập thông tin quan hệ"
                    },
                    "Type": {
                        required: "Bắt buộc nhập loại quan hệ"
                    },
                    "YearBirth": {
                        required: "Bắt buộc nhập năm sinh"
                    }
                },
                invalidHandler: function (form, validator) {
                    validator.errorList[0].element.focus();
                },
            });
        }
        else
        {
            validatorRelationShip = $(this).validate({
                errorClass: "invalid",
                validClass: "valid",
                rules: {
                    "RelationName": {
                        required: true,
                        maxlength: 50
                    },
                    "FullName": {
                        required: true,
                        maxlength: 50
                    },
                    "Type": {
                        required: true
                    }
                },
                messages: {
                    "RelationName": {
                        required: "Bắt buộc nhập tên mối qua hệ",
                        maxlength: "Hãy nhập tối đa 50 ký tự"
                    },
                    "FullName": {
                        required: "Bắt buộc nhập họ tên",
                        maxlength: "Hãy nhập tối đa 50 ký tự"
                    },
                    "Type": {
                        required: "Bắt buộc nhập loại quan hệ"
                    }
                },
                invalidHandler: function (form, validator) {
                    validator.errorList[0].element.focus();
                },
            });
        }
    }

})(jQuery);
